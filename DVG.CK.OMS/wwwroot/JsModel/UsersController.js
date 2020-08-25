'use strict';
var usersApp = angular.module('Users', ["ui.bootstrap", "Common.Service", 'ngSanitize', 'cgNotify']);

// Create the factory that share the Data
usersApp.factory('Data', function () {
    return { Users: {} };
});

(function () {
    usersApp.controller('UsersController', function ($scope, $rootScope, $http, Service, $sce, notify, Data) {

        // Position of message
        $scope.positions = {
            "Center": 'center',
            "Left": 'left',
            "Right": 'right'
        };
        // Class of message
        $scope.classes = {
            "Success": 'alert-success',
            "Error": 'alert-danger'
        };
        // The time display of message
        $scope.duration = 2000;

        $scope.page = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 15
        };

        $scope.ListUserType = {
            "ListStatus": [],
            "CurrentType": 0
        };

        $scope.UsersStatus = {
            "Active": 1,
            "Lock": 0
        };

        $scope.ResetPassWordModel = {
            'UserId': 0,
            'FullName': "",
            'Email': "",
            "NewPassword": ""
        };

        $scope.CatePermissionModel = {
            'UserId': 0,
            'Username': "",
            'ListPermistionSelected': [],
            "ListPermistion": []
        };

        $scope.Search = {};

        $scope.UsersModel = Data.Users;
        $scope.OpenFormUpdate = false;

        $scope.Init = function (obj) {

            validator.defaults.classes.alert = true;
            // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
            $('form')
              .on('blur', 'input[required], input.optional, select.required', validator.checkField)
              .on('change', 'select.required', validator.checkField)
              .on('keypress', 'input[required][pattern]', validator.keypress);

        };

        //$scope.resetFromView = function () {
        //    $(window).scrollTop(0);
        //    $RIGHT_COL.css('min-height', $(".left_col").outerHeight());
        //}

        $scope.ListData = [];

        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            $scope.Search.UserType = $scope.ListUserType.CurrentType;
            Service.post("/Users/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListData = response.Data;
                        if ($scope.ListData !== null && $scope.ListData.length > 0) {
                            $scope.page.totalItems = response.TotalRow;
                        } else {
                            $scope.page.totalItems = 0;
                        }
                    }
                });
        };

        //Get debit card status
        $scope.getListUsersType = function () {
            Service.post("/Users/GetListUserType", {})
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListUserType.ListStatus = response.Data.UsersTypeList;
                    }
                });
        };

        $scope.getListData();
        $scope.getListUsersType();

        $scope.getUserById = function (userId) {
            Service.post("/Users/GetUserById", { userId: userId })
                .then(function (response) {
                    if (response.Success) {
                        Data.Users = response.Data;
                        //$scope.resetFromView();
                    } else {
                        Data.Users = {};
                    }
                    $scope.OpenFormUpdate = true;
                });
        };

        //Update status credit card
        $scope.updateStatus = function (userId, status) {
            var msgConfirm = "";
            var msgSuccess = "";
            if (status === $scope.UsersStatus.Active) {
                msgConfirm = "Bạn có chắc chắn muốn khóa tài khoản?";
                msgSuccess = "Đã khóa tài khoản.";
            } else {
                msgConfirm = "Bạn có chắc chắn muốn mở khóa tài khoản?";
                msgSuccess = "Đã mở khóa tài khoản.";
            }

            if (confirm(msgConfirm)) {
                Service.post("/Users/UpdateStatus", { userId: userId })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.message = msgSuccess;
                            notify({
                                message: $scope.message,
                                classes: $scope.classes.Success,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });

                            setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);

                        } else {
                            notify({
                                message: "Lỗi! Cập nhật không thành công",
                                classes: $scope.classes.Error,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                        }
                    });
            }
        };

        //Update status credit card
        $scope.openResetPassword = function (userId, fullname, email) {
            $scope.ResetPassWordModel.UserId = userId;
            $scope.ResetPassWordModel.FullName = fullname;
            $scope.ResetPassWordModel.Email = email;
            $scope.ResetPassWordModel.NewPassword = "";
            $("#modelChangePass").modal("show");
        };

        $scope.openCatePermission = function (userId, username, userType) {
            $scope.CatePermissionModel.UserId = userId;
            $scope.CatePermissionModel.Username = username;
            $scope.ListUserType.CurrentType = userType;
            $("#modelCatePermission").modal("show");
        };

        $scope.toggleSelection = function (permisId) {

            if ($scope.CatePermissionModel.ListPermistionSelected === null) {
                $scope.CatePermissionModel.ListPermistionSelected = [];
            }
            var idx = $scope.CatePermissionModel.ListPermistionSelected.indexOf(permisId);

            // is currently selected
            if (idx > -1) {
                $scope.CatePermissionModel.ListPermistionSelected.splice(idx, 1);
            }

                // is newly selected
            else {
                $scope.CatePermissionModel.ListPermistionSelected.push(permisId);
            }

        };

        $scope.checkedSelection = function (permisId) {
            return $scope.CatePermissionModel.ListPermistionSelected !== null && $scope.CatePermissionModel.ListPermistionSelected.indexOf(permisId) > -1;
        };


        //Update status credit card
        $scope.autoGeneratePassword = function () {

            Service.post("/Users/GeneratePassWord", {})
                .then(function (response) {
                    if (response.Success) {
                        $scope.ResetPassWordModel.NewPassword = response.Data;
                    }
                });
        };

        //Update status credit card
        $scope.resetPassword = function () {
            var password = $scope.ResetPassWordModel.NewPassword;
            var userId = $scope.ResetPassWordModel.UserId;

            Service.post("/Users/ResetPassWord", { userId: userId, passWord: password })
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "Đã đặt lại mật khẩu thành công.";
                        notify({
                            message: $scope.message,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });

                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);

                    } else {
                        notify({
                            message: "Lỗi! Cập nhật không thành công",
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }

                    $("#modelChangePass").modal("hide");
                });
        };

        $scope.changeCatePermission = function () {

            Service.post("/Users/UpdateCatePermission", { userId: $scope.CatePermissionModel.UserId, userType: $scope.ListUserType.CurrentType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "Phân quyền mật khẩu thành công.";
                        notify({
                            message: $scope.message,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });

                        $("#modelCatePermission").modal("hide");

                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);

                    } else {
                        notify({
                            message: "Lỗi! Cập nhật không thành công",
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }

                    $("#modelChangePass").modal("hide");
                });
        };

        $rootScope.$on("CallCancleUpdateMethod", function () {
            $scope.cancleUpdate();
        });

        $scope.cancleUpdate = function () {
            $scope.getListData();
            $scope.OpenFormUpdate = false;
            $(window).scrollTop(0);
        };

        // Click button search
        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            ; $scope.getListData();
        };

        //Key press
        $scope.keypressAction = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.page.currentPage = 1;
                $scope.getListData();
            }
        };

        // html filter (render text as html)
        $scope.trustedHtml = function (plainText) {
            return $sce.trustAsHtml(plainText);
        };

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        };

    });
})();