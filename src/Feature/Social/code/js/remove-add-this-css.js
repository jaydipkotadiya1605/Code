$(window).load(function () {
    $('a.item').each(function (index) {
        if ($(this).hasClass('at300b')) {
            $(this).removeClass('at300b');
        }
    });
    $('div.addthis_toolbox').removeClass('invisible');
});