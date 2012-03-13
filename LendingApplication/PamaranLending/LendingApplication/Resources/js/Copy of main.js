Ext.ns("X");

var makeTab = function (id, url, title) {
    var win,
        tab,
        hostName,
        exampleName,
        node,
        tabTip;

    if (id === "-") {
        id = Ext.id(undefined, "extnet");
        lookup[url] = id;
    }

    tabTip = url.replace(/^\//g, "");
    tabTip = tabTip.replace(/\/$/g, "");
    tabTip = tabTip.replace(/\//g, " > ");
    tabTip = tabTip.replace(/_/g, " ");

    hostName = window.location.protocol + "//" + window.location.host;
    exampleName = url;

    tab = WorkspaceTabs.add(new Ext.Panel({
        id: id,
        title: title,
        tabTip: tabTip,
        hideMode: "offsets",
        autoLoad: {
            showMask: true,
            scripts: true,
            mode: "iframe",
            url: hostName + "" + url
        },
        listeners: {
            deactivate: {
                fn: function (el) {
                    if (this.sWin && this.sWin.isVisible()) {
                        this.sWin.hide();
                    }
                }
            },

            destroy: function () {
                if (this.sWin) {
                    this.sWin.close();
                    this.sWin.destroy();
                }
            }
        },
        closable: true
    }));

    tab.sWin = win;
    WorkspaceTabs.setActiveTab(tab);

    var node = NavigationTree.getNodeById(id);
    
    if (node) {
        node.ensureVisible(function () {
            NavigationTree.getNodeById(this.id).select();
        }, node);
    } else {
        NavigationTree.on("load", function (node) {
            node = NavigationTree.getNodeById(id);

            if (node) {
                node.ensureVisible(function () {
                    NavigationTree.getNodeById(this.id).select();
                }, node);
            }
        }, this, { single: true });
    }
};

var lookup = {};

var loadExample = function (href, id, title) {
    var tab = WorkspaceTabs.getComponent(id),
        lObj = lookup[href];

    if (id == "-") {
        var tree = NavigationTree
        var foundNode;
        var node = tree.getRootNode().findChildBy(find = function (searchNode) {
            if (typeof (searchNode.attributes) !== "undefined" && searchNode.attributes.href == href) {
                foundNode = searchNode;
                return true;
            }
            for (var i = 0; i < searchNode.childNodes.length; i++) {
                if (find(searchNode.childNodes[i]))
                    return true;
            }
            return false;
        });
        loadExample(href, foundNode.id, foundNode.text);
        return;
    }

    lookup[href] = id;

    if (tab) {
        WorkspaceTabs.setActiveTab(tab);
    } else {
        if (Ext.isEmpty(title)) {
            var m = /(\w+)\/$/g.exec(href);
            title = m == null ? "[No name]" : m[1];
        }

        title = title.replace(/<span>&nbsp;<\/span>/g, "");
        title = title.replace(/_/g, " ");
        makeTab(id, href, title);
    }
};

var selectionChaged = function (dv, nodes) {
    if (nodes.length > 0) {
        var url = nodes[0].getAttribute("ext:url"),
            id = nodes[0].getAttribute("ext:id");

        loadExample(url, id, nodes[0].getAttribute("ext:title"));
    }
};

var viewClick = function (dv, e) {
    var group = e.getTarget("h2", 3, true);

    if (group) {
        group.up("div").toggleClass("collapsed");
    }
};

var beforeSourceShow = function (el) {
    var height = Ext.getBody().getViewSize().height;

    if (el.getSize().height > height) {
        el.setHeight(height - 20);
    }
};

var change = function (token) {
    if (token) {
        loadExample(token, lookup[token] || "-");
    } else {
        WorkspaceTabs.setActiveTab(0);
    }
};

var addToken = function (el, tab) {
    if (tab && tab.autoLoad && tab.autoLoad.url) {
        var host = window.location.protocol + "//" + window.location.host,
            token = tab.autoLoad.url.substr(host.length);

        if (!Ext.isEmpty(token)) {
            HistoryManager.add(token);
        }
    } else {
        HistoryManager.add("");
    }
};

var keyUp = function (el, e) {
    var tree = NavigationTree,
        text = this.getRawValue();

    if (e.getKey() === 40) {
        tree.getRootNode().select();
    }

    if (Ext.isEmpty(text, false)) {
        clearFilter(el);
    }

    if (text.length < 3) {
        return;
    }

    tree.clearFilter();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    el.triggers[0].show();

    if (e.getKey() === Ext.EventObject.ESC) {
        clearFilter(el);
    } else {
        var re = new RegExp(".*" + text + ".*", "i");

        tree.getRootNode().collapse(true, false);

        tree.filterBy(function (node) {
            var match = re.test(node.text.replace(/<span>&nbsp;<\/span>/g, "")),
                pn = node.parentNode;

            if (match && node.isLeaf()) {
                pn.hasMatchNode = true;
            }

            if (pn != null && pn.fixed) {
                if (node.isLeaf() === false) {
                    node.fixed = true;
                }
                return true;
            }

            if (node.isLeaf() === false) {
                node.fixed = match;
                return match;
            }

            return (pn != null && pn.fixed) || match;
        }, { expandNodes: false });

        tree.getRootNode().cascade(function (node) {
            if (node.isRoot) {
                return;
            }

            if ((node.getDepth() === 1) ||
               (node.getDepth() === 2 && node.hasMatchNode)) {
                node.expand(false, false);
            }

            delete node.fixed;
            delete node.hasMatchNode;
        }, tree);
    }
};

var clearFilter = function (el, trigger, index, e) {
    var tree = NavigationTree;

    el.setValue("");
    el.triggers[0].hide();
    tree.clearFilter();
    tree.getRootNode().collapseChildNodes(true);
    el.focus(false, 100);
};

var filterSpecialKey = function (field, e) {
    if (e.getKey() == e.DOWN) {
        var n = NavigationTree.getRootNode().findChildBy(function (node) {
            return node.isLeaf() && !node.hidden;
        }, NavigationTree, true);

        if (n) {
            n.ensureVisible(function () {
                NavigationTree.getSelectionModel().select(n);
            });
        }
    }
};

var loadComments = function (at, url) {
    winComments.url = url;

    winComments.show(at, function () {
        updateComments(false, url);
        TagsView.store.reload();
    });
};

var updateComments = function (updateCount, url) {
    winComments.body.mask("Loading...", "x-mask-loading");
    Ext.net.DirectMethod.request({
        url: "/ExampleLoader.ashx",
        cleanRequest: true,
        params: {
            url: url,
            action: "comments.build"
        },
        success: function (result, response, extraParams, o) {
            if (result && result.length > 0) {
                tplComments.overwrite(CommentsBody.body, result);
            }

            if (updateCount) {
                WorkspaceTabs.getActiveTab().commentsBtn.setText("Comments (" + result.length + ")");
            }
        },
        complete: function (success, result, response, extraParams, o) {
            winComments.body.unmask();
        }
    });
};

if (window.location.href.indexOf("#") > 0) {
    var directLink = window.location.href.substr(window.location.href.indexOf("#") + 1);

    Ext.onReady(function () {
        if (!Ext.isEmpty(directLink, false)) {
            loadExample(directLink, "-");
        }
    }, window, { delay: 100 });
}

Ext.onReady(function () {
    //    var MIF = new Ext.ux.ManagedIFrame.Panel({
    //        autoCreate: { id: 'frame1', width: 600, height: 400 }
    //     , src: 'embedPage1.html'
    //     , disableMessaging: false
    //     , listeners:
    //        //only subscribe to 'startup' tagged messages 
    //         {'message:startup': function (srcFrame, message) {
    //             alert(message.uri + ' says:\n' + message.data);
    //             srcFrame.sendMessage('Acknowledged!', 'startup'); //to 'startup' tag subscribers
    //         },
    //         domready: function (frame) {
    //             if (frame.domWritable()) {
    //                 frame.execScript('init()');
    //             }
    //         }
    //     }
    //    });

    var name = 'test';
    var url = 'http://www.google.com';
    var tab = new Ext.ux.ManagedIFrame.Panel({
        id: id,
        border: false,
        title: Ext.util.Format.ellipsis(name, 15, true),
        tabTip: name,
        autoScroll: true,
        closable: true,
        layout: 'fit',
        loadMask: { msg: 'loading tab - ' + name + '...' },
        frameConfig: { disableMessaging: false },
        defaultSrc: url,
        listeners: {
        // append the id of this tab
        //            message: this.onMessageReceived.createDelegate(this),
        //            blur: this.onMIFBlur,
        //            focus: this.onMIFFocus
    }
});
WorkspaceTabs.add(tab);
});