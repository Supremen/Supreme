

/*
    Serivces  [Login]
*/

var adminloginServices = angular.module('adminloginService', ['commonService']);

adminloginServices.factory('loginService', ['commService', function (commService) {
    var _this = this;

    /*
    [登陆]
    */
    _this.AdminLogin = function (model, callBack) {
        var reqData =
            {
                action: "AdminLogin"
            };
        commService.extend(reqData, model);
        commService.postData(
            baseHandlers.loginHandler,
            reqData,
            function (data)
            {
                callBack(data);
            });
    }

    return _this;
}]);