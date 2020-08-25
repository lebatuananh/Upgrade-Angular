import { environment } from '../../../environments/environment';

export class Const {
  public static readonly MinDate: Date = new Date('0001-01-01T00:00:00');
  public static readonly LoadFileAPI: string = environment.APP_API_URL + '/Common/LoadFile';
  public static readonly UpFileAPI: string = environment.APP_API_URL + '/Common/FileManagerUpload';
  public static readonly MY_DATETIMEPICKER_FORMAT = {
    parseInput: 'DD/MM/YYYY HH:mm',
    fullPickerInput: 'DD/MM/YYYY HH:mm',
    datePickerInput: 'DD',
    timePickerInput: 'HH:mm',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: ' YYYY MMMM',
  };
  public static readonly CKPostingConfig = {
    enterMode: '2',
    height: 200,
    toolbar: [
      {
        name: 'insert',
        items: [
          'Bold',
          'Italic',
          'Underline',
          'Maximize',
          'Source',
          'NumberedList',
          'BulletedList',
          'Outdent',
          'Indent',
          'Link',
          'JustifyLeft',
          'JustifyCenter',
          'JustifyRight',
          'JustifyBlock',
          'Preview'
        ]
      }
    ]
  };
  public static readonly CKArticleConfig = {
    skin: 'moono-lisa,/assets/ckeditor/skins/moono-lisa/',
    enterMode: '1',
    height: 400,
    allowedContent: true,
    extraPlugins:
      'youtube,dvs.image,dvs.slidercompare,eqneditor,dvs.tabletemplate,dvs.oembed,dvs.linkinternal,dvg.inserttextbox,dvg.articledetailrelatedlink',
    toolbar: [
      {
        name: 'document',
        items: [
          'Source',
          '-',
          'Save',
          'NewPage',
          'Preview',
          'Print',
          '-',
          'Templates'
        ]
      },
      {
        name: 'clipboard',
        items: [
          'Cut',
          'Copy',
          'Paste',
          'PasteText',
          'PasteFromWord',
          '-',
          'RemoveFormat',
          '-',
          'Undo',
          'Redo'
        ]
      },
      // {
      //   name: 'forms',
      //   items: [
      //     'Form',
      //     'Checkbox',
      //     'Radio',
      //     'TextField',
      //     'Textarea',
      //     'Select',
      //     'Button',
      //     'ImageButton',
      //     'HiddenField'
      //   ]
      // },
      { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
      '/',
      {
        name: 'editing',
        items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt']
      },
      {
        name: 'basicstyles',
        items: [
          'Bold',
          'Italic',
          'Underline',
          'Strike',
          'Subscript',
          'Superscript'
        ]
      },
      {
        name: 'paragraph',
        items: [
          'NumberedList',
          'BulletedList',
          '-',
          'Outdent',
          'Indent',
          '-',
          'Blockquote',
          'CreateDiv',
          '-',
          'JustifyLeft',
          'JustifyCenter',
          'JustifyRight',
          'JustifyBlock',
          '-',
          'BidiLtr',
          'BidiRtl'
        ]
      },
      { name: 'links', items: ['Link', 'Unlink', 'Anchor', 'dvs.linkinternal', 'dvg.articledetailrelatedlink'] },
      '/',
      {
        name: 'insert',
        items: [
          'Image',
          'Flash',
          'dvs.tabletemplate',
          'HorizontalRule',
          'Smiley',
          'SpecialChar',
          'PageBreak',
          'Iframe',
          'Youtube',
          'Templates',
          'EqnEditor',
          'dvs.image',
          'dvs.slidercompare',
          'dvs.oembed',
          'dvg.inserttextbox'
        ]
      },
      { name: 'styles', items: ['Styles', 'Format'] }
    ]
  };
}
