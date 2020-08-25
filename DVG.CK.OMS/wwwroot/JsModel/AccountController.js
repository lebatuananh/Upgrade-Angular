(function () {
    angular.module('Account', ["ui.bootstrap", "Common.Service"])
        .controller('AccountController', function ($scope, $http, Service) {
            // START
            //string email, string password, string returnUrl = null, bool isSavedPassword = false
            $scope.Model = {
                email: '',
                password: '',
                isSavedPassword: false
            };
            $scope.returnUrl = '';

            validator.message.date = 'not a real date';

            $scope.init = function (returnUrl) {
                $scope.returnUrl = returnUrl;

                // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
                $('form')
                  .on('blur', 'input[required], input.optional, select.required', validator.checkField)
                  .on('change', 'select.required', validator.checkField)
                  .on('keypress', 'input[required][pattern]', validator.keypress);
            };

            $scope.doLogin = function () {
                console.log(Service);
                Service.post("/Account/DoLogin", { email: $scope.Model.email, password: $scope.Model.password, isSavedPassword: $scope.Model.isSavedPassword })
                    .then(function (response) {
                        if (response.status) {
                            return location.href = $scope.returnUrl;
                        } else {
                            alert(response.message);
                        }
                    });
            };

            $scope.ShortSubmit = function (event) {
                if (event.which === 13)
                    $scope.doLogin();
            };

            $scope.ChangePasswordModel = {
                CurrentPassword: '',
                Password: '',
                ConfirmPassword: ''
            };

            $scope.WhenSubmitChange = function (event) {
                if (event.which === 13)
                    $scope.doChangePassword();
            };

            $scope.doChangePassword = function () {
                Service.post("/Account/ChangePassword", { saveModel: $scope.ChangePasswordModel })
                    .then(function (response) {
                        if (response.status) {
                            return location.reload();
                        } else {
                            alert(response.message);
                        }
                    });
            };

            // END
        });
})();