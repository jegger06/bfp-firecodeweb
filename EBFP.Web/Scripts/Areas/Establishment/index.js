jQuery(function($) {
  $('#establishMentTable').DataTable({
    searching: false,
    columnDefs: [
      {
        orderable: false,
        targets: -1
      }
    ]
  });
});