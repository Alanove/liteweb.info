/*! jQuery UI - v1.10.3 - 2013-06-07
* http://jqueryui.com
* Includes: jquery.ui.core.js, jquery.ui.widget.js, jquery.ui.mouse.js, jquery.ui.position.js, jquery.ui.draggable.js, jquery.ui.droppable.js, jquery.ui.resizable.js, jquery.ui.selectable.js, jquery.ui.sortable.js, jquery.ui.datepicker.js
* Copyright 2013 jQuery Foundation and other contributors Licensed MIT */
(function (e, t) { function i(t, i) { var a, n, r, o = t.nodeName.toLowerCase(); return "area" === o ? (a = t.parentNode, n = a.name, t.href && n && "map" === a.nodeName.toLowerCase() ? (r = e("img[usemap=#" + n + "]")[0], !!r && s(r)) : !1) : (/input|select|textarea|button|object/.test(o) ? !t.disabled : "a" === o ? t.href || i : i) && s(t) } function s(t) { return e.expr.filters.visible(t) && !e(t).parents().addBack().filter(function () { return "hidden" === e.css(this, "visibility") }).length } var a = 0, n = /^ui-id-\d+$/; e.ui = e.ui || {}, e.extend(e.ui, { version: "1.10.3", keyCode: { BACKSPACE: 8, COMMA: 188, DELETE: 46, DOWN: 40, END: 35, ENTER: 13, ESCAPE: 27, HOME: 36, LEFT: 37, NUMPAD_ADD: 107, NUMPAD_DECIMAL: 110, NUMPAD_DIVIDE: 111, NUMPAD_ENTER: 108, NUMPAD_MULTIPLY: 106, NUMPAD_SUBTRACT: 109, PAGE_DOWN: 34, PAGE_UP: 33, PERIOD: 190, RIGHT: 39, SPACE: 32, TAB: 9, UP: 38 } }), e.fn.extend({ focus: function (t) { return function (i, s) { return "number" == typeof i ? this.each(function () { var t = this; setTimeout(function () { e(t).focus(), s && s.call(t) }, i) }) : t.apply(this, arguments) } }(e.fn.focus), scrollParent: function () { var t; return t = e.ui.ie && /(static|relative)/.test(this.css("position")) || /absolute/.test(this.css("position")) ? this.parents().filter(function () { return /(relative|absolute|fixed)/.test(e.css(this, "position")) && /(auto|scroll)/.test(e.css(this, "overflow") + e.css(this, "overflow-y") + e.css(this, "overflow-x")) }).eq(0) : this.parents().filter(function () { return /(auto|scroll)/.test(e.css(this, "overflow") + e.css(this, "overflow-y") + e.css(this, "overflow-x")) }).eq(0), /fixed/.test(this.css("position")) || !t.length ? e(document) : t }, zIndex: function (i) { if (i !== t) return this.css("zIndex", i); if (this.length) for (var s, a, n = e(this[0]) ; n.length && n[0] !== document;) { if (s = n.css("position"), ("absolute" === s || "relative" === s || "fixed" === s) && (a = parseInt(n.css("zIndex"), 10), !isNaN(a) && 0 !== a)) return a; n = n.parent() } return 0 }, uniqueId: function () { return this.each(function () { this.id || (this.id = "ui-id-" + ++a) }) }, removeUniqueId: function () { return this.each(function () { n.test(this.id) && e(this).removeAttr("id") }) } }), e.extend(e.expr[":"], { data: e.expr.createPseudo ? e.expr.createPseudo(function (t) { return function (i) { return !!e.data(i, t) } }) : function (t, i, s) { return !!e.data(t, s[3]) }, focusable: function (t) { return i(t, !isNaN(e.attr(t, "tabindex"))) }, tabbable: function (t) { var s = e.attr(t, "tabindex"), a = isNaN(s); return (a || s >= 0) && i(t, !a) } }), e("<a>").outerWidth(1).jquery || e.each(["Width", "Height"], function (i, s) { function a(t, i, s, a) { return e.each(n, function () { i -= parseFloat(e.css(t, "padding" + this)) || 0, s && (i -= parseFloat(e.css(t, "border" + this + "Width")) || 0), a && (i -= parseFloat(e.css(t, "margin" + this)) || 0) }), i } var n = "Width" === s ? ["Left", "Right"] : ["Top", "Bottom"], r = s.toLowerCase(), o = { innerWidth: e.fn.innerWidth, innerHeight: e.fn.innerHeight, outerWidth: e.fn.outerWidth, outerHeight: e.fn.outerHeight }; e.fn["inner" + s] = function (i) { return i === t ? o["inner" + s].call(this) : this.each(function () { e(this).css(r, a(this, i) + "px") }) }, e.fn["outer" + s] = function (t, i) { return "number" != typeof t ? o["outer" + s].call(this, t) : this.each(function () { e(this).css(r, a(this, t, !0, i) + "px") }) } }), e.fn.addBack || (e.fn.addBack = function (e) { return this.add(null == e ? this.prevObject : this.prevObject.filter(e)) }), e("<a>").data("a-b", "a").removeData("a-b").data("a-b") && (e.fn.removeData = function (t) { return function (i) { return arguments.length ? t.call(this, e.camelCase(i)) : t.call(this) } }(e.fn.removeData)), e.ui.ie = !!/msie [\w.]+/.exec(navigator.userAgent.toLowerCase()), e.support.selectstart = "onselectstart" in document.createElement("div"), e.fn.extend({ disableSelection: function () { return this.bind((e.support.selectstart ? "selectstart" : "mousedown") + ".ui-disableSelection", function (e) { e.preventDefault() }) }, enableSelection: function () { return this.unbind(".ui-disableSelection") } }), e.extend(e.ui, { plugin: { add: function (t, i, s) { var a, n = e.ui[t].prototype; for (a in s) n.plugins[a] = n.plugins[a] || [], n.plugins[a].push([i, s[a]]) }, call: function (e, t, i) { var s, a = e.plugins[t]; if (a && e.element[0].parentNode && 11 !== e.element[0].parentNode.nodeType) for (s = 0; a.length > s; s++) e.options[a[s][0]] && a[s][1].apply(e.element, i) } }, hasScroll: function (t, i) { if ("hidden" === e(t).css("overflow")) return !1; var s = i && "left" === i ? "scrollLeft" : "scrollTop", a = !1; return t[s] > 0 ? !0 : (t[s] = 1, a = t[s] > 0, t[s] = 0, a) } }) })(jQuery); (function (e, t) { var i = 0, s = Array.prototype.slice, n = e.cleanData; e.cleanData = function (t) { for (var i, s = 0; null != (i = t[s]) ; s++) try { e(i).triggerHandler("remove") } catch (a) { } n(t) }, e.widget = function (i, s, n) { var a, r, o, h, l = {}, u = i.split(".")[0]; i = i.split(".")[1], a = u + "-" + i, n || (n = s, s = e.Widget), e.expr[":"][a.toLowerCase()] = function (t) { return !!e.data(t, a) }, e[u] = e[u] || {}, r = e[u][i], o = e[u][i] = function (e, i) { return this._createWidget ? (arguments.length && this._createWidget(e, i), t) : new o(e, i) }, e.extend(o, r, { version: n.version, _proto: e.extend({}, n), _childConstructors: [] }), h = new s, h.options = e.widget.extend({}, h.options), e.each(n, function (i, n) { return e.isFunction(n) ? (l[i] = function () { var e = function () { return s.prototype[i].apply(this, arguments) }, t = function (e) { return s.prototype[i].apply(this, e) }; return function () { var i, s = this._super, a = this._superApply; return this._super = e, this._superApply = t, i = n.apply(this, arguments), this._super = s, this._superApply = a, i } }(), t) : (l[i] = n, t) }), o.prototype = e.widget.extend(h, { widgetEventPrefix: r ? h.widgetEventPrefix : i }, l, { constructor: o, namespace: u, widgetName: i, widgetFullName: a }), r ? (e.each(r._childConstructors, function (t, i) { var s = i.prototype; e.widget(s.namespace + "." + s.widgetName, o, i._proto) }), delete r._childConstructors) : s._childConstructors.push(o), e.widget.bridge(i, o) }, e.widget.extend = function (i) { for (var n, a, r = s.call(arguments, 1), o = 0, h = r.length; h > o; o++) for (n in r[o]) a = r[o][n], r[o].hasOwnProperty(n) && a !== t && (i[n] = e.isPlainObject(a) ? e.isPlainObject(i[n]) ? e.widget.extend({}, i[n], a) : e.widget.extend({}, a) : a); return i }, e.widget.bridge = function (i, n) { var a = n.prototype.widgetFullName || i; e.fn[i] = function (r) { var o = "string" == typeof r, h = s.call(arguments, 1), l = this; return r = !o && h.length ? e.widget.extend.apply(null, [r].concat(h)) : r, o ? this.each(function () { var s, n = e.data(this, a); return n ? e.isFunction(n[r]) && "_" !== r.charAt(0) ? (s = n[r].apply(n, h), s !== n && s !== t ? (l = s && s.jquery ? l.pushStack(s.get()) : s, !1) : t) : e.error("no such method '" + r + "' for " + i + " widget instance") : e.error("cannot call methods on " + i + " prior to initialization; " + "attempted to call method '" + r + "'") }) : this.each(function () { var t = e.data(this, a); t ? t.option(r || {})._init() : e.data(this, a, new n(r, this)) }), l } }, e.Widget = function () { }, e.Widget._childConstructors = [], e.Widget.prototype = { widgetName: "widget", widgetEventPrefix: "", defaultElement: "<div>", options: { disabled: !1, create: null }, _createWidget: function (t, s) { s = e(s || this.defaultElement || this)[0], this.element = e(s), this.uuid = i++, this.eventNamespace = "." + this.widgetName + this.uuid, this.options = e.widget.extend({}, this.options, this._getCreateOptions(), t), this.bindings = e(), this.hoverable = e(), this.focusable = e(), s !== this && (e.data(s, this.widgetFullName, this), this._on(!0, this.element, { remove: function (e) { e.target === s && this.destroy() } }), this.document = e(s.style ? s.ownerDocument : s.document || s), this.window = e(this.document[0].defaultView || this.document[0].parentWindow)), this._create(), this._trigger("create", null, this._getCreateEventData()), this._init() }, _getCreateOptions: e.noop, _getCreateEventData: e.noop, _create: e.noop, _init: e.noop, destroy: function () { this._destroy(), this.element.unbind(this.eventNamespace).removeData(this.widgetName).removeData(this.widgetFullName).removeData(e.camelCase(this.widgetFullName)), this.widget().unbind(this.eventNamespace).removeAttr("aria-disabled").removeClass(this.widgetFullName + "-disabled " + "ui-state-disabled"), this.bindings.unbind(this.eventNamespace), this.hoverable.removeClass("ui-state-hover"), this.focusable.removeClass("ui-state-focus") }, _destroy: e.noop, widget: function () { return this.element }, option: function (i, s) { var n, a, r, o = i; if (0 === arguments.length) return e.widget.extend({}, this.options); if ("string" == typeof i) if (o = {}, n = i.split("."), i = n.shift(), n.length) { for (a = o[i] = e.widget.extend({}, this.options[i]), r = 0; n.length - 1 > r; r++) a[n[r]] = a[n[r]] || {}, a = a[n[r]]; if (i = n.pop(), s === t) return a[i] === t ? null : a[i]; a[i] = s } else { if (s === t) return this.options[i] === t ? null : this.options[i]; o[i] = s } return this._setOptions(o), this }, _setOptions: function (e) { var t; for (t in e) this._setOption(t, e[t]); return this }, _setOption: function (e, t) { return this.options[e] = t, "disabled" === e && (this.widget().toggleClass(this.widgetFullName + "-disabled ui-state-disabled", !!t).attr("aria-disabled", t), this.hoverable.removeClass("ui-state-hover"), this.focusable.removeClass("ui-state-focus")), this }, enable: function () { return this._setOption("disabled", !1) }, disable: function () { return this._setOption("disabled", !0) }, _on: function (i, s, n) { var a, r = this; "boolean" != typeof i && (n = s, s = i, i = !1), n ? (s = a = e(s), this.bindings = this.bindings.add(s)) : (n = s, s = this.element, a = this.widget()), e.each(n, function (n, o) { function h() { return i || r.options.disabled !== !0 && !e(this).hasClass("ui-state-disabled") ? ("string" == typeof o ? r[o] : o).apply(r, arguments) : t } "string" != typeof o && (h.guid = o.guid = o.guid || h.guid || e.guid++); var l = n.match(/^(\w+)\s*(.*)$/), u = l[1] + r.eventNamespace, c = l[2]; c ? a.delegate(c, u, h) : s.bind(u, h) }) }, _off: function (e, t) { t = (t || "").split(" ").join(this.eventNamespace + " ") + this.eventNamespace, e.unbind(t).undelegate(t) }, _delay: function (e, t) { function i() { return ("string" == typeof e ? s[e] : e).apply(s, arguments) } var s = this; return setTimeout(i, t || 0) }, _hoverable: function (t) { this.hoverable = this.hoverable.add(t), this._on(t, { mouseenter: function (t) { e(t.currentTarget).addClass("ui-state-hover") }, mouseleave: function (t) { e(t.currentTarget).removeClass("ui-state-hover") } }) }, _focusable: function (t) { this.focusable = this.focusable.add(t), this._on(t, { focusin: function (t) { e(t.currentTarget).addClass("ui-state-focus") }, focusout: function (t) { e(t.currentTarget).removeClass("ui-state-focus") } }) }, _trigger: function (t, i, s) { var n, a, r = this.options[t]; if (s = s || {}, i = e.Event(i), i.type = (t === this.widgetEventPrefix ? t : this.widgetEventPrefix + t).toLowerCase(), i.target = this.element[0], a = i.originalEvent) for (n in a) n in i || (i[n] = a[n]); return this.element.trigger(i, s), !(e.isFunction(r) && r.apply(this.element[0], [i].concat(s)) === !1 || i.isDefaultPrevented()) } }, e.each({ show: "fadeIn", hide: "fadeOut" }, function (t, i) { e.Widget.prototype["_" + t] = function (s, n, a) { "string" == typeof n && (n = { effect: n }); var r, o = n ? n === !0 || "number" == typeof n ? i : n.effect || i : t; n = n || {}, "number" == typeof n && (n = { duration: n }), r = !e.isEmptyObject(n), n.complete = a, n.delay && s.delay(n.delay), r && e.effects && e.effects.effect[o] ? s[t](n) : o !== t && s[o] ? s[o](n.duration, n.easing, a) : s.queue(function (i) { e(this)[t](), a && a.call(s[0]), i() }) } }) })(jQuery); (function (e) { var t = !1; e(document).mouseup(function () { t = !1 }), e.widget("ui.mouse", { version: "1.10.3", options: { cancel: "input,textarea,button,select,option", distance: 1, delay: 0 }, _mouseInit: function () { var t = this; this.element.bind("mousedown." + this.widgetName, function (e) { return t._mouseDown(e) }).bind("click." + this.widgetName, function (i) { return !0 === e.data(i.target, t.widgetName + ".preventClickEvent") ? (e.removeData(i.target, t.widgetName + ".preventClickEvent"), i.stopImmediatePropagation(), !1) : undefined }), this.started = !1 }, _mouseDestroy: function () { this.element.unbind("." + this.widgetName), this._mouseMoveDelegate && e(document).unbind("mousemove." + this.widgetName, this._mouseMoveDelegate).unbind("mouseup." + this.widgetName, this._mouseUpDelegate) }, _mouseDown: function (i) { if (!t) { this._mouseStarted && this._mouseUp(i), this._mouseDownEvent = i; var s = this, n = 1 === i.which, a = "string" == typeof this.options.cancel && i.target.nodeName ? e(i.target).closest(this.options.cancel).length : !1; return n && !a && this._mouseCapture(i) ? (this.mouseDelayMet = !this.options.delay, this.mouseDelayMet || (this._mouseDelayTimer = setTimeout(function () { s.mouseDelayMet = !0 }, this.options.delay)), this._mouseDistanceMet(i) && this._mouseDelayMet(i) && (this._mouseStarted = this._mouseStart(i) !== !1, !this._mouseStarted) ? (i.preventDefault(), !0) : (!0 === e.data(i.target, this.widgetName + ".preventClickEvent") && e.removeData(i.target, this.widgetName + ".preventClickEvent"), this._mouseMoveDelegate = function (e) { return s._mouseMove(e) }, this._mouseUpDelegate = function (e) { return s._mouseUp(e) }, e(document).bind("mousemove." + this.widgetName, this._mouseMoveDelegate).bind("mouseup." + this.widgetName, this._mouseUpDelegate), i.preventDefault(), t = !0, !0)) : !0 } }, _mouseMove: function (t) { return e.ui.ie && (!document.documentMode || 9 > document.documentMode) && !t.button ? this._mouseUp(t) : this._mouseStarted ? (this._mouseDrag(t), t.preventDefault()) : (this._mouseDistanceMet(t) && this._mouseDelayMet(t) && (this._mouseStarted = this._mouseStart(this._mouseDownEvent, t) !== !1, this._mouseStarted ? this._mouseDrag(t) : this._mouseUp(t)), !this._mouseStarted) }, _mouseUp: function (t) { return e(document).unbind("mousemove." + this.widgetName, this._mouseMoveDelegate).unbind("mouseup." + this.widgetName, this._mouseUpDelegate), this._mouseStarted && (this._mouseStarted = !1, t.target === this._mouseDownEvent.target && e.data(t.target, this.widgetName + ".preventClickEvent", !0), this._mouseStop(t)), !1 }, _mouseDistanceMet: function (e) { return Math.max(Math.abs(this._mouseDownEvent.pageX - e.pageX), Math.abs(this._mouseDownEvent.pageY - e.pageY)) >= this.options.distance }, _mouseDelayMet: function () { return this.mouseDelayMet }, _mouseStart: function () { }, _mouseDrag: function () { }, _mouseStop: function () { }, _mouseCapture: function () { return !0 } }) })(jQuery); (function (t, e) { function i(t, e, i) { return [parseFloat(t[0]) * (p.test(t[0]) ? e / 100 : 1), parseFloat(t[1]) * (p.test(t[1]) ? i / 100 : 1)] } function s(e, i) { return parseInt(t.css(e, i), 10) || 0 } function n(e) { var i = e[0]; return 9 === i.nodeType ? { width: e.width(), height: e.height(), offset: { top: 0, left: 0 } } : t.isWindow(i) ? { width: e.width(), height: e.height(), offset: { top: e.scrollTop(), left: e.scrollLeft() } } : i.preventDefault ? { width: 0, height: 0, offset: { top: i.pageY, left: i.pageX } } : { width: e.outerWidth(), height: e.outerHeight(), offset: e.offset() } } t.ui = t.ui || {}; var a, o = Math.max, r = Math.abs, h = Math.round, l = /left|center|right/, c = /top|center|bottom/, u = /[\+\-]\d+(\.[\d]+)?%?/, d = /^\w+/, p = /%$/, f = t.fn.position; t.position = { scrollbarWidth: function () { if (a !== e) return a; var i, s, n = t("<div style='display:block;width:50px;height:50px;overflow:hidden;'><div style='height:100px;width:auto;'></div></div>"), o = n.children()[0]; return t("body").append(n), i = o.offsetWidth, n.css("overflow", "scroll"), s = o.offsetWidth, i === s && (s = n[0].clientWidth), n.remove(), a = i - s }, getScrollInfo: function (e) { var i = e.isWindow ? "" : e.element.css("overflow-x"), s = e.isWindow ? "" : e.element.css("overflow-y"), n = "scroll" === i || "auto" === i && e.width < e.element[0].scrollWidth, a = "scroll" === s || "auto" === s && e.height < e.element[0].scrollHeight; return { width: a ? t.position.scrollbarWidth() : 0, height: n ? t.position.scrollbarWidth() : 0 } }, getWithinInfo: function (e) { var i = t(e || window), s = t.isWindow(i[0]); return { element: i, isWindow: s, offset: i.offset() || { left: 0, top: 0 }, scrollLeft: i.scrollLeft(), scrollTop: i.scrollTop(), width: s ? i.width() : i.outerWidth(), height: s ? i.height() : i.outerHeight() } } }, t.fn.position = function (e) { if (!e || !e.of) return f.apply(this, arguments); e = t.extend({}, e); var a, p, m, g, v, b, _ = t(e.of), y = t.position.getWithinInfo(e.within), w = t.position.getScrollInfo(y), x = (e.collision || "flip").split(" "), k = {}; return b = n(_), _[0].preventDefault && (e.at = "left top"), p = b.width, m = b.height, g = b.offset, v = t.extend({}, g), t.each(["my", "at"], function () { var t, i, s = (e[this] || "").split(" "); 1 === s.length && (s = l.test(s[0]) ? s.concat(["center"]) : c.test(s[0]) ? ["center"].concat(s) : ["center", "center"]), s[0] = l.test(s[0]) ? s[0] : "center", s[1] = c.test(s[1]) ? s[1] : "center", t = u.exec(s[0]), i = u.exec(s[1]), k[this] = [t ? t[0] : 0, i ? i[0] : 0], e[this] = [d.exec(s[0])[0], d.exec(s[1])[0]] }), 1 === x.length && (x[1] = x[0]), "right" === e.at[0] ? v.left += p : "center" === e.at[0] && (v.left += p / 2), "bottom" === e.at[1] ? v.top += m : "center" === e.at[1] && (v.top += m / 2), a = i(k.at, p, m), v.left += a[0], v.top += a[1], this.each(function () { var n, l, c = t(this), u = c.outerWidth(), d = c.outerHeight(), f = s(this, "marginLeft"), b = s(this, "marginTop"), D = u + f + s(this, "marginRight") + w.width, T = d + b + s(this, "marginBottom") + w.height, C = t.extend({}, v), M = i(k.my, c.outerWidth(), c.outerHeight()); "right" === e.my[0] ? C.left -= u : "center" === e.my[0] && (C.left -= u / 2), "bottom" === e.my[1] ? C.top -= d : "center" === e.my[1] && (C.top -= d / 2), C.left += M[0], C.top += M[1], t.support.offsetFractions || (C.left = h(C.left), C.top = h(C.top)), n = { marginLeft: f, marginTop: b }, t.each(["left", "top"], function (i, s) { t.ui.position[x[i]] && t.ui.position[x[i]][s](C, { targetWidth: p, targetHeight: m, elemWidth: u, elemHeight: d, collisionPosition: n, collisionWidth: D, collisionHeight: T, offset: [a[0] + M[0], a[1] + M[1]], my: e.my, at: e.at, within: y, elem: c }) }), e.using && (l = function (t) { var i = g.left - C.left, s = i + p - u, n = g.top - C.top, a = n + m - d, h = { target: { element: _, left: g.left, top: g.top, width: p, height: m }, element: { element: c, left: C.left, top: C.top, width: u, height: d }, horizontal: 0 > s ? "left" : i > 0 ? "right" : "center", vertical: 0 > a ? "top" : n > 0 ? "bottom" : "middle" }; u > p && p > r(i + s) && (h.horizontal = "center"), d > m && m > r(n + a) && (h.vertical = "middle"), h.important = o(r(i), r(s)) > o(r(n), r(a)) ? "horizontal" : "vertical", e.using.call(this, t, h) }), c.offset(t.extend(C, { using: l })) }) }, t.ui.position = { fit: { left: function (t, e) { var i, s = e.within, n = s.isWindow ? s.scrollLeft : s.offset.left, a = s.width, r = t.left - e.collisionPosition.marginLeft, h = n - r, l = r + e.collisionWidth - a - n; e.collisionWidth > a ? h > 0 && 0 >= l ? (i = t.left + h + e.collisionWidth - a - n, t.left += h - i) : t.left = l > 0 && 0 >= h ? n : h > l ? n + a - e.collisionWidth : n : h > 0 ? t.left += h : l > 0 ? t.left -= l : t.left = o(t.left - r, t.left) }, top: function (t, e) { var i, s = e.within, n = s.isWindow ? s.scrollTop : s.offset.top, a = e.within.height, r = t.top - e.collisionPosition.marginTop, h = n - r, l = r + e.collisionHeight - a - n; e.collisionHeight > a ? h > 0 && 0 >= l ? (i = t.top + h + e.collisionHeight - a - n, t.top += h - i) : t.top = l > 0 && 0 >= h ? n : h > l ? n + a - e.collisionHeight : n : h > 0 ? t.top += h : l > 0 ? t.top -= l : t.top = o(t.top - r, t.top) } }, flip: { left: function (t, e) { var i, s, n = e.within, a = n.offset.left + n.scrollLeft, o = n.width, h = n.isWindow ? n.scrollLeft : n.offset.left, l = t.left - e.collisionPosition.marginLeft, c = l - h, u = l + e.collisionWidth - o - h, d = "left" === e.my[0] ? -e.elemWidth : "right" === e.my[0] ? e.elemWidth : 0, p = "left" === e.at[0] ? e.targetWidth : "right" === e.at[0] ? -e.targetWidth : 0, f = -2 * e.offset[0]; 0 > c ? (i = t.left + d + p + f + e.collisionWidth - o - a, (0 > i || r(c) > i) && (t.left += d + p + f)) : u > 0 && (s = t.left - e.collisionPosition.marginLeft + d + p + f - h, (s > 0 || u > r(s)) && (t.left += d + p + f)) }, top: function (t, e) { var i, s, n = e.within, a = n.offset.top + n.scrollTop, o = n.height, h = n.isWindow ? n.scrollTop : n.offset.top, l = t.top - e.collisionPosition.marginTop, c = l - h, u = l + e.collisionHeight - o - h, d = "top" === e.my[1], p = d ? -e.elemHeight : "bottom" === e.my[1] ? e.elemHeight : 0, f = "top" === e.at[1] ? e.targetHeight : "bottom" === e.at[1] ? -e.targetHeight : 0, m = -2 * e.offset[1]; 0 > c ? (s = t.top + p + f + m + e.collisionHeight - o - a, t.top + p + f + m > c && (0 > s || r(c) > s) && (t.top += p + f + m)) : u > 0 && (i = t.top - e.collisionPosition.marginTop + p + f + m - h, t.top + p + f + m > u && (i > 0 || u > r(i)) && (t.top += p + f + m)) } }, flipfit: { left: function () { t.ui.position.flip.left.apply(this, arguments), t.ui.position.fit.left.apply(this, arguments) }, top: function () { t.ui.position.flip.top.apply(this, arguments), t.ui.position.fit.top.apply(this, arguments) } } }, function () { var e, i, s, n, a, o = document.getElementsByTagName("body")[0], r = document.createElement("div"); e = document.createElement(o ? "div" : "body"), s = { visibility: "hidden", width: 0, height: 0, border: 0, margin: 0, background: "none" }, o && t.extend(s, { position: "absolute", left: "-1000px", top: "-1000px" }); for (a in s) e.style[a] = s[a]; e.appendChild(r), i = o || document.documentElement, i.insertBefore(e, i.firstChild), r.style.cssText = "position: absolute; left: 10.7432222px;", n = t(r).offset().left, t.support.offsetFractions = n > 10 && 11 > n, e.innerHTML = "", i.removeChild(e) }() })(jQuery); (function (e) { e.widget("ui.draggable", e.ui.mouse, { version: "1.10.3", widgetEventPrefix: "drag", options: { addClasses: !0, appendTo: "parent", axis: !1, connectToSortable: !1, containment: !1, cursor: "auto", cursorAt: !1, grid: !1, handle: !1, helper: "original", iframeFix: !1, opacity: !1, refreshPositions: !1, revert: !1, revertDuration: 500, scope: "default", scroll: !0, scrollSensitivity: 20, scrollSpeed: 20, snap: !1, snapMode: "both", snapTolerance: 20, stack: !1, zIndex: !1, drag: null, start: null, stop: null }, _create: function () { "original" !== this.options.helper || /^(?:r|a|f)/.test(this.element.css("position")) || (this.element[0].style.position = "relative"), this.options.addClasses && this.element.addClass("ui-draggable"), this.options.disabled && this.element.addClass("ui-draggable-disabled"), this._mouseInit() }, _destroy: function () { this.element.removeClass("ui-draggable ui-draggable-dragging ui-draggable-disabled"), this._mouseDestroy() }, _mouseCapture: function (t) { var i = this.options; return this.helper || i.disabled || e(t.target).closest(".ui-resizable-handle").length > 0 ? !1 : (this.handle = this._getHandle(t), this.handle ? (e(i.iframeFix === !0 ? "iframe" : i.iframeFix).each(function () { e("<div class='ui-draggable-iframeFix' style='background: #fff;'></div>").css({ width: this.offsetWidth + "px", height: this.offsetHeight + "px", position: "absolute", opacity: "0.001", zIndex: 1e3 }).css(e(this).offset()).appendTo("body") }), !0) : !1) }, _mouseStart: function (t) { var i = this.options; return this.helper = this._createHelper(t), this.helper.addClass("ui-draggable-dragging"), this._cacheHelperProportions(), e.ui.ddmanager && (e.ui.ddmanager.current = this), this._cacheMargins(), this.cssPosition = this.helper.css("position"), this.scrollParent = this.helper.scrollParent(), this.offsetParent = this.helper.offsetParent(), this.offsetParentCssPosition = this.offsetParent.css("position"), this.offset = this.positionAbs = this.element.offset(), this.offset = { top: this.offset.top - this.margins.top, left: this.offset.left - this.margins.left }, this.offset.scroll = !1, e.extend(this.offset, { click: { left: t.pageX - this.offset.left, top: t.pageY - this.offset.top }, parent: this._getParentOffset(), relative: this._getRelativeOffset() }), this.originalPosition = this.position = this._generatePosition(t), this.originalPageX = t.pageX, this.originalPageY = t.pageY, i.cursorAt && this._adjustOffsetFromHelper(i.cursorAt), this._setContainment(), this._trigger("start", t) === !1 ? (this._clear(), !1) : (this._cacheHelperProportions(), e.ui.ddmanager && !i.dropBehaviour && e.ui.ddmanager.prepareOffsets(this, t), this._mouseDrag(t, !0), e.ui.ddmanager && e.ui.ddmanager.dragStart(this, t), !0) }, _mouseDrag: function (t, i) { if ("fixed" === this.offsetParentCssPosition && (this.offset.parent = this._getParentOffset()), this.position = this._generatePosition(t), this.positionAbs = this._convertPositionTo("absolute"), !i) { var s = this._uiHash(); if (this._trigger("drag", t, s) === !1) return this._mouseUp({}), !1; this.position = s.position } return this.options.axis && "y" === this.options.axis || (this.helper[0].style.left = this.position.left + "px"), this.options.axis && "x" === this.options.axis || (this.helper[0].style.top = this.position.top + "px"), e.ui.ddmanager && e.ui.ddmanager.drag(this, t), !1 }, _mouseStop: function (t) { var i = this, s = !1; return e.ui.ddmanager && !this.options.dropBehaviour && (s = e.ui.ddmanager.drop(this, t)), this.dropped && (s = this.dropped, this.dropped = !1), "original" !== this.options.helper || e.contains(this.element[0].ownerDocument, this.element[0]) ? ("invalid" === this.options.revert && !s || "valid" === this.options.revert && s || this.options.revert === !0 || e.isFunction(this.options.revert) && this.options.revert.call(this.element, s) ? e(this.helper).animate(this.originalPosition, parseInt(this.options.revertDuration, 10), function () { i._trigger("stop", t) !== !1 && i._clear() }) : this._trigger("stop", t) !== !1 && this._clear(), !1) : !1 }, _mouseUp: function (t) { return e("div.ui-draggable-iframeFix").each(function () { this.parentNode.removeChild(this) }), e.ui.ddmanager && e.ui.ddmanager.dragStop(this, t), e.ui.mouse.prototype._mouseUp.call(this, t) }, cancel: function () { return this.helper.is(".ui-draggable-dragging") ? this._mouseUp({}) : this._clear(), this }, _getHandle: function (t) { return this.options.handle ? !!e(t.target).closest(this.element.find(this.options.handle)).length : !0 }, _createHelper: function (t) { var i = this.options, s = e.isFunction(i.helper) ? e(i.helper.apply(this.element[0], [t])) : "clone" === i.helper ? this.element.clone().removeAttr("id") : this.element; return s.parents("body").length || s.appendTo("parent" === i.appendTo ? this.element[0].parentNode : i.appendTo), s[0] === this.element[0] || /(fixed|absolute)/.test(s.css("position")) || s.css("position", "absolute"), s }, _adjustOffsetFromHelper: function (t) { "string" == typeof t && (t = t.split(" ")), e.isArray(t) && (t = { left: +t[0], top: +t[1] || 0 }), "left" in t && (this.offset.click.left = t.left + this.margins.left), "right" in t && (this.offset.click.left = this.helperProportions.width - t.right + this.margins.left), "top" in t && (this.offset.click.top = t.top + this.margins.top), "bottom" in t && (this.offset.click.top = this.helperProportions.height - t.bottom + this.margins.top) }, _getParentOffset: function () { var t = this.offsetParent.offset(); return "absolute" === this.cssPosition && this.scrollParent[0] !== document && e.contains(this.scrollParent[0], this.offsetParent[0]) && (t.left += this.scrollParent.scrollLeft(), t.top += this.scrollParent.scrollTop()), (this.offsetParent[0] === document.body || this.offsetParent[0].tagName && "html" === this.offsetParent[0].tagName.toLowerCase() && e.ui.ie) && (t = { top: 0, left: 0 }), { top: t.top + (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0), left: t.left + (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0) } }, _getRelativeOffset: function () { if ("relative" === this.cssPosition) { var e = this.element.position(); return { top: e.top - (parseInt(this.helper.css("top"), 10) || 0) + this.scrollParent.scrollTop(), left: e.left - (parseInt(this.helper.css("left"), 10) || 0) + this.scrollParent.scrollLeft() } } return { top: 0, left: 0 } }, _cacheMargins: function () { this.margins = { left: parseInt(this.element.css("marginLeft"), 10) || 0, top: parseInt(this.element.css("marginTop"), 10) || 0, right: parseInt(this.element.css("marginRight"), 10) || 0, bottom: parseInt(this.element.css("marginBottom"), 10) || 0 } }, _cacheHelperProportions: function () { this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() } }, _setContainment: function () { var t, i, s, n = this.options; return n.containment ? "window" === n.containment ? (this.containment = [e(window).scrollLeft() - this.offset.relative.left - this.offset.parent.left, e(window).scrollTop() - this.offset.relative.top - this.offset.parent.top, e(window).scrollLeft() + e(window).width() - this.helperProportions.width - this.margins.left, e(window).scrollTop() + (e(window).height() || document.body.parentNode.scrollHeight) - this.helperProportions.height - this.margins.top], undefined) : "document" === n.containment ? (this.containment = [0, 0, e(document).width() - this.helperProportions.width - this.margins.left, (e(document).height() || document.body.parentNode.scrollHeight) - this.helperProportions.height - this.margins.top], undefined) : n.containment.constructor === Array ? (this.containment = n.containment, undefined) : ("parent" === n.containment && (n.containment = this.helper[0].parentNode), i = e(n.containment), s = i[0], s && (t = "hidden" !== i.css("overflow"), this.containment = [(parseInt(i.css("borderLeftWidth"), 10) || 0) + (parseInt(i.css("paddingLeft"), 10) || 0), (parseInt(i.css("borderTopWidth"), 10) || 0) + (parseInt(i.css("paddingTop"), 10) || 0), (t ? Math.max(s.scrollWidth, s.offsetWidth) : s.offsetWidth) - (parseInt(i.css("borderRightWidth"), 10) || 0) - (parseInt(i.css("paddingRight"), 10) || 0) - this.helperProportions.width - this.margins.left - this.margins.right, (t ? Math.max(s.scrollHeight, s.offsetHeight) : s.offsetHeight) - (parseInt(i.css("borderBottomWidth"), 10) || 0) - (parseInt(i.css("paddingBottom"), 10) || 0) - this.helperProportions.height - this.margins.top - this.margins.bottom], this.relative_container = i), undefined) : (this.containment = null, undefined) }, _convertPositionTo: function (t, i) { i || (i = this.position); var s = "absolute" === t ? 1 : -1, n = "absolute" !== this.cssPosition || this.scrollParent[0] !== document && e.contains(this.scrollParent[0], this.offsetParent[0]) ? this.scrollParent : this.offsetParent; return this.offset.scroll || (this.offset.scroll = { top: n.scrollTop(), left: n.scrollLeft() }), { top: i.top + this.offset.relative.top * s + this.offset.parent.top * s - ("fixed" === this.cssPosition ? -this.scrollParent.scrollTop() : this.offset.scroll.top) * s, left: i.left + this.offset.relative.left * s + this.offset.parent.left * s - ("fixed" === this.cssPosition ? -this.scrollParent.scrollLeft() : this.offset.scroll.left) * s } }, _generatePosition: function (t) { var i, s, n, a, o = this.options, r = "absolute" !== this.cssPosition || this.scrollParent[0] !== document && e.contains(this.scrollParent[0], this.offsetParent[0]) ? this.scrollParent : this.offsetParent, h = t.pageX, l = t.pageY; return this.offset.scroll || (this.offset.scroll = { top: r.scrollTop(), left: r.scrollLeft() }), this.originalPosition && (this.containment && (this.relative_container ? (s = this.relative_container.offset(), i = [this.containment[0] + s.left, this.containment[1] + s.top, this.containment[2] + s.left, this.containment[3] + s.top]) : i = this.containment, t.pageX - this.offset.click.left < i[0] && (h = i[0] + this.offset.click.left), t.pageY - this.offset.click.top < i[1] && (l = i[1] + this.offset.click.top), t.pageX - this.offset.click.left > i[2] && (h = i[2] + this.offset.click.left), t.pageY - this.offset.click.top > i[3] && (l = i[3] + this.offset.click.top)), o.grid && (n = o.grid[1] ? this.originalPageY + Math.round((l - this.originalPageY) / o.grid[1]) * o.grid[1] : this.originalPageY, l = i ? n - this.offset.click.top >= i[1] || n - this.offset.click.top > i[3] ? n : n - this.offset.click.top >= i[1] ? n - o.grid[1] : n + o.grid[1] : n, a = o.grid[0] ? this.originalPageX + Math.round((h - this.originalPageX) / o.grid[0]) * o.grid[0] : this.originalPageX, h = i ? a - this.offset.click.left >= i[0] || a - this.offset.click.left > i[2] ? a : a - this.offset.click.left >= i[0] ? a - o.grid[0] : a + o.grid[0] : a)), { top: l - this.offset.click.top - this.offset.relative.top - this.offset.parent.top + ("fixed" === this.cssPosition ? -this.scrollParent.scrollTop() : this.offset.scroll.top), left: h - this.offset.click.left - this.offset.relative.left - this.offset.parent.left + ("fixed" === this.cssPosition ? -this.scrollParent.scrollLeft() : this.offset.scroll.left) } }, _clear: function () { this.helper.removeClass("ui-draggable-dragging"), this.helper[0] === this.element[0] || this.cancelHelperRemoval || this.helper.remove(), this.helper = null, this.cancelHelperRemoval = !1 }, _trigger: function (t, i, s) { return s = s || this._uiHash(), e.ui.plugin.call(this, t, [i, s]), "drag" === t && (this.positionAbs = this._convertPositionTo("absolute")), e.Widget.prototype._trigger.call(this, t, i, s) }, plugins: {}, _uiHash: function () { return { helper: this.helper, position: this.position, originalPosition: this.originalPosition, offset: this.positionAbs } } }), e.ui.plugin.add("draggable", "connectToSortable", { start: function (t, i) { var s = e(this).data("ui-draggable"), n = s.options, a = e.extend({}, i, { item: s.element }); s.sortables = [], e(n.connectToSortable).each(function () { var i = e.data(this, "ui-sortable"); i && !i.options.disabled && (s.sortables.push({ instance: i, shouldRevert: i.options.revert }), i.refreshPositions(), i._trigger("activate", t, a)) }) }, stop: function (t, i) { var s = e(this).data("ui-draggable"), n = e.extend({}, i, { item: s.element }); e.each(s.sortables, function () { this.instance.isOver ? (this.instance.isOver = 0, s.cancelHelperRemoval = !0, this.instance.cancelHelperRemoval = !1, this.shouldRevert && (this.instance.options.revert = this.shouldRevert), this.instance._mouseStop(t), this.instance.options.helper = this.instance.options._helper, "original" === s.options.helper && this.instance.currentItem.css({ top: "auto", left: "auto" })) : (this.instance.cancelHelperRemoval = !1, this.instance._trigger("deactivate", t, n)) }) }, drag: function (t, i) { var s = e(this).data("ui-draggable"), n = this; e.each(s.sortables, function () { var a = !1, o = this; this.instance.positionAbs = s.positionAbs, this.instance.helperProportions = s.helperProportions, this.instance.offset.click = s.offset.click, this.instance._intersectsWith(this.instance.containerCache) && (a = !0, e.each(s.sortables, function () { return this.instance.positionAbs = s.positionAbs, this.instance.helperProportions = s.helperProportions, this.instance.offset.click = s.offset.click, this !== o && this.instance._intersectsWith(this.instance.containerCache) && e.contains(o.instance.element[0], this.instance.element[0]) && (a = !1), a })), a ? (this.instance.isOver || (this.instance.isOver = 1, this.instance.currentItem = e(n).clone().removeAttr("id").appendTo(this.instance.element).data("ui-sortable-item", !0), this.instance.options._helper = this.instance.options.helper, this.instance.options.helper = function () { return i.helper[0] }, t.target = this.instance.currentItem[0], this.instance._mouseCapture(t, !0), this.instance._mouseStart(t, !0, !0), this.instance.offset.click.top = s.offset.click.top, this.instance.offset.click.left = s.offset.click.left, this.instance.offset.parent.left -= s.offset.parent.left - this.instance.offset.parent.left, this.instance.offset.parent.top -= s.offset.parent.top - this.instance.offset.parent.top, s._trigger("toSortable", t), s.dropped = this.instance.element, s.currentItem = s.element, this.instance.fromOutside = s), this.instance.currentItem && this.instance._mouseDrag(t)) : this.instance.isOver && (this.instance.isOver = 0, this.instance.cancelHelperRemoval = !0, this.instance.options.revert = !1, this.instance._trigger("out", t, this.instance._uiHash(this.instance)), this.instance._mouseStop(t, !0), this.instance.options.helper = this.instance.options._helper, this.instance.currentItem.remove(), this.instance.placeholder && this.instance.placeholder.remove(), s._trigger("fromSortable", t), s.dropped = !1) }) } }), e.ui.plugin.add("draggable", "cursor", { start: function () { var t = e("body"), i = e(this).data("ui-draggable").options; t.css("cursor") && (i._cursor = t.css("cursor")), t.css("cursor", i.cursor) }, stop: function () { var t = e(this).data("ui-draggable").options; t._cursor && e("body").css("cursor", t._cursor) } }), e.ui.plugin.add("draggable", "opacity", { start: function (t, i) { var s = e(i.helper), n = e(this).data("ui-draggable").options; s.css("opacity") && (n._opacity = s.css("opacity")), s.css("opacity", n.opacity) }, stop: function (t, i) { var s = e(this).data("ui-draggable").options; s._opacity && e(i.helper).css("opacity", s._opacity) } }), e.ui.plugin.add("draggable", "scroll", { start: function () { var t = e(this).data("ui-draggable"); t.scrollParent[0] !== document && "HTML" !== t.scrollParent[0].tagName && (t.overflowOffset = t.scrollParent.offset()) }, drag: function (t) { var i = e(this).data("ui-draggable"), s = i.options, n = !1; i.scrollParent[0] !== document && "HTML" !== i.scrollParent[0].tagName ? (s.axis && "x" === s.axis || (i.overflowOffset.top + i.scrollParent[0].offsetHeight - t.pageY < s.scrollSensitivity ? i.scrollParent[0].scrollTop = n = i.scrollParent[0].scrollTop + s.scrollSpeed : t.pageY - i.overflowOffset.top < s.scrollSensitivity && (i.scrollParent[0].scrollTop = n = i.scrollParent[0].scrollTop - s.scrollSpeed)), s.axis && "y" === s.axis || (i.overflowOffset.left + i.scrollParent[0].offsetWidth - t.pageX < s.scrollSensitivity ? i.scrollParent[0].scrollLeft = n = i.scrollParent[0].scrollLeft + s.scrollSpeed : t.pageX - i.overflowOffset.left < s.scrollSensitivity && (i.scrollParent[0].scrollLeft = n = i.scrollParent[0].scrollLeft - s.scrollSpeed))) : (s.axis && "x" === s.axis || (t.pageY - e(document).scrollTop() < s.scrollSensitivity ? n = e(document).scrollTop(e(document).scrollTop() - s.scrollSpeed) : e(window).height() - (t.pageY - e(document).scrollTop()) < s.scrollSensitivity && (n = e(document).scrollTop(e(document).scrollTop() + s.scrollSpeed))), s.axis && "y" === s.axis || (t.pageX - e(document).scrollLeft() < s.scrollSensitivity ? n = e(document).scrollLeft(e(document).scrollLeft() - s.scrollSpeed) : e(window).width() - (t.pageX - e(document).scrollLeft()) < s.scrollSensitivity && (n = e(document).scrollLeft(e(document).scrollLeft() + s.scrollSpeed)))), n !== !1 && e.ui.ddmanager && !s.dropBehaviour && e.ui.ddmanager.prepareOffsets(i, t) } }), e.ui.plugin.add("draggable", "snap", { start: function () { var t = e(this).data("ui-draggable"), i = t.options; t.snapElements = [], e(i.snap.constructor !== String ? i.snap.items || ":data(ui-draggable)" : i.snap).each(function () { var i = e(this), s = i.offset(); this !== t.element[0] && t.snapElements.push({ item: this, width: i.outerWidth(), height: i.outerHeight(), top: s.top, left: s.left }) }) }, drag: function (t, i) { var s, n, a, o, r, h, l, u, c, d, p = e(this).data("ui-draggable"), f = p.options, m = f.snapTolerance, g = i.offset.left, v = g + p.helperProportions.width, b = i.offset.top, y = b + p.helperProportions.height; for (c = p.snapElements.length - 1; c >= 0; c--) r = p.snapElements[c].left, h = r + p.snapElements[c].width, l = p.snapElements[c].top, u = l + p.snapElements[c].height, r - m > v || g > h + m || l - m > y || b > u + m || !e.contains(p.snapElements[c].item.ownerDocument, p.snapElements[c].item) ? (p.snapElements[c].snapping && p.options.snap.release && p.options.snap.release.call(p.element, t, e.extend(p._uiHash(), { snapItem: p.snapElements[c].item })), p.snapElements[c].snapping = !1) : ("inner" !== f.snapMode && (s = m >= Math.abs(l - y), n = m >= Math.abs(u - b), a = m >= Math.abs(r - v), o = m >= Math.abs(h - g), s && (i.position.top = p._convertPositionTo("relative", { top: l - p.helperProportions.height, left: 0 }).top - p.margins.top), n && (i.position.top = p._convertPositionTo("relative", { top: u, left: 0 }).top - p.margins.top), a && (i.position.left = p._convertPositionTo("relative", { top: 0, left: r - p.helperProportions.width }).left - p.margins.left), o && (i.position.left = p._convertPositionTo("relative", { top: 0, left: h }).left - p.margins.left)), d = s || n || a || o, "outer" !== f.snapMode && (s = m >= Math.abs(l - b), n = m >= Math.abs(u - y), a = m >= Math.abs(r - g), o = m >= Math.abs(h - v), s && (i.position.top = p._convertPositionTo("relative", { top: l, left: 0 }).top - p.margins.top), n && (i.position.top = p._convertPositionTo("relative", { top: u - p.helperProportions.height, left: 0 }).top - p.margins.top), a && (i.position.left = p._convertPositionTo("relative", { top: 0, left: r }).left - p.margins.left), o && (i.position.left = p._convertPositionTo("relative", { top: 0, left: h - p.helperProportions.width }).left - p.margins.left)), !p.snapElements[c].snapping && (s || n || a || o || d) && p.options.snap.snap && p.options.snap.snap.call(p.element, t, e.extend(p._uiHash(), { snapItem: p.snapElements[c].item })), p.snapElements[c].snapping = s || n || a || o || d) } }), e.ui.plugin.add("draggable", "stack", { start: function () { var t, i = this.data("ui-draggable").options, s = e.makeArray(e(i.stack)).sort(function (t, i) { return (parseInt(e(t).css("zIndex"), 10) || 0) - (parseInt(e(i).css("zIndex"), 10) || 0) }); s.length && (t = parseInt(e(s[0]).css("zIndex"), 10) || 0, e(s).each(function (i) { e(this).css("zIndex", t + i) }), this.css("zIndex", t + s.length)) } }), e.ui.plugin.add("draggable", "zIndex", { start: function (t, i) { var s = e(i.helper), n = e(this).data("ui-draggable").options; s.css("zIndex") && (n._zIndex = s.css("zIndex")), s.css("zIndex", n.zIndex) }, stop: function (t, i) { var s = e(this).data("ui-draggable").options; s._zIndex && e(i.helper).css("zIndex", s._zIndex) } }) })(jQuery); (function (e) { function t(e, t, i) { return e > t && t + i > e } e.widget("ui.droppable", { version: "1.10.3", widgetEventPrefix: "drop", options: { accept: "*", activeClass: !1, addClasses: !0, greedy: !1, hoverClass: !1, scope: "default", tolerance: "intersect", activate: null, deactivate: null, drop: null, out: null, over: null }, _create: function () { var t = this.options, i = t.accept; this.isover = !1, this.isout = !0, this.accept = e.isFunction(i) ? i : function (e) { return e.is(i) }, this.proportions = { width: this.element[0].offsetWidth, height: this.element[0].offsetHeight }, e.ui.ddmanager.droppables[t.scope] = e.ui.ddmanager.droppables[t.scope] || [], e.ui.ddmanager.droppables[t.scope].push(this), t.addClasses && this.element.addClass("ui-droppable") }, _destroy: function () { for (var t = 0, i = e.ui.ddmanager.droppables[this.options.scope]; i.length > t; t++) i[t] === this && i.splice(t, 1); this.element.removeClass("ui-droppable ui-droppable-disabled") }, _setOption: function (t, i) { "accept" === t && (this.accept = e.isFunction(i) ? i : function (e) { return e.is(i) }), e.Widget.prototype._setOption.apply(this, arguments) }, _activate: function (t) { var i = e.ui.ddmanager.current; this.options.activeClass && this.element.addClass(this.options.activeClass), i && this._trigger("activate", t, this.ui(i)) }, _deactivate: function (t) { var i = e.ui.ddmanager.current; this.options.activeClass && this.element.removeClass(this.options.activeClass), i && this._trigger("deactivate", t, this.ui(i)) }, _over: function (t) { var i = e.ui.ddmanager.current; i && (i.currentItem || i.element)[0] !== this.element[0] && this.accept.call(this.element[0], i.currentItem || i.element) && (this.options.hoverClass && this.element.addClass(this.options.hoverClass), this._trigger("over", t, this.ui(i))) }, _out: function (t) { var i = e.ui.ddmanager.current; i && (i.currentItem || i.element)[0] !== this.element[0] && this.accept.call(this.element[0], i.currentItem || i.element) && (this.options.hoverClass && this.element.removeClass(this.options.hoverClass), this._trigger("out", t, this.ui(i))) }, _drop: function (t, i) { var s = i || e.ui.ddmanager.current, n = !1; return s && (s.currentItem || s.element)[0] !== this.element[0] ? (this.element.find(":data(ui-droppable)").not(".ui-draggable-dragging").each(function () { var t = e.data(this, "ui-droppable"); return t.options.greedy && !t.options.disabled && t.options.scope === s.options.scope && t.accept.call(t.element[0], s.currentItem || s.element) && e.ui.intersect(s, e.extend(t, { offset: t.element.offset() }), t.options.tolerance) ? (n = !0, !1) : undefined }), n ? !1 : this.accept.call(this.element[0], s.currentItem || s.element) ? (this.options.activeClass && this.element.removeClass(this.options.activeClass), this.options.hoverClass && this.element.removeClass(this.options.hoverClass), this._trigger("drop", t, this.ui(s)), this.element) : !1) : !1 }, ui: function (e) { return { draggable: e.currentItem || e.element, helper: e.helper, position: e.position, offset: e.positionAbs } } }), e.ui.intersect = function (e, i, s) { if (!i.offset) return !1; var n, a, o = (e.positionAbs || e.position.absolute).left, r = o + e.helperProportions.width, h = (e.positionAbs || e.position.absolute).top, l = h + e.helperProportions.height, u = i.offset.left, c = u + i.proportions.width, d = i.offset.top, p = d + i.proportions.height; switch (s) { case "fit": return o >= u && c >= r && h >= d && p >= l; case "intersect": return o + e.helperProportions.width / 2 > u && c > r - e.helperProportions.width / 2 && h + e.helperProportions.height / 2 > d && p > l - e.helperProportions.height / 2; case "pointer": return n = (e.positionAbs || e.position.absolute).left + (e.clickOffset || e.offset.click).left, a = (e.positionAbs || e.position.absolute).top + (e.clickOffset || e.offset.click).top, t(a, d, i.proportions.height) && t(n, u, i.proportions.width); case "touch": return (h >= d && p >= h || l >= d && p >= l || d > h && l > p) && (o >= u && c >= o || r >= u && c >= r || u > o && r > c); default: return !1 } }, e.ui.ddmanager = { current: null, droppables: { "default": [] }, prepareOffsets: function (t, i) { var s, n, a = e.ui.ddmanager.droppables[t.options.scope] || [], o = i ? i.type : null, r = (t.currentItem || t.element).find(":data(ui-droppable)").addBack(); e: for (s = 0; a.length > s; s++) if (!(a[s].options.disabled || t && !a[s].accept.call(a[s].element[0], t.currentItem || t.element))) { for (n = 0; r.length > n; n++) if (r[n] === a[s].element[0]) { a[s].proportions.height = 0; continue e } a[s].visible = "none" !== a[s].element.css("display"), a[s].visible && ("mousedown" === o && a[s]._activate.call(a[s], i), a[s].offset = a[s].element.offset(), a[s].proportions = { width: a[s].element[0].offsetWidth, height: a[s].element[0].offsetHeight }) } }, drop: function (t, i) { var s = !1; return e.each((e.ui.ddmanager.droppables[t.options.scope] || []).slice(), function () { this.options && (!this.options.disabled && this.visible && e.ui.intersect(t, this, this.options.tolerance) && (s = this._drop.call(this, i) || s), !this.options.disabled && this.visible && this.accept.call(this.element[0], t.currentItem || t.element) && (this.isout = !0, this.isover = !1, this._deactivate.call(this, i))) }), s }, dragStart: function (t, i) { t.element.parentsUntil("body").bind("scroll.droppable", function () { t.options.refreshPositions || e.ui.ddmanager.prepareOffsets(t, i) }) }, drag: function (t, i) { t.options.refreshPositions && e.ui.ddmanager.prepareOffsets(t, i), e.each(e.ui.ddmanager.droppables[t.options.scope] || [], function () { if (!this.options.disabled && !this.greedyChild && this.visible) { var s, n, a, o = e.ui.intersect(t, this, this.options.tolerance), r = !o && this.isover ? "isout" : o && !this.isover ? "isover" : null; r && (this.options.greedy && (n = this.options.scope, a = this.element.parents(":data(ui-droppable)").filter(function () { return e.data(this, "ui-droppable").options.scope === n }), a.length && (s = e.data(a[0], "ui-droppable"), s.greedyChild = "isover" === r)), s && "isover" === r && (s.isover = !1, s.isout = !0, s._out.call(s, i)), this[r] = !0, this["isout" === r ? "isover" : "isout"] = !1, this["isover" === r ? "_over" : "_out"].call(this, i), s && "isout" === r && (s.isout = !1, s.isover = !0, s._over.call(s, i))) } }) }, dragStop: function (t, i) { t.element.parentsUntil("body").unbind("scroll.droppable"), t.options.refreshPositions || e.ui.ddmanager.prepareOffsets(t, i) } } })(jQuery); (function (e) { function t(e) { return parseInt(e, 10) || 0 } function i(e) { return !isNaN(parseInt(e, 10)) } e.widget("ui.resizable", e.ui.mouse, { version: "1.10.3", widgetEventPrefix: "resize", options: { alsoResize: !1, animate: !1, animateDuration: "slow", animateEasing: "swing", aspectRatio: !1, autoHide: !1, containment: !1, ghost: !1, grid: !1, handles: "e,s,se", helper: !1, maxHeight: null, maxWidth: null, minHeight: 10, minWidth: 10, zIndex: 90, resize: null, start: null, stop: null }, _create: function () { var t, i, s, n, a, o = this, r = this.options; if (this.element.addClass("ui-resizable"), e.extend(this, { _aspectRatio: !!r.aspectRatio, aspectRatio: r.aspectRatio, originalElement: this.element, _proportionallyResizeElements: [], _helper: r.helper || r.ghost || r.animate ? r.helper || "ui-resizable-helper" : null }), this.element[0].nodeName.match(/canvas|textarea|input|select|button|img/i) && (this.element.wrap(e("<div class='ui-wrapper' style='overflow: hidden;'></div>").css({ position: this.element.css("position"), width: this.element.outerWidth(), height: this.element.outerHeight(), top: this.element.css("top"), left: this.element.css("left") })), this.element = this.element.parent().data("ui-resizable", this.element.data("ui-resizable")), this.elementIsWrapper = !0, this.element.css({ marginLeft: this.originalElement.css("marginLeft"), marginTop: this.originalElement.css("marginTop"), marginRight: this.originalElement.css("marginRight"), marginBottom: this.originalElement.css("marginBottom") }), this.originalElement.css({ marginLeft: 0, marginTop: 0, marginRight: 0, marginBottom: 0 }), this.originalResizeStyle = this.originalElement.css("resize"), this.originalElement.css("resize", "none"), this._proportionallyResizeElements.push(this.originalElement.css({ position: "static", zoom: 1, display: "block" })), this.originalElement.css({ margin: this.originalElement.css("margin") }), this._proportionallyResize()), this.handles = r.handles || (e(".ui-resizable-handle", this.element).length ? { n: ".ui-resizable-n", e: ".ui-resizable-e", s: ".ui-resizable-s", w: ".ui-resizable-w", se: ".ui-resizable-se", sw: ".ui-resizable-sw", ne: ".ui-resizable-ne", nw: ".ui-resizable-nw" } : "e,s,se"), this.handles.constructor === String) for ("all" === this.handles && (this.handles = "n,e,s,w,se,sw,ne,nw"), t = this.handles.split(","), this.handles = {}, i = 0; t.length > i; i++) s = e.trim(t[i]), a = "ui-resizable-" + s, n = e("<div class='ui-resizable-handle " + a + "'></div>"), n.css({ zIndex: r.zIndex }), "se" === s && n.addClass("ui-icon ui-icon-gripsmall-diagonal-se"), this.handles[s] = ".ui-resizable-" + s, this.element.append(n); this._renderAxis = function (t) { var i, s, n, a; t = t || this.element; for (i in this.handles) this.handles[i].constructor === String && (this.handles[i] = e(this.handles[i], this.element).show()), this.elementIsWrapper && this.originalElement[0].nodeName.match(/textarea|input|select|button/i) && (s = e(this.handles[i], this.element), a = /sw|ne|nw|se|n|s/.test(i) ? s.outerHeight() : s.outerWidth(), n = ["padding", /ne|nw|n/.test(i) ? "Top" : /se|sw|s/.test(i) ? "Bottom" : /^e$/.test(i) ? "Right" : "Left"].join(""), t.css(n, a), this._proportionallyResize()), e(this.handles[i]).length }, this._renderAxis(this.element), this._handles = e(".ui-resizable-handle", this.element).disableSelection(), this._handles.mouseover(function () { o.resizing || (this.className && (n = this.className.match(/ui-resizable-(se|sw|ne|nw|n|e|s|w)/i)), o.axis = n && n[1] ? n[1] : "se") }), r.autoHide && (this._handles.hide(), e(this.element).addClass("ui-resizable-autohide").mouseenter(function () { r.disabled || (e(this).removeClass("ui-resizable-autohide"), o._handles.show()) }).mouseleave(function () { r.disabled || o.resizing || (e(this).addClass("ui-resizable-autohide"), o._handles.hide()) })), this._mouseInit() }, _destroy: function () { this._mouseDestroy(); var t, i = function (t) { e(t).removeClass("ui-resizable ui-resizable-disabled ui-resizable-resizing").removeData("resizable").removeData("ui-resizable").unbind(".resizable").find(".ui-resizable-handle").remove() }; return this.elementIsWrapper && (i(this.element), t = this.element, this.originalElement.css({ position: t.css("position"), width: t.outerWidth(), height: t.outerHeight(), top: t.css("top"), left: t.css("left") }).insertAfter(t), t.remove()), this.originalElement.css("resize", this.originalResizeStyle), i(this.originalElement), this }, _mouseCapture: function (t) { var i, s, n = !1; for (i in this.handles) s = e(this.handles[i])[0], (s === t.target || e.contains(s, t.target)) && (n = !0); return !this.options.disabled && n }, _mouseStart: function (i) { var s, n, a, o = this.options, r = this.element.position(), h = this.element; return this.resizing = !0, /absolute/.test(h.css("position")) ? h.css({ position: "absolute", top: h.css("top"), left: h.css("left") }) : h.is(".ui-draggable") && h.css({ position: "absolute", top: r.top, left: r.left }), this._renderProxy(), s = t(this.helper.css("left")), n = t(this.helper.css("top")), o.containment && (s += e(o.containment).scrollLeft() || 0, n += e(o.containment).scrollTop() || 0), this.offset = this.helper.offset(), this.position = { left: s, top: n }, this.size = this._helper ? { width: h.outerWidth(), height: h.outerHeight() } : { width: h.width(), height: h.height() }, this.originalSize = this._helper ? { width: h.outerWidth(), height: h.outerHeight() } : { width: h.width(), height: h.height() }, this.originalPosition = { left: s, top: n }, this.sizeDiff = { width: h.outerWidth() - h.width(), height: h.outerHeight() - h.height() }, this.originalMousePosition = { left: i.pageX, top: i.pageY }, this.aspectRatio = "number" == typeof o.aspectRatio ? o.aspectRatio : this.originalSize.width / this.originalSize.height || 1, a = e(".ui-resizable-" + this.axis).css("cursor"), e("body").css("cursor", "auto" === a ? this.axis + "-resize" : a), h.addClass("ui-resizable-resizing"), this._propagate("start", i), !0 }, _mouseDrag: function (t) { var i, s = this.helper, n = {}, a = this.originalMousePosition, o = this.axis, r = this.position.top, h = this.position.left, l = this.size.width, u = this.size.height, c = t.pageX - a.left || 0, d = t.pageY - a.top || 0, p = this._change[o]; return p ? (i = p.apply(this, [t, c, d]), this._updateVirtualBoundaries(t.shiftKey), (this._aspectRatio || t.shiftKey) && (i = this._updateRatio(i, t)), i = this._respectSize(i, t), this._updateCache(i), this._propagate("resize", t), this.position.top !== r && (n.top = this.position.top + "px"), this.position.left !== h && (n.left = this.position.left + "px"), this.size.width !== l && (n.width = this.size.width + "px"), this.size.height !== u && (n.height = this.size.height + "px"), s.css(n), !this._helper && this._proportionallyResizeElements.length && this._proportionallyResize(), e.isEmptyObject(n) || this._trigger("resize", t, this.ui()), !1) : !1 }, _mouseStop: function (t) { this.resizing = !1; var i, s, n, a, o, r, h, l = this.options, u = this; return this._helper && (i = this._proportionallyResizeElements, s = i.length && /textarea/i.test(i[0].nodeName), n = s && e.ui.hasScroll(i[0], "left") ? 0 : u.sizeDiff.height, a = s ? 0 : u.sizeDiff.width, o = { width: u.helper.width() - a, height: u.helper.height() - n }, r = parseInt(u.element.css("left"), 10) + (u.position.left - u.originalPosition.left) || null, h = parseInt(u.element.css("top"), 10) + (u.position.top - u.originalPosition.top) || null, l.animate || this.element.css(e.extend(o, { top: h, left: r })), u.helper.height(u.size.height), u.helper.width(u.size.width), this._helper && !l.animate && this._proportionallyResize()), e("body").css("cursor", "auto"), this.element.removeClass("ui-resizable-resizing"), this._propagate("stop", t), this._helper && this.helper.remove(), !1 }, _updateVirtualBoundaries: function (e) { var t, s, n, a, o, r = this.options; o = { minWidth: i(r.minWidth) ? r.minWidth : 0, maxWidth: i(r.maxWidth) ? r.maxWidth : 1 / 0, minHeight: i(r.minHeight) ? r.minHeight : 0, maxHeight: i(r.maxHeight) ? r.maxHeight : 1 / 0 }, (this._aspectRatio || e) && (t = o.minHeight * this.aspectRatio, n = o.minWidth / this.aspectRatio, s = o.maxHeight * this.aspectRatio, a = o.maxWidth / this.aspectRatio, t > o.minWidth && (o.minWidth = t), n > o.minHeight && (o.minHeight = n), o.maxWidth > s && (o.maxWidth = s), o.maxHeight > a && (o.maxHeight = a)), this._vBoundaries = o }, _updateCache: function (e) { this.offset = this.helper.offset(), i(e.left) && (this.position.left = e.left), i(e.top) && (this.position.top = e.top), i(e.height) && (this.size.height = e.height), i(e.width) && (this.size.width = e.width) }, _updateRatio: function (e) { var t = this.position, s = this.size, n = this.axis; return i(e.height) ? e.width = e.height * this.aspectRatio : i(e.width) && (e.height = e.width / this.aspectRatio), "sw" === n && (e.left = t.left + (s.width - e.width), e.top = null), "nw" === n && (e.top = t.top + (s.height - e.height), e.left = t.left + (s.width - e.width)), e }, _respectSize: function (e) { var t = this._vBoundaries, s = this.axis, n = i(e.width) && t.maxWidth && t.maxWidth < e.width, a = i(e.height) && t.maxHeight && t.maxHeight < e.height, o = i(e.width) && t.minWidth && t.minWidth > e.width, r = i(e.height) && t.minHeight && t.minHeight > e.height, h = this.originalPosition.left + this.originalSize.width, l = this.position.top + this.size.height, u = /sw|nw|w/.test(s), c = /nw|ne|n/.test(s); return o && (e.width = t.minWidth), r && (e.height = t.minHeight), n && (e.width = t.maxWidth), a && (e.height = t.maxHeight), o && u && (e.left = h - t.minWidth), n && u && (e.left = h - t.maxWidth), r && c && (e.top = l - t.minHeight), a && c && (e.top = l - t.maxHeight), e.width || e.height || e.left || !e.top ? e.width || e.height || e.top || !e.left || (e.left = null) : e.top = null, e }, _proportionallyResize: function () { if (this._proportionallyResizeElements.length) { var e, t, i, s, n, a = this.helper || this.element; for (e = 0; this._proportionallyResizeElements.length > e; e++) { if (n = this._proportionallyResizeElements[e], !this.borderDif) for (this.borderDif = [], i = [n.css("borderTopWidth"), n.css("borderRightWidth"), n.css("borderBottomWidth"), n.css("borderLeftWidth")], s = [n.css("paddingTop"), n.css("paddingRight"), n.css("paddingBottom"), n.css("paddingLeft")], t = 0; i.length > t; t++) this.borderDif[t] = (parseInt(i[t], 10) || 0) + (parseInt(s[t], 10) || 0); n.css({ height: a.height() - this.borderDif[0] - this.borderDif[2] || 0, width: a.width() - this.borderDif[1] - this.borderDif[3] || 0 }) } } }, _renderProxy: function () { var t = this.element, i = this.options; this.elementOffset = t.offset(), this._helper ? (this.helper = this.helper || e("<div style='overflow:hidden;'></div>"), this.helper.addClass(this._helper).css({ width: this.element.outerWidth() - 1, height: this.element.outerHeight() - 1, position: "absolute", left: this.elementOffset.left + "px", top: this.elementOffset.top + "px", zIndex: ++i.zIndex }), this.helper.appendTo("body").disableSelection()) : this.helper = this.element }, _change: { e: function (e, t) { return { width: this.originalSize.width + t } }, w: function (e, t) { var i = this.originalSize, s = this.originalPosition; return { left: s.left + t, width: i.width - t } }, n: function (e, t, i) { var s = this.originalSize, n = this.originalPosition; return { top: n.top + i, height: s.height - i } }, s: function (e, t, i) { return { height: this.originalSize.height + i } }, se: function (t, i, s) { return e.extend(this._change.s.apply(this, arguments), this._change.e.apply(this, [t, i, s])) }, sw: function (t, i, s) { return e.extend(this._change.s.apply(this, arguments), this._change.w.apply(this, [t, i, s])) }, ne: function (t, i, s) { return e.extend(this._change.n.apply(this, arguments), this._change.e.apply(this, [t, i, s])) }, nw: function (t, i, s) { return e.extend(this._change.n.apply(this, arguments), this._change.w.apply(this, [t, i, s])) } }, _propagate: function (t, i) { e.ui.plugin.call(this, t, [i, this.ui()]), "resize" !== t && this._trigger(t, i, this.ui()) }, plugins: {}, ui: function () { return { originalElement: this.originalElement, element: this.element, helper: this.helper, position: this.position, size: this.size, originalSize: this.originalSize, originalPosition: this.originalPosition } } }), e.ui.plugin.add("resizable", "animate", { stop: function (t) { var i = e(this).data("ui-resizable"), s = i.options, n = i._proportionallyResizeElements, a = n.length && /textarea/i.test(n[0].nodeName), o = a && e.ui.hasScroll(n[0], "left") ? 0 : i.sizeDiff.height, r = a ? 0 : i.sizeDiff.width, h = { width: i.size.width - r, height: i.size.height - o }, l = parseInt(i.element.css("left"), 10) + (i.position.left - i.originalPosition.left) || null, u = parseInt(i.element.css("top"), 10) + (i.position.top - i.originalPosition.top) || null; i.element.animate(e.extend(h, u && l ? { top: u, left: l } : {}), { duration: s.animateDuration, easing: s.animateEasing, step: function () { var s = { width: parseInt(i.element.css("width"), 10), height: parseInt(i.element.css("height"), 10), top: parseInt(i.element.css("top"), 10), left: parseInt(i.element.css("left"), 10) }; n && n.length && e(n[0]).css({ width: s.width, height: s.height }), i._updateCache(s), i._propagate("resize", t) } }) } }), e.ui.plugin.add("resizable", "containment", { start: function () { var i, s, n, a, o, r, h, l = e(this).data("ui-resizable"), u = l.options, c = l.element, d = u.containment, p = d instanceof e ? d.get(0) : /parent/.test(d) ? c.parent().get(0) : d; p && (l.containerElement = e(p), /document/.test(d) || d === document ? (l.containerOffset = { left: 0, top: 0 }, l.containerPosition = { left: 0, top: 0 }, l.parentData = { element: e(document), left: 0, top: 0, width: e(document).width(), height: e(document).height() || document.body.parentNode.scrollHeight }) : (i = e(p), s = [], e(["Top", "Right", "Left", "Bottom"]).each(function (e, n) { s[e] = t(i.css("padding" + n)) }), l.containerOffset = i.offset(), l.containerPosition = i.position(), l.containerSize = { height: i.innerHeight() - s[3], width: i.innerWidth() - s[1] }, n = l.containerOffset, a = l.containerSize.height, o = l.containerSize.width, r = e.ui.hasScroll(p, "left") ? p.scrollWidth : o, h = e.ui.hasScroll(p) ? p.scrollHeight : a, l.parentData = { element: p, left: n.left, top: n.top, width: r, height: h })) }, resize: function (t) { var i, s, n, a, o = e(this).data("ui-resizable"), r = o.options, h = o.containerOffset, l = o.position, u = o._aspectRatio || t.shiftKey, c = { top: 0, left: 0 }, d = o.containerElement; d[0] !== document && /static/.test(d.css("position")) && (c = h), l.left < (o._helper ? h.left : 0) && (o.size.width = o.size.width + (o._helper ? o.position.left - h.left : o.position.left - c.left), u && (o.size.height = o.size.width / o.aspectRatio), o.position.left = r.helper ? h.left : 0), l.top < (o._helper ? h.top : 0) && (o.size.height = o.size.height + (o._helper ? o.position.top - h.top : o.position.top), u && (o.size.width = o.size.height * o.aspectRatio), o.position.top = o._helper ? h.top : 0), o.offset.left = o.parentData.left + o.position.left, o.offset.top = o.parentData.top + o.position.top, i = Math.abs((o._helper ? o.offset.left - c.left : o.offset.left - c.left) + o.sizeDiff.width), s = Math.abs((o._helper ? o.offset.top - c.top : o.offset.top - h.top) + o.sizeDiff.height), n = o.containerElement.get(0) === o.element.parent().get(0), a = /relative|absolute/.test(o.containerElement.css("position")), n && a && (i -= o.parentData.left), i + o.size.width >= o.parentData.width && (o.size.width = o.parentData.width - i, u && (o.size.height = o.size.width / o.aspectRatio)), s + o.size.height >= o.parentData.height && (o.size.height = o.parentData.height - s, u && (o.size.width = o.size.height * o.aspectRatio)) }, stop: function () { var t = e(this).data("ui-resizable"), i = t.options, s = t.containerOffset, n = t.containerPosition, a = t.containerElement, o = e(t.helper), r = o.offset(), h = o.outerWidth() - t.sizeDiff.width, l = o.outerHeight() - t.sizeDiff.height; t._helper && !i.animate && /relative/.test(a.css("position")) && e(this).css({ left: r.left - n.left - s.left, width: h, height: l }), t._helper && !i.animate && /static/.test(a.css("position")) && e(this).css({ left: r.left - n.left - s.left, width: h, height: l }) } }), e.ui.plugin.add("resizable", "alsoResize", { start: function () { var t = e(this).data("ui-resizable"), i = t.options, s = function (t) { e(t).each(function () { var t = e(this); t.data("ui-resizable-alsoresize", { width: parseInt(t.width(), 10), height: parseInt(t.height(), 10), left: parseInt(t.css("left"), 10), top: parseInt(t.css("top"), 10) }) }) }; "object" != typeof i.alsoResize || i.alsoResize.parentNode ? s(i.alsoResize) : i.alsoResize.length ? (i.alsoResize = i.alsoResize[0], s(i.alsoResize)) : e.each(i.alsoResize, function (e) { s(e) }) }, resize: function (t, i) { var s = e(this).data("ui-resizable"), n = s.options, a = s.originalSize, o = s.originalPosition, r = { height: s.size.height - a.height || 0, width: s.size.width - a.width || 0, top: s.position.top - o.top || 0, left: s.position.left - o.left || 0 }, h = function (t, s) { e(t).each(function () { var t = e(this), n = e(this).data("ui-resizable-alsoresize"), a = {}, o = s && s.length ? s : t.parents(i.originalElement[0]).length ? ["width", "height"] : ["width", "height", "top", "left"]; e.each(o, function (e, t) { var i = (n[t] || 0) + (r[t] || 0); i && i >= 0 && (a[t] = i || null) }), t.css(a) }) }; "object" != typeof n.alsoResize || n.alsoResize.nodeType ? h(n.alsoResize) : e.each(n.alsoResize, function (e, t) { h(e, t) }) }, stop: function () { e(this).removeData("resizable-alsoresize") } }), e.ui.plugin.add("resizable", "ghost", { start: function () { var t = e(this).data("ui-resizable"), i = t.options, s = t.size; t.ghost = t.originalElement.clone(), t.ghost.css({ opacity: .25, display: "block", position: "relative", height: s.height, width: s.width, margin: 0, left: 0, top: 0 }).addClass("ui-resizable-ghost").addClass("string" == typeof i.ghost ? i.ghost : ""), t.ghost.appendTo(t.helper) }, resize: function () { var t = e(this).data("ui-resizable"); t.ghost && t.ghost.css({ position: "relative", height: t.size.height, width: t.size.width }) }, stop: function () { var t = e(this).data("ui-resizable"); t.ghost && t.helper && t.helper.get(0).removeChild(t.ghost.get(0)) } }), e.ui.plugin.add("resizable", "grid", { resize: function () { var t = e(this).data("ui-resizable"), i = t.options, s = t.size, n = t.originalSize, a = t.originalPosition, o = t.axis, r = "number" == typeof i.grid ? [i.grid, i.grid] : i.grid, h = r[0] || 1, l = r[1] || 1, u = Math.round((s.width - n.width) / h) * h, c = Math.round((s.height - n.height) / l) * l, d = n.width + u, p = n.height + c, f = i.maxWidth && d > i.maxWidth, m = i.maxHeight && p > i.maxHeight, g = i.minWidth && i.minWidth > d, v = i.minHeight && i.minHeight > p; i.grid = r, g && (d += h), v && (p += l), f && (d -= h), m && (p -= l), /^(se|s|e)$/.test(o) ? (t.size.width = d, t.size.height = p) : /^(ne)$/.test(o) ? (t.size.width = d, t.size.height = p, t.position.top = a.top - c) : /^(sw)$/.test(o) ? (t.size.width = d, t.size.height = p, t.position.left = a.left - u) : (t.size.width = d, t.size.height = p, t.position.top = a.top - c, t.position.left = a.left - u) } }) })(jQuery); (function (e) { e.widget("ui.selectable", e.ui.mouse, { version: "1.10.3", options: { appendTo: "body", autoRefresh: !0, distance: 0, filter: "*", tolerance: "touch", selected: null, selecting: null, start: null, stop: null, unselected: null, unselecting: null }, _create: function () { var t, i = this; this.element.addClass("ui-selectable"), this.dragged = !1, this.refresh = function () { t = e(i.options.filter, i.element[0]), t.addClass("ui-selectee"), t.each(function () { var t = e(this), i = t.offset(); e.data(this, "selectable-item", { element: this, $element: t, left: i.left, top: i.top, right: i.left + t.outerWidth(), bottom: i.top + t.outerHeight(), startselected: !1, selected: t.hasClass("ui-selected"), selecting: t.hasClass("ui-selecting"), unselecting: t.hasClass("ui-unselecting") }) }) }, this.refresh(), this.selectees = t.addClass("ui-selectee"), this._mouseInit(), this.helper = e("<div class='ui-selectable-helper'></div>") }, _destroy: function () { this.selectees.removeClass("ui-selectee").removeData("selectable-item"), this.element.removeClass("ui-selectable ui-selectable-disabled"), this._mouseDestroy() }, _mouseStart: function (t) { var i = this, s = this.options; this.opos = [t.pageX, t.pageY], this.options.disabled || (this.selectees = e(s.filter, this.element[0]), this._trigger("start", t), e(s.appendTo).append(this.helper), this.helper.css({ left: t.pageX, top: t.pageY, width: 0, height: 0 }), s.autoRefresh && this.refresh(), this.selectees.filter(".ui-selected").each(function () { var s = e.data(this, "selectable-item"); s.startselected = !0, t.metaKey || t.ctrlKey || (s.$element.removeClass("ui-selected"), s.selected = !1, s.$element.addClass("ui-unselecting"), s.unselecting = !0, i._trigger("unselecting", t, { unselecting: s.element })) }), e(t.target).parents().addBack().each(function () { var s, n = e.data(this, "selectable-item"); return n ? (s = !t.metaKey && !t.ctrlKey || !n.$element.hasClass("ui-selected"), n.$element.removeClass(s ? "ui-unselecting" : "ui-selected").addClass(s ? "ui-selecting" : "ui-unselecting"), n.unselecting = !s, n.selecting = s, n.selected = s, s ? i._trigger("selecting", t, { selecting: n.element }) : i._trigger("unselecting", t, { unselecting: n.element }), !1) : undefined })) }, _mouseDrag: function (t) { if (this.dragged = !0, !this.options.disabled) { var i, s = this, n = this.options, a = this.opos[0], o = this.opos[1], r = t.pageX, h = t.pageY; return a > r && (i = r, r = a, a = i), o > h && (i = h, h = o, o = i), this.helper.css({ left: a, top: o, width: r - a, height: h - o }), this.selectees.each(function () { var i = e.data(this, "selectable-item"), l = !1; i && i.element !== s.element[0] && ("touch" === n.tolerance ? l = !(i.left > r || a > i.right || i.top > h || o > i.bottom) : "fit" === n.tolerance && (l = i.left > a && r > i.right && i.top > o && h > i.bottom), l ? (i.selected && (i.$element.removeClass("ui-selected"), i.selected = !1), i.unselecting && (i.$element.removeClass("ui-unselecting"), i.unselecting = !1), i.selecting || (i.$element.addClass("ui-selecting"), i.selecting = !0, s._trigger("selecting", t, { selecting: i.element }))) : (i.selecting && ((t.metaKey || t.ctrlKey) && i.startselected ? (i.$element.removeClass("ui-selecting"), i.selecting = !1, i.$element.addClass("ui-selected"), i.selected = !0) : (i.$element.removeClass("ui-selecting"), i.selecting = !1, i.startselected && (i.$element.addClass("ui-unselecting"), i.unselecting = !0), s._trigger("unselecting", t, { unselecting: i.element }))), i.selected && (t.metaKey || t.ctrlKey || i.startselected || (i.$element.removeClass("ui-selected"), i.selected = !1, i.$element.addClass("ui-unselecting"), i.unselecting = !0, s._trigger("unselecting", t, { unselecting: i.element }))))) }), !1 } }, _mouseStop: function (t) { var i = this; return this.dragged = !1, e(".ui-unselecting", this.element[0]).each(function () { var s = e.data(this, "selectable-item"); s.$element.removeClass("ui-unselecting"), s.unselecting = !1, s.startselected = !1, i._trigger("unselected", t, { unselected: s.element }) }), e(".ui-selecting", this.element[0]).each(function () { var s = e.data(this, "selectable-item"); s.$element.removeClass("ui-selecting").addClass("ui-selected"), s.selecting = !1, s.selected = !0, s.startselected = !0, i._trigger("selected", t, { selected: s.element }) }), this._trigger("stop", t), this.helper.remove(), !1 } }) })(jQuery); (function (t) { function e(t, e, i) { return t > e && e + i > t } function i(t) { return /left|right/.test(t.css("float")) || /inline|table-cell/.test(t.css("display")) } t.widget("ui.sortable", t.ui.mouse, { version: "1.10.3", widgetEventPrefix: "sort", ready: !1, options: { appendTo: "parent", axis: !1, connectWith: !1, containment: !1, cursor: "auto", cursorAt: !1, dropOnEmpty: !0, forcePlaceholderSize: !1, forceHelperSize: !1, grid: !1, handle: !1, helper: "original", items: "> *", opacity: !1, placeholder: !1, revert: !1, scroll: !0, scrollSensitivity: 20, scrollSpeed: 20, scope: "default", tolerance: "intersect", zIndex: 1e3, activate: null, beforeStop: null, change: null, deactivate: null, out: null, over: null, receive: null, remove: null, sort: null, start: null, stop: null, update: null }, _create: function () { var t = this.options; this.containerCache = {}, this.element.addClass("ui-sortable"), this.refresh(), this.floating = this.items.length ? "x" === t.axis || i(this.items[0].item) : !1, this.offset = this.element.offset(), this._mouseInit(), this.ready = !0 }, _destroy: function () { this.element.removeClass("ui-sortable ui-sortable-disabled"), this._mouseDestroy(); for (var t = this.items.length - 1; t >= 0; t--) this.items[t].item.removeData(this.widgetName + "-item"); return this }, _setOption: function (e, i) { "disabled" === e ? (this.options[e] = i, this.widget().toggleClass("ui-sortable-disabled", !!i)) : t.Widget.prototype._setOption.apply(this, arguments) }, _mouseCapture: function (e, i) { var s = null, n = !1, a = this; return this.reverting ? !1 : this.options.disabled || "static" === this.options.type ? !1 : (this._refreshItems(e), t(e.target).parents().each(function () { return t.data(this, a.widgetName + "-item") === a ? (s = t(this), !1) : undefined }), t.data(e.target, a.widgetName + "-item") === a && (s = t(e.target)), s ? !this.options.handle || i || (t(this.options.handle, s).find("*").addBack().each(function () { this === e.target && (n = !0) }), n) ? (this.currentItem = s, this._removeCurrentsFromItems(), !0) : !1 : !1) }, _mouseStart: function (e, i, s) { var n, a, o = this.options; if (this.currentContainer = this, this.refreshPositions(), this.helper = this._createHelper(e), this._cacheHelperProportions(), this._cacheMargins(), this.scrollParent = this.helper.scrollParent(), this.offset = this.currentItem.offset(), this.offset = { top: this.offset.top - this.margins.top, left: this.offset.left - this.margins.left }, t.extend(this.offset, { click: { left: e.pageX - this.offset.left, top: e.pageY - this.offset.top }, parent: this._getParentOffset(), relative: this._getRelativeOffset() }), this.helper.css("position", "absolute"), this.cssPosition = this.helper.css("position"), this.originalPosition = this._generatePosition(e), this.originalPageX = e.pageX, this.originalPageY = e.pageY, o.cursorAt && this._adjustOffsetFromHelper(o.cursorAt), this.domPosition = { prev: this.currentItem.prev()[0], parent: this.currentItem.parent()[0] }, this.helper[0] !== this.currentItem[0] && this.currentItem.hide(), this._createPlaceholder(), o.containment && this._setContainment(), o.cursor && "auto" !== o.cursor && (a = this.document.find("body"), this.storedCursor = a.css("cursor"), a.css("cursor", o.cursor), this.storedStylesheet = t("<style>*{ cursor: " + o.cursor + " !important; }</style>").appendTo(a)), o.opacity && (this.helper.css("opacity") && (this._storedOpacity = this.helper.css("opacity")), this.helper.css("opacity", o.opacity)), o.zIndex && (this.helper.css("zIndex") && (this._storedZIndex = this.helper.css("zIndex")), this.helper.css("zIndex", o.zIndex)), this.scrollParent[0] !== document && "HTML" !== this.scrollParent[0].tagName && (this.overflowOffset = this.scrollParent.offset()), this._trigger("start", e, this._uiHash()), this._preserveHelperProportions || this._cacheHelperProportions(), !s) for (n = this.containers.length - 1; n >= 0; n--) this.containers[n]._trigger("activate", e, this._uiHash(this)); return t.ui.ddmanager && (t.ui.ddmanager.current = this), t.ui.ddmanager && !o.dropBehaviour && t.ui.ddmanager.prepareOffsets(this, e), this.dragging = !0, this.helper.addClass("ui-sortable-helper"), this._mouseDrag(e), !0 }, _mouseDrag: function (e) { var i, s, n, a, o = this.options, r = !1; for (this.position = this._generatePosition(e), this.positionAbs = this._convertPositionTo("absolute"), this.lastPositionAbs || (this.lastPositionAbs = this.positionAbs), this.options.scroll && (this.scrollParent[0] !== document && "HTML" !== this.scrollParent[0].tagName ? (this.overflowOffset.top + this.scrollParent[0].offsetHeight - e.pageY < o.scrollSensitivity ? this.scrollParent[0].scrollTop = r = this.scrollParent[0].scrollTop + o.scrollSpeed : e.pageY - this.overflowOffset.top < o.scrollSensitivity && (this.scrollParent[0].scrollTop = r = this.scrollParent[0].scrollTop - o.scrollSpeed), this.overflowOffset.left + this.scrollParent[0].offsetWidth - e.pageX < o.scrollSensitivity ? this.scrollParent[0].scrollLeft = r = this.scrollParent[0].scrollLeft + o.scrollSpeed : e.pageX - this.overflowOffset.left < o.scrollSensitivity && (this.scrollParent[0].scrollLeft = r = this.scrollParent[0].scrollLeft - o.scrollSpeed)) : (e.pageY - t(document).scrollTop() < o.scrollSensitivity ? r = t(document).scrollTop(t(document).scrollTop() - o.scrollSpeed) : t(window).height() - (e.pageY - t(document).scrollTop()) < o.scrollSensitivity && (r = t(document).scrollTop(t(document).scrollTop() + o.scrollSpeed)), e.pageX - t(document).scrollLeft() < o.scrollSensitivity ? r = t(document).scrollLeft(t(document).scrollLeft() - o.scrollSpeed) : t(window).width() - (e.pageX - t(document).scrollLeft()) < o.scrollSensitivity && (r = t(document).scrollLeft(t(document).scrollLeft() + o.scrollSpeed))), r !== !1 && t.ui.ddmanager && !o.dropBehaviour && t.ui.ddmanager.prepareOffsets(this, e)), this.positionAbs = this._convertPositionTo("absolute"), this.options.axis && "y" === this.options.axis || (this.helper[0].style.left = this.position.left + "px"), this.options.axis && "x" === this.options.axis || (this.helper[0].style.top = this.position.top + "px"), i = this.items.length - 1; i >= 0; i--) if (s = this.items[i], n = s.item[0], a = this._intersectsWithPointer(s), a && s.instance === this.currentContainer && n !== this.currentItem[0] && this.placeholder[1 === a ? "next" : "prev"]()[0] !== n && !t.contains(this.placeholder[0], n) && ("semi-dynamic" === this.options.type ? !t.contains(this.element[0], n) : !0)) { if (this.direction = 1 === a ? "down" : "up", "pointer" !== this.options.tolerance && !this._intersectsWithSides(s)) break; this._rearrange(e, s), this._trigger("change", e, this._uiHash()); break } return this._contactContainers(e), t.ui.ddmanager && t.ui.ddmanager.drag(this, e), this._trigger("sort", e, this._uiHash()), this.lastPositionAbs = this.positionAbs, !1 }, _mouseStop: function (e, i) { if (e) { if (t.ui.ddmanager && !this.options.dropBehaviour && t.ui.ddmanager.drop(this, e), this.options.revert) { var s = this, n = this.placeholder.offset(), a = this.options.axis, o = {}; a && "x" !== a || (o.left = n.left - this.offset.parent.left - this.margins.left + (this.offsetParent[0] === document.body ? 0 : this.offsetParent[0].scrollLeft)), a && "y" !== a || (o.top = n.top - this.offset.parent.top - this.margins.top + (this.offsetParent[0] === document.body ? 0 : this.offsetParent[0].scrollTop)), this.reverting = !0, t(this.helper).animate(o, parseInt(this.options.revert, 10) || 500, function () { s._clear(e) }) } else this._clear(e, i); return !1 } }, cancel: function () { if (this.dragging) { this._mouseUp({ target: null }), "original" === this.options.helper ? this.currentItem.css(this._storedCSS).removeClass("ui-sortable-helper") : this.currentItem.show(); for (var e = this.containers.length - 1; e >= 0; e--) this.containers[e]._trigger("deactivate", null, this._uiHash(this)), this.containers[e].containerCache.over && (this.containers[e]._trigger("out", null, this._uiHash(this)), this.containers[e].containerCache.over = 0) } return this.placeholder && (this.placeholder[0].parentNode && this.placeholder[0].parentNode.removeChild(this.placeholder[0]), "original" !== this.options.helper && this.helper && this.helper[0].parentNode && this.helper.remove(), t.extend(this, { helper: null, dragging: !1, reverting: !1, _noFinalSort: null }), this.domPosition.prev ? t(this.domPosition.prev).after(this.currentItem) : t(this.domPosition.parent).prepend(this.currentItem)), this }, serialize: function (e) { var i = this._getItemsAsjQuery(e && e.connected), s = []; return e = e || {}, t(i).each(function () { var i = (t(e.item || this).attr(e.attribute || "id") || "").match(e.expression || /(.+)[\-=_](.+)/); i && s.push((e.key || i[1] + "[]") + "=" + (e.key && e.expression ? i[1] : i[2])) }), !s.length && e.key && s.push(e.key + "="), s.join("&") }, toArray: function (e) { var i = this._getItemsAsjQuery(e && e.connected), s = []; return e = e || {}, i.each(function () { s.push(t(e.item || this).attr(e.attribute || "id") || "") }), s }, _intersectsWith: function (t) { var e = this.positionAbs.left, i = e + this.helperProportions.width, s = this.positionAbs.top, n = s + this.helperProportions.height, a = t.left, o = a + t.width, r = t.top, h = r + t.height, l = this.offset.click.top, c = this.offset.click.left, u = "x" === this.options.axis || s + l > r && h > s + l, d = "y" === this.options.axis || e + c > a && o > e + c, p = u && d; return "pointer" === this.options.tolerance || this.options.forcePointerForContainers || "pointer" !== this.options.tolerance && this.helperProportions[this.floating ? "width" : "height"] > t[this.floating ? "width" : "height"] ? p : e + this.helperProportions.width / 2 > a && o > i - this.helperProportions.width / 2 && s + this.helperProportions.height / 2 > r && h > n - this.helperProportions.height / 2 }, _intersectsWithPointer: function (t) { var i = "x" === this.options.axis || e(this.positionAbs.top + this.offset.click.top, t.top, t.height), s = "y" === this.options.axis || e(this.positionAbs.left + this.offset.click.left, t.left, t.width), n = i && s, a = this._getDragVerticalDirection(), o = this._getDragHorizontalDirection(); return n ? this.floating ? o && "right" === o || "down" === a ? 2 : 1 : a && ("down" === a ? 2 : 1) : !1 }, _intersectsWithSides: function (t) { var i = e(this.positionAbs.top + this.offset.click.top, t.top + t.height / 2, t.height), s = e(this.positionAbs.left + this.offset.click.left, t.left + t.width / 2, t.width), n = this._getDragVerticalDirection(), a = this._getDragHorizontalDirection(); return this.floating && a ? "right" === a && s || "left" === a && !s : n && ("down" === n && i || "up" === n && !i) }, _getDragVerticalDirection: function () { var t = this.positionAbs.top - this.lastPositionAbs.top; return 0 !== t && (t > 0 ? "down" : "up") }, _getDragHorizontalDirection: function () { var t = this.positionAbs.left - this.lastPositionAbs.left; return 0 !== t && (t > 0 ? "right" : "left") }, refresh: function (t) { return this._refreshItems(t), this.refreshPositions(), this }, _connectWith: function () { var t = this.options; return t.connectWith.constructor === String ? [t.connectWith] : t.connectWith }, _getItemsAsjQuery: function (e) { var i, s, n, a, o = [], r = [], h = this._connectWith(); if (h && e) for (i = h.length - 1; i >= 0; i--) for (n = t(h[i]), s = n.length - 1; s >= 0; s--) a = t.data(n[s], this.widgetFullName), a && a !== this && !a.options.disabled && r.push([t.isFunction(a.options.items) ? a.options.items.call(a.element) : t(a.options.items, a.element).not(".ui-sortable-helper").not(".ui-sortable-placeholder"), a]); for (r.push([t.isFunction(this.options.items) ? this.options.items.call(this.element, null, { options: this.options, item: this.currentItem }) : t(this.options.items, this.element).not(".ui-sortable-helper").not(".ui-sortable-placeholder"), this]), i = r.length - 1; i >= 0; i--) r[i][0].each(function () { o.push(this) }); return t(o) }, _removeCurrentsFromItems: function () { var e = this.currentItem.find(":data(" + this.widgetName + "-item)"); this.items = t.grep(this.items, function (t) { for (var i = 0; e.length > i; i++) if (e[i] === t.item[0]) return !1; return !0 }) }, _refreshItems: function (e) { this.items = [], this.containers = [this]; var i, s, n, a, o, r, h, l, c = this.items, u = [[t.isFunction(this.options.items) ? this.options.items.call(this.element[0], e, { item: this.currentItem }) : t(this.options.items, this.element), this]], d = this._connectWith(); if (d && this.ready) for (i = d.length - 1; i >= 0; i--) for (n = t(d[i]), s = n.length - 1; s >= 0; s--) a = t.data(n[s], this.widgetFullName), a && a !== this && !a.options.disabled && (u.push([t.isFunction(a.options.items) ? a.options.items.call(a.element[0], e, { item: this.currentItem }) : t(a.options.items, a.element), a]), this.containers.push(a)); for (i = u.length - 1; i >= 0; i--) for (o = u[i][1], r = u[i][0], s = 0, l = r.length; l > s; s++) h = t(r[s]), h.data(this.widgetName + "-item", o), c.push({ item: h, instance: o, width: 0, height: 0, left: 0, top: 0 }) }, refreshPositions: function (e) { this.offsetParent && this.helper && (this.offset.parent = this._getParentOffset()); var i, s, n, a; for (i = this.items.length - 1; i >= 0; i--) s = this.items[i], s.instance !== this.currentContainer && this.currentContainer && s.item[0] !== this.currentItem[0] || (n = this.options.toleranceElement ? t(this.options.toleranceElement, s.item) : s.item, e || (s.width = n.outerWidth(), s.height = n.outerHeight()), a = n.offset(), s.left = a.left, s.top = a.top); if (this.options.custom && this.options.custom.refreshContainers) this.options.custom.refreshContainers.call(this); else for (i = this.containers.length - 1; i >= 0; i--) a = this.containers[i].element.offset(), this.containers[i].containerCache.left = a.left, this.containers[i].containerCache.top = a.top, this.containers[i].containerCache.width = this.containers[i].element.outerWidth(), this.containers[i].containerCache.height = this.containers[i].element.outerHeight(); return this }, _createPlaceholder: function (e) { e = e || this; var i, s = e.options; s.placeholder && s.placeholder.constructor !== String || (i = s.placeholder, s.placeholder = { element: function () { var s = e.currentItem[0].nodeName.toLowerCase(), n = t("<" + s + ">", e.document[0]).addClass(i || e.currentItem[0].className + " ui-sortable-placeholder").removeClass("ui-sortable-helper"); return "tr" === s ? e.currentItem.children().each(function () { t("<td>&#160;</td>", e.document[0]).attr("colspan", t(this).attr("colspan") || 1).appendTo(n) }) : "img" === s && n.attr("src", e.currentItem.attr("src")), i || n.css("visibility", "hidden"), n }, update: function (t, n) { (!i || s.forcePlaceholderSize) && (n.height() || n.height(e.currentItem.innerHeight() - parseInt(e.currentItem.css("paddingTop") || 0, 10) - parseInt(e.currentItem.css("paddingBottom") || 0, 10)), n.width() || n.width(e.currentItem.innerWidth() - parseInt(e.currentItem.css("paddingLeft") || 0, 10) - parseInt(e.currentItem.css("paddingRight") || 0, 10))) } }), e.placeholder = t(s.placeholder.element.call(e.element, e.currentItem)), e.currentItem.after(e.placeholder), s.placeholder.update(e, e.placeholder) }, _contactContainers: function (s) { var n, a, o, r, h, l, c, u, d, p, f = null, m = null; for (n = this.containers.length - 1; n >= 0; n--) if (!t.contains(this.currentItem[0], this.containers[n].element[0])) if (this._intersectsWith(this.containers[n].containerCache)) { if (f && t.contains(this.containers[n].element[0], f.element[0])) continue; f = this.containers[n], m = n } else this.containers[n].containerCache.over && (this.containers[n]._trigger("out", s, this._uiHash(this)), this.containers[n].containerCache.over = 0); if (f) if (1 === this.containers.length) this.containers[m].containerCache.over || (this.containers[m]._trigger("over", s, this._uiHash(this)), this.containers[m].containerCache.over = 1); else { for (o = 1e4, r = null, p = f.floating || i(this.currentItem), h = p ? "left" : "top", l = p ? "width" : "height", c = this.positionAbs[h] + this.offset.click[h], a = this.items.length - 1; a >= 0; a--) t.contains(this.containers[m].element[0], this.items[a].item[0]) && this.items[a].item[0] !== this.currentItem[0] && (!p || e(this.positionAbs.top + this.offset.click.top, this.items[a].top, this.items[a].height)) && (u = this.items[a].item.offset()[h], d = !1, Math.abs(u - c) > Math.abs(u + this.items[a][l] - c) && (d = !0, u += this.items[a][l]), o > Math.abs(u - c) && (o = Math.abs(u - c), r = this.items[a], this.direction = d ? "up" : "down")); if (!r && !this.options.dropOnEmpty) return; if (this.currentContainer === this.containers[m]) return; r ? this._rearrange(s, r, null, !0) : this._rearrange(s, null, this.containers[m].element, !0), this._trigger("change", s, this._uiHash()), this.containers[m]._trigger("change", s, this._uiHash(this)), this.currentContainer = this.containers[m], this.options.placeholder.update(this.currentContainer, this.placeholder), this.containers[m]._trigger("over", s, this._uiHash(this)), this.containers[m].containerCache.over = 1 } }, _createHelper: function (e) { var i = this.options, s = t.isFunction(i.helper) ? t(i.helper.apply(this.element[0], [e, this.currentItem])) : "clone" === i.helper ? this.currentItem.clone() : this.currentItem; return s.parents("body").length || t("parent" !== i.appendTo ? i.appendTo : this.currentItem[0].parentNode)[0].appendChild(s[0]), s[0] === this.currentItem[0] && (this._storedCSS = { width: this.currentItem[0].style.width, height: this.currentItem[0].style.height, position: this.currentItem.css("position"), top: this.currentItem.css("top"), left: this.currentItem.css("left") }), (!s[0].style.width || i.forceHelperSize) && s.width(this.currentItem.width()), (!s[0].style.height || i.forceHelperSize) && s.height(this.currentItem.height()), s }, _adjustOffsetFromHelper: function (e) { "string" == typeof e && (e = e.split(" ")), t.isArray(e) && (e = { left: +e[0], top: +e[1] || 0 }), "left" in e && (this.offset.click.left = e.left + this.margins.left), "right" in e && (this.offset.click.left = this.helperProportions.width - e.right + this.margins.left), "top" in e && (this.offset.click.top = e.top + this.margins.top), "bottom" in e && (this.offset.click.top = this.helperProportions.height - e.bottom + this.margins.top) }, _getParentOffset: function () { this.offsetParent = this.helper.offsetParent(); var e = this.offsetParent.offset(); return "absolute" === this.cssPosition && this.scrollParent[0] !== document && t.contains(this.scrollParent[0], this.offsetParent[0]) && (e.left += this.scrollParent.scrollLeft(), e.top += this.scrollParent.scrollTop()), (this.offsetParent[0] === document.body || this.offsetParent[0].tagName && "html" === this.offsetParent[0].tagName.toLowerCase() && t.ui.ie) && (e = { top: 0, left: 0 }), { top: e.top + (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0), left: e.left + (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0) } }, _getRelativeOffset: function () { if ("relative" === this.cssPosition) { var t = this.currentItem.position(); return { top: t.top - (parseInt(this.helper.css("top"), 10) || 0) + this.scrollParent.scrollTop(), left: t.left - (parseInt(this.helper.css("left"), 10) || 0) + this.scrollParent.scrollLeft() } } return { top: 0, left: 0 } }, _cacheMargins: function () { this.margins = { left: parseInt(this.currentItem.css("marginLeft"), 10) || 0, top: parseInt(this.currentItem.css("marginTop"), 10) || 0 } }, _cacheHelperProportions: function () { this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() } }, _setContainment: function () { var e, i, s, n = this.options; "parent" === n.containment && (n.containment = this.helper[0].parentNode), ("document" === n.containment || "window" === n.containment) && (this.containment = [0 - this.offset.relative.left - this.offset.parent.left, 0 - this.offset.relative.top - this.offset.parent.top, t("document" === n.containment ? document : window).width() - this.helperProportions.width - this.margins.left, (t("document" === n.containment ? document : window).height() || document.body.parentNode.scrollHeight) - this.helperProportions.height - this.margins.top]), /^(document|window|parent)$/.test(n.containment) || (e = t(n.containment)[0], i = t(n.containment).offset(), s = "hidden" !== t(e).css("overflow"), this.containment = [i.left + (parseInt(t(e).css("borderLeftWidth"), 10) || 0) + (parseInt(t(e).css("paddingLeft"), 10) || 0) - this.margins.left, i.top + (parseInt(t(e).css("borderTopWidth"), 10) || 0) + (parseInt(t(e).css("paddingTop"), 10) || 0) - this.margins.top, i.left + (s ? Math.max(e.scrollWidth, e.offsetWidth) : e.offsetWidth) - (parseInt(t(e).css("borderLeftWidth"), 10) || 0) - (parseInt(t(e).css("paddingRight"), 10) || 0) - this.helperProportions.width - this.margins.left, i.top + (s ? Math.max(e.scrollHeight, e.offsetHeight) : e.offsetHeight) - (parseInt(t(e).css("borderTopWidth"), 10) || 0) - (parseInt(t(e).css("paddingBottom"), 10) || 0) - this.helperProportions.height - this.margins.top]) }, _convertPositionTo: function (e, i) { i || (i = this.position); var s = "absolute" === e ? 1 : -1, n = "absolute" !== this.cssPosition || this.scrollParent[0] !== document && t.contains(this.scrollParent[0], this.offsetParent[0]) ? this.scrollParent : this.offsetParent, a = /(html|body)/i.test(n[0].tagName); return { top: i.top + this.offset.relative.top * s + this.offset.parent.top * s - ("fixed" === this.cssPosition ? -this.scrollParent.scrollTop() : a ? 0 : n.scrollTop()) * s, left: i.left + this.offset.relative.left * s + this.offset.parent.left * s - ("fixed" === this.cssPosition ? -this.scrollParent.scrollLeft() : a ? 0 : n.scrollLeft()) * s } }, _generatePosition: function (e) { var i, s, n = this.options, a = e.pageX, o = e.pageY, r = "absolute" !== this.cssPosition || this.scrollParent[0] !== document && t.contains(this.scrollParent[0], this.offsetParent[0]) ? this.scrollParent : this.offsetParent, h = /(html|body)/i.test(r[0].tagName); return "relative" !== this.cssPosition || this.scrollParent[0] !== document && this.scrollParent[0] !== this.offsetParent[0] || (this.offset.relative = this._getRelativeOffset()), this.originalPosition && (this.containment && (e.pageX - this.offset.click.left < this.containment[0] && (a = this.containment[0] + this.offset.click.left), e.pageY - this.offset.click.top < this.containment[1] && (o = this.containment[1] + this.offset.click.top), e.pageX - this.offset.click.left > this.containment[2] && (a = this.containment[2] + this.offset.click.left), e.pageY - this.offset.click.top > this.containment[3] && (o = this.containment[3] + this.offset.click.top)), n.grid && (i = this.originalPageY + Math.round((o - this.originalPageY) / n.grid[1]) * n.grid[1], o = this.containment ? i - this.offset.click.top >= this.containment[1] && i - this.offset.click.top <= this.containment[3] ? i : i - this.offset.click.top >= this.containment[1] ? i - n.grid[1] : i + n.grid[1] : i, s = this.originalPageX + Math.round((a - this.originalPageX) / n.grid[0]) * n.grid[0], a = this.containment ? s - this.offset.click.left >= this.containment[0] && s - this.offset.click.left <= this.containment[2] ? s : s - this.offset.click.left >= this.containment[0] ? s - n.grid[0] : s + n.grid[0] : s)), { top: o - this.offset.click.top - this.offset.relative.top - this.offset.parent.top + ("fixed" === this.cssPosition ? -this.scrollParent.scrollTop() : h ? 0 : r.scrollTop()), left: a - this.offset.click.left - this.offset.relative.left - this.offset.parent.left + ("fixed" === this.cssPosition ? -this.scrollParent.scrollLeft() : h ? 0 : r.scrollLeft()) } }, _rearrange: function (t, e, i, s) { i ? i[0].appendChild(this.placeholder[0]) : e.item[0].parentNode.insertBefore(this.placeholder[0], "down" === this.direction ? e.item[0] : e.item[0].nextSibling), this.counter = this.counter ? ++this.counter : 1; var n = this.counter; this._delay(function () { n === this.counter && this.refreshPositions(!s) }) }, _clear: function (t, e) { this.reverting = !1; var i, s = []; if (!this._noFinalSort && this.currentItem.parent().length && this.placeholder.before(this.currentItem), this._noFinalSort = null, this.helper[0] === this.currentItem[0]) { for (i in this._storedCSS) ("auto" === this._storedCSS[i] || "static" === this._storedCSS[i]) && (this._storedCSS[i] = ""); this.currentItem.css(this._storedCSS).removeClass("ui-sortable-helper") } else this.currentItem.show(); for (this.fromOutside && !e && s.push(function (t) { this._trigger("receive", t, this._uiHash(this.fromOutside)) }), !this.fromOutside && this.domPosition.prev === this.currentItem.prev().not(".ui-sortable-helper")[0] && this.domPosition.parent === this.currentItem.parent()[0] || e || s.push(function (t) { this._trigger("update", t, this._uiHash()) }), this !== this.currentContainer && (e || (s.push(function (t) { this._trigger("remove", t, this._uiHash()) }), s.push(function (t) { return function (e) { t._trigger("receive", e, this._uiHash(this)) } }.call(this, this.currentContainer)), s.push(function (t) { return function (e) { t._trigger("update", e, this._uiHash(this)) } }.call(this, this.currentContainer)))), i = this.containers.length - 1; i >= 0; i--) e || s.push(function (t) { return function (e) { t._trigger("deactivate", e, this._uiHash(this)) } }.call(this, this.containers[i])), this.containers[i].containerCache.over && (s.push(function (t) { return function (e) { t._trigger("out", e, this._uiHash(this)) } }.call(this, this.containers[i])), this.containers[i].containerCache.over = 0); if (this.storedCursor && (this.document.find("body").css("cursor", this.storedCursor), this.storedStylesheet.remove()), this._storedOpacity && this.helper.css("opacity", this._storedOpacity), this._storedZIndex && this.helper.css("zIndex", "auto" === this._storedZIndex ? "" : this._storedZIndex), this.dragging = !1, this.cancelHelperRemoval) { if (!e) { for (this._trigger("beforeStop", t, this._uiHash()), i = 0; s.length > i; i++) s[i].call(this, t); this._trigger("stop", t, this._uiHash()) } return this.fromOutside = !1, !1 } if (e || this._trigger("beforeStop", t, this._uiHash()), this.placeholder[0].parentNode.removeChild(this.placeholder[0]), this.helper[0] !== this.currentItem[0] && this.helper.remove(), this.helper = null, !e) { for (i = 0; s.length > i; i++) s[i].call(this, t); this._trigger("stop", t, this._uiHash()) } return this.fromOutside = !1, !0 }, _trigger: function () { t.Widget.prototype._trigger.apply(this, arguments) === !1 && this.cancel() }, _uiHash: function (e) { var i = e || this; return { helper: i.helper, placeholder: i.placeholder || t([]), position: i.position, originalPosition: i.originalPosition, offset: i.positionAbs, item: i.currentItem, sender: e ? e.element : null } } }) })(jQuery); (function (t, e) {
	function i() { this._curInst = null, this._keyEvent = !1, this._disabledInputs = [], this._datepickerShowing = !1, this._inDialog = !1, this._mainDivId = "ui-datepicker-div", this._inlineClass = "ui-datepicker-inline", this._appendClass = "ui-datepicker-append", this._triggerClass = "ui-datepicker-trigger", this._dialogClass = "ui-datepicker-dialog", this._disableClass = "ui-datepicker-disabled", this._unselectableClass = "ui-datepicker-unselectable", this._currentClass = "ui-datepicker-current-day", this._dayOverClass = "ui-datepicker-days-cell-over", this.regional = [], this.regional[""] = { closeText: "Done", prevText: "Prev", nextText: "Next", currentText: "Today", monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"], weekHeader: "Wk", dateFormat: "mm/dd/yy", firstDay: 0, isRTL: !1, showMonthAfterYear: !1, yearSuffix: "" }, this._defaults = { showOn: "focus", showAnim: "fadeIn", showOptions: {}, defaultDate: null, appendText: "", buttonText: "...", buttonImage: "", buttonImageOnly: !1, hideIfNoPrevNext: !1, navigationAsDateFormat: !1, gotoCurrent: !1, changeMonth: !1, changeYear: !1, yearRange: "c-10:c+10", showOtherMonths: !1, selectOtherMonths: !1, showWeek: !1, calculateWeek: this.iso8601Week, shortYearCutoff: "+10", minDate: null, maxDate: null, duration: "fast", beforeShowDay: null, beforeShow: null, onSelect: null, onChangeMonthYear: null, onClose: null, numberOfMonths: 1, showCurrentAtPos: 0, stepMonths: 1, stepBigMonths: 12, altField: "", altFormat: "", constrainInput: !0, showButtonPanel: !1, autoSize: !1, disabled: !1 }, t.extend(this._defaults, this.regional[""]), this.dpDiv = s(t("<div id='" + this._mainDivId + "' class='ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all'></div>")) } function s(e) { var i = "button, .ui-datepicker-prev, .ui-datepicker-next, .ui-datepicker-calendar td a"; return e.delegate(i, "mouseout", function () { t(this).removeClass("ui-state-hover"), -1 !== this.className.indexOf("ui-datepicker-prev") && t(this).removeClass("ui-datepicker-prev-hover"), -1 !== this.className.indexOf("ui-datepicker-next") && t(this).removeClass("ui-datepicker-next-hover") }).delegate(i, "mouseover", function () { t.datepicker._isDisabledDatepicker(a.inline ? e.parent()[0] : a.input[0]) || (t(this).parents(".ui-datepicker-calendar").find("a").removeClass("ui-state-hover"), t(this).addClass("ui-state-hover"), -1 !== this.className.indexOf("ui-datepicker-prev") && t(this).addClass("ui-datepicker-prev-hover"), -1 !== this.className.indexOf("ui-datepicker-next") && t(this).addClass("ui-datepicker-next-hover")) }) } function n(e, i) { t.extend(e, i); for (var s in i) null == i[s] && (e[s] = i[s]); return e } t.extend(t.ui, { datepicker: { version: "1.10.3" } }); var a, r = "datepicker"; t.extend(i.prototype, {
		markerClassName: "hasDatepicker", maxRows: 4, _widgetDatepicker: function () { return this.dpDiv }, setDefaults: function (t) { return n(this._defaults, t || {}), this }, _attachDatepicker: function (e, i) { var s, n, a; s = e.nodeName.toLowerCase(), n = "div" === s || "span" === s, e.id || (this.uuid += 1, e.id = "dp" + this.uuid), a = this._newInst(t(e), n), a.settings = t.extend({}, i || {}), "input" === s ? this._connectDatepicker(e, a) : n && this._inlineDatepicker(e, a) }, _newInst: function (e, i) { var n = e[0].id.replace(/([^A-Za-z0-9_\-])/g, "\\\\$1"); return { id: n, input: e, selectedDay: 0, selectedMonth: 0, selectedYear: 0, drawMonth: 0, drawYear: 0, inline: i, dpDiv: i ? s(t("<div class='" + this._inlineClass + " ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all'></div>")) : this.dpDiv } }, _connectDatepicker: function (e, i) { var s = t(e); i.append = t([]), i.trigger = t([]), s.hasClass(this.markerClassName) || (this._attachments(s, i), s.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress).keyup(this._doKeyUp), this._autoSize(i), t.data(e, r, i), i.settings.disabled && this._disableDatepicker(e)) }, _attachments: function (e, i) { var s, n, a, r = this._get(i, "appendText"), o = this._get(i, "isRTL"); i.append && i.append.remove(), r && (i.append = t("<span class='" + this._appendClass + "'>" + r + "</span>"), e[o ? "before" : "after"](i.append)), e.unbind("focus", this._showDatepicker), i.trigger && i.trigger.remove(), s = this._get(i, "showOn"), ("focus" === s || "both" === s) && e.focus(this._showDatepicker), ("button" === s || "both" === s) && (n = this._get(i, "buttonText"), a = this._get(i, "buttonImage"), i.trigger = t(this._get(i, "buttonImageOnly") ? t("<img/>").addClass(this._triggerClass).attr({ src: a, alt: n, title: n }) : t("<button type='button'></button>").addClass(this._triggerClass).html(a ? t("<img/>").attr({ src: a, alt: n, title: n }) : n)), e[o ? "before" : "after"](i.trigger), i.trigger.click(function () { return t.datepicker._datepickerShowing && t.datepicker._lastInput === e[0] ? t.datepicker._hideDatepicker() : t.datepicker._datepickerShowing && t.datepicker._lastInput !== e[0] ? (t.datepicker._hideDatepicker(), t.datepicker._showDatepicker(e[0])) : t.datepicker._showDatepicker(e[0]), !1 })) }, _autoSize: function (t) { if (this._get(t, "autoSize") && !t.inline) { var e, i, s, n, a = new Date(2009, 11, 20), r = this._get(t, "dateFormat"); r.match(/[DM]/) && (e = function (t) { for (i = 0, s = 0, n = 0; t.length > n; n++) t[n].length > i && (i = t[n].length, s = n); return s }, a.setMonth(e(this._get(t, r.match(/MM/) ? "monthNames" : "monthNamesShort"))), a.setDate(e(this._get(t, r.match(/DD/) ? "dayNames" : "dayNamesShort")) + 20 - a.getDay())), t.input.attr("size", this._formatDate(t, a).length) } }, _inlineDatepicker: function (e, i) { var s = t(e); s.hasClass(this.markerClassName) || (s.addClass(this.markerClassName).append(i.dpDiv), t.data(e, r, i), this._setDate(i, this._getDefaultDate(i), !0), this._updateDatepicker(i), this._updateAlternate(i), i.settings.disabled && this._disableDatepicker(e), i.dpDiv.css("display", "block")) }, _dialogDatepicker: function (e, i, s, a, o) { var h, l, c, u, d, p = this._dialogInst; return p || (this.uuid += 1, h = "dp" + this.uuid, this._dialogInput = t("<input type='text' id='" + h + "' style='position: absolute; top: -100px; width: 0px;'/>"), this._dialogInput.keydown(this._doKeyDown), t("body").append(this._dialogInput), p = this._dialogInst = this._newInst(this._dialogInput, !1), p.settings = {}, t.data(this._dialogInput[0], r, p)), n(p.settings, a || {}), i = i && i.constructor === Date ? this._formatDate(p, i) : i, this._dialogInput.val(i), this._pos = o ? o.length ? o : [o.pageX, o.pageY] : null, this._pos || (l = document.documentElement.clientWidth, c = document.documentElement.clientHeight, u = document.documentElement.scrollLeft || document.body.scrollLeft, d = document.documentElement.scrollTop || document.body.scrollTop, this._pos = [l / 2 - 100 + u, c / 2 - 150 + d]), this._dialogInput.css("left", this._pos[0] + 20 + "px").css("top", this._pos[1] + "px"), p.settings.onSelect = s, this._inDialog = !0, this.dpDiv.addClass(this._dialogClass), this._showDatepicker(this._dialogInput[0]), t.blockUI && t.blockUI(this.dpDiv), t.data(this._dialogInput[0], r, p), this }, _destroyDatepicker: function (e) { var i, s = t(e), n = t.data(e, r); s.hasClass(this.markerClassName) && (i = e.nodeName.toLowerCase(), t.removeData(e, r), "input" === i ? (n.append.remove(), n.trigger.remove(), s.removeClass(this.markerClassName).unbind("focus", this._showDatepicker).unbind("keydown", this._doKeyDown).unbind("keypress", this._doKeyPress).unbind("keyup", this._doKeyUp)) : ("div" === i || "span" === i) && s.removeClass(this.markerClassName).empty()) }, _enableDatepicker: function (e) { var i, s, n = t(e), a = t.data(e, r); n.hasClass(this.markerClassName) && (i = e.nodeName.toLowerCase(), "input" === i ? (e.disabled = !1, a.trigger.filter("button").each(function () { this.disabled = !1 }).end().filter("img").css({ opacity: "1.0", cursor: "" })) : ("div" === i || "span" === i) && (s = n.children("." + this._inlineClass), s.children().removeClass("ui-state-disabled"), s.find("select.ui-datepicker-month, select.ui-datepicker-year").prop("disabled", !1)), this._disabledInputs = t.map(this._disabledInputs, function (t) { return t === e ? null : t })) }, _disableDatepicker: function (e) { var i, s, n = t(e), a = t.data(e, r); n.hasClass(this.markerClassName) && (i = e.nodeName.toLowerCase(), "input" === i ? (e.disabled = !0, a.trigger.filter("button").each(function () { this.disabled = !0 }).end().filter("img").css({ opacity: "0.5", cursor: "default" })) : ("div" === i || "span" === i) && (s = n.children("." + this._inlineClass), s.children().addClass("ui-state-disabled"), s.find("select.ui-datepicker-month, select.ui-datepicker-year").prop("disabled", !0)), this._disabledInputs = t.map(this._disabledInputs, function (t) { return t === e ? null : t }), this._disabledInputs[this._disabledInputs.length] = e) }, _isDisabledDatepicker: function (t) { if (!t) return !1; for (var e = 0; this._disabledInputs.length > e; e++) if (this._disabledInputs[e] === t) return !0; return !1 }, _getInst: function (e) { try { return t.data(e, r) } catch (i) { throw "Missing instance data for this datepicker" } }, _optionDatepicker: function (i, s, a) { var r, o, h, l, c = this._getInst(i); return 2 === arguments.length && "string" == typeof s ? "defaults" === s ? t.extend({}, t.datepicker._defaults) : c ? "all" === s ? t.extend({}, c.settings) : this._get(c, s) : null : (r = s || {}, "string" == typeof s && (r = {}, r[s] = a), c && (this._curInst === c && this._hideDatepicker(), o = this._getDateDatepicker(i, !0), h = this._getMinMaxDate(c, "min"), l = this._getMinMaxDate(c, "max"), n(c.settings, r), null !== h && r.dateFormat !== e && r.minDate === e && (c.settings.minDate = this._formatDate(c, h)), null !== l && r.dateFormat !== e && r.maxDate === e && (c.settings.maxDate = this._formatDate(c, l)), "disabled" in r && (r.disabled ? this._disableDatepicker(i) : this._enableDatepicker(i)), this._attachments(t(i), c), this._autoSize(c), this._setDate(c, o), this._updateAlternate(c), this._updateDatepicker(c)), e) }, _changeDatepicker: function (t, e, i) { this._optionDatepicker(t, e, i) }, _refreshDatepicker: function (t) { var e = this._getInst(t); e && this._updateDatepicker(e) }, _setDateDatepicker: function (t, e) { var i = this._getInst(t); i && (this._setDate(i, e), this._updateDatepicker(i), this._updateAlternate(i)) }, _getDateDatepicker: function (t, e) { var i = this._getInst(t); return i && !i.inline && this._setDateFromField(i, e), i ? this._getDate(i) : null }, _doKeyDown: function (e) { var i, s, n, a = t.datepicker._getInst(e.target), r = !0, o = a.dpDiv.is(".ui-datepicker-rtl"); if (a._keyEvent = !0, t.datepicker._datepickerShowing) switch (e.keyCode) { case 9: t.datepicker._hideDatepicker(), r = !1; break; case 13: return n = t("td." + t.datepicker._dayOverClass + ":not(." + t.datepicker._currentClass + ")", a.dpDiv), n[0] && t.datepicker._selectDay(e.target, a.selectedMonth, a.selectedYear, n[0]), i = t.datepicker._get(a, "onSelect"), i ? (s = t.datepicker._formatDate(a), i.apply(a.input ? a.input[0] : null, [s, a])) : t.datepicker._hideDatepicker(), !1; case 27: t.datepicker._hideDatepicker(); break; case 33: t.datepicker._adjustDate(e.target, e.ctrlKey ? -t.datepicker._get(a, "stepBigMonths") : -t.datepicker._get(a, "stepMonths"), "M"); break; case 34: t.datepicker._adjustDate(e.target, e.ctrlKey ? +t.datepicker._get(a, "stepBigMonths") : +t.datepicker._get(a, "stepMonths"), "M"); break; case 35: (e.ctrlKey || e.metaKey) && t.datepicker._clearDate(e.target), r = e.ctrlKey || e.metaKey; break; case 36: (e.ctrlKey || e.metaKey) && t.datepicker._gotoToday(e.target), r = e.ctrlKey || e.metaKey; break; case 37: (e.ctrlKey || e.metaKey) && t.datepicker._adjustDate(e.target, o ? 1 : -1, "D"), r = e.ctrlKey || e.metaKey, e.originalEvent.altKey && t.datepicker._adjustDate(e.target, e.ctrlKey ? -t.datepicker._get(a, "stepBigMonths") : -t.datepicker._get(a, "stepMonths"), "M"); break; case 38: (e.ctrlKey || e.metaKey) && t.datepicker._adjustDate(e.target, -7, "D"), r = e.ctrlKey || e.metaKey; break; case 39: (e.ctrlKey || e.metaKey) && t.datepicker._adjustDate(e.target, o ? -1 : 1, "D"), r = e.ctrlKey || e.metaKey, e.originalEvent.altKey && t.datepicker._adjustDate(e.target, e.ctrlKey ? +t.datepicker._get(a, "stepBigMonths") : +t.datepicker._get(a, "stepMonths"), "M"); break; case 40: (e.ctrlKey || e.metaKey) && t.datepicker._adjustDate(e.target, 7, "D"), r = e.ctrlKey || e.metaKey; break; default: r = !1 } else 36 === e.keyCode && e.ctrlKey ? t.datepicker._showDatepicker(this) : r = !1; r && (e.preventDefault(), e.stopPropagation()) }, _doKeyPress: function (i) { var s, n, a = t.datepicker._getInst(i.target); return t.datepicker._get(a, "constrainInput") ? (s = t.datepicker._possibleChars(t.datepicker._get(a, "dateFormat")), n = String.fromCharCode(null == i.charCode ? i.keyCode : i.charCode), i.ctrlKey || i.metaKey || " " > n || !s || s.indexOf(n) > -1) : e }, _doKeyUp: function (e) { var i, s = t.datepicker._getInst(e.target); if (s.input.val() !== s.lastVal) try { i = t.datepicker.parseDate(t.datepicker._get(s, "dateFormat"), s.input ? s.input.val() : null, t.datepicker._getFormatConfig(s)), i && (t.datepicker._setDateFromField(s), t.datepicker._updateAlternate(s), t.datepicker._updateDatepicker(s)) } catch (n) { } return !0 }, _showDatepicker: function (e) { if (e = e.target || e, "input" !== e.nodeName.toLowerCase() && (e = t("input", e.parentNode)[0]), !t.datepicker._isDisabledDatepicker(e) && t.datepicker._lastInput !== e) { var i, s, a, r, o, h, l; i = t.datepicker._getInst(e), t.datepicker._curInst && t.datepicker._curInst !== i && (t.datepicker._curInst.dpDiv.stop(!0, !0), i && t.datepicker._datepickerShowing && t.datepicker._hideDatepicker(t.datepicker._curInst.input[0])), s = t.datepicker._get(i, "beforeShow"), a = s ? s.apply(e, [e, i]) : {}, a !== !1 && (n(i.settings, a), i.lastVal = null, t.datepicker._lastInput = e, t.datepicker._setDateFromField(i), t.datepicker._inDialog && (e.value = ""), t.datepicker._pos || (t.datepicker._pos = t.datepicker._findPos(e), t.datepicker._pos[1] += e.offsetHeight), r = !1, t(e).parents().each(function () { return r |= "fixed" === t(this).css("position"), !r }), o = { left: t.datepicker._pos[0], top: t.datepicker._pos[1] }, t.datepicker._pos = null, i.dpDiv.empty(), i.dpDiv.css({ position: "absolute", display: "block", top: "-1000px" }), t.datepicker._updateDatepicker(i), o = t.datepicker._checkOffset(i, o, r), i.dpDiv.css({ position: t.datepicker._inDialog && t.blockUI ? "static" : r ? "fixed" : "absolute", display: "none", left: o.left + "px", top: o.top + "px" }), i.inline || (h = t.datepicker._get(i, "showAnim"), l = t.datepicker._get(i, "duration"), i.dpDiv.zIndex(t(e).zIndex() + 1), t.datepicker._datepickerShowing = !0, t.effects && t.effects.effect[h] ? i.dpDiv.show(h, t.datepicker._get(i, "showOptions"), l) : i.dpDiv[h || "show"](h ? l : null), t.datepicker._shouldFocusInput(i) && i.input.focus(), t.datepicker._curInst = i)) } }, _updateDatepicker: function (e) { this.maxRows = 4, a = e, e.dpDiv.empty().append(this._generateHTML(e)), this._attachHandlers(e), e.dpDiv.find("." + this._dayOverClass + " a").mouseover(); var i, s = this._getNumberOfMonths(e), n = s[1], r = 17; e.dpDiv.removeClass("ui-datepicker-multi-2 ui-datepicker-multi-3 ui-datepicker-multi-4").width(""), n > 1 && e.dpDiv.addClass("ui-datepicker-multi-" + n).css("width", r * n + "em"), e.dpDiv[(1 !== s[0] || 1 !== s[1] ? "add" : "remove") + "Class"]("ui-datepicker-multi"), e.dpDiv[(this._get(e, "isRTL") ? "add" : "remove") + "Class"]("ui-datepicker-rtl"), e === t.datepicker._curInst && t.datepicker._datepickerShowing && t.datepicker._shouldFocusInput(e) && e.input.focus(), e.yearshtml && (i = e.yearshtml, setTimeout(function () { i === e.yearshtml && e.yearshtml && e.dpDiv.find("select.ui-datepicker-year:first").replaceWith(e.yearshtml), i = e.yearshtml = null }, 0)) }, _shouldFocusInput: function (t) { return t.input && t.input.is(":visible") && !t.input.is(":disabled") && !t.input.is(":focus") }, _checkOffset: function (e, i, s) { var n = e.dpDiv.outerWidth(), a = e.dpDiv.outerHeight(), r = e.input ? e.input.outerWidth() : 0, o = e.input ? e.input.outerHeight() : 0, h = document.documentElement.clientWidth + (s ? 0 : t(document).scrollLeft()), l = document.documentElement.clientHeight + (s ? 0 : t(document).scrollTop()); return i.left -= this._get(e, "isRTL") ? n - r : 0, i.left -= s && i.left === e.input.offset().left ? t(document).scrollLeft() : 0, i.top -= s && i.top === e.input.offset().top + o ? t(document).scrollTop() : 0, i.left -= Math.min(i.left, i.left + n > h && h > n ? Math.abs(i.left + n - h) : 0), i.top -= Math.min(i.top, i.top + a > l && l > a ? Math.abs(a + o) : 0), i }, _findPos: function (e) { for (var i, s = this._getInst(e), n = this._get(s, "isRTL") ; e && ("hidden" === e.type || 1 !== e.nodeType || t.expr.filters.hidden(e)) ;) e = e[n ? "previousSibling" : "nextSibling"]; return i = t(e).offset(), [i.left, i.top] }, _hideDatepicker: function (e) { var i, s, n, a, o = this._curInst; !o || e && o !== t.data(e, r) || this._datepickerShowing && (i = this._get(o, "showAnim"), s = this._get(o, "duration"), n = function () { t.datepicker._tidyDialog(o) }, t.effects && (t.effects.effect[i] || t.effects[i]) ? o.dpDiv.hide(i, t.datepicker._get(o, "showOptions"), s, n) : o.dpDiv["slideDown" === i ? "slideUp" : "fadeIn" === i ? "fadeOut" : "hide"](i ? s : null, n), i || n(), this._datepickerShowing = !1, a = this._get(o, "onClose"), a && a.apply(o.input ? o.input[0] : null, [o.input ? o.input.val() : "", o]), this._lastInput = null, this._inDialog && (this._dialogInput.css({ position: "absolute", left: "0", top: "-100px" }), t.blockUI && (t.unblockUI(), t("body").append(this.dpDiv))), this._inDialog = !1) }, _tidyDialog: function (t) { t.dpDiv.removeClass(this._dialogClass).unbind(".ui-datepicker-calendar") }, _checkExternalClick: function (e) { if (t.datepicker._curInst) { var i = t(e.target), s = t.datepicker._getInst(i[0]); (i[0].id !== t.datepicker._mainDivId && 0 === i.parents("#" + t.datepicker._mainDivId).length && !i.hasClass(t.datepicker.markerClassName) && !i.closest("." + t.datepicker._triggerClass).length && t.datepicker._datepickerShowing && (!t.datepicker._inDialog || !t.blockUI) || i.hasClass(t.datepicker.markerClassName) && t.datepicker._curInst !== s) && t.datepicker._hideDatepicker() } }, _adjustDate: function (e, i, s) { var n = t(e), a = this._getInst(n[0]); this._isDisabledDatepicker(n[0]) || (this._adjustInstDate(a, i + ("M" === s ? this._get(a, "showCurrentAtPos") : 0), s), this._updateDatepicker(a)) }, _gotoToday: function (e) { var i, s = t(e), n = this._getInst(s[0]); this._get(n, "gotoCurrent") && n.currentDay ? (n.selectedDay = n.currentDay, n.drawMonth = n.selectedMonth = n.currentMonth, n.drawYear = n.selectedYear = n.currentYear) : (i = new Date, n.selectedDay = i.getDate(), n.drawMonth = n.selectedMonth = i.getMonth(), n.drawYear = n.selectedYear = i.getFullYear()), this._notifyChange(n), this._adjustDate(s) }, _selectMonthYear: function (e, i, s) { var n = t(e), a = this._getInst(n[0]); a["selected" + ("M" === s ? "Month" : "Year")] = a["draw" + ("M" === s ? "Month" : "Year")] = parseInt(i.options[i.selectedIndex].value, 10), this._notifyChange(a), this._adjustDate(n) }, _selectDay: function (e, i, s, n) { var a, r = t(e); t(n).hasClass(this._unselectableClass) || this._isDisabledDatepicker(r[0]) || (a = this._getInst(r[0]), a.selectedDay = a.currentDay = t("a", n).html(), a.selectedMonth = a.currentMonth = i, a.selectedYear = a.currentYear = s, this._selectDate(e, this._formatDate(a, a.currentDay, a.currentMonth, a.currentYear))) }, _clearDate: function (e) { var i = t(e); this._selectDate(i, "") }, _selectDate: function (e, i) { var s, n = t(e), a = this._getInst(n[0]); i = null != i ? i : this._formatDate(a), a.input && a.input.val(i), this._updateAlternate(a), s = this._get(a, "onSelect"), s ? s.apply(a.input ? a.input[0] : null, [i, a]) : a.input && a.input.trigger("change"), a.inline ? this._updateDatepicker(a) : (this._hideDatepicker(), this._lastInput = a.input[0], "object" != typeof a.input[0] && a.input.focus(), this._lastInput = null) }, _updateAlternate: function (e) { var i, s, n, a = this._get(e, "altField"); a && (i = this._get(e, "altFormat") || this._get(e, "dateFormat"), s = this._getDate(e), n = this.formatDate(i, s, this._getFormatConfig(e)), t(a).each(function () { t(this).val(n) })) }, noWeekends: function (t) { var e = t.getDay(); return [e > 0 && 6 > e, ""] }, iso8601Week: function (t) { var e, i = new Date(t.getTime()); return i.setDate(i.getDate() + 4 - (i.getDay() || 7)), e = i.getTime(), i.setMonth(0), i.setDate(1), Math.floor(Math.round((e - i) / 864e5) / 7) + 1 }, parseDate: function (i, s, n) { if (null == i || null == s) throw "Invalid arguments"; if (s = "object" == typeof s ? "" + s : s + "", "" === s) return null; var a, r, o, h, l = 0, c = (n ? n.shortYearCutoff : null) || this._defaults.shortYearCutoff, u = "string" != typeof c ? c : (new Date).getFullYear() % 100 + parseInt(c, 10), d = (n ? n.dayNamesShort : null) || this._defaults.dayNamesShort, p = (n ? n.dayNames : null) || this._defaults.dayNames, f = (n ? n.monthNamesShort : null) || this._defaults.monthNamesShort, m = (n ? n.monthNames : null) || this._defaults.monthNames, g = -1, v = -1, _ = -1, b = -1, y = !1, x = function (t) { var e = i.length > a + 1 && i.charAt(a + 1) === t; return e && a++, e }, k = function (t) { var e = x(t), i = "@" === t ? 14 : "!" === t ? 20 : "y" === t && e ? 4 : "o" === t ? 3 : 2, n = RegExp("^\\d{1," + i + "}"), a = s.substring(l).match(n); if (!a) throw "Missing number at position " + l; return l += a[0].length, parseInt(a[0], 10) }, w = function (i, n, a) { var r = -1, o = t.map(x(i) ? a : n, function (t, e) { return [[e, t]] }).sort(function (t, e) { return -(t[1].length - e[1].length) }); if (t.each(o, function (t, i) { var n = i[1]; return s.substr(l, n.length).toLowerCase() === n.toLowerCase() ? (r = i[0], l += n.length, !1) : e }), -1 !== r) return r + 1; throw "Unknown name at position " + l }, D = function () { if (s.charAt(l) !== i.charAt(a)) throw "Unexpected literal at position " + l; l++ }; for (a = 0; i.length > a; a++) if (y) "'" !== i.charAt(a) || x("'") ? D() : y = !1; else switch (i.charAt(a)) { case "d": _ = k("d"); break; case "D": w("D", d, p); break; case "o": b = k("o"); break; case "m": v = k("m"); break; case "M": v = w("M", f, m); break; case "y": g = k("y"); break; case "@": h = new Date(k("@")), g = h.getFullYear(), v = h.getMonth() + 1, _ = h.getDate(); break; case "!": h = new Date((k("!") - this._ticksTo1970) / 1e4), g = h.getFullYear(), v = h.getMonth() + 1, _ = h.getDate(); break; case "'": x("'") ? D() : y = !0; break; default: D() } if (s.length > l && (o = s.substr(l), !/^\s+/.test(o))) throw "Extra/unparsed characters found in date: " + o; if (-1 === g ? g = (new Date).getFullYear() : 100 > g && (g += (new Date).getFullYear() - (new Date).getFullYear() % 100 + (u >= g ? 0 : -100)), b > -1) for (v = 1, _ = b; ;) { if (r = this._getDaysInMonth(g, v - 1), r >= _) break; v++, _ -= r } if (h = this._daylightSavingAdjust(new Date(g, v - 1, _)), h.getFullYear() !== g || h.getMonth() + 1 !== v || h.getDate() !== _) throw "Invalid date"; return h }, ATOM: "yy-mm-dd", COOKIE: "D, dd M yy", ISO_8601: "yy-mm-dd", RFC_822: "D, d M y", RFC_850: "DD, dd-M-y", RFC_1036: "D, d M y", RFC_1123: "D, d M yy", RFC_2822: "D, d M yy", RSS: "D, d M y", TICKS: "!", TIMESTAMP: "@", W3C: "yy-mm-dd", _ticksTo1970: 1e7 * 60 * 60 * 24 * (718685 + Math.floor(492.5) - Math.floor(19.7) + Math.floor(4.925)), formatDate: function (t, e, i) { if (!e) return ""; var s, n = (i ? i.dayNamesShort : null) || this._defaults.dayNamesShort, a = (i ? i.dayNames : null) || this._defaults.dayNames, r = (i ? i.monthNamesShort : null) || this._defaults.monthNamesShort, o = (i ? i.monthNames : null) || this._defaults.monthNames, h = function (e) { var i = t.length > s + 1 && t.charAt(s + 1) === e; return i && s++, i }, l = function (t, e, i) { var s = "" + e; if (h(t)) for (; i > s.length;) s = "0" + s; return s }, c = function (t, e, i, s) { return h(t) ? s[e] : i[e] }, u = "", d = !1; if (e) for (s = 0; t.length > s; s++) if (d) "'" !== t.charAt(s) || h("'") ? u += t.charAt(s) : d = !1; else switch (t.charAt(s)) { case "d": u += l("d", e.getDate(), 2); break; case "D": u += c("D", e.getDay(), n, a); break; case "o": u += l("o", Math.round((new Date(e.getFullYear(), e.getMonth(), e.getDate()).getTime() - new Date(e.getFullYear(), 0, 0).getTime()) / 864e5), 3); break; case "m": u += l("m", e.getMonth() + 1, 2); break; case "M": u += c("M", e.getMonth(), r, o); break; case "y": u += h("y") ? e.getFullYear() : (10 > e.getYear() % 100 ? "0" : "") + e.getYear() % 100; break; case "@": u += e.getTime(); break; case "!": u += 1e4 * e.getTime() + this._ticksTo1970; break; case "'": h("'") ? u += "'" : d = !0; break; default: u += t.charAt(s) } return u }, _possibleChars: function (t) { var e, i = "", s = !1, n = function (i) { var s = t.length > e + 1 && t.charAt(e + 1) === i; return s && e++, s }; for (e = 0; t.length > e; e++) if (s) "'" !== t.charAt(e) || n("'") ? i += t.charAt(e) : s = !1; else switch (t.charAt(e)) { case "d": case "m": case "y": case "@": i += "0123456789"; break; case "D": case "M": return null; case "'": n("'") ? i += "'" : s = !0; break; default: i += t.charAt(e) } return i }, _get: function (t, i) { return t.settings[i] !== e ? t.settings[i] : this._defaults[i] }, _setDateFromField: function (t, e) { if (t.input.val() !== t.lastVal) { var i = this._get(t, "dateFormat"), s = t.lastVal = t.input ? t.input.val() : null, n = this._getDefaultDate(t), a = n, r = this._getFormatConfig(t); try { a = this.parseDate(i, s, r) || n } catch (o) { s = e ? "" : s } t.selectedDay = a.getDate(), t.drawMonth = t.selectedMonth = a.getMonth(), t.drawYear = t.selectedYear = a.getFullYear(), t.currentDay = s ? a.getDate() : 0, t.currentMonth = s ? a.getMonth() : 0, t.currentYear = s ? a.getFullYear() : 0, this._adjustInstDate(t) } }, _getDefaultDate: function (t) { return this._restrictMinMax(t, this._determineDate(t, this._get(t, "defaultDate"), new Date)) }, _determineDate: function (e, i, s) { var n = function (t) { var e = new Date; return e.setDate(e.getDate() + t), e }, a = function (i) { try { return t.datepicker.parseDate(t.datepicker._get(e, "dateFormat"), i, t.datepicker._getFormatConfig(e)) } catch (s) { } for (var n = (i.toLowerCase().match(/^c/) ? t.datepicker._getDate(e) : null) || new Date, a = n.getFullYear(), r = n.getMonth(), o = n.getDate(), h = /([+\-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?/g, l = h.exec(i) ; l;) { switch (l[2] || "d") { case "d": case "D": o += parseInt(l[1], 10); break; case "w": case "W": o += 7 * parseInt(l[1], 10); break; case "m": case "M": r += parseInt(l[1], 10), o = Math.min(o, t.datepicker._getDaysInMonth(a, r)); break; case "y": case "Y": a += parseInt(l[1], 10), o = Math.min(o, t.datepicker._getDaysInMonth(a, r)) } l = h.exec(i) } return new Date(a, r, o) }, r = null == i || "" === i ? s : "string" == typeof i ? a(i) : "number" == typeof i ? isNaN(i) ? s : n(i) : new Date(i.getTime()); return r = r && "Invalid Date" == "" + r ? s : r, r && (r.setHours(0), r.setMinutes(0), r.setSeconds(0), r.setMilliseconds(0)), this._daylightSavingAdjust(r) }, _daylightSavingAdjust: function (t) { return t ? (t.setHours(t.getHours() > 12 ? t.getHours() + 2 : 0), t) : null }, _setDate: function (t, e, i) { var s = !e, n = t.selectedMonth, a = t.selectedYear, r = this._restrictMinMax(t, this._determineDate(t, e, new Date)); t.selectedDay = t.currentDay = r.getDate(), t.drawMonth = t.selectedMonth = t.currentMonth = r.getMonth(), t.drawYear = t.selectedYear = t.currentYear = r.getFullYear(), n === t.selectedMonth && a === t.selectedYear || i || this._notifyChange(t), this._adjustInstDate(t), t.input && t.input.val(s ? "" : this._formatDate(t)) }, _getDate: function (t) { var e = !t.currentYear || t.input && "" === t.input.val() ? null : this._daylightSavingAdjust(new Date(t.currentYear, t.currentMonth, t.currentDay)); return e }, _attachHandlers: function (e) { var i = this._get(e, "stepMonths"), s = "#" + e.id.replace(/\\\\/g, "\\"); e.dpDiv.find("[data-handler]").map(function () { var e = { prev: function () { t.datepicker._adjustDate(s, -i, "M") }, next: function () { t.datepicker._adjustDate(s, +i, "M") }, hide: function () { t.datepicker._hideDatepicker() }, today: function () { t.datepicker._gotoToday(s) }, selectDay: function () { return t.datepicker._selectDay(s, +this.getAttribute("data-month"), +this.getAttribute("data-year"), this), !1 }, selectMonth: function () { return t.datepicker._selectMonthYear(s, this, "M"), !1 }, selectYear: function () { return t.datepicker._selectMonthYear(s, this, "Y"), !1 } }; t(this).bind(this.getAttribute("data-event"), e[this.getAttribute("data-handler")]) }) }, _generateHTML: function (t) { var e, i, s, n, a, r, o, h, l, c, u, d, p, f, m, g, v, _, b, y, x, k, w, D, T, C, M, S, N, I, P, A, z, H, E, F, O, W, j, R = new Date, L = this._daylightSavingAdjust(new Date(R.getFullYear(), R.getMonth(), R.getDate())), Y = this._get(t, "isRTL"), B = this._get(t, "showButtonPanel"), J = this._get(t, "hideIfNoPrevNext"), K = this._get(t, "navigationAsDateFormat"), Q = this._getNumberOfMonths(t), V = this._get(t, "showCurrentAtPos"), U = this._get(t, "stepMonths"), q = 1 !== Q[0] || 1 !== Q[1], X = this._daylightSavingAdjust(t.currentDay ? new Date(t.currentYear, t.currentMonth, t.currentDay) : new Date(9999, 9, 9)), G = this._getMinMaxDate(t, "min"), $ = this._getMinMaxDate(t, "max"), Z = t.drawMonth - V, te = t.drawYear; if (0 > Z && (Z += 12, te--), $) for (e = this._daylightSavingAdjust(new Date($.getFullYear(), $.getMonth() - Q[0] * Q[1] + 1, $.getDate())), e = G && G > e ? G : e; this._daylightSavingAdjust(new Date(te, Z, 1)) > e;) Z--, 0 > Z && (Z = 11, te--); for (t.drawMonth = Z, t.drawYear = te, i = this._get(t, "prevText"), i = K ? this.formatDate(i, this._daylightSavingAdjust(new Date(te, Z - U, 1)), this._getFormatConfig(t)) : i, s = this._canAdjustMonth(t, -1, te, Z) ? "<a class='ui-datepicker-prev ui-corner-all' data-handler='prev' data-event='click' title='" + i + "'><span class='ui-icon ui-icon-circle-triangle-" + (Y ? "e" : "w") + "'>" + i + "</span></a>" : J ? "" : "<a class='ui-datepicker-prev ui-corner-all ui-state-disabled' title='" + i + "'><span class='ui-icon ui-icon-circle-triangle-" + (Y ? "e" : "w") + "'>" + i + "</span></a>", n = this._get(t, "nextText"), n = K ? this.formatDate(n, this._daylightSavingAdjust(new Date(te, Z + U, 1)), this._getFormatConfig(t)) : n, a = this._canAdjustMonth(t, 1, te, Z) ? "<a class='ui-datepicker-next ui-corner-all' data-handler='next' data-event='click' title='" + n + "'><span class='ui-icon ui-icon-circle-triangle-" + (Y ? "w" : "e") + "'>" + n + "</span></a>" : J ? "" : "<a class='ui-datepicker-next ui-corner-all ui-state-disabled' title='" + n + "'><span class='ui-icon ui-icon-circle-triangle-" + (Y ? "w" : "e") + "'>" + n + "</span></a>", r = this._get(t, "currentText"), o = this._get(t, "gotoCurrent") && t.currentDay ? X : L, r = K ? this.formatDate(r, o, this._getFormatConfig(t)) : r, h = t.inline ? "" : "<button type='button' class='ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all' data-handler='hide' data-event='click'>" + this._get(t, "closeText") + "</button>", l = B ? "<div class='ui-datepicker-buttonpane ui-widget-content'>" + (Y ? h : "") + (this._isInRange(t, o) ? "<button type='button' class='ui-datepicker-current ui-state-default ui-priority-secondary ui-corner-all' data-handler='today' data-event='click'>" + r + "</button>" : "") + (Y ? "" : h) + "</div>" : "", c = parseInt(this._get(t, "firstDay"), 10), c = isNaN(c) ? 0 : c, u = this._get(t, "showWeek"), d = this._get(t, "dayNames"), p = this._get(t, "dayNamesMin"), f = this._get(t, "monthNames"), m = this._get(t, "monthNamesShort"), g = this._get(t, "beforeShowDay"), v = this._get(t, "showOtherMonths"), _ = this._get(t, "selectOtherMonths"), b = this._getDefaultDate(t), y = "", k = 0; Q[0] > k; k++) { for (w = "", this.maxRows = 4, D = 0; Q[1] > D; D++) { if (T = this._daylightSavingAdjust(new Date(te, Z, t.selectedDay)), C = " ui-corner-all", M = "", q) { if (M += "<div class='ui-datepicker-group", Q[1] > 1) switch (D) { case 0: M += " ui-datepicker-group-first", C = " ui-corner-" + (Y ? "right" : "left"); break; case Q[1] - 1: M += " ui-datepicker-group-last", C = " ui-corner-" + (Y ? "left" : "right"); break; default: M += " ui-datepicker-group-middle", C = "" } M += "'>" } for (M += "<div class='ui-datepicker-header ui-widget-header ui-helper-clearfix" + C + "'>" + (/all|left/.test(C) && 0 === k ? Y ? a : s : "") + (/all|right/.test(C) && 0 === k ? Y ? s : a : "") + this._generateMonthYearHeader(t, Z, te, G, $, k > 0 || D > 0, f, m) + "</div><table class='ui-datepicker-calendar'><thead>" + "<tr>", S = u ? "<th class='ui-datepicker-week-col'>" + this._get(t, "weekHeader") + "</th>" : "", x = 0; 7 > x; x++) N = (x + c) % 7, S += "<th" + ((x + c + 6) % 7 >= 5 ? " class='ui-datepicker-week-end'" : "") + ">" + "<span title='" + d[N] + "'>" + p[N] + "</span></th>"; for (M += S + "</tr></thead><tbody>", I = this._getDaysInMonth(te, Z), te === t.selectedYear && Z === t.selectedMonth && (t.selectedDay = Math.min(t.selectedDay, I)), P = (this._getFirstDayOfMonth(te, Z) - c + 7) % 7, A = Math.ceil((P + I) / 7), z = q ? this.maxRows > A ? this.maxRows : A : A, this.maxRows = z, H = this._daylightSavingAdjust(new Date(te, Z, 1 - P)), E = 0; z > E; E++) { for (M += "<tr>", F = u ? "<td class='ui-datepicker-week-col'>" + this._get(t, "calculateWeek")(H) + "</td>" : "", x = 0; 7 > x; x++) O = g ? g.apply(t.input ? t.input[0] : null, [H]) : [!0, ""], W = H.getMonth() !== Z, j = W && !_ || !O[0] || G && G > H || $ && H > $, F += "<td class='" + ((x + c + 6) % 7 >= 5 ? " ui-datepicker-week-end" : "") + (W ? " ui-datepicker-other-month" : "") + (H.getTime() === T.getTime() && Z === t.selectedMonth && t._keyEvent || b.getTime() === H.getTime() && b.getTime() === T.getTime() ? " " + this._dayOverClass : "") + (j ? " " + this._unselectableClass + " ui-state-disabled" : "") + (W && !v ? "" : " " + O[1] + (H.getTime() === X.getTime() ? " " + this._currentClass : "") + (H.getTime() === L.getTime() ? " ui-datepicker-today" : "")) + "'" + (W && !v || !O[2] ? "" : " title='" + O[2].replace(/'/g, "&#39;") + "'") + (j ? "" : " data-handler='selectDay' data-event='click' data-month='" + H.getMonth() + "' data-year='" + H.getFullYear() + "'") + ">" + (W && !v ? "&#xa0;" : j ? "<span class='ui-state-default'>" + H.getDate() + "</span>" : "<a class='ui-state-default" + (H.getTime() === L.getTime() ? " ui-state-highlight" : "") + (H.getTime() === X.getTime() ? " ui-state-active" : "") + (W ? " ui-priority-secondary" : "") + "' href='#'>" + H.getDate() + "</a>") + "</td>", H.setDate(H.getDate() + 1), H = this._daylightSavingAdjust(H); M += F + "</tr>" } Z++, Z > 11 && (Z = 0, te++), M += "</tbody></table>" + (q ? "</div>" + (Q[0] > 0 && D === Q[1] - 1 ? "<div class='ui-datepicker-row-break'></div>" : "") : ""), w += M } y += w } return y += l, t._keyEvent = !1, y }, _generateMonthYearHeader: function (t, e, i, s, n, a, r, o) {
			var h, l, c, u, d, p, f, m, g = this._get(t, "changeMonth"), v = this._get(t, "changeYear"), _ = this._get(t, "showMonthAfterYear"), b = "<div class='ui-datepicker-title'>", y = ""; if (a || !g) y += "<span class='ui-datepicker-month'>" + r[e] + "</span>"; else { for (h = s && s.getFullYear() === i, l = n && n.getFullYear() === i, y += "<select class='ui-datepicker-month' data-handler='selectMonth' data-event='change'>", c = 0; 12 > c; c++) (!h || c >= s.getMonth()) && (!l || n.getMonth() >= c) && (y += "<option value='" + c + "'" + (c === e ? " selected='selected'" : "") + ">" + o[c] + "</option>"); y += "</select>" } if (_ || (b += y + (!a && g && v ? "" : "&#xa0;")), !t.yearshtml) if (t.yearshtml = "", a || !v) b += "<span class='ui-datepicker-year'>" + i + "</span>"; else {
				for (u = this._get(t, "yearRange").split(":"), d = (new Date).getFullYear(), p = function (t) {
				var e = t.match(/c[+\-].*/) ? i + parseInt(t.substring(1), 10) : t.match(/[+\-].*/) ? d + parseInt(t, 10) : parseInt(t, 10);
				return isNaN(e) ? d : e
				}, f = p(u[0]), m = Math.max(f, p(u[1] || "")), f = s ? Math.max(f, s.getFullYear()) : f, m = n ? Math.min(m, n.getFullYear()) : m, t.yearshtml += "<select class='ui-datepicker-year' data-handler='selectYear' data-event='change'>"; m >= f; f++) t.yearshtml += "<option value='" + f + "'" + (f === i ? " selected='selected'" : "") + ">" + f + "</option>"; t.yearshtml += "</select>", b += t.yearshtml, t.yearshtml = null
			} return b += this._get(t, "yearSuffix"), _ && (b += (!a && g && v ? "" : "&#xa0;") + y), b += "</div>"
		}, _adjustInstDate: function (t, e, i) { var s = t.drawYear + ("Y" === i ? e : 0), n = t.drawMonth + ("M" === i ? e : 0), a = Math.min(t.selectedDay, this._getDaysInMonth(s, n)) + ("D" === i ? e : 0), r = this._restrictMinMax(t, this._daylightSavingAdjust(new Date(s, n, a))); t.selectedDay = r.getDate(), t.drawMonth = t.selectedMonth = r.getMonth(), t.drawYear = t.selectedYear = r.getFullYear(), ("M" === i || "Y" === i) && this._notifyChange(t) }, _restrictMinMax: function (t, e) { var i = this._getMinMaxDate(t, "min"), s = this._getMinMaxDate(t, "max"), n = i && i > e ? i : e; return s && n > s ? s : n }, _notifyChange: function (t) { var e = this._get(t, "onChangeMonthYear"); e && e.apply(t.input ? t.input[0] : null, [t.selectedYear, t.selectedMonth + 1, t]) }, _getNumberOfMonths: function (t) { var e = this._get(t, "numberOfMonths"); return null == e ? [1, 1] : "number" == typeof e ? [1, e] : e }, _getMinMaxDate: function (t, e) { return this._determineDate(t, this._get(t, e + "Date"), null) }, _getDaysInMonth: function (t, e) { return 32 - this._daylightSavingAdjust(new Date(t, e, 32)).getDate() }, _getFirstDayOfMonth: function (t, e) { return new Date(t, e, 1).getDay() }, _canAdjustMonth: function (t, e, i, s) { var n = this._getNumberOfMonths(t), a = this._daylightSavingAdjust(new Date(i, s + (0 > e ? e : n[0] * n[1]), 1)); return 0 > e && a.setDate(this._getDaysInMonth(a.getFullYear(), a.getMonth())), this._isInRange(t, a) }, _isInRange: function (t, e) { var i, s, n = this._getMinMaxDate(t, "min"), a = this._getMinMaxDate(t, "max"), r = null, o = null, h = this._get(t, "yearRange"); return h && (i = h.split(":"), s = (new Date).getFullYear(), r = parseInt(i[0], 10), o = parseInt(i[1], 10), i[0].match(/[+\-].*/) && (r += s), i[1].match(/[+\-].*/) && (o += s)), (!n || e.getTime() >= n.getTime()) && (!a || e.getTime() <= a.getTime()) && (!r || e.getFullYear() >= r) && (!o || o >= e.getFullYear()) }, _getFormatConfig: function (t) { var e = this._get(t, "shortYearCutoff"); return e = "string" != typeof e ? e : (new Date).getFullYear() % 100 + parseInt(e, 10), { shortYearCutoff: e, dayNamesShort: this._get(t, "dayNamesShort"), dayNames: this._get(t, "dayNames"), monthNamesShort: this._get(t, "monthNamesShort"), monthNames: this._get(t, "monthNames") } }, _formatDate: function (t, e, i, s) { e || (t.currentDay = t.selectedDay, t.currentMonth = t.selectedMonth, t.currentYear = t.selectedYear); var n = e ? "object" == typeof e ? e : this._daylightSavingAdjust(new Date(s, i, e)) : this._daylightSavingAdjust(new Date(t.currentYear, t.currentMonth, t.currentDay)); return this.formatDate(this._get(t, "dateFormat"), n, this._getFormatConfig(t)) }
	}), t.fn.datepicker = function (e) { if (!this.length) return this; t.datepicker.initialized || (t(document).mousedown(t.datepicker._checkExternalClick), t.datepicker.initialized = !0), 0 === t("#" + t.datepicker._mainDivId).length && t("body").append(t.datepicker.dpDiv); var i = Array.prototype.slice.call(arguments, 1); return "string" != typeof e || "isDisabled" !== e && "getDate" !== e && "widget" !== e ? "option" === e && 2 === arguments.length && "string" == typeof arguments[1] ? t.datepicker["_" + e + "Datepicker"].apply(t.datepicker, [this[0]].concat(i)) : this.each(function () { "string" == typeof e ? t.datepicker["_" + e + "Datepicker"].apply(t.datepicker, [this].concat(i)) : t.datepicker._attachDatepicker(this, e) }) : t.datepicker["_" + e + "Datepicker"].apply(t.datepicker, [this[0]].concat(i)) }, t.datepicker = new i, t.datepicker.initialized = !1, t.datepicker.uuid = (new Date).getTime(), t.datepicker.version = "1.10.3"
})(jQuery);


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


/*
* jQuery history plugin
* 
* The MIT License
* 
* Copyright (c) 2006-2009 Taku Sano (Mikage Sawatari)
* Copyright (c) 2010 Takayuki Miwa
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

(function ($) {
	var locationWrapper = {
		put: function (hash, win) {
			(win || window).location.hash = this.encoder(hash);
		},
		get: function (win) {
			var hash = ((win || window).location.hash).replace(/^#/, '');
			try {
				return $.browser.mozilla ? hash : decodeURIComponent(hash);
			}
			catch (error) {
				return hash;
			}
		},
		encoder: encodeURIComponent
	};

	var iframeWrapper = {
		id: "__jQuery_history",
		init: function () {
			var html = '<iframe id="' + this.id + '" style="display:none;position:absolute" src="javascript:false;" />';
			$("body").prepend(html);
			return this;
		},
		_document: function () {
			return $("#" + this.id)[0].contentWindow.document;
		},
		put: function (hash) {
			var doc = this._document();
			doc.open();
			doc.close();
			locationWrapper.put(hash, doc);
		},
		get: function () {
			return locationWrapper.get(this._document());
		}
	};

	function initObjects(options) {
		options = $.extend({
			unescape: false
		}, options || {});

		locationWrapper.encoder = encoder(options.unescape);

		function encoder(unescape_) {
			if (unescape_ === true) {
				return function (hash) { return hash; };
			}
			if (typeof unescape_ == "string" &&
               (unescape_ = partialDecoder(unescape_.split("")))
               || typeof unescape_ == "function") {
				return function (hash) { return unescape_(encodeURIComponent(hash)); };
			}
			return encodeURIComponent;
		}

		function partialDecoder(chars) {
			var re = new RegExp($.map(chars, encodeURIComponent).join("|"), "ig");
			return function (enc) { return enc.replace(re, decodeURIComponent); };
		}
	}

	var implementations = {};

	implementations.base = {
		callback: undefined,
		type: undefined,

		check: function () { },
		load: function (hash) { },
		init: function (callback, options) {
			initObjects(options);
			self.callback = callback;
			self._options = options;
			self._init();
		},

		_init: function () { },
		_options: {}
	};

	implementations.timer = {
		_appState: undefined,
		_init: function () {
			var current_hash = locationWrapper.get();
			self._appState = current_hash;
			self.callback(current_hash);
			setInterval(self.check, 100);
		},
		check: function () {
			var current_hash = locationWrapper.get();
			if (current_hash != self._appState) {
				self._appState = current_hash;
				self.callback(current_hash);
			}
		},
		load: function (hash) {
			if (hash != self._appState) {
				locationWrapper.put(hash);
				self._appState = hash;
				self.callback(hash);
			}
		}
	};

	implementations.iframeTimer = {
		_appState: undefined,
		_init: function () {
			var current_hash = locationWrapper.get();
			self._appState = current_hash;
			iframeWrapper.init().put(current_hash);
			self.callback(current_hash);
			setInterval(self.check, 100);
		},
		check: function () {
			var iframe_hash = iframeWrapper.get(),
                location_hash = locationWrapper.get();

			if (location_hash != iframe_hash) {
				if (location_hash == self._appState) {    // user used Back or Forward button
					self._appState = iframe_hash;
					locationWrapper.put(iframe_hash);
					self.callback(iframe_hash);
				} else {                              // user loaded new bookmark
					self._appState = location_hash;
					iframeWrapper.put(location_hash);
					self.callback(location_hash);
				}
			}
		},
		load: function (hash) {
			if (hash != self._appState) {
				locationWrapper.put(hash);
				iframeWrapper.put(hash);
				self._appState = hash;
				self.callback(hash);
			}
		}
	};

	implementations.hashchangeEvent = {
		_init: function () {
			self.callback(locationWrapper.get());
			$(window).bind('hashchange', self.check);
		},
		check: function () {
			self.callback(locationWrapper.get());
		},
		load: function (hash) {
			locationWrapper.put(hash);
		}
	};

	var self = $.extend({}, implementations.base);

	if ($.browser.msie && ($.browser.version < 8 || document.documentMode < 8)) {
		self.type = 'iframeTimer';
	} else if ("onhashchange" in window) {
		self.type = 'hashchangeEvent';
	} else {
		self.type = 'timer';
	}

	$.extend(self, implementations[self.type]);
	$.history = self;
})(jQuery);



/*! Copyright (c) 2010 Brandon Aaron (http://brandonaaron.net)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Thanks to: http://adomas.org/javascript-mouse-wheel/ for some pointers.
 * Thanks to: Mathias Bank(http://www.mathias-bank.de) for a scope bug fix.
 * Thanks to: Seamus Leahy for adding deltaX and deltaY
 *
 * Version: 3.0.4
 * 
 * Requires: 1.2.2+
 */
(function($) {

var types = ['DOMMouseScroll', 'mousewheel'];

$.event.special.mousewheel = {
    setup: function() {
        if ( this.addEventListener ) {
            for ( var i=types.length; i; ) {
                this.addEventListener( types[--i], handler, false );
            }
        } else {
            this.onmousewheel = handler;
        }
    },
    
    teardown: function() {
        if ( this.removeEventListener ) {
            for ( var i=types.length; i; ) {
                this.removeEventListener( types[--i], handler, false );
            }
        } else {
            this.onmousewheel = null;
        }
    }
};

$.fn.extend({
    mousewheel: function(fn) {
        return fn ? this.bind("mousewheel", fn) : this.trigger("mousewheel");
    },
    
    unmousewheel: function(fn) {
        return this.unbind("mousewheel", fn);
    }
});


function handler(event) {
    var orgEvent = event || window.event, args = [].slice.call( arguments, 1 ), delta = 0, returnValue = true, deltaX = 0, deltaY = 0;
    event = $.event.fix(orgEvent);
    event.type = "mousewheel";
    
    // Old school scrollwheel delta
    if ( event.wheelDelta ) { delta = event.wheelDelta/120; }
    if ( event.detail     ) { delta = -event.detail/3; }
    
    // New school multidimensional scroll (touchpads) deltas
    deltaY = delta;
    
    // Gecko
    if ( orgEvent.axis !== undefined && orgEvent.axis === orgEvent.HORIZONTAL_AXIS ) {
        deltaY = 0;
        deltaX = -1*delta;
    }
    
    // Webkit
    if ( orgEvent.wheelDeltaY !== undefined ) { deltaY = orgEvent.wheelDeltaY/120; }
    if ( orgEvent.wheelDeltaX !== undefined ) { deltaX = -1*orgEvent.wheelDeltaX/120; }
    
    // Add event and delta to the front of the arguments
    args.unshift(event, delta, deltaX, deltaY);
    
    return $.event.handle.apply(this, args);
}

})(jQuery);

/*!
* jQuery Form Plugin
* version: 2.87 (20-OCT-2011)
* @requires jQuery v1.3.2 or later
*
* Examples and documentation at: http://malsup.com/jquery/form/
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/
; (function ($) {

	/*
	Usage Note:
	-----------
	Do not use both ajaxSubmit and ajaxForm on the same form.  These
	functions are intended to be exclusive.  Use ajaxSubmit if you want
	to bind your own submit handler to the form.  For example,

	$(document).ready(function() {
	$('#myForm').bind('submit', function(e) {
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

	When using ajaxForm, the ajaxSubmit function will be invoked for you
	at the appropriate time.
	*/

	/**
	* ajaxSubmit() provides a mechanism for immediately submitting
	* an HTML form using AJAX.
	*/
	$.fn.ajaxSubmit = function (options) {
		// fast fail if nothing selected (http://dev.jquery.com/ticket/2752)
		if (!this.length) {
			log('ajaxSubmit: skipping submit process - no element selected');
			return this;
		}

		var method, action, url, $form = this;

		if (typeof options == 'function') {
			options = { success: options };
		}

		method = this.attr('method');
		action = this.attr('action');
		url = (typeof action === 'string') ? $.trim(action) : '';
		url = url || window.location.href || '';
		if (url) {
			// clean url (don't include hash vaue)
			url = (url.match(/^([^#]+)/) || [])[1];
		}

		options = $.extend(true, {
			url: url,
			success: $.ajaxSettings.success,
			type: method || 'GET',
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

		var qx, n, v, a = this.formToArray(options.semantic);
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
		if (qx)
			q = (q ? (q + '&' + qx) : qx);

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
			var context = options.context || options;   // jQuery 1.4+ supports scope context 
			for (var i = 0, max = callbacks.length; i < max; i++) {
				callbacks[i].apply(context, [data, status, xhr || $form, $form]);
			}
		};

		// are there files to upload?
		var fileInputs = $('input:file', this).length > 0;
		var mp = 'multipart/form-data';
		var multipart = ($form.attr('enctype') == mp || $form.attr('encoding') == mp);

		// options.iframe allows user to force iframe mode
		// 06-NOV-09: now defaulting to iframe mode if file input is detected
		if (options.iframe !== false && (fileInputs || options.iframe || multipart)) {
			// hack to fix Safari hang (thanks to Tim Molendijk for this)
			// see:  http://groups.google.com/group/jquery-dev/browse_thread/thread/36395b7ab510dd5d
			if (options.closeKeepAlive) {
				$.get(options.closeKeepAlive, function () { fileUpload(a); });
			}
			else {
				fileUpload(a);
			}
		}
		else {
			// IE7 massage (see issue 57)
			if ($.browser.msie && method == 'get' && typeof options.type === "undefined") {
				var ieMeth = $form[0].getAttribute('method');
				if (typeof ieMeth === 'string')
					options.type = ieMeth;
			}
			$.ajax(options);
		}

		// fire 'notify' event
		this.trigger('form-submit-notify', [this, options]);
		return this;


		// private function for handling file uploads (hat tip to YAHOO!)
		function fileUpload(a) {
			var form = $form[0], el, i, s, g, id, $io, io, xhr, sub, n, timedOut, timeoutHandle;
			var useProp = !!$.fn.prop;

			if (a) {
				if (useProp) {
					// ensure that every serialized input is still enabled
					for (i = 0; i < a.length; i++) {
						el = $(form[a[i].name]);
						el.prop('disabled', false);
					}
				} else {
					for (i = 0; i < a.length; i++) {
						el = $(form[a[i].name]);
						el.removeAttr('disabled');
					}
				};
			}

			if ($(':input[name=submit],:input[id=submit]', form).length) {
				// if there is an input with a name or id of 'submit' then we won't be
				// able to invoke the submit fn on the form (at least not x-browser)
				alert('Error: Form elements must not have name or id of "submit".');
				return;
			}

			s = $.extend(true, {}, $.ajaxSettings, options);
			s.context = s.context || s;
			id = 'jqFormIO' + (new Date().getTime());
			if (s.iframeTarget) {
				$io = $(s.iframeTarget);
				n = $io.attr('name');
				if (n == null)
					$io.attr('name', id);
				else
					id = n;
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
					$io.attr('src', s.iframeSrc); // abort op in progress
					xhr.error = e;
					s.error && s.error.call(s.context, xhr, e, status);
					g && $.event.trigger("ajaxError", [xhr, s, e]);
					s.complete && s.complete.call(s.context, xhr, e);
				}
			};

			g = s.global;
			// trigger ajax global events so that activity/block indicators work like normal
			if (g && !$.active++) {
				$.event.trigger("ajaxStart");
			}
			if (g) {
				$.event.trigger("ajaxSend", [xhr, s]);
			}

			if (s.beforeSend && s.beforeSend.call(s.context, xhr, s) === false) {
				if (s.global) {
					$.active--;
				}
				return;
			}
			if (xhr.aborted) {
				return;
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
				var doc = frame.contentWindow ? frame.contentWindow.document : frame.contentDocument ? frame.contentDocument : frame.document;
				return doc;
			}

			// take a breath so that pending repaints get some cpu time before the upload starts
			function doSubmit() {
				// make sure form attrs are set
				var t = $form.attr('target'), a = $form.attr('action');

				// update form attrs in IE friendly way
				form.setAttribute('target', id);
				if (!method) {
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
						if (state.toLowerCase() == 'uninitialized')
							setTimeout(checkState, 50);
					}
					catch (e) {
						log('Server abort: ', e, ' (', e.name, ')');
						cb(SERVER_ABORT);
						timeoutHandle && clearTimeout(timeoutHandle);
						timeoutHandle = undefined;
					}
				}

				// add "extra" data to form if provided in options
				var extraInputs = [];
				try {
					if (s.extraData) {
						for (var n in s.extraData) {
							extraInputs.push(
							$('<input type="hidden" name="' + n + '" />').attr('value', s.extraData[n])
								.appendTo(form)[0]);
						}
					}

					if (!s.iframeTarget) {
						// add iframe to doc and submit the form
						$io.appendTo('body');
						io.attachEvent ? io.attachEvent('onload', cb) : io.addEventListener('load', cb, false);
					}
					setTimeout(checkState, 15);
					form.submit();
				}
				finally {
					// reset attrs and remove "extra" input elements
					form.setAttribute('action', a);
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
				try {
					doc = getDoc(io);
				}
				catch (ex) {
					log('cannot access response document: ', ex);
					e = SERVER_ABORT;
				}
				if (e === CLIENT_TIMEOUT_ABORT && xhr) {
					xhr.abort('timeout');
					return;
				}
				else if (e == SERVER_ABORT && xhr) {
					xhr.abort('server abort');
					return;
				}

				if (!doc || doc.location.href == s.iframeSrc) {
					// response not received yet
					if (!timedOut)
						return;
				}
				io.detachEvent ? io.detachEvent('onload', cb) : io.removeEventListener('load', cb, false);

				var status = 'success', errMsg;
				try {
					if (timedOut) {
						throw 'timeout';
					}

					var isXml = s.dataType == 'xml' || doc.XMLDocument || $.isXMLDoc(doc);
					log('isXml=' + isXml);
					if (!isXml && window.opera && (doc.body == null || doc.body.innerHTML == '')) {
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
					if (isXml)
						s.dataType = 'xml';
					xhr.getResponseHeader = function (header) {
						var headers = { 'content-type': s.dataType };
						return headers[header];
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
					else if (dt == 'xml' && !xhr.responseXML && xhr.responseText != null) {
						xhr.responseXML = toXml(xhr.responseText);
					}

					try {
						data = httpData(xhr, dt, s);
					}
					catch (e) {
						status = 'parsererror';
						xhr.error = errMsg = (e || status);
					}
				}
				catch (e) {
					log('error caught: ', e);
					status = 'error';
					xhr.error = errMsg = (e || status);
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
					s.success && s.success.call(s.context, data, 'success', xhr);
					g && $.event.trigger("ajaxSuccess", [xhr, s]);
				}
				else if (status) {
					if (errMsg == undefined)
						errMsg = xhr.statusText;
					s.error && s.error.call(s.context, xhr, status, errMsg);
					g && $.event.trigger("ajaxError", [xhr, s, errMsg]);
				}

				g && $.event.trigger("ajaxComplete", [xhr, s]);

				if (g && ! --$.active) {
					$.event.trigger("ajaxStop");
				}

				s.complete && s.complete.call(s.context, xhr, status);

				callbackProcessed = true;
				if (s.timeout)
					clearTimeout(timeoutHandle);

				// clean up
				setTimeout(function () {
					if (!s.iframeTarget)
						$io.remove();
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
				return window['eval']('(' + s + ')');
			};

			var httpData = function (xhr, type, s) { // mostly lifted from jq1.4.4

				var ct = xhr.getResponseHeader('content-type') || '',
				xml = type === 'xml' || !type && ct.indexOf('xml') >= 0,
				data = xml ? xhr.responseXML : xhr.responseText;

				if (xml && data.documentElement.nodeName === 'parsererror') {
					$.error && $.error('parsererror');
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
		}
	};

	/**
	* ajaxForm() provides a mechanism for fully automating form submission.
	*
	* The advantages of using this method instead of ajaxSubmit() are:
	*
	* 1: This method will include coordinates for <input type="image" /> elements (if the element
	*	is used to submit the form).
	* 2. This method will include the submit element's name/value data (for the element that was
	*	used to submit the form).
	* 3. This method binds the submit() method to the form for you.
	*
	* The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
	* passes the options argument along after properly binding events for submit elements and
	* the form itself.
	*/
	$.fn.ajaxForm = function (options) {
		// in jQuery 1.3+ we can fix mistakes with the ready state
		if (this.length === 0) {
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

		return this.ajaxFormUnbind().bind('submit.form-plugin', function (e) {
			if (!e.isDefaultPrevented()) { // if event has been canceled, don't proceed
				e.preventDefault();
				$(this).ajaxSubmit(options);
			}
		}).bind('click.form-plugin', function (e) {
			var target = e.target;
			var $el = $(target);
			if (!($el.is(":submit,input:image"))) {
				// is this a child element of the submit el?  (ex: a span within a button)
				var t = $el.closest(':submit');
				if (t.length == 0) {
					return;
				}
				target = t[0];
			}
			var form = this;
			form.clk = target;
			if (target.type == 'image') {
				if (e.offsetX != undefined) {
					form.clk_x = e.offsetX;
					form.clk_y = e.offsetY;
				} else if (typeof $.fn.offset == 'function') { // try to use dimensions plugin
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
		});
	};

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
	$.fn.formToArray = function (semantic) {
		var a = [];
		if (this.length === 0) {
			return a;
		}

		var form = this[0];
		var els = semantic ? form.getElementsByTagName('*') : form.elements;
		if (!els) {
			return a;
		}

		var i, j, n, v, el, max, jmax;
		for (i = 0, max = els.length; i < max; i++) {
			el = els[i];
			n = el.name;
			if (!n) {
				continue;
			}

			if (semantic && form.clk && el.type == "image") {
				// handle image inputs on the fly when semantic == true
				if (!el.disabled && form.clk == el) {
					a.push({ name: n, value: $(el).val() });
					a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
				}
				continue;
			}

			v = $.fieldValue(el, true);
			if (v && v.constructor == Array) {
				for (j = 0, jmax = v.length; j < jmax; j++) {
					a.push({ name: n, value: v[j] });
				}
			}
			else if (v !== null && typeof v != 'undefined') {
				a.push({ name: n, value: v });
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
	*	  <input name="A" type="text" />
	*	  <input name="A" type="text" />
	*	  <input name="B" type="checkbox" value="B1" />
	*	  <input name="B" type="checkbox" value="B2"/>
	*	  <input name="C" type="radio" value="C1" />
	*	  <input name="C" type="radio" value="C2" />
	*  </fieldset></form>
	*
	*  var v = $(':text').fieldValue();
	*  // if no values are entered into the text inputs
	*  v == ['','']
	*  // if values entered into the text inputs are 'foo' and 'bar'
	*  v == ['foo','bar']
	*
	*  var v = $(':checkbox').fieldValue();
	*  // if neither checkbox is checked
	*  v === undefined
	*  // if both checkboxes are checked
	*  v == ['B1', 'B2']
	*
	*  var v = $(':radio').fieldValue();
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
	*	   array will be empty, otherwise it will contain one or more values.
	*/
	$.fn.fieldValue = function (successful) {
		for (var val = [], i = 0, max = this.length; i < max; i++) {
			var el = this[i];
			var v = $.fieldValue(el, successful);
			if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length)) {
				continue;
			}
			v.constructor == Array ? $.merge(val, v) : val.push(v);
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
			for (var i = (one ? index : 0); i < max; i++) {
				var op = ops[i];
				if (op.selected) {
					var v = op.value;
					if (!v) { // extra pain for IE...
						v = (op.attributes && op.attributes['value'] && !(op.attributes['value'].specified)) ? op.text : op.value;
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
			if (re.test(t) || tag == 'textarea' || (includeHidden && /hidden/.test(t))) {
				this.value = '';
			}
			else if (t == 'checkbox' || t == 'radio') {
				this.checked = false;
			}
			else if (tag == 'select') {
				this.selectedIndex = -1;
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
		if (!$.fn.ajaxSubmit.debug)
			return;
		var msg = '[jquery.form] ' + Array.prototype.join.call(arguments, '');
		if (window.console && window.console.log) {
			window.console.log(msg);
		}
		else if (window.opera && window.opera.postError) {
			window.opera.postError(msg);
		}
	};

})(jQuery);


/*
 * Hammer.JS
 * version 0.6.4
 * author: Eight Media
 * https://github.com/EightMedia/hammer.js
 * Licensed under the MIT license.
 */
function Hammer(element, options, undefined) {
	var self = this;

	var defaults = mergeObject({
		// prevent the default event or not... might be buggy when false
		prevent_default: false,
		css_hacks: true,

		swipe: true,
		swipe_time: 500,   // ms
		swipe_min_distance: 20,   // pixels

		drag: true,
		drag_vertical: true,
		drag_horizontal: true,
		// minimum distance before the drag event starts
		drag_min_distance: 20,    // pixels

		// pinch zoom and rotation
		transform: true,
		scale_treshold: 0.1,
		rotation_treshold: 15,    // degrees

		tap: true,
		tap_double: true,
		tap_max_interval: 300,
		tap_max_distance: 10,
		tap_double_distance: 20,

		hold: true,
		hold_timeout: 500
	}, Hammer.defaults || {});
	options = mergeObject(defaults, options);

	// some css hacks
	(function () {
		if (!options.css_hacks) {
			return false;
		}

		var vendors = ['webkit', 'moz', 'ms', 'o', ''];
		var css_props = {
			"userSelect": "none",
			"touchCallout": "none",
			"touchAction": "none",
			"userDrag": "none",
			"tapHighlightColor": "rgba(0,0,0,0)"
		};

		var prop = '';
		for (var i = 0; i < vendors.length; i++) {
			for (var p in css_props) {
				prop = p;
				if (vendors[i]) {
					prop = vendors[i] + prop.substring(0, 1).toUpperCase() + prop.substring(1);
				}
				element.style[prop] = css_props[p];
			}
		}
	})();

	// holds the distance that has been moved
	var _distance = 0;

	// holds the exact angle that has been moved
	var _angle = 0;

	// holds the direction that has been moved
	var _direction = 0;

	// holds position movement for sliding
	var _pos = {};

	// how many fingers are on the screen
	var _fingers = 0;

	var _first = false;

	var _gesture = null;
	var _prev_gesture = null;

	var _touch_start_time = null;
	var _prev_tap_pos = { x: 0, y: 0 };
	var _prev_tap_end_time = null;

	var _hold_timer = null;

	var _offset = {};

	// keep track of the mouse status
	var _mousedown = false;

	var _event_start;
	var _event_move;
	var _event_end;

	var _has_touch = ('ontouchstart' in window);

	var _can_tap = false;


	/**
     * option setter/getter
     * @param   string  key
     * @param   mixed   value
     * @return  mixed   value
     */
	this.option = function (key, val) {
		if (val !== undefined) {
			options[key] = val;
		}

		return options[key];
	};


	/**
     * angle to direction define
     * @param  float    angle
     * @return string   direction
     */
	this.getDirectionFromAngle = function (angle) {
		var directions = {
			down: angle >= 45 && angle < 135, //90
			left: angle >= 135 || angle <= -135, //180
			up: angle < -45 && angle > -135, //270
			right: angle >= -45 && angle <= 45 //0
		};

		var direction, key;
		for (key in directions) {
			if (directions[key]) {
				direction = key;
				break;
			}
		}
		return direction;
	};


	/**
     * destroy events
     * @return  void
     */
	this.destroy = function () {
		if (_has_touch) {
			removeEvent(element, "touchstart touchmove touchend touchcancel", handleEvents);
		}
			// for non-touch
		else {
			removeEvent(element, "mouseup mousedown mousemove", handleEvents);
			removeEvent(element, "mouseout", handleMouseOut);
		}
	};


	/**
     * count the number of fingers in the event
     * when no fingers are detected, one finger is returned (mouse pointer)
     * @param  event
     * @return int  fingers
     */
	function countFingers(event) {
		// there is a bug on android (until v4?) that touches is always 1,
		// so no multitouch is supported, e.g. no, zoom and rotation...
		return event.touches ? event.touches.length : 1;
	}


	/**
     * get the x and y positions from the event object
     * @param  event
     * @return array  [{ x: int, y: int }]
     */
	function getXYfromEvent(event) {
		event = event || window.event;

		// no touches, use the event pageX and pageY
		if (!_has_touch) {
			var doc = document,
                body = doc.body;

			return [{
				x: event.pageX || event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && doc.clientLeft || 0),
				y: event.pageY || event.clientY + (doc && doc.scrollTop || body && body.scrollTop || 0) - (doc && doc.clientTop || body && doc.clientTop || 0)
			}];
		}
			// multitouch, return array with positions
		else {
			var pos = [], src;
			for (var t = 0, len = event.touches.length; t < len; t++) {
				src = event.touches[t];
				pos.push({ x: src.pageX, y: src.pageY });
			}
			return pos;
		}
	}


	/**
     * calculate the angle between two points
     * @param   object  pos1 { x: int, y: int }
     * @param   object  pos2 { x: int, y: int }
     */
	function getAngle(pos1, pos2) {
		return Math.atan2(pos2.y - pos1.y, pos2.x - pos1.x) * 180 / Math.PI;
	}

	/**
     * calculate the distance between two points
     * @param   object  pos1 { x: int, y: int }
     * @param   object  pos2 { x: int, y: int }
     */
	function getDistance(pos1, pos2) {
		var x = pos2.x - pos1.x, y = pos2.y - pos1.y;
		return Math.sqrt((x * x) + (y * y));
	}


	/**
     * calculate the scale size between two fingers
     * @param   object  pos_start
     * @param   object  pos_move
     * @return  float   scale
     */
	function calculateScale(pos_start, pos_move) {
		if (pos_start.length == 2 && pos_move.length == 2) {
			var start_distance = getDistance(pos_start[0], pos_start[1]);
			var end_distance = getDistance(pos_move[0], pos_move[1]);
			return end_distance / start_distance;
		}

		return 0;
	}


	/**
     * calculate the rotation degrees between two fingers
     * @param   object  pos_start
     * @param   object  pos_move
     * @return  float   rotation
     */
	function calculateRotation(pos_start, pos_move) {
		if (pos_start.length == 2 && pos_move.length == 2) {
			var start_rotation = getAngle(pos_start[1], pos_start[0]);
			var end_rotation = getAngle(pos_move[1], pos_move[0]);
			return end_rotation - start_rotation;
		}

		return 0;
	}


	/**
     * trigger an event/callback by name with params
     * @param string name
     * @param array  params
     */
	function triggerEvent(eventName, params) {
		// return touches object
		params.touches = getXYfromEvent(params.originalEvent);
		params.type = eventName;

		// trigger callback
		if (isFunction(self["on" + eventName])) {
			self["on" + eventName].call(self, params);
		}
	}


	/**
     * cancel event
     * @param   object  event
     * @return  void
     */

	function cancelEvent(event) {
		event = event || window.event;
		if (event.preventDefault) {
			event.preventDefault();
			event.stopPropagation();
		} else {
			event.returnValue = false;
			event.cancelBubble = true;
		}
	}


	/**
     * reset the internal vars to the start values
     */
	function reset() {
		_pos = {};
		_first = false;
		_fingers = 0;
		_distance = 0;
		_angle = 0;
		_gesture = null;
	}


	var gestures = {
		// hold gesture
		// fired on touchstart
		hold: function (event) {
			// only when one finger is on the screen
			if (options.hold) {
				_gesture = 'hold';
				clearTimeout(_hold_timer);

				_hold_timer = setTimeout(function () {
					if (_gesture == 'hold') {
						triggerEvent("hold", {
							originalEvent: event,
							position: _pos.start
						});
					}
				}, options.hold_timeout);
			}
		},

		// swipe gesture
		// fired on touchend
		swipe: function (event) {
			if (!_pos.move || _gesture === "transform") {
				return;
			}

			// get the distance we moved
			var _distance_x = _pos.move[0].x - _pos.start[0].x;
			var _distance_y = _pos.move[0].y - _pos.start[0].y;
			_distance = Math.sqrt(_distance_x * _distance_x + _distance_y * _distance_y);

			// compare the kind of gesture by time
			var now = new Date().getTime();
			var touch_time = now - _touch_start_time;

			if (options.swipe && (options.swipe_time >= touch_time) && (_distance >= options.swipe_min_distance)) {
				// calculate the angle
				_angle = getAngle(_pos.start[0], _pos.move[0]);
				_direction = self.getDirectionFromAngle(_angle);

				_gesture = 'swipe';

				var position = {
					x: _pos.move[0].x - _offset.left,
					y: _pos.move[0].y - _offset.top
				};

				var event_obj = {
					originalEvent: event,
					position: position,
					direction: _direction,
					distance: _distance,
					distanceX: _distance_x,
					distanceY: _distance_y,
					angle: _angle
				};

				// normal slide event
				triggerEvent("swipe", event_obj);
			}
		},


		// drag gesture
		// fired on mousemove
		drag: function (event) {
			// get the distance we moved
			var _distance_x = _pos.move[0].x - _pos.start[0].x;
			var _distance_y = _pos.move[0].y - _pos.start[0].y;
			_distance = Math.sqrt(_distance_x * _distance_x + _distance_y * _distance_y);

			// drag
			// minimal movement required
			if (options.drag && (_distance > options.drag_min_distance) || _gesture == 'drag') {
				// calculate the angle
				_angle = getAngle(_pos.start[0], _pos.move[0]);
				_direction = self.getDirectionFromAngle(_angle);

				// check the movement and stop if we go in the wrong direction
				var is_vertical = (_direction == 'up' || _direction == 'down');

				if (((is_vertical && !options.drag_vertical) || (!is_vertical && !options.drag_horizontal)) && (_distance > options.drag_min_distance)) {
					return;
				}

				_gesture = 'drag';

				var position = {
					x: _pos.move[0].x - _offset.left,
					y: _pos.move[0].y - _offset.top
				};

				var event_obj = {
					originalEvent: event,
					position: position,
					direction: _direction,
					distance: _distance,
					distanceX: _distance_x,
					distanceY: _distance_y,
					angle: _angle
				};

				// on the first time trigger the start event
				if (_first) {
					triggerEvent("dragstart", event_obj);

					_first = false;
				}

				// normal slide event
				triggerEvent("drag", event_obj);

				cancelEvent(event);
			}
		},


		// transform gesture
		// fired on touchmove
		transform: function (event) {
			if (options.transform) {
				var count = countFingers(event);
				if (count !== 2) {
					return false;
				}

				var rotation = calculateRotation(_pos.start, _pos.move);
				var scale = calculateScale(_pos.start, _pos.move);

				if (_gesture === 'transform' ||
                    Math.abs(1 - scale) > options.scale_treshold ||
                    Math.abs(rotation) > options.rotation_treshold) {

					_gesture = 'transform';
					_pos.center = {
						x: ((_pos.move[0].x + _pos.move[1].x) / 2) - _offset.left,
						y: ((_pos.move[0].y + _pos.move[1].y) / 2) - _offset.top
					};

					if (_first)
						_pos.startCenter = _pos.center;

					var _distance_x = _pos.center.x - _pos.startCenter.x;
					var _distance_y = _pos.center.y - _pos.startCenter.y;
					_distance = Math.sqrt(_distance_x * _distance_x + _distance_y * _distance_y);

					var event_obj = {
						originalEvent: event,
						position: _pos.center,
						scale: scale,
						rotation: rotation,
						distance: _distance,
						distanceX: _distance_x,
						distanceY: _distance_y
					};

					// on the first time trigger the start event
					if (_first) {
						triggerEvent("transformstart", event_obj);
						_first = false;
					}

					triggerEvent("transform", event_obj);

					cancelEvent(event);

					return true;
				}
			}

			return false;
		},


		// tap and double tap gesture
		// fired on touchend
		tap: function (event) {
			// compare the kind of gesture by time
			var now = new Date().getTime();
			var touch_time = now - _touch_start_time;

			// dont fire when hold is fired
			if (options.hold && !(options.hold && options.hold_timeout > touch_time)) {
				return;
			}

			// when previous event was tap and the tap was max_interval ms ago
			var is_double_tap = (function () {
				if (_prev_tap_pos &&
                    options.tap_double &&
                    _prev_gesture == 'tap' &&
                    _pos.start &&
                    (_touch_start_time - _prev_tap_end_time) < options.tap_max_interval) {
					var x_distance = Math.abs(_prev_tap_pos[0].x - _pos.start[0].x);
					var y_distance = Math.abs(_prev_tap_pos[0].y - _pos.start[0].y);
					return (_prev_tap_pos && _pos.start && Math.max(x_distance, y_distance) < options.tap_double_distance);
				}
				return false;
			})();

			if (is_double_tap) {
				_gesture = 'double_tap';
				_prev_tap_end_time = null;

				triggerEvent("doubletap", {
					originalEvent: event,
					position: _pos.start
				});
				cancelEvent(event);
			}

				// single tap is single touch
			else {
				var x_distance = (_pos.move) ? Math.abs(_pos.move[0].x - _pos.start[0].x) : 0;
				var y_distance = (_pos.move) ? Math.abs(_pos.move[0].y - _pos.start[0].y) : 0;
				_distance = Math.max(x_distance, y_distance);

				if (_distance < options.tap_max_distance) {
					_gesture = 'tap';
					_prev_tap_end_time = now;
					_prev_tap_pos = _pos.start;

					if (options.tap) {
						triggerEvent("tap", {
							originalEvent: event,
							position: _pos.start
						});
						cancelEvent(event);
					}
				}
			}
		}
	};


	function handleEvents(event) {
		var count;
		switch (event.type) {
			case 'mousedown':
			case 'touchstart':
				count = countFingers(event);
				_can_tap = count === 1;

				//We were dragging and now we are zooming.
				if (count === 2 && _gesture === "drag") {

					//The user needs to have the dragend to be fired to ensure that
					//there is proper cleanup from the drag and move onto transforming.
					triggerEvent("dragend", {
						originalEvent: event,
						direction: _direction,
						distance: _distance,
						angle: _angle
					});
				}
				_setup();

				if (options.prevent_default) {
					cancelEvent(event);
				}
				break;

			case 'mousemove':
			case 'touchmove':
				count = countFingers(event);

				//The user has gone from transforming to dragging.  The
				//user needs to have the proper cleanup of the state and
				//setup with the new "start" points.
				if (!_mousedown && count === 1) {
					return false;
				} else if (!_mousedown && count === 2) {
					_can_tap = false;

					reset();
					_setup();
				}

				_event_move = event;
				_pos.move = getXYfromEvent(event);

				if (!gestures.transform(event)) {
					gestures.drag(event);
				}
				break;

			case 'mouseup':
			case 'mouseout':
			case 'touchcancel':
			case 'touchend':
				var callReset = true;

				_mousedown = false;
				_event_end = event;

				// swipe gesture
				gestures.swipe(event);

				// drag gesture
				// dragstart is triggered, so dragend is possible
				if (_gesture == 'drag') {
					triggerEvent("dragend", {
						originalEvent: event,
						direction: _direction,
						distance: _distance,
						angle: _angle
					});
				}

					// transform
					// transformstart is triggered, so transformed is possible
				else if (_gesture == 'transform') {
					// define the transform distance
					var _distance_x = _pos.center.x - _pos.startCenter.x;
					var _distance_y = _pos.center.y - _pos.startCenter.y;

					triggerEvent("transformend", {
						originalEvent: event,
						position: _pos.center,
						scale: calculateScale(_pos.start, _pos.move),
						rotation: calculateRotation(_pos.start, _pos.move),
						distance: _distance,
						distanceX: _distance_x,
						distanceY: _distance_y
					});

					//If the user goes from transformation to drag there needs to be a
					//state reset so that way a dragstart/drag/dragend will be properly
					//fired.
					if (countFingers(event) === 1) {
						reset();
						_setup();
						callReset = false;
					}
				} else if (_can_tap) {
					gestures.tap(_event_start);
				}

				_prev_gesture = _gesture;

				// trigger release event
				// "release" by default doesn't return the co-ords where your
				// finger was released. "position" will return "the last touched co-ords"

				triggerEvent("release", {
					originalEvent: event,
					gesture: _gesture,
					position: _pos.move || _pos.start
				});

				// reset vars if this was not a transform->drag touch end operation.
				if (callReset) {
					reset();
				}
				break;
		} // end switch

		/**
         * Performs a blank setup.
         * @private
         */
		function _setup() {
			_pos.start = getXYfromEvent(event);
			_touch_start_time = new Date().getTime();
			_fingers = countFingers(event);
			_first = true;
			_event_start = event;

			// borrowed from jquery offset https://github.com/jquery/jquery/blob/master/src/offset.js
			var box = element.getBoundingClientRect();
			var clientTop = element.clientTop || document.body.clientTop || 0;
			var clientLeft = element.clientLeft || document.body.clientLeft || 0;
			var scrollTop = window.pageYOffset || element.scrollTop || document.body.scrollTop;
			var scrollLeft = window.pageXOffset || element.scrollLeft || document.body.scrollLeft;

			_offset = {
				top: box.top + scrollTop - clientTop,
				left: box.left + scrollLeft - clientLeft
			};

			_mousedown = true;

			// hold gesture
			gestures.hold(event);
		}
	}


	function handleMouseOut(event) {
		if (!isInsideHammer(element, event.relatedTarget)) {
			handleEvents(event);
		}
	}


	// bind events for touch devices
	// except for windows phone 7.5, it doesnt support touch events..!
	if (_has_touch) {
		addEvent(element, "touchstart touchmove touchend touchcancel", handleEvents);
	}
		// for non-touch
	else {
		addEvent(element, "mouseup mousedown mousemove", handleEvents);
		addEvent(element, "mouseout", handleMouseOut);
	}


	/**
     * find if element is (inside) given parent element
     * @param   object  element
     * @param   object  parent
     * @return  bool    inside
     */
	function isInsideHammer(parent, child) {
		// get related target for IE
		if (!child && window.event && window.event.toElement) {
			child = window.event.toElement;
		}

		if (parent === child) {
			return true;
		}

		// loop over parentNodes of child until we find hammer element
		if (child) {
			var node = child.parentNode;
			while (node !== null) {
				if (node === parent) {
					return true;
				}
				node = node.parentNode;
			}
		}
		return false;
	}


	/**
     * merge 2 objects into a new object
     * @param   object  obj1
     * @param   object  obj2
     * @return  object  merged object
     */
	function mergeObject(obj1, obj2) {
		var output = {};

		if (!obj2) {
			return obj1;
		}

		for (var prop in obj1) {
			if (prop in obj2) {
				output[prop] = obj2[prop];
			} else {
				output[prop] = obj1[prop];
			}
		}
		return output;
	}


	/**
     * check if object is a function
     * @param   object  obj
     * @return  bool    is function
     */
	function isFunction(obj) {
		return Object.prototype.toString.call(obj) == "[object Function]";
	}


	/**
     * attach event
     * @param   node    element
     * @param   string  types
     * @param   object  callback
     */
	function addEvent(element, types, callback) {
		types = types.split(" ");
		for (var t = 0, len = types.length; t < len; t++) {
			if (element.addEventListener) {
				element.addEventListener(types[t], callback, false);
			}
			else if (document.attachEvent) {
				element.attachEvent("on" + types[t], callback);
			}
		}
	}


	/**
     * detach event
     * @param   node    element
     * @param   string  types
     * @param   object  callback
     */
	function removeEvent(element, types, callback) {
		types = types.split(" ");
		for (var t = 0, len = types.length; t < len; t++) {
			if (element.removeEventListener) {
				element.removeEventListener(types[t], callback, false);
			}
			else if (document.detachEvent) {
				element.detachEvent("on" + types[t], callback);
			}
		}
	}
}


/*
 * special event API with Hammer.JS
 * version 0.9
 * author: Damien Antipa
 * https://github.com/dantipa/hammer.js
 */
(function ($) {
	var hammerEvents = ['hold', 'tap', 'doubletap', 'transformstart', 'transform', 'transformend', 'dragstart', 'drag', 'dragend', 'swipe', 'release'];

	/*
     * HammerSEFunctions
     * maintains which function should be used to handle
     * events.
     * 
     * _fn : is the original function used to handle events.
     *  Faster than _delegateFn, but it does not support delegated
     *  or bubbled events through jQuery.
     *
     * _delegateFn : a function that supports delegated/bubbled events
     *  in jQuery, but it is slower than _fn as it doesn't use a cached
     *  jQuery call ($target) to trigger from and must construct a new
     *  jQuery object based on the event target each time the event triggers.
     *
     * addDelegate : adds the handler guid created by jQuery to _delegateGuids.
     *
     * removeDelegate : removes the handler guid (if found) in _delegateGuids.
     *
     * getFn : returns either _fn or _delegateFn based on there being any handler
     *  guids saved in _delegateGuids. If there are no guids saved, then the faster
     *  function (_fn) is used. Otherwise, _delegateFn must be used and will be
     *  returned.
     *
     * destroy : destroys all dynamically generated properties in the object. This
     *  may be overkill, but it will help to reduce the chances of any sneaky
     *  memory leaks cropping up.
     */

	function HammerSEFunctions(/*event,$target*/) {
		var event = arguments[0],
            $target = arguments[1];

		this._delegateGuids = [];

		this._fn = function (ev) {

			$target.trigger($.Event(event, ev));

		};

		this._delegateFn = function (ev) {
			var jqevt = $.Event(event, ev);

			$(jqevt.originalEvent.target).trigger(jqevt);

		};
	}
	HammerSEFunctions.prototype = {

		addDelegate: function (guid) {

			this._delegateGuids.push(guid);
			return this;

		},

		removeDelegate: function (guid) {
			var index = this._delegateGuids.indexOf(guid);

			if (index >= 0) {

				this._delegateGuids.splice(index);

			}

			return this;
		},

		getFn: function () {

			return this._delegateGuids.length > 0 ? this._delegateFn : this._fn;

		},

		destroy: function () {

			this._fn = null;
			this._delegateFn = null;
			this._delegateGuids = null;

			delete this._fn;
			delete this._delegateFn;
			delete this._delegateGuids;

		}

	};

	$.each(hammerEvents, function (i, event) {

		$.event.special[event] = {

			setup: function (data, namespaces, eventHandle) {
				var $target = $(this),
                    hammer = $target.data('hammerjs');

				if (!hammer) {
					hammer = new Hammer(this, data);
					hammer.__Fns = {};
					$target.data('hammerjs', hammer);
				}

				hammer.__Fns[event] = new HammerSEFunctions(event, $target);
			},
			add: function (handleObj) {
				var hammer = $(this).data('hammerjs');

				if (!!handleObj.selector) {
					hammer.__Fns[event].addDelegate(handleObj.guid);
				}

				hammer['on' + event] = hammer.__Fns[event].getFn();
			},
			remove: function (handleObj) {
				var hammer = $(this).data('hammerjs');

				if (!!handleObj.selector) {
					hammer.__Fns[event].removeDelegate(handleObj.guid);
				}

				hammer['on' + event] = hammer.__Fns[event].getFn();
			},
			teardown: function (namespaces) {
				var hammer = $(this).data('hammerjs');

				hammer.__Fns[event].destroy();
				delete hammer.__Fns[event];
			}
		};
	});
}(jQuery));

/*!
 * iButton jQuery Plug-in
 *
 * Copyright 2011 Giva, Inc. (http://www.givainc.com/labs/) 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * 	http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Date: 2011-07-26
 * Rev:  1.0.03
 */
; (function ($) {
	// set default options
	$.iButton = {
		version: "1.0.03",
		setDefaults: function (options) {
			$.extend(defaults, options);
		}
	};

	$.fn.iButton = function (options) {
		var method = typeof arguments[0] == "string" && arguments[0];
		var args = method && Array.prototype.slice.call(arguments, 1) || arguments;
		// get a reference to the first iButton found
		var self = (this.length == 0) ? null : $.data(this[0], "iButton");

		// if a method is supplied, execute it for non-empty results
		if (self && method && this.length) {

			// if request a copy of the object, return it			
			if (method.toLowerCase() == "object") return self;
				// if method is defined, run it and return either it's results or the chain
			else if (self[method]) {
				// define a result variable to return to the jQuery chain
				var result;
				this.each(function (i) {
					// apply the method to the current element
					var r = $.data(this, "iButton")[method].apply(self, args);
					// if first iteration we need to check if we're done processing or need to add it to the jquery chain
					if (i == 0 && r) {
						// if this is a jQuery item, we need to store them in a collection
						if (!!r.jquery) {
							result = $([]).add(r);
							// otherwise, just store the result and stop executing
						} else {
							result = r;
							// since we're a non-jQuery item, just cancel processing further items
							return false;
						}
						// keep adding jQuery objects to the results
					} else if (!!r && !!r.jquery) {
						result = result.add(r);
					}
				});

				// return either the results (which could be a jQuery object) or the original chain
				return result || this;
				// everything else, return the chain
			} else return this;
			// initializing request (only do if iButton not already initialized)
		} else {
			// create a new iButton for each object found
			return this.each(function () {
				new iButton(this, options);
			});
		};
	};

	// count instances	
	var counter = 0;
	// detect iPhone
	$.browser.iphone = (navigator.userAgent.toLowerCase().indexOf("iphone") > -1);

	var iButton = function (input, options) {
		var self = this
			, $input = $(input)
			, id = ++counter
			, disabled = false
			, width = {}
			, mouse = { dragging: false, clicked: null }
			, dragStart = { position: null, offset: null, time: null }
			// make a copy of the options and use the metadata if provided
			, options = $.extend({}, defaults, options, (!!$.metadata ? $input.metadata() : {}))
			// check to see if we're using the default labels
			, bDefaultLabelsUsed = (options.labelOn == ON && options.labelOff == OFF)
			// set valid field types
			, allow = ":checkbox, :radio";

		// only do for checkboxes buttons, if matches inside that node
		if (!$input.is(allow)) return $input.find(allow).iButton(options);
			// if iButton already exists, stop processing
		else if ($.data($input[0], "iButton")) return;

		// store a reference to this marquee
		$.data($input[0], "iButton", self);

		// if using the "auto" setting, then don't resize handle or container if using the default label (since we'll trust the CSS)
		if (options.resizeHandle == "auto") options.resizeHandle = !bDefaultLabelsUsed;
		if (options.resizeContainer == "auto") options.resizeContainer = !bDefaultLabelsUsed;

		// toggles the state of a button (or can turn on/off)
		this.toggle = function (t) {
			var toggle = (arguments.length > 0) ? t : !$input[0].checked;
			$input.attr("checked", toggle).trigger("change");
		};

		// disable/enable the control
		this.disable = function (t) {
			var toggle = (arguments.length > 0) ? t : !disabled;
			// mark the control disabled
			disabled = toggle;
			// mark the input disabled
			$input.attr("disabled", toggle);
			// set the diabled styles
			$container[toggle ? "addClass" : "removeClass"](options.classDisabled);
			// run callback
			if ($.isFunction(options.disable)) options.disable.apply(self, [disabled, $input, options]);
		};

		// repaint the button
		this.repaint = function () {
			positionHandle();
		};

		// this will destroy the iButton style
		this.destroy = function () {
			// remove behaviors
			$([$input[0], $container[0]]).unbind(".iButton");
			$(document).unbind(".iButton_" + id);
			// move the checkbox to it's original location
			$container.after($input).remove();
			// kill the reference
			$.data($input[0], "iButton", null);
			// run callback
			if ($.isFunction(options.destroy)) options.destroy.apply(self, [$input, options]);
		};

		$input
				// create the wrapper code
				.wrap('<div class="' + $.trim(options.classContainer + ' ' + options.className) + '" />')
			.after(
					  '<div class="' + options.classHandle + '"><div class="' + options.classHandleRight + '"><div class="' + options.classHandleMiddle + '" /></div></div>'
			+ '<div class="' + options.classLabelOff + '"><span><label>' + options.labelOff + '</label></span></div>'
			+ '<div class="' + options.classLabelOn + '"><span><label>' + options.labelOn + '</label></span></div>'
			+ '<div class="' + options.classPaddingLeft + '"></div><div class="' + options.classPaddingRight + '"></div>'
				);

		var $container = $input.parent()
				, $handle = $input.siblings("." + options.classHandle)
				, $offlabel = $input.siblings("." + options.classLabelOff)
				, $offspan = $offlabel.children("span")
				, $onlabel = $input.siblings("." + options.classLabelOn)
				, $onspan = $onlabel.children("span");


		// if we need to do some resizing, get the widths only once
		if (options.resizeHandle || options.resizeContainer) {
			width.onspan = $onspan.outerWidth();
			width.offspan = $offspan.outerWidth();
		}

		// automatically resize the handle
		if (options.resizeHandle) {
			width.handle = Math.min(width.onspan, width.offspan);
			$handle.css("width", width.handle);
		} else {
			width.handle = $handle.width();
		}

		// automatically resize the control
		if (options.resizeContainer) {
			width.container = (Math.max(width.onspan, width.offspan) + width.handle + 20);
			$container.css("width", width.container);
			// adjust the off label to match the new container size
			$offlabel.css("width", width.container - 5);
		} else {
			width.container = $container.width();
		}

		var handleRight = width.container - width.handle - 6;

		var positionHandle = function (animate) {
			var checked = $input[0].checked
				, x = (checked) ? handleRight : 0
				, animate = (arguments.length > 0) ? arguments[0] : true;

			if (animate && options.enableFx) {
				$handle.stop().animate({ left: x }, options.duration, options.easing);
				$onlabel.stop().animate({ width: x + 4 }, options.duration, options.easing);
				$onspan.stop().animate({ marginLeft: x - handleRight }, options.duration, options.easing);
				$offspan.stop().animate({ marginRight: -x }, options.duration, options.easing);
			} else {
				$handle.css("left", x);
				$onlabel.css("width", x + 4);
				$onspan.css("marginLeft", x - handleRight);
				$offspan.css("marginRight", -x);
			}
		};

		// place the buttons in their default location	
		positionHandle(false);

		var getDragPos = function (e) {
			return e.pageX || ((e.originalEvent.changedTouches) ? e.originalEvent.changedTouches[0].pageX : 0);
		};

		// monitor mouse clicks in the container
		$container.bind("mousedown.iButton touchstart.iButton", function (e) {
			// abort if disabled or allow clicking the input to toggle the status (if input is visible)
			if ($(e.target).is(allow) || disabled || (!options.allowRadioUncheck && $input.is(":radio:checked"))) return;

			e.preventDefault();
			mouse.clicked = $handle;
			dragStart.position = getDragPos(e);
			dragStart.offset = dragStart.position - (parseInt($handle.css("left"), 10) || 0);
			dragStart.time = (new Date()).getTime();
			return false;
		});

		// make sure dragging support is enabled		
		if (options.enableDrag) {
			// monitor mouse movement on the page
			$(document).bind("mousemove.iButton_" + id + " touchmove.iButton_" + id, function (e) {
				// if we haven't clicked on the container, cancel event
				if (mouse.clicked != $handle) { return }
				e.preventDefault();

				var x = getDragPos(e);
				if (x != dragStart.offset) {
					mouse.dragging = true;
					$container.addClass(options.classHandleActive);
				}

				// make sure number is between 0 and 1			
				var pct = Math.min(1, Math.max(0, (x - dragStart.offset) / handleRight));

				$handle.css("left", pct * handleRight);
				$onlabel.css("width", pct * handleRight + 4);
				$offspan.css("marginRight", -pct * handleRight);
				$onspan.css("marginLeft", -(1 - pct) * handleRight);

				return false;
			});
		}

		// monitor when the mouse button is released
		$(document).bind("mouseup.iButton_" + id + " touchend.iButton_" + id, function (e) {
			if (mouse.clicked != $handle) { return false }
			e.preventDefault();

			// track if the value has changed			
			var changed = true;

			// if not dragging or click time under a certain millisecond, then just toggle			
			if (!mouse.dragging || (((new Date()).getTime() - dragStart.time) < options.clickOffset)) {
				var checked = $input[0].checked;
				$input.attr("checked", !checked);

				// run callback
				if ($.isFunction(options.click)) options.click.apply(self, [!checked, $input, options]);
			} else {
				var x = getDragPos(e);

				var pct = (x - dragStart.offset) / handleRight;
				var checked = (pct >= 0.5);

				// if the value is the same, don't run change event
				if ($input[0].checked == checked) changed = false;

				$input.attr("checked", checked);
			}

			// remove the active handler class			
			$container.removeClass(options.classHandleActive);
			mouse.clicked = null;
			mouse.dragging = null;
			// run any change event for the element
			if (changed) $input.trigger("change");
				// if the value didn't change, just reset the handle
			else positionHandle();

			return false;
		});

		// animate when we get a change event
		$input
			.bind("change.iButton", function () {
				// move handle
				positionHandle();

				// if a radio element, then we must repaint the other elements in it's group to show them as not selected
				if ($input.is(":radio")) {
					var el = $input[0];

					// try to use the DOM to get the grouped elements, but if not in a form get by name attr
					var $radio = $(el.form ? el.form[el.name] : ":radio[name=" + el.name + "]");

					// repaint the radio elements that are not checked	
					$radio.filter(":not(:checked)").iButton("repaint");
				}

				// run callback
				if ($.isFunction(options.change)) options.change.apply(self, [$input, options]);
			})
			// if the element has focus, we need to highlight the container
			.bind("focus.iButton", function () {
				$container.addClass(options.classFocus);
			})
			// if the element has focus, we need to highlight the container
			.bind("blur.iButton", function () {
				$container.removeClass(options.classFocus);
			});

		// if a click event is registered, we must register on the checkbox so it's fired if triggered on the checkbox itself
		if ($.isFunction(options.click)) {
			$input.bind("click.iButton", function () {
				options.click.apply(self, [$input[0].checked, $input, options]);
			});
		}

		// if the field is disabled, mark it as such
		if ($input.is(":disabled")) this.disable(true);

		// special behaviors for IE    
		if ($.browser.msie) {
			// disable text selection in IE, other browsers are controlled via CSS
			$container.find("*").andSelf().attr("unselectable", "on");
			// IE needs to register to the "click" event to make changes immediately (the change event only occurs on blur)
			$input.bind("click.iButton", function () { $input.triggerHandler("change.iButton"); });
		}

		// run the init callback
		if ($.isFunction(options.init)) options.init.apply(self, [$input, options]);
	};

	var defaults = {
		duration: 200                           // the speed of the animation
		, easing: "swing"                         // the easing animation to use
		, labelOn: "ON"                           // the text to show when toggled on
		, labelOff: "OFF"                         // the text to show when toggled off
		, resizeHandle: "auto"                    // determines if handle should be resized
		, resizeContainer: "auto"                 // determines if container should be resized
		, enableDrag: true                        // determines if we allow dragging
		, enableFx: true                          // determines if we show animation
		, allowRadioUncheck: false                // determine if a radio button should be able to be unchecked
		, clickOffset: 120                        // if millseconds between a mousedown & mouseup event this value, then considered a mouse click

		// define the class statements
		, className: ""
		, classContainer: "ibutton-container"
		, classDisabled: "ibutton-disabled"
		, classFocus: "ibutton-focus"
		, classLabelOn: "ibutton-label-on"
		, classLabelOff: "ibutton-label-off"
		, classHandle: "ibutton-handle"
		, classHandleMiddle: "ibutton-handle-middle"
		, classHandleRight: "ibutton-handle-right"
		, classHandleActive: "ibutton-active-handle"
		, classPaddingLeft: "ibutton-padding-left"
		, classPaddingRight: "ibutton-padding-right"

		// event handlers
		, init: null                              // callback that occurs when a iButton is initialized
		, change: null                            // callback that occurs when the button state is changed
		, click: null                             // callback that occurs when the button is clicked
		, disable: null                           // callback that occurs when the button is disabled/enabled
		, destroy: null                           // callback that occurs when the button is destroyed
	}, ON = defaults.labelOn, OFF = defaults.labelOff;

})(jQuery);

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


/*!
 * jQuery Transit - CSS3 transitions and transformations
 * (c) 2011-2012 Rico Sta. Cruz <rico@ricostacruz.com>
 * MIT Licensed.
 *
 * http://ricostacruz.com/jquery.transit
 * http://github.com/rstacruz/jquery.transit
 */
(function (k) { k.transit = { version: "0.9.9", propertyMap: { marginLeft: "margin", marginRight: "margin", marginBottom: "margin", marginTop: "margin", paddingLeft: "padding", paddingRight: "padding", paddingBottom: "padding", paddingTop: "padding" }, enabled: true, useTransitionEnd: false }; var d = document.createElement("div"); var q = {}; function b(v) { if (v in d.style) { return v } var u = ["Moz", "Webkit", "O", "ms"]; var r = v.charAt(0).toUpperCase() + v.substr(1); if (v in d.style) { return v } for (var t = 0; t < u.length; ++t) { var s = u[t] + r; if (s in d.style) { return s } } } function e() { d.style[q.transform] = ""; d.style[q.transform] = "rotateY(90deg)"; return d.style[q.transform] !== "" } var a = navigator.userAgent.toLowerCase().indexOf("chrome") > -1; q.transition = b("transition"); q.transitionDelay = b("transitionDelay"); q.transform = b("transform"); q.transformOrigin = b("transformOrigin"); q.transform3d = e(); var i = { transition: "transitionEnd", MozTransition: "transitionend", OTransition: "oTransitionEnd", WebkitTransition: "webkitTransitionEnd", msTransition: "MSTransitionEnd" }; var f = q.transitionEnd = i[q.transition] || null; for (var p in q) { if (q.hasOwnProperty(p) && typeof k.support[p] === "undefined") { k.support[p] = q[p] } } d = null; k.cssEase = { _default: "ease", "in": "ease-in", out: "ease-out", "in-out": "ease-in-out", snap: "cubic-bezier(0,1,.5,1)", easeOutCubic: "cubic-bezier(.215,.61,.355,1)", easeInOutCubic: "cubic-bezier(.645,.045,.355,1)", easeInCirc: "cubic-bezier(.6,.04,.98,.335)", easeOutCirc: "cubic-bezier(.075,.82,.165,1)", easeInOutCirc: "cubic-bezier(.785,.135,.15,.86)", easeInExpo: "cubic-bezier(.95,.05,.795,.035)", easeOutExpo: "cubic-bezier(.19,1,.22,1)", easeInOutExpo: "cubic-bezier(1,0,0,1)", easeInQuad: "cubic-bezier(.55,.085,.68,.53)", easeOutQuad: "cubic-bezier(.25,.46,.45,.94)", easeInOutQuad: "cubic-bezier(.455,.03,.515,.955)", easeInQuart: "cubic-bezier(.895,.03,.685,.22)", easeOutQuart: "cubic-bezier(.165,.84,.44,1)", easeInOutQuart: "cubic-bezier(.77,0,.175,1)", easeInQuint: "cubic-bezier(.755,.05,.855,.06)", easeOutQuint: "cubic-bezier(.23,1,.32,1)", easeInOutQuint: "cubic-bezier(.86,0,.07,1)", easeInSine: "cubic-bezier(.47,0,.745,.715)", easeOutSine: "cubic-bezier(.39,.575,.565,1)", easeInOutSine: "cubic-bezier(.445,.05,.55,.95)", easeInBack: "cubic-bezier(.6,-.28,.735,.045)", easeOutBack: "cubic-bezier(.175, .885,.32,1.275)", easeInOutBack: "cubic-bezier(.68,-.55,.265,1.55)" }; k.cssHooks["transit:transform"] = { get: function (r) { return k(r).data("transform") || new j() }, set: function (s, r) { var t = r; if (!(t instanceof j)) { t = new j(t) } if (q.transform === "WebkitTransform" && !a) { s.style[q.transform] = t.toString(true) } else { s.style[q.transform] = t.toString() } k(s).data("transform", t) } }; k.cssHooks.transform = { set: k.cssHooks["transit:transform"].set }; if (k.fn.jquery < "1.8") { k.cssHooks.transformOrigin = { get: function (r) { return r.style[q.transformOrigin] }, set: function (r, s) { r.style[q.transformOrigin] = s } }; k.cssHooks.transition = { get: function (r) { return r.style[q.transition] }, set: function (r, s) { r.style[q.transition] = s } } } n("scale"); n("translate"); n("rotate"); n("rotateX"); n("rotateY"); n("rotate3d"); n("perspective"); n("skewX"); n("skewY"); n("x", true); n("y", true); function j(r) { if (typeof r === "string") { this.parse(r) } return this } j.prototype = { setFromString: function (t, s) { var r = (typeof s === "string") ? s.split(",") : (s.constructor === Array) ? s : [s]; r.unshift(t); j.prototype.set.apply(this, r) }, set: function (s) { var r = Array.prototype.slice.apply(arguments, [1]); if (this.setter[s]) { this.setter[s].apply(this, r) } else { this[s] = r.join(",") } }, get: function (r) { if (this.getter[r]) { return this.getter[r].apply(this) } else { return this[r] || 0 } }, setter: { rotate: function (r) { this.rotate = o(r, "deg") }, rotateX: function (r) { this.rotateX = o(r, "deg") }, rotateY: function (r) { this.rotateY = o(r, "deg") }, scale: function (r, s) { if (s === undefined) { s = r } this.scale = r + "," + s }, skewX: function (r) { this.skewX = o(r, "deg") }, skewY: function (r) { this.skewY = o(r, "deg") }, perspective: function (r) { this.perspective = o(r, "px") }, x: function (r) { this.set("translate", r, null) }, y: function (r) { this.set("translate", null, r) }, translate: function (r, s) { if (this._translateX === undefined) { this._translateX = 0 } if (this._translateY === undefined) { this._translateY = 0 } if (r !== null && r !== undefined) { this._translateX = o(r, "px") } if (s !== null && s !== undefined) { this._translateY = o(s, "px") } this.translate = this._translateX + "," + this._translateY } }, getter: { x: function () { return this._translateX || 0 }, y: function () { return this._translateY || 0 }, scale: function () { var r = (this.scale || "1,1").split(","); if (r[0]) { r[0] = parseFloat(r[0]) } if (r[1]) { r[1] = parseFloat(r[1]) } return (r[0] === r[1]) ? r[0] : r }, rotate3d: function () { var t = (this.rotate3d || "0,0,0,0deg").split(","); for (var r = 0; r <= 3; ++r) { if (t[r]) { t[r] = parseFloat(t[r]) } } if (t[3]) { t[3] = o(t[3], "deg") } return t } }, parse: function (s) { var r = this; s.replace(/([a-zA-Z0-9]+)\((.*?)\)/g, function (t, v, u) { r.setFromString(v, u) }) }, toString: function (t) { var s = []; for (var r in this) { if (this.hasOwnProperty(r)) { if ((!q.transform3d) && ((r === "rotateX") || (r === "rotateY") || (r === "perspective") || (r === "transformOrigin"))) { continue } if (r[0] !== "_") { if (t && (r === "scale")) { s.push(r + "3d(" + this[r] + ",1)") } else { if (t && (r === "translate")) { s.push(r + "3d(" + this[r] + ",0)") } else { s.push(r + "(" + this[r] + ")") } } } } } return s.join(" ") } }; function m(s, r, t) { if (r === true) { s.queue(t) } else { if (r) { s.queue(r, t) } else { t() } } } function h(s) { var r = []; k.each(s, function (t) { t = k.camelCase(t); t = k.transit.propertyMap[t] || k.cssProps[t] || t; t = c(t); if (k.inArray(t, r) === -1) { r.push(t) } }); return r } function g(s, v, x, r) { var t = h(s); if (k.cssEase[x]) { x = k.cssEase[x] } var w = "" + l(v) + " " + x; if (parseInt(r, 10) > 0) { w += " " + l(r) } var u = []; k.each(t, function (z, y) { u.push(y + " " + w) }); return u.join(", ") } k.fn.transition = k.fn.transit = function (z, s, y, C) { var D = this; var u = 0; var w = true; if (typeof s === "function") { C = s; s = undefined } if (typeof y === "function") { C = y; y = undefined } if (typeof z.easing !== "undefined") { y = z.easing; delete z.easing } if (typeof z.duration !== "undefined") { s = z.duration; delete z.duration } if (typeof z.complete !== "undefined") { C = z.complete; delete z.complete } if (typeof z.queue !== "undefined") { w = z.queue; delete z.queue } if (typeof z.delay !== "undefined") { u = z.delay; delete z.delay } if (typeof s === "undefined") { s = k.fx.speeds._default } if (typeof y === "undefined") { y = k.cssEase._default } s = l(s); var E = g(z, s, y, u); var B = k.transit.enabled && q.transition; var t = B ? (parseInt(s, 10) + parseInt(u, 10)) : 0; if (t === 0) { var A = function (F) { D.css(z); if (C) { C.apply(D) } if (F) { F() } }; m(D, w, A); return D } var x = {}; var r = function (H) { var G = false; var F = function () { if (G) { D.unbind(f, F) } if (t > 0) { D.each(function () { this.style[q.transition] = (x[this] || null) }) } if (typeof C === "function") { C.apply(D) } if (typeof H === "function") { H() } }; if ((t > 0) && (f) && (k.transit.useTransitionEnd)) { G = true; D.bind(f, F) } else { window.setTimeout(F, t) } D.each(function () { if (t > 0) { this.style[q.transition] = E } k(this).css(z) }) }; var v = function (F) { this.offsetWidth; r(F) }; m(D, w, v); return this }; function n(s, r) { if (!r) { k.cssNumber[s] = true } k.transit.propertyMap[s] = q.transform; k.cssHooks[s] = { get: function (v) { var u = k(v).css("transit:transform"); return u.get(s) }, set: function (v, w) { var u = k(v).css("transit:transform"); u.setFromString(s, w); k(v).css({ "transit:transform": u }) } } } function c(r) { return r.replace(/([A-Z])/g, function (s) { return "-" + s.toLowerCase() }) } function o(s, r) { if ((typeof s === "string") && (!s.match(/^[\-0-9\.]+$/))) { return s } else { return "" + s + r } } function l(s) { var r = s; if (k.fx.speeds[r]) { r = k.fx.speeds[r] } return o(r, "ms") } k.transit.getTransitionValue = g })(jQuery);
