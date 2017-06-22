/// <reference path="jquery-vsdoc.js" />
/// <reference path="utils.js" />
/// <reference path="lw.js" />

/// Copyright 2012 www.liteweb.info
/// Version: 2.1
/// Last Update: 1/22/2013

(function ($) {
	//Creating the lw object (used to hold global variables)
	var lw = this.lw ? this.lw : {};

	//switch between 2 images 
	//usually one grayscale and one colored
	$.fn.fadeOnHover = function (options) {
		var opts = {
			param: "-hover",
			duration: 500
		};
		opts = $.extend(opts, options);
		return this.each(function () {
			var $this = $(this);
			if (!this.img)
				this.img = $this.children("img");
			if (this.img.length === 0)
				return true;
			this.imgSrc = this.img[0].src;
			this.img.css("position", "absolute");
			var temp = this.imgSrc.split(".");
			var extension = temp.pop();
			this.hoverSrc = temp.join(".") + opts.param + "." + extension;

			this.hoverImg = $("<img />");
			this.hoverImg.attr("src", this.hoverSrc).css({
				posiotion: "absolute",
				opacity: 0
			}).appendTo($this);
			$this.bind("mouseover", function () {
				this.hoverImg.stop().animate({ opacity: 1 }, { duration: opts.duration });
				this.img.stop().animate({ opacity: 0 }, { duration: opts.duration });
			});
			$this.bind("mouseout", function () {
				this.hoverImg.stop().animate({ opacity: 0 }, { duration: opts.duration });
				this.img.stop().animate({ opacity: 1 }, { duration: opts.duration });
			});
		});
	};

	//created an internal popup window
	//pass the id as an anchor <a href="#popupid" />
	$.fn.popup = function (options) {
		var opts = {
			overlay: {
				color: "#000000",
				opacity: 0.55,
				duration: 600,
				open: null,
				close: null
			},
			duration: 500,
			close: null,
			open: null,
			openEasing: "easeInCirc",
			closeEasing: "easeOutCirc",
			top: 155
		};
		opts = $.extend(opts, options);

		function closeHandler() {
			if (typeof opts.close === "function")
				return opts.close();
			closeOverlay();
			this.el.popupDiv.animate({ top: -5000 }, { duration: opts.duration, easing: opts.closeEasing });
		}
		function closeOverlay() {
			if (typeof opts.overlay.close === "function")
				return opts.overlay.close();

			var h = $(document).height();
			var w = $(document).width();
			var overlays = lw.overlays;
			for (var i = 0; i < lw.overlays.length; i++) {
				obj = {
					width: 0,
					height: 0,
					left: i % 2 === 0 ? 0 : w,
					top: i < 2 ? 0 : h,
					opacity: 0
				};
				lw.overlays[i].animate(obj, {
					duration: opts.overlay.duration,
					complete: function () {
						$(document.body).css("overflow", "auto");
						$(document).unbind("keyup");
					}
				});
			}
		}
		function showOverlay() {
			if (typeof opts.overlay.open === "function")
				return opts.overlay.open();

			var body = $(document.body);
			body.css("overflow", "hidden");
			if (!lw.overlays) {


				lw.overlays = new Array(4);
				for (var i = 0; i < lw.overlays.length; i++) {
					lw.overlays[i] = $(document.createElement("div"));
					body.append(lw.overlays[i]);
				}
			}
			var h = $(document).height();
			var w = $(document).width();

			for (i = 0; i < lw.overlays.length; i++) {
				var obj = {
					position: "absolute",
					width: 0,
					height: 0,
					background: opts.overlay.color,
					opacity: 0,
					top: i < 2 ? 0 : h,
					left: i % 2 === 0 ? 0 : w,
					"z-index": 9000
				};
				lw.overlays[i].css(obj);
				obj = {
					width: w / 2,
					height: h / 2,
					left: i % 2 === 0 ? 0 : w / 2,
					top: i < 2 ? 0 : h / 2,
					opacity: opts.overlay.opacity
				};
				lw.overlays[i].animate(obj, { duration: opts.overlay.duration });
			}
		}
		function clickHandler() {
			showOverlay();
			showPopup(this);
		}
		function showPopup(el) {
			var p = el.popupDiv;
			obj = {
				position: "absolute",
				left: $(window).width() / 2 - p.width() / 2,
				display: "block",
				"z-index": 9001
			};
			p.css(obj);
			p.animate({ top: opts.top },
				{ duration: opts.duration, easing: opts.openEasing });

			var closeB = el.closeButton;
			$(document).bind("keyup", function (e) {
				if (e.keyCode == 27)
					closeB.trigger("click");
			})
		}


		return this.each(function () {
			var $this = $(this);
			this.popupDiv = $("#" + this.href.split("#").pop());
			this.closeButton = $("<div class=close title=Close />").css({
				"margin-left": this.popupDiv.width() - 15,
				"margin-top": 5
			});
			this.popupDiv.prepend(this.closeButton);
			this.closeButton[0].el = this;
			this.closeButton.bind("click", closeHandler);
			$this.bind("click", clickHandler);
		});
	}


	$.fn.menu = function (options) {
		var opts = {
			duration: 300,
			easing: "easeOutQuad",
			direction: "v",
			childrenContainer: "ol",
			margin: 0
		};
		opts = $.extend(opts, options);

		function createMenu(el, parent) {
			var m = {};
			m.el = $(el);
			m._a = $(el).children("a");

			m.child = $(el).children(opts.childrenContainer);

			if (m._a.length == 0 || m.child.length == 0)
				return;

			m.a = m._a[0];
			m.ul = m.child[0];

			m.li = m.child.children("li");
			m.maxHeight = m.child.outerHeight() + 2; //m.li.length * 23 + 22 + ($.browser.safari && $.browser.version < 526 ? (-2) : 0);
			m.maxWidth = m.child.outerWidth() + 2;

			m.visible = false;
			m.hideMe = false;
			m.parent = parent;
			m.a.menu = m;
			m.child[0].menu = m;


			m.show = function () {
				this.hideMe = false;
				if (this.visible)
					return;
				this.visible = true;

				this.child.css({
					overflow: "hidden",
					visibility: "visible",
					height: opts.direction == "v" ? 0 : "auto",
					top: opts.direction == "v" ? this._a.position().top + this._a.outerHeight() : this._a.position().top + (m._index == 0 ? -1 : 0),
					left: opts.direction == "v" ? this._a.position().left : this._a.position().left + this._a.outerWidth() + opts.margin,
					width: opts.direction != "v" ? 0 : "auto"
				});
				this.child.stop();
				var animateTo = { height: this.maxHeight };
				if (opts.direction != "v")
					animateTo = { width: this.maxWidth };

				$.extend(animateTo, { opacity: 1 });

				this.child.animate(animateTo, { duration: opts.duration, easing: opts.easing });
				this._a.addClass("_hover");
			};
			m.hide = function () {
				this.hideMe = true;
				window.setTimeout(function () {
					m._hide();
				}, 50);
			}
			m._hide = function () {
				if (!this.hideMe)
					return;
				this.visible = false;
				for (var i = 0; i < this.children.length; i++) {
					if (this.children[i].visible)
						return;
				}
				this.child.stop();

				var animateTo = { height: 0 };
				if (opts.direction != "v")
					animateTo = { width: 0 };

				$.extend(animateTo, { opacity: 0 });

				this.child.animate(animateTo, {
					duration: opts.duration / 2, easing: opts.easing,
					complete: function () {
						if (!this.menu.hideMe)
							return;
						this.menu.child.css("visibility", "hidden");
						this.menu._a.removeClass("_hover");
					}
				});
			}
			m.children = [];
			m.li.each(function (i, el) {
				m.children.push(el);
				el.menu = createMenu(el, this);
				var $a = $($(this).children("a"));
				if ($(el).children(opts.childrenContainer).length == 0)
					$a.addClass("arrow1");
			});
			if (m.children.length > 0 && m.parent != null) {
				m._a.addClass("arrow");
			}
			m._a.bind("mouseover", function () {
				this.title = "";
				if (this.menu) {
					this.menu.show();
				}
			});
			m._a.bind("mouseout", function () {
				if (this.menu)
					this.menu.hide();
			});
			m.child.bind("mouseover", function () {
				if (this.menu)
					this.menu.show();
			});
			m.child.bind("mouseout", function () {
				if (this.menu)
					this.menu.hide();
			});
			return m;
		}
		function hideMenu(el) {
			window.setTimeout(function () {
				_hideMenu(el);
			}, 200);
		}
		function _hideMenu(el) {
			if (el.childVisible)
				return;
			el._sub.css("display", "none");
			el._ol.css("height", 0)
			el._a.removeClass("_hover");
		}
		return this.each(function (i, el) {
			el.menu = createMenu(el, opts.parent, i);
		});
	}

	$.fx._default = function (fx) {
		if (fx.elem.style)
			fx.elem.style[fx.prop] = fx.now + fx.unit;
		else
			fx[fx.prop] = fx.now;
	}


	$.fn.lwCarousel = function (options) {
		var opts = {
			footer: null,
			visibleItems: null,
			next: null,
			prev: null,
			visibleItems: null,
			easing: "easeInSine",
			duration: 500,
			durationBetween: 100,
			children: "article",
			auto: null
		};
		opts = $.extend(opts, options);
		opts.positions = [];
		var els = this.children(opts.children);
		var width = this.width();
		if (opts.visibleItems == null && els.length > 0) {
			var w = $(els[0]).width();
			opts.visibleItems = Math.max(Math.floor(width / w), 1);
		}

		for (var i = 0; i < Math.min(opts.visibleItems, els.length) ; i++) {
			var pos = $(els[i]).position();
			opts.positions.push(pos);
		}
		var lastStep = 0;
		var isAnimating = false;
		var nextItems = [];
		var currentItems = [];
		var nextStartAt = 0;
		opts.autoAnim = null;
		var auto = true;

		function GoToStep(el) {
			var _el = el && el.length > 0 ? el[0] : this;
			if (el == null) {
				auto = false;
				clearTimeout(opts.autoAnim);
			}

			var step = parseInt(_el.innerHTML) - 1;
			if (step == lastStep) {
				return;
			}
			if (isAnimating)
				return;

			dir = step > lastStep ? 1 : -1;
			var oldStep = lastStep;
			lastStep = step;


			isAnimating = true;

			nextItems = [];
			nextStartAt = lastStep * opts.visibleItems * dir;



			if (dir < 0 && !auto) {
				$(els[oldStep]).css({ top: 0 });
				$(els[oldStep]).animate({ left: 300 }, { duration: opts.duration, easing: opts.easing });

				$(els[nextStartAt]).css({ left: -300, top: 0 });
				$(els[nextStartAt]).animate({ left: 0 }, { duration: opts.duration, easing: opts.easing });
			} else {
				$(els[oldStep]).css({ position: "absolute" });
				$(els[oldStep]).animate({ left: -300 }, { duration: opts.duration, easing: opts.easing });

				$(els[nextStartAt]).css({ left: 300, top: 0 });
				$(els[nextStartAt]).animate({ left: 0 }, { duration: opts.duration, easing: opts.easing });
			}


			$("._active", opts.footer).removeClass("_active");
			$(_el).addClass("_active");
			setTimeout(function () {
				isAnimating = false;
			}, 1);

		}
		var pages = Math.round(els.length / opts.visibleItems);
		if (pages < els.length / opts.visibleItems) {
			pages++;
		}

		if (opts.footer) {
			if (typeof opts.footer == "string")
				opts.footer = $(opts.footer);
			if (opts.footer.length > 0) {
				var ul = $("<ul />");
				for (var i = 0; i < pages; i++) {
					var li = $("<li />");
					li.html(i + 1);
					if (i == 0)
						li.addClass("_active");
					li.click(GoToStep);
					ul.append(li);
				}
				opts.footer.append(ul);
			}
		}
		els.each(function (i) {
			if (i >= opts.visibleItems)
				$(this).css({ left: -5000 });
			$(this).css({ position: "absolute" });
		});

		function NextForAuto() {
			var s = lastStep + 1;
			if (lastStep + 1 == pages)
				s = 0;
			GoToStep(opts.footer.children("ul").children(":nth-child(" + (s + 1) + ")"));
			opts.autoAnim = setTimeout(NextForAuto, opts.auto);
		}
		if (opts.auto) {
			opts.autoAnim = setTimeout(NextForAuto, opts.auto);
		}
	};

	

	$.fn.lwSelect = function (opts) {
		var options = {
			duration: 300,
			easing: "easeOutQuad",
			direction: "up",
			maxHeight: 250,
			onchange: function () {
			}
		};
		options = $.extend(options, opts);

		return this.each(function () {
			if (this.lwSelectInit)
				return;
			var $el = $(this);

			if ($el.hasClass("no-transform"))
				return;

			var div = $el.parent();

			$el.css("display", "none");

			div.addClass("lw-select");

			var input = $("<input type=text class=\"textfield\" name=\"{0}-input\" value=\"{1}\" />".Format($el.attr("name"), $el[0].selectedIndex >= 0 ? $el[0].options[$el[0].selectedIndex].text : ""));
			var button = $("<input type=button class=\"button\" />");
			var hideTimer = null;

			input.width($el.width());

			input.oval = this.options[0].text;
			//input.val(input.oval);

			div.css({ position: "relative" });

			button.css({
				position: "absolute",
				left: $el.width() - 20,
				border: "none"
			});


			var fillTimer = null;
			var list = $("<div />");
			list.addClass("select-list");

			list.width($el.width() + 3);

			var listOpen = false;

			input.bind("focus click", function (e) {
				if (input.val() == input.oval)
					input.val("");
				clearTimeout(hideTimer);
				e.stopPropagation();
			});
			input.blur(function () {
				if (input.val() == "")
					input.val(input.oval);
				initHide();
			});

			var fillTimer = null;
			input.bind("keyup", function (e) {
				clearTimeout(fillTimer);
				fillTimer = setTimeout(fillList, 100);
				e.stopPropagation();
			});

			function fillList(noSearch) {
				noSearch = noSearch ? noSearch : false;
				var filter = input.val();
				if (filter == input.oval)
					filter = " ";

				var ar = ["<ul>"];

				for (var i = 0; i < $el[0].options.length; i++) {
					var v = $el[0].options[i].value;
					var t = $el[0].options[i].text;

					if (noSearch || (v + " " + t).toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
						var reg = new RegExp(filter, "ig");
						//.replace(reg, "<b>" + filter + "</b>")
						ar.push("<li v=\"{0}\" ><a href=\"javascript:\">{1}</a></li>".Format(!isOk(v) ? "lw-empty-value" : v, !isOk(t) ? "&nbsp;" : t));
					}
				}
				ar.push("</ul>");

				if (ar.length == 2) {
					hideList();
					return;
				}

				listOpen = true;

				list.html(ar.join(""));

				list.find("li").on("click", function (e) {

					var el = e.currentTarget;

					while (el.tagName.toLowerCase() != "li") {
						el = el.parentNode;
					}

					el = $(el);

					var val = el.attr("v");

					input.val(el.text());

					this.value = val;

					$el.attr("v", val);

					if (val == "lw-empty-value") {
						$el[0].selectedIndex = 0;
					}
					else {
						for (var i = 0; i < $el[0].options.length; i++) {
							var v = $el[0].options[i].value;
							if (v == val) {
								$el[0].options[i].selected = true;
							}
						}
					}
					list.trigger("onchange");
				});

				clearTimeout(hideTimer);

				list.height((ar.length - 2) * 20 < options.maxHeight ? (ar.length - 2) * 20 : options.maxHeight);

				//$(document.body).unbind("click", initHide);
				$(document).bind("click", initHide);

				list.show();

				if (options.onchange) {
					list.bind("onchange", options.onchange);
				}
			}
			function hideList() {
				list.hide();
				listOpen = false;
				clearTimeout(hideTimer);
				$(document).unbind("click", initHide);
			}

			function initHide() {
				//alert("init hide");
				hideTimer = setTimeout(hideList, 75);
			}

			button.click(function (e) {
				if (listOpen)
					return hideList();
				fillList(true);
				e.stopPropagation();
			});
			button.blur(function () {
				//initHide();
			});
			div.bind("focus", function (e) {
				clearTimeout(hideTimer);
				e.stopPropagation();
			});
			list.bind("blur", function () {
				initHide();
			});

			list.hide();

			div.append(input, button, list);


		});
	};




	///Checks if an event is already bound.
	$.fn.isBound = function (type, fn) {
		try {
			var data = this.data('events')[type];

			if (data === undefined || data.length === 0) {
				return false;
			}

			return (-1 !== $.inArray(fn, data));
		}
		catch (e) {
			return false;
		}
	};


	$.fn.AnalogueClock = function (options) {
		var opts = {
			rad: 250,
			secondsColor: "#fff",
			minutesColor: "#fff",
			hoursColor: "#fff",
			drawClockShape: true,
			insideColor: "red",
			circumferanceColor: "yellow"
		};
		opts = $.extend(opts, options);

		return this.each(function () {
			var rad = opts.rad;

			var cv = this;
			var cx;

			if (cv.getContext('2d')) {
				cx = cv.getContext('2d');
			}

			cv.width = cv.height = rad;


			cx.fillStyle = "rgba(55,133,144,0.5)";
			cx.fillRect(0, 0, rad, rad);


			var _clock_face = new (function (ctx, rad) {

				this.radius = rad / 2 - 5;
				this.center = rad / 2;
				this.canvas = ctx;

				this.draw = function () {
					this.canvas.clearRect(0, 0, this.center * 2, this.center * 2);

					if (opts.drawClockShape) {
						this.canvas.lineWidth = 4.0;
						this.canvas.strokeStyle = opts.circumferanceColor;
						this.canvas.beginPath();
						this.canvas.arc(this.center, this.center, this.radius, 0, Math.PI * 2, true);
						this.canvas.closePath();
						this.canvas.stroke();
					}

					this.drawDotes();
					this.drawHourDotes();
				}

				this.drawDotes = function () {
					if (!opts.drawClockShape)
						return;

					var theta = 0;
					var distance = this.radius * 0.9; // 90% from the center

					this.canvas.lineWidth = 0.5;
					this.canvas.strokeStyle = "#137";

					for (var i = 0; i < 60; i++) {
						theta = theta + (6 * Math.PI / 180);
						x = this.center + distance * Math.cos(theta);
						y = this.center + distance * Math.sin(theta);

						this.canvas.beginPath();
						this.canvas.arc(x, y, 1, 0, Math.PI * 2, true);
						this.canvas.closePath();
						this.canvas.stroke();
					}
				}

				this.drawHourDotes = function () {
					if (!opts.drawClockShape)
						return;

					var theta = 0;
					var distance = this.radius * 0.9;

					this.canvas.lineWidth = 5.0;
					this.canvas.strokeStyle = opts.insideColor;

					for (var i = 0; i < 12; i++) {
						theta = theta + (30 * Math.PI / 180);
						x = this.center + distance * Math.cos(theta);
						y = this.center + distance * Math.sin(theta);

						this.canvas.beginPath();
						this.canvas.arc(x, y, 1, 0, Math.PI * 2, true);
						this.canvas.closePath();
						this.canvas.stroke();
					}
				}
			})(cx, rad);

			var _second_needle = new (function (ctx, rad, sec) {
				this.sec = sec;
				this.canvas = ctx;
				this.center = rad / 2;
				this.size = this.center * 0.80;

				this.update = function (s) {
					this.sec = s;
				}

				this.draw = function () {
					theta = (6 * Math.PI / 180);
					x = this.center + this.size * Math.cos(this.sec * theta - Math.PI / 2);
					y = this.center + this.size * Math.sin(this.sec * theta - Math.PI / 2);

					this.canvas.lineWidth = 2.0;
					this.canvas.strokeStyle = opts.secondsColor;
					this.canvas.lineCap = "round";

					this.canvas.beginPath();
					this.canvas.moveTo(x, y);
					this.canvas.lineTo(this.center, this.center);
					this.canvas.closePath();
					this.canvas.stroke();

					this.next();

				}

				this.next = function () {
					this.sec++;
					if (this.sec == 60) this.sec = 0;
				}
			})(cx, rad, 0);

			var _minute_needle = new (function (ctx, rad, min, sec) {
				this.sec = sec;
				this.min = min;
				this.canvas = ctx;
				this.center = rad / 2;
				this.size = this.center * 0.65;

				this.update = function (m, s) {
					this.sec = s;
					this.min = m;
				}

				this.draw = function () {
					theta = (6 * Math.PI / 180);
					x = this.center + this.size * Math.cos(((this.min + this.sec / 60) * theta) - Math.PI / 2);
					y = this.center + this.size * Math.sin(((this.min + this.sec / 60) * theta) - Math.PI / 2);

					this.canvas.lineWidth = 3.0;
					this.canvas.strokeStyle = opts.minutesColor;
					this.canvas.lineCap = "round";

					this.canvas.beginPath();
					this.canvas.moveTo(x, y);
					this.canvas.lineTo(this.center, this.center);
					this.canvas.closePath();
					this.canvas.stroke();

					this.next();

				}

				this.next = function () {
					this.sec++;
					if (this.sec == 60) {
						this.min++;
						this.sec = 0;
						if (this.min == 60) {
							this.min = 0;
						}
					}
				}
			})(cx, rad, 0, 0);

			var _hour_needle = new (function (ctx, rad, hour, min, sec) {
				this.sec = sec;
				this.min = min;
				this.hour = hour;
				this.canvas = ctx;
				this.center = rad / 2;
				this.size = this.center * 0.40;

				this.update = function (h, m, s) {
					this.sec = s;
					this.min = m;
					this.hour = h;
				}

				this.draw = function () {
					theta = (30 * Math.PI / 180);
					x = this.center + this.size * Math.cos(((this.hour + this.min / 60 + this.sec / 3600) * theta) - Math.PI / 2);
					y = this.center + this.size * Math.sin(((this.hour + this.min / 60 + this.sec / 3600) * theta) - Math.PI / 2);

					this.canvas.lineWidth = 5.0;
					this.canvas.strokeStyle = opts.hoursColor;
					this.canvas.lineCap = "round";

					this.canvas.beginPath();
					this.canvas.moveTo(x, y);
					this.canvas.lineTo(this.center, this.center);
					this.canvas.closePath();

					this.canvas.stroke();

					this.next();

				}

				this.next = function () {
					this.sec++;
					if (this.sec == 60) {
						this.sec = 0;
						this.min++;
						if (this.min == 60) {
							this.min = 0;
							this.hour++;
							if (this.hour == 12) {
								this.hour = 0;
							}
						}
					}
				}
			})(cx, rad, 1, 0, 0);

			var _dateObject = new (function () {
				this.dateObject = new Date();

				this.hours = this.dateObject.getHours();
				this.minutes = this.dateObject.getMinutes();
				this.seconds = this.dateObject.getSeconds();

				this.refresh = function () {
					this.hours = this.dateObject.getHours();
					this.minutes = this.dateObject.getMinutes();
					this.seconds = this.dateObject.getSeconds();
				}
			})();

			var _init = function () {
				_dateObject.refresh();
				_second_needle.update(_dateObject.seconds);
				_minute_needle.update(_dateObject.minutes, _dateObject.seconds);
				_hour_needle.update(_dateObject.hours, _dateObject.minutes, _dateObject.seconds);
			}

			var _clock = function () {
				_clock_face.draw();
				_second_needle.draw();
				_minute_needle.draw();
				_hour_needle.draw();
				setTimeout(_clock, 1000 / 1);
			}

			_init();

			_clock();
		});
	}

})(jQuery);


//Used to create a scrollbar inside a div
//currently only vertical scroll is supported
//height is passed as a parameter
(function ($) {

	jQuery.fn.extend({
		lwScroll: function (options) {


			var defaults = {
				wheelStep: 20,
				width: 'auto',
				height: 'auto',
				size: '7px',
				color: '#000',
				position: 'right',
				distance: '0px',
				start: 'top',
				opacity: .4,
				containerOverflowHidden: true,
				alwaysVisible: false,
				disableFadeOut: false,
				railVisible: false,
				railColor: '#333',
				railOpacity: '0.2',
				railClass: 'lwsScrollRail',
				barClass: 'lwScrollBar',
				wrapperClass: 'lwScrollDiv',
				allowPageScroll: false,
				scroll: 0,
				barHeight: null,
				reset: false
			};

			var o = ops = $.extend(defaults, options);

			// do it for every element that matches selector
			var $this = this;
			$this.each(function () {

				var isOverPanel, isOverBar, isDragg, queueHide, touchDif,
				  barHeight, percentScroll, lastScroll,
				  divEl = "<div />",
				  minBarHeight = 30,
				  releaseScroll = false,
				  wheelStep = parseInt(o.wheelStep),
				  cwidth = isOk(o.width) ? o.width : me.width(),
				  cheight = isOk(o.height) ? o.height : me.height(),
				  size = o.size,
				  color = o.color,
				  position = o.position,
				  distance = o.distance,
				  start = o.start,
				  opacity = o.opacity,
				  disableFadeOut = o.disableFadeOut,
                  containerOverflowHidden = o.containerOverflowHidden,
				  alwaysVisible = o.alwaysVisible,
				  railVisible = o.railVisible,
				  railColor = o.railColor,
				  railOpacity = o.railOpacity,
				  allowPageScroll = o.allowPageScroll,
				  scroll = o.scroll,
				reset = o.reset;

				var me = $(this);

				me.scrollTop(0);

				if (lw.isMobile) {
					wheelStep = 50;

					me.css({
						//"overflow": "auto",
						//height: "90%"
					});
					var viewPort = { width: $(window).width(), height: $(window).height() };
					if (viewPort.width <= 1024) {
						me.css({
							"overflow": "auto",
							height: "100%"
						});

						return;
					}
				}

				var _parent = me.parent();

				if (_parent.hasClass(o.railClass))
					_parent = _parent.parent();

				if (me[0].scrollHeight <= Math.max(me.outerHeight(), _parent.outerHeight())) {

					wrapper = me.parent();

					if (wrapper.hasClass(o.wrapperClass)) {
						rail = wrapper.find("div." + o.railClass);
						bar = wrapper.find("div." + o.barClass);

						rail.remove();
						bar.remove();

						me.unwrap();

					}
					return;
				}


				var wrapper = wrapper = $(divEl).addClass(o.wrapperClass);
				var rail = $(divEl)
				  .addClass(o.railClass);
				var bar = $(divEl)
				  .addClass(o.barClass);

				//if the wrapper is already there we don't need to create it again
				if (me.parent().hasClass(o.wrapperClass)) {
					wrapper = me.parent();
					rail = wrapper.find("div." + o.railClass);
					bar = wrapper.find("div." + o.barClass)
				}
				else {
					me.wrap(wrapper);
					wrapper = me.parent();

					wrapper.append(bar);
					wrapper.append(rail);
				}

				//set the parameters
				wrapper
				  .css({
				  	position: 'relative',
				  	overflow: (containerOverflowHidden == true) ? 'hidden' : '',
				  	width: cwidth,
				  	height: cheight
				  });


				me.css({
					overflow: 'hidden',
					width: cwidth,
					height: cheight
				});


				// create scrollbar rail
				rail.css({
					width: size,
					height: '100%',
					position: 'absolute',
					top: 0,
					display: (alwaysVisible && railVisible) ? 'block' : 'none',
					zIndex: 90
				});

				// create scrollbar
				bar.css({
					width: size,
					position: 'absolute',
					top: 0,
					cursor: "pointer",
					display: alwaysVisible ? 'block' : 'none',
					zIndex: 999
				});



				// set position
				var posCss = (position == 'right') ? { right: distance } : { left: distance };
				rail.css(posCss);
				bar.css(posCss);


				// make it draggable
				bar.draggable({
					axis: 'y',
					containment: 'parent',
					start: function () { isDragg = true; },
					stop: function () { isDragg = false; hideBar(); },
					drag: function (e) {
						// scroll content
						scrollContent(0, $(this).position().top, false);
					}
				});

				// on rail over
				rail.hover(function () {
					showBar();
				}, function () {
					hideBar();
				});

				// on bar over
				bar.hover(function () {
					isOverBar = true;
				}, function () {
					isOverBar = false;
				});



				// show on parent mouseover
				me.hover(function () {
					isOverPanel = true;
					showBar();
					hideBar();
				}, function () {
					isOverPanel = false;
					hideBar();
				});

				// support for mobile
				me.bind('touchstart', function (e, b) {
					if (e.originalEvent.touches.length) {
						// record where touch started
						touchDif = e.originalEvent.touches[0].pageY;
					}
				});

				me.bind('touchmove', function (e) {
					// prevent scrolling the page
					e.originalEvent.preventDefault();
					if (e.originalEvent.touches.length) {
						// see how far user swiped
						var diff = (touchDif - e.originalEvent.touches[0].pageY) / 100;
						// scroll content
						scrollContent(diff / 2, true);
					}

				});

				var _onWheel = function (e) {
					// use mouse wheel only when mouse is over
					if (!isOverPanel) { return; }

					var e = e || window.event;

					var delta = 0;
					if (e.wheelDelta) { delta = -e.wheelDelta / 120; }
					if (e.detail) { delta = e.detail / 3; }

					// scroll content
					scrollContent(delta, true);

					// stop window scroll
					if (e.preventDefault && !releaseScroll) { e.preventDefault(); }
					if (!releaseScroll) { e.returnValue = false; }
				}

				function scrollContent(y, isWheel, isJump) {
					var delta = y;

					if (isWheel) {
						// move bar with mouse wheel
						delta = parseInt(bar.css('top')) + y * wheelStep / 100 * bar.outerHeight();

						// move bar, make sure it doesn't go out
						var maxTop = me.outerHeight() - bar.outerHeight();
						delta = Math.min(Math.max(delta, 0), maxTop);

						// scroll the scrollbar
						bar.css({ top: delta + 'px' });
					}

					// calculate actual scroll amount
					percentScroll = parseInt(bar.css('top')) / (me.outerHeight() - bar.outerHeight());
					delta = percentScroll * (me[0].scrollHeight - me.outerHeight());

					if (isJump) {
						delta = y;
						var offsetTop = delta / me[0].scrollHeight * me.outerHeight();
						bar.css({ top: offsetTop + 'px' });
					}

					// scroll content
					me.scrollTop(delta);

					// ensure bar is visible
					showBar();

					// trigger hide when scroll is stopped
					hideBar();
				}

				var attachWheel = function () {
					if (window.addEventListener) {
						this.addEventListener('DOMMouseScroll', _onWheel, false);
						this.addEventListener('mousewheel', _onWheel, false);
					}
					else {
						document.attachEvent("onmousewheel", _onWheel)
					}
				}

				// attach scroll events
				attachWheel();

				function getBarHeight() {

					barHeight = isOk(o.barHeight) ?
						o.barHeight :
						// calculate scrollbar height and make sure it is not too small
						Math.max((me.outerHeight() / me[0].scrollHeight) * me.outerHeight(), minBarHeight);
					bar.css({ height: barHeight + 'px' });
				}

				// set up initial height
				getBarHeight();

				function showBar() {
					// recalculate bar height
					getBarHeight();
					clearTimeout(queueHide);

					// when bar reached top or bottom
					if (percentScroll == ~~percentScroll) {
						//release wheel 
						releaseScroll = allowPageScroll;

						// publish approporiate event
						if (lastScroll != percentScroll) {
							var msg = (~~percentScroll == 0) ? 'top' : 'bottom';
							me.trigger('slimscroll', msg);
						}
					}
					lastScroll = percentScroll;

					// show only when required
					if (barHeight >= me.outerHeight()) {
						//allow window scroll
						releaseScroll = true;
						return;
					}
					bar.stop(true, true).fadeIn('fast');
					if (railVisible) { rail.stop(true, true).fadeIn('fast'); }
				}

				function hideBar() {
					// only hide when options allow it
					if (!alwaysVisible) {
						queueHide = setTimeout(function () {
							if (!(disableFadeOut && isOverPanel) && !isOverBar && !isDragg) {
								bar.fadeOut('slow');
								rail.fadeOut('slow');
							}
						}, 1000);
					}
				}

				// check start position
				if (start == 'bottom') {
					// scroll content to bottom
					bar.css({ top: me.outerHeight() - bar.outerHeight() });
					scrollContent(0, true);
				}
				else if (typeof start == 'object') {
					// scroll content
					scrollContent($(start).position().top, null, true);

					// make sure bar stays hidden
					if (!alwaysVisible) { bar.hide(); }
				}
			});

			// maintain chainability
			return this;
		}
	});

	jQuery.fn.extend({
		lwScroll: jQuery.fn.lwScroll
	});

})(jQuery);


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
			top: el.css("top")
		});
		if (!options.container) {
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

(function ($) {
	$.fn.drags = function (opt) {
		opt = $.extend({ handle: "", cursor: "move" }, opt);
		if (opt.handle === "") {
			var $el = this;
		} else {
			var $el = this.find(opt.handle);
		}
		return $el.css('cursor', opt.cursor).on("mousedown", function (e) {
			if (opt.handle === "") {
				var $drag = $(this).addClass('draggable');
			} else {
				var $drag = $(this).addClass('active-handle').parent().addClass('draggable');
			}
			var z_idx = $drag.css('z-index'),
                drg_h = $drag.outerHeight(),
                drg_w = $drag.outerWidth(),
                pos_y = $drag.offset().top + drg_h - e.pageY,
                pos_x = $drag.offset().left + drg_w - e.pageX;
			$drag.css('z-index', lw.utils.getTopZIndex()).parents().on("mousemove", function (e) {
				$('.draggable').offset({
					top: e.pageY + pos_y - drg_h,
					left: e.pageX + pos_x - drg_w
				}).on("mouseup", function () {
					$(this).removeClass('draggable').css('z-index', z_idx);
				});
			});
			e.preventDefault(); // disable selection
		}).on("mouseup", function () {
			if (opt.handle === "") {
				$(this).removeClass('draggable');
			} else {
				$(this).removeClass('active-handle').parent().removeClass('draggable');
			}
		});
	}
})(jQuery);