

/*
    LoginCtrl .
*/

var adminloginController = angular.module('adminloginCtrl', ['adminloginService']);

adminloginController.controller('loginCtrl', ['$rootScope', '$scope', 'loginService', function ($rootScope, $scope, loginService) {
    console.log('loginController');

    $scope.SignIn = function () {
        loginService.AdminLogin(
            $scope.user,
            function (data) {
                if (data.status) {
                   location.href = "#/home";
                } else {
                    Alert(data.msg,2);
                }
            });
    }
}]);