(function () {
    angular.module('ProductAction', ["ui.bootstrap", "Common.Service", 'cgNotify'])
        .controller('ProductActionController', function ($scope, $http, Service, notify) {

            $scope.Product = {};

            $scope.noImg = $("#hddNoImage").val();

            // Template for message
            $scope.template = '';
            // Position of message
            $scope.positions = ['center', 'left', 'right'];
            // Class of message
            $scope.classes = {
                "Success": 'alert-success',
                "Error": 'alert-danger'
            };
            $scope.position = $scope.positions[0];
            // The time display of message
            $scope.duration = 2000;

            $scope.Init = function (obj) {
                if (obj) {
                    $scope.Product = obj;
					$scope.Product.CountOfDes = wordCount(RemoveHtmlTag($scope.Product.Description)).characters;
                    CKEDITOR.on("instanceReady", function () {
                        window.setTimeout(function () {
                            CKEDITOR.instances["content"].setData(obj.Content);
                            CKEDITOR.instances["description2"].setData(obj.Description2);
                        }, 200);
                    });
                }

				$(".txt-currency").FormatCurrency();
				InitSEO();
			};

			//On change product name
			$scope.onChangeProductName = function () {
				if ($scope.Product.Name && !$scope.Product.StatusOfProduct.IsApproved) {
					$scope.Product.Url = UnicodeToUnsignAndSlash($scope.Product.Name);
				}
				$scope.Product.MetaTitle = $scope.Product.Name;
			};

			$scope.blurTitleInput = function () {
				$("#txtUrl").blur();
			};

			$scope.onChangeDescription = function () {
				$scope.Product.CountOfDes = wordCount(RemoveHtmlTag($scope.Product.Description)).characters;
				$scope.Product.MetaDescription = $scope.Product.Description;
			};

            $scope.doUpdate = function () {
                if (validate()) {
                    $scope.Product.Content = $scope.regexCkeditor(CKEDITOR.instances["content"].getData());
                    $scope.Product.Description2 = $scope.regexCkeditor(CKEDITOR.instances["description2"].getData());
                    $scope.Product.PublishedDateStr = $("#txtPublishedDate").val();
                    $scope.Product.Price = UnFormatNumber($scope.Product.PriceStr);

                    if ($scope.Product.Content) {
                        Service.post("/Product/Update", { productModel: $scope.Product })
                            .then(function (response) {
                                if (response.Success) {
                                    notify({
                                        message: "Cập nhật thành công",
                                        classes: $scope.classes.Success,
                                        templateUrl: $scope.template,
                                        position: $scope.position,
                                        duration: $scope.duration
                                    });
                                    window.setTimeout(function () {
                                        window.close();
                                    }, $scope.duration);

                                } else {
                                    notify({
                                        message: response.Message,
                                        classes: $scope.classes.Error,
                                        templateUrl: $scope.template,
                                        position: $scope.position,
                                        duration: $scope.duration
                                    });
                                }
                            });
                    } else {
                        notify({
                            message: Const.message.contentInvalid,
                            classes: $scope.classes.Error,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                }
            };

            // Back to page from form update
            $scope.gotoPage = function (type) {
                switch (type) {
                    case $scope.ProductStatus.Deleted:
                        window.location = "";
                        break;
                    case $scope.ProductStatus.Pending:
                        window.location = "";
                        break;
                    case $scope.ProductStatus.Published:
                        window.location = "";
                        break;
                    case $scope.ProductStatus.Returned:
                        window.location = "";
                        break;
                    case $scope.ProductStatus.UnPublished:
                        window.location = "";
                        break;
                    default:
                        window.location = "";
                }
            };

            $scope.choseAvatar = function () {
                var w = window.open("/FileManager/Index", "File manager", "width = 950, height = 600");
                w.callback = function (result) {
                    w.close();

                    if (!result || !result.Result) {
                        sysmess.warning("No file selected!");
                        return;
                    }

                    if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.FullPath) === null) {
                        sysmess.warning("Please select image");
                        return;
                    }
                    $("#ProductAvatar").attr("src", result.FullPath).show();
                    $scope.Product.Avatar = result.FullPath;

                };
            };

            $scope.delAvatar = function () {
                $("#ProductAvatar").attr("src", $('#hdNoImage').val()).show();
                $scope.Product.Avatar = '';
            };

            //Chọn lại ảnh avatar hoặc ảnh slide
            $scope.selectImages = function (index) {
                var w = window.open("/FileManager/Index", "File manager", "width = 950, height = 600");
                w.callback = function (result) {
                    w.close();

                    if (!result || !result.Result) {
                        sysmess.warning("No file selected!");
                        return;
                    }

                    if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.FullPath) === null) {
                        sysmess.warning("Please select image");
                        return;
                    }
                    $scope.$apply(function () {
                        $scope.Product.ListImage[index].ImageUrl = result.FullPath;
                    });

                };
            };

            //Xóa ảnh avatar hoặc ảnh slide
            $scope.delImages = function (index) {
                $scope.Product.ListImage.splice(index, 1);
            };

            //Thêm ảnh slide
            $scope.addImages = function () {
                var w = window.open("/FileManager/Index", "File manager", "width = 950, height = 600");
                w.callback = function (result) {
                    w.close();

                    if (!result || !result.Result) {
                        sysmess.warning("No file selected!");
                        return;
                    }

                    if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.FullPath) === null) {
                        sysmess.warning("Please select image");
                        return;
                    }
                    var imageadd = new Object();
                    imageadd.Id = 0;
                    imageadd.ImageUrl = result.FullPath;
                    if ($scope.Product.ListImage === null) {
                        $scope.Product.ListImage = [];
                    }
                    $scope.$apply(function () {
                        $scope.Product.ListImage.push(imageadd);
                    });
                };
            };

            //Xem ảnh gốc avatar hoặc ảnh slide
            $scope.previewImages = function (imageurl) {
                popupCenter(imageurl, "Ảnh gốc", 950, 600);
            };

            $scope.regexVideo = function (str) {
                var regexVideo = /(<iframe.+?<\/iframe>)/g;
                var regexSrcVideo = /(?:src=\")([^"]+)/g;
                var content = str.match(regexVideo), src = "", srcRel = "", len = 0, i = 0, result = "";
                if (content) {
                    len = content.length;
                }
                for (i = 0; i < len; i++) {
                    if (content[i].indexOf('youtube.com') > -1) {
                        result = "<p>" + content[i] + "</p>";
                        src = content[i].match(regexSrcVideo);
                        //Disable show suggested videos at the video's end
                        if (src) {
                            src = src[0].replace("src=\"", "");
                            if (src.indexOf('rel=0') === -1) {
                                if (src.indexOf('?') === -1) {
                                    srcRel = src + "?rel=0";
                                } else {
                                    srcRel = src + "&rel=0";
                                }
                                result = result.replace(src, srcRel);
                            }
                        }
                        break;
                    }
                }
                return result;
            };

            $scope.regexCkeditor = function (str) {
                if (str.indexOf('&acirc;') > -1) {
                    str = str.replace(/&acirc;/g, 'â');
                }
                if (str.indexOf('&ecirc;') > -1) {
                    str = str.replace(/&ecirc;/g, 'ê');
                }
                if (str.indexOf('&agrave;') > -1) {
                    str = str.replace(/&agrave;/g, 'à');
                }
                if (str.indexOf('&uacute;') > -1) {
                    str = str.replace(/&uacute;/g, 'ú');
                }
                if (str.indexOf('&oacute;') > -1) {
                    str = str.replace(/&oacute;/g, 'ó');
                }
                if (str.indexOf('&aacute;') > -1) {
                    str = str.replace(/&aacute;/g, 'á');
                }
                if (str.indexOf('&atilde;') > -1) {
                    str = str.replace(/&atilde;/g, 'ã');
                }
                if (str.indexOf('&egrave;') > -1) {
                    str = str.replace(/&egrave;/g, 'è');
                }
                if (str.indexOf('&eacute;') > -1) {
                    str = str.replace(/&eacute;/g, 'é');
                }
                if (str.indexOf('&igrave;') > -1) {
                    str = str.replace(/&igrave;/g, 'ì');
                }
                if (str.indexOf('&iacute;') > -1) {
                    str = str.replace(/&iacute;/g, 'í');
                }
                if (str.indexOf('&ocirc;') > -1) {
                    str = str.replace(/&ocirc;/g, 'ô');
                }
                if (str.indexOf('&ograve;') > -1) {
                    str = str.replace(/&ograve;/g, 'ò');
                }
                if (str.indexOf('&otilde;') > -1) {
                    str = str.replace(/&otilde;/g, 'õ');
                }
                if (str.indexOf('&ugrave;') > -1) {
                    str = str.replace(/&ugrave;/g, 'ù');
                }
                if (str.indexOf('&nbsp;') > -1) {
                    str = str.replace(/&nbsp;/g, ' ');
                }
                return str;
            };
        });
})();

//Check khi reload trang
function confirmcancel() {
    var conf = confirm('Bạn có chắc muốn hủy?');
    if (conf == false) {
        allowreload = false;
        return false;
    }
    else {
        allowreload = true;
        if (!window.close()) {
            location.href = '/Product/Index';
        }
    }
}
$(window).bind('beforeunload', function () {
    //save info somewhere
    if (!allowreload) {
        return confirm('Bạn có chắc muốn tải lại trang?');
    }
});

function validate() {
    var stt = 1;
    var id;
    $("input[required]").trigger("blur");

    if ($("#sapo").val() === "") {
        $("#sapo").next().next().find("li").show();
        $('html,body').animate({ scrollTop: $("#sapo").offset().top }, 500);
        $(".errorflag").text("sapo");
        stt = 1;
    } else {
        $("#sapo").next().next().find("li").hide();
        $(".errorflag").text($(".errorflag").text().replace("sapo", ""));
        stt = 0;
        if ($("#description2").isCKEDITOREmpty()) {
            $("#description2").next().next().find("li").show();
            $('html,body').animate({ scrollTop: $("#cke_description2").offset().top }, 500);
            $(".errorflag").text("description2");
            stt = 1;
        }
        else if ($("#content").isCKEDITOREmpty()) {
            $("#content").next().next().find("li").show();
            $('html,body').animate({ scrollTop: $("#cke_content").offset().top }, 500);
            $(".errorflag").text("content");
            stt = 1;
        } else {
            $("#content").next().next().find("li").hide();
            $("#description2").next().next().find("li").hide();
            $(".errorflag").text($(".errorflag").text().replace("content", ""));
            $(".errorflag").text($(".errorflag").text().replace("description2", ""));
            stt = 0;
            if ($("select.form-control").val() === "-- Lựa chọn --") {
                $("select.form-control").next().find("li").show();
                $('html,body').animate({ scrollTop: $("select.form-control").offset().top }, 500);
                $(".errorflag").text("Producttype");
                stt = 1;
            } else {
                $("select.form-control").next().find("li").hide();
                $(".errorflag").text($(".errorflag").text().replace("Producttype", ""));
                stt = 0;
            }
        }
    }

    if ($(".errorflag").text() !== "") {
        if (!stt) {
            if ($(".parsley-error[required]").length) {
                $($(".parsley-error[required]")[0]).focus();
            }
        }
        return false;
    }
    return true;
}