Ext.ns('ntfx');

ntfx.tabManager = function (tabPanel) {

    var self = this;

    self.tabs = tabPanel;
    self.idSeparator = '#';
    self.msgProxy = new ntfx.MIFMessagingProxy();
    this.init = function () {
        self.tabs.on('remove', self.onTabRemoved, self);
        self.tabs.on('tabchange', self.onTabChanged, self);
    }

    /**
    * Adds a managed iframe as a tab and attaches a listener for cross-frame messaging.
    */
    this.addMIFTab = function (id, name, url, activate, icon) {

        var panel = new Ext.ux.ManagedIFrame.Panel({
            id: id,
            border: false,
            title: name,
            tabTip: name,
            autoScroll: true,
            closable: true,
            layout: 'Fit',
            iconCls: icon,
            loadMask: { msg: 'Loading Tab - ' + name + '...' },
            frameConfig: { disableMessaging: false },
            defaultSrc: url,
            disableMessaging: false,
            listeners: {
                // append the id of this tab
                message: this.onMessageReceived.createDelegate(this),
                blur: this.onMIFBlur,
                focus: this.onMIFFocus
            }
        });

        self.tabs.add(panel);
        // activate
        if (activate) self.tabs.setActiveTab(panel);
    };

    this.onMIFBlur = function (el, e) {
        var win = el.getWindow();
        if (!win.Ext) return;
        win.Ext.menu.MenuMgr.hideAll();
    };

    this.onMIFFocus = function () {
        window.Ext.menu.MenuMgr.hideAll();
    };

    /**
    * Utility function which extracts the parent tab id from a child tab id.
    * Ids are just joined by a separator character (#) with the child id 
    * appended at the end. So to extract the child tab id, we split the string using
    * the separator character then remove the last item on the array. We then
    * join the array using the separator character.
    */
    this.getParentId = function (childId) {
        var array = childId.split(self.idSeparator);
        array.pop();
        if (array.length < 1) return '';
        return array.join(self.idSeparator);
    };

    /**
    * Listens to all messages sent by iframes in the tab panel.
    */
    this.onMessageReceived = function (frame, message) {

        var senderId = frame.ownerCt.id;

        // these are predefined tags used to determine what to do with the message.
        var baseTag = '';
        // tags provided by the sender.
        var extTag = '';

        // using regex, we determine if any of the base tags has a match on the current tag.
        // if a match is found, we extract the extension tag.
        for (var i = 0; i < self.msgProxy.baseTags.length; i++) {
            var regEx = new RegExp('(' + self.msgProxy.baseTags[i] + ')\/?(.*)');
            if (regEx.test(message.tag)) {
                baseTag = self.msgProxy.baseTags[i];
                extTag = (message.tag == baseTag) ? '' : message.tag.replace(regEx, '$2');
                break;
            }
        }

        switch (baseTag) {
            // a message to all tabs          
            case self.msgProxy.ALL_TAG:
                this.tabs.items.each(
                  function (item, index, length) {
                      if (item.getFrame) {
                          var frame = item.getFrame();
                          if (frame) frame.sendMessage(message.data, extTag);
                      }
                  }
                );
                break;

            // a message to a parent tab          
            case self.msgProxy.PARENT_TAG:
                var parentId = this.getParentId(senderId);
                if (parentId == '') break;
                var parentTab = this.tabs.getItem(parentId);
                if (parentTab) {
                    parentTab.getFrame().sendMessage(message.data, extTag);
                }
                break;

            // a message to a child tab          
            case self.msgProxy.CHILD_TAG:
                var id = self.formatId(message.data.id, senderId);
                var child = this.tabs.getItem(id);
                if (child) {
                    child.getFrame().sendMessage(message.data.message, extTag);
                }
                break;

            // a new tab request          
            case self.msgProxy.NEW_TAB_REQUEST:
                self.addMIFTab(self.formatId(message.data.id, senderId), message.data.title, message.data.url, true);
                break;

            // a close tab request          
            case self.msgProxy.CLOSE_TAB_REQUEST:
                var id = (message.data) ? self.formatId(message.data, senderId) : senderId;
                self.tabs.remove(id);
                break;

            default: /* nothing to do, take a break */
                break;
        }

    };
    this.onTabRemoved = function (container, component) {
        // we try to retrieve the parent of the tab being removed
        // if found, we activate that tab
        var parentId = self.getParentId(component.id);
        var parentTab = self.tabs.getItem(parentId);
        if (parentTab) {
            self.tabs.activate(parentTab);
        }
        // we search for the closing tab's children
        // and we remove all of them.
        var regex = new RegExp(component.id + '.*');
        var children = [];

        self.tabs.items.each(
      function (item, index, length) {
          if (regex.test(item.id)) {
              children.push(item.id);
          }
      }
    );

        for (var i = 0; i < children.length; i++) {
            // remove event handlers
            var tab = self.tabs.get(children[i]);
            tab.un('blur', self.onMIFBlur);
            tab.un('focus', self.onMIFFocus);

            self.tabs.remove(children[i]);
        }
    };

    this.onTabChanged = function (tabPanel, tab) {
        if (tab) {
            var array = tab.id.split(self.idSeparator);
            var topMostParentId = array.shift();
            if (topMostParentId == 'tab-folder-view') {
                topMostParentId = tab.items.itemAt(0).parentNodeId;
            }
        }
    };
    this.formatId = function (id, parentId) {
        var temp = '';
        if (parentId) temp += parentId + this.idSeparator;
        temp += id;
        return temp;
    };
};