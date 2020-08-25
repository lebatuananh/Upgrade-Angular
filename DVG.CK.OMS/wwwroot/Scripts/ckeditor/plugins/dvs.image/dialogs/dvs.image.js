CKEDITOR.dialog.add('ImageDialog', function (editor) {
    return {
        title: 'Image',
        minWidth: 500,
        minHeight: 250,
        contents: [
					{
					    id: 'tab-config',
					    label: "Config",
					    padding: 0,
					    height: 250,
					    width: 500,
					    elements:
						[
                            {
                                type: 'button',
                                id: 'btnUpload',
                                label: 'Upload image',
                                onClick: function () {
                                    var w = window.open("/FileManager/Default.aspx?ac=article", "File manager", "width = 950, height = 600");
                                    
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

                                        CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtImageLink", result.FullPath);
                                    };
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtImageLink',
                                label: 'Link image',
                                validate: CKEDITOR.dialog.validate.notEmpty("Link image field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("src");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtImageLink", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtTitle',
                                label: 'Title',
                                validate: CKEDITOR.dialog.validate.notEmpty("Trường Title không được để trống"),
                                setup: function (e) {
                                    var value = e.getAttribute("title");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtTitle", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtAlt',
                                label: 'Alt',
                                validate: CKEDITOR.dialog.validate.notEmpty("Trường Alt không được để trống"),
                                setup: function (e) {
                                    var value = e.getAttribute("alt");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtAlt", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtWidth", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtHeight", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtClass',
                                label: 'Class',
                                setup: function (e) {
                                    var value = e.getAttribute("data-class");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtClass", value)
                                }
                            },
                            {
                                type: 'textarea',
                                id: 'txtStyle',
                                label: 'Style',
                                setup: function (e) {
                                    var value = e.getAttribute("data-style");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtStyle", value)
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
                element = element.getAscendant('img', true);
            }

            if (!element
                || element.getName() != 'img') {
                element = editor.document.createElement('img');
                this.insertMode = true;
            } else {
                this.insertMode = false;

                this.setupContent(element);
            }

            this.element = element;
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var dialog = this;
            var imageLink = dialog.getValueOf('tab-config', 'txtImageLink');            
            var width = dialog.getValueOf('tab-config', 'txtWidth');
            var height = dialog.getValueOf('tab-config', 'txtHeight');
            var _class = dialog.getValueOf('tab-config', 'txtClass');
            var dataStyle = dialog.getValueOf('tab-config', 'txtStyle');
            var title = dialog.getValueOf('tab-config', 'txtTitle');
            var alt = dialog.getValueOf('tab-config', 'txtAlt');
            var element = editor.document.createElement('img');
            var style = "";

            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(imageLink) == null) {
                alert("Bạn chưa chọn hình ảnh");

                return;
            }

            if (_class) {
                element.setAttribute("class", _class);
            }
            
            if (width) {
                style += "width: " + width + "px;";
            }

            if (height) {
                style += "height: " + height + "px;";
            }

            style += dataStyle;
                        
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("data-class", _class);
            element.setAttribute("data-style", dataStyle);
            element.setAttribute("title", title);
            element.setAttribute("alt", alt);
            element.setAttribute("src", imageLink);
            element.setAttribute("style", style);
            editor.insertElement(element);
        }
    };
});