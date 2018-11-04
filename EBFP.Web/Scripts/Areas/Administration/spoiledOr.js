jQuery(function($) {
  var spoiledOrSubmitBtn = $('.spoiledOrSubmitBtn');
  var spoiledOrSuccess = $('#spoiledOrSuccess');
  var addSpoiledOr = $('#addSpoiledOr');

  spoiledOrSubmitBtn.click(function(e) {
    e.preventDefault();
    SaveSpoiledORSeries();
  });


    function SaveSpoiledORSeries() {
    var model = {
      SOR_Number: $('.modal-content #newSpoiledOrNumber').val()
    };
    $.ajax({
      type: "POST",
      url: '/OR/SaveSpoiledORSeries',
      async: true,
      cache: false,
      data: JSON.stringify(model),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (response) {
        if (response.message === "success") {
          $(".modal-content #newSpoiledOrNumber").val("");
          AllocId = "";
          $("#modalSpoiledOr").modal('hide');
          swal("Saved!", "Spoiled OR successfully saved.", "success");
          spoiledOrTable.ajax.reload();
        } else {
          swal("Error!", response.message, "error");
        }
      },
      error: function (data) {
        $("#modalSpoiledOr").modal('hide');
      }
    });
  }


    $('#btnSearch').click(function () {
      FilterSearch();
    });


    var spoiledOrTable = null;
    var isSearch = false;

    function FilterSearch() {
      isSearch = true;
      spoiledOrTable.ajax.reload();
    }

    $(document).ready(function () {
      isSearch = false;
      LoadTable();
    });

    function LoadTable() {
      $('div.block1').block({
        message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
        css: {
          border: '1px solid #fff !important'
        }
      });

      spoiledOrTable = $('#spoiledOrTable').DataTable({
        order: [[1, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
          url: '/OR/GetSpoiledOR',
          type: 'POST',
          data: function (d) {
            d.gridInfo = SetDatatableParams();
          }
        },
        "initComplete": function (settings, json) {
        },
        "columns": [
            {
              "name": "SOR_Number",
              "searchable": true,
              "sortable": true,
              "render": function (data, type, full) {
                return full.SOR_Number;
              }
            },
            {
              "name": "SOR_CreatedDate",
              "searchable": true,
              "sortable": true,
              "render": function (data, type, full) {
                return formatDate(full.SOR_CreatedDate);
              }
            }
            //,
            //{
            //  "name": "Action",
            //  "searchable": false,
            //  "sortable": false,
            //  "width": "200px",
            //  "render": function(data, type, full) {
            //    return GetActionTemplate(full.sEst_Id);
            //  }

            //}
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

                $("#spoiledOrTable_length #totalGridResult").remove();
                if (settings.json && settings.json.recordsFiltered >= 0)
                  $('#spoiledOrTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" +
                      settings.json.recordsFiltered.toLocaleString() +
                      " result(s)</span>");

              });
    }

    function SetDatatableParams() {
      var datatable = $('#spoiledOrTable').DataTable();
      var datatableInfo = datatable.page.info();
      var columns = datatable.settings().init().columns;
      var order = datatable.order();
      var sortIndex = order[0][0];
      var sortOrder = order[0][1];
      datatableInfo.sortOrder = sortOrder;
      datatableInfo.sortColumnName = columns[sortIndex].name;

      var searchSpoiledOR = {
        SpoiledORNumber: $('#searchSpoiledOrNumber').val(),
        IsSearch: isSearch
      };

      datatableInfo.searchSpoiledOR = searchSpoiledOR;
      return datatableInfo;
    }

});