CKEDITOR.config.customConfig = "/Scripts/ckeditor/common.js";
CKEDITOR.on('instanceReady', function (e) {
    var editor = e.editor;
    editor.on('paste', function (evnt) {
        var data = evnt.data;
        //var regex = new RegExp(/<a[^>]*href=(["'][^"']+["'])[^>]*>(((?!<\/a>).)*)<\/a>/ig);
        var reg = /<a[^>]*href=["']([^"']+)["'][^>]*>(((?!<\/a>).)*)<\/a>/ig;
        var content = data.dataValue.replace(reg, function myFunction(x, x1, x2) { return '<a href="' + x1 + '" class="blue-clr" rel="nofollow" target="_blank">' + x2 + '</a>'; });

        content = removeUnnecessaryTags(content);
        //content = urlify(content);
        data.dataValue = content;
        data.type = 'html';
    });
});

var removeUnnecessaryTags = function (data) {
    if (data == null || data === '') return '';
    //console.log('removeUnnecessaryTags');
    var splitReg = /(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:p\s+[^>]*|p|div[^>]*)>(?:\s|\t|\n|\r|&nbsp;)*/gi,
				removeTagReg = /(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:(?:p\s+|div|table|tbody|tr|th|td|ul|li|br|h1|h2|a|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*|p)>(?:\s|\t|\n|\r|&nbsp;)*|<\!--[\S\s]*-->/gi,
                styleTagReg = /<\/?(?:(?:p\s+|em|u|b|strong|<i\/?>|font|span|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*)>/gi,
				trimReg = /^(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+|(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+$/gi,
				photoReg = /<img[^>]+>/gi,
				beginValidTag = /^(?:\s|\t|\n|\r|&nbsp;)*<(?:div[^>]*|p\s+[^>]*|p|h\d[^>]*)>/gi,
				paragraphs,
		        hasTag = /<\/?[a-z0-9]+[^>]*>/gi;
    data = data.replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, '');
    data = data.replace(/(?:<br>|<br >|<br\/>|<br \/>)/gi, '<p>&nbsp;</p>');
    //data = data.replace(/font-family:[^;]+/gi, '');
    //data = data.replace(/font-size:[^;]+/gi, '');
    //data = data.replace(/font:[^;]+/gi, '');
    //data = data.replace(/line-height:[^;]+/gi, '');
    paragraphs = data.split(splitReg);
    if (paragraphs && paragraphs.length > 1) {
        for (var i = 0; i < paragraphs.length; ++i) {
            paragraphs[i] = paragraphs[i].replace(trimReg, '').trim();
            if (!paragraphs[i] || /^(?:(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:span|div|p|br)+[^>]*>(?:\s|\t|\n|\r|&nbsp;)*)+$/gi.test(paragraphs[i])) {
                paragraphs[i] = '';
            } else {
                // paragraphs[i] = paragraphs[i].replace(/<\/?(?:strong|b)>*|<\!--[\S\s]*-->/gi, ' ');
                paragraphs[i] = paragraphs[i].replace(removeTagReg, ' ');
                paragraphs[i] = paragraphs[i].replace(styleTagReg, '');
                if (paragraphs[i] !== '') {
                    if (!beginValidTag.test(paragraphs[i]) && !photoReg.test(paragraphs[i])) {
                        paragraphs[i] = "<p>" + paragraphs[i] + "</p>";
                    }
                    paragraphs[i] = paragraphs[i].replace(photoReg, function (m) {
                        return '<div class="media_plugin" type="photo" style="text-align:center;"><div>' + m.replace(/style\s*=\s*[\"\'][^\"\']*[\"\']/gi, '') + '</div><div></div></div><p>&nbsp;</p>';
                    });
                }
            }
        }
        data = paragraphs.join('').replace(trimReg, '');
    }
    return data;
};

function urlify(text) {
    var urlRegex = /(https?:\/\/[^\s<"']+)/gi;
    return text.replace(urlRegex, function (url) {
        return '<a href="' + url + '" class="blue-clr" rel="nofollow" target="_blank">' + url + '</a>';
    });
}

var Const = (function ($) {
    var message = {
        invalid: 'Dữ liệu không hợp lệ',
        contentInvalid: 'Nội dung không hợp lệ',
        avartaEmpty: 'Bạn chưa chọn ảnh đại diện',
        listUpdated: 'Danh sách đã được cập nhật',
        itemBanned: 'Bài đã được gõ bỏ',
        checked: 'Bạn phải check vào ô này',
        empty: 'Bạn không được để trống',
        update_succsess: 'Dữ liệu cập nhật thành công',
        update_error: 'Cập nhật không thành công!',
        max: 'input is too long',
        number_min: 'too low',
        number_max: 'too high',
        url: 'invalid URL',
        number: 'not a number',
        password_repeat: 'mật khẩu không đúng',
        repeat: 'không đúng',
        uncomplete: 'Dữ liệu nhập chưa hoàn thành',
        select: 'Please select an option'
    };
    return {
        message: message
    }
})(jQuery);

$(document).ready(function () {
    $(".datepicker-inp").datetimepicker({
        format: 'DD/MM/YYYY HH:mm',
        locale: "vi",
        keepOpen: false
    });

    $(".datepicker-short").datetimepicker({
        format: 'DD/MM/YYYY',
        locale: "vi",
        keepOpen: false
    });
    loadEditors();
});

$.fn.isCKEDITOREmpty = function () {
    var idElem = $(this).attr("id");
    var messageLength = CKEDITOR.instances[idElem].getData().replace("<img", "||img").replace("<iframe", "||iframe").replace(/<[^>]*>/gi, '').replace(/&nbsp;/g, '').trim().length;
    if (!messageLength) return true;
    return false;
}

function wordCount(val) {
    var wom = val.match(/\S+/g);
    return {
        charactersNoSpaces: val.replace(/\s+/g, '').length,
        characters: val.length,
        words: wom ? wom.length : 0,
        lines: val.split(/\r*\n/).length
    };
}

function PostAjax(url, data, callback) {
    var uploadURL = url; //Upload URL
    var jqXHR = $.ajax({
        url: uploadURL,
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        processData: false,
        cache: false,
        data: data,
        async: false,
        success: function (resultData) {
            callback(resultData);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            console.log('ERRORS: ' + textStatus);
        },
        complete: function (resultData) {

        }
    });
}

function loadEditors() {
    var $editors = $("textarea.ckeditor");
    if ($editors.length) {
        $editors.each(function () {
            var editorID = $(this).attr("id");
            var isShortEdit = $(this).hasClass("ckeditor-short");
            var instance = CKEDITOR.instances[editorID];
            if (instance) { instance.destroy(true); }
            if (isShortEdit) {
                CKEDITOR.replace(editorID, { customConfig: '/Scripts/ckeditor/config-short.js' });
            } else {
                CKEDITOR.replace(editorID);
            }
        });
    }
}

function UnFormatNumber(currency) {
    var number = 0;
    if (currency != null) {
        currency = currency.toString().replace(/\./g, "");
        if ((currency.match(/,/g) || []).length > 1) {
            currency = currency.toString().replace(/,/g, "");
        } else {
            currency = currency.toString().replace(/,/g, ".");
        }
        number = Number(currency.replace(/[^0-9\.-]+/g, ""));
    }
    return number;
}

$.fn.FormatNumberInt = function () {
    $(this).on("keyup", function (event) {
        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var regexNumber = /^[0-9]+$/i;
        var val = this.value;
        val = val.replace(/[^\d.-]+/g, "");
        this.value = "";
        val += '';
        var x = val.split('.');
        var x1 = x[0];

        if (!regexNumber.test(x1)) {
            return;
        }
        if (x1.length > 1) {
            x1 = parseInt(x1.replace(/^0/, '')).toString();
        }
        this.value = x1;
    });
}

$.fn.FormatCurrency = function () {
    $(this).on("keyup", function (event) {
        var regexNumber = /^[0-9]+$/i;

        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var val = this.value;
        val = val.replace(/[^\d,-]/g, "");
        this.value = "";
        val += '';
        var x = val.split(',');
        var x1 = x[0];
        var x2 = x.length > 1 ? ',' + x[1] : '';

        var rgx = /(\d+)(\d{3})/;
        if (!regexNumber.test(x1) || (x2 !== "," && x2.replace(/,/g, "") !== "" && !regexNumber.test(x2.replace(/,/g, "")))) {
            return;
        }
        if (x1.length > 1) {
            x1 = parseInt(x1.replace(/^0/, '')).toString();
        }
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + '.' + '$2');
        }
        this.value = x1 + x2;//.replace(/^[0,]/g, "")
    });
}

$.fn.FormatNumberNegativeInt = function () {
    $(this).on("keyup", function () {
        var regexNumber = /^[0-9\-]+$/i;

        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var val = this.value;
        val = val.replace(/[^\d,-]/g, "");
        this.value = "";
        val += '';
        var x = val.split(',');
        var x1 = x[0];
        var x2 = x.length > 1 ? ',' + x[1] : '';

        var rgx = /(\d+)(\d{3})/;
        if (!regexNumber.test(x1) || (x2 !== "," && x2.replace(/,/g, "") !== "" && !regexNumber.test(x2.replace(/,/g, "")))) {
            return;
        }
        if (x1.length > 1) {
            x1 = parseInt(x1.replace(/^0/, '')).toString();
        }
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + '.' + '$2');
        }
        this.value = x1 + x2;//.replace(/^[0,]/g, "")
    });
}


function validFromdateToDate(startdatestring, enddatestring) {
    if (startdatestring == undefined || startdatestring == null || startdatestring == "" ||
		enddatestring == undefined || enddatestring == null || enddatestring == "") {
        return true;
    }
    var startdate = new Date(startdatestring.split("/")[2] + "-" + startdatestring.split("/")[1] + "-" + startdatestring.split("/")[0]);
    var enddate = new Date(enddatestring.split("/")[2] + "-" + enddatestring.split("/")[1] + "-" + enddatestring.split("/")[0]);
    if (startdate > enddate) {
        return false;
    }
    else {
        return true;
    }
}



var UploadMedia = {
    IdInputFile: "",
    ListFile: [],
    IsDisPlayFileInfo: false,
    IsUpdate: false,
    ResponseResult: "",
    OpenFileDialog: function (id) {
        UploadMedia.IdInputFile = id;
        $("#" + id).click();
    },
    Upload: function (token, domain, urlPost, idImg) {
        var files = document.getElementById(UploadMedia.IdInputFile).files;
        var processBarBox = $("#file-upload-box");
        $("#message-box").hide();

        if (files.length > 0) {

            // Kiểm tra file ảnh xem có thoải mãn 1300x400px không
            var _URL = window.URL || window.webkitURL;
            var image;
            image = new Image();
            image.src = _URL.createObjectURL(files[0]);
            image.onload = function () {
                if (this.width < 1300 || this.height < 400) {
                    alert("Ảnh bạn tải lên quá nhỏ, ảnh tối thiểu phải rộng 1300px và cao tối thiểu 400px");
                }
            };

            UploadMedia.HandleUpload(files, processBarBox, token, domain, urlPost, idImg);
        }
    },
    HandleUpload: function (files, processBarBox, token, domain, urlPost, idImg) {
        for (var i = 0; i < files.length; i++) {
            var fd = new FormData();
            fd.append('id', 'TTR');
            fd.append('fileToUpload', files[i]);
            fd.append('project', 'NguyenDu');
            fd.append('UploadType', 'upload');
            fd.append('StringDecypt', token);
            fd.append('submit', 'Upload Image');
            var status = new UploadMedia.CreateStatusbar(processBarBox); //Using this we can set progress.
            status.setFileNameSize(files[i].name, files[i].size);
            UploadMedia.SendAvatarToServer(fd, status, domain, function (response) {
                if (response && response.length > 0) {
                    UploadMedia.ListFile.push(response);

                    // Append button delete file
                    this.deleteFile = $("<div class='button-delete-upload'><i onclick=\"UploadMedia.DeleteUploadFile('" + response + "', this)\" class='fa fa-trash-o'></i></div>").appendTo(status.statusbar);
                    if (UploadMedia.IsUpdate) {
                        UploadMedia.UploadToServer(urlPost, UploadMedia.ListFile[UploadMedia.ListFile.length - 1], idImg);
                    }
                }
            });
        }
    },
    CreateStatusbar: function (obj) {
        if (!UploadMedia.IsDisPlayFileInfo) {
            this.statusbar = $("<div class='statusbar hidden'></div>").appendTo(obj);
        } else {
            this.statusbar = $("<div class='statusbar'></div>").appendTo(obj);
        }
        //var indexFile = UploadMedia.ListFile == null ? 0 : UploadMedia.ListFile.length;
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

            if (progress == 100) {
                this.progressBar.hide();
            }
        }
        this.hideProgressbar = function () {
            this.statusbar.remove();
        }
    },
    SendAvatarToServer: function (formData, status, domainUpload, callBack) {
        var uploadURL = domainUpload + '/UploadHandler.php'; //Upload URL
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
            success: function (dataResult) {
                var convertJsonData = JSON.parse(dataResult);
                //var domainStorageImg = $("#hdDomainImg").val();
                //var imgUrl = domainStorageImg + "/" + convertJsonData.OK;
                var imgUrl = convertJsonData.OK;
                callBack(imgUrl);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // Handle errors here
                console.log('ERRORS: ' + textStatus);
            },
            complete: function (data) {
                status.setProgress(100);
                //status.hideProgressbar();
                UploadMedia.RefreshControl("#" + UploadMedia.IdInputFile);
            }
        });
    },
    UploadToServer: function (urlPost, data, idImg) {
        var dataJson = JSON.stringify({
            avatar: data
        });
        UploadMedia.ResponseResult = data;
        PostAjaxExtention(urlPost, dataJson, null, function (response) {
            if (!response.Success) {
                alert(response.Message);
            } else {
                $("#" + idImg).attr("src", data);
            }
        });
    },
    RefreshControl: function (id) {
        var control = $(id);
        control.replaceWith(control.val('').clone(true));
    },
    DeleteUploadFile: function (filename, elm) {
        if (filename != null && filename != "") {

            if (UploadMedia.ListFile.length > 0) {
                var indexElm = UploadMedia.ListFile.indexOf(filename);
                if (UploadMedia.ListFile[indexElm] != null) {
                    // 1. xóa phần tử tron mảng
                    UploadMedia.ListFile.splice(indexElm, 1);

                    // Xóa phần tử html
                    $(elm).parent().parent().remove();
                }
            }
        }
    }
}

function popupCenter(pageURL, title, w, h) {
    var y = window.top.outerHeight / 2 + window.top.screenY - (h / 2);
    var x = window.top.outerWidth / 2 + window.top.screenX - (w / 2);
    return window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);
}

var fmwindow, fmwindow_txtId, _fileManagementPath, scope_elm, callback_on_elm;
_fileManagementPath = '/FileManagement_5_1_09/';
function chooseFile(scopeelm, i, callback, callBackChoseFile) {
    if (callBackChoseFile == undefined) callBackChoseFile = 'callBackChoseFile';
    callback_on_elm = callback;
    scope_elm = scopeelm;

    var w = 900;
    var h = 580;
    var url = "";
    url = _fileManagementPath + "?hasdomain=true&fileType=images&function=callBackChoseFile&mode=single&share=share&i=" + encodeURIComponent(i);
    openpreview2(url, w, h);
}

function callBackChoseFile(arr) {
    if (!arr || arr == null) {
        alert("Không có file nào được chọn!");
        return;
    }
    if (/(\.|\/)(gif|jpe?g|png|pdf|rar|zip|doc|docx)$/i.exec(arr[0]) == null) {
        alert("Tệp bạn chọn không hợp lệ");
        return;
    }
    str = 'angular.element(document.getElementById("' + scope_elm + '")).scope().' + callback_on_elm + '("' + arr[0] + '")';
    eval(str);
    fmwindow.close();
}

function openpreview2(url, w, h) {
    var winX = 0;
    var winY = 0;
    if (parseInt(navigator.appVersion) >= 4) {
        winX = (screen.availWidth - w) * 0.5;
        winY = (screen.availHeight - h) * 0.5;
    }
    fmwindow = window.open(url, "mywindow" + (new Date).getTime(), "scrollbars,resizable=yes,status=yes, width=" + w + ",height=" + h + ",left=" + winX + ",top=" + winY);
}