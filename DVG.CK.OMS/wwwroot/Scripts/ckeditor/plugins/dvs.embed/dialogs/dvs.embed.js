CKEDITOR.dialog.add('EmbedDialog', function (editor) {
    return {
        title: 'Embed',
        minWidth: 350,
        minHeight: 100,
        contents: [
					{
					    id: 'tab-embed',
					    padding: 0,
					    height: 250,
					    width: 350,
					    elements:
						[
                            {
                                type: 'textarea',
                                id: 'txtEmbed',
                                label: 'Embed or link video',
                                validate: CKEDITOR.dialog.validate.notEmpty("Embed field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-video");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-embed", "txtEmbed", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-embed", "txtWidth", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-embed", "txtHeight", value)
                                }
                            }
						]
					}
        ],
        onLoad: function () {
        },
        onShow: function () {
            var selection = editor.getSelection();
            var element = selection.getStartElement();

            if (element) {
                element = element.getAscendant('div', true);
            }

            if (!element
                || element.getName() != 'div'
                || element.getAttribute("data-type") != "embed") {
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
            var dialog = this;
            var embed = dialog.getValueOf('tab-embed', 'txtEmbed');
            var width = dialog.getValueOf('tab-embed', 'txtWidth');
            var height = dialog.getValueOf('tab-embed', 'txtHeight');
            var element = dialog.element;
            var template = "<img src = '/content/images/embed.png' alt='embed' />";
            
            try {
                var je = $(embed);
                var temp;

                if (je.prop("tagName") == "OBJECT") {
                    temp = je.find("embed");

                    if (temp.length > 0) {
                        je = temp;
                    }
                }

                embed = je.attr("src");

                if (!embed) {
                    embed = je.find("param[name='movie']").attr("value");
                }

                if (!embed) {
                    alert("Không thể tách được liên kết video bạn vui lòng tách liên kết bằng tay!");
                    return false;
                }
            } catch (ex) {
            }

            element.setAttribute("data-type", "embed");
            element.setAttribute("data-video", embed);
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