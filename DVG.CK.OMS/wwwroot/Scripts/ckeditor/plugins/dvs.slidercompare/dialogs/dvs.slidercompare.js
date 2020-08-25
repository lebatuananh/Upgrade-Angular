CKEDITOR.dialog.add('SliderCompareDialog', function (editor) {
    return {
        title: 'Slider compare',
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
                element = element.getAscendant('div', true);
            }

            if (!element
                || element.getName() != 'div'
                || element.getAttribute("data-type") != "slider-compare") {
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
            var element = dialog.element;
            var width = dialog.getValueOf('tab-config', 'txtWidth');
            var height = dialog.getValueOf('tab-config', 'txtHeight');
            var _class = dialog.getValueOf('tab-config', 'txtClass');
            var dataStyle = dialog.getValueOf('tab-config', 'txtStyle');
            var style = "";

            if (_class) {
                element.setAttribute("class", _class);
            }
            
            if (width) {
                style += "width: " + width + "px;";
            }

            if (height) {
                style += "height: " + height + "px;";
            }

            style += "border: 1px solid #CCC; text-align: center;";
            style += dataStyle;
                        
            element.setAttribute("data-type", "slider-compare");
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("data-class", _class);
            element.setAttribute("data-style", dataStyle);            
            element.setAttribute("style", style);
            editor.insertElement(element);
        }
    };
});