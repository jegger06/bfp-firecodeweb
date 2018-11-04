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
        pageLength: 25,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetTruck',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetDatatableParams()
            }
        }, "initComplete": function (settings, json) {
            //if (settings.json && settings.json.recordsFiltered >= 0)
            //    $('#tblTruck_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
        },
        "columns": [
               //{
               //    "name": "Truck_Id_Code ",
               //    "searchable": true,
               //    "sortable": true,
               //    "render": function (data, type, full, meta) {
               //        return full.Truck_Id_Code;
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
            },


{
    "name": "Action",
    "searchable": false,
    "sortable": false,
    "width": "200px",
    "render": function (data, type, full, meta) {
        return GetVehicleActionTemplate(full.sTruck_Id);
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

    var searchTruckModel = {
        Truck_Id_Code: $("#txtTruckId").val(),
        Truck_UnitId: $("#ddlTruckStationName").val() === "" ? 0 : $("#ddlTruckStationName").val(),
        Truck_Type: $("#ddlTruckType").val() === "" ? 0 : $("#ddlTruckType").val(),
        Truck_Capacity: $("#txtCapacity").val(),
        Truck_Status: $("#ddlTruckStatus").val() === "" ? 0 : $("#ddlTruckStatus").val(),
        Truck_Owner: $("#ddlTruckOwner").val() === "" ? 0 : $("#ddlTruckOwner").val(),
        Truck_PlateNumber: $("#txPlatenumber").val(),
        IsSearch: isSearch
    };

    datatableInfo.searchTruckModel = searchTruckModel;
    return datatableInfo;
}

function DeleteTruckConfirm(event) {
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
                    tblTruck.ajax.reload();
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

function GetVehicleActionTemplate(Truck_Id) {
    var concat = "";
    if (hasAccessToModify) {
        concat += '<div style="width:170px !important;">';
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="#" onclick="return DeleteTruckConfirm(\'/Inventory/DeleteTruck?sId=' + Truck_Id + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        } else {
            concat += '<a href="#" onclick="return DeleteTruckConfirm(\'/Inventory/DeleteTruck?sId=' + Truck_Id + '&sMunicipalityId=' + paramMunicipalityId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        }
        
        concat += ' Delete';
        concat += '</a>';
    }
    if (hasAccessToViewDetails || hasAccessToModify) {
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="/Inventory/TruckDetails?sId=' + Truck_Id + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        } else {
            concat += '<a href="/Inventory/TruckDetails?sId=' + Truck_Id + '&sMunicipalityId=' + paramMunicipalityId  + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        }

        concat += ' Details';
        concat += '</a>';
        concat += '</div>';
    }
    
    return concat;
}

function FilterSearch() {
    isSearch = true;
    tblTruck.ajax.reload();
}