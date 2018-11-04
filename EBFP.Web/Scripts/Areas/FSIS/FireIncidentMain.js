var isSearch = false;
var tblFireIncident = null;
$(window).bind("load", function () {
    setTimeout(function () {
      $("#divFireIncident").removeClass("in");
    }, 4000);

});

$(document).ready(function () {
    $('div.block1').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
, css: {
    border: '1px solid #fff !important'
}
    });

    tblFireIncident = $('#tblFireIncident').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
          url: '/FSIS/GetFireIncident',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetDatatableParams()
            }
        }, "initComplete": function (settings, json) {
            //if (settings.json && settings.json.recordsFiltered >= 0)
            //    $('#tblFireIncident_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
        },
        "columns": [
               {
                   "name": "FS_ReportedBy",
                   "searchable": true,
                   "sortable": true,
                   "render": function (data, type, full, meta) {
                       return full.FS_ReportedBy;
                   }
               },
            {
                "name": "FS_MobileNo",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.FS_MobileNo;
                }
            }
             ,
            {
                "name": "FS_Remarks ",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                  return full.FS_Remarks;
                }
            },
            {
              "name": "FS_DateCreated",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                  return formatDate(full.FS_DateCreated);
                }
            },
            {
                "name": "Action",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function(data, type, full, meta) {
                  return GetActionTemplate(full.sFS_Id);
              }

            }
        ]
    })
     .on('preXhr.dt', function () {
         $('div.block1').block({
             message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
           , css: {
               border: '1px solid #fff !important'
           }
         });
     })
     .on('xhr.dt', function (data, settings) {
         console.log(data);
         $('div.block1').unblock();
         if (settings.json && settings.json.recordsFiltered >= 0) {
             $("#tblFireIncident_length #totalGridResult").remove();
             $('#tblFireIncident_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
         }

     });
});
function formatDate(date) {
  if (date) {
    date = new Date(parseInt(date.substr(6)));

    var dt = addLeadingZeros((date.getMonth() + 1), 2) + '/' + addLeadingZeros(date.getDate(), 2) + '/' + date.getFullYear();
    var time = date.toLocaleTimeString().toLowerCase().replace(/([\d]+\d]+)\d]+(\s\w+)/g, "$1$2");

    return (dt + " " + time);
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

function SetDatatableParams() {
    var datatable = $('#tblFireIncident').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;

    var searchFireSuppressionModel = {
      FS_MobileNo: $("#txMobileNo").val(),
      FS_ReportedBy: $("#txReportedBy").val(),
      FS_DateCreated: $("#dpDateCreated").val(),
      IsSearch: isSearch
    };

    datatableInfo.searchFireSuppressionModel = searchFireSuppressionModel;
    return datatableInfo;
}

function DeleteTruckConfirm(event) {

    swal({
        title: "Are you sure?",
        text: "You will not be able to recover deleted item!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            swal("Deleted!", "Item has been deleted.", "success");
            $.get(event, function (data, status) {
                tblFireIncident.ajax.reload();
            });
        } else {
            swal("Cancelled", "Item delete has been cancelled", "error");
        }
    });

}

function GetActionTemplate(fsId) {
  var concat = "";
  if (hasAccessToModify) {
    concat += '<a href="#" onclick="return DeleteTruckConfirm(\'/FSIS/DeleteFireIncident?sId=' + fsId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">'
    concat += ' Delete'
    concat += '</a>';
   }
    concat += '<a href="/FSIS/FireIncidentDetails?sId=' + fsId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">'
    concat += ' Details'
    concat += '</a>';

    return concat;
}

function FilterSearch() {
    isSearch = true;
    tblFireIncident.ajax.reload();
}