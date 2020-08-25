CKEDITOR.plugins.add('dvs.image', {
    init: function (editor) {
        editor.addCommand('dvs.image', new CKEDITOR.dialogCommand('ImageDialog'));
        editor.ui.addButton('dvs.image', {
            label: 'Image',
            command: 'dvs.image',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.image.png'
        });

        CKEDITOR.dialog.add('ImageDialog', this.path + 'dialogs/dvs.image.js');
    }
});