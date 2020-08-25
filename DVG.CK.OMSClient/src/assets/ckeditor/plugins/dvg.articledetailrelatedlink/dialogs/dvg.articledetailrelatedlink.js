var api = api_config.APP_API_URL;

CKEDITOR.dialog.add("ArticleDetailRelatedLinkDialog", function (editor) {
  return {
    title: "Article Detail - Related Link",
    minWidth: 500,
    minHeight: 250,
    contents: [
      {
        id: "tab-config",
        label: "Config",
        padding: 0,
        height: 250,
        width: 500,
        elements:
          [
            {
              type: "text",
              id: "txtDisplayText",
              label: "Display Text (put empty to use Related Article title)",
              setup: function (e) {
                var value = e.getAttribute("title");

                if (!value) {
                  return;
                }

                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtDisplayText", value);
              },
              //validate: CKEDITOR.dialog.validate.notEmpty(" Display text is required !"),
            },
            {
              type: "text",
              id: "txtLink",
              label: "Related Article (Enter less than 5 characters to search Articles)",
              setup: function (e) {
                var value = e.getAttribute("title");

                if (!value) {
                  return;
                }

                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtLink", value);
              },
              validate: CKEDITOR.dialog.validate.notEmpty(" Article is required !"),
            },
            {
              type: "text",
              id: "hiddenLink",
              label: "",
              setup: function (e) {
                var value = e.getAttribute("title");

                if (!value) {
                  return;
                }

                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "hiddenLink", value);
              },
            },
            {
              type: "text",
              id: "hiddenTitle",
              label: "",
              setup: function (e) {
                var value = e.getAttribute("title");

                if (!value) {
                  return;
                }

                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "hiddenTitle", value);
              },
            },
            {
              type: 'html',
              id: 'validateFoundArticles',
              html: '<span id="validateFoundArticles" style="color: red;"></span>'
            }
          ]
      }
    ],
    onLoad: function () {
      /*fmClient.IsEditor = true;
      fmClient.SelectedControl = editor.name;*/
      
    },
    onShow: function () {
      auto.init();

      var hiddenInputLink = auto.getElementLink();
      hiddenInputLink.attr("style", "display:none;");

      var hiddenInputTitle = auto.getElementTitle();
      hiddenInputTitle.attr("style", "display:none;");

      var selection = editor.getSelection();
      var element = selection.getStartElement();

      if (element) {
        element = element.getAscendant('div', true);
      }

      if (!element
        || element.getName() != 'p') {
        element = editor.document.createElement('p');
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
      var displayText = dialog.getValueOf("tab-config", "txtDisplayText");
      var txtArticle = dialog.getValueOf("tab-config", "txtLink");
      var link = dialog.getValueOf("tab-config", "hiddenLink");
      var title = dialog.getValueOf("tab-config", "hiddenTitle");

      var displayTitle = "";
      var titleTag = "";
      if (displayText != "") {
        displayTitle = displayText;
      } else {
        displayTitle = title;
      }
      if (link == "") {
        window.alert("Please input keyword again and select in the suggestion articles !");
        return false;
      }
      var element = dialog.element;
      if (displayTitle !== null || displayTitle.trim(' ') !== '') {
        titleTag = displayTitle.replace( /"/g,"'");
      }

      //var template = "<div class=article-detail-box-related-link>";
      //template +=       "<h3 class=title-box>ALSO READ</h3>";
      //template += "<p><a href='" + link + "' title='" + titleTag + "' class='also-read-link' data-tracking='yes' data-action = 'Related articles' >" + displayTitle+"</a></p>";
      //template += "</div>";
      //template += "<p>&nbsp</p>";
      var template = '<div class=article-detail-box-related-link>';
      template += '<h3 class=title-box>ALSO READ</h3>';
      template += '<p><a href="' + link + '" title="' + titleTag + '" class="also-read-link" data-tracking="yes" data-action = "Related articles" >' + displayTitle + '</a></p>';
      template += '</div>';
      template += '<p>&nbsp</p>';
     
      element.setHtml(template);
      dialog.commitContent(element);

      if (dialog.insertMode) {
        editor.insertHtml(element.getHtml());
      }
    }
  };
});
var auto = {
  initElement: function () {
    var dialog = CKEDITOR.dialog.getCurrent();
    var txtLink = dialog.getContentElement('tab-config', 'txtLink');
    var input = $("#" + txtLink.domId).find("input");
    return input;
  },
  getElementTitle: function () {
    var hiddenTitle = CKEDITOR.dialog.getCurrent().getContentElement('tab-config', 'hiddenTitle');
    var hiddenInputTitle = $("#" + hiddenTitle.domId).find("input");
    return hiddenInputTitle;
  },
  getElementLink: function () {
    var hiddenLink = CKEDITOR.dialog.getCurrent().getContentElement('tab-config', 'hiddenLink');
    var hiddenInputLink = $("#" + hiddenLink.domId).find("input");
    return hiddenInputLink;
  },  
  init: function () {
    var input = auto.initElement();
    var timer;
    var url = api + "/articles/searchbykeyword";
    input.autocomplete({
      source: []
    });
    input.keyup(function () {    
      clearTimeout(timer);
      timer = setTimeout(function (event) {
        var keyword = input.val();
        if (keyword == "" || keyword.length < 3) {          
          return false;
        }
        var data = new Object();
        data.pagesize = 10;
        data.pageindex = 1;
        data.keyword = keyword;

        $.ajax({
          url: url,
          type: "POST",
          cache: false,
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          data: JSON.stringify(data),
          success: function (Msg) {
            if (!Msg.Error) {
              $("#validateFoundArticles").html("");
              var objTag = new Object();
              var lstTag = new Array();
              var lstTagResponse = Msg.Obj.Data;
              if (lstTagResponse.length > 0) {
                for (var i = 0; i < lstTagResponse.length; i++) {
                  objTag = new Object();
                  //objTag.title = lstTagResponse[i].title;
                  objTag.label = lstTagResponse[i].id + ' - ' + lstTagResponse[i].title;
                  objTag.value = lstTagResponse[i].title;
                  objTag.link = lstTagResponse[i].link;
                  lstTag.push(objTag);
                }
                input.autocomplete({
                  source: lstTag,
                  select: function (e, ui) {
                    e.preventDefault();
                    var link = ui.item.link;
                    var title = ui.item.value;
                    input.val(title);

                    var hiddenInputLink = auto.getElementLink();
                    var hiddenInputTitle = auto.getElementTitle();
                    hiddenInputLink.val(link);
                    hiddenInputTitle.val(title);
                  },
                });
                var e = $.Event("keydown");
                e.keyCode = 40; // # Some key code value
                input.trigger(e);
              } else {
                // window.alert("No data found!")
                $("#validateFoundArticles").html("No data found!");
              }
              
            }
            else {
              sysmess.error(Msg.Title);
            }
          }
        });
      }, 1000);
    });
  
  },
 
}
