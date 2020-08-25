CKEDITOR.plugins.add('dvs.tabletemplate', {
    init: function (editor) {
        editor.addCommand('dvs.tabletemplate', new CKEDITOR.dialogCommand('TableTemplateDialog'));
        editor.ui.addButton('dvs.tabletemplate', {
            label: 'Table Template',
            command: 'dvs.tabletemplate',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.tabletemplate.png',
        });

        CKEDITOR.dialog.add('TableTemplateDialog', this.path + 'dialogs/dvs.tabletemplate.js');
    }
});