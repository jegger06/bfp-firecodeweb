app.controller('TruckDashCtrl', function ($scope, $http) {
   

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
        //Truck Age Group
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetTruckAgeGroupCount?sMunicipalityId=' + municipalityId
        }).then(function (response) {
            $scope.TruckAgeGroup = response.data;
            
            $scope.TruckAgeGroup[0].Age = "0 - 24";
            $scope.TruckAgeGroup[1].Age = "25 and older";
            $scope.TruckAgeGroup[2].Age = "TOTAL";

        }, function (response) {

        });
        //Truck Chart
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetTruckCountDetails?sMunicipalityId=' + municipalityId
        }).then(function (response) {
            $scope.TruckStatusDetails = response.data;
        }, function (response) {

        });
    };
});
