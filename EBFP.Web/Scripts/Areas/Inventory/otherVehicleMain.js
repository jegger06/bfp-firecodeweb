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
        pageLength: 25,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetOtherVehicle',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetOtherVehicleDatatableParams()
            }
        }, "initComplete": function (settings, json) {
            //if (settings.json && settings.json.recordsFiltered >= 0)
            //    $('#tblOtherVehicle_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
        },
        "columns": [
            //{
            //    "name": "Vehicle_Id_Code ",
            //    "searchable": true,
            //    "sortable": true,
            //    "render": function (data, type, full, meta) {
            //        return full.Vehicle_Id_Code;
            //    }
            //},
            {
                "name": "Unit_StationName",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_StationName;
                }
            }
            ,
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
            },

            {
                "name": "Action",
                "searchable": false,
                "sortable": false,
                "width": "200px",
                "render": function (data, type, full, meta) {
                    return GetActionTemplate(full.sVehicle_Id);
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

    var searchOtherVehicleModel = {
        Vehicle_Id_Code: $("#txtVehicleId").val(),
        Vehicle_UnitId: $("#ddlVehicleStationName").val() === "" ? 0 : $("#ddlVehicleStationName").val(),
        Vehicle_Type: $("#ddlVehicleType").val() === "" ? 0 : $("#ddlVehicleType").val(),
        Vehicle_Status: $("#ddlVehicleStatus").val() === "" ? 0 : $("#ddlVehicleStatus").val(),
        Vehicle_Owner: $("#ddlVehicleOwner").val() === "" ? 0 : $("#ddlVehicleOwner").val(),
        Vehicle_PlateNumber: $("#txVehiclePlatenumber").val(),
        isVehicleSearch: isVehicleSearch
    };

    datatableInfo.searchOtherVehicleModel = searchOtherVehicleModel;
    return datatableInfo;
}

function DeleteVehicleConfirm(event) {
    if (hasAccessToModify) {
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
        }, function(isConfirm) {
            if (isConfirm) {
                swal("Deleted!", "Item has been deleted.", "success");
                $.get(event, function(data, status) {
                    tblOtherVehicle.ajax.reload();
                });
            } else {
                swal("Cancelled", "Item delete has been cancelled", "error");
            }
        });
    }
    else {
        swal("Unauthorized", "You are not allowed to do this action.", "error");
    }
}

function GetActionTemplate(vehicleId) {
    var concat = "";
    if (hasAccessToModify) {
        concat += '<div style="width:170px !important;">';
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="#" onclick="return DeleteVehicleConfirm(\'/Inventory/DeleteVehicle?sId=' + vehicleId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        } else {
            concat += '<a href="#" onclick="return DeleteVehicleConfirm(\'/Inventory/DeleteVehicle?sId=' + vehicleId + '&sMunicipalityId=' + paramMunicipalityId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        }
        
        concat += ' Delete';
        concat += '</a>';
    }
    if (hasAccessToViewDetails || hasAccessToModify) {

        if (decryptedMunicipalityId === "0") {
            concat += '<a href="/Inventory/OtherVehicleDetails?sId=' + vehicleId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        } else {
            concat += '<a href="/Inventory/OtherVehicleDetails?sId=' + vehicleId + '&sMunicipalityId=' + paramMunicipalityId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        }

        concat += ' Details';
        concat += '</a>';
        concat += '</div>';
    }

    return concat;
}

function FilterVehicleSearch() {
    isVehicleSearch = true;
    tblOtherVehicle.ajax.reload();
}
