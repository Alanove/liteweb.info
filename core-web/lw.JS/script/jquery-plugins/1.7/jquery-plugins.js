/*! jquery plygins 1.7 */

// t: current time, b: begInnIng value, c: change In value, d: duration
jQuery.easing['jswing'] = jQuery.easing['swing'];

jQuery.extend(jQuery.easing,
{
	def: 'easeOutQuad',
	swing: function (x, t, b, c, d) {
		//alert(jQuery.easing.default);
		return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
	},
	easeInQuad: function (x, t, b, c, d) {
		return c * (t /= d) * t + b;
	},
	easeOutQuad: function (x, t, b, c, d) {
		return -c * (t /= d) * (t - 2) + b;
	},
	easeInOutQuad: function (x, t, b, c, d) {
		if ((t /= d / 2) < 1) return c / 2 * t * t + b;
		return -c / 2 * ((--t) * (t - 2) - 1) + b;
	},
	easeInCubic: function (x, t, b, c, d) {
		return c * (t /= d) * t * t + b;
	},
	easeOutCubic: function (x, t, b, c, d) {
		return c * ((t = t / d - 1) * t * t + 1) + b;
	},
	easeInOutCubic: function (x, t, b, c, d) {
		if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
		return c / 2 * ((t -= 2) * t * t + 2) + b;
	},
	easeInQuart: function (x, t, b, c, d) {
		return c * (t /= d) * t * t * t + b;
	},
	easeOutQuart: function (x, t, b, c, d) {
		return -c * ((t = t / d - 1) * t * t * t - 1) + b;
	},
	easeInOutQuart: function (x, t, b, c, d) {
		if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
		return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
	},
	easeInQuint: function (x, t, b, c, d) {
		return c * (t /= d) * t * t * t * t + b;
	},
	easeOutQuint: function (x, t, b, c, d) {
		return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
	},
	easeInOutQuint: function (x, t, b, c, d) {
		if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
		return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
	},
	easeInSine: function (x, t, b, c, d) {
		return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
	},
	easeOutSine: function (x, t, b, c, d) {
		return c * Math.sin(t / d * (Math.PI / 2)) + b;
	},
	easeInOutSine: function (x, t, b, c, d) {
		return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
	},
	easeInExpo: function (x, t, b, c, d) {
		return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
	},
	easeOutExpo: function (x, t, b, c, d) {
		return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
	},
	easeInOutExpo: function (x, t, b, c, d) {
		if (t == 0) return b;
		if (t == d) return b + c;
		if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
		return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
	},
	easeInCirc: function (x, t, b, c, d) {
		return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
	},
	easeOutCirc: function (x, t, b, c, d) {
		return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
	},
	easeInOutCirc: function (x, t, b, c, d) {
		if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
		return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
	},
	easeInElastic: function (x, t, b, c, d) {
		var s = 1.70158; var p = 0; var a = c;
		if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
		if (a < Math.abs(c)) { a = c; var s = p / 4; }
		else var s = p / (2 * Math.PI) * Math.asin(c / a);
		return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
	},
	easeOutElastic: function (x, t, b, c, d) {
		var s = 1.70158; var p = 0; var a = c;
		if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
		if (a < Math.abs(c)) { a = c; var s = p / 4; }
		else var s = p / (2 * Math.PI) * Math.asin(c / a);
		return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
	},
	easeInOutElastic: function (x, t, b, c, d) {
		var s = 1.70158; var p = 0; var a = c;
		if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
		if (a < Math.abs(c)) { a = c; var s = p / 4; }
		else var s = p / (2 * Math.PI) * Math.asin(c / a);
		if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
		return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
	},
	easeInBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158;
		return c * (t /= d) * t * ((s + 1) * t - s) + b;
	},
	easeOutBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158;
		return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
	},
	easeInOutBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158;
		if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
		return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
	},
	easeInBounce: function (x, t, b, c, d) {
		return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
	},
	easeOutBounce: function (x, t, b, c, d) {
		if ((t /= d) < (1 / 2.75)) {
			return c * (7.5625 * t * t) + b;
		} else if (t < (2 / 2.75)) {
			return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
		} else if (t < (2.5 / 2.75)) {
			return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
		} else {
			return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
		}
	},
	easeInOutBounce: function (x, t, b, c, d) {
		if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
		return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
	}
});

/*! Copyright (c) 2013 Brandon Aaron (http://brandon.aaron.sh)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 3.1.11
 *
 * Requires: jQuery 1.2.2+
 */

(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define(['jquery'], factory);
	} else if (typeof exports === 'object') {
		// Node/CommonJS style for Browserify
		module.exports = factory;
	} else {
		// Browser globals
		factory(jQuery);
	}
}(function ($) {

	var toFix = ['wheel', 'mousewheel', 'DOMMouseScroll', 'MozMousePixelScroll'],
		toBind = ('onwheel' in document || document.documentMode >= 9) ?
					['wheel'] : ['mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'],
		slice = Array.prototype.slice,
		nullLowestDeltaTimeout, lowestDelta;

	if ($.event.fixHooks) {
		for (var i = toFix.length; i;) {
			$.event.fixHooks[toFix[--i]] = $.event.mouseHooks;
		}
	}

	var special = $.event.special.mousewheel = {
		version: '3.1.11',

		setup: function () {
			if (this.addEventListener) {
				for (var i = toBind.length; i;) {
					this.addEventListener(toBind[--i], handler, false);
				}
			} else {
				this.onmousewheel = handler;
			}
			// Store the line height and page height for this particular element
			$.data(this, 'mousewheel-line-height', special.getLineHeight(this));
			$.data(this, 'mousewheel-page-height', special.getPageHeight(this));
		},

		teardown: function () {
			if (this.removeEventListener) {
				for (var i = toBind.length; i;) {
					this.removeEventListener(toBind[--i], handler, false);
				}
			} else {
				this.onmousewheel = null;
			}
			// Clean up the data we added to the element
			$.removeData(this, 'mousewheel-line-height');
			$.removeData(this, 'mousewheel-page-height');
		},

		getLineHeight: function (elem) {
			var $parent = $(elem)['offsetParent' in $.fn ? 'offsetParent' : 'parent']();
			if (!$parent.length) {
				$parent = $('body');
			}
			return parseInt($parent.css('fontSize'), 10);
		},

		getPageHeight: function (elem) {
			return $(elem).height();
		},

		settings: {
			adjustOldDeltas: true, // see shouldAdjustOldDeltas() below
			normalizeOffset: true  // calls getBoundingClientRect for each event
		}
	};

	$.fn.extend({
		mousewheel: function (fn) {
			return fn ? this.bind('mousewheel', fn) : this.trigger('mousewheel');
		},

		unmousewheel: function (fn) {
			return this.unbind('mousewheel', fn);
		}
	});


	function handler(event) {
		var orgEvent = event || window.event,
			args = slice.call(arguments, 1),
			delta = 0,
			deltaX = 0,
			deltaY = 0,
			absDelta = 0,
			offsetX = 0,
			offsetY = 0;
		event = $.event.fix(orgEvent);
		event.type = 'mousewheel';

		// Old school scrollwheel delta
		if ('detail' in orgEvent) { deltaY = orgEvent.detail * -1; }
		if ('wheelDelta' in orgEvent) { deltaY = orgEvent.wheelDelta; }
		if ('wheelDeltaY' in orgEvent) { deltaY = orgEvent.wheelDeltaY; }
		if ('wheelDeltaX' in orgEvent) { deltaX = orgEvent.wheelDeltaX * -1; }

		// Firefox < 17 horizontal scrolling related to DOMMouseScroll event
		if ('axis' in orgEvent && orgEvent.axis === orgEvent.HORIZONTAL_AXIS) {
			deltaX = deltaY * -1;
			deltaY = 0;
		}

		// Set delta to be deltaY or deltaX if deltaY is 0 for backwards compatabilitiy
		delta = deltaY === 0 ? deltaX : deltaY;

		// New school wheel delta (wheel event)
		if ('deltaY' in orgEvent) {
			deltaY = orgEvent.deltaY * -1;
			delta = deltaY;
		}
		if ('deltaX' in orgEvent) {
			deltaX = orgEvent.deltaX;
			if (deltaY === 0) { delta = deltaX * -1; }
		}

		// No change actually happened, no reason to go any further
		if (deltaY === 0 && deltaX === 0) { return; }

		// Need to convert lines and pages to pixels if we aren't already in pixels
		// There are three delta modes:
		//   * deltaMode 0 is by pixels, nothing to do
		//   * deltaMode 1 is by lines
		//   * deltaMode 2 is by pages
		if (orgEvent.deltaMode === 1) {
			var lineHeight = $.data(this, 'mousewheel-line-height');
			delta *= lineHeight;
			deltaY *= lineHeight;
			deltaX *= lineHeight;
		} else if (orgEvent.deltaMode === 2) {
			var pageHeight = $.data(this, 'mousewheel-page-height');
			delta *= pageHeight;
			deltaY *= pageHeight;
			deltaX *= pageHeight;
		}

		// Store lowest absolute delta to normalize the delta values
		absDelta = Math.max(Math.abs(deltaY), Math.abs(deltaX));

		if (!lowestDelta || absDelta < lowestDelta) {
			lowestDelta = absDelta;

			// Adjust older deltas if necessary
			if (shouldAdjustOldDeltas(orgEvent, absDelta)) {
				lowestDelta /= 40;
			}
		}

		// Adjust older deltas if necessary
		if (shouldAdjustOldDeltas(orgEvent, absDelta)) {
			// Divide all the things by 40!
			delta /= 40;
			deltaX /= 40;
			deltaY /= 40;
		}

		// Get a whole, normalized value for the deltas
		delta = Math[delta >= 1 ? 'floor' : 'ceil'](delta / lowestDelta);
		deltaX = Math[deltaX >= 1 ? 'floor' : 'ceil'](deltaX / lowestDelta);
		deltaY = Math[deltaY >= 1 ? 'floor' : 'ceil'](deltaY / lowestDelta);

		// Normalise offsetX and offsetY properties
		if (special.settings.normalizeOffset && this.getBoundingClientRect) {
			var boundingRect = this.getBoundingClientRect();
			offsetX = event.clientX - boundingRect.left;
			offsetY = event.clientY - boundingRect.top;
		}

		// Add information to the event object
		event.deltaX = deltaX;
		event.deltaY = deltaY;
		event.deltaFactor = lowestDelta;
		event.offsetX = offsetX;
		event.offsetY = offsetY;
		// Go ahead and set deltaMode to 0 since we converted to pixels
		// Although this is a little odd since we overwrite the deltaX/Y
		// properties with normalized deltas.
		event.deltaMode = 0;

		// Add event and delta to the front of the arguments
		args.unshift(event, delta, deltaX, deltaY);

		// Clearout lowestDelta after sometime to better
		// handle multiple device types that give different
		// a different lowestDelta
		// Ex: trackpad = 3 and mouse wheel = 120
		if (nullLowestDeltaTimeout) { clearTimeout(nullLowestDeltaTimeout); }
		nullLowestDeltaTimeout = setTimeout(nullLowestDelta, 200);

		return ($.event.dispatch || $.event.handle).apply(this, args);
	}

	function nullLowestDelta() {
		lowestDelta = null;
	}

	function shouldAdjustOldDeltas(orgEvent, absDelta) {
		// If this is an older event and the delta is divisable by 120,
		// then we are assuming that the browser is treating this as an
		// older mouse wheel event and that we should divide the deltas
		// by 40 to try and get a more usable deltaFactor.
		// Side note, this actually impacts the reported scroll distance
		// in older browsers and can cause scrolling to be slower than native.
		// Turn this off by setting $.event.special.mousewheel.settings.adjustOldDeltas to false.
		return special.settings.adjustOldDeltas && orgEvent.type === 'mousewheel' && absDelta % 120 === 0;
	}

}));



/*!
 * jQuery Form Plugin
 * version: 3.50.0-2014.02.05
 * Requires jQuery v1.5 or later
 * Copyright (c) 2013 M. Alsup
 * Examples and documentation at: http://malsup.com/jquery/form/
 * Project repository: https://github.com/malsup/form
 * Dual licensed under the MIT and GPL licenses.
 * https://github.com/malsup/form#copyright-and-license
 */
/*global ActiveXObject */

// AMD support
(function (factory) {
	"use strict";
	if (typeof define === 'function' && define.amd) {
		// using AMD; register as anon module
		define(['jquery'], factory);
	} else {
		// no AMD; invoke directly
		factory((typeof (jQuery) != 'undefined') ? jQuery : window.Zepto);
	}
}

(function ($) {
	"use strict";

	/*
		Usage Note:
		-----------
		Do not use both ajaxSubmit and ajaxForm on the same form.  These
		functions are mutually exclusive.  Use ajaxSubmit if you want
		to bind your own submit handler to the form.  For example,
	
		$(document).ready(function() {
			$('#myForm').on('submit', function(e) {
				e.preventDefault(); // <-- important
				$(this).ajaxSubmit({
					target: '#output'
				});
			});
		});
	
		Use ajaxForm when you want the plugin to manage all the event binding
		for you.  For example,
	
		$(document).ready(function() {
			$('#myForm').ajaxForm({
				target: '#output'
			});
		});
	
		You can also use ajaxForm with delegation (requires jQuery v1.7+), so the
		form does not have to exist when you invoke ajaxForm:
	
		$('#myForm').ajaxForm({
			delegation: true,
			target: '#output'
		});
	
		When using ajaxForm, the ajaxSubmit function will be invoked for you
		at the appropriate time.
	*/

	/**
	 * Feature detection
	 */
	var feature = {};
	feature.fileapi = $("<input type='file'/>").get(0).files !== undefined;
	feature.formdata = window.FormData !== undefined;

	var hasProp = !!$.fn.prop;

	// attr2 uses prop when it can but checks the return type for
	// an expected string.  this accounts for the case where a form 
	// contains inputs with names like "action" or "method"; in those
	// cases "prop" returns the element
	$.fn.attr2 = function () {
		if (!hasProp) {
			return this.attr.apply(this, arguments);
		}
		var val = this.prop.apply(this, arguments);
		if ((val && val.jquery) || typeof val === 'string') {
			return val;
		}
		return this.attr.apply(this, arguments);
	};

	/**
	 * ajaxSubmit() provides a mechanism for immediately submitting
	 * an HTML form using AJAX.
	 */
	$.fn.ajaxSubmit = function (options) {
		/*jshint scripturl:true */

		// fast fail if nothing selected (http://dev.jquery.com/ticket/2752)
		if (!this.length) {
			log('ajaxSubmit: skipping submit process - no element selected');
			return this;
		}

		var method, action, url, $form = this;

		if (typeof options == 'function') {
			options = { success: options };
		}
		else if (options === undefined) {
			options = {};
		}

		method = options.type || this.attr2('method');
		action = options.url || this.attr2('action');

		url = (typeof action === 'string') ? $.trim(action) : '';
		url = url || window.location.href || '';
		if (url) {
			// clean url (don't include hash vaue)
			url = (url.match(/^([^#]+)/) || [])[1];
		}

		options = $.extend(true, {
			url: url,
			success: $.ajaxSettings.success,
			type: method || $.ajaxSettings.type,
			iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank'
		}, options);

		// hook for manipulating the form data before it is extracted;
		// convenient for use with rich editors like tinyMCE or FCKEditor
		var veto = {};
		this.trigger('form-pre-serialize', [this, options, veto]);
		if (veto.veto) {
			log('ajaxSubmit: submit vetoed via form-pre-serialize trigger');
			return this;
		}

		// provide opportunity to alter form data before it is serialized
		if (options.beforeSerialize && options.beforeSerialize(this, options) === false) {
			log('ajaxSubmit: submit aborted via beforeSerialize callback');
			return this;
		}

		var traditional = options.traditional;
		if (traditional === undefined) {
			traditional = $.ajaxSettings.traditional;
		}

		var elements = [];
		var qx, a = this.formToArray(options.semantic, elements);
		if (options.data) {
			options.extraData = options.data;
			qx = $.param(options.data, traditional);
		}

		// give pre-submit callback an opportunity to abort the submit
		if (options.beforeSubmit && options.beforeSubmit(a, this, options) === false) {
			log('ajaxSubmit: submit aborted via beforeSubmit callback');
			return this;
		}

		// fire vetoable 'validate' event
		this.trigger('form-submit-validate', [a, this, options, veto]);
		if (veto.veto) {
			log('ajaxSubmit: submit vetoed via form-submit-validate trigger');
			return this;
		}

		var q = $.param(a, traditional);
		if (qx) {
			q = (q ? (q + '&' + qx) : qx);
		}
		if (options.type.toUpperCase() == 'GET') {
			options.url += (options.url.indexOf('?') >= 0 ? '&' : '?') + q;
			options.data = null;  // data is null for 'get'
		}
		else {
			options.data = q; // data is the query string for 'post'
		}

		var callbacks = [];
		if (options.resetForm) {
			callbacks.push(function () { $form.resetForm(); });
		}
		if (options.clearForm) {
			callbacks.push(function () { $form.clearForm(options.includeHidden); });
		}

		// perform a load on the target only if dataType is not provided
		if (!options.dataType && options.target) {
			var oldSuccess = options.success || function () { };
			callbacks.push(function (data) {
				var fn = options.replaceTarget ? 'replaceWith' : 'html';
				$(options.target)[fn](data).each(oldSuccess, arguments);
			});
		}
		else if (options.success) {
			callbacks.push(options.success);
		}

		options.success = function (data, status, xhr) { // jQuery 1.4+ passes xhr as 3rd arg
			var context = options.context || this;    // jQuery 1.4+ supports scope context
			for (var i = 0, max = callbacks.length; i < max; i++) {
				callbacks[i].apply(context, [data, status, xhr || $form, $form]);
			}
		};

		if (options.error) {
			var oldError = options.error;
			options.error = function (xhr, status, error) {
				var context = options.context || this;
				oldError.apply(context, [xhr, status, error, $form]);
			};
		}

		if (options.complete) {
			var oldComplete = options.complete;
			options.complete = function (xhr, status) {
				var context = options.context || this;
				oldComplete.apply(context, [xhr, status, $form]);
			};
		}

		// are there files to upload?

		// [value] (issue #113), also see comment:
		// https://github.com/malsup/form/commit/588306aedba1de01388032d5f42a60159eea9228#commitcomment-2180219
		var fileInputs = $('input[type=file]:enabled', this).filter(function () { return $(this).val() !== ''; });

		var hasFileInputs = fileInputs.length > 0;
		var mp = 'multipart/form-data';
		var multipart = ($form.attr('enctype') == mp || $form.attr('encoding') == mp);

		var fileAPI = feature.fileapi && feature.formdata;
		log("fileAPI :" + fileAPI);
		var shouldUseFrame = (hasFileInputs || multipart) && !fileAPI;

		var jqxhr;

		// options.iframe allows user to force iframe mode
		// 06-NOV-09: now defaulting to iframe mode if file input is detected
		if (options.iframe !== false && (options.iframe || shouldUseFrame)) {
			// hack to fix Safari hang (thanks to Tim Molendijk for this)
			// see:  http://groups.google.com/group/jquery-dev/browse_thread/thread/36395b7ab510dd5d
			if (options.closeKeepAlive) {
				$.get(options.closeKeepAlive, function () {
					jqxhr = fileUploadIframe(a);
				});
			}
			else {
				jqxhr = fileUploadIframe(a);
			}
		}
		else if ((hasFileInputs || multipart) && fileAPI) {
			jqxhr = fileUploadXhr(a);
		}
		else {
			jqxhr = $.ajax(options);
		}

		$form.removeData('jqxhr').data('jqxhr', jqxhr);

		// clear element array
		for (var k = 0; k < elements.length; k++) {
			elements[k] = null;
		}

		// fire 'notify' event
		this.trigger('form-submit-notify', [this, options]);
		return this;

		// utility fn for deep serialization
		function deepSerialize(extraData) {
			var serialized = $.param(extraData, options.traditional).split('&');
			var len = serialized.length;
			var result = [];
			var i, part;
			for (i = 0; i < len; i++) {
				// #252; undo param space replacement
				serialized[i] = serialized[i].replace(/\+/g, ' ');
				part = serialized[i].split('=');
				// #278; use array instead of object storage, favoring array serializations
				result.push([decodeURIComponent(part[0]), decodeURIComponent(part[1])]);
			}
			return result;
		}

		// XMLHttpRequest Level 2 file uploads (big hat tip to francois2metz)
		function fileUploadXhr(a) {
			var formdata = new FormData();

			for (var i = 0; i < a.length; i++) {
				formdata.append(a[i].name, a[i].value);
			}

			if (options.extraData) {
				var serializedData = deepSerialize(options.extraData);
				for (i = 0; i < serializedData.length; i++) {
					if (serializedData[i]) {
						formdata.append(serializedData[i][0], serializedData[i][1]);
					}
				}
			}

			options.data = null;

			var s = $.extend(true, {}, $.ajaxSettings, options, {
				contentType: false,
				processData: false,
				cache: false,
				type: method || 'POST'
			});

			if (options.uploadProgress) {
				// workaround because jqXHR does not expose upload property
				s.xhr = function () {
					var xhr = $.ajaxSettings.xhr();
					if (xhr.upload) {
						xhr.upload.addEventListener('progress', function (event) {
							var percent = 0;
							var position = event.loaded || event.position; /*event.position is deprecated*/
							var total = event.total;
							if (event.lengthComputable) {
								percent = Math.ceil(position / total * 100);
							}
							options.uploadProgress(event, position, total, percent);
						}, false);
					}
					return xhr;
				};
			}

			s.data = null;
			var beforeSend = s.beforeSend;
			s.beforeSend = function (xhr, o) {
				//Send FormData() provided by user
				if (options.formData) {
					o.data = options.formData;
				}
				else {
					o.data = formdata;
				}
				if (beforeSend) {
					beforeSend.call(this, xhr, o);
				}
			};
			return $.ajax(s);
		}

		// private function for handling file uploads (hat tip to YAHOO!)
		function fileUploadIframe(a) {
			var form = $form[0], el, i, s, g, id, $io, io, xhr, sub, n, timedOut, timeoutHandle;
			var deferred = $.Deferred();

			// #341
			deferred.abort = function (status) {
				xhr.abort(status);
			};

			if (a) {
				// ensure that every serialized input is still enabled
				for (i = 0; i < elements.length; i++) {
					el = $(elements[i]);
					if (hasProp) {
						el.prop('disabled', false);
					}
					else {
						el.removeAttr('disabled');
					}
				}
			}

			s = $.extend(true, {}, $.ajaxSettings, options);
			s.context = s.context || s;
			id = 'jqFormIO' + (new Date().getTime());
			if (s.iframeTarget) {
				$io = $(s.iframeTarget);
				n = $io.attr2('name');
				if (!n) {
					$io.attr2('name', id);
				}
				else {
					id = n;
				}
			}
			else {
				$io = $('<iframe name="' + id + '" src="' + s.iframeSrc + '" />');
				$io.css({ position: 'absolute', top: '-1000px', left: '-1000px' });
			}
			io = $io[0];


			xhr = { // mock object
				aborted: 0,
				responseText: null,
				responseXML: null,
				status: 0,
				statusText: 'n/a',
				getAllResponseHeaders: function () { },
				getResponseHeader: function () { },
				setRequestHeader: function () { },
				abort: function (status) {
					var e = (status === 'timeout' ? 'timeout' : 'aborted');
					log('aborting upload... ' + e);
					this.aborted = 1;

					try { // #214, #257
						if (io.contentWindow.document.execCommand) {
							io.contentWindow.document.execCommand('Stop');
						}
					}
					catch (ignore) { }

					$io.attr('src', s.iframeSrc); // abort op in progress
					xhr.error = e;
					if (s.error) {
						s.error.call(s.context, xhr, e, status);
					}
					if (g) {
						$.event.trigger("ajaxError", [xhr, s, e]);
					}
					if (s.complete) {
						s.complete.call(s.context, xhr, e);
					}
				}
			};

			g = s.global;
			// trigger ajax global events so that activity/block indicators work like normal
			if (g && 0 === $.active++) {
				$.event.trigger("ajaxStart");
			}
			if (g) {
				$.event.trigger("ajaxSend", [xhr, s]);
			}

			if (s.beforeSend && s.beforeSend.call(s.context, xhr, s) === false) {
				if (s.global) {
					$.active--;
				}
				deferred.reject();
				return deferred;
			}
			if (xhr.aborted) {
				deferred.reject();
				return deferred;
			}

			// add submitting element to data if we know it
			sub = form.clk;
			if (sub) {
				n = sub.name;
				if (n && !sub.disabled) {
					s.extraData = s.extraData || {};
					s.extraData[n] = sub.value;
					if (sub.type == "image") {
						s.extraData[n + '.x'] = form.clk_x;
						s.extraData[n + '.y'] = form.clk_y;
					}
				}
			}

			var CLIENT_TIMEOUT_ABORT = 1;
			var SERVER_ABORT = 2;

			function getDoc(frame) {
				/* it looks like contentWindow or contentDocument do not
				 * carry the protocol property in ie8, when running under ssl
				 * frame.document is the only valid response document, since
				 * the protocol is know but not on the other two objects. strange?
				 * "Same origin policy" http://en.wikipedia.org/wiki/Same_origin_policy
				 */

				var doc = null;

				// IE8 cascading access check
				try {
					if (frame.contentWindow) {
						doc = frame.contentWindow.document;
					}
				} catch (err) {
					// IE8 access denied under ssl & missing protocol
					log('cannot get iframe.contentWindow document: ' + err);
				}

				if (doc) { // successful getting content
					return doc;
				}

				try { // simply checking may throw in ie8 under ssl or mismatched protocol
					doc = frame.contentDocument ? frame.contentDocument : frame.document;
				} catch (err) {
					// last attempt
					log('cannot get iframe.contentDocument: ' + err);
					doc = frame.document;
				}
				return doc;
			}

			// Rails CSRF hack (thanks to Yvan Barthelemy)
			var csrf_token = $('meta[name=csrf-token]').attr('content');
			var csrf_param = $('meta[name=csrf-param]').attr('content');
			if (csrf_param && csrf_token) {
				s.extraData = s.extraData || {};
				s.extraData[csrf_param] = csrf_token;
			}

			// take a breath so that pending repaints get some cpu time before the upload starts
			function doSubmit() {
				// make sure form attrs are set
				var t = $form.attr2('target'),
					a = $form.attr2('action'),
					mp = 'multipart/form-data',
					et = $form.attr('enctype') || $form.attr('encoding') || mp;

				// update form attrs in IE friendly way
				form.setAttribute('target', id);
				if (!method || /post/i.test(method)) {
					form.setAttribute('method', 'POST');
				}
				if (a != s.url) {
					form.setAttribute('action', s.url);
				}

				// ie borks in some cases when setting encoding
				if (!s.skipEncodingOverride && (!method || /post/i.test(method))) {
					$form.attr({
						encoding: 'multipart/form-data',
						enctype: 'multipart/form-data'
					});
				}

				// support timout
				if (s.timeout) {
					timeoutHandle = setTimeout(function () { timedOut = true; cb(CLIENT_TIMEOUT_ABORT); }, s.timeout);
				}

				// look for server aborts
				function checkState() {
					try {
						var state = getDoc(io).readyState;
						log('state = ' + state);
						if (state && state.toLowerCase() == 'uninitialized') {
							setTimeout(checkState, 50);
						}
					}
					catch (e) {
						log('Server abort: ', e, ' (', e.name, ')');
						cb(SERVER_ABORT);
						if (timeoutHandle) {
							clearTimeout(timeoutHandle);
						}
						timeoutHandle = undefined;
					}
				}

				// add "extra" data to form if provided in options
				var extraInputs = [];
				try {
					if (s.extraData) {
						for (var n in s.extraData) {
							if (s.extraData.hasOwnProperty(n)) {
								// if using the $.param format that allows for multiple values with the same name
								if ($.isPlainObject(s.extraData[n]) && s.extraData[n].hasOwnProperty('name') && s.extraData[n].hasOwnProperty('value')) {
									extraInputs.push(
									$('<input type="hidden" name="' + s.extraData[n].name + '">').val(s.extraData[n].value)
										.appendTo(form)[0]);
								} else {
									extraInputs.push(
									$('<input type="hidden" name="' + n + '">').val(s.extraData[n])
										.appendTo(form)[0]);
								}
							}
						}
					}

					if (!s.iframeTarget) {
						// add iframe to doc and submit the form
						$io.appendTo('body');
					}
					if (io.attachEvent) {
						io.attachEvent('onload', cb);
					}
					else {
						io.addEventListener('load', cb, false);
					}
					setTimeout(checkState, 15);

					try {
						form.submit();
					} catch (err) {
						// just in case form has element with name/id of 'submit'
						var submitFn = document.createElement('form').submit;
						submitFn.apply(form);
					}
				}
				finally {
					// reset attrs and remove "extra" input elements
					form.setAttribute('action', a);
					form.setAttribute('enctype', et); // #380
					if (t) {
						form.setAttribute('target', t);
					} else {
						$form.removeAttr('target');
					}
					$(extraInputs).remove();
				}
			}

			if (s.forceSync) {
				doSubmit();
			}
			else {
				setTimeout(doSubmit, 10); // this lets dom updates render
			}

			var data, doc, domCheckCount = 50, callbackProcessed;

			function cb(e) {
				if (xhr.aborted || callbackProcessed) {
					return;
				}

				doc = getDoc(io);
				if (!doc) {
					log('cannot access response document');
					e = SERVER_ABORT;
				}
				if (e === CLIENT_TIMEOUT_ABORT && xhr) {
					xhr.abort('timeout');
					deferred.reject(xhr, 'timeout');
					return;
				}
				else if (e == SERVER_ABORT && xhr) {
					xhr.abort('server abort');
					deferred.reject(xhr, 'error', 'server abort');
					return;
				}

				if (!doc || doc.location.href == s.iframeSrc) {
					// response not received yet
					if (!timedOut) {
						return;
					}
				}
				if (io.detachEvent) {
					io.detachEvent('onload', cb);
				}
				else {
					io.removeEventListener('load', cb, false);
				}

				var status = 'success', errMsg;
				try {
					if (timedOut) {
						throw 'timeout';
					}

					var isXml = s.dataType == 'xml' || doc.XMLDocument || $.isXMLDoc(doc);
					log('isXml=' + isXml);
					if (!isXml && window.opera && (doc.body === null || !doc.body.innerHTML)) {
						if (--domCheckCount) {
							// in some browsers (Opera) the iframe DOM is not always traversable when
							// the onload callback fires, so we loop a bit to accommodate
							log('requeing onLoad callback, DOM not available');
							setTimeout(cb, 250);
							return;
						}
						// let this fall through because server response could be an empty document
						//log('Could not access iframe DOM after mutiple tries.');
						//throw 'DOMException: not available';
					}

					//log('response detected');
					var docRoot = doc.body ? doc.body : doc.documentElement;
					xhr.responseText = docRoot ? docRoot.innerHTML : null;
					xhr.responseXML = doc.XMLDocument ? doc.XMLDocument : doc;
					if (isXml) {
						s.dataType = 'xml';
					}
					xhr.getResponseHeader = function (header) {
						var headers = { 'content-type': s.dataType };
						return headers[header.toLowerCase()];
					};
					// support for XHR 'status' & 'statusText' emulation :
					if (docRoot) {
						xhr.status = Number(docRoot.getAttribute('status')) || xhr.status;
						xhr.statusText = docRoot.getAttribute('statusText') || xhr.statusText;
					}

					var dt = (s.dataType || '').toLowerCase();
					var scr = /(json|script|text)/.test(dt);
					if (scr || s.textarea) {
						// see if user embedded response in textarea
						var ta = doc.getElementsByTagName('textarea')[0];
						if (ta) {
							xhr.responseText = ta.value;
							// support for XHR 'status' & 'statusText' emulation :
							xhr.status = Number(ta.getAttribute('status')) || xhr.status;
							xhr.statusText = ta.getAttribute('statusText') || xhr.statusText;
						}
						else if (scr) {
							// account for browsers injecting pre around json response
							var pre = doc.getElementsByTagName('pre')[0];
							var b = doc.getElementsByTagName('body')[0];
							if (pre) {
								xhr.responseText = pre.textContent ? pre.textContent : pre.innerText;
							}
							else if (b) {
								xhr.responseText = b.textContent ? b.textContent : b.innerText;
							}
						}
					}
					else if (dt == 'xml' && !xhr.responseXML && xhr.responseText) {
						xhr.responseXML = toXml(xhr.responseText);
					}

					try {
						data = httpData(xhr, dt, s);
					}
					catch (err) {
						status = 'parsererror';
						xhr.error = errMsg = (err || status);
					}
				}
				catch (err) {
					log('error caught: ', err);
					status = 'error';
					xhr.error = errMsg = (err || status);
				}

				if (xhr.aborted) {
					log('upload aborted');
					status = null;
				}

				if (xhr.status) { // we've set xhr.status
					status = (xhr.status >= 200 && xhr.status < 300 || xhr.status === 304) ? 'success' : 'error';
				}

				// ordering of these callbacks/triggers is odd, but that's how $.ajax does it
				if (status === 'success') {
					if (s.success) {
						s.success.call(s.context, data, 'success', xhr);
					}
					deferred.resolve(xhr.responseText, 'success', xhr);
					if (g) {
						$.event.trigger("ajaxSuccess", [xhr, s]);
					}
				}
				else if (status) {
					if (errMsg === undefined) {
						errMsg = xhr.statusText;
					}
					if (s.error) {
						s.error.call(s.context, xhr, status, errMsg);
					}
					deferred.reject(xhr, 'error', errMsg);
					if (g) {
						$.event.trigger("ajaxError", [xhr, s, errMsg]);
					}
				}

				if (g) {
					$.event.trigger("ajaxComplete", [xhr, s]);
				}

				if (g && ! --$.active) {
					$.event.trigger("ajaxStop");
				}

				if (s.complete) {
					s.complete.call(s.context, xhr, status);
				}

				callbackProcessed = true;
				if (s.timeout) {
					clearTimeout(timeoutHandle);
				}

				// clean up
				setTimeout(function () {
					if (!s.iframeTarget) {
						$io.remove();
					}
					else { //adding else to clean up existing iframe response.
						$io.attr('src', s.iframeSrc);
					}
					xhr.responseXML = null;
				}, 100);
			}

			var toXml = $.parseXML || function (s, doc) { // use parseXML if available (jQuery 1.5+)
				if (window.ActiveXObject) {
					doc = new ActiveXObject('Microsoft.XMLDOM');
					doc.async = 'false';
					doc.loadXML(s);
				}
				else {
					doc = (new DOMParser()).parseFromString(s, 'text/xml');
				}
				return (doc && doc.documentElement && doc.documentElement.nodeName != 'parsererror') ? doc : null;
			};
			var parseJSON = $.parseJSON || function (s) {
				/*jslint evil:true */
				return window['eval']('(' + s + ')');
			};

			var httpData = function (xhr, type, s) { // mostly lifted from jq1.4.4

				var ct = xhr.getResponseHeader('content-type') || '',
					xml = type === 'xml' || !type && ct.indexOf('xml') >= 0,
					data = xml ? xhr.responseXML : xhr.responseText;

				if (xml && data.documentElement.nodeName === 'parsererror') {
					if ($.error) {
						$.error('parsererror');
					}
				}
				if (s && s.dataFilter) {
					data = s.dataFilter(data, type);
				}
				if (typeof data === 'string') {
					if (type === 'json' || !type && ct.indexOf('json') >= 0) {
						data = parseJSON(data);
					} else if (type === "script" || !type && ct.indexOf("javascript") >= 0) {
						$.globalEval(data);
					}
				}
				return data;
			};

			return deferred;
		}
	};

	/**
	 * ajaxForm() provides a mechanism for fully automating form submission.
	 *
	 * The advantages of using this method instead of ajaxSubmit() are:
	 *
	 * 1: This method will include coordinates for <input type="image" /> elements (if the element
	 *    is used to submit the form).
	 * 2. This method will include the submit element's name/value data (for the element that was
	 *    used to submit the form).
	 * 3. This method binds the submit() method to the form for you.
	 *
	 * The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
	 * passes the options argument along after properly binding events for submit elements and
	 * the form itself.
	 */
	$.fn.ajaxForm = function (options) {
		options = options || {};
		options.delegation = options.delegation && $.isFunction($.fn.on);

		// in jQuery 1.3+ we can fix mistakes with the ready state
		if (!options.delegation && this.length === 0) {
			var o = { s: this.selector, c: this.context };
			if (!$.isReady && o.s) {
				log('DOM not ready, queuing ajaxForm');
				$(function () {
					$(o.s, o.c).ajaxForm(options);
				});
				return this;
			}
			// is your DOM ready?  http://docs.jquery.com/Tutorials:Introducing_$(document).ready()
			log('terminating; zero elements found by selector' + ($.isReady ? '' : ' (DOM not ready)'));
			return this;
		}

		if (options.delegation) {
			$(document)
				.off('submit.form-plugin', this.selector, doAjaxSubmit)
				.off('click.form-plugin', this.selector, captureSubmittingElement)
				.on('submit.form-plugin', this.selector, options, doAjaxSubmit)
				.on('click.form-plugin', this.selector, options, captureSubmittingElement);
			return this;
		}

		return this.ajaxFormUnbind()
			.bind('submit.form-plugin', options, doAjaxSubmit)
			.bind('click.form-plugin', options, captureSubmittingElement);
	};

	// private event handlers
	function doAjaxSubmit(e) {
		/*jshint validthis:true */
		var options = e.data;
		if (!e.isDefaultPrevented()) { // if event has been canceled, don't proceed
			e.preventDefault();
			$(e.target).ajaxSubmit(options); // #365
		}
	}

	function captureSubmittingElement(e) {
		/*jshint validthis:true */
		var target = e.target;
		var $el = $(target);
		if (!($el.is("[type=submit],[type=image]"))) {
			// is this a child element of the submit el?  (ex: a span within a button)
			var t = $el.closest('[type=submit]');
			if (t.length === 0) {
				return;
			}
			target = t[0];
		}
		var form = this;
		form.clk = target;
		if (target.type == 'image') {
			if (e.offsetX !== undefined) {
				form.clk_x = e.offsetX;
				form.clk_y = e.offsetY;
			} else if (typeof $.fn.offset == 'function') {
				var offset = $el.offset();
				form.clk_x = e.pageX - offset.left;
				form.clk_y = e.pageY - offset.top;
			} else {
				form.clk_x = e.pageX - target.offsetLeft;
				form.clk_y = e.pageY - target.offsetTop;
			}
		}
		// clear form vars
		setTimeout(function () { form.clk = form.clk_x = form.clk_y = null; }, 100);
	}


	// ajaxFormUnbind unbinds the event handlers that were bound by ajaxForm
	$.fn.ajaxFormUnbind = function () {
		return this.unbind('submit.form-plugin click.form-plugin');
	};

	/**
	 * formToArray() gathers form element data into an array of objects that can
	 * be passed to any of the following ajax functions: $.get, $.post, or load.
	 * Each object in the array has both a 'name' and 'value' property.  An example of
	 * an array for a simple login form might be:
	 *
	 * [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
	 *
	 * It is this array that is passed to pre-submit callback functions provided to the
	 * ajaxSubmit() and ajaxForm() methods.
	 */
	$.fn.formToArray = function (semantic, elements) {
		var a = [];
		if (this.length === 0) {
			return a;
		}

		var form = this[0];
		var formId = this.attr('id');
		var els = semantic ? form.getElementsByTagName('*') : form.elements;
		var els2;

		if (els && !/MSIE [678]/.test(navigator.userAgent)) { // #390
			els = $(els).get();  // convert to standard array
		}

		// #386; account for inputs outside the form which use the 'form' attribute
		if (formId) {
			els2 = $(':input[form=' + formId + ']').get();
			if (els2.length) {
				els = (els || []).concat(els2);
			}
		}

		if (!els || !els.length) {
			return a;
		}

		var i, j, n, v, el, max, jmax;
		for (i = 0, max = els.length; i < max; i++) {
			el = els[i];
			n = el.name;
			if (!n || el.disabled) {
				continue;
			}

			if (semantic && form.clk && el.type == "image") {
				// handle image inputs on the fly when semantic == true
				if (form.clk == el) {
					a.push({ name: n, value: $(el).val(), type: el.type });
					a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
				}
				continue;
			}

			v = $.fieldValue(el, true);
			if (v && v.constructor == Array) {
				if (elements) {
					elements.push(el);
				}
				for (j = 0, jmax = v.length; j < jmax; j++) {
					a.push({ name: n, value: v[j] });
				}
			}
			else if (feature.fileapi && el.type == 'file') {
				if (elements) {
					elements.push(el);
				}
				var files = el.files;
				if (files.length) {
					for (j = 0; j < files.length; j++) {
						a.push({ name: n, value: files[j], type: el.type });
					}
				}
				else {
					// #180
					a.push({ name: n, value: '', type: el.type });
				}
			}
			else if (v !== null && typeof v != 'undefined') {
				if (elements) {
					elements.push(el);
				}
				a.push({ name: n, value: v, type: el.type, required: el.required });
			}
		}

		if (!semantic && form.clk) {
			// input type=='image' are not found in elements array! handle it here
			var $input = $(form.clk), input = $input[0];
			n = input.name;
			if (n && !input.disabled && input.type == 'image') {
				a.push({ name: n, value: $input.val() });
				a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
			}
		}
		return a;
	};

	/**
	 * Serializes form data into a 'submittable' string. This method will return a string
	 * in the format: name1=value1&amp;name2=value2
	 */
	$.fn.formSerialize = function (semantic) {
		//hand off to jQuery.param for proper encoding
		return $.param(this.formToArray(semantic));
	};

	/**
	 * Serializes all field elements in the jQuery object into a query string.
	 * This method will return a string in the format: name1=value1&amp;name2=value2
	 */
	$.fn.fieldSerialize = function (successful) {
		var a = [];
		this.each(function () {
			var n = this.name;
			if (!n) {
				return;
			}
			var v = $.fieldValue(this, successful);
			if (v && v.constructor == Array) {
				for (var i = 0, max = v.length; i < max; i++) {
					a.push({ name: n, value: v[i] });
				}
			}
			else if (v !== null && typeof v != 'undefined') {
				a.push({ name: this.name, value: v });
			}
		});
		//hand off to jQuery.param for proper encoding
		return $.param(a);
	};

	/**
	 * Returns the value(s) of the element in the matched set.  For example, consider the following form:
	 *
	 *  <form><fieldset>
	 *      <input name="A" type="text" />
	 *      <input name="A" type="text" />
	 *      <input name="B" type="checkbox" value="B1" />
	 *      <input name="B" type="checkbox" value="B2"/>
	 *      <input name="C" type="radio" value="C1" />
	 *      <input name="C" type="radio" value="C2" />
	 *  </fieldset></form>
	 *
	 *  var v = $('input[type=text]').fieldValue();
	 *  // if no values are entered into the text inputs
	 *  v == ['','']
	 *  // if values entered into the text inputs are 'foo' and 'bar'
	 *  v == ['foo','bar']
	 *
	 *  var v = $('input[type=checkbox]').fieldValue();
	 *  // if neither checkbox is checked
	 *  v === undefined
	 *  // if both checkboxes are checked
	 *  v == ['B1', 'B2']
	 *
	 *  var v = $('input[type=radio]').fieldValue();
	 *  // if neither radio is checked
	 *  v === undefined
	 *  // if first radio is checked
	 *  v == ['C1']
	 *
	 * The successful argument controls whether or not the field element must be 'successful'
	 * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
	 * The default value of the successful argument is true.  If this value is false the value(s)
	 * for each element is returned.
	 *
	 * Note: This method *always* returns an array.  If no valid value can be determined the
	 *    array will be empty, otherwise it will contain one or more values.
	 */
	$.fn.fieldValue = function (successful) {
		for (var val = [], i = 0, max = this.length; i < max; i++) {
			var el = this[i];
			var v = $.fieldValue(el, successful);
			if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length)) {
				continue;
			}
			if (v.constructor == Array) {
				$.merge(val, v);
			}
			else {
				val.push(v);
			}
		}
		return val;
	};

	/**
	 * Returns the value of the field element.
	 */
	$.fieldValue = function (el, successful) {
		var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
		if (successful === undefined) {
			successful = true;
		}

		if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
			(t == 'checkbox' || t == 'radio') && !el.checked ||
			(t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
			tag == 'select' && el.selectedIndex == -1)) {
			return null;
		}

		if (tag == 'select') {
			var index = el.selectedIndex;
			if (index < 0) {
				return null;
			}
			var a = [], ops = el.options;
			var one = (t == 'select-one');
			var max = (one ? index + 1 : ops.length);
			for (var i = (one ? index : 0) ; i < max; i++) {
				var op = ops[i];
				if (op.selected) {
					var v = op.value;
					if (!v) { // extra pain for IE...
						v = (op.attributes && op.attributes.value && !(op.attributes.value.specified)) ? op.text : op.value;
					}
					if (one) {
						return v;
					}
					a.push(v);
				}
			}
			return a;
		}
		return $(el).val();
	};

	/**
	 * Clears the form data.  Takes the following actions on the form's input fields:
	 *  - input text fields will have their 'value' property set to the empty string
	 *  - select elements will have their 'selectedIndex' property set to -1
	 *  - checkbox and radio inputs will have their 'checked' property set to false
	 *  - inputs of type submit, button, reset, and hidden will *not* be effected
	 *  - button elements will *not* be effected
	 */
	$.fn.clearForm = function (includeHidden) {
		return this.each(function () {
			$('input,select,textarea', this).clearFields(includeHidden);
		});
	};

	/**
	 * Clears the selected form elements.
	 */
	$.fn.clearFields = $.fn.clearInputs = function (includeHidden) {
		var re = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i; // 'hidden' is not in this list
		return this.each(function () {
			var t = this.type, tag = this.tagName.toLowerCase();
			if (re.test(t) || tag == 'textarea') {
				this.value = '';
			}
			else if (t == 'checkbox' || t == 'radio') {
				this.checked = false;
			}
			else if (tag == 'select') {
				this.selectedIndex = -1;
			}
			else if (t == "file") {
				if (/MSIE/.test(navigator.userAgent)) {
					$(this).replaceWith($(this).clone(true));
				} else {
					$(this).val('');
				}
			}
			else if (includeHidden) {
				// includeHidden can be the value true, or it can be a selector string
				// indicating a special test; for example:
				//  $('#myForm').clearForm('.special:hidden')
				// the above would clean hidden inputs that have the class of 'special'
				if ((includeHidden === true && /hidden/.test(t)) ||
					 (typeof includeHidden == 'string' && $(this).is(includeHidden))) {
					this.value = '';
				}
			}
		});
	};

	/**
	 * Resets the form data.  Causes all form elements to be reset to their original value.
	 */
	$.fn.resetForm = function () {
		return this.each(function () {
			// guard against an input with the name of 'reset'
			// note that IE reports the reset function as an 'object'
			if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType)) {
				this.reset();
			}
		});
	};

	/**
	 * Enables or disables any matching elements.
	 */
	$.fn.enable = function (b) {
		if (b === undefined) {
			b = true;
		}
		return this.each(function () {
			this.disabled = !b;
		});
	};

	/**
	 * Checks/unchecks any matching checkboxes or radio buttons and
	 * selects/deselects and matching option elements.
	 */
	$.fn.selected = function (select) {
		if (select === undefined) {
			select = true;
		}
		return this.each(function () {
			var t = this.type;
			if (t == 'checkbox' || t == 'radio') {
				this.checked = select;
			}
			else if (this.tagName.toLowerCase() == 'option') {
				var $sel = $(this).parent('select');
				if (select && $sel[0] && $sel[0].type == 'select-one') {
					// deselect all other options
					$sel.find('option').selected(false);
				}
				this.selected = select;
			}
		});
	};

	// expose debug var
	$.fn.ajaxSubmit.debug = false;

	// helper fn for console logging
	function log() {
		if (!$.fn.ajaxSubmit.debug) {
			return;
		}
		var msg = '[jquery.form] ' + Array.prototype.join.call(arguments, '');
		if (window.console && window.console.log) {
			window.console.log(msg);
		}
		else if (window.opera && window.opera.postError) {
			window.opera.postError(msg);
		}
	}

}));


/* http://keith-wood.name/timeEntry.html
   Time entry for jQuery v1.5.0.
   Written by Keith Wood (kbwood{at}iinet.com.au) June 2007.
   Licensed under the MIT (https://github.com/jquery/jquery/blob/master/MIT-LICENSE.txt) license.
   Please attribute the author if you use it. */
(function ($) { function TimeEntry() { this._disabledInputs = []; this.regional = []; this.regional[''] = { show24Hours: false, separator: ':', ampmPrefix: '', ampmNames: ['AM', 'PM'], spinnerTexts: ['Now', 'Previous field', 'Next field', 'Increment', 'Decrement'] }; this._defaults = { appendText: '', showSeconds: false, timeSteps: [1, 1, 1], initialField: 0, noSeparatorEntry: false, useMouseWheel: true, defaultTime: null, minTime: null, maxTime: null, spinnerImage: 'spinnerDefault.png', spinnerSize: [20, 20, 8], spinnerBigImage: '', spinnerBigSize: [40, 40, 16], spinnerIncDecOnly: false, spinnerRepeat: [500, 250], beforeShow: null, beforeSetTime: null }; $.extend(this._defaults, this.regional['']) } $.extend(TimeEntry.prototype, { markerClassName: 'hasTimeEntry', propertyName: 'timeEntry', _wrapClass: 'timeEntry_wrap', _appendClass: 'timeEntry_append', _controlClass: 'timeEntry_control', _expandClass: 'timeEntry_expand', setDefaults: function (a) { $.extend(this._defaults, a || {}); return this }, _attachPlugin: function (b, c) { var d = $(b); if (d.hasClass(this.markerClassName)) { return } var e = { options: $.extend({}, this._defaults, c), input: d, _field: 0, _selectedHour: 0, _selectedMinute: 0, _selectedSecond: 0 }; d.data(this.propertyName, e).addClass(this.markerClassName).bind('focus.' + this.propertyName, this._doFocus).bind('blur.' + this.propertyName, this._doBlur).bind('click.' + this.propertyName, this._doClick).bind('keydown.' + this.propertyName, this._doKeyDown).bind('keypress.' + this.propertyName, this._doKeyPress).wrap('<span class="' + this._wrapClass + '"></span>'); if ($.browser.mozilla) { d.bind('input.' + this.propertyName, function (a) { n._parseTime(e) }) } if ($.browser.msie) { d.bind('paste.' + this.propertyName, function (a) { setTimeout(function () { n._parseTime(e) }, 1) }) } this._optionPlugin(b, c) }, _optionPlugin: function (a, b, c) { a = $(a); var d = a.data(this.propertyName); if (!b || (typeof b == 'string' && c == null)) { var e = b; b = (d || {}).options; return (b && e ? b[e] : b) } if (!a.hasClass(this.markerClassName)) { return } b = b || {}; if (typeof b == 'string') { var e = b; b = {}; b[e] = c } var f = this._extractTime(d); $.extend(d.options, b); if (f) { this._setTime(d, new Date(0, 0, 0, f[0], f[1], f[2])) } a.next('span.' + this._appendClass).remove(); a.parent().find('span.' + this._controlClass).remove(); if ($.fn.mousewheel) { a.unmousewheel() } var g = (!d.options.spinnerImage ? null : $('<span class="' + this._controlClass + '" style="display: inline-block; ' + 'background: url(\'' + d.options.spinnerImage + '\') 0 0 no-repeat; width: ' + d.options.spinnerSize[0] + 'px; height: ' + d.options.spinnerSize[1] + 'px;' + ($.browser.mozilla && $.browser.version < '1.9' ? ' padding-left: ' + d.options.spinnerSize[0] + 'px; padding-bottom: ' + (d.options.spinnerSize[1] - 18) + 'px;' : '') + '"></span>')); a.after(d.options.appendText ? '<span class="' + this._appendClass + '">' + d.options.appendText + '</span>' : '').after(g || ''); if (d.options.useMouseWheel && $.fn.mousewheel) { a.mousewheel(this._doMouseWheel) } if (g) { g.mousedown(this._handleSpinner).mouseup(this._endSpinner).mouseover(this._expandSpinner).mouseout(this._endSpinner).mousemove(this._describeSpinner) } }, _enablePlugin: function (a) { this._enableDisable(a, false) }, _disablePlugin: function (a) { this._enableDisable(a, true) }, _enableDisable: function (b, c) { var d = $.data(b, this.propertyName); if (!d) { return } b.disabled = c; if (b.nextSibling && b.nextSibling.nodeName.toLowerCase() == 'span') { n._changeSpinner(d, b.nextSibling, (c ? 5 : -1)) } n._disabledInputs = $.map(n._disabledInputs, function (a) { return (a == b ? null : a) }); if (c) { n._disabledInputs.push(b) } }, _isDisabledPlugin: function (a) { return $.inArray(a, this._disabledInputs) > -1 }, _destroyPlugin: function (b) { b = $(b); if (!b.hasClass(this.markerClassName)) { return } b.removeClass(this.markerClassName).removeData(this.propertyName).unbind('.' + this.propertyName); if ($.fn.mousewheel) { b.unmousewheel() } this._disabledInputs = $.map(this._disabledInputs, function (a) { return (a == b[0] ? null : a) }); b.parent().replaceWith(b) }, _setTimePlugin: function (a, b) { var c = $.data(a, this.propertyName); if (c) { if (b === null || b === '') { c.input.val('') } else { this._setTime(c, b ? (typeof b == 'object' ? new Date(b.getTime()) : b) : null) } } }, _getTimePlugin: function (a) { var b = $.data(a, this.propertyName); var c = (b ? this._extractTime(b) : null); return (!c ? null : new Date(0, 0, 0, c[0], c[1], c[2])) }, _getOffsetPlugin: function (a) { var b = $.data(a, this.propertyName); var c = (b ? this._extractTime(b) : null); return (!c ? 0 : (c[0] * 3600 + c[1] * 60 + c[2]) * 1000) }, _doFocus: function (a) { var b = (a.nodeName && a.nodeName.toLowerCase() == 'input' ? a : this); if (n._lastInput == b || n._isDisabledPlugin(b)) { n._focussed = false; return } var c = $.data(b, n.propertyName); n._focussed = true; n._lastInput = b; n._blurredInput = null; $.extend(c.options, ($.isFunction(c.options.beforeShow) ? c.options.beforeShow.apply(b, [b]) : {})); n._parseTime(c); setTimeout(function () { n._showField(c) }, 10) }, _doBlur: function (a) { n._blurredInput = n._lastInput; n._lastInput = null }, _doClick: function (b) { var c = b.target; var d = $.data(c, n.propertyName); if (!n._focussed) { var e = d.options.separator.length + 2; d._field = 0; if (c.selectionStart != null) { for (var f = 0; f <= Math.max(1, d._secondField, d._ampmField) ; f++) { var g = (f != d._ampmField ? (f * e) + 2 : (d._ampmField * e) + d.options.ampmPrefix.length + d.options.ampmNames[0].length); d._field = f; if (c.selectionStart < g) { break } } } else if (c.createTextRange) { var h = $(b.srcElement); var i = c.createTextRange(); var j = function (a) { return { thin: 2, medium: 4, thick: 6 }[a] || a }; var k = b.clientX + document.documentElement.scrollLeft - (h.offset().left + parseInt(j(h.css('border-left-width')), 10)) - i.offsetLeft; for (var f = 0; f <= Math.max(1, d._secondField, d._ampmField) ; f++) { var g = (f != d._ampmField ? (f * e) + 2 : (d._ampmField * e) + d.options.ampmPrefix.length + d.options.ampmNames[0].length); i.collapse(); i.moveEnd('character', g); d._field = f; if (k < i.boundingWidth) { break } } } } n._showField(d); n._focussed = false }, _doKeyDown: function (a) { if (a.keyCode >= 48) { return true } var b = $.data(a.target, n.propertyName); switch (a.keyCode) { case 9: return (a.shiftKey ? n._changeField(b, -1, true) : n._changeField(b, +1, true)); case 35: if (a.ctrlKey) { n._setValue(b, '') } else { b._field = Math.max(1, b._secondField, b._ampmField); n._adjustField(b, 0) } break; case 36: if (a.ctrlKey) { n._setTime(b) } else { b._field = 0; n._adjustField(b, 0) } break; case 37: n._changeField(b, -1, false); break; case 38: n._adjustField(b, +1); break; case 39: n._changeField(b, +1, false); break; case 40: n._adjustField(b, -1); break; case 46: n._setValue(b, ''); break; default: return true } return false }, _doKeyPress: function (a) { var b = String.fromCharCode(a.charCode == undefined ? a.keyCode : a.charCode); if (b < ' ') { return true } var c = $.data(a.target, n.propertyName); n._handleKeyPress(c, b); return false }, _doMouseWheel: function (a, b) { if (n._isDisabledPlugin(a.target)) { return } b = ($.browser.opera ? -b / Math.abs(b) : ($.browser.safari ? b / Math.abs(b) : b)); var c = $.data(a.target, n.propertyName); c.input.focus(); if (!c.input.val()) { n._parseTime(c) } n._adjustField(c, b); a.preventDefault() }, _expandSpinner: function (b) { var c = n._getSpinnerTarget(b); var d = $.data(n._getInput(c), n.propertyName); if (n._isDisabledPlugin(d.input[0])) { return } if (d.options.spinnerBigImage) { d._expanded = true; var e = $(c).offset(); var f = null; $(c).parents().each(function () { var a = $(this); if (a.css('position') == 'relative' || a.css('position') == 'absolute') { f = a.offset() } return !f }); $('<div class="' + n._expandClass + '" style="position: absolute; left: ' + (e.left - (d.options.spinnerBigSize[0] - d.options.spinnerSize[0]) / 2 - (f ? f.left : 0)) + 'px; top: ' + (e.top - (d.options.spinnerBigSize[1] - d.options.spinnerSize[1]) / 2 - (f ? f.top : 0)) + 'px; width: ' + d.options.spinnerBigSize[0] + 'px; height: ' + d.options.spinnerBigSize[1] + 'px; background: transparent url(' + d.options.spinnerBigImage + ') no-repeat 0px 0px; z-index: 10;"></div>').mousedown(n._handleSpinner).mouseup(n._endSpinner).mouseout(n._endExpand).mousemove(n._describeSpinner).insertAfter(c) } }, _getInput: function (a) { return $(a).siblings('.' + n.markerClassName)[0] }, _describeSpinner: function (a) { var b = n._getSpinnerTarget(a); var c = $.data(n._getInput(b), n.propertyName); b.title = c.options.spinnerTexts[n._getSpinnerRegion(c, a)] }, _handleSpinner: function (a) { var b = n._getSpinnerTarget(a); var c = n._getInput(b); if (n._isDisabledPlugin(c)) { return } if (c == n._blurredInput) { n._lastInput = c; n._blurredInput = null } var d = $.data(c, n.propertyName); n._doFocus(c); var e = n._getSpinnerRegion(d, a); n._changeSpinner(d, b, e); n._actionSpinner(d, e); n._timer = null; n._handlingSpinner = true; if (e >= 3 && d.options.spinnerRepeat[0]) { n._timer = setTimeout(function () { n._repeatSpinner(d, e) }, d.options.spinnerRepeat[0]); $(b).one('mouseout', n._releaseSpinner).one('mouseup', n._releaseSpinner) } }, _actionSpinner: function (a, b) { if (!a.input.val()) { n._parseTime(a) } switch (b) { case 0: this._setTime(a); break; case 1: this._changeField(a, -1, false); break; case 2: this._changeField(a, +1, false); break; case 3: this._adjustField(a, +1); break; case 4: this._adjustField(a, -1); break } }, _repeatSpinner: function (a, b) { if (!n._timer) { return } n._lastInput = n._blurredInput; this._actionSpinner(a, b); this._timer = setTimeout(function () { n._repeatSpinner(a, b) }, a.options.spinnerRepeat[1]) }, _releaseSpinner: function (a) { clearTimeout(n._timer); n._timer = null }, _endExpand: function (a) { n._timer = null; var b = n._getSpinnerTarget(a); var c = n._getInput(b); var d = $.data(c, n.propertyName); $(b).remove(); d._expanded = false }, _endSpinner: function (a) { n._timer = null; var b = n._getSpinnerTarget(a); var c = n._getInput(b); var d = $.data(c, n.propertyName); if (!n._isDisabledPlugin(c)) { n._changeSpinner(d, b, -1) } if (n._handlingSpinner) { n._lastInput = n._blurredInput } if (n._lastInput && n._handlingSpinner) { n._showField(d) } n._handlingSpinner = false }, _getSpinnerTarget: function (a) { return a.target || a.srcElement }, _getSpinnerRegion: function (a, b) { var c = this._getSpinnerTarget(b); var d = ($.browser.opera || $.browser.safari ? n._findPos(c) : $(c).offset()); var e = ($.browser.safari ? n._findScroll(c) : [document.documentElement.scrollLeft || document.body.scrollLeft, document.documentElement.scrollTop || document.body.scrollTop]); var f = (a.options.spinnerIncDecOnly ? 99 : b.clientX + e[0] - d.left - ($.browser.msie ? 2 : 0)); var g = b.clientY + e[1] - d.top - ($.browser.msie ? 2 : 0); var h = a.options[a._expanded ? 'spinnerBigSize' : 'spinnerSize']; var i = (a.options.spinnerIncDecOnly ? 99 : h[0] - 1 - f); var j = h[1] - 1 - g; if (h[2] > 0 && Math.abs(f - i) <= h[2] && Math.abs(g - j) <= h[2]) { return 0 } var k = Math.min(f, g, i, j); return (k == f ? 1 : (k == i ? 2 : (k == g ? 3 : 4))) }, _changeSpinner: function (a, b, c) { $(b).css('background-position', '-' + ((c + 1) * a.options[a._expanded ? 'spinnerBigSize' : 'spinnerSize'][0]) + 'px 0px') }, _findPos: function (a) { var b = curTop = 0; if (a.offsetParent) { b = a.offsetLeft; curTop = a.offsetTop; while (a = a.offsetParent) { var c = b; b += a.offsetLeft; if (b < 0) { b = c } curTop += a.offsetTop } } return { left: b, top: curTop } }, _findScroll: function (a) { var b = false; $(a).parents().each(function () { b |= $(this).css('position') == 'fixed' }); if (b) { return [0, 0] } var c = a.scrollLeft; var d = a.scrollTop; while (a = a.parentNode) { c += a.scrollLeft || 0; d += a.scrollTop || 0 } return [c, d] }, _parseTime: function (a) { var b = this._extractTime(a); if (b) { a._selectedHour = b[0]; a._selectedMinute = b[1]; a._selectedSecond = b[2] } else { var c = this._constrainTime(a); a._selectedHour = c[0]; a._selectedMinute = c[1]; a._selectedSecond = (a.options.showSeconds ? c[2] : 0) } a._secondField = (a.options.showSeconds ? 2 : -1); a._ampmField = (a.options.show24Hours ? -1 : (a.options.showSeconds ? 3 : 2)); a._lastChr = ''; a._field = Math.max(0, Math.min(Math.max(1, a._secondField, a._ampmField), a.options.initialField)); if (a.input.val() != '') { this._showTime(a) } }, _extractTime: function (a, b) { b = b || a.input.val(); var c = b.split(a.options.separator); if (a.options.separator == '' && b != '') { c[0] = b.substring(0, 2); c[1] = b.substring(2, 4); c[2] = b.substring(4, 6) } if (c.length >= 2) { var d = !a.options.show24Hours && (b.indexOf(a.options.ampmNames[0]) > -1); var e = !a.options.show24Hours && (b.indexOf(a.options.ampmNames[1]) > -1); var f = parseInt(c[0], 10); f = (isNaN(f) ? 0 : f); f = ((d || e) && f == 12 ? 0 : f) + (e ? 12 : 0); var g = parseInt(c[1], 10); g = (isNaN(g) ? 0 : g); var h = (c.length >= 3 ? parseInt(c[2], 10) : 0); h = (isNaN(h) || !a.options.showSeconds ? 0 : h); return this._constrainTime(a, [f, g, h]) } return null }, _constrainTime: function (a, b) { var c = (b != null); if (!c) { var d = this._determineTime(a.options.defaultTime, a) || new Date(); b = [d.getHours(), d.getMinutes(), d.getSeconds()] } var e = false; for (var i = 0; i < a.options.timeSteps.length; i++) { if (e) { b[i] = 0 } else if (a.options.timeSteps[i] > 1) { b[i] = Math.round(b[i] / a.options.timeSteps[i]) * a.options.timeSteps[i]; e = true } } return b }, _showTime: function (a) { var b = (this._formatNumber(a.options.show24Hours ? a._selectedHour : ((a._selectedHour + 11) % 12) + 1) + a.options.separator + this._formatNumber(a._selectedMinute) + (a.options.showSeconds ? a.options.separator + this._formatNumber(a._selectedSecond) : '') + (a.options.show24Hours ? '' : a.options.ampmPrefix + a.options.ampmNames[(a._selectedHour < 12 ? 0 : 1)])); this._setValue(a, b); this._showField(a) }, _showField: function (a) { var b = a.input[0]; if (a.input.is(':hidden') || n._lastInput != b) { return } var c = a.options.separator.length + 2; var d = (a._field != a._ampmField ? (a._field * c) : (a._ampmField * c) - a.options.separator.length + a.options.ampmPrefix.length); var e = d + (a._field != a._ampmField ? 2 : a.options.ampmNames[0].length); if (b.setSelectionRange) { b.setSelectionRange(d, e) } else if (b.createTextRange) { var f = b.createTextRange(); f.moveStart('character', d); f.moveEnd('character', e - a.input.val().length); f.select() } if (!b.disabled) { b.focus() } }, _formatNumber: function (a) { return (a < 10 ? '0' : '') + a }, _setValue: function (a, b) { if (b != a.input.val()) { a.input.val(b).trigger('change') } }, _changeField: function (a, b, c) { var d = (a.input.val() == '' || a._field == (b == -1 ? 0 : Math.max(1, a._secondField, a._ampmField))); if (!d) { a._field += b } this._showField(a); a._lastChr = ''; return (d && c) }, _adjustField: function (a, b) { if (a.input.val() == '') { b = 0 } this._setTime(a, new Date(0, 0, 0, a._selectedHour + (a._field == 0 ? b * a.options.timeSteps[0] : 0) + (a._field == a._ampmField ? b * 12 : 0), a._selectedMinute + (a._field == 1 ? b * a.options.timeSteps[1] : 0), a._selectedSecond + (a._field == a._secondField ? b * a.options.timeSteps[2] : 0))) }, _setTime: function (a, b) { b = this._determineTime(b, a); var c = this._constrainTime(a, b ? [b.getHours(), b.getMinutes(), b.getSeconds()] : null); b = new Date(0, 0, 0, c[0], c[1], c[2]); var b = this._normaliseTime(b); var d = this._normaliseTime(this._determineTime(a.options.minTime, a)); var e = this._normaliseTime(this._determineTime(a.options.maxTime, a)); b = (d && b < d ? d : (e && b > e ? e : b)); if ($.isFunction(a.options.beforeSetTime)) { b = a.options.beforeSetTime.apply(a.input[0], [this._getTimePlugin(a.input[0]), b, d, e]) } a._selectedHour = b.getHours(); a._selectedMinute = b.getMinutes(); a._selectedSecond = b.getSeconds(); this._showTime(a) }, _determineTime: function (i, j) { var k = function (a) { var b = new Date(); b.setTime(b.getTime() + a * 1000); return b }; var l = function (a) { var b = n._extractTime(j, a); var c = new Date(); var d = (b ? b[0] : c.getHours()); var e = (b ? b[1] : c.getMinutes()); var f = (b ? b[2] : c.getSeconds()); if (!b) { var g = /([+-]?[0-9]+)\s*(s|S|m|M|h|H)?/g; var h = g.exec(a); while (h) { switch (h[2] || 's') { case 's': case 'S': f += parseInt(h[1], 10); break; case 'm': case 'M': e += parseInt(h[1], 10); break; case 'h': case 'H': d += parseInt(h[1], 10); break } h = g.exec(a) } } c = new Date(0, 0, 10, d, e, f, 0); if (/^!/.test(a)) { if (c.getDate() > 10) { c = new Date(0, 0, 10, 23, 59, 59) } else if (c.getDate() < 10) { c = new Date(0, 0, 10, 0, 0, 0) } } return c }; return (i ? (typeof i == 'string' ? l(i) : (typeof i == 'number' ? k(i) : i)) : null) }, _normaliseTime: function (a) { if (!a) { return null } a.setFullYear(1900); a.setMonth(0); a.setDate(0); return a }, _handleKeyPress: function (a, b) { if (b == a.options.separator) { this._changeField(a, +1, false) } else if (b >= '0' && b <= '9') { var c = parseInt(b, 10); var d = parseInt(a._lastChr + b, 10); var e = (a._field != 0 ? a._selectedHour : (a.options.show24Hours ? (d < 24 ? d : c) : (d >= 1 && d <= 12 ? d : (c > 0 ? c : a._selectedHour)) % 12 + (a._selectedHour >= 12 ? 12 : 0))); var f = (a._field != 1 ? a._selectedMinute : (d < 60 ? d : c)); var g = (a._field != a._secondField ? a._selectedSecond : (d < 60 ? d : c)); var h = this._constrainTime(a, [e, f, g]); this._setTime(a, new Date(0, 0, 0, h[0], h[1], h[2])); if (a.options.noSeparatorEntry && a._lastChr) { this._changeField(a, +1, false) } else { a._lastChr = b } } else if (!a.options.show24Hours) { b = b.toLowerCase(); if ((b == a.options.ampmNames[0].substring(0, 1).toLowerCase() && a._selectedHour >= 12) || (b == a.options.ampmNames[1].substring(0, 1).toLowerCase() && a._selectedHour < 12)) { var i = a._field; a._field = a._ampmField; this._adjustField(a, +1); a._field = i; this._showField(a) } } } }); var m = ['getOffset', 'getTime', 'isDisabled']; function isNotChained(a, b) { if (a == 'option' && (b.length == 0 || (b.length == 1 && typeof b[0] == 'string'))) { return true } return $.inArray(a, m) > -1 } $.fn.timeEntry = function (b) { var c = Array.prototype.slice.call(arguments, 1); if (isNotChained(b, c)) { return n['_' + b + 'Plugin'].apply(n, [this[0]].concat(c)) } return this.each(function () { if (typeof b == 'string') { if (!n['_' + b + 'Plugin']) { throw 'Unknown command: ' + b; } n['_' + b + 'Plugin'].apply(n, [this].concat(c)) } else { var a = ($.fn.metadata ? $(this).metadata() : {}); n._attachPlugin(this, $.extend(a, b || {})) } }) }; var n = $.timeEntry = new TimeEntry() })(jQuery);


(function (jQuery) {

	var daysInWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
	var shortMonthsInYear = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
	var longMonthsInYear = ["January", "February", "March", "April", "May", "June",
													"July", "August", "September", "October", "November", "December"];
	var shortMonthsToNumber = [];
	shortMonthsToNumber["Jan"] = "01";
	shortMonthsToNumber["Feb"] = "02";
	shortMonthsToNumber["Mar"] = "03";
	shortMonthsToNumber["Apr"] = "04";
	shortMonthsToNumber["May"] = "05";
	shortMonthsToNumber["Jun"] = "06";
	shortMonthsToNumber["Jul"] = "07";
	shortMonthsToNumber["Aug"] = "08";
	shortMonthsToNumber["Sep"] = "09";
	shortMonthsToNumber["Oct"] = "10";
	shortMonthsToNumber["Nov"] = "11";
	shortMonthsToNumber["Dec"] = "12";

	jQuery.format = (function () {
		function strDay(value) {
			return daysInWeek[parseInt(value, 10)] || value;
		}

		function strMonth(value) {
			var monthArrayIndex = parseInt(value, 10) - 1;
			return shortMonthsInYear[monthArrayIndex] || value;
		}

		function strLongMonth(value) {
			var monthArrayIndex = parseInt(value, 10) - 1;
			return longMonthsInYear[monthArrayIndex] || value;
		}

		var parseMonth = function (value) {
			return shortMonthsToNumber[value] || value;
		};

		var parseTime = function (value) {
			var retValue = value;
			var millis = "";
			if (retValue.indexOf(".") !== -1) {
				var delimited = retValue.split('.');
				retValue = delimited[0];
				millis = delimited[1];
			}

			var values3 = retValue.split(":");

			if (values3.length === 3) {
				hour = values3[0];
				minute = values3[1];
				second = values3[2];

				return {
					time: retValue,
					hour: hour,
					minute: minute,
					second: second,
					millis: millis
				};
			} else {
				return {
					time: "",
					hour: "",
					minute: "",
					second: "",
					millis: ""
				};
			}
		};

		return {
			date: function (value, format) {
				/*
					value = new java.util.Date()
					2009-12-18 10:54:50.546
				*/
				try {
					var date = null;
					var year = null;
					var month = null;
					var dayOfMonth = null;
					var dayOfWeek = null;
					var time = null;
					if (typeof value == "number") {
						return this.date(new Date(value), format);
					} else if (typeof value.getFullYear == "function") {
						year = value.getFullYear();
						month = value.getMonth() + 1;
						dayOfMonth = value.getDate();
						dayOfWeek = value.getDay();
						time = parseTime(value.toTimeString());
					} else if (value.search(/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.?\d{0,3}[Z\-+]?(\d{2}:?\d{2})?/) != -1) {
						/* 2009-04-19T16:11:05+02:00 || 2009-04-19T16:11:05Z */
						var values = value.split(/[T\+-]/);
						year = values[0];
						month = values[1];
						dayOfMonth = values[2];
						time = parseTime(values[3].split(".")[0]);
						date = new Date(year, month - 1, dayOfMonth);
						dayOfWeek = date.getDay();
					} else {
						var values = value.split(" ");
						switch (values.length) {
							case 6:
								/* Wed Jan 13 10:43:41 CET 2010 */
								year = values[5];
								month = parseMonth(values[1]);
								dayOfMonth = values[2];
								time = parseTime(values[3]);
								date = new Date(year, month - 1, dayOfMonth);
								dayOfWeek = date.getDay();
								break;
							case 2:
								/* 2009-12-18 10:54:50.546 */
								var values2 = values[0].split("-");
								year = values2[0];
								month = values2[1];
								dayOfMonth = values2[2];
								time = parseTime(values[1]);
								date = new Date(year, month - 1, dayOfMonth);
								dayOfWeek = date.getDay();
								break;
							case 7:
								/* Tue Mar 01 2011 12:01:42 GMT-0800 (PST) */
							case 9:
								/*added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0800 (China Standard Time) */
							case 10:
								/* added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0200 (W. Europe Daylight Time) */
								year = values[3];
								month = parseMonth(values[1]);
								dayOfMonth = values[2];
								time = parseTime(values[4]);
								date = new Date(year, month - 1, dayOfMonth);
								dayOfWeek = date.getDay();
								break;
							case 1:
								/* added by Jonny, for 2012-02-07CET00:00:00 (Doctrine Entity -> Json Serializer) */
								var values2 = values[0].split("");
								year = values2[0] + values2[1] + values2[2] + values2[3];
								month = values2[5] + values2[6];
								dayOfMonth = values2[8] + values2[9];
								time = parseTime(values2[13] + values2[14] + values2[15] + values2[16] + values2[17] + values2[18] + values2[19] + values2[20])
								date = new Date(year, month - 1, dayOfMonth);
								dayOfWeek = date.getDay();
								break;
							default:
								return value;
						}
					}

					var pattern = "";
					var retValue = "";
					var unparsedRest = "";
					/*
						Issue 1 - variable scope issue in format.date
						Thanks jakemonO
					*/
					for (var i = 0; i < format.length; i++) {
						var currentPattern = format.charAt(i);
						pattern += currentPattern;
						unparsedRest = "";
						switch (pattern) {
							case "ddd":
								retValue += strDay(dayOfWeek);
								pattern = "";
								break;
							case "dd":
								if (format.charAt(i + 1) == "d") {
									break;
								}
								if (String(dayOfMonth).length === 1) {
									dayOfMonth = '0' + dayOfMonth;
								}
								retValue += dayOfMonth;
								pattern = "";
								break;
							case "d":
								if (format.charAt(i + 1) == "d") {
									break;
								}
								retValue += parseInt(dayOfMonth, 10);
								pattern = "";
								break;
							case "D":
								if (dayOfMonth == 1 || dayOfMonth == 21 || dayOfMonth == 31) {
									dayOfMonth = dayOfMonth + 'st';
								} else if (dayOfMonth == 2 || dayOfMonth == 22) {
									dayOfMonth = dayOfMonth + 'nd';
								} else if (dayOfMonth == 3 || dayOfMonth == 23) {
									dayOfMonth = dayOfMonth + 'rd';
								} else {
									dayOfMonth = dayOfMonth + 'th';
								}
								retValue += dayOfMonth;
								pattern = "";
								break;
							case "MMMM":
								retValue += strLongMonth(month);
								pattern = "";
								break;
							case "MMM":
								if (format.charAt(i + 1) === "M") {
									break;
								}
								retValue += strMonth(month);
								pattern = "";
								break;
							case "MM":
								if (format.charAt(i + 1) == "M") {
									break;
								}
								if (String(month).length === 1) {
									month = '0' + month;
								}
								retValue += month;
								pattern = "";
								break;
							case "M":
								if (format.charAt(i + 1) == "M") {
									break;
								}
								retValue += parseInt(month, 10);
								pattern = "";
								break;
							case "y":
							case "yyy":
								if (format.charAt(i + 1) == "y") {
									break;
								}
								retValue += pattern;
								pattern = "";
								break;
							case "yy":
								if (format.charAt(i + 1) == "y" &&
								format.charAt(i + 2) == "y") {
									break;
								}
								retValue += String(year).slice(-2);
								pattern = "";
								break;
							case "yyyy":
								retValue += year;
								pattern = "";
								break;
							case "H":
								if (format.charAt(i + 1) == "H") {
									break;
								}
								retValue += pattern;
								pattern = "";
								break;
							case "HH":
								retValue += time.hour;
								pattern = "";
								break;
							case "hh":
								/* time.hour is "00" as string == is used instead of === */
								var hour = (time.hour == 0 ? 12 : time.hour < 13 ? time.hour : time.hour - 12);
								hour = String(hour).length == 1 ? '0' + hour : hour;
								retValue += hour;
								pattern = "";
								break;
							case "h":
								if (format.charAt(i + 1) == "h") {
									break;
								}
								var hour = (time.hour == 0 ? 12 : time.hour < 13 ? time.hour : time.hour - 12);
								retValue += parseInt(hour, 10);
								// Fixing issue https://github.com/phstc/jquery-dateFormat/issues/21
								// retValue = parseInt(retValue, 10);
								pattern = "";
								break;
							case "m":
								if (format.charAt(i + 1) == "m") {
									break;
								}
								retValue += pattern;
								pattern = "";
								break;
							case "mm":
								retValue += time.minute;
								pattern = "";
								break;
							case "s":
								if (format.charAt(i + 1) == "s") {
									break;
								}
								retValue += pattern;
								pattern = "";
								break;
							case "ss":
								/* ensure only seconds are added to the return string */
								retValue += time.second.substring(0, 2);
								pattern = "";
								break;
							case "S":
							case "SS":
								if (format.charAt(i + 1) == "S") {
									break;
								}
								retValue += pattern;
								pattern = "";
								break;
							case "SSS":
								retValue += time.millis.substring(0, 3);
								pattern = "";
								break;
							case "a":
								retValue += time.hour >= 12 ? "PM" : "AM";
								pattern = "";
								break;
							case "p":
								retValue += time.hour >= 12 ? "p.m." : "a.m.";
								pattern = "";
								break;
							default:
								retValue += currentPattern;
								pattern = "";
								break;
						}
					}
					retValue += unparsedRest;
					return retValue;
				} catch (e) {
					console.log(e);
					return value;
				}
			}
		};
	}());
}(jQuery));

jQuery.format.date.defaultShortDateFormat = "dd/MM/yyyy";
jQuery.format.date.defaultLongDateFormat = "dd/MM/yyyy HH:mm:ss";

jQuery(document).ready(function () {
	jQuery(".shortDateFormat").each(function (idx, elem) {
		if (jQuery(elem).is(":input")) {
			jQuery(elem).val(jQuery.format.date(jQuery(elem).val(), jQuery.format.date.defaultShortDateFormat));
		} else {
			jQuery(elem).text(jQuery.format.date(jQuery(elem).text(), jQuery.format.date.defaultShortDateFormat));
		}
	});
	jQuery(".longDateFormat").each(function (idx, elem) {
		if (jQuery(elem).is(":input")) {
			jQuery(elem).val(jQuery.format.date(jQuery(elem).val(), jQuery.format.date.defaultLongDateFormat));
		} else {
			jQuery(elem).text(jQuery.format.date(jQuery(elem).text(), jQuery.format.date.defaultLongDateFormat));
		}
	});
});

/*
 * Toastr
 * Version 2.0.1
 * Copyright 2012 John Papa and Hans Fjällemark.  
 * All Rights Reserved.  
 * Use, reproduction, distribution, and modification of this code is subject to the terms and 
 * conditions of the MIT license, available at http://www.opensource.org/licenses/mit-license.php
 *
 * Author: John Papa and Hans Fjällemark
 * Project: https://github.com/CodeSeven/toastr
 */
; (function (define) {
	define(['jquery'], function ($) {
		return (function () {
			var version = '2.0.1';
			var $container;
			var listener;
			var toastId = 0;
			var toastType = {
				error: 'error',
				info: 'info',
				success: 'success',
				warning: 'warning'
			};

			var toastr = {
				clear: clear,
				error: error,
				getContainer: getContainer,
				info: info,
				options: {},
				subscribe: subscribe,
				success: success,
				version: version,
				warning: warning
			};

			return toastr;

			//#region Accessible Methods
			function error(message, title, optionsOverride) {
				return notify({
					type: toastType.error,
					iconClass: getOptions().iconClasses.error,
					message: message,
					optionsOverride: optionsOverride,
					title: title
				});
			}

			function info(message, title, optionsOverride) {
				return notify({
					type: toastType.info,
					iconClass: getOptions().iconClasses.info,
					message: message,
					optionsOverride: optionsOverride,
					title: title
				});
			}

			function subscribe(callback) {
				listener = callback;
			}

			function success(message, title, optionsOverride) {
				return notify({
					type: toastType.success,
					iconClass: getOptions().iconClasses.success,
					message: message,
					optionsOverride: optionsOverride,
					title: title
				});
			}

			function warning(message, title, optionsOverride) {
				return notify({
					type: toastType.warning,
					iconClass: getOptions().iconClasses.warning,
					message: message,
					optionsOverride: optionsOverride,
					title: title
				});
			}

			function clear($toastElement) {
				var options = getOptions();
				if (!$container) { getContainer(options); }
				if ($toastElement && $(':focus', $toastElement).length === 0) {
					$toastElement[options.hideMethod]({
						duration: options.hideDuration,
						easing: options.hideEasing,
						complete: function () { removeToast($toastElement); }
					});
					return;
				}
				if ($container.children().length) {
					$container[options.hideMethod]({
						duration: options.hideDuration,
						easing: options.hideEasing,
						complete: function () { $container.remove(); }
					});
				}
			}
			//#endregion

			//#region Internal Methods

			function getDefaults() {
				return {
					tapToDismiss: true,
					toastClass: 'toast',
					containerId: 'toast-container',
					debug: false,

					showMethod: 'fadeIn', //fadeIn, slideDown, and show are built into jQuery
					showDuration: 300,
					showEasing: 'swing', //swing and linear are built into jQuery
					onShown: undefined,
					hideMethod: 'fadeOut',
					hideDuration: 1000,
					hideEasing: 'swing',
					onHidden: undefined,

					extendedTimeOut: 1000,
					iconClasses: {
						error: 'toast-error',
						info: 'toast-info',
						success: 'toast-success',
						warning: 'toast-warning'
					},
					iconClass: 'toast-info',
					positionClass: 'toast-top-right',
					timeOut: 5000, // Set timeOut and extendedTimeout to 0 to make it sticky
					titleClass: 'toast-title',
					messageClass: 'toast-message',
					target: 'body',
					closeHtml: '<button>&times;</button>',
					newestOnTop: true
				};
			}

			function publish(args) {
				if (!listener) {
					return;
				}
				listener(args);
			}

			function notify(map) {
				var
					options = getOptions(),
					iconClass = map.iconClass || options.iconClass;

				if (typeof (map.optionsOverride) !== 'undefined') {
					options = $.extend(options, map.optionsOverride);
					iconClass = map.optionsOverride.iconClass || iconClass;
				}

				toastId++;

				$container = getContainer(options);
				var
					intervalId = null,
					$toastElement = $('<div/>'),
					$titleElement = $('<div/>'),
					$messageElement = $('<div/>'),
					$closeElement = $(options.closeHtml),
					response = {
						toastId: toastId,
						state: 'visible',
						startTime: new Date(),
						options: options,
						map: map
					};

				if (map.iconClass) {
					$toastElement.addClass(options.toastClass).addClass(iconClass);
				}

				if (map.title) {
					$titleElement.append(map.title).addClass(options.titleClass);
					$toastElement.append($titleElement);
				}

				if (map.message) {
					$messageElement.append(map.message).addClass(options.messageClass);
					$toastElement.append($messageElement);
				}

				if (options.closeButton) {
					$closeElement.addClass('toast-close-button');
					$toastElement.prepend($closeElement);
				}

				$toastElement.hide();
				if (options.newestOnTop) {
					$container.prepend($toastElement);
				} else {
					$container.append($toastElement);
				}


				$toastElement[options.showMethod](
					{ duration: options.showDuration, easing: options.showEasing, complete: options.onShown }
				);
				if (options.timeOut > 0) {
					intervalId = setTimeout(hideToast, options.timeOut);
				}

				$toastElement.hover(stickAround, delayedhideToast);
				if (!options.onclick && options.tapToDismiss) {
					$toastElement.click(hideToast);
				}
				if (options.closeButton && $closeElement) {
					$closeElement.click(function (event) {
						event.stopPropagation();
						hideToast(true);
					});
				}

				if (options.onclick) {
					$toastElement.click(function () {
						options.onclick();
						hideToast();
					});
				}

				publish(response);

				if (options.debug && console) {
					console.log(response);
				}

				return $toastElement;

				function hideToast(override) {
					if ($(':focus', $toastElement).length && !override) {
						return;
					}
					return $toastElement[options.hideMethod]({
						duration: options.hideDuration,
						easing: options.hideEasing,
						complete: function () {
							removeToast($toastElement);
							if (options.onHidden) {
								options.onHidden();
							}
							response.state = 'hidden';
							response.endTime = new Date(),
							publish(response);
						}
					});
				}

				function delayedhideToast() {
					if (options.timeOut > 0 || options.extendedTimeOut > 0) {
						intervalId = setTimeout(hideToast, options.extendedTimeOut);
					}
				}

				function stickAround() {
					clearTimeout(intervalId);
					$toastElement.stop(true, true)[options.showMethod](
						{ duration: options.showDuration, easing: options.showEasing }
					);
				}
			}
			function getContainer(options) {
				if (!options) { options = getOptions(); }
				$container = $('#' + options.containerId);
				if ($container.length) {
					return $container;
				}
				$container = $('<div/>')
					.attr('id', options.containerId)
					.addClass(options.positionClass);
				$container.appendTo($(options.target));
				return $container;
			}

			function getOptions() {
				return $.extend({}, getDefaults(), toastr.options);
			}

			function removeToast($toastElement) {
				if (!$container) { $container = getContainer(); }
				if ($toastElement.is(':visible')) {
					return;
				}
				$toastElement.remove();
				$toastElement = null;
				if ($container.children().length === 0) {
					$container.remove();
				}
			}
			//#endregion

		})();
	});
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
	if (typeof module !== 'undefined' && module.exports) { //Node
		module.exports = factory(require(deps[0]));
	} else {
		window['toastr'] = factory(window['jQuery']);
	}
}));


/*! Hammer.JS - v1.1.1 - 2014-04-23
 * http://eightmedia.github.io/hammer.js
 *
 * Copyright (c) 2014 Jorik Tangelder <j.tangelder@gmail.com>;
 * Licensed under the MIT license */


!function (a, b) { "use strict"; function c() { d.READY || (s.determineEventTypes(), r.each(d.gestures, function (a) { u.register(a) }), s.onTouch(d.DOCUMENT, n, u.detect), s.onTouch(d.DOCUMENT, o, u.detect), d.READY = !0) } var d = function v(a, b) { return new v.Instance(a, b || {}) }; d.VERSION = "1.1.1", d.defaults = { behavior: { userSelect: "none", touchAction: "none", touchCallout: "none", contentZooming: "none", userDrag: "none", tapHighlightColor: "rgba(0,0,0,0)" } }, d.DOCUMENT = a.document, d.HAS_POINTEREVENTS = a.navigator.pointerEnabled || a.navigator.msPointerEnabled, d.HAS_TOUCHEVENTS = "ontouchstart" in a, d.CALCULATE_INTERVAL = 50; var e = {}, f = d.DIRECTION_DOWN = "down", g = d.DIRECTION_LEFT = "left", h = d.DIRECTION_UP = "up", i = d.DIRECTION_RIGHT = "right", j = d.POINTER_MOUSE = "mouse", k = d.POINTER_TOUCH = "touch", l = d.POINTER_PEN = "pen", m = d.EVENT_START = "start", n = d.EVENT_MOVE = "move", o = d.EVENT_END = "end", p = d.EVENT_RELEASE = "release", q = d.EVENT_TOUCH = "touch"; d.READY = !1, d.plugins = d.plugins || {}, d.gestures = d.gestures || {}; var r = d.utils = { extend: function (a, c, d) { for (var e in c) a[e] !== b && d || "returnValue" == e || (a[e] = c[e]); return a }, on: function (a, b, c) { a.addEventListener(b, c, !1) }, off: function (a, b, c) { a.removeEventListener(b, c, !1) }, each: function (a, c, d) { var e, f; if ("forEach" in a) a.forEach(c, d); else if (a.length !== b) { for (e = 0, f = a.length; f > e; e++) if (c.call(d, a[e], e, a) === !1) return } else for (e in a) if (a.hasOwnProperty(e) && c.call(d, a[e], e, a) === !1) return }, inStr: function (a, b) { return a.indexOf(b) > -1 }, inArray: function (a, b) { if (a.indexOf) { var c = a.indexOf(b); return -1 === c ? !1 : c } for (var d = 0, e = a.length; e > d; d++) if (a[d] === b) return d; return !1 }, toArray: function (a) { return Array.prototype.slice.call(a, 0) }, hasParent: function (a, b) { for (; a;) { if (a == b) return !0; a = a.parentNode } return !1 }, getCenter: function (a) { var b = [], c = [], d = [], e = [], f = Math.min, g = Math.max; return 1 === a.length ? { pageX: a[0].pageX, pageY: a[0].pageY, clientX: a[0].clientX, clientY: a[0].clientY } : (r.each(a, function (a) { b.push(a.pageX), c.push(a.pageY), d.push(a.clientX), e.push(a.clientY) }), { pageX: (f.apply(Math, b) + g.apply(Math, b)) / 2, pageY: (f.apply(Math, c) + g.apply(Math, c)) / 2, clientX: (f.apply(Math, d) + g.apply(Math, d)) / 2, clientY: (f.apply(Math, e) + g.apply(Math, e)) / 2 }) }, getVelocity: function (a, b, c) { return { x: Math.abs(b / a) || 0, y: Math.abs(c / a) || 0 } }, getAngle: function (a, b) { var c = b.clientX - a.clientX, d = b.clientY - a.clientY; return 180 * Math.atan2(d, c) / Math.PI }, getDirection: function (a, b) { var c = Math.abs(a.clientX - b.clientX), d = Math.abs(a.clientY - b.clientY); return c >= d ? a.clientX - b.clientX > 0 ? g : i : a.clientY - b.clientY > 0 ? h : f }, getDistance: function (a, b) { var c = b.clientX - a.clientX, d = b.clientY - a.clientY; return Math.sqrt(c * c + d * d) }, getScale: function (a, b) { return a.length >= 2 && b.length >= 2 ? this.getDistance(b[0], b[1]) / this.getDistance(a[0], a[1]) : 1 }, getRotation: function (a, b) { return a.length >= 2 && b.length >= 2 ? this.getAngle(b[1], b[0]) - this.getAngle(a[1], a[0]) : 0 }, isVertical: function (a) { return a == h || a == f }, toggleBehavior: function (a, b, c) { if (b && a && a.style) { r.each(["webkit", "moz", "Moz", "ms", "o", ""], function (d) { r.each(b, function (b, e) { d && (e = d + e.substring(0, 1).toUpperCase() + e.substring(1)), e in a.style && (a.style[e] = !c && b) }) }); var d = function () { return !1 }; "none" == b.userSelect && (a.onselectstart = !c && d), "none" == b.userDrag && (a.ondragstart = !c && d) } }, toCamelCase: function (a) { return a.replace(/[_-]([a-z])/g, function (a) { return a[1].toUpperCase() }) } }, s = d.event = { preventMouseEvents: !1, started: !1, shouldDetect: !1, on: function (a, b, c, d) { var e = b.split(" "); r.each(e, function (b) { r.on(a, b, c), d && d(b) }) }, off: function (a, b, c, d) { var e = b.split(" "); r.each(e, function (b) { r.off(a, b, c), d && d(b) }) }, onTouch: function (a, b, c) { var f = this, g = function (e) { var g, h = e.type.toLowerCase(), i = d.HAS_POINTEREVENTS, j = r.inStr(h, "mouse"); j && f.preventMouseEvents || (j && b == m ? (f.preventMouseEvents = !1, f.shouldDetect = !0) : b != m || j || (f.preventMouseEvents = !0, f.shouldDetect = !0), i && b != o && t.updatePointer(b, e), f.shouldDetect && (g = f.doDetect.call(f, e, b, a, c)), g == o ? (f.preventMouseEvents = !1, f.shouldDetect = !1, t.reset()) : i && b == o && t.updatePointer(b, e)) }; return this.on(a, e[b], g), g }, doDetect: function (a, b, c, d) { var e = this.getTouchList(a, b), f = e.length, g = b, h = e.trigger, i = f; b == m ? h = q : b == o && (h = p, i = e.length - (a.changedTouches ? a.changedTouches.length : 1)), i > 0 && this.started && (g = n), this.started = !0; var j = this.collectEventData(c, g, e, a); return b != o && d.call(u, j), h && (j.changedLength = i, j.eventType = h, d.call(u, j), j.eventType = g, delete j.changedLength), g == o && (d.call(u, j), this.started = !1), g }, determineEventTypes: function () { var b; return b = d.HAS_POINTEREVENTS ? a.PointerEvent ? ["pointerdown", "pointermove", "pointerup pointercancel"] : ["MSPointerDown", "MSPointerMove", "MSPointerUp MSPointerCancel"] : ["touchstart mousedown", "touchmove mousemove", "touchend touchcancel mouseup"], e[m] = b[0], e[n] = b[1], e[o] = b[2], e }, getTouchList: function (a, b) { if (d.HAS_POINTEREVENTS) return t.getTouchList(); if (a.touches) { if (b == n) return a.touches; var c = [], e = [].concat(r.toArray(a.touches), r.toArray(a.changedTouches)), f = []; return r.each(e, function (a) { r.inArray(c, a.identifier) === !1 && f.push(a), c.push(a.identifier) }), f } return a.identifier = 1, [a] }, collectEventData: function (a, b, c, d) { var e = k; return (r.inStr(d.type, "mouse") || t.matchType(j, d)) && (e = j), { center: r.getCenter(c), timeStamp: Date.now(), target: d.target, touches: c, eventType: b, pointerType: e, srcEvent: d, preventDefault: function () { var a = this.srcEvent; a.preventManipulation && a.preventManipulation(), a.preventDefault && a.preventDefault() }, stopPropagation: function () { this.srcEvent.stopPropagation() }, stopDetect: function () { return u.stopDetect() } } } }, t = d.PointerEvent = { pointers: {}, getTouchList: function () { var a = []; return r.each(this.pointers, function (b) { a.push(b) }), a }, updatePointer: function (a, b) { a == o ? delete this.pointers[b.pointerId] : (b.identifier = b.pointerId, this.pointers[b.pointerId] = b) }, matchType: function (a, b) { if (!b.pointerType) return !1; var c = b.pointerType, d = {}; return d[j] = c === (b.MSPOINTER_TYPE_MOUSE || j), d[k] = c === (b.MSPOINTER_TYPE_TOUCH || k), d[l] = c === (b.MSPOINTER_TYPE_PEN || l), d[a] }, reset: function () { this.pointers = {} } }, u = d.detection = { gestures: [], current: null, previous: null, stopped: !1, startDetect: function (a, b) { this.current || (this.stopped = !1, this.current = { inst: a, startEvent: r.extend({}, b), lastEvent: !1, lastCalcEvent: !1, futureCalcEvent: !1, lastCalcData: {}, name: "" }, this.detect(b)) }, detect: function (a) { if (this.current && !this.stopped) { a = this.extendEventData(a); var b = this.current.inst, c = b.options; return r.each(this.gestures, function (d) { return !this.stopped && b.enabled && c[d.name] && d.handler.call(d, a, b) === !1 ? (this.stopDetect(), !1) : void 0 }, this), this.current && (this.current.lastEvent = a), a.eventType == o && this.stopDetect(), a } }, stopDetect: function () { this.previous = r.extend({}, this.current), this.current = null, this.stopped = !0 }, getCalculatedData: function (a, b, c, e, f) { var g = this.current, h = !1, i = g.lastCalcEvent, j = g.lastCalcData; i && a.timeStamp - i.timeStamp > d.CALCULATE_INTERVAL && (b = i.center, c = a.timeStamp - i.timeStamp, e = a.center.clientX - i.center.clientX, f = a.center.clientY - i.center.clientY, h = !0), (a.eventType == q || a.eventType == p) && (g.futureCalcEvent = a), (!g.lastCalcEvent || h) && (j.velocity = r.getVelocity(c, e, f), j.angle = r.getAngle(b, a.center), j.direction = r.getDirection(b, a.center), g.lastCalcEvent = g.futureCalcEvent || a, g.futureCalcEvent = a), a.velocityX = j.velocity.x, a.velocityY = j.velocity.y, a.interimAngle = j.angle, a.interimDirection = j.direction }, extendEventData: function (a) { var b = this.current, c = b.startEvent, d = b.lastEvent || c; (a.eventType == q || a.eventType == p) && (c.touches = [], r.each(a.touches, function (a) { c.touches.push(r.extend({}, a)) })); var e = a.timeStamp - c.timeStamp, f = a.center.clientX - c.center.clientX, g = a.center.clientY - c.center.clientY; return this.getCalculatedData(a, d.center, e, f, g), r.extend(a, { startEvent: c, deltaTime: e, deltaX: f, deltaY: g, distance: r.getDistance(c.center, a.center), angle: r.getAngle(c.center, a.center), direction: r.getDirection(c.center, a.center), scale: r.getScale(c.touches, a.touches), rotation: r.getRotation(c.touches, a.touches) }), a }, register: function (a) { var c = a.defaults || {}; return c[a.name] === b && (c[a.name] = !0), r.extend(d.defaults, c, !0), a.index = a.index || 1e3, this.gestures.push(a), this.gestures.sort(function (a, b) { return a.index < b.index ? -1 : a.index > b.index ? 1 : 0 }), this.gestures } }; d.Instance = function (a, b) { var e = this; c(), this.element = a, this.enabled = !0, r.each(b, function (a, c) { delete b[c], b[r.toCamelCase(c)] = a }), this.options = r.extend(r.extend({}, d.defaults), b || {}), this.options.behavior && r.toggleBehavior(this.element, this.options.behavior, !1), this.eventStartHandler = s.onTouch(a, m, function (a) { e.enabled && a.eventType == m ? u.startDetect(e, a) : a.eventType == q && u.detect(a) }), this.eventHandlers = [] }, d.Instance.prototype = { on: function (a, b) { var c = this; return s.on(c.element, a, b, function (a) { c.eventHandlers.push({ gesture: a, handler: b }) }), c }, off: function (a, b) { var c = this; return s.off(c.element, a, b, function (a) { var d = r.inArray({ gesture: a, handler: b }); d !== !1 && c.eventHandlers.splice(d, 1) }), c }, trigger: function (a, b) { b || (b = {}); var c = d.DOCUMENT.createEvent("Event"); c.initEvent(a, !0, !0), c.gesture = b; var e = this.element; return r.hasParent(b.target, e) && (e = b.target), e.dispatchEvent(c), this }, enable: function (a) { return this.enabled = a, this }, dispose: function () { var a, b; for (this.options.behavior && r.toggleBehavior(this.element, this.options.behavior, !0), a = -1; b = this.eventHandlers[++a];) r.off(this.element, b.gesture, b.handler); return this.eventHandlers = [], s.off(this.element, e[m], this.eventStartHandler), null } }, function (a) { function b(b, d) { var e = u.current; if (!(d.options.dragMaxTouches > 0 && b.touches.length > d.options.dragMaxTouches)) switch (b.eventType) { case m: c = !1; break; case n: if (b.distance < d.options.dragMinDistance && e.name != a) return; var j = e.startEvent.center; if (e.name != a && (e.name = a, d.options.dragDistanceCorrection && b.distance > 0)) { var k = Math.abs(d.options.dragMinDistance / b.distance); j.pageX += b.deltaX * k, j.pageY += b.deltaY * k, j.clientX += b.deltaX * k, j.clientY += b.deltaY * k, b = u.extendEventData(b) } (e.lastEvent.dragLockToAxis || d.options.dragLockToAxis && d.options.dragLockMinDistance <= b.distance) && (b.dragLockToAxis = !0); var l = e.lastEvent.direction; b.dragLockToAxis && l !== b.direction && (b.direction = r.isVertical(l) ? b.deltaY < 0 ? h : f : b.deltaX < 0 ? g : i), c || (d.trigger(a + "start", b), c = !0), d.trigger(a, b), d.trigger(a + b.direction, b); var q = r.isVertical(b.direction); (d.options.dragBlockVertical && q || d.options.dragBlockHorizontal && !q) && b.preventDefault(); break; case p: c && b.changedLength <= d.options.dragMaxTouches && (d.trigger(a + "end", b), c = !1); break; case o: c = !1 } } var c = !1; d.gestures.Drag = { name: a, index: 50, handler: b, defaults: { dragMinDistance: 10, dragDistanceCorrection: !0, dragMaxTouches: 1, dragBlockHorizontal: !1, dragBlockVertical: !1, dragLockToAxis: !1, dragLockMinDistance: 25 } } }("drag"), d.gestures.Gesture = { name: "gesture", index: 1337, handler: function (a, b) { b.trigger(this.name, a) } }, function (a) { function b(b, d) { var e = d.options, f = u.current; switch (b.eventType) { case m: clearTimeout(c), f.name = a, c = setTimeout(function () { f && f.name == a && d.trigger(a, b) }, e.holdTimeout); break; case n: b.distance > e.holdThreshold && clearTimeout(c); break; case p: clearTimeout(c) } } var c; d.gestures.Hold = { name: a, index: 10, defaults: { holdTimeout: 500, holdThreshold: 2 }, handler: b } }("hold"), d.gestures.Release = { name: "release", index: 1 / 0, handler: function (a, b) { a.eventType == p && b.trigger(this.name, a) } }, d.gestures.Swipe = { name: "swipe", index: 40, defaults: { swipeMinTouches: 1, swipeMaxTouches: 1, swipeVelocityX: .7, swipeVelocityY: .6 }, handler: function (a, b) { if (a.eventType == p) { var c = a.touches.length, d = b.options; if (c < d.swipeMinTouches || c > d.swipeMaxTouches) return; (a.velocityX > d.swipeVelocityX || a.velocityY > d.swipeVelocityY) && (b.trigger(this.name, a), b.trigger(this.name + a.direction, a)) } } }, function (a) { function b(b, d) { var e, f, g = d.options, h = u.current, i = u.previous; switch (b.eventType) { case m: c = !1; break; case n: c = c || b.distance > g.tapMaxDistance; break; case o: "touchcancel" != b.srcEvent.type && b.deltaTime < g.tapMaxTime && !c && (e = i && i.lastEvent && b.timeStamp - i.lastEvent.timeStamp, f = !1, i && i.name == a && e && e < g.doubleTapInterval && b.distance < g.doubleTapDistance && (d.trigger("doubletap", b), f = !0), (!f || g.tapAlways) && (h.name = a, d.trigger(h.name, b))) } } var c = !1; d.gestures.Tap = { name: a, index: 100, handler: b, defaults: { tapMaxTime: 250, tapMaxDistance: 10, tapAlways: !0, doubleTapDistance: 20, doubleTapInterval: 300 } } }("tap"), d.gestures.Touch = { name: "touch", index: -1 / 0, defaults: { preventDefault: !1, preventMouse: !1 }, handler: function (a, b) { return b.options.preventMouse && a.pointerType == j ? void a.stopDetect() : (b.options.preventDefault && a.preventDefault(), void (a.eventType == q && b.trigger("touch", a))) } }, function (a) { function b(b, d) { switch (b.eventType) { case m: c = !1; break; case n: if (b.touches.length < 2) return; var e = Math.abs(1 - b.scale), f = Math.abs(b.rotation); if (e < d.options.transformMinScale && f < d.options.transformMinRotation) return; u.current.name = a, c || (d.trigger(a + "start", b), c = !0), d.trigger(a, b), f > d.options.transformMinRotation && d.trigger("rotate", b), e > d.options.transformMinScale && (d.trigger("pinch", b), d.trigger("pinch" + (b.scale < 1 ? "in" : "out"), b)); break; case p: c && b.changedLength < 2 && (d.trigger(a + "end", b), c = !1) } } var c = !1; d.gestures.Transform = { name: a, index: 45, defaults: { transformMinScale: .01, transformMinRotation: 1 }, handler: b } }("transform"), "function" == typeof define && define.amd ? define(function () { return d }) : "undefined" != typeof module && module.exports ? module.exports = d : a.Hammer = d }(window);


/* Copyright (c) 2012, 2014 Hyeonje Alex Jun and other contributors
 * Licensed under the MIT License
 */
(function (factory) {
	'use strict';

	if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define(['jquery'], factory);
	} else if (typeof exports === 'object') {
		// Node/CommonJS
		factory(require('jquery'));
	} else {
		// Browser globals
		factory(jQuery);
	}
}(function ($) {
	'use strict';

	// The default settings for the plugin
	var defaultSettings = {
		wheelSpeed: 10,
		wheelPropagation: false,
		minScrollbarLength: null,
		useBothWheelAxes: false,
		useKeyboard: true,
		suppressScrollX: false,
		suppressScrollY: false,
		scrollXMarginOffset: 0,
		scrollYMarginOffset: 0,
		includePadding: false
	};

	var getEventClassName = (function () {
		var incrementingId = 0;
		return function () {
			var id = incrementingId;
			incrementingId += 1;
			return '.perfect-scrollbar-' + id;
		};
	}());

	$.fn.perfectScrollbar = function (suppliedSettings, option) {

		return this.each(function () {
			// Use the default settings
			var settings = $.extend(true, {}, defaultSettings),
				$this = $(this);

			if (typeof suppliedSettings === "object") {
				// But over-ride any supplied
				$.extend(true, settings, suppliedSettings);
			} else {
				// If no settings were supplied, then the first param must be the option
				option = suppliedSettings;
			}

			// Catch options

			if (option === 'update') {
				if ($this.data('perfect-scrollbar-update')) {
					$this.data('perfect-scrollbar-update')();
				}
				return $this;
			}
			else if (option === 'destroy') {
				if ($this.data('perfect-scrollbar-destroy')) {
					$this.data('perfect-scrollbar-destroy')();
				}
				return $this;
			}

			if ($this.data('perfect-scrollbar')) {
				// if there's already perfect-scrollbar
				return $this.data('perfect-scrollbar');
			}


			// Or generate new perfectScrollbar

			// Set class to the container
			$this.addClass('ps-container');

			var $scrollbarXRail = $("<div class='ps-scrollbar-x-rail'></div>").appendTo($this),
				$scrollbarYRail = $("<div class='ps-scrollbar-y-rail'></div>").appendTo($this),
				$scrollbarX = $("<div class='ps-scrollbar-x'></div>").appendTo($scrollbarXRail),
				$scrollbarY = $("<div class='ps-scrollbar-y'></div>").appendTo($scrollbarYRail),
				scrollbarXActive,
				scrollbarYActive,
				containerWidth,
				containerHeight,
				contentWidth,
				contentHeight,
				scrollbarXWidth,
				scrollbarXLeft,
				scrollbarXBottom = parseInt($scrollbarXRail.css('bottom'), 10),
				isScrollbarXUsingBottom = scrollbarXBottom === scrollbarXBottom, // !isNaN
				scrollbarXTop = isScrollbarXUsingBottom ? null : parseInt($scrollbarXRail.css('top'), 10),
				scrollbarYHeight,
				scrollbarYTop,
				scrollbarYRight = parseInt($scrollbarYRail.css('right'), 10),
				isScrollbarYUsingRight = scrollbarYRight === scrollbarYRight, // !isNaN
				scrollbarYLeft = isScrollbarYUsingRight ? null : parseInt($scrollbarYRail.css('left'), 10),
				isRtl = $this.css('direction') === "rtl",
				eventClassName = getEventClassName();

			var updateContentScrollTop = function (currentTop, deltaY) {
				var newTop = currentTop + deltaY,
					maxTop = containerHeight - scrollbarYHeight;

				if (newTop < 0) {
					scrollbarYTop = 0;
				}
				else if (newTop > maxTop) {
					scrollbarYTop = maxTop;
				}
				else {
					scrollbarYTop = newTop;
				}

				var scrollTop = parseInt(scrollbarYTop * (contentHeight - containerHeight) / (containerHeight - scrollbarYHeight), 10);
				$this.scrollTop(scrollTop);

				if (isScrollbarXUsingBottom) {
					$scrollbarXRail.css({ bottom: scrollbarXBottom - scrollTop });
				} else {
					$scrollbarXRail.css({ top: scrollbarXTop + scrollTop });
				}
			};

			var updateContentScrollLeft = function (currentLeft, deltaX) {
				var newLeft = currentLeft + deltaX,
					maxLeft = containerWidth - scrollbarXWidth;

				if (newLeft < 0) {
					scrollbarXLeft = 0;
				}
				else if (newLeft > maxLeft) {
					scrollbarXLeft = maxLeft;
				}
				else {
					scrollbarXLeft = newLeft;
				}

				var scrollLeft = parseInt(scrollbarXLeft * (contentWidth - containerWidth) / (containerWidth - scrollbarXWidth), 10);
				$this.scrollLeft(scrollLeft);

				if (isScrollbarYUsingRight) {
					$scrollbarYRail.css({ right: scrollbarYRight - scrollLeft });
				} else {
					$scrollbarYRail.css({ left: scrollbarYLeft + scrollLeft });
				}
			};

			var getSettingsAdjustedThumbSize = function (thumbSize) {
				if (settings.minScrollbarLength) {
					thumbSize = Math.max(thumbSize, settings.minScrollbarLength);
				}
				return thumbSize;
			};

			var updateScrollbarCss = function () {
				var scrollbarXStyles = { width: containerWidth, display: scrollbarXActive ? "inherit" : "none" };
				if (isRtl) {
					scrollbarXStyles.left = $this.scrollLeft() + containerWidth - contentWidth;
				} else {
					scrollbarXStyles.left = $this.scrollLeft();
				}
				if (isScrollbarXUsingBottom) {
					scrollbarXStyles.bottom = scrollbarXBottom - $this.scrollTop();
				} else {
					scrollbarXStyles.top = scrollbarXTop + $this.scrollTop();
				}
				$scrollbarXRail.css(scrollbarXStyles);

				var scrollbarYStyles = { top: $this.scrollTop(), height: containerHeight, display: scrollbarYActive ? "inherit" : "none" };

				if (isScrollbarYUsingRight) {
					if (isRtl) {
						scrollbarYStyles.right = contentWidth - $this.scrollLeft() - scrollbarYRight - $scrollbarY.outerWidth();
					} else {
						scrollbarYStyles.right = scrollbarYRight - $this.scrollLeft();
					}
				} else {
					if (isRtl) {
						scrollbarYStyles.left = $this.scrollLeft() + containerWidth * 2 - contentWidth - scrollbarYLeft - $scrollbarY.outerWidth();
					} else {
						scrollbarYStyles.left = scrollbarYLeft + $this.scrollLeft();
					}
				}
				$scrollbarYRail.css(scrollbarYStyles);

				$scrollbarX.css({ left: scrollbarXLeft, width: scrollbarXWidth });
				$scrollbarY.css({ top: scrollbarYTop, height: scrollbarYHeight });
			};

			var updateBarSizeAndPosition = function () {
				containerWidth = settings.includePadding ? $this.innerWidth() : $this.width();
				containerHeight = settings.includePadding ? $this.innerHeight() : $this.height();
				contentWidth = $this.prop('scrollWidth');
				contentHeight = $this.prop('scrollHeight');

				if (!settings.suppressScrollX && containerWidth + settings.scrollXMarginOffset < contentWidth) {
					scrollbarXActive = true;
					scrollbarXWidth = getSettingsAdjustedThumbSize(parseInt(containerWidth * containerWidth / contentWidth, 10));
					scrollbarXLeft = parseInt($this.scrollLeft() * (containerWidth - scrollbarXWidth) / (contentWidth - containerWidth), 10);
				}
				else {
					scrollbarXActive = false;
					scrollbarXWidth = 0;
					scrollbarXLeft = 0;
					$this.scrollLeft(0);
				}

				if (!settings.suppressScrollY && containerHeight + settings.scrollYMarginOffset < contentHeight) {
					scrollbarYActive = true;
					scrollbarYHeight = getSettingsAdjustedThumbSize(parseInt(containerHeight * containerHeight / contentHeight, 10));
					scrollbarYTop = parseInt($this.scrollTop() * (containerHeight - scrollbarYHeight) / (contentHeight - containerHeight), 10);
				}
				else {
					scrollbarYActive = false;
					scrollbarYHeight = 0;
					scrollbarYTop = 0;
					$this.scrollTop(0);
				}

				if (scrollbarYTop >= containerHeight - scrollbarYHeight) {
					scrollbarYTop = containerHeight - scrollbarYHeight;
				}
				if (scrollbarXLeft >= containerWidth - scrollbarXWidth) {
					scrollbarXLeft = containerWidth - scrollbarXWidth;
				}

				updateScrollbarCss();
			};

			var bindMouseScrollXHandler = function () {
				var currentLeft,
					currentPageX;

				$scrollbarX.bind('mousedown' + eventClassName, function (e) {
					currentPageX = e.pageX;
					currentLeft = $scrollbarX.position().left;
					$scrollbarXRail.addClass('in-scrolling');
					e.stopPropagation();
					e.preventDefault();
				});

				$(document).bind('mousemove' + eventClassName, function (e) {
					if ($scrollbarXRail.hasClass('in-scrolling')) {
						updateContentScrollLeft(currentLeft, e.pageX - currentPageX);
						e.stopPropagation();
						e.preventDefault();
					}
				});

				$(document).bind('mouseup' + eventClassName, function (e) {
					if ($scrollbarXRail.hasClass('in-scrolling')) {
						$scrollbarXRail.removeClass('in-scrolling');
					}
				});

				currentLeft =
				currentPageX = null;
			};

			var bindMouseScrollYHandler = function () {
				var currentTop,
					currentPageY;

				$scrollbarY.bind('mousedown' + eventClassName, function (e) {
					currentPageY = e.pageY;
					currentTop = $scrollbarY.position().top;
					$scrollbarYRail.addClass('in-scrolling');
					e.stopPropagation();
					e.preventDefault();
				});

				$(document).bind('mousemove' + eventClassName, function (e) {
					if ($scrollbarYRail.hasClass('in-scrolling')) {
						updateContentScrollTop(currentTop, e.pageY - currentPageY);
						e.stopPropagation();
						e.preventDefault();
					}
				});

				$(document).bind('mouseup' + eventClassName, function (e) {
					if ($scrollbarYRail.hasClass('in-scrolling')) {
						$scrollbarYRail.removeClass('in-scrolling');
					}
				});

				currentTop =
				currentPageY = null;
			};

			// check if the default scrolling should be prevented.
			var shouldPreventDefault = function (deltaX, deltaY) {
				var scrollTop = $this.scrollTop();
				if (deltaX === 0) {
					if (!scrollbarYActive) {
						return false;
					}
					if ((scrollTop === 0 && deltaY > 0) || (scrollTop >= contentHeight - containerHeight && deltaY < 0)) {
						return !settings.wheelPropagation;
					}
				}

				var scrollLeft = $this.scrollLeft();
				if (deltaY === 0) {
					if (!scrollbarXActive) {
						return false;
					}
					if ((scrollLeft === 0 && deltaX < 0) || (scrollLeft >= contentWidth - containerWidth && deltaX > 0)) {
						return !settings.wheelPropagation;
					}
				}
				return true;
			};

			// bind handlers
			var bindMouseWheelHandler = function () {
				// FIXME: Backward compatibility.
				// After e.deltaFactor applied, wheelSpeed should have smaller value.
				// Currently, there's no way to change the settings after the scrollbar initialized.
				// But if the way is implemented in the future, wheelSpeed should be reset.
				settings.wheelSpeed /= 10;

				var shouldPrevent = false;
				$this.bind('mousewheel' + eventClassName, function (e, deprecatedDelta, deprecatedDeltaX, deprecatedDeltaY) {
					var deltaX = e.deltaX * e.deltaFactor || deprecatedDeltaX,
						deltaY = e.deltaY * e.deltaFactor || deprecatedDeltaY;

					shouldPrevent = false;
					if (!settings.useBothWheelAxes) {
						// deltaX will only be used for horizontal scrolling and deltaY will
						// only be used for vertical scrolling - this is the default
						$this.scrollTop($this.scrollTop() - (deltaY * settings.wheelSpeed));
						$this.scrollLeft($this.scrollLeft() + (deltaX * settings.wheelSpeed));
					} else if (scrollbarYActive && !scrollbarXActive) {
						// only vertical scrollbar is active and useBothWheelAxes option is
						// active, so let's scroll vertical bar using both mouse wheel axes
						if (deltaY) {
							$this.scrollTop($this.scrollTop() - (deltaY * settings.wheelSpeed));
						} else {
							$this.scrollTop($this.scrollTop() + (deltaX * settings.wheelSpeed));
						}
						shouldPrevent = true;
					} else if (scrollbarXActive && !scrollbarYActive) {
						// useBothWheelAxes and only horizontal bar is active, so use both
						// wheel axes for horizontal bar
						if (deltaX) {
							$this.scrollLeft($this.scrollLeft() + (deltaX * settings.wheelSpeed));
						} else {
							$this.scrollLeft($this.scrollLeft() - (deltaY * settings.wheelSpeed));
						}
						shouldPrevent = true;
					}

					// update bar position
					updateBarSizeAndPosition();

					shouldPrevent = (shouldPrevent || shouldPreventDefault(deltaX, deltaY));
					if (shouldPrevent) {
						e.stopPropagation();
						e.preventDefault();
					}
				});

				// fix Firefox scroll problem
				$this.bind('MozMousePixelScroll' + eventClassName, function (e) {
					if (shouldPrevent) {
						e.preventDefault();
					}
				});
			};

			var bindKeyboardHandler = function () {
				var hovered = false;
				$this.bind('mouseenter' + eventClassName, function (e) {
					hovered = true;
				});
				$this.bind('mouseleave' + eventClassName, function (e) {
					hovered = false;
				});

				var shouldPrevent = false;
				$(document).bind('keydown' + eventClassName, function (e) {
					if (!hovered || $(document.activeElement).is(":input,[contenteditable]")) {
						return;
					}

					var deltaX = 0,
						deltaY = 0;

					switch (e.which) {
						case 37: // left
							deltaX = -30;
							break;
						case 38: // up
							deltaY = 30;
							break;
						case 39: // right
							deltaX = 30;
							break;
						case 40: // down
							deltaY = -30;
							break;
						case 33: // page up
							deltaY = 90;
							break;
						case 32: // space bar
						case 34: // page down
							deltaY = -90;
							break;
						case 35: // end
							deltaY = -containerHeight;
							break;
						case 36: // home
							deltaY = containerHeight;
							break;
						default:
							return;
					}

					$this.scrollTop($this.scrollTop() - deltaY);
					$this.scrollLeft($this.scrollLeft() + deltaX);

					shouldPrevent = shouldPreventDefault(deltaX, deltaY);
					if (shouldPrevent) {
						e.preventDefault();
					}
				});
			};

			var bindRailClickHandler = function () {
				var stopPropagation = function (e) { e.stopPropagation(); };

				$scrollbarY.bind('click' + eventClassName, stopPropagation);
				$scrollbarYRail.bind('click' + eventClassName, function (e) {
					var halfOfScrollbarLength = parseInt(scrollbarYHeight / 2, 10),
						positionTop = e.pageY - $scrollbarYRail.offset().top - halfOfScrollbarLength,
						maxPositionTop = containerHeight - scrollbarYHeight,
						positionRatio = positionTop / maxPositionTop;

					if (positionRatio < 0) {
						positionRatio = 0;
					} else if (positionRatio > 1) {
						positionRatio = 1;
					}

					$this.scrollTop((contentHeight - containerHeight) * positionRatio);
				});

				$scrollbarX.bind('click' + eventClassName, stopPropagation);
				$scrollbarXRail.bind('click' + eventClassName, function (e) {
					var halfOfScrollbarLength = parseInt(scrollbarXWidth / 2, 10),
						positionLeft = e.pageX - $scrollbarXRail.offset().left - halfOfScrollbarLength,
						maxPositionLeft = containerWidth - scrollbarXWidth,
						positionRatio = positionLeft / maxPositionLeft;

					if (positionRatio < 0) {
						positionRatio = 0;
					} else if (positionRatio > 1) {
						positionRatio = 1;
					}

					$this.scrollLeft((contentWidth - containerWidth) * positionRatio);
				});
			};

			// bind mobile touch handler
			var bindMobileTouchHandler = function () {
				var applyTouchMove = function (differenceX, differenceY) {
					$this.scrollTop($this.scrollTop() - differenceY);
					$this.scrollLeft($this.scrollLeft() - differenceX);

					// update bar position
					updateBarSizeAndPosition();
				};

				var startCoords = {},
					startTime = 0,
					speed = {},
					breakingProcess = null,
					inGlobalTouch = false;

				$(window).bind("touchstart" + eventClassName, function (e) {
					inGlobalTouch = true;
				});
				$(window).bind("touchend" + eventClassName, function (e) {
					inGlobalTouch = false;
				});

				$this.bind("touchstart" + eventClassName, function (e) {
					var touch = e.originalEvent.targetTouches[0];

					startCoords.pageX = touch.pageX;
					startCoords.pageY = touch.pageY;

					startTime = (new Date()).getTime();

					if (breakingProcess !== null) {
						clearInterval(breakingProcess);
					}

					e.stopPropagation();
				});
				$this.bind("touchmove" + eventClassName, function (e) {
					if (!inGlobalTouch && e.originalEvent.targetTouches.length === 1) {
						var touch = e.originalEvent.targetTouches[0];

						var currentCoords = {};
						currentCoords.pageX = touch.pageX;
						currentCoords.pageY = touch.pageY;

						var differenceX = currentCoords.pageX - startCoords.pageX,
						  differenceY = currentCoords.pageY - startCoords.pageY;

						applyTouchMove(differenceX, differenceY);
						startCoords = currentCoords;

						var currentTime = (new Date()).getTime();

						var timeGap = currentTime - startTime;
						if (timeGap > 0) {
							speed.x = differenceX / timeGap;
							speed.y = differenceY / timeGap;
							startTime = currentTime;
						}

						e.preventDefault();
					}
				});
				$this.bind("touchend" + eventClassName, function (e) {
					clearInterval(breakingProcess);
					breakingProcess = setInterval(function () {
						if (Math.abs(speed.x) < 0.01 && Math.abs(speed.y) < 0.01) {
							clearInterval(breakingProcess);
							return;
						}

						applyTouchMove(speed.x * 30, speed.y * 30);

						speed.x *= 0.8;
						speed.y *= 0.8;
					}, 10);
				});
			};

			var bindScrollHandler = function () {
				$this.bind('scroll' + eventClassName, function (e) {
					updateBarSizeAndPosition();
				});
			};

			var destroy = function () {
				$this.unbind(eventClassName);
				$(window).unbind(eventClassName);
				$(document).unbind(eventClassName);
				$this.data('perfect-scrollbar', null);
				$this.data('perfect-scrollbar-update', null);
				$this.data('perfect-scrollbar-destroy', null);
				$scrollbarX.remove();
				$scrollbarY.remove();
				$scrollbarXRail.remove();
				$scrollbarYRail.remove();

				// clean all variables
				$scrollbarXRail =
				$scrollbarYRail =
				$scrollbarX =
				$scrollbarY =
				scrollbarXActive =
				scrollbarYActive =
				containerWidth =
				containerHeight =
				contentWidth =
				contentHeight =
				scrollbarXWidth =
				scrollbarXLeft =
				scrollbarXBottom =
				isScrollbarXUsingBottom =
				scrollbarXTop =
				scrollbarYHeight =
				scrollbarYTop =
				scrollbarYRight =
				isScrollbarYUsingRight =
				scrollbarYLeft =
				isRtl =
				eventClassName = null;
			};

			var ieSupport = function (version) {
				$this.addClass('ie').addClass('ie' + version);

				var bindHoverHandlers = function () {
					var mouseenter = function () {
						$(this).addClass('hover');
					};
					var mouseleave = function () {
						$(this).removeClass('hover');
					};
					$this.bind('mouseenter' + eventClassName, mouseenter).bind('mouseleave' + eventClassName, mouseleave);
					$scrollbarXRail.bind('mouseenter' + eventClassName, mouseenter).bind('mouseleave' + eventClassName, mouseleave);
					$scrollbarYRail.bind('mouseenter' + eventClassName, mouseenter).bind('mouseleave' + eventClassName, mouseleave);
					$scrollbarX.bind('mouseenter' + eventClassName, mouseenter).bind('mouseleave' + eventClassName, mouseleave);
					$scrollbarY.bind('mouseenter' + eventClassName, mouseenter).bind('mouseleave' + eventClassName, mouseleave);
				};

				var fixIe6ScrollbarPosition = function () {
					updateScrollbarCss = function () {
						var scrollbarXStyles = { left: scrollbarXLeft + $this.scrollLeft(), width: scrollbarXWidth };
						if (isScrollbarXUsingBottom) {
							scrollbarXStyles.bottom = scrollbarXBottom;
						} else {
							scrollbarXStyles.top = scrollbarXTop;
						}
						$scrollbarX.css(scrollbarXStyles);

						var scrollbarYStyles = { top: scrollbarYTop + $this.scrollTop(), height: scrollbarYHeight };
						if (isScrollbarYUsingRight) {
							scrollbarYStyles.right = scrollbarYRight;
						} else {
							scrollbarYStyles.left = scrollbarYLeft;
						}

						$scrollbarY.css(scrollbarYStyles);
						$scrollbarX.hide().show();
						$scrollbarY.hide().show();
					};
				};

				if (version === 6) {
					bindHoverHandlers();
					fixIe6ScrollbarPosition();
				}
			};

			var supportsTouch = (('ontouchstart' in window) || window.DocumentTouch && document instanceof window.DocumentTouch);

			var initialize = function () {
				var ieMatch = navigator.userAgent.toLowerCase().match(/(msie) ([\w.]+)/);
				if (ieMatch && ieMatch[1] === 'msie') {
					// must be executed at first, because 'ieSupport' may addClass to the container
					ieSupport(parseInt(ieMatch[2], 10));
				}

				updateBarSizeAndPosition();
				bindScrollHandler();
				bindMouseScrollXHandler();
				bindMouseScrollYHandler();
				bindRailClickHandler();
				if (supportsTouch) {
					bindMobileTouchHandler();
				}
				if ($this.mousewheel) {
					bindMouseWheelHandler();
				}
				if (settings.useKeyboard) {
					bindKeyboardHandler();
				}
				$this.data('perfect-scrollbar', $this);
				$this.data('perfect-scrollbar-update', updateBarSizeAndPosition);
				$this.data('perfect-scrollbar-destroy', destroy);
			};

			// initialize
			initialize();

			return $this;
		});
	};
}));
