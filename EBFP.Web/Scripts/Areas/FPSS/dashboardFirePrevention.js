var myTable = null;

var complianceTable = null;
$(document).ready(function () {
    
    //Counter
    $.ajax({
        url: '/FPSS/GetDashboardCounter',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            $("#establishment").text(numberWithCommas(data.EST_TotalCount));
            $("#high").text(numberWithCommas(data.EST_High));
            $("#moderate_low").text(numberWithCommas(data.EST_Moderate_Low));

            $("#complaint").text(numberWithCommas(data.IO_Complaint));
            $("#forIssuance_fsic").text(numberWithCommas(data.IO_AT_IssuanceOfFSIC));
            $("#issued_fsic").text(numberWithCommas(data.IO_AT_IssuedFSIC));

            $("#compliance").text(numberWithCommas(data.IO_ForCompliance));
            $("#forIssuance_ntc").text(numberWithCommas(data.IO_AT_IssuanceOfNTC));
            $("#issued_ntc").text(numberWithCommas(data.IO_AT_IssuedNTC));

            $("#closure").text(numberWithCommas(data.IO_ForClosure));
            $("#issued_closure").text(numberWithCommas(data.IO_AT_IssuedClosureOrder));
            $("#pending_evaluation").text(numberWithCommas(data.IO_AT_ClosurePendingEvaluation));

            $("#others").text(numberWithCommas(data.IO_Others));
            $("#cannot_located").text(numberWithCommas(data.IO_RM_CannotBeLocated));
            $("#refuse_entry_inspection").text(numberWithCommas(data.IO_RM_RefuseEntryInspection));
        }
    });

    $('div.block1').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
        css: {
            border: '1px solid #fff !important'
        }
    });

    //Expired Today
    myTable = $('#myTable').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/FPSS/GetTodayComplianceExpiryToday',
            type: 'POST',
            data: function(d) {
                d.gridInfo = SetDatatableParams("today");
            }
        },
        "initComplete": function(settings, json) {
            $('#myTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings._iRecordsTotal.toLocaleString() + " result(s)</span>");
        },
        "columns": [
            {
                "name": "IssueDate",
                "searchable": true,
                "sortable": true,
                "render": function(data, type, full, meta) {
                    return formatDate(full.IssueDate);
                }
            },
            {
                "name": "ComplianceType",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function(data, type, full, meta) {
                    return full.ComplianceType;
                }

            },
            {
                "name": "Deficiency",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function(data, type, full, meta) {
                    return full.Deficiency;
                }

            },
            {
                "name": "Compliance",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function(data, type, full, meta) {
                    return full.Compliance;
                }

            },
            {
                "name": "Remarks",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function(data, type, full, meta) {
                    return full.Remarks;
                }

            }
        ]
    })
        .on('preXhr.dt', function() {
            $('div.block1').block({
                message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
                css: {
                    border: '1px solid #fff !important'
                }
            });
        })
        .on('xhr.dt', function(data, settings) {
            $('div.block1').unblock();
            if (settings._iRecordsTotal > 0) {
                $("#myTable_length #totalGridResult").remove();
                $('#myTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + (settings._iRecordsTotal - 1).toLocaleString() + " result(s)</span>");
            }

        });


    $('div.block1').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
        css: {
            border: '1px solid #fff !important'
        }
    });

    //Expired in Three Days
    complianceTable = $('#complianceTable').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/FPSS/GetTodayComplianceExpiryInThreeDays',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetDatatableParams("threeDays");
            }
        },
        "initComplete": function (settings, json) {
            $('#complianceTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings._iRecordsTotal.toLocaleString() + " result(s)</span>");
        },
        "columns": [
            {
                "name": "IssueDate",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return formatDate(full.IssueDate);
                }
            },
            {
                "name": "ComplianceType",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function (data, type, full, meta) {
                    return full.ComplianceType;
                }

            },
            {
                "name": "Deficiency",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function (data, type, full, meta) {
                    return full.Deficiency;
                }

            },
            {
                "name": "Compliance",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function (data, type, full, meta) {
                    return full.Compliance;
                }

            },
            {
                "name": "Remarks",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function (data, type, full, meta) {
                    return full.Remarks;
                }

            }
        ]
    })
        .on('preXhr.dt', function () {
            $('div.block1').block({
                message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
                css: {
                    border: '1px solid #fff !important'
                }
            });
        })
        .on('xhr.dt', function (data, settings) {
            $('div.block1').unblock();
            if (settings._iRecordsTotal > 0) {
                $("#complianceTable_length #totalGridResult").remove();
                $('#complianceTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + (settings._iRecordsTotal - 1).toLocaleString() + " result(s)</span>");
            }

        });

    function numberWithCommas(x) {
      if (x) {
         return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      }
       
      return 0;
    }
});


function SetDatatableParams(type) {
    var datatable;
    if (type === "today") {
        datatable = $('#myTable').DataTable();
    } else {
        datatable = $('#complianceTable').DataTable();
    }
     
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;
    return datatableInfo;
}
    
function formatDate(date) {
    if (date) {
        date = new Date(parseInt(date.substr(6)));

        return addLeadingZeros((date.getMonth() + 1), 2) + '-' + addLeadingZeros(date.getDate(), 2) + '-' + date.getFullYear();
        //var time = date.toLocaleTimeString().toLowerCase().replace(/([\d]+:[\d]+):[\d]+(\s\w+)/g, "$1$2");

        //return (dt + " " + time);
    }
    return "";
}

function addLeadingZeros(n, length) {
    var str = (n > 0 ? n : -n) + "";
    var zeros = "";
    for (var i = length - str.length; i > 0; i--)
        zeros += "0";
    zeros += str;
    return n >= 0 ? zeros : "-" + zeros;
}


