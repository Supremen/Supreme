

var adminloginService = angular.module('adminloginService', ['commonService']);

adminloginService.factory('loginService', ['commService', function (commService) {
    var _this = this;

    /*
    [登陆]
    */
    //_this.AdminLogin = function (userName,userPwd,callBack) {
    //    commService.postData(
    //        '',
    //        {
    //            username: userName,
    //            userpwd:userPwd
    //        },
    //        function (data)
    //        {
    //            callBack(data);
    //        });
    //}

    return _this;
}]);