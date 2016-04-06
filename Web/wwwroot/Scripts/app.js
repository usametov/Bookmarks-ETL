
var tagBundleModule = angular.module("TagBundleUtil", []).controller
    ("tagBundleCtrl", ['$scope', '$location', '$window', 'tagRepository'
    , function ($scope, $location, $window, tagRepository) {
        
    $scope.states_transition_matrix = {};
    $scope.threshold = 70;

    var tagMover = function (x) {
        //src array and target array names are specified in states transition matrix
        var src_trg_arr = $scope.states_transition_matrix.get(x.srcId)[x.keyCode];
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
       
    $scope.SetStateTranstions = function () {
        $scope.states_transition_matrix = $window.states_dict;
        $scope.arrowKeySrcSelectors = Array.from($scope.states_transition_matrix.keys());
        //wire arrowKeyHandler to select boxes
        angular.forEach($scope.arrowKeySrcSelectors, function (selector) { arrowKeyHandler(selector); });
    }

    $scope.SaveTagBundleAndExcludeList = function () {
        tagRepository.saveTagBundle($scope.selectedTagBundle, $scope.topTags);
        tagRepository.saveExcludeList($scope.selectedTagBundle, $scope.exclTags);
    }

    $scope.GetMostFrequentTags = function () {
        $scope.freqTags = tagRepository.getMostFrequentTags($scope.selectedTagBundle, $scope.threshold);
    }
    
    $scope.GetTagAssociations = function () {
        $scope.associatedTags = tagRepository.getTagAssociations($scope.selectedTagBundle);
    }

    $scope.SetSlctdTagBundle = function () {
        $scope.selectedTagBundle = $location.search()['tagBundle'] ? $location.search()['tagBundle']
                                    : $scope.existingTagBundles ? $scope.existingTagBundles[0]
                                    : 'new';
    }

    $scope.InitFreqTagsModel = function () {
        $scope.SetStateTranstions();
        $scope.topTags = tagRepository.getTagBundle($scope.selectedTagBundle);
        $scope.GetMostFrequentTags();
        $scope.exclTags = tagRepository.getExcludeList($scope.selectedTagBundle);        
        $scope.existingTagBundles = tagRepository.getTagBundles();
        $scope.SetSlctdTagBundle();
    }

    $scope.InitAssociatedTagsModel = function () {
        $scope.SetStateTranstions();
        $scope.topTags = tagRepository.getTagBundle($scope.selectedTagBundle);        
        $scope.exclTags = tagRepository.getExcludeList($scope.selectedTagBundle);
        $scope.GetTagAssociations();
        $scope.existingTagBundles = tagRepository.getTagBundles();
        $scope.SetSlctdTagBundle();
    }
    
    }]).factory("tagRepository", ['$http', function ($http) {

            //these are stubs, to be replaced with real data
            var getTagBundle = function (tagBundle) {
                return ['test1', 'test2', 'test3', 'test4'];
            };

            var getTagBundles = function () {
                return ['cryptography', 'security', 'machine-learning', 'tools','linux'];
            };

            var getMostFrequentTags = function (excludeList, threshold) {
                console.log('getMostFrequentTags');
                return ['_test1', '_test2', '_test3', '_test4'];
            };

            var getTagAssociations = function (tagBundle, excludeList) {
                console.log('getTagAssociations');
                return ['_tst1_ass_', '_tst2_ass_', '_tst3_ass_', '_tst4_ass_'];
            }

            var getExcludeList = function (tagBundle) {
                return ['__test1', '__test2', '__test3', '__test4'];
            }
            
            var saveExcludeList = function (tagBundle, exclTags) {
                //call api here
                console.log('saving exclTags', exclTags);
            }

            var saveTagBundle = function (tagBundle, topTags) {
                //call api here
                console.log('saving topTags', topTags);
            }

            var tagService = {
                getTagBundle: getTagBundle,
                getTagBundles: getTagBundles,
                getMostFrequentTags: getMostFrequentTags,
                getTagAssociations: getTagAssociations,
                getExcludeList: getExcludeList,
                saveExcludeList: saveExcludeList,
                saveTagBundle: saveTagBundle
            };
        
            return tagService;
        }]);




