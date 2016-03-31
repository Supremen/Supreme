
var commonService = angular.module("commonService", ["ngStorage"]);

commonService.factory('commService', ['$http', '$rootScope', '$localStorage',
    function ($http, $rootScope, $localStorage) {
    var commService = {};
    commService.postData = function (url,reqData,callBack) {
        $http({
            method: 'POST',
            url: url,
            data: commService.Params(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (data) {
            callBack(data);
        });
    }

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

    return commService;
}]);