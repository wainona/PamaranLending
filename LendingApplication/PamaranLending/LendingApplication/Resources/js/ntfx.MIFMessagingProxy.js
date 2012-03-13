Ext.ns('ntfx');

ntfx.MIFMessagingProxy = Ext.extend(Ext.util.Observable, {
    /**
    * Determines whether the proxy has been initialized or not.
    */
    initialized: false,
    /**
    * Holds a copy of the window context represented by this proxy.
    */
    context: null,
    /**
    * Contains all base tags defined in this proxy.
    */
    baseTags: [],

    constructor: function () {

        this.BASE_TAG = 'ntfx';
        this.ALL_TAG = this.BASE_TAG + '/all';
        this.PARENT_TAG = this.BASE_TAG + '/parent';
        this.CHILD_TAG = this.BASE_TAG + '/child';
        this.NEW_TAB_REQUEST = this.BASE_TAG + '/tab/new';
        this.CLOSE_TAB_REQUEST = this.BASE_TAG + '/tab/close';

        this.baseTags = [
	    this.ALL_TAG,
	    this.PARENT_TAG,
	    this.CHILD_TAG,
	    this.NEW_TAB_REQUEST,
	    this.CLOSE_TAB_REQUEST
    ];

        ntfx.MIFMessagingProxy.superclass.constructor.apply(this, arguments);
        this.addEvents('messagereceived');
    },

    /**
    * Function which needs to be called before messages can be sent and received. 
    * The DOM has to be fully loaded before calling this method.
    * @param {mixed} Tags used to filter incoming messages. 
    * This can be an array or a single string value.
    * Null means that this proxy will only listen to messages WITHOUT tags.
    * 
    */
    init: function (tags) {

        if (this.initialized) return;

        if (!window.sendMessage || !window.onhostmessage) {
            throw 'Messaging is not enabled.';
        }
        // remember context
        this.context = window;

        // listen for incoming messages
        if (!tags) tags = [null];
        if (!(tags instanceof Array)) tags = [tags];
        for (var i = 0; i < tags.length; i++) {
            this.context.onhostmessage(this.onHostMessageHandler.createDelegate(this), this, false, tags[i]);
        }

        // at last!i'm alive! and listening..and sending messages..
        this.initialized = true;
    },

    onHostMessageHandler: function (message) {
        this.fireEvent('messagereceived', message);
    },

    sendMessage: function (message, tag) {

        if (!this.initialized) {
            throw 'Proxy is not yet initialized.';
        }

        this.context.sendMessage(message, tag);
    },

    /**
    * Utility funciton that appends an extension tag to a base tag.
    * If extension tag is null or empty, the base tag is returned.
    * @param {string} base The base tag.
    * @param {string} ext The extension tag.
    */
    concat: function (base, ext) {
        return ext ? base + '/' + ext : base;
    },

    sendToAll: function (message, tag) {
        this.sendMessage(message, this.concat(this.ALL_TAG, tag));
    },

    sendToParent: function (message, tag) {
        this.sendMessage(message, this.concat(this.PARENT_TAG, tag));
    },

    sendToChild: function (id, message, tag) {
        this.sendMessage({ id: id, message: message }, this.concat(this.CHILD_TAG, tag));
    },

    /**
    * Sends request to host to open a new tab and load the specified url.
    * @param {string} id The id used to identify this newly opened tab.
    * @param {string} url The URL to open.
    * @param {string} title The title of the new tab.
    */
    requestNewTab: function (id, url, title) {
        this.sendMessage({ id: id, url: url, title: title }, this.NEW_TAB_REQUEST);
    },

    /**
    * Sends request to host to close a tab. If an id is provided,
    * the hosts searches for the tab with that id and closes it.
    * Note that tabs can only request to close their children.
    * @param {string} id The id of the tab to close.
    */
    requestClose: function (id) {
        this.sendMessage(id || '', this.CLOSE_TAB_REQUEST);
    }

});


var showAlert = function (title, msg, fn, scope) {
    Ext.MessageBox.show({
        title: title,
        msg: msg,
        buttons: Ext.MessageBox.OK,
        fn: fn,
        scope: scope,
        minWidth: Ext.MessageBox.minWidth,
        closable: false
    });
}

var showConfirm = function (title, msg, fn, scope) {
    Ext.MessageBox.show({
        title: title,
        msg: msg,
        buttons: Ext.MessageBox.YESNO,
        fn: fn,
        scope: scope,
        icon: Ext.MessageBox.QUESTION,
        minWidth: Ext.MessageBox.minWidth,
        closable: false
    });
}