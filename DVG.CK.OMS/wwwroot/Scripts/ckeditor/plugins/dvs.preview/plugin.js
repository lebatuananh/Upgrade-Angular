CKEDITOR.plugins.add('dvs.preview', {
    init: function (editor) {
        editor.addCommand('dvs.preview', new CKEDITOR.dialogCommand('PreviewDialog'));
        editor.ui.addButton('dvs.preview', {
            label: 'Preview',
            command: 'dvs.preview',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.preview.png',
            click: function (editor) {
                var w = window.open();
                var title = $("#Title").val();
                var sapo = $("#Sapo").val(); 
                var html = "<html>"
                    + "<head>"
                    + "<title>Xem trước tin đăng</title>"
                    + "<script type = 'text/javascript' src = '/assets/js/jquery.min.js'></script>"
                    + "<script type = 'text/javascript' src = 'http://img1.banxehoi.com.vn/SubtitleUploadImages/jwplayer/jwplayer.js'></script>"
                    + "<script type = 'text/javascript' src = 'http://banxehoi.com/Scripts/main.js?v=20140413'></script>"
                    + "<link rel='stylesheet' href='http://banxehoi.com/Styles/MainStyle.css?v=20140413' /> "
                    + "<script type = 'text/javascript'>"
                    + "jwplayer.key = '5qMQ1qMprX8KZ79H695ZPnH4X4zDHiI0rCXt1g==';"
                    + "var interval = setInterval(function() {"
                    + "$(\"div[id*='player'][data-type='jwplayer']\").playVideo();"
                    + "$(\"div[data-type='embed']\").playVideo();"
                    + "clearInterval(interval);"
                    + "}, 500);"
                    + "</script>"
                    + "<style type = 'text/css'>"
                    + ".content-newsdetail { margin: 10px auto !important; float: none !important; }"
                    + "</style>"
                    + "</head>"
                    + "<body>"
                    + "<div class='content-newsdetail'>"
                    + "<div class = 'newsmain'>"
                    + "<h1 class = 'oswaldlarge-title'>"
                    + title
                    + "</h1>"
                    + "<div class = 'content-newsmain'>"
                    + "<p class='introduction-newsmain'>"
                    + sapo
                    + "</p>"
                    + editor.getData();
                    + "</div>"
                    + "</div>"
                    + "</div>"
                    + "</body>"
                    + "</html>";

                w.document.write(html);
            }
        });
    }
});