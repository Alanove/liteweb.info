//Copyright 2013
//Version: 1.0.2.3
//Create: May 30 2012
//Modify: December 10 2014

String.prototype.TrimLeft=function()
{
	return this.replace(/^(\s|\xA0|&nbsp;)*/,"");
};
String.prototype.TrimRight=function()
{
	return this.replace(/(\s|\xA0|&nbsp;)*$/,"");
};
String.prototype.trim = String.prototype.Trim=function()
{
	return this.TrimLeft().TrimRight();
};
String.prototype.toLower = function () {
	return this.toLowerCase();
};
String.prototype.Format = function()
{
	var obj = arguments[0];
	var s = this;

	if (typeof obj == "object")
		for (i in obj) {
			var reg = new RegExp("\\\{" + i + "\\\}", "g");
			s = s.replace(reg, obj[i]);
		}
	else
		for(var i=0;i<arguments.length;i++)
		{
			var reg = new RegExp("\\\{"+i+"\\\}","g");
			s=s.replace(reg,arguments[i]);
		}
	return s;
}

function Dimension(el) {
	this.width = el.offsetWidth;
	this.height = el.offsetHeight;
}
function GetPosition(el) {
	var l = el.offsetLeft, t = el.offsetTop;
	while ((el = el.offsetParent) != null) {
		l += el.offsetLeft;
		t += el.offsetTop;
	}
	var obj = {
		"top": t, "left": l
	};
	return obj;
};
function GetMousePosition(f) {
	var top = event.y + window.document.body.scrollTop;
	var left = event.x + window.document.body.scrollLeft;
	if (f) {
		var pos = GetPosition(f.frameElement);
		top += pos.top;
		left += pos.left;
	}
	var obj = {
		"top": top, "left": left
	};
	return obj;
}
/*
///Checks if the object inherits another object
Object.prototype.Is = function (object) {
	for (i in object) {
		if (!this[i])
			return false;
		if (typeof this[i] != typeof object[i])
			return false;
	}
	return true
}
*/
function Dimension(el)
{
	this.width = el.offsetWidth;
	this.height = el.offsetHeight;
}
function Trank(str,len)
{
	return str.length > len? str.substring(0, len) + (len > 3? "...": ""): str;
}
function InitFormValue(f, v)
{
	switch(f.type)
	{
		case "file":
			break;
		case "select-one":
			for(var j = 0; j < f.options.length; j++)
			{
				if(f.options[j].value == v)
				{
					f.options[j].selected = true;
					break;
				}
			}
			break;
		case "checkbox":
			f.checked = v;
			break;
		case "radio":
			f.checked = v;
			break;
		default:
			f.value=value;
			break;
	}
}
function defined(a)
{
	var ret = (typeof(a)=="undefined");
	if (ret == true) 
	   return false;
	if (typeof(a) == "string" && a=="")
	   return false;
	return (a!=null);
}
function isOk(a) {
	return defined(a);
}
function GetViewWidth(full)
{
	if(!jQuery)
		return null;
	return full ? Math.max(jQuery(document).width(), jQuery(window).width()) : jQuery(window).width();
}

function GetViewHeight(full)
{
	if(!jQuery)
		return null;
	return full ? Math.max(jQuery(document).height(), jQuery(window).height()) : jQuery(window).height();
}
function addStyle(name, rules)
{
	var node = document.createElement("style");
	node.setAttribute("type", "text/css");
	var text = "{0}{{1}}".Format(name, rules);
	var isIE = /msie/i.test(navigator.userAgent);
	if(!isIE)
		node.appendChild(document.createTextNode(text));
	document.getElementsByTagName("head")[0].appendChild(node);
	if(isIE && document.styleSheets && document.styleSheets.length > 0)
	{
		var lastStyle = document.styleSheets[document.styleSheets.length - 1];
		if(typeof(lastStyle.addRule) == "object")
			lastStyle.addRule(name, rules);
	}
}

function _trace(obj) {
	window.lw = window.lw ? window.lw : {};
	var lw = window.lw;
	if (!lw.debContainer) {
		lw.debContainer = $("<div style=\"z-index:999999999;padding:5px;box-shadow: -5px 5px 10px #888;background-color:#efefef;border:3px double black;border-radius:5px;width:200px;height:300px;overflow:auto;position:fixed;right:0;top:0\"></div>");
		$(document.body).append(lw.debContainer);
	}
	var el = $("<div />");
	if ((typeof obj == typeof ("")) || (typeof obj == typeof (1)))
		el.html(obj);
	/*
	else {
		for (i in obj) {
			deb("<b>" + i + "</b>");
			deb(obj[i]);
		}
	}
	*/
	lw.debContainer.prepend(el);
}

function inArray(array, obj) {
    return (array.indexOf(obj) != -1);
}

function compare(obj1, obj2, datatype) {
	var greaterThan = ">";
	var lessThan = "<";
	var equal = "=";

	obj1 = obj1 ? obj1 : "";
	obj2 = obj2 ? obj2 : "";


	switch (datatype.toLowerCase()) {
		case "number":
		case "integer":
		case "decimal":
		case "float":
		case "int":
			var val1 = parseFloat(obj1.toString());
			var val2 = parseFloat(obj2.toString());
			if (val1 > val2)
				return greaterThan;
			if (val1 < val2)
				return lessThan;
			return equal;
			break;
		case "date":
			var val1 = Date.parse(obj1.toString());
			var val2 = Date.parse(obj2.toString());
			if (val1 > val2)
				return greaterThan;
			if (val1 < val2)
				return lessThan;
			return equal;
			break;
		case "string":
			var val1 = obj1.toString();
			var val2 = obj2.toString();
			if (val1 > val2)
				return greaterThan;
			if (val1 < val2)
				return lessThan;
			return equal;
			break;
		default:
			var val1 = obj1.toString().length;
			var val2 = obj2.toString().length;
			if (val1 > val2)
				return greaterThan;
			if (val1 < val2)
				return lessThan;
			return equal;
			break;
	}
}


var lwUtils = {
	version: "1.0.2.2",
	createDate: "May 30 2012",
	modifiedDate: "January 16 2013",
	_an: 0,
	an: function () {
		return ++lw.utils._an;
	},
	dimension: Dimension,
	isOk: isOk,
	trace: _trace,
	compare: compare,
	inArray: inArray,
	initFormValue: InitFormValue,
	trankate: Trank,
	viewWith: GetViewWidth,
	viewHeight: GetViewHeight,
	passwordScorrer: function (password) {
		var score = 0;

		var len = password.length;

		if (len >= 21) { score = 7; }
		else if (len >= 16) { score = 6; }
		else if (len >= 13) { score = 5; }
		else if (len >= 10) { score = 4; }
		else if (len >= 8) { score = 3; }
		else if (len >= 5) { score = 2; }

		//lower case char
		if (passwd.match(/[a-z]/)) score += 1;

		//upper case char
		if (passwd.match(/[A-Z]/)) score += 5;

		// NUMBERS
		if (passwd.match(/\d+/)) score += 5;
		if (passwd.match(/(.*[0-9].*[0-9].*[0-9])/)) score += 5;


		// SPECIAL CHAR
		if (passwd.match(/.[!,@,#,$,%,^,&,*,?,_,~]/)) score += 5;

		if (passwd.match(/(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])/)) score += 5;

		// COMBOS
		//lower and upper case
		if (passwd.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) score += 2;

		// letters, numbers
		if (passwd.match(/([a-zA-Z])/) && passwd.match(/([0-9])/)) score += 2;

		// letters, numbers, and special characters
		if (passwd.match(/([a-zA-Z0-9].*[!,@,#,$,%,^,&,*,?,_,~])|([!,@,#,$,%,^,&,*,?,_,~].*[a-zA-Z0-9])/)) score += 2;

		//below 16 weak, above 45 strong a meter can be created from 16(0%) to 45(95%)
		return score;
	},
	equals: function (a, b, ignoreCase) {
		if (ignoreCase) {
			return a.toLowerCase() === b.toLowerCase();
		}
		return a === b;
	},
	fieldMask: function (field, val) {
		if (!isOk(field))
			return;
		field.bind("focus", function () {
			if (field.val() == val) {
				field.val("");
				field.removeClass("empty");
			}
		});
		field.bind("blur", function () {
			if (field.val() == "") {
				field.val(val);
				field.addClass("empty");
			}
		});
	},
	stripOutHtmlTags: function (s) {
		if (s == null)
			return "";

		var r = new RegExp("<[^>]+>");
		return s.replace(r, "");
	},
	trankate: function (s, l, end) {
		end = "..." || end;
		s = lw.utils.stripOutHtmlTags(s);
		if (s.length > l) {
			s = s.substring(0, l - 3);
			var r = new RegExp("\\s([a-z_0-9.&;])*$", "i");
			return lw.utils.sup(s.replace(r, end));
		}
		return s;
	},
	sup: function (s) {
		return s.replace(/<sup>&amp;reg;<\/sup>/ig, "®")
			.replace(/<sup>®<\/sup>/ig, "®")
			.replace(/\&amp;reg;/ig, "&reg;")
			.replace(/\&reg;/ig, "<sup>&reg;</sup>")
			.replace(/®/ig, "<sup>&reg;</sup>");
	},
	relative_time: function (parsed_date) {

		parsed_date = typeof parsed_date === "string" ? Date.parse(parsed_date) : parsed_date;

		var relative_to = (arguments.length > 1) ? arguments[1] : new Date();
	  var delta = parseInt((relative_to.getTime() - parsed_date) / 1000);
	  //delta = delta + (relative_to.getTimezoneOffset() * 60);

	  if (delta < 60) {
		return 'less than a minute ago';
	  } else if(delta < 120) {
		return 'about a minute ago';
	  } else if(delta < (60*60)) {
		return (parseInt(delta / 60)).toString() + ' minutes ago';
	  } else if(delta < (120*60)) {
		return 'about an hour ago';
	  } else if(delta < (24*60*60)) {
		return 'about ' + (parseInt(delta / 3600)).toString() + ' hours ago';
	  } else if(delta < (48*60*60)) {
		return '1 day ago';
	  } else {
		return (parseInt(delta / 86400)).toString() + ' days ago';
	  }
	},
	capitaliseFirstLetter: function(string)
	{
		return string.charAt(0).toUpperCase() + string.slice(1);
	},
	toId: function (str, replacement) {
		if (!isOk(replacement))
			replacement = "-";
		str = str.toLowerCase();

		str = str.replace("&reg;", "");

		var r = /\W/ig;
		var r1 = /\s+/g;

		var before = "àÀâÂäÄáÁéÉèÈêÊëËìÌîÎïÏòÒôÔöÖùÙûÛüÜçÇ’ñ/ó:";
		var after = "aAaAaAaAeEeEeEeEiIiIiIoOoOoOuUuUuUcC-n-o ";

		var cleaned = str;

		for (i = 0; i < before.length; i++)
		{
			cleaned = cleaned.replace(before.charAt(i), after.charAt(i));
		}

		cleaned = cleaned.replace(r, " ");
		cleaned = cleaned.replace(r1, " ");

		cleaned = cleaned.trim().replace(/ /g, replacement);

		return cleaned;
	},
	request: function (key) {
		var _ = lw.utils;
		if (!_.query) {
			_.query = {};
			var temp = location.href.split("?");

			if (temp.length == 0)
				return null;

			var hash = temp[1];

			var arr = hash.split("&");
			_.query = {};
			for (var i = 0; i < arr.length; i++) {
				if (arr[i].Trim() != "") {
					var temp = arr[i].Trim().split("=");
					if (temp.length == 2) {
						_.query[temp[0].Trim()] = temp[1].Trim();
					}
				}
			}
		}

		return _.query[key];
	},
	getTopZIndex: function (container) {
		var _ = lw.utils;
		//if (_.maxIndex)
		//	return _.maxIndex;
		
		container = "body" || container;
		var maxZ = 1;
		$(container + ' *').each(function (e, n) {
			var $this = $(this);
			if ($this.css('position') != 'static' && $this.css('z-index') != "auto") {
				maxZ = Math.max(maxZ, parseInt($this.css('z-index')));
			}
		});
		_.maxIndex = maxZ + 100;
		return _.maxIndex;
	},
	getFromJsonArray: function (jSonArray, objectToTest) {
		//Use TAFFYDB

		var ret = null;

		for (var i = 0; i < jSonArray.length; i++) {

			var testOk = true;

			for (prop in objectToTest) {
				if (typeof prop == "string") {
					if (jSonArray[i][prop] !== objectToTest[prop]) {
						testOk = false;
						break;
					}
				}
			}
			if (testOk) {
				ret = jSonArray[i];
				break;
			}
		}

		return ret;
	},
	toURL: function (str, replacement) {
		if (!isOk(replacement))
			replacement = "-";
		str = str.toLowerCase();
		var htmlRemover = new RegExp("<[^>]+>", "ig");
		str = str.replace(htmlRemover, "");

		str = str.replace(/\&reg;/g, "");

		var r = new RegExp("\\W", "ig");
		var r1 = new RegExp("\\s+", "ig");

		var before = "àÀâÂäÄáÁéÉèÈêÊëËìÌîÎïÏòÒôÔöÖùÙûÛüÜçÇ’ñ/ó:";
		var after = "aAaAaAaAeEeEeEeEiIiIiIoOoOoOuUuUuUcC-n-o ";

		var cleaned = str;

		for (var i = 0; i < before.Length; i++)
		{
			cleaned = cleaned.replace(new RegExp(before[i], "ig"), after[i].ToString());
		}

		cleaned = cleaned.replace(r1, " ").replace(r, " ").trim();

		cleaned = cleaned.replace(/ /g, replacement);

		var r3 = new RegExp(replacement + "+", "ig");

		return cleaned.replace(r3, replacement);
	}
}

this.lw = this.lw ? this.lw : {};
this.lw.utils = lwUtils;




/* ==============================================
Disable Scrolling
=============================================== */

// left: 37, up: 38, right: 39, down: 40,
// spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36
var keys = [37, 38, 39, 40];

function preventDefault(e) {
	e = e || window.event;
	if (e.preventDefault)
		e.preventDefault();
	e.returnValue = false;
}

function keydown(e) {
	for (var i = keys.length; i--;) {
		if (e.keyCode === keys[i]) {
			preventDefault(e);
			return;
		}
	}
}

function wheel(e) {
	preventDefault(e);
}

function disable_scroll() {
	if (window.addEventListener) {
		window.addEventListener('DOMMouseScroll', wheel, false);
	}
	window.onmousewheel = document.onmousewheel = wheel;
	document.onkeydown = keydown;
}

function enable_scroll() {
	if (window.removeEventListener) {
		window.removeEventListener('DOMMouseScroll', wheel, false);
	}
	window.onmousewheel = document.onmousewheel = document.onkeydown = null;
}

lw.utils.enableScroll = enable_scroll;
lw.utils.disableScroll = disable_scroll;