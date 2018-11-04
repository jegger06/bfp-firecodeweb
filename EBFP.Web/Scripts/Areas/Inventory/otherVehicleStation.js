var isVehicleSearch = false;
var tblOtherVehicle = null;
$(window).bind("load", function () {
    setTimeout(function () {
        $("#divOtherVehicle").removeClass("in");
    }, 4000);

});

$(document).ready(function () {
    $('div.block').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
        , css: {
            border: '1px solid #fff !important'
        }
    });

    tblOtherVehicle = $('#tblOtherVehicle').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetStationOtherVehicle',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetOtherVehicleDatatableParams()
            }
        }, "initComplete": function (settings, json) {
        },
        "columns": [
            {
                "name": "Vehicle_Id_Code ",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Vehicle_Id_Code;
                }
            },
            {
                "name": "Vehicle_Type",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Vehicle_TypeName;
                }
            },
            {
                "name": "Vehicle_Status",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Vehicle_StatusName;
                }
            },
            {
                "name": "Vehicle_Owner",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Vehicle_OwnerName;
                }
            },
            {
                "name": "Vehicle_PlateNumber",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Vehicle_PlateNumber;
                }
            }
        ]
    })
        .on('preXhr.dt', function () {
            $('div.block').block({
                message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
                , css: {
                    border: '1px solid #fff !important'
                }
            });
        })
        .on('xhr.dt', function (data, settings) {
            console.log(data);
            $('div.block').unblock();
            if (settings.json && settings.json.recordsFiltered >= 0) {
                $("#tblOtherVehicle_length #totalGridResult").remove();
                $('#tblOtherVehicle_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
            }

        });
});

function SetOtherVehicleDatatableParams() {
    var datatable = $('#tblOtherVehicle').DataTable();
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
