
/*
    HandlerUrl
*/
var baseHandlerPath = '/';
var baseHandlers = {
    loginHandler: baseHandlerPath + 'Admin/handler/NoLoginHandler.ashx',
    homeHandler:  baseHandlerPath + 'Admin/handler/power/Menu.ashx'
};


/*
   Services [Common]
*/
var commonService = angular.module("commonService", []);

commonService.factory('commService', ['$http', '$rootScope',
    function ($http, $rootScope) {
    var commService = {};
        /**
         * [urlParams get参数处理]
         * @param  {[type]} url      [description]
         * @param  {[type]} jsonData [description]
         * @return {[type]}          [description]
         */
    commService.urlParams = function (url, jsonData) {
        var result = url + '?';
        var i = 0;
        var keys = Object.keys(jsonData);
        for (var i = 0; i < keys.length; i++) {
            if (i != 0) {
                result += '&';
            }
            result += keys[i] + '=' + jsonData[keys[i]];
        }
        return result;
    };
        /**
         * [urlParams get参数处理]
         * @param  {[type]} url      [description]
         * @param  {[type]} jsonData [description]
         * @return {[type]}          [description]
         */
    commService.Params = function (jsonData) {
        var result = "";
        var i = 0;
        var keys = Object.keys(jsonData);
        for (var i = 0; i < keys.length; i++) {
            if (i != 0) {
                result += "&";
            }
            result += keys[i] + '=' + jsonData[keys[i]];
        }
        return result;
    };

    commService.extend = function (reqData, option) {
        var keys = Object.keys(option);
        for (var i = 0; i < keys.length; i++) {
            if (option[keys[i]] || option[keys[i]] == 0) reqData[keys[i]] = option[keys[i]];
        }
    };
        /**
         * [urlParams get获取]
         * @param  {[type]} url      [description]
         * @param  {[type]} jsonData [description]
         * @function  {[type]} callBack [description]
         * @function  {[type]} failCallBack [description]
         */
    commService.loadRemoteData = function (url, reqData, callBack, errorCallBack) {
        $http.get(commService.urlParams(url, reqData)).success(function (data) {
            callBack(data);
        }).error(function (data) {
            errorCallBack(data);
        });
    };
        /**
         * [urlParams post提交]
         * @param  {[type]} url      [description]
         * @param  {[type]} jsonData [description]
         * @function  {[type]} callBack [description]
         * @function  {[type]} failCallBack [description]
         */
    commService.postData = function (url, reqData, callBack) {
        $http({
            method: 'POST',
            url: url,
            data: commService.Params(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (data) {
            callBack(data)
        });
    };

    return commService;
}]);