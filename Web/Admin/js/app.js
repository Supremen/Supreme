var app = angular.module("myApp", ['ngRoute', 'adminloginController']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/adminlogin', {
            templateUrl:'/adminlogin.html'
        }).
        when('/home',{
            templateUrl:'/Admin/html/home.html'
        }).
        otherwise({
            redirectTo: '/adminlogin'
        })
}]);