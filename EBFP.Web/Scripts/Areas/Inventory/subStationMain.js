var isSubSearch = false;
var tblSubStation = null;
$(window).bind("load", function () {
    setTimeout(function () {
        $("#divSubStation").removeClass("in");
    }, 4000);

});

$(document).ready(function () {
    $('div.block3').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
         , css: {
             border: '1px solid #fff !important'
         }
    });

    tblSubStation = $('#tblSubStation').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 25,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetSubStation',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetSubDatatableParams();
            }
        }, "initComplete": function (settings, json) {
            //if (settings.json && settings.json.recordsFiltered >= 0)
            //    $('#tblSubStation_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
        },
        "columns": [
            {
                "name": "Sub_Unit_Name",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_Unit_Name;
                }
            },
               //{
               //    "name": "Sub_Station_Code",
               //    "searchable": true,
               //    "sortable": true,
               //    "render": function (data, type, full, meta) {
               //        return full.Sub_Station_Code;
               //    }
               //},
            {
                "name": "Sub_Station_Name",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_Station_Name;
                }
            }
             ,
            {
                "name": "Sub_BuildingStatus_Text",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_BuildingStatus_Text;
                }
            },
            {
                "name": "Sub_BuildingOwner_Text",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_BuildingOwner_Text;
                }
            },
            {
                "name": "Sub_LotOwner_Text",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_LotOwner_Text;
                }
            },
            {
                "name": "Sub_LotStatus_Text",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Sub_LotStatus_Text;
                }
            },

{
    "name": "Action",
    "searchable": false,
    "sortable": false,
    "width": "200px",
    "render": function (data, type, full, meta) {
        return GetSubActionTemplate(full.sSub_Id);
    }

}
        ]
    })
     .on('preXhr.dt', function () {
         $('div.block3').block({
             message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
           , css: {
               border: '1px solid #fff !important'
           }
         });
     })
     .on('xhr.dt', function (data, settings) {
         console.log(data);
         $('div.block3').unblock();
         if (settings.json && settings.json.recordsFiltered >= 0) {
             $("#tblSubStation_length #totalGridResult").remove();
             $('#tblSubStation_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + (settings.json.recordsFiltered).toLocaleString() + " result(s)</span>");
         }
     });
});

function SetSubDatatableParams() {
    var datatable = $('#tblSubStation').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;

    var searchSubStationModel = {
        Sub_Station_Code: $("#txtSubStationId").val(),
        Sub_Station_Name: $("#txtSubStationName").val(),
        Sub_Unit_Id: $("#ddlSubSearchUnit").val() === "" ? 0 : $("#ddlSubSearchUnit").val(),
        Sub_BuildingStatus: $("#ddlBuildingStatus").val() === "" ? 0 : $("#ddlBuildingStatus").val(),
        Sub_BuildingOwner: $("#ddlBuildingOwner").val() === "" ? 0 : $("#ddlBuildingOwner").val(),
        Sub_LotOwner: $("#ddlLotOwner").val() === "" ? 0 : $("#ddlLotOwner").val(),
        Sub_LotStatus: $("#ddlLotStatus").val() === "" ? 0 : $("#ddlLotStatus").val(),
        IsSearch: isSubSearch
    };

    datatableInfo.searchSubStationModel = searchSubStationModel;

    return datatableInfo;
}

function DeleteConfirm(event) {
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
                    tblSubStation.ajax.reload();
                });
            } else {
                swal("Cancelled", "Item delete has been cancelled", "error");
            }
        });
    }
    else
    {
       swal("Unauthorized", "You are not allowed to do this action.", "error");
    }
}

function GetSubActionTemplate(SubStationId) {
    var concat = "";
    if (hasAccessToModify) {
        concat += '<div style="width:170px !important;">';
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="#" onclick="return DeleteConfirm(\'/Inventory/DeleteSubStation?sId=' + SubStationId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        } else {
            concat += '<a href="#" onclick="return DeleteConfirm(\'/Inventory/DeleteSubStation?sId=' + SubStationId + '&sMunicipalityId=' + paramMunicipalityId + '\');"   alt="alert" class="btn btn-danger pull-right m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-trash-o">';
        }

        concat += ' Delete';
        concat += '</a>';
    }
    if (hasAccessToViewDetails || hasAccessToModify) {
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="/Inventory/SubStationDetails?sId=' + SubStationId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        } else {
            concat += '<a href="/Inventory/SubStationDetails?sId=' + SubStationId + '&sMunicipalityId=' + paramMunicipalityId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        }
        concat += ' Details';
        concat += '</a>';
        concat += '</div>';
    }

    return concat;
}

function FilterSubStationSearch() {
    isSubSearch = true;
    tblSubStation.ajax.reload();
}
