
var tagBundleModule = angular.module("TagBundleUtil", ['angular-clipboard']).controller
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

    // move val from src array to target array returning next value in src array to focus
    var move = function (arrSrc, arrTrg, val) {

        var nextVal2focus = getNextVal(arrSrc, val);
        arrSrc = arrSrc.filter(t=> t != val);
        arrTrg = arrTrg.concat(val);

        return { arrSrc: arrSrc, arrTrg: arrTrg, val2focus: nextVal2focus };
    }

    //get index of value in src array, then use this index to get next value
    var getNextVal = function (arrSrc, val) {
        var fndIdx = getIdx(arrSrc, val);
        var result = fndIdx > 0 ? arrSrc[fndIdx - 1] :
                arrSrc.length > 1 ? arrSrc[fndIdx + 1] : null;

        return result;
    }

    var getIdx = function (arrSrc, val) {
        return arrSrc.findIndex(t=> t == val);
    }

    //hook tagmover routine to element's keyup event 
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
    
    //state transitions are provided by $window service
    //keys in state transtions dictionary represent element ids (see select boxes below) 
    //to which we hook our keyup event handlers
    $scope.SetStateTranstions = function () {
        $scope.states_transition_matrix = $window.states_dict;
        $scope.arrowKeySrcSelectors = Array.from($scope.states_transition_matrix.keys());
        //wire arrowKeyHandler to select boxes
        angular.forEach($scope.arrowKeySrcSelectors, function (selector) { arrowKeyHandler(selector); });
    }

    $scope.SaveTagBundleAndExcludeList = function () {
        resolvePromise(tagRepository.saveTagBundle
                        ($scope.selectedTagBundle, $scope.topTags,
                         $scope.bookmarksCollection)
                       , function (response) {
                           console.log("SaveTagBundleAndExcludeList, response status", response.status);
                       });

        $scope.saveExcludeList();
    }

    $scope.addEditTagBundleAndExcludeList = function () {
        
        if ($scope.isNewTagBundle()) {
            resolvePromise(tagRepository.createTagBundle($scope.newTagBundleName, $scope.bookmarksCollection)
                        , function (response) {
                            $scope.selectedTagBundle = $scope.newTagBundleName;
                            $scope.exclTags = $scope.exclTagsText.split(',');
                            $scope.$apply();
                            $scope.saveExcludeList();
                        });
        } else {
            $scope.saveExcludeList();
        }   
    }

    $scope.saveExcludeList = function () {
        resolvePromise(tagRepository.saveExcludeList($scope.selectedTagBundle,
                                                     $scope.exclTags, $scope.bookmarksCollection)
                       , function (response) {
                           console.log("saveExcludeList, response status", response.status);
                       });
    }
    
    var resolvePromise = function (promise, successFn) {
        Rx.Observable.fromPromise(promise)
                    .subscribe(successFn, function (err) {
                        console.log('Error: %s', err);
                    }
                    , null);
    }

    $scope.SetMostFrequentTags = function (bundleName) {
        
        var promise = tagRepository.getMostFrequentTags(bundleName
                            , $scope.GetBookmarksCollectionName()
                            , $scope.threshold);
                
        resolvePromise(promise, function (response) {
            $scope.freqTags = response.data; //console.log("freq tags",response.data);
            $scope.$apply();
        });
       
    }
   
    $scope.SetTagAssociations = function (bundleName) {
        var promise = tagRepository.getTagAssociations
                        (bundleName, $scope.GetBookmarksCollectionName());
        
        resolvePromise(promise, function (response) {
            $scope.associatedTags = response.data;
            console.log("associated tags", response.data);
            $scope.$apply();
        });
    }
    
    $scope.ReloadPage = function (selectedTagBundle) {
        //set url    
        $location.search({ 'tagBundle': selectedTagBundle });
        //now reload
        $window.location.reload();
    }

    $scope.GetSlctdTagBundle = function (firstBundle) {
        console.log("first bundle", firstBundle);
        return $location.search()['tagBundle'] ? $location.search()['tagBundle']
                                    : firstBundle || 'new';        
    }

    $scope.GetBookmarksCollectionName = function () {
        //this is a stub
        return 'default';
    }

    $scope.InitPage = function (funcArray) {

        var bookmarksCollectionName = $scope.GetBookmarksCollectionName();

        var promise = tagRepository.getTagBundles(bookmarksCollectionName);

        resolvePromise(promise, function (response) {
            $scope.existingTagBundles = response.data;
            console.log("tag bundles", response.data);
            var slctBundle = $scope.GetSlctdTagBundle(response.data ? response.data[0] : null);
            angular.forEach(funcArray, function (func)
            {
                func(slctBundle);
            });
            
            $scope.selectedTagBundle = slctBundle;
            $scope.bookmarksCollection = bookmarksCollectionName;
            
            $scope.$apply();
        });
    }

    $scope.SetTagBundle = function (bundleName) {
        
        var promise = tagRepository.getTagBundle
                                   (bundleName, $scope.GetBookmarksCollectionName());

        resolvePromise(promise, function (response) {
            $scope.topTags = response.data;
            console.log("top tags", response.data);
            $scope.$apply();
        });
    }

    $scope.SetExcludeList = function (bundleName) {
        
        if (bundleName == 'new')
        {
            $scope.exclTags = [];
            return;
        }

        var promise = tagRepository.getExcludeList
                                    (bundleName, $scope.GetBookmarksCollectionName());

        resolvePromise(promise, function (response) {
            $scope.exclTags = response.data;
            console.log("exclude list", response.data);
            $scope.$apply();
        });
    }
   
    $scope.pasteCopiedTags = function (list2edit) {        
        $scope[list2edit] = $scope.tagsToCopy;
        $scope.tagsToCopy || alert("nothing copied");
    }

    $scope.InitFreqTagsModel = function () {
        $scope.SetStateTranstions();
        $scope.InitPage
        ([$scope.SetTagBundle
        , $scope.SetExcludeList
        , $scope.SetMostFrequentTags]);        
    }

    $scope.InitAssociatedTagsModel = function () {
        $scope.SetStateTranstions();
        $scope.InitPage
        ([$scope.SetTagBundle
        , $scope.SetExcludeList
        , $scope.SetTagAssociations]);        
    }
    
    $scope.InitAddEditTagBundle = function () {        
        $scope.InitPage([$scope.SetExcludeList]);
    }

    }]).factory("tagRepository", ['$http', function ($http) {

            //these are stubs, to be replaced with real data
        var getTagBundle = function (bundleName, bookmarksCollectionName) {
                var promise = $http({
                    url: "http://localhost:57809/api/tags/GetTagBundle?bundleName=" + bundleName + "&bookmarksCollectionName=" + bookmarksCollectionName,
                    method: "GET"                    
                });

                return promise;
            };

        var getTagBundles = function (bookmarksCollection) {
            var promise = $http({
                url: "http://localhost:57809/api/tags/GetTagBundles?bookmarksCollectionName="+bookmarksCollection,
                method: "GET"
            });

            return promise;
            };

        var getMostFrequentTags = function (bundleName, bookmarksCollectionName, threshold) {
                            
                var promise = $http({
                    url: "http://localhost:57809/api/tags/CalculateMostFreqTerms",
                    method: "POST",
                    data:
                   {
                       "Threshold": threshold
                     , "BundleName": bundleName
                     , "BookmarksCollectionName": bookmarksCollectionName
                   }
                });

                return promise;                
            };

            var getTagAssociations = function (bundleName, bookmarksCollectionName) {
                var promise = $http({
                    url: "http://localhost:57809/api/tags/CalculateAssociatedTerms",
                    method: "POST",
                    data:
                   {
                       "BundleName": bundleName
                     , "BookmarksCollectionName": bookmarksCollectionName
                   }
                });

                return promise;                
            }

            var getExcludeList = function (bundleName, bookmarksCollectionName) {

                var promise = $http({
                    url: "http://localhost:57809/api/tags/GetExcludeList?bundleName=" + bundleName + "&bookmarksCollectionName=" + bookmarksCollectionName,
                    method: "GET"
                });

                return promise;
            };
            
            var saveExcludeList = function (tagBundleName, exclTags, bookmarksCollectionName) {

                console.log('tagBundle in saving exclTags', tagBundleName);
                var promise = $http({
                    url: "http://localhost:57809/api/tags/SaveExcludeList",
                    method: "POST",
                    data:
                   {
                       "BundleName": tagBundleName
                     , "BookmarksCollectionName": bookmarksCollectionName
                     , "ExcludeList": exclTags
                   }
                });

                return promise;
            }

            var saveTagBundle = function (tagBundleName, topTags, bookmarksCollectionName) {
                
                console.log('tagBundle in saving toptags', tagBundleName);
                var promise = $http({
                    url: "http://localhost:57809/api/tags/SaveTagBundle",
                    method: "POST",
                    data:
                   {
                       "TagBundle": topTags
                     , "BundleName": tagBundleName
                     , "BookmarksCollectionName": bookmarksCollectionName                     
                   }
                });

                return promise;
            }

            var createTagBundle = function (tagBundleName, bookmarksCollectionName) {
                console.log('created new tag bundle', tagBundleName);

                var promise = $http({
                    url: "http://localhost:57809/api/tags/CreateTagBundle",
                    method: "POST",
                    data:
                   {
                       "BundleName": tagBundleName
                     , "BookmarksCollectionName": bookmarksCollectionName
                   }
                });

                return promise;
            }

            var tagService = {
                getTagBundle: getTagBundle,
                getTagBundles: getTagBundles,
                getMostFrequentTags: getMostFrequentTags,
                getTagAssociations: getTagAssociations,
                getExcludeList: getExcludeList,
                saveExcludeList: saveExcludeList,
                saveTagBundle: saveTagBundle,
                createTagBundle: createTagBundle
            };
        
            return tagService;
        }]);



