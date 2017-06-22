/// <reference path="jquery-vsdoc.js" />
/// <reference path="utils.js" />
/// <reference path="lw.js" />
/// <reference path="jquery.form.js" />

/**
* Version: 1.0.0.6 
* Build Date: January 20 2015
* Copyright (c) 2006-2015
* License: Licensed under The MIT License.
*/
var lw = this.lw ? this.lw : {};

lw.validator = {
	version: "1.0.0.3",
	createDate: "May 01 2012",
	modifiedDate: "Jan 26 2013",
	groups: {},
	resources: {},
	fields: {},
	init: function () {

		var _ = lw.validator;

		for (i in _.groups) {
			var group = _.groups[i];

			var form = $("#" + group.Form);
			var d = form.data("groups");

			if (!defined(d)) {
				d = {};
			}
			d[group.id] = group;

			form.data("groups", d);

			form.data("_validate", true);

			var form = $("#" + group["Form"]);
			if (!form.attr("ajax"))
				form.bind("submit", _.validateForm);

			group.fields = [];
		}

		$(".lw-validate").each(function () {
			_.initField($(this));
		});
	},
	disableFutureSubmittion: function (form) {
		form.data("submitted", true);
		form.find("input[type=submit]").addClass("submitted");
		form.find("button[type=submit]").addClass("submitted");
	},
	enableFutureSubmittion: function (form) {
		form.data("submitted", null);
		form.find("input[type=submit]").removeClass("submitted");
		form.find("button[type=submit]").removeClass("submitted");
	},
	validateForm: function (e, form, group) {
		var _ = lw.validator;
		var form = form ? form : $(this);

		form.data("validated", null);

		/*
		if (isOk(form.data("submitted")) && form.data("submitted") == true)
			return false;
		else {
			_.disableFutureSubmittion(form);
		}
		*/
		try {
			var cKeditors = form.find("textarea.ckeditor");
			cKeditors.each(function () {
				this.value = CKEDITOR.instances[this.id].getSnapshot();
			});
		}
		catch (e) { }


		var groups = form.data("groups");

		var summary = [_.resources.PleaseFix];
		var validated = true;

		for (i in groups) {
			if (isOk(group)) {
				var temp = i.split(":");
				if(temp[temp.length - 1] != group)
					continue;
			}

			var group = groups[i];
			var fields = group.fields;

			for (var j in fields) {
				var temp = _.validate(fields[j]);

				_.downlightField(fields[j]);

				if (!temp.validated) {
					summary.push(temp.message);
					validated = false;
					_.hilightField(fields[j], temp.message, false);
				}
			}

			var html = _.getHtmlMessage(summary);

			if (!validated && group["SummaryMessageClientID"]) {
				$("#" + group["SummaryMessageClientID"]).html(html);
			}

			if (!validated && group["AlertErrorSummary"]) {
				lw.alert(html, lw.siteName, "OK", "validation-error", function () {
					_.enableFutureSubmittion(form);
				});
			}
		}

		form.data("validated", validated);

		return validated;
	},
	validateGroup: function (e, form, groupId) {
		var _ = lw.validator;
		var form = form ? form : $(this);

		form.data("validated", null);

		try {
			var cKeditors = form.find("textarea.ckeditor");
			cKeditors.each(function () {
				this.value = CKEDITOR.instances[this.id].getSnapshot();
			});
		}
		catch (e) { }

		var groups = form.data("groups");
		var summary = new Array();
		summary[0] = [_.resources.PleaseFix];
		var validated = true;

		var gV = _.groupValidation(groups[groupId], summary, validated, form);

		validated = gV[0];
		var message = gV[1];

		form.data("validated", validated);
		form.data("message", message);

		return validated;
	},
	groupValidation: function (group, summary, validated, form) {
		var _ = lw.validator;
		var fields = group.fields;

		$.each(fields, function (j) {
			var temp = _.validate(fields[j]);

			_.downlightField(fields[j]);

			if (!temp.validated) {
				summary.push(temp.message);
				validated = false;
				_.hilightField(fields[j], temp.message, false);
			}
		});

		var html = _.getHtmlMessage(summary);

		if (!validated && group["SummaryMessageID"]) {
			$("#" + group["SummaryMessageID"]).html(html);
		}
		if (!validated && group["AlertErrorSummary"]) {
			lw.alert(html, lw.siteName, "OK", "validation-error", function () {
				_.enableFutureSubmittion(form);
			});
		}
		return [validated, html];
	},
	getHtmlMessage: function (summary) {
		return "<h6>" + summary.shift() + "</h6><ul><li>" + summary.join("</li><li>") + "</li></ul>";
	},
	checkEmptyField: function (field) {
		var _ = lw.validator;
		var data = field.data("d");

		var val = field.val();
		if (isOk(val)) {
			return;
		}
		if (($.browser.msie) && ($.browser.version < 10)) {
			switch (field[0].type) {
				case "text":
				case "textarea":
					if (data["EmptyText"]) {
						field.val(data["EmptyText"]);
						field.addClass(data["EmptyClass"]);
					}
					break;
				case "password":
					var input;
					if (field.data("maskinput"))
						input = field.data("maskinput");
					else {
						input = $("<input type=text />");
						input.addClass(field[0].className);
						field.data("maskinput", input);
						input.val(data["EmptyText"]);
						input[0].className = field[0].className;
						input.addClass(data["EmptyClass"]);
						input.css("position", "absolute");
						input.bind("focus click mousedown keydown", function () {
							input.css("display", "none");
							setTimeout(function () { field[0].focus(); }, 50);
						});
						$(field[0].parentNode).append(input);
					}
					input.css({
						display: "block",
						width: field.width(),
						height: field.height(),
						top: field.position().top,
						left: field.position().left
					});
					break;
				default:
					break;
			}
		} else {
			switch (field[0].type) {
				case "text":
				case "textarea":
				case "password":
					if (data["EmptyText"]) {
						field.addClass(data["EmptyClass"]);
					}
					break;
				default:
					break;
			}
		}

	},
	initField: function (field) {
		var _ = lw.validator;
		var data = {};
		var attr = field.attr("data-lw-validate");
		if (!isOk(attr))
			attr = field.attr("validate");
		if (!isOk(attr))
			return;
		var arr = attr.split("&");

		for (var i = 0; i < arr.length; i++) {
			if (arr[i].Trim() != "") {
				var temp = arr[i].Trim().split("=");
				if (temp.length == 2) {
					data[temp[0].Trim()] = temp[1].Trim();
				}
			}
		}

		field.data("d", data);

		_.checkEmptyField(field);

		field.bind("focus click mousedown keydown", function () {
			field.removeClass(data["EmptyClass"]);
			var input = field.data("maskinput");
			if (isOk(input)) {
				input.css("display", "none");
			}
		});

		field.bind("change blur", function () {
			var ret = lw.validator.validate(field);

			if (!ret.validated) {
				_.hilightField(field, ret.message, true);
			}
			else {
				_.downlightField(field);
			}
		});
		field.bind("blur", function () {
			_.checkEmptyField(field);
		});

		_.fields[field.attr("id")] = field;


		if (_.groups[data["Group"]])
			_.groups[data["Group"]].fields.push(field);
	},
	hilightField: function (field, message, bubble) {
		var _ = lw.validator;
		field.addClass("lw-not-validated");
		if (_.groups[field.data("d")["Group"]] && _.groups[field.data("d")["Group"]].Bubble) {
			field.addClass("lw-bubble");

			if (bubble)
				lw.showTooltip(field, "red", message, 1500);
			else
				lw.showTooltip(field, "red", message, 0, null, true);


			if (field.data("maskinput")) {
				field.data("maskinput").css("display", "none");
			}
		}
		field.data("title", field.attr("title"));
		//field.attr("title", message);
	},
	downlightField: function (field, message, bubble) {
		var _ = lw.validator;
		field.removeClass("lw-not-validated");
		field.removeClass("lw-bubble");
		field.attr("title", field.data("title"));
		lw.deleteTooltip(field);
	},
	validate: function (field, data) {
		var _ = lw.validator;
		var data = data ? data : field.data("d");

		var value = field.val().trim();

		if (isOk(data["ValidateWith"])) {
			var validateWith = $(field[0].form).find("#" + data["ValidateWith"]);
			var compareVal = validateWith.val();
			if (validateWith[0].type == "checkbox" || validateWith[0].type == "radio") {
				compareVal = validateWith[0].checked ? compareVal : null;
			}
			var validateWithValue = data["ValidateWithValue"];

			if (!data["ValidateWithCondition"])
			    data["ValidateWithCondition"] = "=";

			switch (data["ValidateWithCondition"]) {
				case ".":
					if (!isOk(compareVal))
						return { validated: true };
					break;
				case "<>":
					if (compareVal == validateWithValue)
						return { validated: true };
					break;
				default:
					if (compare(compareVal, validateWithValue, data["DataType"]) != data["ValidateWithCondition"])
						return { validated: true };
					break;
			}
		}


		if (isOk(data.Required) && data.Required.toLowerCase() == "true") {
			var validated = false;
			switch (data["DataType"]) {
				case "CheckBox":
					validated = field[0].checked;
					break;
				case "RadioList":
					var name = field.attr("name");
					var radios = $("input[name='" + name + "']");
					radios.each(function () {
						if (this.checked) {
							validated = true;
							return false;
						}
					});
					break;
				default:
					validated = value.length > 0;
					break;
			}
			if (!validated)
				return new _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources.Required.Format(data.DisplayName));
		}

		if (isOk(value) || isOk(data["Min"]) || isOk(data["Max"])) {
			switch (data["DataType"]) {
				case "Number":
				case "Decimal":
				case "Integer":
					var pat = /^-?\d*(\.\d+)?$/;
					if (pat.test(value)) {
						if ((isOk(data["Min"]) && parseFloat(value) < parseFloat(data["Min"])) || (isOk(data["Max"]) && parseFloat(value) > parseFloat(data["Max"]))) {
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_Number"].Format(data.DisplayName, data["Min"], data["Max"]));
						}
					}
					else {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources[data["DataType"]].Format(data.DisplayName));
					}
					break;
				case "Integer":
					var pat = /^\d*$/;
					if (pat.test(value)) {
						if ((isOk(data["Min"]) && parseFloat(value) < parseFloat(data["Min"])) || (isOk(data["Max"]) && parseFloat(value) > parseFloat(data["Max"]))) {
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_Number"].Format(data.DisplayName, data["Min"], data["Max"]));
						}
					}
					else {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources[data["DataType"]].Format(data.DisplayName));
					}
					break;
				case "Date":
					var dateValue = Date.parse(value);
					if (!isNaN(dateValue)) {
						if (isOk(data["Min"]) && dateValue < Date.parse(data["Min"])) {
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_Date"].Format(data.DisplayName, data["Min"], data["Max"]));
						}
						if (isOk(data["Max"]) && dateValue > Date.parse(data["Max"])) {
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_Date"].Format(data.DisplayName, data["Min"], data["Max"]));
						}
					} else {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources[data["DataType"]].Format(data.DisplayName));
					}
					break;
				case "Email":
					var pat = /^[a-z]([\.\-\_]{0,}([a-z_0-9]|[a-z_0-9]\.[a-z_0-9])*)+@([a-z0-9]|[a-z0-9][\.\-][a-z0-9])+\.[a-z]{2,}$/i;
					if (!pat.test(value)) {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources[data["DataType"]].Format(data.DisplayName));
					}
					break;
				case "Image":
					var pat = /\.((jpg)|(jpeg)|(bmp)|(gif)|(png))$/i;
					if (!pat.test(value)) {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources[data["DataType"]].Format(data.DisplayName));
					}
					break;
				case "String":
					if (isOk(data["Min"]) && value.length < parseFloat(data["Min"])) {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_String"].Format(data.DisplayName, data["Min"], data["Max"]));
					}
					if (isOk(data["Max"]) && value.length > parseFloat(data["Max"])) {
						return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Range_String"].Format(data.DisplayName, data["Min"], data["Max"]));
					}
					break;
				default:
					break;
			}

			if (isOk(data["CompareTo"])) {
				var compareTo = $(field[0].form).find("#" + data["CompareTo"]);
				var compareVal = compareTo.val();
				switch (data["CompareCondition"]) {
					case ">":
						if (compare(value, compareVal, data["DataType"]) != ">")
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Compare_GreaterThan"].Format(data.DisplayName, data.CompareToDisplayName));
						break;
					case "<":
						if (compare(value, compareVal, data["DataType"]) != "<")
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Compare_LessThan"].Format(data.DisplayName, data.CompareToDisplayName));
						break;
					default:
						if (compare(value, compareVal, data["DataType"]) != "=")
							return _.validateMessage(false, data.CustomMessage ? data.CustomMessage : _.resources["Compare_Equal"].Format(data.DisplayName, data.CompareToDisplayName));
						break;
				}
			}
		}
		return { validated: true };
	},
	validateMessage: function (validated, message) {
		return { validated: validated, message: message };
	}
};
lw.ajaxForms = {
	selector: "form[ajax=true]",
	forms: {},
	init: function () {

		var _ = lw.ajaxForms;
		_.forms = $(_.selector);

		_.forms.each(function () {
			var $this = $(this);
			if ($this.data("ajaxInit"))
				return;
			$this.data("ajaxInit", true);
			$this.bind("submit", _.submit);
		});
	},
	submit: function (e) {
		var form = $(this);
		if (form.data("alreadysubmitted")) {
			e.stopPropagation();
			return false;
		}
		form.data("alreadysubmitted", true);

		//if (form.data("_validate") == true)
		lw.validator.validateForm(e, form);

		var validated = form.data("validated");

		if (isOk(validated)) {
			if (validated == false) {
				form.data("alreadysubmitted", false);
				return false;
			}
		}
		lw.loader(form);

		var ajaxForm;

		function abort(e) {
			if (e.keyCode == 27) {
				ajaxForm.abort();
				$(document.body).unbind("keyup", abort);
			}
		}



		//

		//alert(form.attr("AjaxCallback"));

		var callBack = form.attr("AjaxCallback");
		var dataType = "json" || form.attr("DataType");

		ajaxForm = form.ajaxSubmit({
			data: { ajax: true },
			cache: false,
			dataType: dataType,
			success: function (e) {
				lw.hideLoader(form);

				var ajaxCallBack = e.callBack && typeof [e.callBack] == "function" ? e.callBack : callBack;

				if (isOk(ajaxCallBack) && typeof window[ajaxCallBack] == "function") {
					window[ajaxCallBack](e);
				}
			},
			error: function (e) {

				lw.hideLoader(form);

				if (e.status == 0)
					return;

				lw.alert(e.responseText, lw.siteName, "OK", "validation-error");
			},
			complete: function (e) {
				lw.hideLoader(form);
				$(document.body).unbind("keyup", abort);
				form.data("alreadysubmitted", null);

				if (dataType != "json") {

					var ajaxCallBack = e.callBack && typeof [e.callBack] == "function" ? e.callBack : callBack;

					if (isOk(ajaxCallBack) && typeof window[ajaxCallBack] == "function") {
						window[ajaxCallBack](e);
					}
				}
			}
		});
		$(document.body).bind("keyup", abort);
		return false;
	}
};

