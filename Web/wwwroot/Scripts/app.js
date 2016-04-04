
var tagBundleModule = angular.module("TagBundleUtil", []);

tagBundleModule.controller("tagBundleCtrl", function ($scope, $location) {
    //these are stubs, to be replaced with real data
    $scope.topTags = ['test1', 'test2', 'test3', 'test4'];
    $scope.freqTags = ['_test1', '_test2', '_test3', '_test4'];
    $scope.exclTags = ['__test1', '__test2', '__test3', '__test4'];

    $scope.existingTagBundles = ['cryptography','security','machine-learning','tools'];

    var tagMover = function (x) {
        var src_trg_arr = states_dict.get(x.srcId)[x.keyCode];
        var res = move($scope[src_trg_arr[0]], $scope[src_trg_arr[1]], x.slctValue);
        $scope[src_trg_arr[0]] = res.arrSrc;
        $scope[src_trg_arr[1]] = res.arrTrg;

        $scope.$apply();
        angular.element(x.srcId).val(res.val2focus);
    }

    var move = function (arrSrc, arrTrg, val) {

        var nextVal2focus = getNextVal(arrSrc, val);
        arrSrc = arrSrc.filter(t=> t != val);
        arrTrg = arrTrg.concat(val);

        return { arrSrc: arrSrc, arrTrg: arrTrg, val2focus: nextVal2focus };
    }

    var getNextVal = function (arrSrc, val) {
        var fndIdx = getIdx(arrSrc, val);
        var result = fndIdx > 0 ? arrSrc[fndIdx - 1] :
                arrSrc.length > 1 ? arrSrc[fndIdx + 1] : null;

        return result;
    }

    var getIdx = function (arrSrc, val) {
        return arrSrc.findIndex(t=> t == val);
    }

    var arrowKeyHandler = function (listIdSelector) {
        var srcList = $(listIdSelector);

        var arrowKeyUp = Rx.Observable.fromEvent(srcList, 'keyup')
                                      .filter(e=>e.keyCode == 37 || e.keyCode == 39)
                                      .map(function (e) {
                                          return {
                                              keyCode: e.keyCode
                                              , srcId: listIdSelector
                                              , slctValue: srcList.val()
                                          };
                                      });

        arrowKeyUp.subscribe(tagMover);
    }

    $scope.isNewTagBundle = function () {
        console.log("$scope.selectedTagBundle: " + $scope.selectedTagBundle);
        return !$scope.selectedTagBundle || $scope.selectedTagBundle == 'new';
    }

    angular.forEach(arrowKeySrcSelectors, function (selector) { arrowKeyHandler(selector); });
    
    $scope.selectedTagBundle = $location.search()['tagBundle'] ? $location.search()['tagBundle'] : 'new';
   
});