var app = angular.module('myApp', []);

(function () {

    app.controller('transactionsCtrl', function ($scope, $http) {
        jMyApp = $('#overview').append('<div class="modal"></div>');
        jMyApp.addClass('loading');
        $http({
            url: '/api/Transaction/GetTransactionsOverview',
            method: 'GET',
            params: { numberOfDays: 30 }
        })
        .then(function (response) {
            if (response && response.data && response.data.length > 0) {
                $scope.items = response.data;
            } else {
                $scope.items = [
                    { Title: 'Today', Total: 0, TotalAmount: 0 },
                    { Title: 'Last 30 days', Total: 0, TotalAmount: 0 },
                ];
            }
            jMyApp.removeClass('loading');
        });
    });

})();
