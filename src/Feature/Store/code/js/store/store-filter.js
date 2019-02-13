var url = window.location.origin + window.location.pathname;
var storeParams = getUrlVars();

// Update UI correspond to query param
$(document).ready(function () {
    if (storeParams.categoryId) {
        $('#store-category').val(storeParams.categoryId);
        $('#store-category-mobile').val(storeParams.categoryId);
    }
    if (storeParams.wingId) {
        $('#store-wing').val(storeParams.wingId);
        $('#store-wing-mobile').val(storeParams.wingId);
    }
    if (storeParams.mallId) {
        $('#store-mall').val(storeParams.mallId);
        $('#store-mall-mobile').val(storeParams.mallId);
    }
    if (storeParams.alphabetId) {
        $('#store-alphabet')
            .children()
            .removeClass('active');

        $('#store-alphabet > a')
            .filter(function () {
                return $(this).text() === storeParams.alphabetId;
            })
            .addClass('active');
        $('#store-alphabet-mobile').val(storeParams.alphabetId);
    }
    else {
        $('#store-alphabet a:first-child').addClass('active');
    }

    if (storeParams.storeOfferId) {
        $('#store-offer div, #store-offer-mobile div').each(function (index) {
            if (storeParams.storeOfferId.indexOf(this.dataset.id) > -1) {
                $(this).addClass('active');
            }
        });
    }
});

$('#store-category').change(function () {
    storeParams.categoryId = $('#store-category option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-category-mobile').change(function () {
    storeParams.categoryId = $('#store-category-mobile option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-wing').change(function () {
    storeParams.wingId = $('#store-wing option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-wing-mobile').change(function () {
    storeParams.wingId = $('#store-wing-mobile option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-mall').change(function () {
    storeParams.mallId = $('#store-mall option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-mall-mobile').change(function () {
    storeParams.mallId = $('#store-mall-mobile option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-alphabet').click(function () {
    event.preventDefault();
    if ($(event.target).is('a')) {
        storeParams.alphabetId = $(event.target).text();
        navigateWithParams(storeParams);
    }
});

$('#store-alphabet-mobile').change(function () {
    storeParams.alphabetId = $('#store-alphabet-mobile option:selected').val();
    navigateWithParams(storeParams);
});

$('#store-offer, #store-offer-mobile').click(function () {
    event.preventDefault();
    if ($(event.target).is('div')) {
        var offerIds = storeParams.storeOfferId
            ? decodeURIComponent(storeParams.storeOfferId).split(',')
            : [];
        var id = event.target.dataset.id;
        if (offerIds.indexOf(id) > -1) {
            offerIds = offerIds.filter(n => n !== id);
        } else {
            offerIds.push(id);
        }
        storeParams.storeOfferId = offerIds.join(',');
        navigateWithParams(storeParams);
    }
});
