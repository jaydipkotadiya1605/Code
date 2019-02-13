var url = window.location.origin + window.location.pathname;
var storeParams = getUrlVars();
var seachResultPageUrl;
$(document).ready(function () {
    //desktop
    var element = $('#full-overlay'),
        keyword = element.find('input.search-field'),
        btnSearch = element.find('.btn'),
        seachResultPageUrl = element.find('#searchResultPageUrl').val();

    btnSearch.click(function (event) {
        event.preventDefault();
        handleSearch(keyword.val());
    });
    keyword.on('keydown', function (e) {
        if (e.which == 13) {
            e.preventDefault();
            handleSearch(keyword.val());
        }
    });
    function handleSearch(text) {
        storeParams.keyword = text;
        if (storeParams.pageIndex != undefined) {
            storeParams.pageIndex = 1;
        }
        navigateTo(seachResultPageUrl, storeParams);
    }

    //mobile
    var searchbar = $('.row.searchbar'),
        textSearch = searchbar.find('.textfield input[type="text"]'),
        aSearch = searchbar.find('.search-icon a');

    aSearch.click(function(event) {
        event.preventDefault();
        handleSearch(textSearch.val());
    });

    textSearch.on('keydown', function (e) {
        if (e.which == 13) {
            e.preventDefault();
            handleSearch(textSearch.val());
        }
    });
});
