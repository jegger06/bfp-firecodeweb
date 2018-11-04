var isSearch = false;
var tblStationPersonnel = null;
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

    tblStationPersonnel = $('#tblStationPersonnel').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
            url: '/Inventory/GetPersonneList',
            type: 'POST',
            data: function (d) {
                d.gridInfo = SetStationPersonnelDatatableParams()
            }
        }, "initComplete": function (settings, json) {
        },
        "columns": [
              {
                  "name": "Rank_Name ",
                  "searchable": false,
                  "sortable": false,
                  "render": function (data, type, full, meta) {
                      return full.Rank_Name;
                  }
              },
            {
                "name": "Full_Name",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.Full_Name;
                }
            }
             ,
            {
                "name": "Present_Designation",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.Present_Designation;
                }
            },
            {
                "name": "Specific_Designation",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.Specific_Designation;
                }
            },
            {
                "name": "Contact_Number",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.Contact_Number;
                }
            },
            {
                "name": "Email",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.Email;
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
             $("#tblPersonnel_length #totalGridResult").remove();
             $('#tblPersonnel_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
         }

     });
});

function SetStationPersonnelDatatableParams() {
    var datatable = $('#tblStationPersonnel').DataTable();
    var datatableInfo = datatable.page.info();
    
    var searchPersonnelModel = {
        Municipality_Id: dcMunicipalityId,
        Station_Id: unitId
    };

    datatableInfo.searchPersonnelModel = searchPersonnelModel;
    return datatableInfo;
}