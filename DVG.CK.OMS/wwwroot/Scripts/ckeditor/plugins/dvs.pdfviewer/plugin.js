CKEDITOR.plugins.add('dvs.pdfviewer', {
    init: function (editor) {
        editor.addCommand('dvs.pdfviewer', new CKEDITOR.dialogCommand('PDFviewerDialog'));
        editor.ui.addButton('dvs.pdfviewer', {
            label: 'PDF viewer',
            command: 'dvs.pdfviewer',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.pdf.png'
        });

        CKEDITOR.dialog.add('PDFviewerDialog', this.path + 'dialogs/dvs.pdfviewer.js');
        editor.on('doubleclick', function (evt) {
            var element = evt.data.element;
            if (element.data("cke-real-element-type") && element.data("cke-real-element-type") == "iframe")
                evt.data.dialog = 'PDFviewerDialog';
        });
    }
});