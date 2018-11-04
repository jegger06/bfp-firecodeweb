jQuery(function($) {
  var includeDate = $('.includeDate');
  var depositsSubmitBtn = $('.depositsSubmitBtn');
  var depositSuccess = $('#depositSuccess');
  var addDeposit = $('#addDeposit');

  enableDisableDates(includeDate);

  includeDate.change(function() {
    enableDisableDates($(this));
  });

  $('.input-daterange, .newDepositDate').datepicker({
    todayBtn: 'linked'
  });

  // Trigger datepicker if icon is clicked
  $('.newDateIcon').click(function (e) {
    e.preventDefault();
    $(this).prev().focus();
  });

  function enableDisableDates(element) {
    var startDate = $('.startDate');
    var endDate = $('.endDate');
    if (element.is(':checked')) {
      startDate.prop('disabled', false);
      endDate.prop('disabled', false);
    } else {
      startDate.prop('disabled', true);
      endDate.prop('disabled', true);
    }
  }


  var depositsTable = null;
  var isSearch = false;
  $(document).ready(function () {
    isSearch = false;
    InitSelection();
    InitSearchSelection();
    LoadTable();

  });


  $("#cbSearchincludeDate").change(function () {
    if (this.checked) {
      $('#cbSearchincludeDate').val(true);

    } else {
      $('#cbSearchincludeDate').val(false);
    }
  });


  function InitSearchSelection() {
    $(".depositorSearchSelection").select2({
      ajax: {
        url: '/Deposits/SelectionAutoComplete',
        //url: '@Url.Action("SelectionAutoComplete", "Establishment")',
        dataType: 'json',
        delay: 250,
        data: function (params) {
          return {
            search: params.term // search term
          };
        },
        processResults: function (data) {
          return {
            results: data.data
          };
        },
        cache: true

      },
      minimumInputLength: 3
    });
  }
  function InitSelection() {
    $(".depositorSelection").select2({
      dropdownParent: $('#modalAddDeposit'),
      ajax: {
        url: '/Deposits/SelectionAutoComplete',
        //url: '@Url.Action("SelectionAutoComplete", "Establishment")',
        dataType: 'json',
        delay: 250,
        data: function (params) {
          return {
            search: params.term // search term
          };
        },
        processResults: function (data) {
          return {
            results: data.data
          };
        },
        cache: true

      },
      minimumInputLength: 3
    });
  }
  function SaveDeposits() {
    var model = {
      Dep_LC_No: $('.modal-content #newLcNumber').val(),
      Dep_Bank: $('.modal-content #newBankName').val(),
      Dep_Amount: $('.modal-content #newAmount').val(),
      Dep_Depositor_Emp_Id: $(".modal-content #ddlDepositor").val() === "" ? 0 : $(".modal-content #ddlDepositor").val(),
      Dep_Collection_StartDate: $('.modal-content #newStartDate').val(),
      Dep_Collection_EndDate: $('.modal-content #newEndDate').val(),
      Dep_DepositDate: $('.modal-content #newDepositDate').val()
    };
    $.ajax({
      type: "POST",
      url: '/Deposits/SaveDeposit',
      async: true,
      cache: false,
      data: JSON.stringify(model),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (response) {
        if (response.message === "success") {
          $(".modal-content #newLcNumber").val("");
          AllocId = "";
          $("#modalAddDeposit").modal('hide');
          swal("Saved!", "Deposit successfully saved.", "success");
          depositsTable.ajax.reload();
        } else {
          swal("Error!", response.message, "error");
        }
      },
      error: function (data) {
        $("#modalAddDeposit").modal('hide');
      }
    });
  }

  $('#btnDepositModal').click(function () {
    $("#modalAddDeposit").modal();
  });

  $('#btnSaveDeposits').click(function () {
    SaveDeposits();
  });

  $('#btnSearch').click(function () {
    FilterSearch();
  });



  function SetDatatableParams() {
    var datatable = $('#depositsTable').DataTable();
    var datatableInfo = datatable.page.info();
    var columns = datatable.settings().init().columns;
    var order = datatable.order();
    var sortIndex = order[0][0];
    var sortOrder = order[0][1];
    datatableInfo.sortOrder = sortOrder;
    datatableInfo.sortColumnName = columns[sortIndex].name;

    var searchDepositModel = {
      //TradeName: $("#txttradeName").val(),
      //MPNumber: $("#txtmpTinBin").val(),
      //BusinessPermit: $("#txtbusinessPermitNumber").val(),
      //OwnerName: $("#txtownerName").val(),
      //EstablishmentStatus: $("#ddlEstStatus").val() === "" ? 0 : $("#ddlEstStatus").val(),
      Depositor: $("#ddlSearchDepositor").val() === "" ? 0 : $("#ddlSearchDepositor").val(),
      LCNumber: $('#txtSearchlcNumber').val(),
      BankName: $('#txtSearchbankName').val(),
      IncludeDates: $('#cbSearchincludeDate').val(),
      DepositFrom: $('#startDate').val(),
      DepositTo: $('#endDate').val(),
      IsSearch: isSearch
    };
    
    datatableInfo.searchDepositModel = searchDepositModel;
    return datatableInfo;
  }

  function FilterSearch() {
    isSearch = true;
    depositsTable.ajax.reload();
  }
  function numberWithCommas(x) {
    if (x > 0)
      return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
  }
  function LoadTable() {
    $('div.block1').block({
      message: '<h3><i class="ace-icon fa fa-spinner fa-spin blue bigger-300"></i> Please Wait...</h3>',
      css: {
        border: '1px solid #fff !important'
      }
    });

    depositsTable = $('#depositsTable').DataTable({
      order: [[5, "asc"]],
      serverSide: true,
      deferRender: true,
      pageLength: 50,
      processing: true,
      responsive: true,
      searching: false,
      ajax: {
        //url: '@Url.Action("GetDeposit", "Deposits")',
         url: '/Deposits/GetDeposit',
        type: 'POST',
        data: function (d) {
          d.gridInfo = SetDatatableParams();
        }
      },
      "initComplete": function (settings, json) {
        //if (settings.json.recordsFiltered)
        //$('#depositsTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" + settings.json.recordsFiltered.toLocaleString() + " result(s)</span>");
      },
      "columns": [
          {
            "name": "Dep_LC_No",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return full.Dep_LC_No;
            }
          },
          {
            "name": "Dep_Amount",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return numberWithCommas(full.Dep_Amount);
            }
          },
          {
            "name": "Dep_Bank",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return full.Dep_Bank;
            }
          },
          {
            "name": "Dep_Collection_StartDate",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return formatDate(full.Dep_Collection_StartDate);
            }
          },
          {
            "name": "Dep_Collection_EndDate",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return formatDate(full.Dep_Collection_EndDate);
            }
          },
          {
            "name": "Dep_DepositDate",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return formatDate(full.Dep_DepositDate);
            }
          },
          {
            "name": "Est_EstablishmentStatusName",
            "searchable": true,
            "sortable": true,
            "render": function (data, type, full) {
              return full.Dep_Depositor_Emp_Name;
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

              $("#depositsTable_length #totalGridResult").remove();
              if (settings.json && settings.json.recordsFiltered >= 0)
                $('#depositsTable_length').append("<span id='totalGridResult'> &nbsp;&nbsp;&nbsp;" +
                    settings.json.recordsFiltered.toLocaleString() +
                    " result(s)</span>");

            });
  }


});



