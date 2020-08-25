CKEDITOR.plugins.add('dvg.inserttextbox', {
    init: function (editor) {
      editor.addCommand('dvg.inserttextbox', {
        exec: function (editor) {
          var html = "<div class=text-box-seo>";
          html += "[Phần nội dung được hiển thị]";
          html += "<div class=text-box-seo-readmore><a href=#>Click to read more</a></div>";
          html += "<div class=text-box-seo-body>";
          html += "[Phần nội dung được xổ ra / thu gọn khi user bấm vào link read more]";
          html += "</div>";
          html += "</div>";
          editor.insertHtml(html);
        }
      });
      editor.ui.addButton('dvg.inserttextbox', {
            label: 'Insert Text Box SEO',
            command: 'dvg.inserttextbox',
            toolbar: 'insert',
            icon: this.path + 'icons/dvg.inserttextbox.png'
        });
    }
});
