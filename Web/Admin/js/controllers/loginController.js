

var adminloginController = angular.module('adminloginController', []);

adminloginController.controller('loginController', ['$scope', function ($scope) {
    console.log('loginController');

    $scope.SignIn = function () {
        //loginService.AdminLogin(
        //    user.username,
        //    user.pwd,
        //    function (data) {
        //        if (data) {
        //            location.href = "#/home";
        //        }
        //    });


        //location.href = "#/home";

    }

}]);