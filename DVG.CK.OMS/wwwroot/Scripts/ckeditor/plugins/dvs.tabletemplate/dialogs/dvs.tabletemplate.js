CKEDITOR.dialog.add('TableTemplateDialog', function (editor) {
    return {
        title: 'Table template',
        minWidth: 350,
        minHeight: 100,
        contents: [
					{
					    id: 'tab-table-dialog',
					    padding: 0,
					    height: 250,
					    width: 350,
					    elements:
						[
                            //{
                            //    type: 'text',
                            //    id: 'txtWidth',
                            //    label: 'Width',
                            //    validate: CKEDITOR.dialog.validate.notEmpty("Width field cannot be empty"),
                            //    setup: function (e) {
                            //        var value = e.getAttribute("data-width");

                            //        if (!value) {
                            //            return;
                            //        }

                            //        CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtWidth", value)
                            //    }
                            //},
                            {
                                type: 'text',
                                id: 'txtColumn',
                                label: 'Column',
                                validate: CKEDITOR.dialog.validate.notEmpty("Column field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-column");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtColumn", value)
                                }
                            },
                            {
                                type: 'text',
                                id: 'txtRow',
                                label: 'Row',
                                validate: CKEDITOR.dialog.validate.notEmpty("Row field cannot be empty"),
                                setup: function (e) {
                                    var value = e.getAttribute("data-row");

                                    if (!value) {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtRow", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtClass", value)
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

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtStyle", value)
                                }
                            }
                            ,
                            {
                                type: 'checkbox',
                                id: 'chkDefaultTemplate',
                                label: 'Use default template',
                                setup: function (e) {
                                    var value = e.getAttribute("data-use-default");

                                    if (!value || value == "false") {
                                        return;
                                    }

                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "chkDefaultTemplate", value)
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
                element = element.getAscendant('table', true);
            }

            if (!element || element.getName() != 'table') {
                element = editor.document.createElement('table');
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
            //var width = dialog.getValueOf('tab-table-dialog', 'txtWidth');
            var column = dialog.getValueOf('tab-table-dialog', 'txtColumn');
            var row = dialog.getValueOf('tab-table-dialog', 'txtRow');
            var _class = dialog.getValueOf('tab-table-dialog', 'txtClass');
            var style = dialog.getValueOf('tab-table-dialog', 'txtStyle');
            var useDefault = dialog.getValueOf('tab-table-dialog', 'chkDefaultTemplate');
            var element = dialog.element;
            var template = "<table>";

            if (useDefault) {
                template += createTr("", column, "trhead highlight");
                template += createTr("", column, "trprice");
                template += createTr("Xuất xứ", column, "highlight");
                template += createTr("Dáng xe", column);
                template += createTr("Số chỗ ngồi", column, "highlight");
                template += createTr("Số cửa", column);
                template += createTr("Kiểu động cơ", column, "highlight");
                template += createTr("Dung tích động cơ", column);
                template += createTr("Công suất cực đại", column, "highlight");
                template += createTr("Moment xoắn cực đại", column);
                template += createTr("Hộp số", column, "highlight");
                template += createTr("Kiểu dẫn động", column);
                template += createTr("Thể tích thùng nhiên liệu", column, "highlight");
                template += createTr("Kích thước tổng thể (mm)", column);
                template += createTr("Chiều dài cơ sở (mm)", column, "highlight");
                template += createTr("Bán kính vòng quay tối thiểu (m)", column);
                template += createTr("Hệ thống treo trước", column, "highlight");
                template += createTr("Hệ thống treo sau", column);
                template += createTr("Hệ thống phanh trước", column, "highlight");
                template += createTr("Hệ thống phanh sau", column);
                template += createTr("Thông số lốp", column, "highlight");
                template += createTr("Mâm xe", column);
                template += createTr("Đời xe", column, "highlight");
            } else {
                template += createTr("", column, "trhead highlight");
                template += createTr("", column, "trprice");

                for (var i = 0; i < row - 2; i++) {
                    template += createTr("", column, i % 2 == 0 ? "highlight" : "");
                }
            }

            template += "</table>";  
            
            element.setAttribute("data-column", column);
            element.setAttribute("data-row", row);
            element.setAttribute("data-class", _class);
            element.setAttribute("class", "cke_show_border tskt " + _class);
            element.setAttribute("data-use-default", useDefault);
            element.setAttribute("style", style);
            element.setAttribute("data-style", style);
            //element.setAttribute("data-width", width);
            element.setHtml(template);
            dialog.commitContent(element);

            if (dialog.insertMode) {
                editor.insertElement(element);
            }
        }
    };
});

function createTr(text, column, _class) {
    if (!text) {
        text = "&nbsp;";
    }

    if (_class) {
        _class = "class = '" + _class + "'";
    }

    var template = "<tr " + _class + "><td class = 'first'>" + text + "</td>";

    for (var i = 0; i < column - 1; i++) {
        template += "<td>&nbsp;</td>";
    }

    return template + "</tr>";
}