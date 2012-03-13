Ext.ns('ntfx');

ntfx.ListManager = Ext.extend(Ext.Panel, {
  
  idSeparator : '#',
  
  msgProxy : new ntfx.MIFMessagingProxy(),

  initComponent : function(){
	  
	  if (!this.initialConfig.nodeManager)
    {
      throw 'nodeManager is missing in ntfx.ListManager.initComponent.';
    }
	  
	  this.tabPanel = new Ext.TabPanel({
      cls : 'tabbed-view',
			animScroll : true,
			resizeTabs : true,
			minTabWidth	: 100,
			enableTabScroll	: true,			
			activeTab : 0,
			autoScroll : true,
			border : true,
			layoutOnTabChange : true
    });
	  this.tabPanel.on('remove', this.onTabRemoved, this);
    this.tabPanel.on('tabchange', this.onTabChanged, this);
	  
	  // home tab
	  var home = new Ext.Panel({
	    id : 'tab-home',
	    title : 'Home',
	    layout : 'fit',
	    autoHeight : true
	  });
	  this.tabPanel.add(home);
	  this.tabPanel.setActiveTab(home);
	  
	  
		var config = {
			region		: 'center',
			layout		: 'fit',
			border		: false,
			items			: [this.tabPanel]
		};
		
		Ext.apply(this, Ext.apply(config, this.initialConfig));
		
		ntfx.ListManager.superclass.initComponent.apply(this, arguments);

    this.nodeManager.on('deleteNodeSuccess', this.onNodeDeleted, this);
	},

  onNodeDeleted : function(nodeId){
    // if node is a list, an application or a weblink,
    // close its tab if it is open
	  this.tabPanel.remove(this.formatId(nodeId));
	},
	
	showNode : function(node){
	  switch (node.type.toLowerCase())
	  {
	    case 'folder':
	      this.showFolderView(node.id, node.name);
	      break;
	      
	    case 'shortcut':
	      switch (node.original_node_type.toLowerCase())
	      {
	        case 'folder': 
	          this.showFolderView(node.original_node_id, node.name);
	          break;
	          
	        case 'weblink':
            var regex = new RegExp('http://.*');
            if (!regex.test(node.url))
            {
              node.url= 'http://' + node.url;
            }
            console.log(node.url);
	          this.addMIFTab(
              this.formatId(node.original_node_id),
              node.name,
              node.url,
              true
            );
	          break;
	          	      
	        case 'list':
	          this.addMIFTab(
              this.formatId(node.original_node_id),
              node.name,
              ntfx.Config.Url.Node.redirectToList + '/id/' + node.original_node_id,
              true
            );
	          break;
	      
          case 'application':
            this.addMIFTab(
              this.formatId(node.original_node_id),
              node.name,
              ntfx.Config.Url.Node.redirectToApplication + '/id/' + node.original_node_id,
              true
            );
            break;

          case 'file':
            var command = new ntfx.commands.DownloadFile(node.original_node_id);
            command.execute();
            break;

	        default: /*nothing to do here*/ 
	          break;
	      } // switch
	      break;
	      
	    case 'weblink':
	      var regex = new RegExp('http://.*');
	      if (!regex.test(node.url))
	      {
	        node.url= 'http://' + node.url;
	      }
	      this.addMIFTab(this.formatId(node.id), node.name, node.url, true);
	      break;
	      
	    case 'list':
	      this.addMIFTab(
          this.formatId(node.id),
          node.name,
          ntfx.Config.Url.Node.redirectToList + '/id/' + node.id,
          true
        );
	      break;
	      
	    case 'application':
	      this.addMIFTab(
          this.formatId(node.id),
          node.name,
          ntfx.Config.Url.Node.redirectToApplication + '/id/' + node.id,
          true
        );
	      break;
	      
	    default : throw 'Unexpected node type: ' + node.type;
	  } // switch
	},
	
	/**
	 * Event handler called every time a tab is removed.
	 */
	onTabRemoved : function(container, component){
    // we try to retrieve the parent of the tab being removed
    // if found, we activate that tab
    var parentId = this.getParentId(component.id);
    var parentTab = this.tabPanel.getItem(parentId);
    if (parentTab)
    {
      this.tabPanel.activate(parentTab);
    } 
    // we search for the closing tab's children
    // and we remove all of them.
    var regex = new RegExp(component.id + '.*');
    var children = [];
    
    this.tabPanel.items.each(
      function(item, index, length){
        if (regex.test(item.id))
        {
          children.push(item.id);
        }
      }
    );
    
    for (var i = 0; i < children.length; i++)
    {
      // remove event handlers
      var tab = this.tabPanel.get(children[i]);
      tab.un('blur', this.onMIFBlur);
      tab.un('focus', this.onMIFFocus);

      this.tabPanel.remove(children[i]);
    }
	},

  onTabChanged : function(tabPanel, tab){
    var array = tab.id.split(this.idSeparator);
	  var topMostParentId = array.shift();
    if (topMostParentId == 'tab-folder-view')
    {
      topMostParentId = tab.items.itemAt(0).parentNodeId;
    }
    this.fireEvent('nodeSelected', {id: topMostParentId, type: ''});
  },

	/**
	 * Utility function that formats tab ids.
	 * Returns values in this format: [parentId][idSeparator][id]
	 * This is helps avoid conflicts of ids among tabs.
	 */
	formatId : function(id, parentId){
	  var temp = '';
	  if (parentId) temp += parentId + this.idSeparator;
	  temp += id;
	  return temp;
	},
	
	/**
	 * Adds a managed iframe as a tab and attaches a listener for cross-frame messaging.
	 */
	addMIFTab : function(id, name, url, activate){
	  
	  var panel = new Ext.ux.ManagedIFrame.Panel({
      id		            : id,
      border	          : false,
      title             : Ext.util.Format.ellipsis(name, 15, true),
      tabTip            : name,
      autoScroll        : true,
      closable          : true,
      layout	          : 'fit',
      loadMask          : {msg:'loading tab - ' + name + '...'},
      frameConfig       : { disableMessaging  : false },
		  defaultSrc        : url,
		  listeners         : {
		    // append the id of this tab
        message : this.onMessageReceived.createDelegate(this),
        blur : this.onMIFBlur,
        focus : this.onMIFFocus
		  } 
	  });
	  
	  this.tabPanel.add(panel);
	  // activate
	  if (activate) this.tabPanel.setActiveTab(panel);
	},

  onMIFBlur : function(el, e){
    var win = el.getWindow();
    if (!win.Ext) return;
    win.Ext.menu.MenuMgr.hideAll();
  },

  onMIFFocus : function(){
    window.Ext.menu.MenuMgr.hideAll();
  },
	
	/**
	 * Utility function which extracts the parent tab id from a child tab id.
	 * Ids are just joined by a separator character (#) with the child id 
	 * appended at the end. So to extract the child tab id, we split the string using
	 * the separator character then remove the last item on the array. We then
	 * join the array using the separator character.
	 */
	getParentId : function(childId){
	  var array = childId.split(this.idSeparator);
	  array.pop();
	  if (array.length < 1) return '';
	  return array.join(this.idSeparator);
	},
	
	/**
	 * Listens to all messages sent by iframes in the tab panel.
	 */
	onMessageReceived : function(frame, message){
	
	  var senderId = frame.ownerCt.id;
	  
    // these are predefined tags used to determine what to do with the message.
    var baseTag = '';
    // tags provided by the sender.
    var extTag = '';
    
    // using regex, we determine if any of the base tags has a match on the current tag.
    // if a match is found, we extract the extension tag.
    for (var i = 0; i < this.msgProxy.baseTags.length; i++)
    {
      var regEx = new RegExp('(' + this.msgProxy.baseTags[i] + ')\/?(.*)');
      if (regEx.test(message.tag))
      {
        baseTag = this.msgProxy.baseTags[i];
        extTag = (message.tag == baseTag) ? '' : message.tag.replace(regEx, '$2');
        break;
      }
    }
    
    switch (baseTag)
    { 
      // a message to all tabs
      case this.msgProxy.ALL_TAG:
        this.tabPanel.items.each(
          function(item, index, length){
            if (item.getFrame)
            {
              var frame = item.getFrame();
              if(frame) frame.sendMessage(message.data, extTag);
            }
          }
        );
	      break;
	      
	    // a message to a parent tab
	    case this.msgProxy.PARENT_TAG:
	      var parentId = this.getParentId(senderId);
	      if (parentId == '') break;
	      var parentTab = this.tabPanel.getItem(parentId);
        if (parentTab)
        {	
          parentTab.getFrame().sendMessage(message.data, extTag);
        }
	      break;
	    
	    // a message to a child tab
	    case this.msgProxy.CHILD_TAG:
	      var id = this.formatId(message.data.id, senderId);
	      var child = this.tabPanel.getItem(id);
	      if (child)
	      {
	        child.getFrame().sendMessage(message.data.message, extTag);
	      }
	      break;
	    
	    // a new tab request
	    case this.msgProxy.NEW_TAB_REQUEST:
	      this.addMIFTab(this.formatId(message.data.id, senderId), message.data.title, message.data.url, true);
	      break;
	    
	    // a close tab request
	    case this.msgProxy.CLOSE_TAB_REQUEST:
        var id = (message.data) ? this.formatId(message.data, senderId) : senderId;
        this.tabPanel.remove(id);
        break;
      
      default: /* nothing to do, take a break */ 
        break;
    }
        
	},
	
	showFolderView : function(nodeId, nodeName){
    // try to retrieve the folder view from the tabs
    var folder = this.tabPanel.getItem('tab-folder-view');
    // if its not available, create it
    if (!folder)
    {
      var folderView = new ntfx.FolderView({nodeManager : this.nodeManager});
      
      this.relayEvents(folderView, [
        'nodeselected',
        'nodecreate', 
        'nodemodify', 
        'nodedelete', 
        'nodeproperties'
      ]);
      
      this.on('nodeselected', this.onNodeSelected, this);
      
      folder = new Ext.Panel({
        id		      : 'tab-folder-view',
        border      : false,
        title       : Ext.util.Format.ellipsis(nodeName, 15, true),
        tabTip      : nodeName,
        autoScroll  : true,
        closable    : true,
        layout	    : 'fit',
        items	      : [folderView]							
      });			    
    }
    folder.setTitle(nodeName);
    // add the folder view to the tabs and activate it
    this.tabPanel.add(folder);
    this.tabPanel.setActiveTab(folder);
    // load the folder contents
    folder.items.itemAt(0).loadContents(nodeId);
	},
	
	onNodeSelected : function(node){
	  var type = node.type.toLowerCase();
    if (type == 'list' || type == 'application' || type == 'weblink' || type == 'shortcut' || type == 'folder')
    {
      this.showNode(node);
    }
    else if (type == 'file')
    {
      var command = new ntfx.commands.DownloadFile(node.id);
      command.execute();
    }
    else {/* do nothing */}
	}
	
});