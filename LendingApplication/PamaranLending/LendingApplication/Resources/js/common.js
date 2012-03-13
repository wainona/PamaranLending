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