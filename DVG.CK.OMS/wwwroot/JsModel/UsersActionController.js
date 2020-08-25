
(function () {
    usersApp.controller('UpdateUsersController', function ($scope, $rootScope, $http, Service, $sce, notify, Data) {

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

        $scope.ListAllPermistion = {
            "ListPermistion": [],
            "ListPermistionSelected": []
        };

        // The time display of message
        $scope.duration = 2000;

        $scope.Search = {};
        $scope.PatternEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;
        $scope.PatternPhone = /^[0-9]{9,12}$/;
        $scope.PatternUserName = /^[a-zA-Z0-9-_]{6,}$/;


        $scope.UsersActionModel = Data.Users;


        $scope.Init = function () {

            $(".datepicker-short").datetimepicker({
                format: 'DD/MM/YYYY',
                locale: "vi",
                keepOpen: false,
                minDate: new Date(1800, 2 - 1, 1)
            });

            // giới tính
            if ($scope.UsersActionModel.Gender === "True" || $scope.UsersActionModel.Gender === true) {
                document.getElementById('rdoMale').checked = true;
            } else {
                document.getElementById('rdoFeMale').checked = true;
            }

            // Load giá trị mặc định cho các combobox
            if ($scope.UsersActionModel !== null && JSON.stringify($scope.UsersActionModel) !== "{}") {
                $scope.ListUserType.CurrentType = "" + $scope.UsersActionModel.UserType + "";
                $scope.ListAllPermistion.ListPermistionSelected = $scope.UsersActionModel.ListPermistionSelected;
            }

            $scope.getListUsersType();
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


        $scope.doUpdate = function () {
            $scope.UsersActionModel.BirthdayStr = $("#txtDob").val();
            $scope.UsersActionModel.Gender = document.getElementById("rdoMale").checked;
            //$scope.UsersActionModel.UserType = $scope.ListUserType.CurrentType;

            Service.post("/Users/Update", { users: $scope.UsersActionModel })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: "Đã cập nhật thông tin.",
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);
                    } else {
                        notify({
                            message: "Lỗi! " + response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                });

        };

        $scope.toggleSelection = function (permisId) {

            if ($scope.ListAllPermistion.ListPermistionSelected === null) {
                $scope.ListAllPermistion.ListPermistionSelected = [];
            }
            var idx = $scope.ListAllPermistion.ListPermistionSelected.indexOf(permisId);

            // is currently selected
            if (idx > -1) {
                $scope.ListAllPermistion.ListPermistionSelected.splice(idx, 1);
            }

                // is newly selected
            else {
                $scope.ListAllPermistion.ListPermistionSelected.push(permisId);
            }

        };

        $scope.checkedSelection = function (permisId) {
            return $scope.ListAllPermistion.ListPermistionSelected !== null && $scope.ListAllPermistion.ListPermistionSelected.indexOf(permisId) > -1;
        };

        $scope.cancleUpdate = function () {
            $rootScope.$emit("CallCancleUpdateMethod", {});
        }

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
                $("#avatarUsers").attr("src", result.FullPath).show();
                $scope.UsersActionModel.Avatar = result.FullPath;

            };
        };

        //Xóa ảnh avatar hoặc ảnh slide
        $scope.delImages = function (index) {
            $scope.UsersActionModel.Avatar = "/Content/images/no-image.png";
            $("#avatarUsers").attr("src", $scope.Article.Avatar).show();
        };

        //Xem ảnh gốc avatar hoặc ảnh slide
        $scope.previewImages = function (imageurl) {
            popupCenter(imageurl, "Ảnh gốc", 950, 600);
        };

        // html filter (render text as html)
        $scope.trustedHtml = function (plainText) {
            return $sce.trustAsHtml(plainText);
        }

    });
})();