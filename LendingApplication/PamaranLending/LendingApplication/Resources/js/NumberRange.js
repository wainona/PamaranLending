Ext.apply(Ext.form.VTypes, {
    numberrange: function (val, field) {
        if (!val) {
            return;
        }

        if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
            var start = Ext.getCmp(field.startNumberField);

            if (start) {
                start.setMaxValue(val);
                field.numberRangeMax = val;
                start.validate();
            }
        } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
            var end = Ext.getCmp(field.endNumberField);

            if (end) {
                end.setMinValue(val);
                field.numberRangeMin = val;
                end.validate();
            }
        }

        return true;
    }
});