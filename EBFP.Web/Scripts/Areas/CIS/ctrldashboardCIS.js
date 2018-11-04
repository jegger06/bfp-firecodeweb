app.controller('CapabilityDashCtrl', function ($scope, $http) {

    $scope.LoadData = function () {

        $scope.FireIncidentRespondedYears = [((new Date()).getFullYear() - 4), ((new Date()).getFullYear() - 3), ((new Date()).getFullYear() - 2), ((new Date()).getFullYear() - 1), (new Date()).getFullYear()];

        //Truck Age Group
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetTruckAgeGroupCount'
        }).then(function (response) {
            $scope.TruckAgeGroup = response.data;
            
            $scope.TruckAgeGroup[0].Age = "0 - 24";
            $scope.TruckAgeGroup[1].Age = "25 and older";
            $scope.TruckAgeGroup[2].Age = "TOTAL";

        }, function (response) {

        });


        //Fire Fighting capabilities
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireFightingCountDetails'
        }).then(function (response) {
            $scope.FireFightingCapability = response.data;
        }, function (response) {

        });

        //Fire Incident Responded To
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireIncidentRespondedTo'
        }).then(function (response) {
            $scope.FireIncidentRespondedTo = response.data;
        }, function (response) {

        });

        //Fire Incident Injured
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireIncidentInjured'
        }).then(function (response) {
            $scope.FireIncidentInjured = response.data;
        }, function (response) {

        });


        //Fire Incident Deaths
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireIncidentDeaths'
        }).then(function (response) {
            $scope.FireIncidentDeaths = response.data;
        }, function (response) {

        });

        //Fire Incident Damages
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireIncidentDamages'
        }).then(function (response) {
            $scope.FireIncidentDamages = response.data;
        }, function (response) {

        });


        //Fire Incidents Statistic
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFireIncidentsStatistics'
        }).then(function (response) {
            var year = 'CY ' + ((new Date()).getFullYear() - 4) + ' - CY ' + (new Date()).getFullYear();
            $scope.CYYear = year;

            $scope.FireIncidentStatistics = response.data;
           
        }, function (response) {

        });

        //Fire Incident Damages
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFirePreventionActivities'
        }).then(function (response) {
            $scope.FirePrevention = response.data;
        }, function (response) {

        });
        
        //FIRE SAFETY INSPECTION CONDUCTED ON ESTABLISHMENTS 
        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetFSICEstablishments'
        }).then(function (response) {
            $scope.FSICEstablishments = response.data;
           
        }, function (response) {

        });


        $http({
            method: "GET",
            url: '/CIS/Dashboard/GetPersonnelPerRegion'
        }).then(function (response) {
            $scope.PersonnelPerRegion = response.data;
            $scope.OfficerRanks = $scope.sum(response.data, 'OfficerRanks');
            $scope.NonOfficerRanks = $scope.sum(response.data, 'NonOfficerRanks');
            $scope.NonUniformedPersonnel = $scope.sum(response.data, 'NonUniformedPersonnel');
            $scope.TotalPersonnel = $scope.sum(response.data, 'TotalPersonnel');

            $scope.NUPAdminPersonnel = $scope.sum(response.data, 'NUPAdminPersonnel');
            $scope.NUPOperationPersonnel = $scope.sum(response.data, 'NUPOperationPersonnel');

            $scope.Total = $scope.NUPAdminPersonnel + $scope.NUPOperationPersonnel;
           
              if ($scope.Total > 0 && $scope.Total != null) {
                $scope.NUPAdminPersonnelPercent = Math.round(($scope.NUPAdminPersonnel * 100) / $scope.Total);
                $scope.NUPOperationPersonnelPercent = Math.round(($scope.NUPOperationPersonnel * 100) / $scope.Total);
              } else {
                $scope.NUPAdminPersonnelPercent = 0;
                $scope.NUPOperationPersonnelPercent = 0;
              }
                 
            $scope.TotalFireTrucks = $scope.sum(response.data, 'ActualFireTrucks');
            $scope.OverallTotalPersonnel = $scope.sum(response.data, 'TotalPersonnel');
        }, function (response) {
        });
        $scope.sum = function (items, prop) {
            return items.reduce(function (a, b) {
                return a + b[prop];
            }, 0);
        };

      //Population
        $http({
          method: "GET",
          url: '/CIS/Dashboard/GetPopulationCount'
        }).then(function (response) {
          $scope.Populations = response.data;
          $scope.IdealFireTrucks = Math.round(response.data / 28000);
        }, function (response) {
        
        });


        
      //Inspected Establishment
        $http({
          method: "GET",
          url: '/CIS/Dashboard/GetInspectedEstablishment'
        }).then(function (response) {
          $scope.InspectedEstablishmentYears = [((new Date()).getFullYear() - 4), ((new Date()).getFullYear() - 3), ((new Date()).getFullYear() - 2), ((new Date()).getFullYear() - 1), (new Date()).getFullYear()];

          $scope.InspectedEstablishment = response.data;
        }, function (response) {

        });

        //Fire Code Fees Collection
        $http({
          method: "GET",
          url: '/CIS/Dashboard/GetFireCodeFees'
        }).then(function (response) {
          $scope.FireCodeFeesYears = [((new Date()).getFullYear() - 4), ((new Date()).getFullYear() - 3), ((new Date()).getFullYear() - 2), ((new Date()).getFullYear() - 1), (new Date()).getFullYear()];

          $scope.FireCodeFees = response.data;
        }, function (response) {

        });

    };
});