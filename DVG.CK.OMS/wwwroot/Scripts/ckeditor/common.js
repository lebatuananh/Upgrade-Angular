
CKEDITOR.editorConfig = function (config) {
    config.language = 'vi';
    // config.uiColor = '#AADC6E';
    config.extraPlugins = 'youtube,myimage,dvs.pdfviewer';//dvs.image
    config.removePlugins = 'elementspath';
    config.height = "450px";
    config.allowedContent = true;
    config.youtube_related = false;
    config.toolbar = [
        { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'NewPage', 'Preview', 'Print', '-', 'Templates', 'PasteFromWord'] },
        //    ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'], ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
        //    { name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'Button', 'ImageButton', 'HiddenField'] },
        //    ['-', 'NumberedList', 'BulletedList'],
            { name: 'links', items: ['-', 'Link', 'Unlink', 'Anchor', 'myimage', 'Youtube', 'dvs.pdfviewer'] },
            { name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar', 'PageBreak', 'Iframe', 'Slideshow'] },//'Smiley',
            { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
            ['About'],
            '/',
        //    ['Blockquote', 'syntaxhighlight', 'bbcodeselector'],
            {
                name: 'basicstyles', groups: ['basicstyles', 'cleanup'],
                items: ['Bold', 'Italic', 'Underline', 'TextColor', 'Styles', 'Format', 'Font', 'FontSize']   //'Strike', 'Subscript', 'Superscript', '-',
            },
            ['NumberedList', 'BulletedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight']
        //    ['Outdent', 'Indent'],
            
    ];

    config.toolbarGroups = [
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
        { name: 'forms' },
        '/',
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
        { name: 'links' },
        { name: 'insert' },
        '/',
        { name: 'styles' },
        { name: 'colors' },
        { name: 'tools' },
        { name: 'others' },
        { name: 'about' }
    ];
    config.startupOutlineBlocks = true;
};


