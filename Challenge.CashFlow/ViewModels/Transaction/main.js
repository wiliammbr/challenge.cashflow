var app = angular.module('myApp', ['ngTable', 'ngResource']);
var jListTable = $('#listTable');
jListTable.append('<div class="modal"></div>');

(function () {

    app.controller('transactionListCtrl', function (NgTableParams, $scope, $http, $resource) {
        this.tableParams = new NgTableParams({
            page: 1,
            count: 10,
            total: 200,
            sorting: {
                name: 'asc'
            },
            filter: {
                name: undefined
            }
        }, {
            getData: function (params) {
                var $deferred = $.Deferred();

                debugger;
                jListTable.addClass('loading');
                var dateFilter = params.filter().Date;
                if (dateFilter && dateFilter.length === 10) {
                    dateFilter = moment(params.filter().Date, 'DD/MM/YYYY').format('MM/DD/YYYY');
                } else {
                    dateFilter = '';
                }

                $http({
                    url: '/api/Transaction/GetTransactions',
                    method: 'GET',
                    params: {
                        Description: params.filter().Description,
                        Amount: params.filter().Amount,
                        PaymentType: params.filter().PaymentType,
                        Date: dateFilter,
                        PageNumber: params.page(),
                        PageSize: params.count(),
                        DescriptionSort: params.sorting().Description,
                        AmountSort: params.sorting().Amount,
                        PaymentTypeSort: params.sorting().PaymentType,
                        DateSort: params.sorting().Date
                    }
                })
                .success(function (data, status, headers, config) {
                    this.pagingData = JSON.parse(headers('paging-headers'));
                    params.total(pagingData.totalCount); // recal. page nav controls
                    jListTable.removeClass('loading');
                    // $scope.data = $scope.$data = data;
                    // this.data = this.dataset = data;
                    $deferred.resolve({
                        results: data,
                        inlineCount: pagingData.totalCount
                    });
                });
                return $deferred.promise().then(function (data) {
                    params.total(data.inlineCount); // recal. page nav controls
                    return data.results;
                });
            }
        });
        this.tableParams.reload();
    });

})();
