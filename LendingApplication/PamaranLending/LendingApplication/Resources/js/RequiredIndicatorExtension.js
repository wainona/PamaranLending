Ext.intercept(Ext.form.Field.prototype, 'initComponent', function () {
    var fl = this.fieldLabel, ab = this.allowBlank;
    if (ab === false && fl) {
        this.fieldLabel = '<span style="color:Red;font-weight:bold;">*</span>' + fl;
    }
});

var markIfRequired = function (element) {
    var fl = element.fieldLabel, ab = element.allowBlank;
    fl = fl.replace('<span style="color:Red;font-weight:bold;">*</span>', '');
    if (ab === false && fl) {
        fl = fl.replace(':', '');
        fl = '<span style="color:Red;font-weight:bold;">*</span>' + fl + ':';
    }

    setFieldLabel(element, fl);
}

var markIfRequired = function (element, compositeField) {
    var fl = element.fieldLabel, ab = element.allowBlank;
    if (fl)
        fl = fl.replace('<span style="color:Red;font-weight:bold;">*</span>', '');
    if (ab === false && fl) {
        fl = fl.replace(':', '');
        fl = '<span style="color:Red;font-weight:bold;">*</span>' + fl + ':';
    }

    setFieldLabel(element, fl, compositeField);
}

var setFieldLabel = function (element, text, compositeField) {
    if (element.rendered) {
        compositeField.el.up('.x-form-item', 10, true).child('.x-form-item-label').update(text);
    }
    compositeField.fieldLabel = text;
}

var setFieldLabel = function (element, text) {
    if (element.rendered) {
        element.el.up('.x-form-item', 10, true).child('.x-form-item-label').update(text);
    }
    element.fieldLabel = text;
}