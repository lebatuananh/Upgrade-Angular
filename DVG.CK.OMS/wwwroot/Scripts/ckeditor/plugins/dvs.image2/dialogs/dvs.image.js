
CKEDITOR.dialog.add('ImageDialog', function (editor) {
    return {
        title: 'File Manager',
        minWidth: 650,
        minHeight: 400, 
        contents: [
					{
					    id: 'tab1',
					    padding: 0,
					    height: 410,
                        width: 660,
					    elements:
						[
							{
							    type: 'html',
							    html: '<iframe id="iframeFile" src="/filemanager/fm.aspx" style="border:0; width: 660px !important; height: 410px"></iframe>'
							}
						]
					}
        ],
        onLoad: function () {  
            fmClient.IsEditor = true; 
            fmClient.SelectedControl = editor.name;
        },
        buttons: [], 
        onOk: function () { 
             
        }
    };
});