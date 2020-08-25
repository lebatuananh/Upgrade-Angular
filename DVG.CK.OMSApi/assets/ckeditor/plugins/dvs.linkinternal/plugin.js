CKEDITOR.plugins.add('dvs.linkinternal', {
    init: function (editor) {
      editor.addCommand('dvs.linkinternal', new CKEDITOR.dialogCommand('LinkInternalDialog'));
      editor.ui.addButton('dvs.linkinternal', {
            label: 'Link Internal',
            command: 'dvs.linkinternal',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.linkinternal.jpg'
        });

      CKEDITOR.dialog.add('LinkInternalDialog', this.path + 'dialogs/dvs.linkinternal.js');
    }
});
