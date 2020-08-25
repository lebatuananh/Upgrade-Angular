var _objEdtior;
var _file;
(function () {
    CKEDITOR.plugins.add('myimage', {
        init: function (editor) {
            // Plugin logic goes here...
            //editor.addCommand('myimage', new CKEDITOR.dialogCommand('myimage', {
            //    allowedContent: 'iframe[!width,!height,!src,!frameborder,!allowfullscreen]; object param[*]'
            //}));
            editor.addCommand("myimage", {
                allowedContent: "img[src,alt,width,height]{*}",
                exec: function (edt) {
                    _objEdtior = editor;

                    var element = editor.document.createElement('img');

                    var w = window.open("/FileManager/Index", "File manager", "width = 950, height = 600");

                    w.callback = function (result) {
                        w.close();

                        if (!result || !result.Result) {
                            alert("Không có file nào được chọn!");

                            return;
                        }

                        if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.FullPath) == null) {
                            alert("Bạn cần chọn hình ảnh");

                            return;
                        }

                        element.setAttribute("src", result.FullPath);
                        editor.insertElement(element);
                    };
                }
            });
            editor.ui.addButton('myimage',
                {
                    label: 'Hình ảnh',
                    toolbar: 'insert',
                    command: 'myimage',
                    icon: this.path + 'images/icon.png'
                });
        }
    });
})();

/**
 * Author: [2016.03.23][DucMV]
 * Upload file to server
 * @formData : form data to upload
 * @status : status for progress
 * @domainUpload : name of domain to upload
*/
function sendFileToServer(formData, status, domainUpload) {
    var uploadURL = domainUpload + '/UploadHandler.php'; //Upload URL
    var extraData = {}; //Extra Data.
    var jqXHR = $.ajax({
        xhr: function () {
            var xhrobj = $.ajaxSettings.xhr();
            if (xhrobj.upload) {
                xhrobj.upload.addEventListener('progress', function (event) {
                    var percent = 0;
                    var position = event.loaded || event.position;
                    var total = event.total;
                    if (event.lengthComputable) {
                        percent = Math.ceil(position / total * 100);
                    }
                    //Set progress
                    status.setProgress(percent);
                }, false);
            }
            return xhrobj;
        },
        url: uploadURL,
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: formData,
        success: function (data) {
            status.setProgress(100);
            status.hideProgressbar();
            var convertJsonData = JSON.parse(data);
            SetImageToCkEditor("http://img1.banxehoi.com/" + convertJsonData.OK);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            console.log('ERRORS: ' + textStatus);
        },
        complete: function (data) {
            CloseModal();
        }
    });

}
/**
 * Author: [2016.03.23][DucMV]
 * Create progress bar
 * @obj : The element container progress upload
*/
function createStatusbar(obj) {
    this.statusbar = $("<div class='statusbar'></div>").appendTo(obj);
    this.filename = $("<div class='filename'></div>").appendTo(this.statusbar);
    this.size = $("<div class='filesize'></div>").appendTo(this.statusbar);
    this.progressBar = $("<div class='progressBar'><div></div></div>").appendTo(this.statusbar);
    //obj.after(this.statusbar);

    this.setFileNameSize = function (name, size) {
        var sizeStr = "";
        var sizeKB = size / 1024;
        if (parseInt(sizeKB) > 1024) {
            var sizeMB = sizeKB / 1024;
            sizeStr = sizeMB.toFixed(2) + " MB";
        }
        else {
            sizeStr = sizeKB.toFixed(2) + " KB";
        }

        this.filename.html(name);
        this.size.html(sizeStr);
    }
    this.setProgress = function (progress) {
        var progressBarWidth = progress * this.progressBar.width() / 100;
        this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "% ");
    }
    this.hideProgressbar = function () {
        this.statusbar.remove();
    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Handle upload file
 * @files : The files object to upload
 * @obj : The element container progress upload
 * @tokenString : token to upload
 * @domainUpload : domain name to upload
*/
function handleUpload(files, obj, tokenString, domainUpload) {
    for (var i = 0; i < files.length; i++) {
        var fd = new FormData();
        fd.append('id', 'TTR');
        fd.append('fileToUpload', files[i]);
        fd.append('project', 'banxehoi');
        fd.append('UploadType', 'upload');
        fd.append('StringDecypt', tokenString);
        fd.append('submit', 'Upload Image');
        //fd.append('file', files[i]);

        var status = new createStatusbar(obj); //Using this we can set progress.
        status.setFileNameSize(files[i].name, files[i].size);
        sendFileToServer(fd, status, domainUpload);

    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Read file object and preview
 * @file : The file object to upload
*/
function readImage(file) {
    if (file) {
        $("#dropzone-img").addClass("deactive");
        $("#dragandrophandler p").addClass("deactive");
        $("#dropzone-preview").addClass("activePreview");
        if (window.FileReader) {
            var fr = new FileReader();
            fr.onload = function (e) {
                $('#imagePreview').attr("src", e.target.result);
            };
            fr.readAsDataURL(file);
        } else {
            $("#dropzone-preview").html("<b>Browser do not support for preview. Upload now...</b>");
        }
    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Refresh input data control
 * @file : The file object to upload
*/
function RefreshUpload() {
    var control = $("#user-input-file");
    control.replaceWith(control.val('').clone(true));
    _file = null;
}
/**
 * Author: [2016.03.23][DucMV]
 * Upload to preview
 * @file : The file object to upload
*/
function UploadPreview(file) {
    var files = file ? file : document.getElementById('user-input-file').files;
    if (files.length > 0) {
        _file = files;
        for (var i = 0; i < files.length; i++) {
            readImage(files[i]);
        }
    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Upload to server
 * @tokenString : token string to upload
 * @domainUpload : domain name to upload
*/
function Upload(tokenString, domainUpload) {
    var obj = $("#statusbarBox");
    var files = _file ? _file : document.getElementById('user-input-file').files;
    if (files.length > 0) {
        handleUpload(files, obj, tokenString, domainUpload);
        RefreshUpload();
    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Reset modal upload when close modal
*/
function ResetModalUpload() {
    $("#dropzone-img").removeClass("deactive");
    $("#dragandrophandler p").removeClass("deactive");
    $("#dropzone-preview").removeClass("activePreview");
    RefreshUpload();
}
/**
 * Author: [2016.03.23][DucMV]
 * Close modal
*/
function CloseModal() {
    $("#uploadModal").removeClass("activeModal");
    ResetModalUpload();
}
/**
 * Author: [2016.03.23][DucMV]
 * Set image preivew to ekeditor
*/
var SetImageToCkEditor = function (strValue) {
    var imgHtml = '<a class="fancybox" href="' + strValue + '" data-fancybox-group="gallery"> <img src="' + strValue + '" style="margin: 0 auto;display: block;" /></a>';
    _objEdtior.insertHtml(imgHtml);
};

var callBackChoseFileCK = function (result) {
    var element = _objEdtior.document.createElement('img');
    if (!result) {
        alert("Không có file nào được chọn!");

        return;
    }
    if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result) == null) {
        alert("Bạn cần chọn hình ảnh");

        return;
    }
    element.setAttribute("src", result);
    _objEdtior.insertElement(element);
    fmwindow.close();
}

$(document).ready(function () {
    //Hide modal
    $("#closeModal").click(function () {
        CloseModal();
    });

    //Drag and drop file
    var obj = $("#dragandrophandler");
    obj.click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        $("#user-input-file").click();
    });
    obj.on('dragenter', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $(this).css('border', '2px solid #0B85A1');
    });
    obj.on('dragover', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
    obj.on('drop', function (e) {

        $(this).css('border', '2px dotted #0B85A1');
        e.preventDefault();
        var files = e.originalEvent.dataTransfer.files;
        var objStatusbar = $("#statusbarBox");
        //We need to send dropped files to preview
        UploadPreview(files);
    });
    $(document).on('dragenter', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
    $(document).on('dragover', function (e) {
        e.stopPropagation();
        e.preventDefault();
        obj.css('border', '2px dotted #0B85A1');
    });
    $(document).on('drop', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });

});