/// <reference path="../../../js/jquery-vsdoc.js" />
/// <reference path="../../../js/jquery-plugins.js" />
/// <reference path="../../../js/lw-jquery-plugins.js" />
/// <reference path="../../../js/utils.js" />
/// <reference path="../../../js/lw.js" />
/// <reference path="manager.js" />

//Note that that document.tabInfo can be used to access tab variables, section, xml...

//reference to the global lw object
var Manager = top.lw.manager;
var hilightColor = "#fff4c0";

/*!
 * jquery.customSelect() - v0.4.1
 * http://adam.co/lab/jquery/customselect/
 * 2013-05-13
 *
 * Copyright 2013 Adam Coulombe
 * @license http://www.opensource.org/licenses/mit-license.html MIT License
 * @license http://www.gnu.org/licenses/gpl.html GPL2 License
 */
/*!
 * jquery.customSelect() - v0.4.1
 * http://adam.co/lab/jquery/customselect/
 * 2013-05-13
 *
 * Copyright 2013 Adam Coulombe
 * @license http://www.opensource.org/licenses/mit-license.html MIT License
 * @license http://www.gnu.org/licenses/gpl.html GPL2 License 
 */

(function ($) {
	'use strict';

	$.fn.extend({
		customSelect: function (options) {
			// filter out <= IE6
			if (typeof document.body.style.maxHeight === 'undefined') {
				return this;
			}
			var defaults = {
				customClass: 'customSelect',
				mapClass: true,
				mapStyle: true
			},
			options = $.extend(defaults, options),
			prefix = options.customClass,
			changed = function ($select, customSelectSpan) {
				var currentSelected = $select.find(':selected'),
				customSelectSpanInner = customSelectSpan.children(':first'),
				html = currentSelected.html() || '&nbsp;';

				// Start of Code added by Joe Chahoud
				if (html.indexOf('®') >= 0) {
					html = html.replace('®', '<sup>®</sup>');
				}
				// End of Code added by Joe Chahoud
				customSelectSpanInner.html(html);

				if (currentSelected.attr('disabled')) {
					customSelectSpan.addClass(getClass('DisabledOption'));
				} else {
					customSelectSpan.removeClass(getClass('DisabledOption'));
				}

				setTimeout(function () {
					customSelectSpan.removeClass(getClass('Open'));
					customSelectSpan.addClass(getClass('Changed'));
					$(document).off('mouseup.' + getClass('Open'));
				}, 60);
			},
			getClass = function (suffix) {
				return prefix + suffix;
			};

			return this.each(function () {
				var $select = $(this),
					customSelectInnerSpan = $('<span />').addClass(getClass('Inner')),
					customSelectSpan = $('<span />');

				$select.after(customSelectSpan.append(customSelectInnerSpan));

				customSelectSpan.addClass(prefix);

				if (options.mapClass) {
					customSelectSpan.addClass($select.attr('class'));
				}
				if (options.mapStyle) {
					customSelectSpan.attr('style', $select.attr('style'));
				}

				$select
					.addClass('hasCustomSelect')
					.on('update', function () {
						changed($select, customSelectSpan);

						var selectBoxWidth = parseInt($select.outerWidth(), 10) -
								(parseInt(customSelectSpan.outerWidth(), 10) -
									parseInt(customSelectSpan.width(), 10));

						// Set to inline-block before calculating outerHeight
						customSelectSpan.css({
							display: 'inline-block'
						});

						var selectBoxHeight = customSelectSpan.outerHeight();

						if ($select.attr('disabled')) {
							customSelectSpan.addClass(getClass('Disabled'));
						} else {
							customSelectSpan.removeClass(getClass('Disabled'));
						}

						customSelectInnerSpan.css({
							width: selectBoxWidth,
							display: 'inline-block'
						});

						$select.css({
							'-webkit-appearance': 'menulist-button',
							width: customSelectSpan.outerWidth(),
							position: 'absolute',
							opacity: 0,
							height: selectBoxHeight,
							fontSize: customSelectSpan.css('font-size')
						});
					})
					.on('change', function () {
						customSelectSpan.addClass(getClass('Changed'));
						changed($select, customSelectSpan);
					})
					.on('keyup', function (e) {
						if (!customSelectSpan.hasClass(getClass('Open'))) {
							$select.blur();
							$select.focus();
						} else {
							if (e.which == 13 || e.which == 27) {
								changed($select, customSelectSpan);
							}
						}
					})
					.on('mousedown', function (e) {
						customSelectSpan.removeClass(getClass('Changed'));
					})
					.on('mouseup', function (e) {

						if (!customSelectSpan.hasClass(getClass('Open'))) {
							// if FF and there are other selects open, just apply focus
							if ($('.' + getClass('Open')).not(customSelectSpan).length > 0 && typeof InstallTrigger !== 'undefined') {
								$select.focus();
							} else {
								customSelectSpan.addClass(getClass('Open'));
								e.stopPropagation();
								$(document).one('mouseup.' + getClass('Open'), function (e) {
									if (e.target != $select.get(0) && $.inArray(e.target, $select.find('*').get()) < 0) {
										$select.blur();
									} else {
										changed($select, customSelectSpan);
									}
								});
							}
						}
					})
					.focus(function () {
						customSelectSpan.removeClass(getClass('Changed')).addClass(getClass('Focus'));
					})
					.blur(function () {
						customSelectSpan.removeClass(getClass('Focus') + ' ' + getClass('Open'));
					})
					.hover(function () {
						customSelectSpan.addClass(getClass('Hover'));
					}, function () {
						customSelectSpan.removeClass(getClass('Hover'));
					})
					.trigger('update');
			});
		}
	});
})(jQuery);



function _resize() {

	return;
	$('article').css({
		width: $(document.body).width(),
		height: $(document.body).height() - $("header").height() - 27
	});
	$('article').lwScroll({
		height: $(document.body).height() - $("header").height() - 27,
		width: $(document.body).width(),
		alwaysVisible: false
	});
}
function _initInside() {
	var field = $("input[name=q]");
	if (field.hasClass("no-animation"))
		return;

	field.on("focus", function () {
		var el = $(this);
		if (!isOk(el.data("o-width")))
			el.data("o-width", el.width());
		var width = el.data("o-width");
		el.stop().animate({ width: parseInt(width) * 2 });
	});
	field.on("blur", function () {
		var el = $(this);
		var _val = el.val().Trim();
		if (_val == "" || _val == el.data("d")["EmptyText"])
			el.stop().animate({ width: el.data("o-width") });
	});
}

function createTab(title, url, fullUrl) {
	var tabInfo = document.tabInfo;


	Manager.createTab({
		tabName: title,
		tabText: title,
		section: tabInfo.section,
		sub: tabInfo.sub,
		path: (typeof (fullUrl) == "undefined") ? tabInfo.url + "/" + url : url,
		currentNode: tabInfo.currentNode
	});
}

function _relativeTimes() {
	$(".date-relative").each(function () {
		var el = $(this);
		var data = el.html();
		if (el.data("has-relative-date")) {
			return;
		}
		data = lw.utils.relative_time(Date.parse(data));
		el.html(data);
		el.data("has-relative-date", true);
	});
}

function _initCheckBoxes() {
	$("td:first-child input[type=checkbox]").on("click", function (e) {
		var el = $(this);
		var tr = el.closest("tr");

		

		if (!isOk(tr.data("old-bg"))) {
			var background = tr.css("background-color");
			if (!isOk(background)) {
				tr.css("background-color", "#ffffff");
			}
			tr.data("old-bg", tr.css("background-color"));
		}

		var _checked = this.checked;

		if (this.checked) {
			tr.stop().animate({ "background-color": hilightColor }, 600);
		}
		else {
			tr.stop().animate({ "background-color": tr.data("old-bg") }, 600);
		}

		if (e.shiftKey) {
			var len = tr.parent().children("tr").length;
			var checkboxesArray = [];

			var next = tr.next();
			var found = false;
			while (next.length > 0) {
				checkboxesArray.push($(next.children("td")[0]).find("input")[0]);
				if (checkboxesArray[checkboxesArray.length - 1].id == lw.lastManagerCheckbox.id) {
					found = true;
					break;
				}
				next = next.next();
			}
			if (!found) {
				checkboxesArray = [];
				next = tr.prev();
				while (next.length > 0) {
					checkboxesArray.push($(next.children("td")[0]).find("input")[0]);
					if (checkboxesArray[checkboxesArray.length - 1].id == lw.lastManagerCheckbox.id) {
						found = true;
						break;
					}
					next = next.prev();
				}
			}

			$.each(checkboxesArray, function () {
				if (_checked != this.checked) {
					$(this).trigger("click");
				}
			});
		}

		lw.lastManagerCheckbox = this;
	});
}

function SearchCallback(resp) {
	if (resp.success) {
		$("article").html(resp.data);
		_relativeTimes();
		_initCheckBoxes();
		//$(".custom-checkbox").iButton();
	}
	if (resp.error) {
		alert("An error has occured.\r\b" + resp.message);
	}
}

lw.AppendInit(function () {
	$(window).on("resize", _resize);
	setTimeout(_resize, 500);

	_relativeTimes();

	_initCheckBoxes();

	_initInside();

	//$("select").combobox();

	$(".date-field").datepicker();
	if ($.fn.timeEntry)
		$('.time-field').timeEntry({ show24Hours: true });

	//$(".custom-checkbox").iButton();

	$(".select-all input[type=checkbox]").on("change", function () {
		$("td:first-child input[type=checkbox]").attr("checked", this.checked);
		$("td:first-child input[type=checkbox]").trigger("change");
	});


	lw.openPanel = null;
	lw.panelOriginalColor = null;

	$('.panels h2').live('click', function () {

		//closing other open panels
		if (lw.openPanel) {
			$(lw.openPanel).removeClass('open-panel');
			$(lw.openPanel).parent().animate({ height: $(this).height() }, 600);
			$(lw.openPanel).next().animate({ height: 0 }, 600);
			$(lw.openPanel).animate({ color: $(lw.openPanel).data("color"), backgroundColor: $(lw.openPanel).data("bgcolor") }, 600);
		}

		//if this was an open panel, it was already closed above but we do not want to 
		//open it again
		if (lw.openPanel == this) {
			lw.openPanel = null;
			return;
		}

		lw.openPanel = this;


		$(this).parent().addClass('open-list');
		$(this).addClass('open-panel');

		var el = $(this).next();
		if (!el.data("heightTo")) {
			el.data("heightTo", el.height());
			$(this).data("bgcolor", "#ebebeb"); //was taking the hover color $(this).css("background-color"));
			$(this).data("color", $(this).css("color"));
		}

		$(this).animate({ backgroundColor: "#5f5f5f", color: "#ffffff" }, 600);

		el.css({ height: 0, display: "block", overflow: "hidden" });
		el.animate({ height: el.data("heightTo") }, 600);
		$(this).parent().animate({ height: $(this).height() + el.data("heightTo") + 20 }, 600);
	});

	if ($('.panels .open-panel').length > 0) {
		$('.open-panel').trigger('click');
	};
});

function GetSelectedItems(validate) {
	var selector = "td:first-child input:checked";
	var sel = $(selector);
	if (validate) {
		if (sel.length == 0) {
			lw.alert("Please select one or more item by clicking on the checkbox to the left.");
			return [];
		}
	}
	return sel;
}

function PreviewInEdit() {
	var id = lw.utils.request("Id");
	if (!isOk(id))
		id = lw.utils.request("id");
	_preview(id);
}

function _preview(id)                           {
	window.open("/preview.aspx?Id=" + id + "&type=" + previewType, "_blank");
}
function Preview() {
	var id = arguments[0];
	if (!isNaN(parseInt(id))) {
		var val = $(event.srcElement || event.target).closest("tr").find("h4").html();
		_preview(id);
	} else {
		var sel = GetSelectedItems(true);
		if (sel.length > 1) {
			lw.yesNo("You are previewing more than one item!<BR />This might slow down your browser or connection.", function () {

				sel.each(function () {
					var val = $(this).closest("tr").find("h4").html();
					_preview(this.value);
				});

			}, null, "Ok, Continue", "Cancel");
		} else {
			if (sel.length > 0) {
				sel.each(function () {
					var val = $(this).closest("tr").find("h4").html();
					_preview(this.value);
				});
			}
		}
	}
	return sel;
}


function EditItems(fullUrl) {
	if (typeof (fullUrl.length) != "undefined") {
		GoTo(arguments[0], "Edit.aspx", fullUrl.toString());
	}
	else {
		GoTo(arguments[0], "Edit.aspx", null, "Edit");
	}
}

function GoTo(id, url, fullUrl, title, paramName) {
	url = url + (url.indexOf("?") >= 0 ? "&" : "?");
	paramName = paramName ? paramName : "Id";

	if (isOk(id) && !isNaN(parseInt(id))) {
		var val = $(event.srcElement || event.target).closest("tr").find("h4").html();
		(fullUrl == null) ? createTab(title + " " + val, url + paramName + "=" + id) : createTab("Edit " + val, url + paramName + "=" + id, fullUrl);
	} else {
		var sel = GetSelectedItems(true);
		if (sel.length > 1) {
			lw.yesNo("You are opening more than one item!<BR />This might slow down your browser or connection.", function () {

				sel.each(function () {
					var val = $(this).closest("tr").find("h4").html();
					(fullUrl == null) ? createTab(title + " " + val, url + paramName + "=" + this.value) : createTab(title + " " + val, url + paramName + "=" + this.value, fullUrl);
				});

			}, null, "Ok, Continue", "Cancel");
		} else {
			if (sel.length > 0) {
				sel.each(function () {
					var val = $(this).closest("tr").find("h4").html();
					(fullUrl == null) ? createTab(title + " " + val, url + paramName + "=" + this.value) : createTab(title + " " + val, url + paramName + "=" + this.value, fullUrl);
				});
			}
		}
	}
	return sel;
}


function DeleteItems(fullUrl) {
	if (typeof (fullUrl.length) != "undefined") {
		url = fullUrl + "action/delete.ashx";
	}
	else {
		url = "action/delete.ashx";
	}
	function _delete(ids, trs) {
		lw.loader($(document.body), "Please wait, deleting items...");
		$.ajax({
			url: url,
			context: trs,
			data: { ids: ids.join(",") },
			dataType: "json",
			cache: false,
			success: function (ret) {
				lw.hideLoader($(document.body));

				$.each(trs, function () {
					$(this).remove();
				});
			},
			error: function (e) {
				alert("An error has occured.\r\n" + e.description);
			}
		});
	}

	var id = arguments[0];
	if (!isNaN(parseInt(id))) {
		var tr = $(event.srcElement || event.target).closest("tr");
		var val = tr.find("h4").html();
		if (!isOk(val)) {
			val = tr.children("td")[1].innerHTML;
		}
		lw.yesNo("Delete {0}?<BR><BR><i>Please note that all related items, networks will be modified accordingly...</i>".Format(val), function () {
			_delete([id], [tr]);
		}, null, "Ok, Delete", "No");
	} else {
		var sel = GetSelectedItems(true);
		if (sel.length > 0) {
			var ids = [];
			var trs = [];
			var titles = [];

			sel.each(function () {
				var tr = $(this).closest("tr");
				titles.push(tr.find("h4").html());
				trs.push(tr);
				ids.push(this.value);
			});

			lw.yesNo("You are sure you want to delete the following items: <BR><BR>{0}<BR><BR><i>Please note that all related items, networks will be modified accordingly...</i>".Format(titles.join(", ")),
				function () {
					_delete(ids, trs);
				}, null, "Ok, Delete", "Cancel");
		}
	}
}

lw.Delay(function () {
	lw.siteName = "CMS";
	try {
		if (this.buttons && typeof this.buttons == "object") {
			Manager.createButtons(document.tabInfo.section, buttons, document.tabInfo.tabName);
		}
	}
	catch (e) {

	}
}, 1);


//init buttons that link to files.
lw.AppendInit(function () {
	var buttons = $(".edit-file");
	buttons.each(function () {
		var el = $(this);
		var _for = $("#" + el.attr("for"));
		if (_for.length > 0 && _for.attr("type") == "file") {

			_for.live("change", function () {
				var dataType = _for.attr("data-type");
				switch (dataType) {
					case "image":
						var _forImage = $("#" + _for.attr("for"));
						$(_for[0].form).submit();
						break;
					default:
						break;
				}
			});
		}
	});
});