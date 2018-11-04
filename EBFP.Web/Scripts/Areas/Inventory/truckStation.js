
var isSearch = false;
var tblTruck = null;
$(window).bind("load", function () {
    setTimeout(function () {
        $("#divTruck").removeClass("in");
    }, 4000);

});

$(document).ready(function () {
    $('div.block1').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
, css: {
    border: '1px solid #fff !important'
}
    });

    tblTruck = $('#tblTruck').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetTruckStation',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetDatatableParams()
            }
        }, "initComplete": function (settings, json) {
        },
        "columns": [
               {
                   "name": "Truck_Id_Code ",
                   "searchable": true,
                   "sortable": true,
                   "render": function (data, type, full, meta) {
                       return full.Truck_Id_Code;
                   }
               },
               {
                "name": "Truck_Type ",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Truck_TypeName;
                }
            },
            {
                "name": "Truck_Status",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Truck_StatusName;
                }
            },
            {
                "name": "Truck_Owner",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Truck_OwnerName;
                }
            },
            {
                "name": "Truck_PlateNumber   ",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Truck_PlateNumber;
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
             $("#tblTruck_length #totalGridResult").remove();
             $('#tblTruck_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
         }

     });
});

function SetDatatableParams() {
    var datatable = $('#tblTruck').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;

    var searchStationDetailsModel = {
        Station_Id: unitId,
        SubStation_Id: subStationId
    };

    datatableInfo.searchStationDetailsModel = searchStationDetailsModel;
    return datatableInfo;
}
