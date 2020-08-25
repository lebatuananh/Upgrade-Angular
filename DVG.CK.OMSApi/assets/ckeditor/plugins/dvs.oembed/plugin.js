CKEDITOR.plugins.add( 'dvs.oembed', {
    icons: 'embed', // %REMOVE_LINE_CORE%
    hidpi: true, // %REMOVE_LINE_CORE%

    init: function( editor ) {
        editor.addCommand('dvs.oembed', new CKEDITOR.dialogCommand('OEmbedDialog'));
        editor.ui.addButton('dvs.oembed', {
            label: 'embed',
            command: 'dvs.oembed',
            toolbar: 'insert',
            icon: this.path + 'icons/embed.png'
        });

        CKEDITOR.dialog.add('OEmbedDialog', this.path + 'dialogs/dvs.oembed.js');

        editor.on('doubleclick', function (evt) {
            var clickElem = evt.data.element.$; // will contain a native element
            var oEmbedElem = $(clickElem).closest("div.dvs-oembed");
            if (oEmbedElem.length > 0) {
                editor.openDialog('OEmbedDialog');
                evt.stop();
            }
        });
    }
} );