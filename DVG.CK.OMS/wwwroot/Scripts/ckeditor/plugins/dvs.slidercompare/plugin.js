CKEDITOR.plugins.add('dvs.slidercompare', {
    init: function (editor) {
        editor.addCommand('dvs.slidercompare', new CKEDITOR.dialogCommand('SliderCompareDialog'));
        editor.ui.addButton('dvs.slidercompare', {
            label: 'Slider compare',
            command: 'dvs.slidercompare',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.slidercompare.png'
        });

        CKEDITOR.dialog.add('SliderCompareDialog', this.path + 'dialogs/dvs.slidercompare.js');
    }
});