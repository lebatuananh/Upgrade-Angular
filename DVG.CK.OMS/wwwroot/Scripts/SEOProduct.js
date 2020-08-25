//BOXSEO---------------
var BaseUrl = 'http://yenchanlong.vn/';

function ValidateKeyword() {

	var keywords = $('#txtStandardKeyword').val();
	var title = $('#title').val();
	var sapo = $("#sapo").val();
	var content = CKEDITOR.instances["content"].getData();
	var seoTitle = $('#txtMetaTitle').val();
	var seoSapo = $('#txtMetaDesc').val();
	var pathname = window.location.pathname;
	var newsid = $("#hddIDLong").val();
	if (newsid != null && newsid != '') { }
	else { newsid = '201303053463478' }

	var cat_unsign = "san-pham";//UnicodeToUnsignAndSlash($('#category option:selected').text());
	if (cat_unsign == null || cat_unsign == '') cat_unsign = 'san-pham';
	var link = BaseUrl + cat_unsign + '/' + UnicodeToUnsignAndSlash(title);
	$('#ggLink').html(link);
	if (keywords.length > 2) {
		var countTitle = CountKeywordsMatch(title, keywords);
		var countSapo = CountKeywordsMatch(sapo, keywords);
		var countcontent = CountKeywordsMatch(content, keywords);
		var countSeoTitle = CountKeywordsMatch(seoTitle, keywords);
		var countSeoSapo = CountKeywordsMatch(seoSapo, keywords);

		var pat = new RegExp(keywords + '(?![^<]*>)', "gi");


		var preSeoSapo = seoSapo.replace(pat, function (match, t, original) {
			return '<b>' + match + '</b>'
		});
		$('#ggDes').html(preSeoSapo);

		var preSeoTitle = seoTitle.replace(pat, function (match, t, original) {
			return '<b>' + match + '</b>'
		});
		$('#ggTitle').html(preSeoTitle);

		if (countTitle > 0) {
			$('#kwTitle').css('color', '#2f7f1c'); $('#kwTitle').html('(' + countTitle + ') lần');
		}
		else {
			$('#kwTitle').css('color', 'red'); $('#kwTitle').html('(0) lần');
		}

		if (countSapo > 0) {
			$('#kwSapo').css('color', '#2f7f1c'); $('#kwSapo').html('(' + countSapo + ') lần');
		}
		else {
			$('#kwSapo').css('color', 'red'); $('#kwSapo').html('(0) lần');
		}

		if (countcontent > 0) {

			$('#kwContent').css('color', '#2f7f1c'); $('#kwContent').html('(' + countcontent + ') lần');
		}
		else {
			$('#kwContent').css('color', 'red'); $('#kwContent').html('(0) lần');
		}

		if (countSeoTitle > 0) {
			$('#kwSeoTitle').css('color', '#2f7f1c'); $('#kwSeoTitle').html('(' + countSeoTitle + ') lần');
		}
		else {
			$('#kwSeoTitle').css('color', 'red'); $('#kwSeoTitle').html('(0) lần');
		}

		if (countSeoSapo > 0) {
			$('#kwSeoSapo').css('color', '#2f7f1c'); $('#kwSeoSapo').html('(' + countSeoSapo + ') lần');
		}
		else {
			$('#kwSeoSapo').css('color', 'red'); $('#kwSeoSapo').html('(0) lần');
		}
	}
	else {
		$('#kwTitle').css('color', 'red'); $('#kwTitle').html('(0) lần');
		$('#kwSapo').css('color', 'red'); $('#kwSapo').html('(0) lần');
		$('#kwContent').css('color', 'red'); $('#kwContent').html('(0) lần');
		$('#kwSeoTitle').css('color', 'red'); $('#kwSeoTitle').html('(0) lần');
		$('#kwSeoSapo').css('color', 'red'); $('#kwSeoSapo').html('(0) lần');

		$('#ggTitle').html(seoTitle);
		$('#ggDes').html(seoSapo);

	}

}

function CountKeywordsMatch(content, item) {
	var pat = new RegExp(item, "gi");
	var pat2 = new RegExp(item + "[a-z]", "gi");
	var matchResult = RemoveHtmlTag(content).match(pat);
	var match2 = content.match(pat2);
	if (matchResult != null) {
		if (match2 != null) {
			count = matchResult.length - match2.length;
		}
		else {
			count = matchResult.length
		}
		return count;
	}
	else {
		return 0;
	}
}

function RemoveHtmlTag(content) {
	var regex = /(<([^>]+)>)/ig;
	var result = content ? content.replace(regex, "") : "";
	return result;
}

function UnicodeToUnsignAndSlash(s) {
	if (s == null || s == "") return "";
	strChar = "abcdefghijklmnopqrstxyzuvxw0123456789 ";
	s = UnicodeToUnsign(s.toLowerCase());
	sReturn = ""; for (i = 0; i < s.length; i++) { if (strChar.indexOf(s.charAt(i)) > -1) { if (s.charAt(i) != ' ') { sReturn += s.charAt(i); } else if (i > 0 && s.charAt(i - 1) != ' ' && s.charAt(i - 1) != '-') { sReturn += "-"; } } } return sReturn;
}
function UnicodeToUnsign(s) {
	uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
	UnsignChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
	retVal = ""; for (i = 0; i < s.length; i++) { pos = uniChars.indexOf(s.charAt(i)); if (pos >= 0) { retVal += UnsignChars.charAt(pos); } else { retVal += s.charAt(i); } } return retVal;
}

function InitSEO() {

	var elem = $("#metaTitleLimit");
	var idmessenger1 = $("#metaTitleLimitMessenger");
	var elem2 = $("#metaDesLimit");
	var idmessenger2 = $("#metaDesLimitMessenger");

	$('#txtMetaTitle').limiter(58, elem, idmessenger1);
	$('#txtMetaDesc').limiter(150, elem2, idmessenger2);

	ValidateKeyword();

	$('#txtStandardKeyword').blur(function () {
		ValidateKeyword();
	});
	$('#txtMetaTitle').blur(function () {
		ValidateKeyword();
	});
	$('#txtMetaDesc').blur(function () {
		ValidateKeyword();
	});
	$('#title').blur(function () {
		$('#txtMetaTitle').attr("placeHolder", $(this).val());
		ValidateKeyword();
	});
	$('#sapo').blur(function () {
		$('#txtMetaDesc').attr("placeHolder", $(this).val());
		ValidateKeyword();
	});
	CKEDITOR.instances.content.on('blur', function () {
		ValidateKeyword();
	});
}
//---------------------
(function (jQuery) {

	jQuery.fn.extend({
		limiter: function (limit, elem, idmessenger) {
			jQuery(this)
				.on("keyup focus", function () {
					setCount(this, elem, idmessenger);

				});

			function setCount(src, elem, idmessenger) {
				var chars = src.value.length;
				//                if (chars > limit) {
				//                    src.value = src.value.substr(0, limit);
				//                    chars = limit;
				//                }

				if (limit - chars < 0) {
					jQuery(idmessenger).css("display", "block");
				} else {
					jQuery(idmessenger).css("display", "none");
				}
				elem.html(limit - chars);

			}
			setCount(jQuery(this)[0], elem, idmessenger);

		}
	});
})(jQuery);