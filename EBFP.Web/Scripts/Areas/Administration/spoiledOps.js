jQuery(function ($) {
  var spoiledOpsSubmitBtn = $('.spoiledOpsSubmitBtn');
  var spoiledOpsSuccess = $('#spoiledOpsSuccess');
  var modalSpoiledOps = $('#modalSpoiledOps');

  spoiledOpsSubmitBtn.click(function (e) {
    e.preventDefault();
    SaveOPSSeries();
  });

  $('#btnSearch').click(function () {
    FilterSearch();
  });

 
  var spoiledOPSTable = null;
  var isSearch = false;

  function FilterSearch() {
    isSearch = true;
    spoiledOPSTable.ajax.reload();
  }

  $(document).ready(function () {
    isSearch = false;
    LoadTable();
  });

  function SaveOPSSeries() {
    var model = {
      SOPS_Number: $('.modal-content #newSpoiledOpsNumber').val()
    };
    $.ajax({
      type: "POST",
      url: '/OPS/SaveOPSSeries',
      async: true,
      cache: false,
      data: JSON.stringify(model),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (response) {
        if (response.message === "success") {
          $(".modal-content #newSpoiledOpsNumber").val("");
          AllocId = "";
          $("#modalSpoiledOps").modal('hide');
          swal("Saved!", "Spoiled OPS successfully saved.", "success");
          spoiledOPSTable.ajax.reload();
        } else {
          swal("Error!", response.message, "error");
        }
      },
      error: function (data) {
        $("#modalSpoiledOps").modal('hide');
      }
    });
  }

  function LoadTable() {
    $('div.block1').block({
      message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
      css: {
        border: '1px solid #fff !important'
      }
    });

    spoiledOPSTable = $('#spoiledOPSTable').DataTable({
      order: [[1, "asc"]],
      serverSide: true,
      deferRender: true,
      pageLength: 50,
      processing: true,
      responsive: true,
      searching: false,
      ajax: {
        url: '/OPS/GetSpoiledOPS',
        type: 'POST',
        data: function (d) {
          d.gridInfo = SetDatatableParams();
        }
      },
      "initComplete": function (settings, json) {
      },
      "columns": [
          {
            "name": "SOPS_Number",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return full.SOPS_Number;
            }
          },
          {
            "name": "SOPS_CreatedDate",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return formatDate(full.SOPS_CreatedDate);
            }
          }
      ]
    })
        .on('preXhr.dt',
            function () {
              $('div.block1').block({
                message:
                    '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
                css: {
                  border: '1px solid #fff !important'
                }
              });
            })
        .on('xhr.dt',
            function (data, settings) {
              $('div.block1').unblock();

              $("#spoiledOPSTable_length #totalGridResult").remove();
              if (settings.json && settings.json.recordsFiltered >= 0)
                $('#spoiledOPSTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" +
                    settings.json.recordsFiltered.toLocaleString() +
                    " result(s)</span>");

            });
  }

  function SetDatatableParams() {
    var datatable = $('#spoiledOPSTable').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;

    var searchSpoiledOPS = {
      SpoiledOPSNumber: $('#searchSpoiledOpsNumber').val(),
      IsSearch: isSearch
    };

    datatableInfo.searchSpoiledOPS = searchSpoiledOPS;
    return datatableInfo;
  }

});