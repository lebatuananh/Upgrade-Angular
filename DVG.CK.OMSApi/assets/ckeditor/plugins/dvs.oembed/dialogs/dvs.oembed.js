CKEDITOR.dialog.add('OEmbedDialog', function (editor) {
    return {
        title: 'Media Embed',
        minWidth: 350,
		minHeight: 50,
        contents: [
            {
                id: 'tab-embed',
                padding: 0,
                elements:
                [
                    {
                        type: 'hbox',
                        widths: ['70%', '30%'],
                        children: [
                            {
                                type: 'text',
                                id: 'txtOEmbedUrl',
                                label: 'URL',
                                'default': ''
                            }
                        ]
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
                || !element.getAttribute("data-oembed-url") && !$(element.$).closest('div.dvs-oembed')) {
                element = editor.document.createElement('div');
                this.insertMode = true;
            } else {
                // Set value to dialog
                var obj = $(element.$).closest('div.dvs-oembed');
                element = new CKEDITOR.dom.element(obj[0]);
                var linkOEmbed = $(obj).attr('data-oembed-url');
                if (obj) {
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-embed", "txtOEmbedUrl", linkOEmbed);
                }
                this.insertMode = false;
                this.setupContent(element);
            }

            this.element = element;
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var dialog = this;
            var linkPost = dialog.getValueOf('tab-embed', 'txtOEmbedUrl');
            var regexIns = /(https?:\/\/(?:www\.)?instagram\.com\/p\/([^/?#&]+)).*/ig;
            var regexTwi = /http(?:s)?:\/\/(?:www.)?twitter\.com\/([a-zA-Z0-9_]+)\/status\/([a-zA-Z0-9_]+)/ig;
            var isIns = linkPost.match(regexIns);
            var isTwi = linkPost.match(regexTwi);

            //check link valid
            if (isIns || isTwi) {
                //Call instagram api
                if (isIns) {
                    embedService.loadContentRequest(linkPost, embedService.instagramApi, embedService.instagramDataType, function(contentRequest){
                        var element = dialog.element;
                        element.setAttribute("data-oembed-url", linkPost);
                        element.setAttribute("class", "dvs-oembed");
                        element.setHtml(contentRequest);

                        dialog.commitContent(element);

                        if (dialog.insertMode) {
                            editor.insertElement(element);
                        }
                    });
                }else{ // Call twitter api
                    embedService.loadContentRequest(linkPost, embedService.twitterApi, embedService.twitterDataType, function(contentRequest){
                        var element = dialog.element;
                        element.setAttribute("data-oembed-url", linkPost);
                        element.setAttribute("class", "dvs-oembed");
                        element.setHtml(contentRequest);

                        dialog.commitContent(element);

                        if (dialog.insertMode) {
                            editor.insertElement(element);
                        }
                    });
                }


            }else{
                alert('Link invalid!');
            }
        },
        onHide: function (e) {

        }
    };
});

/**
 * Object oembed api service
 * Autthor: DucMV
 */
var embedService = {
    instagramApi:'https://api.instagram.com/oembed',
    twitterApi:'https://publish.twitter.com/oembed',
    instagramDataType: 'json',
    twitterDataType: 'jsonp',
    loadContentRequest: function(url, oEmbedApi, dataType, callBack){
        $.ajax({
            url: oEmbedApi + "?url=" + url,
            dataType: dataType, // this is important
            cache: false,
            success: function (response) {
                callBack(response.html);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }
}
