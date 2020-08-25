var _count = 0;
var _uploadUrl = "/News/UploadFile";
var _parentSrc = "http://thcsnguyendu.edu.vn/PDFViewer/?f=";

CKEDITOR.dialog.add('PDFviewerDialog', function (editor) {
    var plugin = CKEDITOR.plugins.get('dvs.pdfviewer');
    CKEDITOR.document.appendStyleSheet(CKEDITOR.getUrl(plugin.path + 'dialogs/pdfviewer.css'));
    return {
        title: 'PDF viewer',
        minWidth: 350,
        minHeight: 260,
        contents: [
            {
                id: 'tab-doc',
                //padding: 0,
                minWidth: 350, minHeight: 260,
                elements:
                [
                    {
                        type: 'hbox',
                        widths: ['30%', "70%"],
                        //style: 'margin-top: 10px',
                        children: [
                            {
                                type: 'button',
                                id: 'btnUpload',
                                label: 'Upload tài liệu',
                                onClick: function () {
                                    var dialog = CKEDITOR.dialog.getCurrent();
                                    var fileDom = dialog.getContentElement('tab-doc', 'docFile');
                                    var inputFile = document.getElementById($("#" + fileDom.domId + " iframe").attr("id")).contentDocument.getElementById(fileDom.getInputElement().getId());
                                    inputFile.click();
                                    $("#statusbarBox").empty();
                                    $(inputFile).on("change", function () {
                                        if (_count === 0) {
                                            _count++;
                                            uploadData.Upload(this.files, this, function (result, elem) {
                                                RefreshUpload(elem);
                                                _count = 0;
                                                if (result) {
                                                    //Set link to text box
                                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtDocumentLink", result);
                                                }
                                            });
                                        }
                                    });
                                    //var w = window.open("/FileManager/Default.aspx?ac=subtitle", "File manager", "width = 950, height = 600");
                                    //var acceptFileTypes = /(\.|\/)(pdf)$/i;
                                    //w.callback = function (result) {
                                    //    if (!result || !result.Result) {
                                    //        alert("Không có file nào được chọn!");

                                    //        return;
                                    //    }

                                    //    if (acceptFileTypes.exec(result.Name) == null) {
                                    //        alert("Bạn cần chọn tệp tài liệu định dạng pdf");

                                    //        return;
                                    //    }

                                    //    CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtDocumentLink", result.FullPath);
                                    //    w.close();
                                    //};
                                }
                            },
                            {
                                type: 'html',
                                html: '<div id="statusbarBox"></div>'
                            }
                        ]
                    },
                    {
                        type: 'file',
                        id: 'docFile',
                        style: 'opacity: 0; filter:alpha(opacity=0);border: none;padding: 0;margin: 0;width: 0;height: 0;'
                    },
                    {
                        type: 'text',
                        id: 'txtDocumentLink',
                        label: 'Link tài liệu(tệp định dạng pdf)',
                        validate: CKEDITOR.dialog.validate.notEmpty("Link pdf field cannot be empty"),
                        setup: function (e) {
                            var value = e.getAttribute("data-pdf");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtDocumentLink", value);
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtWidth',
                        label: 'Width',
                        setup: function (e) {
                            var value = e.getAttribute("data-width");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtWidth", value);
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtHeight',
                        label: 'Height',
                        setup: function (e) {
                            var value = e.getAttribute("data-height");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtHeight", value);
                        }
                    }
                ]
            }
        ],
        onLoad: function () {
        },
        onShow: function () {
            //var selection = editor.getSelection();
            //var element = selection.getStartElement();
            this.fakeImage = this.iframeNode = null;
            var selectedElem = this.getSelectedElement();
            selectedElem &&
                (selectedElem.data("cke-real-element-type") && "iframe" == selectedElem.data("cke-real-element-type")) &&
                (
                    this.fakeImage = selectedElem,
                    this.iframeNode = selectedElem = editor.restoreRealElement(selectedElem),
                    this.setupContent(selectedElem)
                );

            if (selectedElem) {
                var documentLink = $(selectedElem).attr('src');
                var width = $(selectedElem).attr('width');
                var height = $(selectedElem).attr('height');
                CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtDocumentLink", documentLink);
                CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtWidth", width);
                CKEDITOR.dialog.getCurrent().setValueOf("tab-doc", "txtHeight", height);
            }
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var h = this.getValueOf('tab-doc', 'txtHeight');
            var w = this.getValueOf('tab-doc', 'txtWidth');
            var width = w ? w : "";
            var height = h ? h : "600";
            var documentLink = this.getValueOf('tab-doc', 'txtDocumentLink');
            if (documentLink.indexOf(_parentSrc) == -1) {
                documentLink = _parentSrc + documentLink;
            }
            var pdfId = "pdfviewer" + new Date().getTime();
            var iframe;
            iframe = this.fakeImage ? this.iframeNode : new CKEDITOR.dom.element("iframe");
            this.commitContent(iframe);
            iframe.setAttribute("id", pdfId);
            iframe.setAttribute("src", documentLink);
            iframe.setAttribute("frameborder", "0");
            iframe.setAttribute("scrolling", "no");
            if (width) {
                iframe.setAttribute("width", width);
            } else {
                iframe.removeAttribute("width");
            }
            iframe.setAttribute("height", height);

            iframe = editor.createFakeElement(iframe, "cke_iframe", "iframe", !0);

            if (this.fakeImage)
                (iframe.replace(this.fakeImage), editor.getSelection().selectElement(iframe));
            else
                editor.insertElement(iframe);
        }
    };
});

var uploadData = {
    Tokenkey: "",
    Init: function () {
    },
    Upload: function (files, elem, callBack) {
        var obj = $("#statusbarBox");
        if (files.length > 0) {
            handleUpload(files, obj, this.Tokenkey, function (result) {
                if (!result && result != 1) {
                    setTimeout(function () {
                        $("#statusbarBox").empty().html("<span style='color:red'>Đã xảy ra lỗi khi tải lên</span>");
                    }, 1000);
                }
                result = result == 1 ? "" : result;
                callBack(result, elem);
                RefreshUpload(elem);
            });

        }
    }
}

/**
 * Author: [2016.03.23][DucMV]
 * Upload file to server
 * @formData : form data to upload
 * @status : status for progress
 * @domainUpload : name of domain to upload
*/
function sendFileToServer(formData, status, callBack) {
    var uploadURL = _uploadUrl;//domainUpload + '/UploadHandler.php'; //Upload URL

    var progressbar = $("#statusbarBox .progressBar");
    var pendingProgress =
        '<div id="squaresWaveG">' +
            '<div id="squaresWaveG_1" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_2" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_3" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_4" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_5" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_6" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_7" class="squaresWaveG"></div>' +
            '<div id="squaresWaveG_8" class="squaresWaveG"></div></div>';

    var ajax = new XMLHttpRequest();
    ajax.upload.addEventListener("progress",
        function (event) {
            var percent = 0;
            var position = event.loaded || event.position;
            var total = event.total;
            if (event.lengthComputable) {
                percent = Math.ceil(position / total * 100);
            }
            //Set progress
            status.setProgress(percent);
            if (percent === 100) {
                progressbar.empty().removeClass('progressBar').addClass('progressPending').html(pendingProgress);
            }
        }, false);
    ajax.addEventListener("load", function (data) {
        status.setProgress(100);
        status.hideProgressbar();
        var result = JSON.parse(data.target.response);
        callBack(result);
    }, false);
    ajax.addEventListener("error", errorHandler, false);
    ajax.addEventListener("abort", abortHandler, false);
    ajax.open("POST", uploadURL, true);
    ajax.send(formData);
}

function completeHandler(event) {
    _("status").innerHTML = event.target.responseText;
    _("progressBar").value = 0;
}
function errorHandler(event) {
}
function abortHandler(event) {
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
function handleUpload(files, obj, tokenString, callBack) {
    var acceptFileTypes = /(\.|\/)(pdf)$/i;
    for (var i = 0; i < files.length; i++) {
        if (acceptFileTypes.exec(files[i].name) == null) {
            $("#statusbarBox").empty().html("<span style='color:red'>Tệp tải lên không hợp lệ.</span>");
            callBack(1);
            break;
        } else {
            var fd = new FormData();
            fd.append('id', 'TTR');
            fd.append('file1', files[i]);
            //fd.append('project', uploadProject);
            fd.append('UploadType', 'upload');
            fd.append('StringDecypt', tokenString);
            fd.append('submit', 'Upload Image');

            var status = new createStatusbar(obj); //Using this we can set progress.
            status.setFileNameSize(files[i].name, files[i].size);
            sendFileToServer(fd, status, function (result) {
                callBack(result);
            });
        }
    }
}

/**
 * Author: [2016.03.23][DucMV]
 * Refresh input data control
 * @file : The file object to upload
*/
function RefreshUpload(elem) {
    var control = $(elem);
    control.replaceWith(control.val('').clone(true));
    _file = null;
}