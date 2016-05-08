
/*
    Services [Home]
*/

var homeService = angular.module('homeServices', ['commonService']);

homeService.factory('homeService', ['commService', function (commService) {
    var _this = this;

    _this.GetMenuInfoList = function (callBack) {
        var reqData =
            {
                action: "List"
            };
        commService.postData(
            baseHandlers.homeHandler,
            reqData,
            function (data) {
                callBack(data);
            });
    }

    return _this;
}]);