// Morris bar chart
$(document).ready(function () {

    //Dynamic Pie Chart
    $.ajax({
        url: '/FCRS/GetFirecodeFeesByRegion',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            var count = 0;
            $.each(data, function(index, value) {
                var str = "";
                str += "<div style=\"overflow:hidden\" class=\"col-md-6 col-lg-6 col-xs-6\"><div class=\"white-box\">";
                str += "<div class=\"box-title\">" + value.Region + "</div>";
                str += "<div  style=\"overflow:hidden\">"; 
                str += "<div style=\"width:50%; float:left;overflow:hidden\"><div  style=\"height:220px\" id=\"pie-fees-" + value.Region + "\"></div></div>";
                str += "</div>";
                str += "<div style=\"overflow:hidden\"><ul class=\"list text-left chartLables\">";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #FA8072;\"></i>Cities With Fire Station Building And Fire Truck/s</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #1E8449;\"></i>Municipalities With Fire Station Building And Fire Truck/s</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #3A89C9;\"></i>With Fire Station Building And Fire Truck/s Sub Total</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #F26C4F;\"></i>Municipalities Without Fire Station BLDG  And Without Fire Truck</h7></ol>";

                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #E53935;\"></i>Total Number Of Cities</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #AF7AC5;padding-left:70px\"></i>Total Number Of Municipalities</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #85C1E9;\"></i>Total Cities and Municipalities</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #76D7C4;padding-left:68px\"></i>Municipalities With Fire Truck Only</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #F7DC6F;\"></i>Municipalities Without Fire Truck</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #EB984E;padding-left:56px\"></i>Municipalities With Fire Station BLDG Only</h7></ol>";
               
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #ECEFF1;\"></i>Municipalities Without Fire Station BLDG</h7></ol>";
                str += "<ol><h7><i class=\"fa fa-circle m-r-5\" style=\"color: #CC00FF;padding-left:13px\"></i>Number Of Fire Sub-Station BLDG</h7></ol>";
                str += "</ul></div>";
                str += "</div></div>";

                $("#pie").append(str)


                Morris.Donut({
                    element: 'pie-fees-' + value.Region,
                    colors: ["#FA8072", "#1E8449", "#3A89C9", "#E53935", "#AF7AC5", "#85C1E9", "#76D7C4", "#F7DC6F", "#EB984E", "#F26C4F", "#ECEFF1", "#CC00FF"],
                    data: [
                        { label: "City", value: value.FireCodeAdminFine },
                        { label: "Municipality", value: value.ConstructionTax },
                        { label: "Sub Total", value: value.ConveyanceClearanceFee },
                        { label: "Total City", value: value.FireSafetyInspectionFee },
                        { label: "Total Municipality", value: value.InstallationClearanceFee },
                        { label: "Total Cities and Municipalities", value: value.OtherFee },
                        { label: "Municipalities With Fire Truck Only", value: value.PremiumTax },
                        { label: "Municipalities Without Fire Truck", value: value.ProceedsTax },
                        { label: "Municipalities With Fire Station BLDG Only", value: value.RealtyTax },
                        { label: "Municipalities Without Fire Station BLDG  And Without Fire Truck", value: value.SalesTax },
                        { label: "Municipalities Without Fire Station BLDG", value: value.InstallationClearanceFee },
                        { label: "Number Of Fire Sub-Station BLDG", value: value.InstallationClearanceFee }
                    ]
                });
                count++;
            });
        }
    });


    //Truck Bar Chart
    $.ajax({
        url: '/CIS/Dashboard/GetTruckCountDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function (index, value) {
                var model = {
                    statusName: value.StatusName,
                    bfpOwned: numberWithCommas(value.BFPOwnedCount),
                    lguOwned: numberWithCommas(value.LGUOwnedCount)
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'truck-bar-chart',
                data: list,
                xkey: 'statusName',
                ykeys: ['bfpOwned', 'lguOwned'],
                labels: ['BFP Owned', 'LGU Owned'],
                barColors: ['#03a9f3', '#87CEFA'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                resize: true,
                horizontal: true
            });

        }
    });
    
    //Truck Age Bar Chart
    $.ajax({
        url: '/CIS/Dashboard/GetTruckAgeCountDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function (index, value) {
                var model = {
                    age: value.Age,
                    bfpOwned: numberWithCommas(value.BFPOwnedCount),
                    lguOwned: numberWithCommas(value.LGUOwnedCount)
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'truckAge-bar-chart',
                data: list,
                xkey: 'age',
                ykeys: ['bfpOwned', 'lguOwned'],
                labels: ['BFP Owned', 'LGU Owned'],
                barColors: ['#03a9f3', '#87CEFA'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                xLabelAngle: 60,
                resize: true,
                horizontal: true
            });

        }
    });

    ////Total Count Summary Truck
    $.ajax({
        url: '/CIS/Dashboard/GetTruckSummaryCount',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#totalTrucks").text(numberWithCommas(data.TotalTruck));
            $("#totalServiceable").text(numberWithCommas(data.TotalServiceable));
            $("#totalUnserviceable").text(numberWithCommas(data.TotalUnserviceable));
            $("#totalUnderRepair").text(numberWithCommas(data.TotalUnderRepair));
            $("#totalBFP").text(numberWithCommas(data.TotalBFPOwned));
            $("#totalLGU").text(numberWithCommas(data.TotalLGUOwned));
        }
    });

    //Total Count Summary Truck
    $.ajax({
        url: '/CIS/Dashboard/GetTruckAgeGroupCount',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
           
            $("#bfpNew").text(numberWithCommas(data[0].BFPOwnedCount));
            $("#lguNew").text(numberWithCommas(data[0].LGUOwnedCount));
            $("#subTotalNew").text(numberWithCommas(data[0].SubTotal));
            $("#ShareNew").text(numberWithCommas(data[0].Share));

            $("#bfpOld").text(numberWithCommas(data[1].BFPOwnedCount));
            $("#lguOld").text(numberWithCommas(data[1].LGUOwnedCount));
            $("#subTotalOld").text(numberWithCommas(data[1].SubTotal));
            $("#ShareOld").text(numberWithCommas(data[1].Share));

            $("#bfpTotal").text(numberWithCommas(data[2].BFPOwnedCount));
            $("#lguTotal").text(numberWithCommas(data[2].LGUOwnedCount));
            $("#subTotalTotal").text(numberWithCommas(data[2].SubTotal));
            $("#ShareTotal").text(numberWithCommas(data[2].Share));
        }
    });


    //Personnel Strength
    $.ajax({
        url: '/CIS/Dashboard/GetPersonnelStrenght?type=OR',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (value) {
            var total = (value.OfficerRanks + value.NonUniformedPersonnel + value.NonOfficerRanks);
            var officerRanksPercent = (value.OfficerRanks * 100) / total;
            var nonOfficerRanksPercent = (value.NonOfficerRanks * 100) / total;
            var nonUniformedPercent = (value.NonUniformedPersonnel * 100) / total;
            Morris.Donut({
                element: 'personnestrength-pie',
                colors: ["#cc0000", "#e6e600", "#0000cc"],
                data: [
                    { label: "Officer Ranks " + Math.round(officerRanksPercent) + "%", value: value.OfficerRanks },
                    { label: "Non-Uniformed Personnel " + Math.round(nonUniformedPercent) + "%", value: value.NonUniformedPersonnel },
                    { label: "Non-Officer Ranks " + Math.round(nonOfficerRanksPercent) + "%", value: value.NonOfficerRanks }
                ]
            });
        }
    });

    //Actual Vs DBM-Authorized
    $.ajax({
      url: '/CIS/Dashboard/GetOfficerRankDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function (index, value) {
                var model = {
                    rankName: value.Rank,
                    dbmAuthorized: (value.DBMAuthorized),
                    actualStrength: (value.ActualStrength),
                    variance: (value.Variance)
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'officerRank',
                data: list,
                xkey: 'rankName',
                ykeys: ['dbmAuthorized', 'actualStrength', 'variance'],
                labels: ['DBM-Authorized', 'Actual Strength', 'Variance'],
                barColors: ['#03a9f3', '#87CEFA', '#0000CC'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                resize: true,
                horizontal: true
            });

        }
    });

    //Rank
    $.ajax({
        url: '/CIS/Dashboard/GetRankDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function (index, value) {
                var model = {
                    rankName: value.Rank,
                    dbmAuthorized: (value.DBMAuthorized),
                    actualStrength: (value.ActualStrength),
                    variance: (value.Variance)
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'nonOfficerRank',
                data: list,
                xkey: 'rankName',
                ykeys: ['dbmAuthorized', 'actualStrength', 'variance'],
                labels: ['DBM-Authorized', 'Actual Strength', 'Variance'],
                barColors: ['#03a9f3', '#87CEFA', '#0000CC'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                resize: true,
                horizontal: true
            });

        }
    });


    ////Vehicle Bar Chart
    $.ajax({
        url: '/CIS/Dashboard/GetVehicleCountDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function (index, value) {
                var model = {
                    TypeName: value.TypeName,
                    bfpOwned: numberWithCommas(value.BFPOwnedCount),
                    lguOwned: numberWithCommas(value.LGUOwnedCount)
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'vehicle-bar-chart',
                data: list,
                xkey: 'TypeName',
                ykeys: ['bfpOwned', 'lguOwned'],
                labels: ['BFP Owned', 'LGU Owned'],
                barColors: ['#03a9f3', '#87CEFA'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                resize: true,
                horizontal: true,
                //xLabelAngle: 60,
            });

        }
    });

    ////Total Count Summary Vehicle
    $.ajax({
        url: '/CIS/Dashboard/GetVehicleSummaryCount',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#totalVehicles").text(numberWithCommas(data.TotalVehicle));
            $("#totalAmbulance").text(numberWithCommas(data.TotalAmbulances));
            $("#totalRescueVans").text(numberWithCommas(data.TotalRescueVehicle));
            $("#totalFireBoats").text(numberWithCommas(data.TotalFireBoats));
            $("#totalFireTrucks").text(numberWithCommas(data.TotalFireTrucks));
        }
    });

    ////Total Actual vs DBM
    $.ajax({
        url: '/CIS/Dashboard/GetPersonnelStrenthTotals',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#totalDBMOfficer").text(numberWithCommas(data.TotalDBMOfficer));
            $("#totalActualOfficer").text(numberWithCommas(data.TotalActualOfficer));
            $("#totalVarianceOfficer").text(numberWithCommas(data.TotalDBMOfficer - data.TotalActualOfficer));

            $("#totalDBMNonOfficer").text(numberWithCommas(data.TotalDBMNonOfficer));
            $("#totalActualNonOfficer").text(numberWithCommas(data.TotalActualNonOfficer));
            $("#totalVarianceNonOfficer").text(numberWithCommas(data.TotalDBMNonOfficer - data.TotalActualNonOfficer));

            $("#totalDBMUnifomedPersonnel").text(numberWithCommas(data.TotalDBMNonOfficer + data.TotalDBMOfficer));
            $("#totalActualUnifomedPersonnel").text(numberWithCommas(data.TotalActualOfficer + data.TotalActualNonOfficer));
            $("#totalVarianceUnifomedPersonnel").text(numberWithCommas((data.TotalDBMNonOfficer + data.TotalDBMOfficer) - (data.TotalActualOfficer + data.TotalActualNonOfficer)));

            $("#totalDBMNonUnifomedPersonnel").text(numberWithCommas(data.TotalDBMNonUnifomedPersonnel));
            $("#totalActualNonUnifomedPersonnel").text(numberWithCommas(data.TotalActualNonUnifomedPersonnel));
            $("#totalVarianceNonUnifomedPersonnel").text(numberWithCommas(data.TotalDBMNonUnifomedPersonnel - data.TotalActualNonUnifomedPersonnel));
        }
    });

    function numberWithCommas(x) {
        if(x > 0)
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
});


