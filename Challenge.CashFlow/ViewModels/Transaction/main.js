var app = angular.module('myApp', ['ngTable', 'ngResource']);
var jListTable = $('#listTable');
jListTable.append('<div class="modal"></div>');

(function () {

    app.controller('transactionListCtrl', function (NgTableParams, $scope, $http, $resource) {       

        this.amountFilterDef = {
            AmountFrom: {
                id: "number",
                placeholder: "From"
            },
            AmountTo: {
                id: "number",
                placeholder: "To"
            }
        };
        this.dateFilterDef = {
            DateFrom: {
                id: "text",
                placeholder: "From"
            },
            DateTo: {
                id: "text",
                placeholder: "To"
            }
        };

        this.dateFilterDef = {
            DateFrom: {
                id: "text",
                placeholder: "From"
            },
            DateTo: {
                id: "text",
                placeholder: "To"
            }
        };

        this.paymentTypes = [
            {
                'id': 'C',
                'title': 'Credit Card'
            },
            {
                'id': 'M',
                'title': 'Money'
            }
        ];
        
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
                jListTable.addClass('loading');
                debugger;
                $http({
                    url: '/api/Transaction/GetTransactions',
                    method: 'GET',
                    params: {
                        Description: params.filter().Description,
                        AmountFrom: params.filter().AmountFrom,
                        AmountTo: params.filter().AmountTo,
                        PaymentType: params.filter().PaymentType,
                        DateFrom: formatDate(params.filter().DateFrom),
                        DateTo: formatDate(params.filter().DateTo),
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

formatDate = function (date) {
    if (date && date.length === 10) {
        return moment(date, 'DD/MM/YYYY').format('MM/DD/YYYY');
    } else {
        return '';
    }
};

$(document).ready(function ($) {
    $('input[type=text][name*=Date]').mask('99/99/9999');
});