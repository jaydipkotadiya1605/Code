var url = window.location.origin + window.location.pathname;
var eventParams = getUrlVars();

// Update UI correspond to query param
$(document).ready(function() {
	if (eventParams.mallId) {
	  $('#store-mall').val(eventParams.mallId);
	  $('#store-mall-mobile').val(eventParams.mallId);
  }
});

$('#store-mall').change(function() {
	eventParams.mallId = $('#store-mall option:selected').val();
	eventParams.pageIndex = 1;
	navigateWithParams(eventParams);
});

$('#store-mall-mobile').change(function() {
	eventParams.mallId = $('#store-mall-mobile option:selected').val();
  navigateWithParams(eventParams);
});
