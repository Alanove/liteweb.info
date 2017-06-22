/// <reference path="../../../js/jquery-vsdoc.js" />
/// <reference path="../../../js/utils.js" />
/// <reference path="../../../js/lw.js" />


function buttonsInit() {
	$("button").bind("mousedown", function () {
		$(this).addClass("mousedown");
	});
	$("button").bind("mouseup", function () {
		$(this).removeClass("mousedown");
	});
}

top.lw = lw;

lw.manager = {
	user: null,
	header: null,
	sprite: null,
	menu: null,
	main: null,
	activeArea: null,
	activeSection: null,
	sections: {},
	search: null,
	xml: null,
	query: null,
	menuDuration: 300,
	_an: 0,
	an: function(){
		return lw.manager._an++;
	},
	init: function (obj) {
		var _ = lw.manager;
		_.user = obj;
		_.header = $("header");
		_.xml = $($(_.user.xml)[1]);

		var h1 = $("<h2>Welcome {0}!</h2>".Format(obj.Name));
		h1.css("opacity", 0);


		_.header.find("form").fadeOut(300, function () {
			_.header.find("form").remove();
			_.header.find("aside").append(h1);
			h1.animate({ opacity: 1 }, 300);
		});

		_.header.animate({ height: 85 }, 300);
		_.header.find(".logo").animate({ height: 48, top: 5 }, 300);
		_.header.find(".logo img").animate({ width: 213 }, 300);
		//_.header.find(".logo").animate({ bottom: -20 }, 300);
		_.header.find(".logo img").attr("src", "images/logo-small.png");

		$(".logout").removeClass("hidden").css({ opacity: 0 }).delay(300).animate({ opacity: 1 }, 300);


		$(".welcome-note").fadeOut(300, function () { $(".welcome-note").remove() });
		$(".video").fadeOut(300, function () { $(".video").remove() });

		_.sprite = $("footer .sprite");
		_.sprite.removeClass("hidden").css({ bottom: -70 }).delay(0).animate({ bottom: 0 }, 300);

		_.menu = $(".content .menu");
		_.main = $(".content .main");
		_.activeArea = $(".content .active");


		_.main.removeClass("hidden");

		_.buildMenu();

		_.onResize();

		_.main.css({ width: GetViewWidth() - 20, height: GetViewHeight() - 110, opacity: 0 }).delay(300).animate({ opacity: 1 }, 300);

		$(window).bind("resize", _.onResize);

		_.initSearch();

		try{
			if (isOk(location.href.split("#")[1]))
				location.href = "#";
		}
		catch (e) {

		}

		$.history.init(function (hash) {
			var _ = lw.manager;
			hash = hash.replace(/-d$/, "");
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

			if (!_.query["section"]) {
				_.query["section"] = "PAGES";
			}
			lw.manager.initSection();
			lw.manager.initTab();
		});
	},
	initSearch: function () {
		try{
			var _ = lw.manager;
			_.search = $("section.search");
			_.search.css({ display: "none", opacity: 0 });
			_.search.animate({ opacity: 1 }, 300);

			_.search.field = $("section.search .f input[name=q]");
			_.search.button = $("section.search .f input[type=button]");
			_.search.advanced = $("section.search .f a");


			_.search.emptyText = "Type your search here...";

			_.search.field.val(_.search.emptyText);

			lw.utils.fieldMask(_.search.field, _.search.emptyText);
		}
		catch (E)
		{
		}
	},
	initSection: function () {
		var _ = lw.manager;
		var section = _.query["section"] || "Home";
		var _section = _.sections[section];

		if (_.activeSection == section) {
			if (_section["ul"])
				_section["ul"].children().removeClass("_active");
			_section["panel"].css("display", "block");
			_section["sprite"].css("display", "block");

			_.initSubSection();
			return;
		}
		_.hideActiveSection();
		_.activeSection = section;

		var li = _section["li"];

		var active = li.data("_active");
		var div = li.data("div");

		if (!active.data("child-bind")) {
			active.data("child-bind", true);

			var xml = $(li.data("node")).children();

			var ul = $("<ul />");

			xml.each(function (index, item) {
				if (["new", "edit", "delete"].indexOf(this.nodeName.toLowerCase()) >= 0) {
					return;
				}

				var li = $("<li />");

				var name = _.GetText(this);

				var a = $("<a />").html(name);
				li.data("node", this);
				a.attr("href", "#section=" + section + "&sub=" + this.nodeName);
				li.addClass(this.nodeName);
				li.append(a);

				if (_.hasPermission(this, "new") && false) {
					var newButton = $("<a class=new>+</a>");
					newButton.attr("href", "#section=" + section + "&action=new&sub=" + this.nodeName);
					li.append(newButton);
				}

				_section.subs[this.nodeName] = { li: li, name: name };

				ul.append(li);
			});

			if (ul.children().length > 0) {
				active.append(ul);
				li.data("children", ul);

				_section["ul"] = ul;
			}

		}
		var children = li.data("children");

		if (isOk(children)) {

			var w = li.width();
			li.css("width", w + 2);
			active.css({ "display": "block", opacity: 0 })
				.delay(_.menuDuration / 3)
				.animate({ opacity: 1, height: children.height() + 26 }, { duration: _.menuDuration, complete: function () { } });
			li.animate({ height: children.height() + 30 }, { duration: _.menuDuration, complete: function () { li.css("width", w); } });
			if (_section["ul"])
				_section["ul"].children().removeClass("_active");
		} else {
			active.css({ "display": "block", opacity: 0 }).
				animate({ opacity: 1 }, { duration: _.menuDuration });
		}
		div.delay(_.menuDuration / 3).animate({ opacity: 0 }, { duration: _.menuDuration, complete: function () { _.initSubSection(); } });

		if (!_section["panel"]) {
			_.createPanel(_section);
		}
		_section["panel"].css("display", "block");
		_section["sprite"].css("display", "block");
	},
	initSubSection: function () {
		var _ = lw.manager;
		var sub = _.query["sub"];
		var action = _.query["action"];

		var section = _.sections[_.activeSection];

		if (!isOk(section.subs[sub]))
			return;

		if (!isOk(section))
			return;

		if (section.oldSub && section.oldSub != sub) {
			section.ul.find("." + section.oldSub).removeClass("_active");
			section.ul.find("." + section.oldSub).find("a").removeClass("_active");
		}
		section.oldSub = sub;

		var subObj = section.subs[sub];


		if (isOk(sub)) {
			var li = section.ul.find("." + sub).addClass("_active");
			li.find("a").addClass("_active");
		}
	},
	createPanel: function (_section) {
		var _ = lw.manager;
		var panel = $("<div />");
		panel.addClass("panel").addClass(_section.nodeName);
		_.activeArea.append(panel);
		_section["panel"] = panel;
		var sprite = $("<div />");
		sprite.appendTo(_.sprite);
		_section["sprite"] = sprite;
	},
	initTab: function () {
		var _ = lw.manager;
		var tabName = _.query["tab"] ||
			(
				_.query["section"] +
				(isOk(_.query["sub"]) ? "-" + _.query["sub"] : "") +
				(isOk(_.query["action"]) ? "-" + _.query["action"] : "")
			);
		_.createTab(tabName, _.sections[_.query["section"]], _.query["sub"]);
	},
	createTab: function (tab, _section, sub) {
		var _ = lw.manager;

		var tabName, url, currentNode, path, tabText;
		if (typeof tab == "object") {
			tabName = tab.tabName;
			tabText = tab.tabText;
			_section = tab.section;
			sub = tab.sub;
			path = tab.path;
			currentNode = tab.currentNode;
		}
		else {
			tabName = tab;
		}

		tabName = lw.utils.toId(tabName)

		if (!_section["panel"]) {
			_.createPanel(_section);
		}
		if (!_section["tabs"]) {
			_section["tabs-panels"] = $("<section class=tabs/>").appendTo(_section["panel"]);
			_section["tabs"] = $("<ul />").appendTo(_section["tabs-panels"]);
			_section["tabsindex"] = {};
			_section["tabs-queue"] = [];
		}
		if (!isOk(_section["tabsindex"][tabName]) || _section["tabs"].find("li[name='" + tabName + "']").length == 0) {
			if (!isOk(path)) {
				path = "main>{0}".Format(_section.nodeName);
				if (isOk(sub))
					path = path + ">" + sub;
			}
			if (!isOk(currentNode)) {
				var currentNode = _.xml.find(path);
				if (isOk(currentNode.attr("justmenu"))) {
					currentNode = $(currentNode.children()[0]);
					sub = _.query["sub"] = currentNode[0].nodeName;
					_.initSubSection();
					_.initTab();
					return;
				}
			}

			var li = $("<li/>").attr("name", tabName);
			var span = $("<span />").appendTo(li);
			var close = $("<a class=close title=\"Close Tab\"/>").appendTo(span);
			close.attr("href", "#close=tab");
			close.click(_.closeTab);

			if (!isOk(tabText))
				tabText = _.GetText(currentNode);

			var a = $("<a class=tab/>").html(tabText).appendTo(li);
			a.attr("href", "#section={0}&sub={1}&tab={2}".Format(_section.nodeName, isOk(sub) ? sub : "", tabName));

			if (lw.tabTimeout)
				clearTimeout(lw.tabTimeout);

			lw.tabTimeout = setTimeout(function () {
				top.location.href = a.attr("href");
			}, 100);

			_section["tabs"].append(li);

			var div = $("<div />").addClass("tab-panel").appendTo(_section["tabs-panels"]);

			url = path.replace(/>/ig, "/");

			var iframe = $("<iframe />")
				.attr("src", url)
				.appendTo(div);

			iframe.bind("load", function () {
				this.contentDocument.tabInfo = {
					tabName: tabName,
					tabText: tabText,
					path: path,
					sub: sub,
					currentNode: currentNode,
					section: _section,
					url: url.split("#")[0].split("?")[0]
				};
			});

			li.data("div", div);

			li.data("iframe", iframe);

			_section["tabsindex"][tabName] = li;
		}
		_.activateTab(_section, tabName);
	},
	closeTab: function () {
		var _ = lw.manager;
		var section = _.sections[_.activeSection];

		var q = section["tabs-queue"];

		var tabName = $(this).parents("li").attr("name");
		var tab = section["tabsindex"][tabName];

		if (!isOk(tab))
			return;

		var content = tab.data("div");

		var activeTab = tab.hasClass("_active");

		if (content)
			content.remove();

		tab.remove();

		delete section["tabsindex"][tabName];

		if (q.length > 1 && activeTab) {
			var next = q[q.length - 2];
			location.href = section["tabsindex"][next].children("a").attr("href");
		}
		else
			_.onResize();

		var ar = [];
		$.each(q, function () {
			if (this != tabName) {
				ar.push(this);
			}
		});

		section["tabs-queue"] = ar;

		_.sprite.find("div#buttons_" + tabName).remove();

		return false;
	},
	activateTab: function (_section, tabName) {
		var _ = lw.manager;

		var ar = [];
		$.each(_section["tabs-queue"], function () {
			if (this != tabName) {
				ar.push(this);
				if (_section["tabsindex"][this]) {
					var li = _section["tabsindex"][this];
					li.removeClass("_active");
					li.data("div").css("display", "none");

					_.sprite.find("div#buttons_" + this).hide();
				}
			}
		});
		ar.push(tabName);
		_section["tabs-queue"] = ar;

		_section["tabsindex"][tabName].addClass("_active");
		_section["tabsindex"][tabName].data("div").css("display", "block");
		_.sprite.find("div#buttons_" + tabName).show();

		_.onResize();
	},
	hideActiveSection: function () {
		var _ = lw.manager;
		var oldSection = _.activeSection;
		if (!isOk(oldSection))
			return;
		var _section = _.sections[oldSection];
		var li = _section["li"];

		var active = li.data("_active");
		var div = li.data("div");

		li.delay(_.menuDuration / 3).animate({ height: 30 }, _.menuDuration);
		active.animate({ opacity: 0 }, _.menuDuration);
		div.animate({ opacity: 1 }, _.menuDuration);

		if (_section["panel"])
			_section["panel"].css("display", "none");
		if (_section["sprite"]) {
			_section["sprite"].css("display", "none");
		}
	},
	onResize: function () {
		var _ = lw.manager;
		var _height = GetViewHeight() - 70;
		_.main.css({ height: _height, width: GetViewWidth() - 20 });
		_.menu.css({ height: _height });
		_.activeArea.css({ height: _height, width: GetViewWidth() - _.menu.width() - 20 });

		var w = _.activeArea.width();
		var h = _.activeArea.height();

		var section = _.sections[_.activeSection];
		if (isOk(section) && isOk(section["tabs"])) {
			var tabs = section["tabs"].children("li");
			var left = 0;
			var it = 1;
			tabs.each(function (i) {
				var $this = $(this);
				$this.css({ "left": left, "position": "absolute", "z-index": 10 + i * it });
				left += $this.width() - 20;
				if ($this.hasClass("_active")) {
					$this.css("z-index", 1000);
					it = -1;
				}

				var iframe = $this.data("iframe");
				iframe.width(w - 7);
				iframe.height(h - _.sprite.height() - tabs.height() - 14);
			});
		}
	},
	buildMenu: function () {
		var _ = lw.manager;
		var ul = $("<ul>");

		//var li = _.createMenuButton("Zones and Screens", ul);
		//_.sections["Home"] = { li: li, subs: {}, name: "Zones and Screens", nodeName: "Home", icon: "screen" };

		var main = _.xml.find("main").children();

		main.each(function (index, item) {
			var li = _.createMenuButton(this, ul);
			_.sections[this.nodeName] = { li: li, subs: {}, name: _.GetText(this), nodeName: this.nodeName };
		});
		_.menu.append(ul);
		_.menu.delay(300).animate({ opacity: 1 }, 300);
	},
	createMenuButton: function (node, ul) {
		var _ = lw.manager;

		var text = _.GetText(node);
		var section = typeof node == "string" ? node : node.nodeName;

		var li = $("<li />");
		li.data("node", node);
		li.addClass(section);

		var div = $("<div />");

		var a = $("<a />").html(text);
		a.bind("click", lw.manager.menuButtonAction);

		a.attr("href", "#section=" + section);
		div.append(a);

		if (_.hasPermission(node, "new") && false) {
			var newButton = $("<a class=new>+</a>");
			newButton.attr("href", "#section=" + section + "&action=new");
			div.append(newButton);
		}

		var div1 = div.clone(true);
		div1.addClass("_active");

		li.append(div, div1);

		li.data("div", div);
		li.data("_active", div1);

		ul.append(li);

		return li;
	},
	hasPermission: function (node, action) {
		var selection = $(node).children("new");
		return selection.length > 0;
	},
	menuButtonAction: function () {
	},
	GetText: function (obj) {
		if (typeof obj == "string")
			return obj;
		var text = "";

		text = $(obj).attr("Text");

		if (!(text && text.trim()) && obj.nodeName)
			text = obj.nodeName;
		text = isOk(text)? text.trim(): "";

		return text;
	},
	registerframe: function (window) {
		
	},
	createButtons: function (_section, buttons, pageName) {
		var _ = lw.manager;
		
		var id = "buttons_{0}".Format(lw.utils.toId(pageName));

		if (_.sprite.find("#" + id).length > 0) {
			_.sprite.find("#" + id).remove();
		}

		var div = $("<div id=\"{0}\"/>".Format(id));

		$.each(buttons, function () {
			var b = $("<button />").html("<span>" + this.text + "</span>");
			b.addClass(this.css).on("click", this.action);
			b.appendTo(div);
		});
		div.appendTo(_section["sprite"]);
	}
}
