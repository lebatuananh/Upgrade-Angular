
/*gọi ý tìm kiếm star*/
$(function () {

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {

        var t = '', label = '', fullname = '';

        if (item.option != null)//đăng tin
        {
            label = $.trim(item.value);
            fullname = $.trim(item.fullname);
            $(ul).addClass('ulPost');
            return $("<li class='row'></li>").data("item.autocomplete", item).append("<span> <strong> " + fullname + " </strong> &lt;" + label + " &gt;</span>").appendTo(ul);
            //return $("<li class='row'></li>").data("item.autocomplete", item).append("<span>" + label + "</span>").appendTo(ul);
        }
        else {
            t = item.value;
            fullname = item.fullname;
            return $("<li class='item_row clearfix'></li>").data("item.autocomplete", item).append("<span class='stext'> <strong> " + fullname + " </strong> &lt;" + t + "&gt</span>").appendTo(ul);
            //return $("<li class='item_row clearfix'></li>").data("item.autocomplete", item).append("<span class='stext'>" + t + "</span>").appendTo(ul);
        }
    };

    ___isIE = /MSIE (\d+\.\d+);/.test(navigator.userAgent);
    if (___isIE) {

        $('#action-form .bootstrap-tagsinput input[type="text"]').css('color', '#777');
        $('#action-form .bootstrap-tagsinput input[type="text"]').css('height', '19px');
        $('#action-form .bootstrap-tagsinput input[type="text"]').css('padding-top', '2px');
        $('#action-form .bootstrap-tagsinput input[type="text"]').css('padding-bottom', '2px');
        $('#action-form .bootstrap-tagsinput input[type="text"]').css('line-height', '18px');
    }
    var xhr = null;
    $('#action-form .bootstrap-tagsinput input[type="text"]').autocomplete({
        close: function (event, ui) {
            $('.bg_opacity').hide();
            $('.pl-search').attr("style", "z-index: inherit");
            $('.ui-autocomplete-input').val("");
        },
        open: function (event, ui) {
            $('.bg_opacity').show();
            $('.pl-search').attr("style", "z-index: 13");
        },
        source: function (request, response) {
            var term = $.trim(request.term);
            if (term.length > 2) {
                var urlRequest = '';
                urlRequest = '/Category/UserComplete';
                if (xhr != null && xhr.readyState !== 4) {
                    xhr.abort();
                }
                xhr = $.ajax({
                    url: urlRequest,
                    data: { "keyword": term },
                    success: function (dataJson) {
                        response(eval(dataJson));
                    }
                });
            }
        },
        minLength: 2,
        delay: 500,
        select: function (event, ui) {
            if (ui.item != null) {
                $('.ui-autocomplete-input').val("");
                $('#txtAddMod').tagsinput('add', { UserId: ui.item.uid, UserName: ui.item.value }, { preventPost: true });
            }
        }
    }).on('click', function () {
        if (document.hasFocus()) {
            $('ul.ui-autocomplete, .bg_opacity').hide();
        }
    });
});

/*gọi ý tìm kiếm end*/

