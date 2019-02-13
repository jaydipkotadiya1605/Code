var url = window.location.origin + window.location.pathname;
var happeningParams = getUrlVars();

$('#tab-category-filter, #tab-category-filter-mobile-dropdown').click(function () {
	event.preventDefault();
	if ($(event.target).is('a')) {
		happeningParams.category = event.target.dataset.name;
		happeningParams.pageIndex = 1;
		if (typeof happeningParams.mallId != "undefined")
		    happeningParams.mallId = "";
		navigateWithParams(happeningParams);
	}
});

$(document).ready(function () {
	if (happeningParams.category) {
		$('#tab-category-filter')
			.children()
			.removeClass('active');
		var categoryValue = toTitleCase(happeningParams.category);
		$(
			'#tab-category-filter > a:contains("' + categoryValue + '")'
		).addClass('active');
		$('#tab-category-filter-mobile-text').text(categoryValue);
	}
});

function toTitleCase(str) {
	return str.replace(/\w\S*/g, function (txt) {
		return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
	});
}