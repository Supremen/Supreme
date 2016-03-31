var app = angular.module("myApp", ['ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/adminlogin', {
            templateUrl:'/adminlogin.html'
        }).
        when('/index',{
            templateUrl:'/Admin/html/Index.html'
        }).
        otherwise({
            redirectTo: '/adminlogin'
        })
}]);