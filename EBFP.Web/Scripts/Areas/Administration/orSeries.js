jQuery(function($) {
  var deleteBtn = $('.deleteOrSeries'),
      deleteModal = $('#deleteOrSeries'),
      editModal = $('#modalEditOrAddSeries'),
      newDateIssued = $('.newDateIssued'),
      newStartSeries = $('#newStartSeries'),
      newEndSeries = $('#newEndSeries'),
      //orSeriesTable = $('#orSeriesTable'),
      addOrEditOrSeriesBtn = $('.addOrEditOrSeriesBtn'),
      actionBtn = $('.actionBtn'),
      successMessage = $('.successMessage'),
      table = $('.table');

  deleteBtn.click(function(e) {
    e.preventDefault();
    deleteModal.modal('show');
    getSeriesId($(this));
  });

  table.on('click', '.editOrSeries', function(e) {
    e.preventDefault();
    editModal.modal('show');
    //setDataSeries(getSeriesId($(this)));
    addOrEditOrSeriesBtn.text('Save')
  });

  // Datepicker config
  newDateIssued.datepicker({
    todayBtn: 'linked'
  });

  // Trigger datepicker if icon is clicked
  $('.newDateIssuedIcon').click(function(e) {
    e.preventDefault();
    newDateIssued.focus();
  });

  //// DataTable Configurations
  //orSeriesTable.DataTable({
  //  searching: false,
  //});

  actionBtn.click(function(e) {
    e.preventDefault();
    SaveORSeries();
  })

  editModal.on('hidden.bs.modal', function(e) {
    e.preventDefault();
    newStartSeries.val('');
    newEndSeries.val('');
    newDateIssued.val('').datepicker('update');
    addOrEditOrSeriesBtn.text('Submit');
  });

  //function setDataSeries(id) {
  //  // Query from DB and set to form the values
  //  newStartSeries.val('2');
  //  newEndSeries.val('5')
  //  newDateIssued.datepicker('setDate', new Date(2018, 9, 24));
  //}

  function getSeriesId(element) {
    var seriesId = element.parents('tr').data('id');
    return seriesId;
  }

  function actionSuccessMessage(action) {
    deleteModal.modal('hide');
    editModal.modal('hide');
    if (action === 'Submit') {
      // Submit btn is clicked
      successMessage.text('A new OR Series has been successfully saved!');
    } else if (action === 'Save') {
      // Save btn is clicked
      successMessage.text('An OR Series has been successfully updated!');
    } else {
      // Delete btn is clicked
      successMessage.text('An OR Series has been successfully deleted!');
    }
    $('#orSeriesSuccess').modal('show');
  }

    $('#btnSearch').click(function () {
      FilterSearch();
    });


 
    var isSearch = false;

    function SaveORSeries() {
      var model = {
        OR_StartSeries: $('.modal-content #newStartSeries').val(),
        OR_EndSeries: $('.modal-content #newEndSeries').val(),
        OR_Issue_Date: $('.modal-content #newDateIssued').val(),
        OR_Id: orSeriesID
      };
      $.ajax({
        type: "POST",
        url: '/OR/SaveORSeries',
        async: true,
        cache: false,
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          if (response.message === "success") {
            $(".modal-content #newStartSeries").val("");
            $(".modal-content #newEndSeries").val("");
            $(".modal-content #newDateIssued").val("");
            orSeriesID = "";
            $("#modalEditOrAddSeries").modal('hide');
            swal("Saved!", "OR Series successfully saved.", "success");
            orSeriesTable.ajax.reload();
          } else {
            swal("Error!", response.message, "error");
          }
        },
        error: function (data) {
          $("#modalEditOrAddSeries").modal('hide');
        }
      });
    }


    function FilterSearch() {
      isSearch = true;
      orSeriesTable.ajax.reload();
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

      orSeriesTable = $('#orSeriesTable').DataTable({
        order: [[0, "asc"]],
        serverSide: true,
        deferRender: true,
        pageLength: 50,
        processing: true,
        responsive: true,
        searching: false,
        ajax: {
          url: '/OR/GetOR',
          type: 'POST',
          data: function (d) {
            d.gridInfo = SetDatatableParams();
          }
        },
        "initComplete": function (settings, json) {
        },
        "columns": [
            {
              "name": "OR_StartSeries",
              "searchable": true,
              "sortable": true,
              "render": function (data, type, full) {
                return full.OR_StartSeries;
              }
            },
            {
              "name": "OR_EndSeries",
              "searchable": true,
              "sortable": true,
              "render": function (data, type, full) {
                return full.OR_EndSeries;
              }
            }
            ,
             {
               "name": "OR_Issue_Date",
              "searchable": true,
              "sortable": true,
              "render": function (data, type, full) {
                return formatDate(full.OR_Issue_Date);
              }
            }
            ,
            {
              "name": "Action",
              "searchable": false,
              "sortable": false,
              "render": function (data, type, full, meta) {
                return GetActionTemplate(full.OR_Id, full.OR_StartSeries, full.OR_EndSeries,formatDate(full.OR_Issue_Date));
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

                $("#orSeriesTable_length #totalGridResult").remove();
                if (settings.json && settings.json.recordsFiltered >= 0)
                  $('#orSeriesTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" +
                      settings.json.recordsFiltered.toLocaleString() +
                      " result(s)</span>");

              });
    }

    function SetDatatableParams() {
      var datatable = $('#orSeriesTable').DataTable();
      var datatableInfo = datatable.page.info();
      var columns = datatable.settings().init().columns;
      var order = datatable.order();
      var sortIndex = order[0][0];
      var sortOrder = order[0][1];
      datatableInfo.sortOrder = sortOrder;
      datatableInfo.sortColumnName = columns[sortIndex].name;

      var searchOR = {
        StartORSeries: $('#searchStartSeries').val(),
        EndORSeries: $('#searchEndSeries').val(),
        IsSearch: isSearch
      };

      datatableInfo.searchOR = searchOR;
      return datatableInfo;
    }


    function GetActionTemplate(orId, startSeries,endSeries,issueDate) {
      var concat = "";

      concat += "<a href=\"#\" onclick=\"editOr('" + orId + "','" + startSeries + "','" + endSeries + "','" + issueDate + "');\" class=\"btn btn-primary\"><i class=\"fa fa-pencil-square-o\"></i>Edit&nbsp;";
      concat += '</a>&nbsp;';
      //concat += '<a href="#" onclick="return DeleteAllocationConfirm(\'@Url.Action("DeleteORSeries", "OR")?OR_StartSeries=' + startSeries + ' ,&OR_EndSeries=' + endSeries + ',&OR_Id=' + orId + '\');"   alt="alert" class="btn btn-danger deleteOrSeries"><i class="fa fa-trash-o fa-delete"></i>';
      //concat += '</a>';

      concat += '<a href="#" onclick="return DeleteORSeriesConfirm(' + orId + ',' + startSeries + ',' + endSeries + ');"   alt="alert" class="btn btn-danger" title="Delete"><i class="fa fa-trash-o"></i>Delete</a>';
   
      //<a href="#" class="btn btn-primary editOrSeries" title="Edit"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Edit</a>
      //       <a href="#" class="btn btn-danger deleteOrSeries" title="Delete"><i class="fa fa-trash-o" aria-hidden="true"></i> Delete</a>
      //     </td>
      return concat;
    }

   
});