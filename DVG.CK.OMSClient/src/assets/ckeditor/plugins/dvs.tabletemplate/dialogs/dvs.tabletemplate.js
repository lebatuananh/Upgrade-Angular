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

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "txtWidth", value)
                            	}
                            },
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
                            //,
                            //{
                            //    type: 'checkbox',
                            //    id: 'chkCarPriceTemplate',
                            //    label: 'Use car price template',
                            //    setup: function (e) {
                            //        var value = e.getAttribute("data-use-default");

                            //        if (!value || value == "false") {
                            //            return;
                            //        }

                            //        CKEDITOR.dialog.getCurrent().setValueOf("tab-table-dialog", "chkCarPriceTemplate", value)
                            //    }
                            //}
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
                || element.getAttribute("class") != "divresponsive") {
				element = editor.document.createElement('div');
				this.insertMode = true;
			} else {
				this.insertMode = false;

				this.setupContent(element);
			}
			element.setAttribute("class", "divresponsive");
			this.element = element;
		},
		buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
		onOk: function (e) {
			var dialog = this;
			var width = dialog.getValueOf('tab-table-dialog', 'txtWidth');
			var column = dialog.getValueOf('tab-table-dialog', 'txtColumn');
			var row = dialog.getValueOf('tab-table-dialog', 'txtRow');
			if (parseInt(width) < 1) {
				alert("Please input width > 0");
				return false;
			}
			if (parseInt(column) < 1) {
				alert("Please input column > 0");
				return false;
			}
			if (parseInt(row) < 1) {
				alert("Please input column > 0");
				return false;
			}
			var _class = dialog.getValueOf('tab-table-dialog', 'txtClass');
			var style = dialog.getValueOf('tab-table-dialog', 'txtStyle');
			//var useCarPriceTemplate = dialog.getValueOf('tab-table-dialog', 'chkCarPriceTemplate');
			var element = dialog.element;
			var template = "<table class='cke_show_border " + _class + "' style='" + style + "' width='" + width + "'>";

			template += createTrHead("", column, "");
			template += createTr("", column, "");
			for (var i = 0; i < row - 2; i++) {
				template += createTr("", column, "");
			}

			template += "</table>";

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

	var template = "<tr " + _class + "><td>" + text + "</td>";

	for (var i = 0; i < column - 1; i++) {
		template += "<td>&nbsp;</td>";
	}

	return template + "</tr>";
}
function createTrHead(text, column, _class) {
	if (!text) {
		text = "&nbsp;";
	}

	if (_class) {
		_class = "class = '" + _class + "'";
	}

	var template = "<tr " + _class + "><th>" + text + "</th>";

	for (var i = 0; i < column - 1; i++) {
		template += "<th>&nbsp;</th>";
	}

	return template + "</tr>";
}