
/*
    HomeCtrl  
*/

var homeController = angular.module('homeCtrl', []);

//homeController.controller('homeCtrl', ['$rootScope', '$scope', 'homeService', function ($rootScope, $scope, homeService) {

//    console.log('homeCtrl');


//}]);

/*
    menuCtrl 左边导航栏
*/
homeController.controller('menuCtrl', ['$rootScope', '$scope', function ($rootScope, $scope) {
    console.log('menuCtrl');
    //homeService.GetMenuInfoList(function (data) {
    //    console.log(data);
    //});
}]);