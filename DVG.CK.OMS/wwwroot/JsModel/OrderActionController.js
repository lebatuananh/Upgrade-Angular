(function () {
    angular.module('OrderAction', ["ui.bootstrap", "Common.Service", 'cgNotify'])
        .controller('OrderActionController', function ($scope, $http, Service, notify) {
            $scope.Order = {};

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
            $scope.page = {
                'totalItems': 0,
                'currentPage': 1,
                'itemsPerPage': 10
            };
            $scope.Search = {
                'KeyWord': '',
                'PageIndex': 1,
                'PageSize': 10
            };
            $scope.DiscountStr = "0";
            $scope.IsErrorPhone = false;
            $scope.IsErrorDeliveryDate = false;
            $scope.StatusUpdate = null;
            $scope.OrderStatusEnum = {
                //[Description("Chưa kích hoạt")]
                Inactive: 0,
                //[Description("Chờ đẩy sang POS")]
                Pending: 1,
            }

            $scope.Init = function (obj) {
                if (obj) {
                    console.log(obj);
                    $scope.Order = obj;
                    if ($scope.Order.ListProductViewModel.length > 0) {
                        var Discount = $scope.Order.OrderOriginPrice - $scope.Order.OrderPrice;
                        $scope.DiscountStr = $scope.FormatNumber(Discount);
                    }
                }
                $scope.initListOrderType();
                $scope.initPrice();
                $scope.doSearch();
                $scope.getShip();
            };

            $scope.initPrice = function () {
                var totalPrice = parseInt($("#totalPrice").text());
                if (totalPrice > 0) {
                    discount = parseInt($("#hddDiscountValue").val()) / 100 * totalprice;
                    $("#discount").text(discount);
                }
            };

            //Get list data
            $scope.getListData = function () {
                $scope.Search.PageIndex = $scope.page.currentPage;
                $scope.Search.PageSize = $scope.page.itemsPerPage;
                Service.post("/Product/Search", { model: $scope.Search })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.Order.lstOrderDetailViewModel = response.Data;
                            $scope.page.totalItems = response.TotalRow;
                        } else {
                            $scope.Order.lstOrderDetailViewModel = null;
                            $scope.page.totalItems = 0;
                        }
                    });
            };

            //Key press search
            $scope.keypressAction = function (keyEvent) {
                if (keyEvent.which === 13) {
                    $scope.doSearch();
                }
            };
            $scope.doSearch = function () {
                $scope.page.currentPage = 1;
                $scope.getListData();
            };
            $scope.roundNumberPaging = function (itemPerPage, totalItem) {
                return Math.ceil(parseFloat(totalItem) / itemPerPage);
            };
            $scope.insertProduct = function (item) {
                var id = item.ProductId;
                var pid = "p" + id;
                if ($scope.Order.ListProductViewModel.length > 0) {
                    var isExist = false;
                    $scope.Order.ListProductViewModel.map(x => {
                        if (x.ProductId == item.ProductId) {
                            isExist = true;
                            x.Quantity = x.Quantity + 1;

                            return;
                        }
                    });
                    if (!isExist) {
                        $scope.Order.ProductViewModelItem = {};
                        $scope.Order.ProductViewModelItem.ProductId = id;
                        $scope.Order.ProductViewModelItem.Quantity = 1;
                        $scope.Order.ProductViewModelItem.OriginPrice = item.OriginPrice;
                        $scope.Order.ProductViewModelItem.Price = item.Price;
                        $scope.Order.ProductViewModelItem.PriceStr = $scope.FormatNumber(item.Price);
                        $scope.Order.ProductViewModelItem.OriginPriceStr = $scope.FormatNumber(item.OriginPrice);
                        $scope.Order.ProductViewModelItem.Note = '';
                        $scope.Order.ProductViewModelItem.ProductName = item.ProductName;
                        $scope.Order.ProductViewModelItem.Code = item.Code;
                        $scope.Order.ListProductViewModel.push($scope.Order.ProductViewModelItem);
                    }
                }
                //if ($("#listOrderDetail #" + id).length) {
                //    var quantaty = parseInt($("#listOrderDetail #" + id + " .quantaty").val());
                //    $("#listOrderDetail #" + id + " .quantaty").val(quantaty + 1);
                //    $scope.setPrice(id);
                //}
                else {
                    //var name = $("#" + pid).find(".name").text();
                    //var code = $("#" + pid).find(".code").text();
                    //var origin_price = $("#" + pid).find(".origin_price").text();
                    //var price = $("#" + pid).find(".price").text();
                    $scope.Order.ProductViewModelItem = {};
                    $scope.Order.ProductViewModelItem.ProductId = id;
                    $scope.Order.ProductViewModelItem.Quantity = 1;
                    $scope.Order.ProductViewModelItem.OriginPrice = item.OriginPrice;
                    $scope.Order.ProductViewModelItem.Price = item.Price;
                    $scope.Order.ProductViewModelItem.PriceStr = item.PriceStr;
                    $scope.Order.ProductViewModelItem.OriginPriceStr = item.OriginPriceStr;
                    $scope.Order.ProductViewModelItem.Note = '';
                    $scope.Order.ProductViewModelItem.ProductName = item.ProductName;
                    $scope.Order.ProductViewModelItem.Code = item.Code;
                    $scope.Order.ListProductViewModel.push($scope.Order.ProductViewModelItem);
                }
                $scope.setPrice(item.ProductId);
            }
            $scope.onChangeQuanity = function (item) {
                if (item.Quantity == 0) {
                    $scope.decreaseProduct(item);
                } else {
                    $scope.setPrice(item.ProductId);
                }
            }
            $scope.decreaseProduct = function (item) {                
                $scope.Order.ListProductViewModel = $scope.Order.ListProductViewModel.filter(x => x.ProductId !== item.ProductId);
                $scope.setPrice(item.ProductId);
            }
            $scope.delItem = function (item) {                
                $scope.Order.ListProductViewModel.map((x, i) => {
                    if (x.ProductId == item.ProductId && x.Quantity > 1) {
                        x.Quantity = x.Quantity - 1;
                    }
                });
                $scope.setPrice(item.ProductId);
            }
            $scope.addItem = function (item) {
                $scope.Order.ListProductViewModel.map((x, i) => {
                    if (x.ProductId == item.ProductId) {
                        x.Quantity = x.Quantity + 1;
                    }
                });
                $scope.setPrice(item.ProductId);
            }
            $scope.setPrice = function (id) {
                var price;
                var totalprice = 0;
                var discount = 0;
                var lastprice = 0;
                for (var i = 0; i < $scope.Order.ListProductViewModel.length; i++) {
                    price = parseInt($scope.Order.ListProductViewModel[i].Price) * parseInt($scope.Order.ListProductViewModel[i].Quantity)
                    totalprice += price;
                }
                $("#totalPrice").text($scope.FormatNumber(totalprice));
                $scope.Order.OrderOriginPriceStr = $scope.FormatNumber(totalprice);
                discount = parseInt($("#hddDiscountValue").val()) / 100 * totalprice;
                $("#discount").text($scope.FormatNumber(discount));
                $scope.DiscountStr = $scope.FormatNumber(discount);
                $("#lastPrice").text($scope.FormatNumber(totalprice - discount));
                $scope.Order.OrderPriceStr = $scope.FormatNumber(totalprice - discount);

            }

            $scope.initListOrderType = function () {
                if ($scope.Order.order_type == parseInt($("#hddOrderTypeTakeAway").val())) {
                    $(".addresscustomer").hide();
                    $(".addresscustomer #DeliveryAddress").removeAttr("required");
                    $scope.Order.DistrictId = 0;
                    $scope.Order.WardId = 0;
                    $scope.Order.DeliveryAddress = "";
                    $scope.Order.ShipFee = 0;
                } else {
                    $(".addresscustomer").show();
                    $(".addresscustomer #DeliveryAddress").attr("required", "required");
                }
            }

            $scope.onChangeOrderType = function () {
                if ($("#OrderType").val() == $("#hddOrderTypeTakeAway").val()) {
                    $(".addresscustomer").hide();
                    $(".addresscustomer .input-sm").removeAttr("required");
                    $(".addresscustomer #DeliveryAddress").removeAttr("required");
                    $(".addresscustomer #DeliveryAddress").val(1).trigger("blur");
                    $scope.Order.DistrictId = 0;
                    $scope.Order.WardId = 0;
                    $scope.Order.DeliveryAddress = "";
                    $scope.Order.ShipFee = 0;
                } else {
                    $(".addresscustomer").show();
                    $(".addresscustomer .input-sm").attr("required", "required");
                    $(".addresscustomer #DeliveryAddress").attr("required", "required");
                }
            }
            $scope.getShip = function () {
                if ($scope.Order.DistrictId > 0) {
                    $scope.Order.ListDistrict.map((item, i) => {
                        if (item.district_id == $scope.Order.DistrictId) {
                            $scope.Order.ShipFee = item.ship_fee;
                            $scope.Order.ShipFeeStr = $scope.FormatNumber(item.ship_fee);
                        }
                    })
                } else {
                    $scope.Order.ShipFee = 0;
                    $scope.Order.ShipFeeStr = "0";
                }
            }
            $scope.onChangeDistrict = function () {
                $scope.Order.WardId = 0;
                $scope.getShip();
                $scope.initWardData($scope.Order.DistrictId);
                $scope.Order.ShipFee = parseInt($("#district option:selected").attr("rel"));
            }

            $scope.initWardData = function (district_id) {
                Service.post("/Ward/SearchByDistrictId", { districtId: district_id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.Order.ListWard = response.Data;
                        }
                    });
            }

            $scope.checkOrderType = function () {
                if ($scope.Order.SourceType == 0) return true;
                else return false;
            }

            $scope.onChangeMobileOrder = function () {
                var regexMobile1 = /^0[1-9][0-9]{8,9}$/;
                var regexMobile2 = /^84[0-9]{8,9}$/;
                if (!regexMobile1.test($scope.Order.CustomerPhone) && !regexMobile2.test($scope.Order.CustomerPhone)) {
                    $scope.IsErrorPhone = true;
                } else {
                    $scope.IsErrorPhone = false;
                }
            }
            $scope.onChangeDeliveryDate = function (event) {
                console.log("event", event)
                var regex = /^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/[0-9]{4} (00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9])$/;
                if ($scope.Order.DeliverDateStr == null || $scope.Order.DeliverDateStr == '') {
                    $scope.IsErrorDeliveryDate = true;
                } else {
                    $scope.IsErrorDeliveryDate = false;
                    //if (!regex.test($scope.Order.DeliverDateStr)) {
                    //    $scope.IsErrorDeliveryDate = true;
                    //} else {
                    //    $scope.IsErrorDeliveryDate = false;
                    //}    
                }
            }
            $scope.doUpdate = function (status) {
                $scope.StatusUpdate = status;
                if (validate()) {
                    $scope.Order.Status = status;
                    Service.post("/Order/UpdateOrder", { model: $scope.Order })
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
                                    //window.close();
                                    if ($scope.StatusUpdate == $scope.OrderStatusEnum.Inactive) {
                                        location.href = '/Order/Index?status=0';
                                    } else if ($scope.StatusUpdate == $scope.OrderStatusEnum.Pending) {
                                        location.href = '/Order/Index?status=1';
                                    }

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
                }
            };
            $scope.FormatNumber = function (sNumber, sperator = ".") {
                var result = sNumber + "";
                result = result.replace(/(.)(?=(\d{3})+$)/g, '$1.');
                return result;

            }
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
        }).directive('convertToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (val) {
                        return parseInt(val, 10);
                    });
                    ngModel.$formatters.push(function (val) {
                        return '' + val;
                    });
                }
            };
        }).directive('ckeditor', function ($rootScope) {
            return {
                require: 'ngModel',
                link: function (scope, element, attr, ngModel) {
                    CKEDITOR.on("instanceReady", function () {
                        window.setTimeout(function () {
                            // enable ckeditor
                            var ckeditor = CKEDITOR.instances[$(element).attr("id")];

                            // update ngModel on change
                            ckeditor.on('change', function () {
                                ngModel.$setViewValue(this.getData());
                            });

                        }, 200);
                    });

                }
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
            location.href = '/Order/Index';
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
    $("input[required]").trigger("blur");

    if ($(".errorflag").text() != "") {
        if (!stt) {
            if ($(".parsley-error[required]").length) {
                $($(".parsley-error[required]")[0]).focus();
            }
        }
        return false;
    }
    return true;
}

var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'VND',
});


$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});