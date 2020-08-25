CKEDITOR.dialog.add('VideoDialog', function (editor) {
    return {
        title: 'Video',
        minWidth: 300,
        minHeight: 100,
        contents: [
					{
					    id: 'tab-video',
					    padding: 0,
					    height: 250,
					    width: 300,
					    elements:
						[
                            {
                                type: 'button',
                                id: 'btnUpload',
                                label: 'Upload subtitle file',
                                onClick: function () {
                                    var w = window.open("/FileManager/Default.aspx?ac=subtitle", "File manager", "width = 950, height = 600");

                                    w.callback = function (result) {
                                        if (!result || !result.Result) {
                                            alert("Không có file nào được chọn!");

                                            return;
                                        }

                                        if (result.FullPath.lastIndexOf(".srt") == -1) {
                                            alert("Bạn cần chọn tệp subtitle");

                                            return;
                                        }

                                        CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkSubtitle", result.FullPath);
                                        w.close();
                                    };
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtLinkVideo',
                                label: 'Link video',
                                validate: CKEDITOR.dialog.validate.notEmpty("Link video field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-video");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkVideo", value)
                                }
                            },
                            //{
                            //    type: 'file',
                            //    id: 'upload',
                            //    label: editor.lang.image.btnUpload,
                            //    size: 38,
                            //    action: "/ImageManager/UploadFile.ashx"
                            //},
                            //{
                            //    type: 'fileButton',
                            //    id: 'fileId',
                            //    label: editor.lang.image.btnUpload,
                            //    'for': ['tab-video', 'upload'],
                            //    filebrowser: {
                            //        onSelect: function (fileUrl, data) {
                            //            alert('Successfully uploaded: ' + fileUrl);
                            //        }
                            //    }
                            //},                            
                            {
                                type: 'text',
                                id: 'txtLinkSubtitle',
                                label: 'Link subtitle',
                                //validate: CKEDITOR.dialog.validate.notEmpty("Link subtitle field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-subtitle");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkSubtitle", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtWidth',
                                label: 'Width',
                                validate: CKEDITOR.dialog.validate.notEmpty("Width field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-width");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtWidth", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtHeight',
                                label: 'Height',
                                validate: CKEDITOR.dialog.validate.notEmpty("Height field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-height");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtHeight", value)
                                }
                            }
						]
					}
        ],
        onLoad: function () {
            /*fmClient.IsEditor = true;
            fmClient.SelectedControl = editor.name;*/
        },
        onShow: function () {
            var selection = editor.getSelection();
            var element = selection.getStartElement();

            if (element) {
                element = element.getAscendant('div', true);
            }

            if (!element
                || element.getName() != 'div'
                || element.getAttribute("data-type") != "jwplayer") {
                element = editor.document.createElement('div');                    
                this.insertMode = true;
            } else {
                this.insertMode = false;

                this.setupContent(element);
            }
            
            this.element = element;
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            /*var dialog = this;
            var videoLink = dialog.getValueOf('tab-video', 'txtLinkVideo');
            var subtitleLink = dialog.getValueOf('tab-video', 'txtLinkSubtitle');
            var width = dialog.getValueOf('tab-video', 'txtWidth');
            var height = dialog.getValueOf('tab-video', 'txtHeight');
            var jwId = "player" + new Date().getTime();
            var element = dialog.element;//editor.document.createElement('div');
            var template = "<div id='" + jwId + "'><img src = '/assets/img/video.png' alt='video' /></div><script type='text/javascript'>jwplayer('" + jwId + "').setup({ width: " + width + ", height: " + height + ", controlbar: 'bottom', playlist: [{ captions: [ { file: '" + subtitleLink + "', label: 'English', 'default': true } ], file: '" + videoLink + "' }] });</script>";
            element.setAttribute("data-type", "jwplayer");
            element.setAttribute("data-video", videoLink);
            element.setAttribute("data-subtitle", subtitleLink);
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("contenteditable", false);
            element.setAttribute("style", "width: " + width + "px; height: " + height + "px; text-align: center; border: 1px solid #CCC; margin: 5px auto;");
            element.setHtml(template);
            dialog.commitContent(element);

            if (dialog.insertMode) {
                editor.insertElement(element);
            }*/

            var dialog = this;
            var videoLink = dialog.getValueOf('tab-video', 'txtLinkVideo');
            var subtitleLink = dialog.getValueOf('tab-video', 'txtLinkSubtitle');
            var width = dialog.getValueOf('tab-video', 'txtWidth');
            var height = dialog.getValueOf('tab-video', 'txtHeight');
            var jwId = "player" + new Date().getTime();
            var element = dialog.element;
            var template = "<img src = '/content/images/video.png' alt='video' />";
            element.setAttribute("id", jwId);
            element.setAttribute("data-type", "jwplayer");
            element.setAttribute("data-video", videoLink);
            element.setAttribute("data-subtitle", subtitleLink);
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("contenteditable", false);
            element.setAttribute("style", "width: " + width + "px; height: " + height + "px; text-align: center; border: 1px solid #CCC; margin: 5px auto;");
            element.setHtml(template);
            dialog.commitContent(element);

            if (dialog.insertMode) {
                editor.insertElement(element);
            }
        }
    };
});