(function () {
    angular.module('Product', ["ui.bootstrap", "Common.Service", 'ngSanitize', 'cgNotify'])
        .controller('ProductController', function ($scope, $http, Service, $sce, notify) {
            $scope.page = {
                'totalItems': 0,
                'currentPage': 1,
                'itemsPerPage': 15
            };

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

            $scope.SearchProduct = {};

            $scope.currentOrder = 0;

            $scope.ProductFiltered = [];

            $scope.Init = function (obj) {
                if (obj) {
                    $scope.SearchProduct = obj;
                    // nếu ProductForm = 2 => load list highlight
                    if (obj.ProductForm === 2) {
                        $scope.getListHighlight();
                    }
                }
                $scope.getListData();

            };

            $scope.ListData = [];
            $scope.ListHighlight = [];

            //Get list category data
            $scope.getListData = function () {
                $scope.SearchProduct.PageIndex = $scope.page.currentPage;
                $scope.SearchProduct.PageSize = $scope.page.itemsPerPage;
                Service.post("/Product/Search", { searchModel: $scope.SearchProduct })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.ListData = response.Data;
                            $scope.page.totalItems = response.TotalRow;
                        }
                    });
            };

            // List highlight
            $scope.getListHighlight = function () {
                Service.post("/Product/GetListHighlight", {})
                    .then(function (response) {
                        if (response.Success) {
                            $scope.ListHighlight = response.Data;
                        }
                    });
            };

            // xuất bản
            $scope.doApprove = function (encryptId) {
                if (confirm("Bạn muốn đăng sản phẩm này")) {
                    Service.post("/Product/Approve", { encryptId: encryptId })
                        .then(function (response) {
                            if (response.Success) {
                                notify({
                                    message: "Đăng sản phẩm thành công",
                                    classes: $scope.classes.Success,
                                    position: $scope.position,
                                    duration: $scope.duration
                                });
                                window.setTimeout(function () {
                                    location.reload();
                                }, $scope.duration);
                            } else {
                                notify({
                                    message: response.Message,
                                    classes: $scope.classes.Error,
                                    position: $scope.position,
                                    duration: $scope.duration
                                });
                            }
                        });
                }
            };

            //Gỡ bài đã publish
            $scope.doNotApprove = function (encryptId) {
                if (confirm("Bạn muốn gỡ sản phẩm này")) {
                    Service.post("/Product/UnApprove", { encryptId: encryptId })
                        .then(function (response) {
                            if (response.Success) {
                                notify({
                                    message: "Sản phẩm đã được gỡ bỏ",
                                    classes: $scope.classes.Success,
                                    position: $scope.position,
                                    duration: $scope.duration
                                });
                                window.setTimeout(function () {
                                    location.reload();
                                }, $scope.duration);
                            } else {
                                notify({
                                    message: response.Message,
                                    classes: $scope.classes.Error,
                                    position: $scope.position,
                                    duration: $scope.duration
                                });
                            }
                        });
                }
            };

            $scope.roundNumberPaging = function (itemPerPage, totalItem) {
                return Math.ceil(parseFloat(totalItem) / itemPerPage);
            };

            $scope.doDelete = function (encryptId) {
                if (confirm("Bạn có chắc chắn muốn xóa sản phẩm?")) {
                    Service.post("/Product/Delete", { encryptId: encryptId })
                        .then(function (response) {
                            if (response.Success) {
                                notify({
                                    message: "Xóa thành công",
                                    classes: $scope.classes.Success,
                                    position: $scope.position,
                                    duration: $scope.duration
                                });
                                window.setTimeout(function () {
                                    location.reload();
                                }, $scope.duration);

                            } else {
                                alert(response.Message);
                            }
                        });
                }
            };

            $scope.geById = function (encryptId) {
                Service.post("/Product/GetByEncryptId", { encryptId: encryptId })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.Product = response.Data;
                            $("#listview").hide();
                            $("#detailview").show();
                        }
                    });
            };

            $scope.doSearch = function () {
                $scope.page.currentPage = 1;
                $scope.getListData();
            };

            // highlight
            $scope.doHighlight = function (encryptId) {
                Service.post("/Product/SetHighlight", { encryptId: encryptId })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Set nổi bật thành công",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            $scope.getListData();
                            $scope.getListHighlight();
                        } else {
                            notify({
                                message: response.Message,
                                classes: $scope.classes.Error,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                        }
                    });
            };

            $scope.doRemoveHighlight = function (encryptId) {
                Service.post("/Product/RemoveHighlight", { encryptId: encryptId })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Gỡ nổi bật thành công",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            $scope.getListData();
                            $scope.getListHighlight();
                        } else {
                            notify({
                                message: response.Message,
                                classes: $scope.classes.Error,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                        }
                    });
            };

            //Key press search
            $scope.keypressAction = function (keyEvent) {
                if (keyEvent.which === 13) {
                    $scope.doSearch();
                }
            };

            //Get current order of topic
            $scope.getCurrentOrder = function (order) {
                $scope.currentOrder = order;
            };

            // html filter (render text as html)
            $scope.trustedHtml = function (plainText) {
                return $sce.trustAsHtml(plainText);
            };

        });
})();

$(document).ready(function () {
    $("#Productmanager").addClass("active");
    $("#Productmanager ul.child_menu").show();
});

function backtolist() {
    $("#listview").show();
    $("#detailview").hide();
}

