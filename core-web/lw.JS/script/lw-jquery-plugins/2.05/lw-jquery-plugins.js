/// <reference path="jquery-vsdoc.js" />
/// <reference path="utils.js" />
/// <reference path="lw.js" />



(function ($) {
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
			parallaxOffsetTop: 30,
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


			if (currentLoadedImage >= opts.max) {
				if (!opts.startPaused)
					setTimeout(thisSlideShow.play, opts.wait);

				return;
			}

			var img = $("<img />");
			img.attr("src", images[currentLoadedImage]);

			$el.append(img);

			img.css({ "z-index": 1 });

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


		}


		var scrollLock = false;
		var animateTimeout = null;

		if (opts.parallax === true) {
			function CheckScroll() {
				try {
					var scrollTop = $(this).scrollTop();
					var scrollLeft = $(this).scrollLeft();

					var pos = $el.offset();

					var img = images[current - 1];

					if (!img.data("o-top"))
						img.data("o-top", img.position().top);

					if (scrollTop > pos.top) {
						scrollLock = true;
						animateFlag = false;

						if (typeof (img) != "undefined") {

							//img.stop().animate({ opacity: 1 });

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
							img.addClass("notransition");
							slogansContainer.addClass("notransition");
							img.css({ top: (scrollTop + img.data("o-top") - pos.top) / imgDensity - opts.parallaxOffsetTop });
							slogansContainer.css({ top: (scrollTop - pos.top) / sloganDensity - opts.parallaxOffsetTop });
							setTimeout(function () {
								img.removeClass("notransition");
								slogansContainer.removeClass("notransition");
							}, 500);
						}
						if (animateTimeout)
							clearTimeout(animateTimeout);
					} else {
						if (scrollLock) {
							scrollLock = false;
							animateFlag = true;
							$el.find("img").css({ top: img.data("o-top") });
							slogansContainer.css({ top: slogansContainer.data("top") });
							if (animateTimeout)
								clearTimeout(animateTimeout);
							animateTimeout = window.setTimeout(thisSlideShow._animate, opts.wait);
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
						$img.removeClass("im-visible");
					};
					try {
						var oldSlogan = $($el.find(opts.slogansContainer).children()[oldIndex]);
						oldSlogan.each(function (i) {
							$(this).css({ display: 'none' })
						});
						oldSlogan.children().each(function (i) {
							$(this).delay(i * 200).animate({
								left: -100,
								opacity: 0
							}, opts.speed / 2);
						});
					}
					catch (e) {

					}

					//old.stop().animate({ left: opts.left, opacity: 0 }, { duration: opts.speed });
					//preparing next picture
					//next.css({ left: opts.left, top: opts.top, "display": "block" });
					next.addClass("im-visible");
					try {
						var oldSlogan = $($el.find(opts.slogansContainer).children()[current - 1]);

						if (!oldSlogan.data("init")) {
							oldSlogan.data("init", true);
							oldSlogan.css("display", "block");
							oldSlogan.children().each(function (i) {
								$(this).css({
									left: -100,
									opacity: 0
								});
							});
						}

						oldSlogan.each(function (i) {
							$(this).css({ display: 'block' })
						});
						oldSlogan.children().each(function (i) {
							$(this).delay(opts.speed + i * 200).animate({
								left: 0,
								opacity: 1
							}, opts.speed / 2);
						});
					}
					catch (e) {

					}


					if (animateFlag) {

						if (animateTimeout)
							clearTimeout(animateTimeout);
						animateTimeout = window.setTimeout(_Animate, opts.wait);
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
		};
		this.pause = function () {
			$(this).trigger("pause");

			animateFlag = false;
		};
		this.stop = this.pause;

		init();
		return this;
	};
})(jQuery);

(function ($) {
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
				//width: el.css("width"),//.width(),
				height: el.height(),
				isSticky: false,
				position: el.css("position"),
				top: parseInt(el.css("top"))
			});
			if (!options.container) {
				if (el.css("position") == "static") {
					var div = el.wrap("<div class=\"lw-sticky-container\" />").parent();

					div.css({
						//width: el.css("width"),//el.outerWidth(),
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
							//width: obj.el.css("width"),//.width(),
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
})(jQuery);



(function ($) {
	//var opts = {
	//	proxy: null,
	//	container: null,
	//	cssClass: "auto-complete",
	//	minChars: null,
	//	onChange: null,
	//	template: "<div><a href=\"{vroot}/{UniqueName}\">{Name}</a></div>",
	//	templateSelector: "div",
	//	templateSelectedClass: "selected",
	//	multiple: false,
	//	onShow: null,
	//	onHide: null,
	//	maxResults: null,
	//	moreResultsLink: null,
	//	moreResultsText: "More Results",
	//	moreResultsClass: "more-resutls",
	//	stopEnter: false
	//};


	function lwSearchSite(opts, el) {
		this.arrData = null;

		this.textField = el;
		this.proxy = opts.proxy;

		var minChars = 1;
		var currRow = 0;
		this.divId = "autocomplete_" + lw.utils.an();

		var listDiv = $("<div id=" + this.divId + "/>");

		if (opts.container)
			opts.container.append(listDiv);
		else
			this.textField.after(listDiv);
		this.textField.attr("autocomplete", "off");

		this.holder = listDiv;
		this.holder.addClass(opts.cssClass).addClass("hidden");

		this.onSelected = null;

		var me = this;
		this.textField.on("keyup",

			function (e) {

				var val = encodeURI($(this).val());

				if (e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40 && e.keyCode != 13) {
					if (val.length >= minChars) {
						$.ajax({
							url: me.proxy + val,
							success: function (data) {
								try {
									//debugger;
									var arr = data.data;
									var html = [];

									currRow = 0;

									if (arr == null || arr.length == 0 || data.total == 0) {
										me.hide();
									}
									else {
										if (arr.length > 0) {
											for (i = 0; i < Math.min(opts.maxResults, arr.length) ; i++) {
												arr[i].vroot = lw.vroot;
												if (arr[i]["UniqueName"].indexOf("testimonials/") != -1 || arr[i]["Path"].indexOf("testimonials/") != -1) {
													arr[i]["Path"] = arr[i]["Path"].replace("testimonials/", "testimonials#");
													arr[i]["UniqueName"] = arr[i]["UniqueName"].replace("testimonials/", "testimonials#");
												}
												html.push(opts.template.Format(arr[i]));
											}

											opts.val = val;
											opts.vroot = lw.vroot;

											if (opts.maxResults < arr.length)
												html.push("<div class=\"{moreResultsClass}\"><a href=\"{vroot}/{moreResultsLink}{val}\">{moreResultsText}</a>".Format(opts));

											me.holder.html(html.join(""));

											var target = me.holder.find("div");

											me.show();
										}
									}
								}
								catch (e) {
									//alert('Sorry, an error has occured!');
								}
							},
							error: function (xhr, status, ex) {
								//alert('Sorry, an error has occured!');
							}
						});
					}
					else {
						me.hide();
					}
				}
				else {
					if (me.holder.css("display") != "none") {
						checkKey(e);
					}
					else {
						// Callback function
						if (me.onSelected != null) {
							me.onSelected.call(this, null);
						}
					}
				}
			}
		);

		this.textField.on({
			"blur":
			function (e) {
				setTimeout(function () {
					me.hide();
				}, 100);
			},
			"keydown": function (e) {
				if (e.keyCode == 13) {
					var item = me.holder.find("." + opts.templateSelectedClass);
					if (item.length > 0) {
						var a = item.find("a");
						location.href = a[0].href;
						return false;
					}
				}
			}
		});

		this.show = function (height) {
			this.holder.removeClass("hidden");
			if (opts.onShow && "function" == typeof opts.onShow)
				opts.onShow(this);
		};
		this.hide = function () {
			this.holder.addClass("hidden");
			if (opts.onHide && "function" == typeof opts.onHide)
				opts.onHide(this);
		};
		this.setMinChars = function (c) {
			minChars = c;
		};
		this.preventEnter = function () {
			this.textField.keypress(
				function (e) {
					if (e.keyCode == 13) {
						return false;
					}

					return true;
				}
			);
		};

		function checkKey(e) {
			if (me.holder.css("display") != "none") {
				var selected = me.holder.find(">" + opts.templateSelector + "." + opts.templateSelectedClass);
				if (e.keyCode == 40) { //moving down
					if (selected.length > 0) {
						var next = selected.next();
						if (next.length > 0) {
							next.addClass(opts.templateSelectedClass);
							selected.removeClass(opts.templateSelectedClass);
						}
					} else {
						$(me.holder.find(">" + opts.templateSelector)[0]).addClass(opts.templateSelectedClass);
					}
				}
				else if (e.keyCode == 38) { //moving up
					if (selected.length > 0) {
						var next = selected.prev();
						if (next.length > 0) {
							next.addClass(opts.templateSelectedClass);
							selected.removeClass(opts.templateSelectedClass);
						}
					} else {
						var items = me.holder.find(">" + opts.templateSelector);
						$(items[items.length - 1]).addClass(opts.templateSelectedClass);
					}
				}
				else if (e.keyCode == 13) { //enter key

					//if (selected.length > 0)
					//	return false;

					//return true;

					//me.hide();
				}
			}
			else {
				// Callback function
				if (me.onSelected != null) {
					me.onSelected.call(this, null);
				}
			}

			return true;
		}
	}

	$.fn.lwSearchSite = function (options) {
		var opts = {
			proxy: null,
			container: null,
			cssClass: "auto-complete",
			minChars: null,
			onChange: null,
			template: "<div><a href=\"{vroot}/{UniqueName}\">{Name}</a></div>",
			templateSelector: "div",
			templateSelectedClass: "selected",
			multiple: false,
			onShow: null,
			onHide: null,
			maxResults: null,
			moreResultsLink: null,
			moreResultsText: "More Results",
			moreResultsClass: "more-resutls",
			stopEnter: false
		};
		$.extend(opts, options);

		return this.each(function () {
			var obj = new lwSearchSite(opts, $(this));

			if (opts.width != null) {
				obj.setWidth(settings.width);
			}

			if (opts.minChars != null) {
				obj.setMinChars(opts.minChars);
			}

			if (opts.stopEnter)
				obj.preventEnter();

			if ($.isFunction(opts.onChange) == true) {
				obj.onChange = opts.onChange;
			}
		});
	};
})(jQuery);




/*
		 * Swipe 2.0
		 *
		 * Brad Birdsall
		 * Copyright 2013, MIT License
		 * 
		 * Modified by Alain added options.createBullets 
		 * which adds a bullets container that you can click 
		 * and animate to the corresponding slide
		 *
		*/
function Swipe(container, options) {

	"use strict";

	// utilities
	var noop = function () { }; // simple no operation function
	var offloadFn = function (fn) { setTimeout(fn || noop, 0); }; // offload a functions execution

	// check browser capabilities
	var browser = {
		addEventListener: !!window.addEventListener,
		touch: ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch,
		transitions: (function (temp) {
			var props = ['transitionProperty', 'WebkitTransition', 'MozTransition', 'OTransition', 'msTransition'];
			for (var i in props) if (temp.style[props[i]] !== undefined) return true;
			return false;
		})(document.createElement('swipe'))
	};

	// quit if no root element
	if (!container) return;
	var element = container.children[0];
	var slides, slidePos, width, length;
	options = options || {};
	var index = parseInt(options.startSlide, 10) || 0;
	var speed = options.speed || 300;
	options.continuous = options.continuous !== undefined ? options.continuous : true;
	var animateProperty = "left";
	if (options.rtl)
		animateProperty = "right";

	function setup() {

		// cache slides
		slides = element.children;
		length = slides.length;

		// set continuous to false if only one slide
		if (slides.length < 2) options.continuous = false;

		//special case if two slides
		if (browser.transitions && options.continuous && slides.length < 3) {
			element.appendChild(slides[0].cloneNode(true));
			element.appendChild(element.children[1].cloneNode(true));
			slides = element.children;
		}

		// create an array to store current positions of each slide
		slidePos = new Array(slides.length);

		// determine width of each slide
		width = container.getBoundingClientRect().width || container.offsetWidth;

		element.style.width = (slides.length * width) + 'px';

		// stack elements
		var pos = slides.length;
		while (pos--) {

			var slide = slides[pos];

			slide.style.width = width + 'px';
			slide.setAttribute('data-index', pos);

			if (browser.transitions) {
				slide.style[animateProperty] = (pos * -width) + 'px';
				move(pos, index > pos ? -width : (index < pos ? width : 0), 0);
			}

		}

		// reposition elements before and after index
		if (options.continuous && browser.transitions) {
			move(circle(index - 1), -width, 0);
			move(circle(index + 1), width, 0);
		}

		if (!browser.transitions) element.style[animateProperty] = (index * -width) + 'px';

		container.style.visibility = 'visible';


		if (options.createBullets)
			createBullets();
	}

	function createBullets() {
		if (!isOk(options.bulletsContainer))
			return;

		if (length <= 1)
			return;

		var cont = $(options.bulletsContainer);
		cont.html("");

		var list = $("<ul/>");

		for (var i = 0; i < length; i++) {
			var a = $("<li data-index={0}>{0}</li>".Format(i));
			if (i == index)
				a.addClass("selected");

			list.append(a);
			a.click(function () {
				var index = $(this).data("index");
				stop();
				slide(index);
			});
		}
		cont.append(list);
	}

	function selectBullet() {
		if (options.createBullets) {
			$("{0} ul li".Format(options.bulletsContainer)).removeClass("selected");
			$("{0} ul li:nth-child({1})".Format(options.bulletsContainer, parseInt(index) + 1)).addClass("selected");
		}
	}

	function prev() {

		if (options.continuous) slide(index - 1);
		else if (index) slide(index - 1);

	}

	function next() {

		if (options.continuous) slide(index + 1);
		else if (index < slides.length - 1) slide(index + 1);

	}

	function circle(index) {

		// a simple positive modulo using slides.length
		return (slides.length + (index % slides.length)) % slides.length;

	}

	function slide(to, slideSpeed) {

		// do nothing if already on requested slide
		if (index == to) return;

		if (browser.transitions) {

			var direction = Math.abs(index - to) / (index - to); // 1: backward, -1: forward

			// get the actual position of the slide
			if (options.continuous) {
				var natural_direction = direction;
				direction = -slidePos[circle(to)] / width;

				// if going forward but to < index, use to = slides.length + to
				// if going backward but to > index, use to = -slides.length + to
				if (direction !== natural_direction) to = -direction * slides.length + to;

			}

			var diff = Math.abs(index - to) - 1;

			// move all the slides between index and to in the right direction
			while (diff--) move(circle((to > index ? to : index) - diff - 1), width * direction, 0);

			to = circle(to);

			move(index, width * direction, slideSpeed || speed);
			move(to, 0, slideSpeed || speed);

			if (options.continuous) move(circle(to - direction), -(width * direction), 0); // we need to get the next in place

		} else {

			to = circle(to);
			animate(index * -width, to * -width, slideSpeed || speed);
			//no fallback for a circular continuous if the browser does not accept transitions
		}

		index = to;

		selectBullet(index);

		offloadFn(options.callback && options.callback(index, slides[index]));
	}

	function move(index, dist, speed) {

		translate(index, dist, speed);
		slidePos[index] = dist;

		selectBullet(index);
	}

	function translate(index, dist, speed) {

		var slide = slides[index];
		var style = slide && slide.style;

		if (!style) return;

		style.webkitTransitionDuration =
		style.MozTransitionDuration =
		style.msTransitionDuration =
		style.OTransitionDuration =
		style.transitionDuration = speed + 'ms';

		style.webkitTransform = 'translate(' + dist + 'px,0)' + 'translateZ(0)';
		style.msTransform =
		style.MozTransform =
		style.OTransform = 'translateX(' + dist + 'px)';

	}

	function animate(from, to, speed) {

		// if not an animation, just reposition
		if (!speed) {

			element.style[animateProperty] = to + 'px';
			return;

		}

		var start = +new Date;

		var timer = setInterval(function () {

			var timeElap = +new Date - start;

			if (timeElap > speed) {

				element.style[animateProperty] = to + 'px';

				if (delay) begin();

				options.transitionEnd && options.transitionEnd.call(event, index, slides[index]);

				clearInterval(timer);
				return;

			}

			element.style[animateProperty] = (((to - from) * (Math.floor((timeElap / speed) * 100) / 100)) + from) + 'px';

		}, 4);

	}

	// setup auto slideshow
	var delay = options.auto || 0;
	var interval;

	function begin() {

		interval = setTimeout(next, delay);

	}

	function stop() {

		delay = 0;
		clearTimeout(interval);

	}


	// setup initial vars
	var start = {};
	var delta = {};
	var isScrolling;

	// setup event capturing
	var events = {

		handleEvent: function (event) {

			switch (event.type) {
				case 'touchstart': this.start(event); break;
				case 'touchmove': this.move(event); break;
				case 'touchend': offloadFn(this.end(event)); break;
				case 'webkitTransitionEnd':
				case 'msTransitionEnd':
				case 'oTransitionEnd':
				case 'otransitionend':
				case 'transitionend': offloadFn(this.transitionEnd(event)); break;
				case 'resize': offloadFn(setup); break;
			}

			if (options.stopPropagation) event.stopPropagation();

		},
		start: function (event) {

			var touches = event.touches[0];

			// measure start values
			start = {

				// get initial touch coords
				x: touches.pageX,
				y: touches.pageY,

				// store time to determine touch duration
				time: +new Date

			};

			// used for testing first move event
			isScrolling = undefined;

			// reset delta and end measurements
			delta = {};

			// attach touchmove and touchend listeners
			element.addEventListener('touchmove', this, false);
			element.addEventListener('touchend', this, false);

		},
		move: function (event) {

			// ensure swiping with one touch and not pinching
			if (event.touches.length > 1 || event.scale && event.scale !== 1) return;
			if (options.disableScroll) event.preventDefault();

			var touches = event.touches[0];

			// measure change in x and y
			delta = {
				x: touches.pageX - start.x,
				y: touches.pageY - start.y
			}; // determine if scrolling test has run - one time test
			if (typeof isScrolling == 'undefined') {
				isScrolling = !!(isScrolling || Math.abs(delta.x) < Math.abs(delta.y));
			}

			// if user is not trying to scroll vertically
			if (!isScrolling) {

				// prevent native scrolling
				event.preventDefault();

				// stop slideshow
				stop();

				// increase resistance if first or last slide
				if (options.continuous) { // we don't add resistance at the end

					translate(circle(index - 1), delta.x + slidePos[circle(index - 1)], 0);
					translate(index, delta.x + slidePos[index], 0);
					translate(circle(index + 1), delta.x + slidePos[circle(index + 1)], 0);

				} else {

					delta.x =
					  delta.x /
						((!index && delta.x > 0               // if first slide and sliding left
						  || index == slides.length - 1        // or if last slide and sliding right
						  && delta.x < 0                       // and if sliding at all
						) ?
						(Math.abs(delta.x) / width + 1)      // determine resistance level
						: 1);                                 // no resistance if false

					// translate 1:1
					translate(index - 1, delta.x + slidePos[index - 1], 0);
					translate(index, delta.x + slidePos[index], 0);
					translate(index + 1, delta.x + slidePos[index + 1], 0);
				}

			}

		},
		end: function (event) {

			// measure duration
			var duration = +new Date - start.time;

			// determine if slide attempt triggers next/prev slide
			var isValidSlide =
				  Number(duration) < 250               // if slide duration is less than 250ms
				  && Math.abs(delta.x) > 20            // and if slide amt is greater than 20px
				  || Math.abs(delta.x) > width / 2;      // or if slide amt is greater than half the width

			// determine if slide attempt is past start and end
			var isPastBounds =
				  !index && delta.x > 0                            // if first slide and slide amt is greater than 0
				  || index == slides.length - 1 && delta.x < 0;    // or if last slide and slide amt is less than 0

			if (options.continuous) isPastBounds = false;

			// determine direction of swipe (true:right, false:left)
			var direction = delta.x < 0;

			// if not scrolling vertically
			if (!isScrolling) {

				if (isValidSlide && !isPastBounds) {

					if (direction) {

						if (options.continuous) { // we need to get the next in this direction in place

							move(circle(index - 1), -width, 0);
							move(circle(index + 2), width, 0);

						} else {
							move(index - 1, -width, 0);
						}

						move(index, slidePos[index] - width, speed);
						move(circle(index + 1), slidePos[circle(index + 1)] - width, speed);
						index = circle(index + 1);

					} else {
						if (options.continuous) { // we need to get the next in this direction in place

							move(circle(index + 1), width, 0);
							move(circle(index - 2), -width, 0);

						} else {
							move(index + 1, width, 0);
						}

						move(index, slidePos[index] + width, speed);
						move(circle(index - 1), slidePos[circle(index - 1)] + width, speed);
						index = circle(index - 1);

					}

					options.callback && options.callback(index, slides[index]);

				} else {

					if (options.continuous) {

						move(circle(index - 1), -width, speed);
						move(index, 0, speed);
						move(circle(index + 1), width, speed);

					} else {

						move(index - 1, -width, speed);
						move(index, 0, speed);
						move(index + 1, width, speed);
					}

				}

			}

			// kill touchmove and touchend event listeners until touchstart called again
			element.removeEventListener('touchmove', events, false);
			element.removeEventListener('touchend', events, false);
		},
		transitionEnd: function (event) {

			if (parseInt(event.target.getAttribute('data-index'), 10) == index) {

				if (delay) begin();

				options.transitionEnd && options.transitionEnd.call(event, index, slides[index]);

			}

		}

	}; // trigger setup
	setup();

	// start auto slideshow if applicable
	if (delay) begin();


	// add event listeners
	if (browser.addEventListener) {

		// set touchstart event on element
		if (browser.touch) element.addEventListener('touchstart', events, false);

		if (browser.transitions) {
			element.addEventListener('webkitTransitionEnd', events, false);
			element.addEventListener('msTransitionEnd', events, false);
			element.addEventListener('oTransitionEnd', events, false);
			element.addEventListener('otransitionend', events, false);
			element.addEventListener('transitionend', events, false);
		}

		// set resize event on window
		window.addEventListener('resize', events, false);

	} else {

		window.onresize = function () { setup(); }; // to play nice with old IE

	}

	// expose the Swipe API
	return {
		setup: function () {

			setup();

		},
		slide: function (to, speed) {

			// cancel slideshow
			stop();

			slide(to, speed);

		},
		prev: function () {

			// cancel slideshow
			stop();

			prev();

		},
		next: function () {

			// cancel slideshow
			stop();

			next();

		},
		stop: function () {

			// cancel slideshow
			stop();

		},
		getPos: function () {

			// return current index position
			return index;

		},
		getNumSlides: function () {

			// return total number of slides
			return length;
		},
		kill: function () {

			// cancel slideshow
			stop();

			// reset element
			element.style.width = '';
			element.style[animateProperty] = '';

			// reset slides
			var pos = slides.length;
			while (pos--) {

				var slide = slides[pos];
				slide.style.width = '';
				slide.style[animateProperty] = '';

				if (browser.transitions) translate(pos, 0, 0);

			}

			// removed event listeners
			if (browser.addEventListener) {

				// remove current event listeners
				element.removeEventListener("touchstart", events, false);
				element.removeEventListener('webkitTransitionEnd', events, false);
				element.removeEventListener('msTransitionEnd', events, false);
				element.removeEventListener('oTransitionEnd', events, false);
				element.removeEventListener('otransitionend', events, false);
				element.removeEventListener('transitionend', events, false);
				window.removeEventListener('resize', events, false);

			}
			else {

				window.onresize = null;

			}

		}
	};
}


if (window.jQuery || window.Zepto) {
	(function ($) {
		$.fn.Swipe = function (params) {
			return this.each(function () {
				$(this).data('Swipe', new Swipe($(this)[0], params));
			});
		};
	})(window.jQuery || window.Zepto);
}

