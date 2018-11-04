jQuery(function($) {
	var addEditModal = $('#editViolation'),
			addEditViolationBtn = $('.addEditViolationBtn'),
			addEditViolationForm = $('.addEditViolationForm'),
			addEditCode = $('.addEditCode'),
			addEditFindingViolation = $('.addEditFindingViolation'),
			addEditSection = $('.addEditSection'),
			addEditCompliance = $('.addEditCompliance'),
			addEditGracePeriod = $('.addEditGracePeriod'),
			addEditOccupancyType = $('.addEditOccupancyType'),
			addEditMinimumFine = $('.addEditMinimumFine'),
			addEditMaximumFine = $('.addEditMaximumFine'),
			violationTable = $('#violationTable'),
			table = $('.table');

	table.on('click', '.editViolation', function(e) {
		e.preventDefault();
		addEditModal.modal('show');
		setDataViolation(getViolationId($(this)));
		addEditViolationBtn.text('Save')
	})

	addEditModal.on('hidden.bs.modal', function(e) {
		e.preventDefault();
		addEditViolationForm.trigger('reset');
    addEditViolationBtn.text('Submit');
	});

	// DataTable Configurations
	violationTable.DataTable({
		searching: false,
	});

	// Allow numbers only on minimum and maximum amounts
	addEditMinimumFine.keydown(numbersOnly);
	addEditMaximumFine.keydown(numbersOnly);

	function getViolationId(element) {
		var seriesId = element.parents('tr').data('id');
    return seriesId;
	}
	
	function setDataViolation(id) {
		// Query from db and populate data into form
		addEditCode.val('123');
		addEditFindingViolation.val('Sample Violation')
		addEditSection.val('sample section')
		addEditCompliance.val('sample compliance');
		addEditGracePeriod.val('sample grace period');
		addEditOccupancyType.val('1');
		addEditMinimumFine.val('1.00');
		addEditMaximumFine.val('5.00');
	}

	function numbersOnly(e) {
		if (
			// Allow: backspace, delete, tab, escape, enter, . and numpad .
			$.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
			// Allow: Ctrl+A, Command+A
			(e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
			// Allow: home, end, left, right, down, up
			(e.keyCode >= 35 && e.keyCode <= 40)
		) {
			// let it happen, don't do anything
			return;
		}
			// Ensure that it is a number and stop the keypress
		if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
			e.preventDefault();
		}
	}
});