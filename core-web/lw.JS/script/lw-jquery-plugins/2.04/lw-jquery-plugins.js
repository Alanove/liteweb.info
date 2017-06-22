/// <reference path="jquery-vsdoc.js" />
/// <reference path="utils.js" />
/// <reference path="lw.js" />


$.fn.lwSlideShow = function (options) {


	//TODO: Scroll left parallax
	var opts = {
		folder: "images/slide-show",
		max: 5,
		speed: 1000,
		wait: 5000,
		topMargin: 0,
		leftMargin: 20,
		left: 0,
		top: 0,
		slogansContainer: "#slogans",
		parallax: true,
		parallaxDensity: 1.5,
		slogansParallaxDensity: 1.8,
		bulletsAsThumbs: true,
		extension: "jpg",
		thumbExtension: "jpg",
		showBullets: true,
		images: null,
		bulletsContainer: null,
		startPaused: false,
		cycle: null,
		parallaxCustomDensity: null

	};

	opts = $.extend(opts, options);

	this.options = opts;

	var currentLoadedImage = opts.startAt;
	var current = 1;

	var $el = this;
	var animateInterval = null;
	var images = opts.images || [];

	$el.children("img").each(function () {
		images.push($(this));
	});

	currentLoadedImage = images.length;

	if (!isOk(opts.images)) {
		for (var i = currentLoadedImage + 1; i <= opts.max; i++)
			images.push(opts.folder + "/" + i + "." + opts.extension);
	}

	opts.max = images.length;

	var animateFlag = true;

	var slogansContainer = $(opts.slogansContainer);
	if (slogansContainer.length > 0) {
		slogansContainer.data("top", slogansContainer.position().top);
		slogansContainer.data("left", slogansContainer.position().left);
	}

	var bulletsContainer;

	var thisSlideShow = this;

	if (opts.max > 1 && opts.showBullets) {

		var manual = opts.bulletsContainer ? $(opts.bulletsContainer) : $("<div class='bullets'/>");
		var clickable = $("<ul />");
		for (i = 1; i <= opts.max; i++) {
			var inner = i;
			if (opts.bulletsAsThumbs)
				inner = "<img src=\"{0}/{1}-thumb.{2}\" />".Format(opts.folder, i, opts.thumbExtension);
			var list = $("<li id='bullet-" + i + "'>" + inner + "</li>");

			clickable.append(list);
		}
		clickable.children().each(function () {
			$(this).click(function () {
				animateFlag = true;
				var item = parseInt($(this).attr("id").split("-")[1]);
				thisSlideShow._animate(null, null, true, item);
				clearTimeout(animateTimeout);
				animateTimeout = window.setTimeout(thisSlideShow._animate, opts.wait * 3);
			});
		});
		manual.append(clickable);

		if (!opts.bulletsContainer)
			$el.append(manual);

		bulletsContainer = manual;

		bulletsContainer.find("#bullet-1").addClass('selected');
	}


	function loadNextImage() {


		if (currentLoadedImage >= opts.max)
			return;

		var img = $("<img />");
		img.attr("src", images[currentLoadedImage]);

		$el.append(img);

		img.css({ position: "absolute", opacity: 0, "z-index": 1 });

		images[currentLoadedImage] = img;

		//loadNextImage();

		img.bind("load", loadNextImage);
		currentLoadedImage++;
	}
	function init() {
		var rel = $el.attr("rel");
		if (isOk(rel) && rel.trim() != "") {
			var temp = rel.split("|");
			opts.folder = temp[0];
			if (temp.length > 1)
				opts.max = parseInt(temp[1]);
		}
		loadNextImage();

		if (!opts.startPaused)
			setTimeout(thisSlideShow.play, opts.wait);
	}


	var scrollLock = false;
	var animateTimeout = null;

	if (opts.parallax === true) {
		function CheckScroll() {
			try {
				var scrollTop = $(this).scrollTop();
				var scrollLeft = $(this).scrollLeft();

				var pos = $el.offset();



				if (scrollTop > pos.top) {
					scrollLock = true;
					animateFlag = false;

					var img = images[current - 1];
					if (typeof (img) != "undefined") {

						img.stop().animate({ opacity: 1 });

						var imgDensity = opts.parallaxDensity;
						var sloganDensity = opts.slogansParallaxDensity;

						if (opts.parallaxCustomDensity) {
							if (opts.parallaxCustomDensity.images) {
								for (i in opts.parallaxCustomDensity.images) {
									if (img.hasClass(i)) {
										imgDensity = opts.parallaxCustomDensity.images[i];
										break;
									}
								}
							}
							if (opts.parallaxCustomDensity.slogans) {
								for (i in opts.parallaxCustomDensity.slogans) {
									if (img.hasClass(i)) {
										sloganDensity = opts.parallaxCustomDensity.slogans[i];
										break;
									}
								}
							}
						}
						img.css({ position: "absolute", top: (scrollTop - pos.top) / imgDensity });
						slogansContainer.css({ top: (scrollTop - pos.top) / sloganDensity });
					}
					if (animateTimeout)
						clearTimeout(animateTimeout);
				} else {
					if (scrollLock) {
						scrollLock = false;
						animateFlag = true;
						$el.find("img").css({ top: 0 });
						slogansContainer.css({ top: slogansContainer.data("top") });
						animateTimeout = window.setTimeout(animate, opts.wait);
					}
				}
			}
			catch (e) { }
		}

		$(window).scroll(CheckScroll);
	}



	function _Animate(a, b, stopAfter, next) {

		try {


			if (opts.max <= 1)
				animateFlag = false;

			if (animateFlag) {

				if (stopAfter)
					animateFlag = false;

				var oldIndex = current - 1;

				var old = images[oldIndex];

				current = isOk(next) ? next : current + 1;

				if (current > currentLoadedImage)
					current = 1;

				var next = images[current - 1];

				$($el).data("current-slide", current);
				$($el).trigger("change");

				//Hiding old picture
				for (var i = 0; i < images.length; i++) {
					var $img = images[i];
					$img.stop();
					if (parseInt($img.css("opacity")) > 0)
						$img.stop().animate({ left: opts.left, opacity: 0 }, { duration: opts.speed });
				};
				try {
					var oldSlogan = $($el.find(opts.slogansContainer).children()[oldIndex]);
					oldSlogan.children().each(function (i) {
						$(this).delay(i * 200).animate({
							left: -100,
							opacity: 0
						}, opts.speed / 2)
					});
				}
				catch (e) {

				}

				//old.stop().animate({ left: opts.left, opacity: 0 }, { duration: opts.speed });
				//preparing next picture
				next.css({ left: opts.left, top: opts.top, "display": "block" });
				next.stop().animate({ left: opts.left - opts.leftMargin, top: opts.top - opts.topMargin, opacity: 1 }, { duration: opts.speed });
				try {
					var oldSlogan = $($el.find(opts.slogansContainer).children()[current - 1]);

					if (!oldSlogan.data("init")) {
						oldSlogan.data("init", true);
						oldSlogan.css("display", "block");
						oldSlogan.children().each(function (i) {
							$(this).css({
								left: -100,
								opacity: 0
							})
						});
					}

					oldSlogan.children().each(function (i) {
						$(this).delay(opts.speed + i * 200).animate({
							left: 0,
							opacity: 1
						}, opts.speed / 2)
					});
				}
				catch (e) {

				}


				if (animateFlag) {
					next.delay(opts.wait)
						.queue(_Animate);
				}

				bulletsContainer.find("li").removeClass('selected');
				bulletsContainer.find("#bullet-" + current).addClass('selected');
			}
		}
		catch (e) { }
	}

	this._animate = _Animate;

	this.play = function () {
		$(this).trigger("play");

		animateFlag = true;
		thisSlideShow._animate();
	}
	this.pause = function () {
		$(this).trigger("pause");

		animateFlag = false;
	}
	this.stop = this.pause;

	init();
	return this;
};

$.fn.lwSticky = function (opts) {
	var options = {
		offset: { top: 0, left: 0 },
		scrollOffset: { top: 0, left: 0 },
		container: null,
		onSticky: null,
		onUnSticky: null,
		zIndex: 10000
	};
	options = $.extend(options, opts);

	var stickyMap = StickyMap();

	this.each(function () {
		var el = $(this);

		stickyMap.push({
			el: el,
			options: options,
			pos: el.position(),
			width: el.width(),
			height: el.height(),
			isSticky: false,
			position: el.css("position"),
			top: parseInt(el.css("top"))
		});
		if (!options.container) {
			if (el.css("position") == "static") {
				var div = el.wrap("<div class=\"lw-sticky-container\" />").parent();

				div.css({
					width: el.outerWidth(),
					height: el.outerHeight(),
					display: el.css("display"),
					float: el.css("float"),
					position: el.css("position"),
					padding: 0,
					margin: el.css("margin"),
					top: el.css("top"),
					left: el.css("left")
				});
			}
		} else {
			if (typeof options.container == "string")
				options.container = $(options.container);
		}
	});


	$(window).scroll(function () {
		var scrollTop = $(this).scrollTop();
		var scrollLeft = $(this).scrollLeft();

		for (var i = 0; i < stickyMap.length; i++) {
			var obj = stickyMap[i];

			if (scrollTop > obj.pos.top + obj.options.offset.top) {
				if (obj.isSticky === false) {
					obj.el.data("o-left", obj.el.css("left"));
					obj.el.data("o-top", obj.el.css("top"));
					obj.el.css({
						top: obj.options.scrollOffset.top,
						position: "fixed",
						zIndex: obj.options.zIndex,
						width: obj.el.width(),
						left: obj.el.offset().left
					});

					if (obj.options.onSticky && typeof obj.options.onSticky == "function") {
						obj.options.onSticky(obj);
					}

					obj.isSticky = true;
				}
			} else {
				if (obj.isSticky === true) {
					obj.el.css({ position: obj.position, top: obj.top, left: obj.el.data("o-left") });

					if (obj.options.onUnSticky && typeof obj.options.onUnSticky == "function")
						obj.options.onUnSticky(obj);

					obj.isSticky = false;
				}
			}
		}
	});

	function StickyMap() {
		if (!window.stickyMap)
			window.stickyMap = [];

		return window.stickyMap;
	}
};