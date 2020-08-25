CKEDITOR.plugins.add('dvg.articledetailrelatedlink', {
    init: function (editor) {
      editor.addCommand('dvg.articledetailrelatedlink', new CKEDITOR.dialogCommand('ArticleDetailRelatedLinkDialog'));
      editor.addContentsCss(this.path + 'css/style.css');
      editor.ui.addButton('dvg.articledetailrelatedlink', {
            label: 'Article Detail Related Link',
            command: 'dvg.articledetailrelatedlink',
            toolbar: 'insert',
            icon: this.path + 'icons/dvg.articledetailrelatedlink.png'
        });

      CKEDITOR.dialog.add('ArticleDetailRelatedLinkDialog', this.path + 'dialogs/dvg.articledetailrelatedlink.js');
    }
});
