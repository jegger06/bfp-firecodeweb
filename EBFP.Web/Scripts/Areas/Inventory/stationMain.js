var isStationSearch = false;
var tblStation = null;
$(window).bind("load", function () {
    setTimeout(function () {
        $("#divStation").removeClass("in");
    }, 4000);

});

$(document).ready(function () {
    $('div.block4').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
         , css: {
             border: '1px solid #fff !important'
         }
    });

    tblStation = $('#tblStation').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 25,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetStation',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetStationDatatableParams();
            }
        }, "initComplete": function (settings, json) {
            //if (settings.json && settings.json.recordsFiltered >= 0)
            //    $('#tblStation_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
        },
        "columns": [
         {
             "name": "Unit_StationName",
             "searchable": true,
             "sortable": true,
             "render": function (data, type, full, meta) {
                 return full.Unit_StationName;
             }
         },
               //{
               //    "name": "Unit_Code",
               //    "searchable": true,
               //    "sortable": true,
               //    "render": function (data, type, full, meta) {
               //        return full.Unit_Code;
               //    }
               //},
            {
              "name": "Unit_Category",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_CategoryName;
                }
            }
             ,
            {
              "name": "Unit_BuildingStatus",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_BuildingStatus_Text;
                }
            },
            {
              "name": "Unit_BuildingOwner",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_BuildingOwner_Text;
                }
            },
            {
              "name": "Unit_LotOwner",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_LotOwner_Text;
                }
            },
            {
              "name": "Unit_LotStatus",
                "searchable": true,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return full.Unit_LotStatus_Text;
                }
            },

{
    "name": "Action",
    "searchable": false,
    "sortable": false,
    "width": "200px",
    "render": function (data, type, full, meta) {
        return GetStationActionTemplate(full.sUnit_Id);
    }

}
        ]
    })
     .on('preXhr.dt', function () {
         $('div.block4').block({
             message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>'
           , css: {
               border: '1px solid #fff !important'
           }
         });
     })
     .on('xhr.dt', function (data, settings) {
         console.log(data);
         $('div.block4').unblock();
         if (settings.json && settings.json.recordsFiltered >= 0) {
             $("#tblStation_length #totalGridResult").remove();
             $('#tblStation_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + (settings.json.recordsFiltered).toLocaleString() + " result(s)</span>");
         }
     });
});

function SetStationDatatableParams() {
    var datatable = $('#tblStation').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;
    var searchStationModel = {
        Unit_Code: $("#txtStationId").val(),
        Unit_StationName: $("#txtStationName").val(),
        Unit_Category: $("#ddlCategory").val() === "" ? 0 : $("#ddlCategory").val(),
        Unit_BuildingStatus: $("#ddlUnitBuildingStatus").val() === "" ? 0 : $("#ddlUnitBuildingStatus").val(),
        Unit_BuildingOwner: $("#ddlUnitBuildingOwner").val() === "" ? 0 : $("#ddlUnitBuildingOwner").val(),
        Unit_LotOwner: $("#ddlUnitLotOwner").val() === "" ? 0 : $("#ddlUnitLotOwner").val(),
        Unit_LotStatus: $("#ddlUnitLotStatus").val() === "" ? 0 : $("#ddlUnitLotStatus").val(),
        IsSearch: isStationSearch
    };

    datatableInfo.searchStationModel = searchStationModel;

    return datatableInfo;
}


function GetStationActionTemplate(StationId) {
    var concat = "";
    
    if (hasAccessToModify) {
        if (decryptedMunicipalityId === "0") {
            concat += '<a href="/Inventory/StationDetails?sId=' + StationId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        } else {
            concat += '<a href="/Inventory/StationDetails?sId=' + StationId + '&sMunicipalityId=' + paramMunicipalityId + '" class="btn btn-success m-l-20 btn-rounded btn-outline waves-effect waves-light fa fa-pencil">';
        }
        concat += ' Details';
        concat += '</a>';
    }

    return concat;
}

function FilterStationSearch() {
    isStationSearch = true;
    tblStation.ajax.reload();
}

