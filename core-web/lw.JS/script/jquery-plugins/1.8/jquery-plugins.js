/*!jquery plygins 1.8 */

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
/* Copyright (c) 2012, 2014 Hyunje Alex Jun and other contributors
 * Licensed under the MIT License ver: 0.5.8
 * 
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
})(function ($) {
    'use strict';

    function getInt(x) {
        if (typeof x === 'string') {
            return parseInt(x, 10);
        } else {
            return ~~x;
        }
    }

    var defaultSettings = {
        wheelSpeed: 1,
        wheelPropagation: false,
        swipePropagation: true,
        minScrollbarLength: null,
        maxScrollbarLength: null,
        useBothWheelAxes: false,
        useKeyboard: true,
        suppressScrollX: false,
        suppressScrollY: false,
        scrollXMarginOffset: 0,
        scrollYMarginOffset: 0,
        includePadding: false
    };

    var incrementingId = 0;
    var eventClassFactory = function () {
        var id = incrementingId++;
        return function (eventName) {
            var className = '.perfect-scrollbar-' + id;
            if (typeof eventName === 'undefined') {
                return className;
            } else {
                return eventName + className;
            }
        };
    };

    var isWebkit = 'WebkitAppearance' in document.documentElement.style;

    $.fn.perfectScrollbar = function (suppliedSettings, option) {

        return this.each(function () {
            var settings = $.extend(true, {}, defaultSettings);
            var $this = $(this);
            var isPluginAlive = function () { return !!$this; };

            if (typeof suppliedSettings === "object") {
                // Override default settings with any supplied
                $.extend(true, settings, suppliedSettings);
            } else {
                // If no setting was supplied, then the first param must be the option
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

            $this.addClass('ps-container');

            var containerWidth;
            var containerHeight;
            var contentWidth;
            var contentHeight;

            var isRtl = $this.css('direction') === "rtl";
            var eventClass = eventClassFactory();
            var ownerDocument = this.ownerDocument || document;

            var $scrollbarXRail = $("<div class='ps-scrollbar-x-rail'>").appendTo($this);
            var $scrollbarX = $("<div class='ps-scrollbar-x'>").appendTo($scrollbarXRail);
            var scrollbarXActive;
            var scrollbarXWidth;
            var scrollbarXLeft;
            var scrollbarXBottom = getInt($scrollbarXRail.css('bottom'));
            var isScrollbarXUsingBottom = scrollbarXBottom === scrollbarXBottom; // !isNaN
            var scrollbarXTop = isScrollbarXUsingBottom ? null : getInt($scrollbarXRail.css('top'));
            var railBorderXWidth = getInt($scrollbarXRail.css('borderLeftWidth')) + getInt($scrollbarXRail.css('borderRightWidth'));
            var railXMarginWidth = getInt($scrollbarXRail.css('marginLeft')) + getInt($scrollbarXRail.css('marginRight'));
            var railXWidth;

            var $scrollbarYRail = $("<div class='ps-scrollbar-y-rail'>").appendTo($this);
            var $scrollbarY = $("<div class='ps-scrollbar-y'>").appendTo($scrollbarYRail);
            var scrollbarYActive;
            var scrollbarYHeight;
            var scrollbarYTop;
            var scrollbarYRight = getInt($scrollbarYRail.css('right'));
            var isScrollbarYUsingRight = scrollbarYRight === scrollbarYRight; // !isNaN
            var scrollbarYLeft = isScrollbarYUsingRight ? null : getInt($scrollbarYRail.css('left'));
            var railBorderYWidth = getInt($scrollbarYRail.css('borderTopWidth')) + getInt($scrollbarYRail.css('borderBottomWidth'));
            var railYMarginHeight = getInt($scrollbarYRail.css('marginTop')) + getInt($scrollbarYRail.css('marginBottom'));
            var railYHeight;

            function updateScrollTop(currentTop, deltaY) {
                var newTop = currentTop + deltaY;
                var maxTop = containerHeight - scrollbarYHeight;

                if (newTop < 0) {
                    scrollbarYTop = 0;
                } else if (newTop > maxTop) {
                    scrollbarYTop = maxTop;
                } else {
                    scrollbarYTop = newTop;
                }

                var scrollTop = getInt(scrollbarYTop * (contentHeight - containerHeight) / (containerHeight - scrollbarYHeight));
                $this.scrollTop(scrollTop);
            }

            function updateScrollLeft(currentLeft, deltaX) {
                var newLeft = currentLeft + deltaX;
                var maxLeft = containerWidth - scrollbarXWidth;

                if (newLeft < 0) {
                    scrollbarXLeft = 0;
                } else if (newLeft > maxLeft) {
                    scrollbarXLeft = maxLeft;
                } else {
                    scrollbarXLeft = newLeft;
                }

                var scrollLeft = getInt(scrollbarXLeft * (contentWidth - containerWidth) / (containerWidth - scrollbarXWidth));
                $this.scrollLeft(scrollLeft);
            }

            function getThumbSize(thumbSize) {
                if (settings.minScrollbarLength) {
                    thumbSize = Math.max(thumbSize, settings.minScrollbarLength);
                }
                if (settings.maxScrollbarLength) {
                    thumbSize = Math.min(thumbSize, settings.maxScrollbarLength);
                }
                return thumbSize;
            }

            function updateCss() {
                var xRailOffset = { width: railXWidth };
                if (isRtl) {
                    xRailOffset.left = $this.scrollLeft() + containerWidth - contentWidth;
                } else {
                    xRailOffset.left = $this.scrollLeft();
                }
                if (isScrollbarXUsingBottom) {
                    xRailOffset.bottom = scrollbarXBottom - $this.scrollTop();
                } else {
                    xRailOffset.top = scrollbarXTop + $this.scrollTop();
                }
                $scrollbarXRail.css(xRailOffset);

                var railYOffset = { top: $this.scrollTop(), height: railYHeight };

                if (isScrollbarYUsingRight) {
                    if (isRtl) {
                        railYOffset.right = contentWidth - $this.scrollLeft() - scrollbarYRight - $scrollbarY.outerWidth();
                    } else {
                        railYOffset.right = scrollbarYRight - $this.scrollLeft();
                    }
                } else {
                    if (isRtl) {
                        railYOffset.left = $this.scrollLeft() + containerWidth * 2 - contentWidth - scrollbarYLeft - $scrollbarY.outerWidth();
                    } else {
                        railYOffset.left = scrollbarYLeft + $this.scrollLeft();
                    }
                }
                $scrollbarYRail.css(railYOffset);

                $scrollbarX.css({ left: scrollbarXLeft, width: scrollbarXWidth - railBorderXWidth });
                $scrollbarY.css({ top: scrollbarYTop, height: scrollbarYHeight - railBorderYWidth });
            }

            function updateGeometry() {
                // Hide scrollbars not to affect scrollWidth and scrollHeight
                $this.removeClass('ps-active-x');
                $this.removeClass('ps-active-y');

                containerWidth = settings.includePadding ? $this.innerWidth() : $this.width();
                containerHeight = settings.includePadding ? $this.innerHeight() : $this.height();
                contentWidth = $this.prop('scrollWidth');
                contentHeight = $this.prop('scrollHeight');

                if (!settings.suppressScrollX && containerWidth + settings.scrollXMarginOffset < contentWidth) {
                    scrollbarXActive = true;
                    railXWidth = containerWidth - railXMarginWidth;
                    scrollbarXWidth = getThumbSize(getInt(railXWidth * containerWidth / contentWidth));
                    scrollbarXLeft = getInt($this.scrollLeft() * (railXWidth - scrollbarXWidth) / (contentWidth - containerWidth));
                } else {
                    scrollbarXActive = false;
                    scrollbarXWidth = 0;
                    scrollbarXLeft = 0;
                    $this.scrollLeft(0);
                }

                if (!settings.suppressScrollY && containerHeight + settings.scrollYMarginOffset < contentHeight) {
                    scrollbarYActive = true;
                    railYHeight = containerHeight - railYMarginHeight;
                    scrollbarYHeight = getThumbSize(getInt(railYHeight * containerHeight / contentHeight));
                    scrollbarYTop = getInt($this.scrollTop() * (railYHeight - scrollbarYHeight) / (contentHeight - containerHeight));
                } else {
                    scrollbarYActive = false;
                    scrollbarYHeight = 0;
                    scrollbarYTop = 0;
                    $this.scrollTop(0);
                }

                if (scrollbarXLeft >= railXWidth - scrollbarXWidth) {
                    scrollbarXLeft = railXWidth - scrollbarXWidth;
                }
                if (scrollbarYTop >= railYHeight - scrollbarYHeight) {
                    scrollbarYTop = railYHeight - scrollbarYHeight;
                }

                updateCss();

                if (scrollbarXActive) {
                    $this.addClass('ps-active-x');
                }
                if (scrollbarYActive) {
                    $this.addClass('ps-active-y');
                }
            }

            function bindMouseScrollXHandler() {
                var currentLeft;
                var currentPageX;

                var mouseMoveHandler = function (e) {
                    updateScrollLeft(currentLeft, e.pageX - currentPageX);
                    updateGeometry();
                    e.stopPropagation();
                    e.preventDefault();
                };

                var mouseUpHandler = function (e) {
                    $scrollbarXRail.removeClass('in-scrolling');
                    $(ownerDocument).unbind(eventClass('mousemove'), mouseMoveHandler);
                };

                $scrollbarX.bind(eventClass('mousedown'), function (e) {
                    currentPageX = e.pageX;
                    currentLeft = $scrollbarX.position().left;
                    $scrollbarXRail.addClass('in-scrolling');

                    $(ownerDocument).bind(eventClass('mousemove'), mouseMoveHandler);
                    $(ownerDocument).one(eventClass('mouseup'), mouseUpHandler);

                    e.stopPropagation();
                    e.preventDefault();
                });

                currentLeft =
                currentPageX = null;
            }

            function bindMouseScrollYHandler() {
                var currentTop;
                var currentPageY;

                var mouseMoveHandler = function (e) {
                    updateScrollTop(currentTop, e.pageY - currentPageY);
                    updateGeometry();
                    e.stopPropagation();
                    e.preventDefault();
                };

                var mouseUpHandler = function (e) {
                    $scrollbarYRail.removeClass('in-scrolling');
                    $(ownerDocument).unbind(eventClass('mousemove'), mouseMoveHandler);
                };

                $scrollbarY.bind(eventClass('mousedown'), function (e) {
                    currentPageY = e.pageY;
                    currentTop = $scrollbarY.position().top;
                    $scrollbarYRail.addClass('in-scrolling');

                    $(ownerDocument).bind(eventClass('mousemove'), mouseMoveHandler);
                    $(ownerDocument).one(eventClass('mouseup'), mouseUpHandler);

                    e.stopPropagation();
                    e.preventDefault();
                });

                currentTop =
                currentPageY = null;
            }

            function shouldPreventWheel(deltaX, deltaY) {
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
            }

            function shouldPreventSwipe(deltaX, deltaY) {
                var scrollTop = $this.scrollTop();
                var scrollLeft = $this.scrollLeft();
                var magnitudeX = Math.abs(deltaX);
                var magnitudeY = Math.abs(deltaY);

                if (magnitudeY > magnitudeX) {
                    // user is perhaps trying to swipe up/down the page

                    if (((deltaY < 0) && (scrollTop === contentHeight - containerHeight)) ||
                        ((deltaY > 0) && (scrollTop === 0))) {
                        return !settings.swipePropagation;
                    }
                } else if (magnitudeX > magnitudeY) {
                    // user is perhaps trying to swipe left/right across the page

                    if (((deltaX < 0) && (scrollLeft === contentWidth - containerWidth)) ||
                        ((deltaX > 0) && (scrollLeft === 0))) {
                        return !settings.swipePropagation;
                    }
                }

                return true;
            }

            function bindMouseWheelHandler() {
                var shouldPrevent = false;

                function getDeltaFromEvent(e) {
                    var deltaX = e.originalEvent.deltaX;
                    var deltaY = -1 * e.originalEvent.deltaY;

                    if (typeof deltaX === "undefined" || typeof deltaY === "undefined") {
                        // OS X Safari
                        deltaX = -1 * e.originalEvent.wheelDeltaX / 6;
                        deltaY = e.originalEvent.wheelDeltaY / 6;
                    }

                    if (e.originalEvent.deltaMode && e.originalEvent.deltaMode === 1) {
                        // Firefox in deltaMode 1: Line scrolling
                        deltaX *= 10;
                        deltaY *= 10;
                    }

                    if (deltaX !== deltaX && deltaY !== deltaY/* NaN checks */) {
                        // IE in some mouse drivers
                        deltaX = 0;
                        deltaY = e.originalEvent.wheelDelta;
                    }

                    return [deltaX, deltaY];
                }

                function mousewheelHandler(e) {
                    // FIXME: this is a quick fix for the select problem in FF and IE.
                    // If there comes an effective way to deal with the problem,
                    // this lines should be removed.
                    if (!isWebkit && $this.find('select:focus').length > 0) {
                        return;
                    }

                    var delta = getDeltaFromEvent(e);

                    var deltaX = delta[0];
                    var deltaY = delta[1];

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

                    updateGeometry();

                    shouldPrevent = (shouldPrevent || shouldPreventWheel(deltaX, deltaY));
                    if (shouldPrevent) {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                }

                if (typeof window.onwheel !== "undefined") {
                    $this.bind(eventClass('wheel'), mousewheelHandler);
                } else if (typeof window.onmousewheel !== "undefined") {
                    $this.bind(eventClass('mousewheel'), mousewheelHandler);
                }
            }

            function bindKeyboardHandler() {
                var hovered = false;
                $this.bind(eventClass('mouseenter'), function (e) {
                    hovered = true;
                });
                $this.bind(eventClass('mouseleave'), function (e) {
                    hovered = false;
                });

                var shouldPrevent = false;
                $(ownerDocument).bind(eventClass('keydown'), function (e) {
                    if (e.isDefaultPrevented && e.isDefaultPrevented()) {
                        return;
                    }

                    if (!hovered) {
                        return;
                    }

                    var activeElement = document.activeElement ? document.activeElement : ownerDocument.activeElement;
                    // go deeper if element is a webcomponent
                    while (activeElement.shadowRoot) {
                        activeElement = activeElement.shadowRoot.activeElement;
                    }
                    if ($(activeElement).is(":input,[contenteditable]")) {
                        return;
                    }

                    var deltaX = 0;
                    var deltaY = 0;

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
                            if (e.ctrlKey) {
                                deltaY = -contentHeight;
                            } else {
                                deltaY = -containerHeight;
                            }
                            break;
                        case 36: // home
                            if (e.ctrlKey) {
                                deltaY = $this.scrollTop();
                            } else {
                                deltaY = containerHeight;
                            }
                            break;
                        default:
                            return;
                    }

                    $this.scrollTop($this.scrollTop() - deltaY);
                    $this.scrollLeft($this.scrollLeft() + deltaX);

                    shouldPrevent = shouldPreventWheel(deltaX, deltaY);
                    if (shouldPrevent) {
                        e.preventDefault();
                    }
                });
            }

            function bindRailClickHandler() {
                function stopPropagation(e) { e.stopPropagation(); }

                $scrollbarY.bind(eventClass('click'), stopPropagation);
                $scrollbarYRail.bind(eventClass('click'), function (e) {
                    var halfOfScrollbarLength = getInt(scrollbarYHeight / 2);
                    var positionTop = e.pageY - $scrollbarYRail.offset().top - halfOfScrollbarLength;
                    var maxPositionTop = containerHeight - scrollbarYHeight;
                    var positionRatio = positionTop / maxPositionTop;

                    if (positionRatio < 0) {
                        positionRatio = 0;
                    } else if (positionRatio > 1) {
                        positionRatio = 1;
                    }

                    $this.scrollTop((contentHeight - containerHeight) * positionRatio);
                });

                $scrollbarX.bind(eventClass('click'), stopPropagation);
                $scrollbarXRail.bind(eventClass('click'), function (e) {
                    var halfOfScrollbarLength = getInt(scrollbarXWidth / 2);
                    var positionLeft = e.pageX - $scrollbarXRail.offset().left - halfOfScrollbarLength;
                    var maxPositionLeft = containerWidth - scrollbarXWidth;
                    var positionRatio = positionLeft / maxPositionLeft;

                    if (positionRatio < 0) {
                        positionRatio = 0;
                    } else if (positionRatio > 1) {
                        positionRatio = 1;
                    }

                    $this.scrollLeft((contentWidth - containerWidth) * positionRatio);
                });
            }

            function bindSelectionHandler() {
                function getRangeNode() {
                    var selection = window.getSelection ? window.getSelection() :
                                    document.getSlection ? document.getSlection() : { rangeCount: 0 };
                    if (selection.rangeCount === 0) {
                        return null;
                    } else {
                        return selection.getRangeAt(0).commonAncestorContainer;
                    }
                }

                var scrollingLoop = null;
                var scrollDiff = { top: 0, left: 0 };
                function startScrolling() {
                    if (!scrollingLoop) {
                        scrollingLoop = setInterval(function () {
                            if (!isPluginAlive()) {
                                clearInterval(scrollingLoop);
                                return;
                            }

                            $this.scrollTop($this.scrollTop() + scrollDiff.top);
                            $this.scrollLeft($this.scrollLeft() + scrollDiff.left);
                            updateGeometry();
                        }, 50); // every .1 sec
                    }
                }
                function stopScrolling() {
                    if (scrollingLoop) {
                        clearInterval(scrollingLoop);
                        scrollingLoop = null;
                    }
                    $scrollbarXRail.removeClass('in-scrolling');
                    $scrollbarYRail.removeClass('in-scrolling');
                }

                var isSelected = false;
                $(ownerDocument).bind(eventClass('selectionchange'), function (e) {
                    if ($.contains($this[0], getRangeNode())) {
                        isSelected = true;
                    } else {
                        isSelected = false;
                        stopScrolling();
                    }
                });
                $(window).bind(eventClass('mouseup'), function (e) {
                    if (isSelected) {
                        isSelected = false;
                        stopScrolling();
                    }
                });

                $(window).bind(eventClass('mousemove'), function (e) {
                    if (isSelected) {
                        var mousePosition = { x: e.pageX, y: e.pageY };
                        var containerOffset = $this.offset();
                        var containerGeometry = {
                            left: containerOffset.left,
                            right: containerOffset.left + $this.outerWidth(),
                            top: containerOffset.top,
                            bottom: containerOffset.top + $this.outerHeight()
                        };

                        if (mousePosition.x < containerGeometry.left + 3) {
                            scrollDiff.left = -5;
                            $scrollbarXRail.addClass('in-scrolling');
                        } else if (mousePosition.x > containerGeometry.right - 3) {
                            scrollDiff.left = 5;
                            $scrollbarXRail.addClass('in-scrolling');
                        } else {
                            scrollDiff.left = 0;
                        }

                        if (mousePosition.y < containerGeometry.top + 3) {
                            if (containerGeometry.top + 3 - mousePosition.y < 5) {
                                scrollDiff.top = -5;
                            } else {
                                scrollDiff.top = -20;
                            }
                            $scrollbarYRail.addClass('in-scrolling');
                        } else if (mousePosition.y > containerGeometry.bottom - 3) {
                            if (mousePosition.y - containerGeometry.bottom + 3 < 5) {
                                scrollDiff.top = 5;
                            } else {
                                scrollDiff.top = 20;
                            }
                            $scrollbarYRail.addClass('in-scrolling');
                        } else {
                            scrollDiff.top = 0;
                        }

                        if (scrollDiff.top === 0 && scrollDiff.left === 0) {
                            stopScrolling();
                        } else {
                            startScrolling();
                        }
                    }
                });
            }

            function bindTouchHandler(supportsTouch, supportsIePointer) {
                function applyTouchMove(differenceX, differenceY) {
                    $this.scrollTop($this.scrollTop() - differenceY);
                    $this.scrollLeft($this.scrollLeft() - differenceX);

                    updateGeometry();
                }

                var startOffset = {};
                var startTime = 0;
                var speed = {};
                var easingLoop = null;
                var inGlobalTouch = false;
                var inLocalTouch = false;

                function globalTouchStart(e) {
                    inGlobalTouch = true;
                }
                function globalTouchEnd(e) {
                    inGlobalTouch = false;
                }

                function getTouch(e) {
                    if (e.originalEvent.targetTouches) {
                        return e.originalEvent.targetTouches[0];
                    } else {
                        // Maybe IE pointer
                        return e.originalEvent;
                    }
                }
                function shouldHandle(e) {
                    var event = e.originalEvent;
                    if (event.targetTouches && event.targetTouches.length === 1) {
                        return true;
                    }
                    if (event.pointerType && event.pointerType !== 'mouse' && event.pointerType !== event.MSPOINTER_TYPE_MOUSE) {
                        return true;
                    }
                    return false;
                }
                function touchStart(e) {
                    if (shouldHandle(e)) {
                        inLocalTouch = true;

                        var touch = getTouch(e);

                        startOffset.pageX = touch.pageX;
                        startOffset.pageY = touch.pageY;

                        startTime = (new Date()).getTime();

                        if (easingLoop !== null) {
                            clearInterval(easingLoop);
                        }

                        e.stopPropagation();
                    }
                }
                function touchMove(e) {
                    if (!inGlobalTouch && inLocalTouch && shouldHandle(e)) {
                        var touch = getTouch(e);

                        var currentOffset = { pageX: touch.pageX, pageY: touch.pageY };

                        var differenceX = currentOffset.pageX - startOffset.pageX;
                        var differenceY = currentOffset.pageY - startOffset.pageY;

                        applyTouchMove(differenceX, differenceY);
                        startOffset = currentOffset;

                        var currentTime = (new Date()).getTime();

                        var timeGap = currentTime - startTime;
                        if (timeGap > 0) {
                            speed.x = differenceX / timeGap;
                            speed.y = differenceY / timeGap;
                            startTime = currentTime;
                        }

                        if (shouldPreventSwipe(differenceX, differenceY)) {
                            e.stopPropagation();
                            e.preventDefault();
                        }
                    }
                }
                function touchEnd(e) {
                    if (!inGlobalTouch && inLocalTouch) {
                        inLocalTouch = false;

                        clearInterval(easingLoop);
                        easingLoop = setInterval(function () {
                            if (!isPluginAlive()) {
                                clearInterval(easingLoop);
                                return;
                            }

                            if (Math.abs(speed.x) < 0.01 && Math.abs(speed.y) < 0.01) {
                                clearInterval(easingLoop);
                                return;
                            }

                            applyTouchMove(speed.x * 30, speed.y * 30);

                            speed.x *= 0.8;
                            speed.y *= 0.8;
                        }, 10);
                    }
                }

                if (supportsTouch) {
                    $(window).bind(eventClass("touchstart"), globalTouchStart);
                    $(window).bind(eventClass("touchend"), globalTouchEnd);
                    $this.bind(eventClass("touchstart"), touchStart);
                    $this.bind(eventClass("touchmove"), touchMove);
                    $this.bind(eventClass("touchend"), touchEnd);
                }

                if (supportsIePointer) {
                    if (window.PointerEvent) {
                        $(window).bind(eventClass("pointerdown"), globalTouchStart);
                        $(window).bind(eventClass("pointerup"), globalTouchEnd);
                        $this.bind(eventClass("pointerdown"), touchStart);
                        $this.bind(eventClass("pointermove"), touchMove);
                        $this.bind(eventClass("pointerup"), touchEnd);
                    } else if (window.MSPointerEvent) {
                        $(window).bind(eventClass("MSPointerDown"), globalTouchStart);
                        $(window).bind(eventClass("MSPointerUp"), globalTouchEnd);
                        $this.bind(eventClass("MSPointerDown"), touchStart);
                        $this.bind(eventClass("MSPointerMove"), touchMove);
                        $this.bind(eventClass("MSPointerUp"), touchEnd);
                    }
                }
            }

            function bindScrollHandler() {
                $this.bind(eventClass('scroll'), function (e) {
                    updateGeometry();
                });
            }

            function destroy() {
                $this.unbind(eventClass());
                $(window).unbind(eventClass());
                $(ownerDocument).unbind(eventClass());
                $this.data('perfect-scrollbar', null);
                $this.data('perfect-scrollbar-update', null);
                $this.data('perfect-scrollbar-destroy', null);
                $scrollbarX.remove();
                $scrollbarY.remove();
                $scrollbarXRail.remove();
                $scrollbarYRail.remove();

                // clean all variables
                $this =
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
                eventClass = null;
            }

            var supportsTouch = (('ontouchstart' in window) || window.DocumentTouch && document instanceof window.DocumentTouch);
            var supportsIePointer = window.navigator.msMaxTouchPoints !== null;

            function initialize() {
                updateGeometry();
                bindScrollHandler();
                bindMouseScrollXHandler();
                bindMouseScrollYHandler();
                bindRailClickHandler();
                bindSelectionHandler();
                bindMouseWheelHandler();

                if (supportsTouch || supportsIePointer) {
                    bindTouchHandler(supportsTouch, supportsIePointer);
                }
                if (settings.useKeyboard) {
                    bindKeyboardHandler();
                }
                $this.data('perfect-scrollbar', $this);
                $this.data('perfect-scrollbar-update', updateGeometry);
                $this.data('perfect-scrollbar-destroy', destroy);
            }

            initialize();

            return $this;
        });
    };
});


/*
Plugin Name: 	BrowserSelector
Version: 		0.2
*/

(function ($) {
	$.extend({

		browserSelector: function () {

			var u = navigator.userAgent,
				ua = u.toLowerCase(),
				is = function (t) {
					return ua.indexOf(t) > -1;
				},
				g = 'gecko',
				w = 'webkit',
				s = 'safari',
				o = 'opera',
				h = document.documentElement,
				b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + parseFloat(navigator.appVersion.split("MSIE")[1])) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.jQuery1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.jQuery2 : '')) : is('konqueror') ? 'konqueror' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.jQuery1 : '') : is('mozilla/') ? g : '', is('j2me') ? 'mobile' : is('iphone') ? 'iphone' : is('ipod') ? 'ipod' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js'];

			c = b.join(' ');
			h.className += ' ' + c;

			var isIE11 = !(window.ActiveXObject) && "ActiveXObject" in window;

			if (isIE11) {
				$('html').removeClass('gecko').addClass('ie ie11');
				return;
			}

		}

	});
})(jQuery);

/*
Plugin Name: 	smoothScroll for jQuery.
Version: 		0.1

Based on:

	SmoothScroll v1.2.1
	Licensed under the terms of the MIT license.

	People involved
	 - Balazs Galambosi (maintainer)
	 - Patrick Brunner  (original idea)
	 - Michael Herf     (Pulse Algorithm)

*/
(function ($) {
	$.extend({

		smoothScroll: function () {

			var defaultOptions = {

				// Scrolling Core
				frameRate: 150, // [Hz]
				animationTime: 400, // [px]
				stepSize: 120, // [px]

				// Pulse (less tweakable)
				// ratio of "tail" to "acceleration"
				pulseAlgorithm: true,
				pulseScale: 8,
				pulseNormalize: 1,

				// Acceleration
				accelerationDelta: 20,  // 20
				accelerationMax: 1,   // 1

				// Keyboard Settings
				keyboardSupport: true,  // option
				arrowScroll: 50,     // [px]

				// Other
				touchpadSupport: true,
				fixedBackground: true,
				excluded: ""
			};

			var options = defaultOptions;


			// Other Variables
			var isExcluded = false;
			var isFrame = false;
			var direction = { x: 0, y: 0 };
			var initDone = false;
			var root = document.documentElement;
			var activeElement;
			var observer;
			var deltaBuffer = [120, 120, 120];

			var key = {
				left: 37, up: 38, right: 39, down: 40, spacebar: 32,
				pageup: 33, pagedown: 34, end: 35, home: 36
			};


			/***********************************************
             * SETTINGS
             ***********************************************/

			var options = defaultOptions;


			/***********************************************
             * INITIALIZE
             ***********************************************/

			/**
             * Tests if smooth scrolling is allowed. Shuts down everything if not.
             */
			function initTest() {

				var disableKeyboard = false;

				// disable keyboard support if anything above requested it
				if (disableKeyboard) {
					removeEvent("keydown", keydown);
				}

				if (options.keyboardSupport && !disableKeyboard) {
					addEvent("keydown", keydown);
				}
			}

			/**
             * Sets up scrolls array, determines if frames are involved.
             */
			function init() {

				if (!document.body) return;

				var body = document.body;
				var html = document.documentElement;
				var windowHeight = window.innerHeight;
				var scrollHeight = body.scrollHeight;

				// check compat mode for root element
				root = (document.compatMode.indexOf('CSS') >= 0) ? html : body;
				activeElement = body;

				initTest();
				initDone = true;

				// Checks if this script is running in a frame
				if (top != self) {
					isFrame = true;
				}

					/**
                     * This fixes a bug where the areas left and right to 
                     * the content does not trigger the onmousewheel event
                     * on some pages. e.g.: html, body { height: 100% }
                     */
				else if (scrollHeight > windowHeight &&
                        (body.offsetHeight <= windowHeight ||
                         html.offsetHeight <= windowHeight)) {

					// DOMChange (throttle): fix height
					var pending = false;
					var refresh = function () {
						if (!pending && html.scrollHeight != document.height) {
							pending = true; // add a new pending action
							setTimeout(function () {
								html.style.height = document.height + 'px';
								pending = false;
							}, 500); // act rarely to stay fast
						}
					};
					html.style.height = 'auto';
					setTimeout(refresh, 10);

					// clearfix
					if (root.offsetHeight <= windowHeight) {
						var underlay = document.createElement("div");
						underlay.style.clear = "both";
						body.appendChild(underlay);
					}
				}

				// disable fixed background
				if (!options.fixedBackground && !isExcluded) {
					body.style.backgroundAttachment = "scroll";
					html.style.backgroundAttachment = "scroll";
				}
			}


			/************************************************
             * SCROLLING 
             ************************************************/

			var que = [];
			var pending = false;
			var lastScroll = +new Date;

			/**
             * Pushes scroll actions to the scrolling queue.
             */
			function scrollArray(elem, left, top, delay) {

				delay || (delay = 1000);
				directionCheck(left, top);

				if (options.accelerationMax != 1) {
					var now = +new Date;
					var elapsed = now - lastScroll;
					if (elapsed < options.accelerationDelta) {
						var factor = (1 + (30 / elapsed)) / 2;
						if (factor > 1) {
							factor = Math.min(factor, options.accelerationMax);
							left *= factor;
							top *= factor;
						}
					}
					lastScroll = +new Date;
				}

				// push a scroll command
				que.push({
					x: left,
					y: top,
					lastX: (left < 0) ? 0.99 : -0.99,
					lastY: (top < 0) ? 0.99 : -0.99,
					start: +new Date
				});

				// don't act if there's a pending queue
				if (pending) {
					return;
				}

				var scrollWindow = (elem === document.body);

				var step = function (time) {

					var now = +new Date;
					var scrollX = 0;
					var scrollY = 0;

					for (var i = 0; i < que.length; i++) {

						var item = que[i];
						var elapsed = now - item.start;
						var finished = (elapsed >= options.animationTime);

						// scroll position: [0, 1]
						var position = (finished) ? 1 : elapsed / options.animationTime;

						// easing [optional]
						if (options.pulseAlgorithm) {
							position = pulse(position);
						}

						// only need the difference
						var x = (item.x * position - item.lastX) >> 0;
						var y = (item.y * position - item.lastY) >> 0;

						// add this to the total scrolling
						scrollX += x;
						scrollY += y;

						// update last values
						item.lastX += x;
						item.lastY += y;

						// delete and step back if it's over
						if (finished) {
							que.splice(i, 1); i--;
						}
					}

					// scroll left and top
					if (scrollWindow) {
						window.scrollBy(scrollX, scrollY);
					}
					else {
						if (scrollX) elem.scrollLeft += scrollX;
						if (scrollY) elem.scrollTop += scrollY;
					}

					// clean up if there's nothing left to do
					if (!left && !top) {
						que = [];
					}

					if (que.length) {
						requestFrame(step, elem, (delay / options.frameRate + 1));
					} else {
						pending = false;
					}
				};

				// start a new queue of actions
				requestFrame(step, elem, 0);
				pending = true;
			}


			/***********************************************
             * EVENTS
             ***********************************************/

			/**
             * Mouse wheel handler.
             * @param {Object} event
             */
			function wheel(event) {

				if (!initDone) {
					init();
				}

				var target = event.target;
				var overflowing = overflowingAncestor(target);

				// use default if there's no overflowing
				// element or default action is prevented    
				if (!overflowing || event.defaultPrevented ||
                    isNodeName(activeElement, "embed") ||
                   (isNodeName(target, "embed") && /\.pdf/i.test(target.src))) {
					return true;
				}

				var deltaX = event.wheelDeltaX || 0;
				var deltaY = event.wheelDeltaY || 0;

				// use wheelDelta if deltaX/Y is not available
				if (!deltaX && !deltaY) {
					deltaY = event.wheelDelta || 0;
				}

				// check if it's a touchpad scroll that should be ignored
				if (!options.touchpadSupport && isTouchpad(deltaY)) {
					return true;
				}

				// scale by step size
				// delta is 120 most of the time
				// synaptics seems to send 1 sometimes
				if (Math.abs(deltaX) > 1.2) {
					deltaX *= options.stepSize / 120;
				}
				if (Math.abs(deltaY) > 1.2) {
					deltaY *= options.stepSize / 120;
				}

				scrollArray(overflowing, -deltaX, -deltaY);
				event.preventDefault();
			}

			/**
             * Keydown event handler.
             * @param {Object} event
             */
			function keydown(event) {

				var target = event.target;
				var modifier = event.ctrlKey || event.altKey || event.metaKey ||
                              (event.shiftKey && event.keyCode !== key.spacebar);

				// do nothing if user is editing text
				// or using a modifier key (except shift)
				// or in a dropdown
				if (/input|textarea|select|embed/i.test(target.nodeName) ||
                     target.isContentEditable ||
                     event.defaultPrevented ||
                     modifier) {
					return true;
				}
				// spacebar should trigger button press
				if (isNodeName(target, "button") &&
                    event.keyCode === key.spacebar) {
					return true;
				}

				var shift, x = 0, y = 0;
				var elem = overflowingAncestor(activeElement);
				var clientHeight = elem.clientHeight;

				if (elem == document.body) {
					clientHeight = window.innerHeight;
				}

				switch (event.keyCode) {
					case key.up:
						y = -options.arrowScroll;
						break;
					case key.down:
						y = options.arrowScroll;
						break;
					case key.spacebar: // (+ shift)
						shift = event.shiftKey ? 1 : -1;
						y = -shift * clientHeight * 0.9;
						break;
					case key.pageup:
						y = -clientHeight * 0.9;
						break;
					case key.pagedown:
						y = clientHeight * 0.9;
						break;
					case key.home:
						y = -elem.scrollTop;
						break;
					case key.end:
						var damt = elem.scrollHeight - elem.scrollTop - clientHeight;
						y = (damt > 0) ? damt + 10 : 0;
						break;
					case key.left:
						x = -options.arrowScroll;
						break;
					case key.right:
						x = options.arrowScroll;
						break;
					default:
						return true; // a key we don't care about
				}

				scrollArray(elem, x, y);
				event.preventDefault();
			}

			/**
             * Mousedown event only for updating activeElement
             */
			function mousedown(event) {
				activeElement = event.target;
			}


			/***********************************************
             * OVERFLOW
             ***********************************************/

			var cache = {}; // cleared out every once in while
			setInterval(function () { cache = {}; }, 10 * 1000);

			var uniqueID = (function () {
				var i = 0;
				return function (el) {
					return el.uniqueID || (el.uniqueID = i++);
				};
			})();

			function setCache(elems, overflowing) {
				for (var i = elems.length; i--;)
					cache[uniqueID(elems[i])] = overflowing;
				return overflowing;
			}

			function overflowingAncestor(el) {
				var elems = [];
				var rootScrollHeight = root.scrollHeight;
				do {
					var cached = cache[uniqueID(el)];
					if (cached) {
						return setCache(elems, cached);
					}
					elems.push(el);
					if (rootScrollHeight === el.scrollHeight) {
						if (!isFrame || root.clientHeight + 10 < rootScrollHeight) {
							return setCache(elems, document.body); // scrolling root in WebKit
						}
					} else if (el.clientHeight + 10 < el.scrollHeight) {
						overflow = getComputedStyle(el, "").getPropertyValue("overflow-y");
						if (overflow === "scroll" || overflow === "auto") {
							return setCache(elems, el);
						}
					}
				} while (el = el.parentNode);
			}


			/***********************************************
             * HELPERS
             ***********************************************/

			function addEvent(type, fn, bubble) {
				window.addEventListener(type, fn, (bubble || false));
			}

			function removeEvent(type, fn, bubble) {
				window.removeEventListener(type, fn, (bubble || false));
			}

			function isNodeName(el, tag) {
				return (el.nodeName || "").toLowerCase() === tag.toLowerCase();
			}

			function directionCheck(x, y) {
				x = (x > 0) ? 1 : -1;
				y = (y > 0) ? 1 : -1;
				if (direction.x !== x || direction.y !== y) {
					direction.x = x;
					direction.y = y;
					que = [];
					lastScroll = 0;
				}
			}

			var deltaBufferTimer;

			function isTouchpad(deltaY) {
				if (!deltaY) return;
				deltaY = Math.abs(deltaY)
				deltaBuffer.push(deltaY);
				deltaBuffer.shift();
				clearTimeout(deltaBufferTimer);
				var allDivisable = (isDivisible(deltaBuffer[0], 120) &&
                                    isDivisible(deltaBuffer[1], 120) &&
                                    isDivisible(deltaBuffer[2], 120));
				return !allDivisable;
			}

			function isDivisible(n, divisor) {
				return (Math.floor(n / divisor) == n / divisor);
			}

			var requestFrame = (function () {
				return window.requestAnimationFrame ||
                        window.webkitRequestAnimationFrame ||
                        function (callback, element, delay) {
                        	window.setTimeout(callback, delay || (1000 / 60));
                        };
			})();


			/***********************************************
             * PULSE
             ***********************************************/

			/**
             * Viscous fluid with a pulse for part and decay for the rest.
             * - Applies a fixed force over an interval (a damped acceleration), and
             * - Lets the exponential bleed away the velocity over a longer interval
             * - Michael Herf, http://stereopsis.com/stopping/
             */
			function pulse_(x) {
				var val, start, expx;
				// test
				x = x * options.pulseScale;
				if (x < 1) { // acceleartion
					val = x - (1 - Math.exp(-x));
				} else {     // tail
					// the previous animation ended here:
					start = Math.exp(-1);
					// simple viscous drag
					x -= 1;
					expx = 1 - Math.exp(-x);
					val = start + (expx * (1 - start));
				}
				return val * options.pulseNormalize;
			}

			function pulse(x) {
				if (x >= 1) return 1;
				if (x <= 0) return 0;

				if (options.pulseNormalize == 1) {
					options.pulseNormalize /= pulse_(1);
				}
				return pulse_(x);
			}

			var isChrome = /chrome/i.test(window.navigator.userAgent);
			var wheelEvent = null;
			if ("onwheel" in document.createElement("div"))
				wheelEvent = "wheel";
			else if ("onmousewheel" in document.createElement("div"))
				wheelEvent = "mousewheel";

			if (wheelEvent && isChrome) {
				addEvent(wheelEvent, wheel);
				addEvent("mousedown", mousedown);
				addEvent("load", init);
			}


		}

	});
})(jQuery);



/*! jQuery Mobile v1.4.5 | Copyright 2010, 2014 jQuery Foundation, Inc. | jquery.org/license */

(function (e, t, n) { typeof define == "function" && define.amd ? define(["jquery"], function (r) { return n(r, e, t), r.mobile }) : n(e.jQuery, e, t) })(this, document, function (e, t, n, r) { (function (e, t, n, r) { function T(e) { while (e && typeof e.originalEvent != "undefined") e = e.originalEvent; return e } function N(t, n) { var i = t.type, s, o, a, l, c, h, p, d, v; t = e.Event(t), t.type = n, s = t.originalEvent, o = e.event.props, i.search(/^(mouse|click)/) > -1 && (o = f); if (s) for (p = o.length, l; p;) l = o[--p], t[l] = s[l]; i.search(/mouse(down|up)|click/) > -1 && !t.which && (t.which = 1); if (i.search(/^touch/) !== -1) { a = T(s), i = a.touches, c = a.changedTouches, h = i && i.length ? i[0] : c && c.length ? c[0] : r; if (h) for (d = 0, v = u.length; d < v; d++) l = u[d], t[l] = h[l] } return t } function C(t) { var n = {}, r, s; while (t) { r = e.data(t, i); for (s in r) r[s] && (n[s] = n.hasVirtualBinding = !0); t = t.parentNode } return n } function k(t, n) { var r; while (t) { r = e.data(t, i); if (r && (!n || r[n])) return t; t = t.parentNode } return null } function L() { g = !1 } function A() { g = !0 } function O() { E = 0, v.length = 0, m = !1, A() } function M() { L() } function _() { D(), c = setTimeout(function () { c = 0, O() }, e.vmouse.resetTimerDuration) } function D() { c && (clearTimeout(c), c = 0) } function P(t, n, r) { var i; if (r && r[t] || !r && k(n.target, t)) i = N(n, t), e(n.target).trigger(i); return i } function H(t) { var n = e.data(t.target, s), r; !m && (!E || E !== n) && (r = P("v" + t.type, t), r && (r.isDefaultPrevented() && t.preventDefault(), r.isPropagationStopped() && t.stopPropagation(), r.isImmediatePropagationStopped() && t.stopImmediatePropagation())) } function B(t) { var n = T(t).touches, r, i, o; n && n.length === 1 && (r = t.target, i = C(r), i.hasVirtualBinding && (E = w++, e.data(r, s, E), D(), M(), d = !1, o = T(t).touches[0], h = o.pageX, p = o.pageY, P("vmouseover", t, i), P("vmousedown", t, i))) } function j(e) { if (g) return; d || P("vmousecancel", e, C(e.target)), d = !0, _() } function F(t) { if (g) return; var n = T(t).touches[0], r = d, i = e.vmouse.moveDistanceThreshold, s = C(t.target); d = d || Math.abs(n.pageX - h) > i || Math.abs(n.pageY - p) > i, d && !r && P("vmousecancel", t, s), P("vmousemove", t, s), _() } function I(e) { if (g) return; A(); var t = C(e.target), n, r; P("vmouseup", e, t), d || (n = P("vclick", e, t), n && n.isDefaultPrevented() && (r = T(e).changedTouches[0], v.push({ touchID: E, x: r.clientX, y: r.clientY }), m = !0)), P("vmouseout", e, t), d = !1, _() } function q(t) { var n = e.data(t, i), r; if (n) for (r in n) if (n[r]) return !0; return !1 } function R() { } function U(t) { var n = t.substr(1); return { setup: function () { q(this) || e.data(this, i, {}); var r = e.data(this, i); r[t] = !0, l[t] = (l[t] || 0) + 1, l[t] === 1 && b.bind(n, H), e(this).bind(n, R), y && (l.touchstart = (l.touchstart || 0) + 1, l.touchstart === 1 && b.bind("touchstart", B).bind("touchend", I).bind("touchmove", F).bind("scroll", j)) }, teardown: function () { --l[t], l[t] || b.unbind(n, H), y && (--l.touchstart, l.touchstart || b.unbind("touchstart", B).unbind("touchmove", F).unbind("touchend", I).unbind("scroll", j)); var r = e(this), s = e.data(this, i); s && (s[t] = !1), r.unbind(n, R), q(this) || r.removeData(i) } } } var i = "virtualMouseBindings", s = "virtualTouchID", o = "vmouseover vmousedown vmousemove vmouseup vclick vmouseout vmousecancel".split(" "), u = "clientX clientY pageX pageY screenX screenY".split(" "), a = e.event.mouseHooks ? e.event.mouseHooks.props : [], f = e.event.props.concat(a), l = {}, c = 0, h = 0, p = 0, d = !1, v = [], m = !1, g = !1, y = "addEventListener" in n, b = e(n), w = 1, E = 0, S, x; e.vmouse = { moveDistanceThreshold: 10, clickDistanceThreshold: 10, resetTimerDuration: 1500 }; for (x = 0; x < o.length; x++) e.event.special[o[x]] = U(o[x]); y && n.addEventListener("click", function (t) { var n = v.length, r = t.target, i, o, u, a, f, l; if (n) { i = t.clientX, o = t.clientY, S = e.vmouse.clickDistanceThreshold, u = r; while (u) { for (a = 0; a < n; a++) { f = v[a], l = 0; if (u === r && Math.abs(f.x - i) < S && Math.abs(f.y - o) < S || e.data(u, s) === f.touchID) { t.preventDefault(), t.stopPropagation(); return } } u = u.parentNode } } }, !0) })(e, t, n), function (e) { e.mobile = {} }(e), function (e, t) { var r = { touch: "ontouchend" in n }; e.mobile.support = e.mobile.support || {}, e.extend(e.support, r), e.extend(e.mobile.support, r) }(e), function (e, t, r) { function l(t, n, i, s) { var o = i.type; i.type = n, s ? e.event.trigger(i, r, t) : e.event.dispatch.call(t, i), i.type = o } var i = e(n), s = e.mobile.support.touch, o = "touchmove scroll", u = s ? "touchstart" : "mousedown", a = s ? "touchend" : "mouseup", f = s ? "touchmove" : "mousemove"; e.each("touchstart touchmove touchend tap taphold swipe swipeleft swiperight scrollstart scrollstop".split(" "), function (t, n) { e.fn[n] = function (e) { return e ? this.bind(n, e) : this.trigger(n) }, e.attrFn && (e.attrFn[n] = !0) }), e.event.special.scrollstart = { enabled: !0, setup: function () { function s(e, n) { r = n, l(t, r ? "scrollstart" : "scrollstop", e) } var t = this, n = e(t), r, i; n.bind(o, function (t) { if (!e.event.special.scrollstart.enabled) return; r || s(t, !0), clearTimeout(i), i = setTimeout(function () { s(t, !1) }, 50) }) }, teardown: function () { e(this).unbind(o) } }, e.event.special.tap = { tapholdThreshold: 750, emitTapOnTaphold: !0, setup: function () { var t = this, n = e(t), r = !1; n.bind("vmousedown", function (s) { function a() { clearTimeout(u) } function f() { a(), n.unbind("vclick", c).unbind("vmouseup", a), i.unbind("vmousecancel", f) } function c(e) { f(), !r && o === e.target ? l(t, "tap", e) : r && e.preventDefault() } r = !1; if (s.which && s.which !== 1) return !1; var o = s.target, u; n.bind("vmouseup", a).bind("vclick", c), i.bind("vmousecancel", f), u = setTimeout(function () { e.event.special.tap.emitTapOnTaphold || (r = !0), l(t, "taphold", e.Event("taphold", { target: o })) }, e.event.special.tap.tapholdThreshold) }) }, teardown: function () { e(this).unbind("vmousedown").unbind("vclick").unbind("vmouseup"), i.unbind("vmousecancel") } }, e.event.special.swipe = { scrollSupressionThreshold: 30, durationThreshold: 1e3, horizontalDistanceThreshold: 30, verticalDistanceThreshold: 30, getLocation: function (e) { var n = t.pageXOffset, r = t.pageYOffset, i = e.clientX, s = e.clientY; if (e.pageY === 0 && Math.floor(s) > Math.floor(e.pageY) || e.pageX === 0 && Math.floor(i) > Math.floor(e.pageX)) i -= n, s -= r; else if (s < e.pageY - r || i < e.pageX - n) i = e.pageX - n, s = e.pageY - r; return { x: i, y: s } }, start: function (t) { var n = t.originalEvent.touches ? t.originalEvent.touches[0] : t, r = e.event.special.swipe.getLocation(n); return { time: (new Date).getTime(), coords: [r.x, r.y], origin: e(t.target) } }, stop: function (t) { var n = t.originalEvent.touches ? t.originalEvent.touches[0] : t, r = e.event.special.swipe.getLocation(n); return { time: (new Date).getTime(), coords: [r.x, r.y] } }, handleSwipe: function (t, n, r, i) { if (n.time - t.time < e.event.special.swipe.durationThreshold && Math.abs(t.coords[0] - n.coords[0]) > e.event.special.swipe.horizontalDistanceThreshold && Math.abs(t.coords[1] - n.coords[1]) < e.event.special.swipe.verticalDistanceThreshold) { var s = t.coords[0] > n.coords[0] ? "swipeleft" : "swiperight"; return l(r, "swipe", e.Event("swipe", { target: i, swipestart: t, swipestop: n }), !0), l(r, s, e.Event(s, { target: i, swipestart: t, swipestop: n }), !0), !0 } return !1 }, eventInProgress: !1, setup: function () { var t, n = this, r = e(n), s = {}; t = e.data(this, "mobile-events"), t || (t = { length: 0 }, e.data(this, "mobile-events", t)), t.length++, t.swipe = s, s.start = function (t) { if (e.event.special.swipe.eventInProgress) return; e.event.special.swipe.eventInProgress = !0; var r, o = e.event.special.swipe.start(t), u = t.target, l = !1; s.move = function (t) { if (!o || t.isDefaultPrevented()) return; r = e.event.special.swipe.stop(t), l || (l = e.event.special.swipe.handleSwipe(o, r, n, u), l && (e.event.special.swipe.eventInProgress = !1)), Math.abs(o.coords[0] - r.coords[0]) > e.event.special.swipe.scrollSupressionThreshold && t.preventDefault() }, s.stop = function () { l = !0, e.event.special.swipe.eventInProgress = !1, i.off(f, s.move), s.move = null }, i.on(f, s.move).one(a, s.stop) }, r.on(u, s.start) }, teardown: function () { var t, n; t = e.data(this, "mobile-events"), t && (n = t.swipe, delete t.swipe, t.length--, t.length === 0 && e.removeData(this, "mobile-events")), n && (n.start && e(this).off(u, n.start), n.move && i.off(f, n.move), n.stop && i.off(a, n.stop)) } }, e.each({ scrollstop: "scrollstart", taphold: "tap", swipeleft: "swipe.left", swiperight: "swipe.right" }, function (t, n) { e.event.special[t] = { setup: function () { e(this).bind(n, e.noop) }, teardown: function () { e(this).unbind(n) } } }) }(e, this) });