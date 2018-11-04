app.controller('VehicleDashCtrl', function ($scope, $http) {

    $scope.LoadData = function () {
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        var municipalityId = getParameterByName('sMunicipalityId');

        //Vehicle Summary Chart
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetVehicleCountDetails?sMunicipalityId=' + municipalityId
        }).then(function (response) {
            $scope.VehicleSummaryDetails = response.data;
        }, function (response) {

        });
    };
});
