CKEDITOR.plugins.add('dvs.embed', {
    init: function (editor) {
        editor.addCommand('dvs.embed', new CKEDITOR.dialogCommand('EmbedDialog'));
        editor.ui.addButton('dvs.embed', {
            label: 'Embed',
            command: 'dvs.embed',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.embed.png',
        });

        CKEDITOR.dialog.add('EmbedDialog', this.path + 'dialogs/dvs.embed.js');
    }
});