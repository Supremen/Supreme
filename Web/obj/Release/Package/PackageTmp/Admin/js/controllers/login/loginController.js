
//var adminloginController = angular.module('loginController',[]);

app.controller("adminloginController", ["$scope", function ($scope) {
    console.log('loginController');
    $scope.username = 'admin';

    $scope.SignIn = function () {
        location = "#/index";
    }

}]);