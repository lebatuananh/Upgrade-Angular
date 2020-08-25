
CKEDITOR.editorConfig = function (config) {
    config.language = 'vi';
    // config.uiColor = '#AADC6E';
    config.extraPlugins = 'youtube,dvs.image';
    config.removePlugins = 'elementspath';
    config.height = "190px";
    config.allowedContent = true;
    config.toolbar = [
            //{ name: 'links', items: ['-', 'Link', 'Unlink', 'Anchor', 'dvs.image', 'Youtube'] },
            {
                name: 'basicstyles', groups: ['basicstyles', 'cleanup'],
                items: ['Bold', 'Italic', 'Underline', 'TextColor', 'Styles', 'Format', 'FontSize']   //'Strike', 'Subscript', 'Superscript', '-',
            },
            ['NumberedList', 'BulletedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'],
            { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
            ['About']
            //'/',
            
            
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


