(function () {
    angular.module('Order', ["ui.bootstrap", "Common.Service", 'ngSanitize', 'cgNotify'])
        .controller('OrderController', function ($scope, $http, Service, $sce, notify) {
            $scope.page = {
                'totalItems': 0,
                'currentPage': 1,
                'itemsPerPage': 15
            };

            $scope.RolesEnum = {
                //[Description("Quản trị viên")]
                Staff: 1,
                // [Description("Admin")]
                Admin: 2,
                // [Description("Quản lý nhà hàng")]
                Manager: 3,
                //[Description("Chăm sóc khách hàng")]
                CustomerService: 4,
                //  [Description("Thu ngân")]
                Cashier: 5,
                //  [Description("Nhân viên bếp")]
                Kitchen: 6,
                //  [Description("Quản lý bếp")]
                KitchenManager: 7
            };

            $scope.OrderStatusEnum = {
                ///[Description("Chưa kích hoạt")]
                Inactive: 0,
                ///[Description("Chờ đẩy sang POS")]
                Pending: 1,
                /// [Description("Đã đẩy sang POS")]
                PushToPOS: 2,
                /// [Description("Bếp nhận nấu")]
                KitchenAccept: 3,
                ///[Description("Bếp báo done")]
                KitchenDone: 4,
                //[Description("Đang vận chuyển")]
                Delivering: 5,
                //[Description("Giao hàng không thành công")]
                Failure: 6,
                //[Description("Giao hàng thành công")]
                Success: 7,
                //[Description("Hủy đơn hàng")]
                Destroy: 8
            };

            $scope.DeliveryStatusEnum =
            {
                // Các trạng thái Delivery: 0: Chưa giao hàng; 1: IDLE; 2:ASSIGNING;3:ACCEPTED;4:IN PROCESS;5:COMPLETED;6:CANCELLED
                //[Description("Chưa giao hàng")]
                Default: 0,
                //[Description("IDLE")]
                Idle: 1,
                // [Description("ASSIGNING")]
                Asigning: 2,
                //[Description("ACCEPTED")]
                Accepted: 3,
                //[Description("IN PROCESS")]
                InProcess: 4,
                //[Description("COMPLETED")]
                Completed: 5,
                //[Description("CANCELLED")]
                Cancelled: 6
            }

            $scope.RequestTypeEnum =
            {
                //[Description("Không có yêu cầu")]
                Normal: 0,
                //[Description("CSKH báo hủy đơn")]
                CSRequestDestroy: 1,
                //  [Description("Hủy đơn theo yêu cầu của CSKH")]
                ConfirmDestroyForCS: 2,
                //   [Description("Báo lại khách do bếp hủy")]
                KitchenDestroy: 3,
                //   [Description("Hủy đơn đã báo lại khách")]
                ConfirmCustomerForDestroy: 4
            }
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

            $scope.SearchOrder = {
                'KeyWord': '',
                'RequestType': -1,
                'DeliveryStatus': -1,
                'OrderType': -1,
                'SourceType': -1,
                'Status': -1,
                'FromCreatedDate': null,
                'ToCreatedDate': null,
                'PageIndex': 1,
                'PageSize': 15
            };
            $scope.IndexModel = {};
            $scope.OrderDetail = {};
            $scope.currentOrder = 0;

            $scope.OrderFiltered = [];

            $scope.Role = 0;

            $scope.ConfirmActionTitle = '';
            $scope.ConfirmAction = '';
            $scope.SelectedOrderId = 0;
            $scope.ResonNote = "";
            $scope.IsShowDetail = false;

            $scope.Init = function (obj) {
                if (obj) {
                    $scope.ListData = [];
                    $scope.IndexModel = obj;
                    $scope.page.totalItems = 0;
                    $scope.page.currentPage = 1;
                    $scope.Role = obj.Role;
                    setDefaultTab();
                }
            };

            $scope.InitTab = function (status, requestType) {
                $scope.ListData = [];
                $scope.page.totalItems = 0;
                $scope.page.currentPage = 1;
                $scope.SearchOrder.Status = status;
                $scope.SearchOrder.RequestType = requestType;
                $scope.getListData();

            };

            var setDefaultTab = function (role) {
                if ($scope.Role == $scope.RolesEnum.Kitchen || $scope.Role == $scope.RolesEnum.KitchenManager) {
                    $('#tab_3').click();
                    $scope.InitTab(2, -1);
                }
                else if ($scope.Role == $scope.RolesEnum.Cashier) {
                    $('#tab_5').click();
                    $scope.InitTab(4, -1);
                }
                else {
                    if ($scope.IndexModel.Status == 0) {
                        $('#tab_1').click();
                        $scope.InitTab(0, -1);
                    } else if ($scope.IndexModel.Status == 1) {
                        $('#tab_2').click();
                        $scope.InitTab(1, -1);
                    } else {
                        $('#tab_1').click();
                        $scope.InitTab(0, -1);
                    }
                }
            }

            $scope.ListData = [];

            //Get list data
            $scope.getListData = function () {
                $scope.SearchOrder.FromCreatedDate = $("#txtFromCreatedDate").val();
                $scope.SearchOrder.ToCreatedDate = $("#txtToCreatedDate").val();
                $scope.SearchOrder.PageIndex = $scope.page.currentPage;
                $scope.SearchOrder.PageSize = $scope.page.itemsPerPage;
                Service.post("/Order/Search", { model: $scope.SearchOrder })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.ListData = response.Data;
                            $scope.page.totalItems = response.TotalRow;
                        } else {
                            $scope.ListData = null;
                            $scope.page.totalItems = 0;
                        }
                    });
            };

            //Get list district data
            $scope.getListDistrictData = function () {
                $scope.SearchOrder.PageIndex = $scope.page.currentPage;
                $scope.SearchOrder.PageSize = $scope.page.itemsPerPage;
                Service.post("/Order/Search", { model: $scope.SearchOrder })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.ListData = response.Data;
                            $scope.page.totalItems = response.TotalRow;
                        }
                    });
            };

            // Duyệt đơn
            $scope.doAproveOrder = function (orderId) {
                $scope.ConfirmActionTitle = "Duyệt đơn";
                $scope.ConfirmAction = 'AproveOrder';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            //Bếp nhận
            $scope.doKitchenAccept = function (orderId) {
                $scope.ConfirmActionTitle = "Bếp nhận nấu";
                $scope.ConfirmAction = 'KitchenAccept';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            //Bếp đã nấu xong
            $scope.doKitchenDone = function (orderId) {
                $scope.ConfirmActionTitle = "Đã nấu xong";
                $scope.ConfirmAction = 'KitchenDone';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            //Nhận tiền
            $scope.doCashierReceive = function (orderId) {
                $scope.ConfirmActionTitle = "Nhận tiền";
                $scope.ConfirmAction = 'CashierReceive';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            //CSKH yêu cầu hủy đơn
            $scope.doRequestDestroy = function (orderId) {
                $scope.ConfirmActionTitle = "CS báo hủy";
                $scope.ConfirmAction = 'RequestDestroy';
                $scope.SelectedOrderId = orderId;
                $('#txtConfirmWithReason').val('');
                $('#modelConfirmWithReason').modal();
            };

            // CS đã báo khách
            $scope.doRequestConfirmCustomer = function (orderId) {
                $scope.ConfirmActionTitle = "CS đã báo khách";
                $scope.ConfirmAction = 'RequestConfirmCustomer';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            //Hủy đơn
            $scope.doDestroy = function (orderId) {
                $scope.ConfirmActionTitle = "Hủy đơn";
                $scope.ConfirmAction = 'Destroy';
                $scope.SelectedOrderId = orderId;
                // nếu Hủy đơn CS báo hủy => không cần nhập lý do
                if ($scope.SearchOrder.RequestType == $scope.RequestTypeEnum.CSRequestDestroy) {
                    $('#txtConfirmWithReason').val('');
                    $('#modelConfirm').modal();
                }
                else {
                    $('#txtConfirmWithReason').val('');
                    $('#modelConfirmWithReason').modal();
                }
            };

            //In hóa đơn
            $scope.doPrintKitchen = function (orderId) {
                $scope.ConfirmActionTitle = "In hóa đơn";
                $scope.ConfirmAction = 'PrintKitchen';
                $scope.SelectedOrderId = orderId;
                $('#modelConfirm').modal();
            };

            var AproveOrder = function (orderId) {
                Service.post("/Order/AproveOrder", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Duyệt đơn thành công",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var KitchenAccept = function (orderId) {
                Service.post("/Order/KitchenAccept", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {
                            // xử lý call api của máy in
                            console.log("call máy in kitchen =>");
                            Service.post("/Order/CallPrinter", { orderId: orderId, type: 1 })
                                .then(function (response) {                                    
                                    console.log("call máy in kitchen done");
                                    if (response.Success) {
                                        notify({
                                            message: "Đang in đơn",
                                            classes: $scope.classes.Success,
                                            position: $scope.position,
                                            duration: $scope.duration
                                        });
                                        window.setTimeout(function () {
                                            $scope.getListData();
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


                            notify({
                                message: "Bếp đã nhận nấu",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var KitchenDone = function (orderId) {
                Service.post("/Order/KitchenDone", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {

                            // xử lý call api của máy in
                            console.log("call máy in cashier =>");
                            Service.post("/Order/CallPrinter", { orderId: orderId, type: 2 })
                                .then(function (response) {
                                    console.log("call máy in cashier done");
                                    if (response.Success) {
                                        notify({
                                            message: "Đang in đơn",
                                            classes: $scope.classes.Success,
                                            position: $scope.position,
                                            duration: $scope.duration
                                        });
                                        window.setTimeout(function () {
                                            $scope.getListData();
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

                            notify({
                                message: "Xác nhận đã nấu xong",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var CashierReceive = function (orderId) {
                Service.post("/Order/CashierReceive", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Xác nhận đã nhận tiền thành công",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var RequestDestroy = function (orderId, mess) {
                Service.post("/Order/RequestDestroy", { orderId: orderId, reasonNote: mess })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Đã gửi yêu cầu hủy đơn",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var RequestConfirmCustomer = function (orderId) {
                Service.post("/Order/RequestConfirmCustomer", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Xác nhận đã báo lại khách",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var Destroy = function (orderId, mess) {
                Service.post("/Order/Destroy", { orderId: orderId, reasonNote: mess })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Hủy đơn thành công",
                                classes: $scope.classes.Success,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            window.setTimeout(function () {
                                $scope.getListData();
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
            };

            var PrintKitchen = function (orderId) {
                // xử lý call api của máy in
                notify({
                    message: "Đã xác nhận in hóa đơn. Xin chờ đợi!",
                    classes: $scope.classes.Success,
                    position: $scope.position,
                    duration: 5000
                });
            };


            $scope.doConfirm = function (func) {
                var id = $scope.SelectedOrderId;

                if (func == "AproveOrder") {
                    AproveOrder(id);
                }
                if (func == "KitchenAccept") {
                    KitchenAccept(id);
                }
                if (func == "KitchenDone") {
                    KitchenDone(id);
                }
                if (func == "CashierReceive") {
                    CashierReceive(id);
                }
                if (func == "RequestDestroy") {
                    var mess = $('#txtConfirmWithReason').val();
                    RequestDestroy(id, mess);
                }
                if (func == "RequestConfirmCustomer") {
                    RequestConfirmCustomer(id);
                }
                if (func == "PrintKitchen") {
                    PrintKitchen(id);
                }
                if (func == "Destroy") {
                    var mess = $('#txtConfirmWithReason').val();
                    Destroy(id, mess);
                }

                $('#modelConfirm').modal("hide");
                $('#modelConfirmWithReason').modal("hide");
            };

            $scope.doCancel = function () {
                $scope.ConfirmAction = '';
                $scope.SelectedOrderId = 0;
                $scope.ResonNote = "";
            }

            $scope.roundNumberPaging = function (itemPerPage, totalItem) {
                return Math.ceil(parseFloat(totalItem) / itemPerPage);
            };

            $scope.doDelete = function (encryptId) {
                if (confirm("Bạn có chắc chắn muốn xóa đơn hàng?")) {
                    Service.post("/Order/Delete", { encryptId: encryptId })
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

            $scope.getById = function (encryptId) {
                location.href = "/Order/Update?encryptId=" + encryptId;
                //Service.post("/Order/Update", { encryptId: encryptId })
                //    .then(function (response) {
                //        if (response.Success) {
                //            $scope.Order = response.Data;
                //            $("#listview").hide();
                //            $("#detailview").show();
                //        }
                //    });
            };

            $scope.getOrderById = function (orderId) {
                Service.post("/Order/GetOrderById", { orderId: orderId })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.OrderDetail = response.Data;
                            $scope.IsShowDetail = true;
                        }
                    });
            };


            $scope.doSearch = function () {
                $scope.page.currentPage = 1;
                $scope.getListData();
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


            $scope.backtolist = function () {
                $scope.IsShowDetail = false;
            }
        });
})();

$(document).ready(function () {
    $("#Ordermanager").addClass("active");
    $("#Ordermanager ul.child_menu").show();
});


