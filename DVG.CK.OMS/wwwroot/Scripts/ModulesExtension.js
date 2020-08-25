'use strict';
var baseService = angular.module('Common.Service', []);
var appService = function ($q, $http) {
    var services = {
        post: function (url, obj) {
            var d = $q.defer();
            //$http({
            //    method: 'POST',
            //    url: url,
            //    data: obj
            //}).success(function (data) {
            //    var pathname = window.location.pathname;
            //    var url = data.url !== undefined ? data.url : '/dang-nhap';
            //    if (data.login !== undefined && !data.login) {
            //        window.location.href = url + "?returnUrl=" + pathname;
            //    } else if (data.access !== undefined && !data.access) {
            //        window.location.href = url;
            //    }
            //    d.resolve(data);

            //    setTimeout(function () {
            //        $('[data-toggle=tooltip]').hover(function () {
            //            // on mouseenter
            //            $(this).tooltip('show');
            //        }, function () {
            //            // on mouseleave
            //            $(this).tooltip('hide');
            //        });
            //    }, 1000);
            //}).error(function (reason) {
            //    d.reject(reason);
            //});
            $.ajax({
                url: url,
                type: 'POST',
                data: obj
            }).done(function (data) {
                var pathname = window.location.pathname;
                var url = data.url !== undefined ? data.url : '/dang-nhap';
                if (data.login !== undefined && !data.login) {
                    window.location.href = url + "?returnUrl=" + pathname;
                } else if (data.access !== undefined && !data.access) {
                    window.location.href = url;
                }
                d.resolve(data);

                setTimeout(function () {
                    $('[data-toggle=tooltip]').hover(function () {
                        // on mouseenter
                        $(this).tooltip('show');
                    }, function () {
                        // on mouseleave
                        $(this).tooltip('hide');
                    });
                }, 1000);
            });
            return d.promise;
        }
    };
    return services;
};
appService.$inject = ["$q", "$http"];
baseService.factory("Service", appService);


var resetValueTextbox = function () {

    $('input[type="text"]').val("");

}