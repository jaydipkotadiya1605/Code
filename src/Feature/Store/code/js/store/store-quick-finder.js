var url = window.location.origin + window.location.pathname;
var storeParams = getUrlVars();
var seachResultPageUrl;
$(document).ready(function () {
    var element = $('.store-finder'),
        keyword = element.find('input#keyword'),
        btnSearch = element.find('#btnStoreSearch'),
        seachResultPageUrl = element.find('#searchResultPageUrl').val();

    btnSearch.click(function (event) {
        event.preventDefault();
        handleSearch();
    });

    keyword.on('keydown', function (e) {
        if (e.which == 13) {
            e.preventDefault();
            handleSearch();
        }
    });
    function handleSearch() {
        storeParams.keyword = keyword.val();
        navigateTo(seachResultPageUrl, storeParams);
    }
});

