// Morris bar chart
$(document).ready(function () {
    //HRIS Bar Chart
    $.ajax({
        url: '/HRIS/Dashboard/GetDashboardDetails',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            var list = [];
            $.each(data, function(index, value) {
                var model = {
                    rank: value.RankName,
                    male: value.MaleCount,
                    female: value.FemaleCount
                }

                list.push(model);
            });

            Morris.Bar({
                element: 'morris-bar-chart',
                data: list,
                xkey: 'rank',
                ykeys: ['male', 'female'],
                labels: ['MalePeronnel', 'Female Personnel'],
                barColors: ['#03a9f3', '#FFB6C1'],
                hideHover: 'auto',
                gridLineColor: '#03a9f3',
                xLabelAngle: 60,
                resize: true,
                horizontal: true
            });

        }
    });

  //Duty Status Bar Chart
    $.ajax({
        url: '/HRIS/Dashboard/GetDutyStatusChartDetails',
      type: 'GET',
      dataType: 'json',
      contentType: 'application/json; charset=utf-8',
      data: {},
      success: function (data) {
        var list = [];
        $.each(data, function (index, value) {
          var model = {
            dutyStatus: value.Duty_Status,
            count: value.DutyStatusCount

          }

          list.push(model);
        });

        Morris.Bar({
          element: 'morris-bar-chart2',
          data: list,
          xkey: 'dutyStatus',
          ykeys: ['count'],
          labels: ['Total'],
          barColors: ['#03a9f3'],
          hideHover: 'auto',
          gridLineColor: '#03a9f3',
          xLabelAngle: 60,
          resize: true,
          horizontal: true
        });

      }
    });
    //Counter
    $.ajax({
        url: '/HRIS/Dashboard/GetHRISDashboardCounter',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#unit").text(numberWithCommas(data.Unit));
            $("#totalStrength").text(numberWithCommas(data.TotalStrength));
            $("#male").text(numberWithCommas(data.Male));
            $("#female").text(numberWithCommas(data.Female));
        }
    });

    //Appointment Counter
    $.ajax({
        url: '/HRIS/Appointment/GetDashboardAppointmentCounter',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#today").text(numberWithCommas(data.Today));
            $("#threeDays").text(numberWithCommas(data.ThreeDays));
            $("#oneMonth").text(numberWithCommas(data.OneMonth));
        }
    });

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
});


function appointmentSearch(type) {
    window.location = "/HRIS/Appointment/?type=" + type;
}