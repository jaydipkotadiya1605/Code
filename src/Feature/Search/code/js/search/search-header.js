var url = window.location.origin + window.location.pathname;
var storeParams = getUrlVars();
var seachResultPageUrl;
$(document).ready(function () {
    var element = $('.refine-search'),
        totalItem = element.find('#totalNumberOfResults'),
        searchKeywordItem = element.find('#searchKeyword'),
        keyword = element.find('input#keyword'),
        btnSearch = element.find('#btnSearch'),
        seachResultPageUrl = element.find('#searchResultPageUrl').val(),
        searchResults = $('#searchResults input#totalNumberOfResults');

    if (storeParams.keyword == undefined) {
        storeParams.keyword = "";
    }
    if (searchResults.length) {
        var total = 0;
        searchResults.each(function () {
            total += parseInt(this.value);
        });

        totalItem.text(total);
        searchKeywordItem.text(`"${storeParams.keyword}"`);
        keyword.val(storeParams.keyword);
    }

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
        if (storeParams.pageIndex != undefined) {
            storeParams.pageIndex = 1;
        }
        navigateTo(seachResultPageUrl, storeParams);
    }
});