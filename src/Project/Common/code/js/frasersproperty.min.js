var GLOBAL = {
    LEASING_FORM_NO_EMPTY_FIELDS: {
        "applicant-name": "Please enter the name of applicant",
        "existing-shop-name": "Please enter applicant’s existing shop name. If not applicable, enter NIL",
        "trade-merchandise": "Please enter applicant’s trade/merchandise. If not applicable, enter NIL",
        "area-require": "Please enter the area required. If not applicable, enter NIL"
    }
};

function getUrlVars() {
    if (-1 === window.location.href.indexOf("?")) return {};
    for (var e, a = {}, t = window.location.href.slice(window.location.href.indexOf("?") + 1).split("&"), r = 0; r < t.length; r++) a[(e = t[r].split("="))[0]] = decodeURIComponent(e[1]);
    return a
}

function navigateWithParams(e, a = "") {
    window.location.assign(url + "?" + $.param(e, !0).replace(/\+/g, "%20") + a)
}

function navigateTo(e, a) {
    window.location.assign(e + "?" + $.param(a, !0).replace(/\+/g, "%20"))
}

function validateInteger(e) {
    return /^\d+$/.test(e)
}

function validateEmail(e) {
    return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(String(e).toLowerCase())
}

function between(e, a, t) {
    return e >= a && e <= t
}

function enquiryChecker() {
    return $("input[name=enquiry]:checked").val() ? ($("#enquiry-error").text(""), !0) : ($("#enquiry-error").text("Please select the space you would like to lease."), !1)
}

function salutationChecker() {
    var e = $("select[name=applicant-prefix]");
    return "Select" !== e.find(":selected").text() ? (e.removeClass("alert-danger"), $("#applicant-prefix-error").text(""), !0) : (e.addClass("alert-danger"), $("#applicant-prefix-error").text("Please select your salutation."), !1)
}

function emailChecker() {
    var e = $("input[name=email]");
    return validateEmail(e.val()) ? (e.removeClass("alert-danger"), $("#email-error").text(""), !0) : (e.addClass("alert-danger"), $("#email-error").text("Please enter a valid email address."), !1)
}

function emptyFieldChecker(e) {
    return 0 !== $("input[name=" + e + "]").val().length ? ($("input[name=" + e + "]").removeClass("alert-danger"), $("#" + e + "-error").text(""), !0) : ($("input[name=" + e + "]").addClass("alert-danger"), $("#" + e + "-error").text(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS[e]), !1)
}

function contactChecker() {
    var e = $("input[name=mobile-number]"),
        a = $("input[name=office-number]"),
        t = e.val(),
        r = a.val();
    if (t.length + r.length === 0) e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please enter at least mobile or office/home number.");
    else {
        if (validateInteger(t + r) && !between(t.length, 1, 7) && !between(r.length, 1, 7)) return e.removeClass("alert-danger"), a.removeClass("alert-danger"), $("#contact-error").text(""), !0;
        e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please check your entry")
    }
    return !1
}

function agreementChecker() {
    return $("#agreement").is(":checked") ? ($("#agreement-error").addClass("d-none"), !0) : ($("#agreement-error").removeClass("d-none"), !1)
}

function leasingFormChecker() {
    var e = event.target.name;
    switch (e) {
        case "enquiry":
            enquiryChecker();
            break;
        case "applicant-prefix":
            salutationChecker();
            break;
        case "email":
            emailChecker();
            break;
        case "mobile-number":
        case "office-number":
            contactChecker();
            break;
        default:
            GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS.hasOwnProperty(e) && emptyFieldChecker(e)
    }
}

function submitLeasing() {
    event.preventDefault();
    var e = Object.keys(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS),
        a = !0;
    a &= enquiryChecker(), a &= salutationChecker(), a &= emailChecker(), a &= contactChecker(), a &= agreementChecker();
    for (var t = 0; t < e.length; t++) a &= emptyFieldChecker(e[t]);
    a && $("#submit-success").removeClass("d-none")
}
$("#full-overlay").on("click", closeOverlay), $("#full-overlay .search-wrapper .row").on("click", function () {
    event.stopPropagation()
}), $(document).keyup(function (e) {
    27 == e.keyCode && "100%" === document.getElementById("full-overlay").style.height && closeOverlay()
}), $(".leasingform").on("focusout", leasingFormChecker), $("input[name=enquiry]").change(enquiryChecker), $("select[name=applicant-prefix]").change(salutationChecker), $("#agreement").on("click", agreementChecker);
GLOBAL = {
    LEASING_FORM_NO_EMPTY_FIELDS: {
        "applicant-name": "Please enter the name of applicant",
        "existing-shop-name": "Please enter applicant’s existing shop name. If not applicable, enter NIL",
        "trade-merchandise": "Please enter applicant’s trade/merchandise. If not applicable, enter NIL",
        "area-require": "Please enter the area required. If not applicable, enter NIL"
    }
};

function getUrlVars() {
    if (-1 === window.location.href.indexOf("?")) return {};
    for (var e, a = {}, t = window.location.href.slice(window.location.href.indexOf("?") + 1).split("&"), r = 0; r < t.length; r++) a[(e = t[r].split("="))[0]] = decodeURIComponent(e[1]);
    return a
}

function navigateWithParams(e, a = "") {
    window.location.assign(url + "?" + $.param(e, !0).replace(/\+/g, "%20") + a)
}

function navigateTo(e, a) {
    window.location.assign(e + "?" + $.param(a, !0).replace(/\+/g, "%20"))
}

function validateInteger(e) {
    return /^\d+$/.test(e)
}

function validateEmail(e) {
    return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(String(e).toLowerCase())
}

function between(e, a, t) {
    return e >= a && e <= t
}

function enquiryChecker() {
    return $("input[name=enquiry]:checked").val() ? ($("#enquiry-error").text(""), !0) : ($("#enquiry-error").text("Please select the space you would like to lease."), !1)
}

function salutationChecker() {
    var e = $("select[name=applicant-prefix]");
    return "Select" !== e.find(":selected").text() ? (e.removeClass("alert-danger"), $("#applicant-prefix-error").text(""), !0) : (e.addClass("alert-danger"), $("#applicant-prefix-error").text("Please select your salutation."), !1)
}

function emailChecker() {
    var e = $("input[name=email]");
    return validateEmail(e.val()) ? (e.removeClass("alert-danger"), $("#email-error").text(""), !0) : (e.addClass("alert-danger"), $("#email-error").text("Please enter a valid email address."), !1)
}

function emptyFieldChecker(e) {
    return 0 !== $("input[name=" + e + "]").val().length ? ($("input[name=" + e + "]").removeClass("alert-danger"), $("#" + e + "-error").text(""), !0) : ($("input[name=" + e + "]").addClass("alert-danger"), $("#" + e + "-error").text(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS[e]), !1)
}

function contactChecker() {
    var e = $("input[name=mobile-number]"),
        a = $("input[name=office-number]"),
        t = e.val(),
        r = a.val();
    if (t.length + r.length === 0) e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please enter at least mobile or office/home number.");
    else {
        if (validateInteger(t + r) && !between(t.length, 1, 7) && !between(r.length, 1, 7)) return e.removeClass("alert-danger"), a.removeClass("alert-danger"), $("#contact-error").text(""), !0;
        e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please check your entry")
    }
    return !1
}

function agreementChecker() {
    return $("#agreement").is(":checked") ? ($("#agreement-error").addClass("d-none"), !0) : ($("#agreement-error").removeClass("d-none"), !1)
}

function leasingFormChecker() {
    var e = event.target.name;
    switch (e) {
        case "enquiry":
            enquiryChecker();
            break;
        case "applicant-prefix":
            salutationChecker();
            break;
        case "email":
            emailChecker();
            break;
        case "mobile-number":
        case "office-number":
            contactChecker();
            break;
        default:
            GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS.hasOwnProperty(e) && emptyFieldChecker(e)
    }
}

function submitLeasing() {
    event.preventDefault();
    var e = Object.keys(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS),
        a = !0;
    a &= enquiryChecker(), a &= salutationChecker(), a &= emailChecker(), a &= contactChecker(), a &= agreementChecker();
    for (var t = 0; t < e.length; t++) a &= emptyFieldChecker(e[t]);
    a && $("#submit-success").removeClass("d-none")
}
$("#full-overlay").on("click", closeOverlay), $("#full-overlay .search-wrapper .row").on("click", function () {
    event.stopPropagation()
}), $(document).keyup(function (e) {
    27 == e.keyCode && "100%" === document.getElementById("full-overlay").style.height && closeOverlay()
}), $(".leasingform").on("focusout", leasingFormChecker), $("input[name=enquiry]").change(enquiryChecker), $("select[name=applicant-prefix]").change(salutationChecker), $("#agreement").on("click", agreementChecker);
GLOBAL = {
    LEASING_FORM_NO_EMPTY_FIELDS: {
        "applicant-name": "Please enter the name of applicant",
        "existing-shop-name": "Please enter applicant’s existing shop name. If not applicable, enter NIL",
        "trade-merchandise": "Please enter applicant’s trade/merchandise. If not applicable, enter NIL",
        "area-require": "Please enter the area required. If not applicable, enter NIL"
    }
};

function getUrlVars() {
    if (-1 === window.location.href.indexOf("?")) return {};
    for (var e, a = {}, t = window.location.href.slice(window.location.href.indexOf("?") + 1).split("&"), r = 0; r < t.length; r++) a[(e = t[r].split("="))[0]] = decodeURIComponent(e[1]);
    return a
}

function navigateWithParams(e, a = "") {
    window.location.assign(url + "?" + $.param(e, !0).replace(/\+/g, "%20") + a)
}

function navigateTo(e, a) {
    window.location.assign(e + "?" + $.param(a, !0).replace(/\+/g, "%20"))
}

function validateInteger(e) {
    return /^\d+$/.test(e)
}

function validateEmail(e) {
    return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(String(e).toLowerCase())
}

function between(e, a, t) {
    return e >= a && e <= t
}

function enquiryChecker() {
    return $("input[name=enquiry]:checked").val() ? ($("#enquiry-error").text(""), !0) : ($("#enquiry-error").text("Please select the space you would like to lease."), !1)
}

function salutationChecker() {
    var e = $("select[name=applicant-prefix]");
    return "Select" !== e.find(":selected").text() ? (e.removeClass("alert-danger"), $("#applicant-prefix-error").text(""), !0) : (e.addClass("alert-danger"), $("#applicant-prefix-error").text("Please select your salutation."), !1)
}

function emailChecker() {
    var e = $("input[name=email]");
    return validateEmail(e.val()) ? (e.removeClass("alert-danger"), $("#email-error").text(""), !0) : (e.addClass("alert-danger"), $("#email-error").text("Please enter a valid email address."), !1)
}

function emptyFieldChecker(e) {
    return 0 !== $("input[name=" + e + "]").val().length ? ($("input[name=" + e + "]").removeClass("alert-danger"), $("#" + e + "-error").text(""), !0) : ($("input[name=" + e + "]").addClass("alert-danger"), $("#" + e + "-error").text(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS[e]), !1)
}

function contactChecker() {
    var e = $("input[name=mobile-number]"),
        a = $("input[name=office-number]"),
        t = e.val(),
        r = a.val();
    if (t.length + r.length === 0) e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please enter at least mobile or office/home number.");
    else {
        if (validateInteger(t + r) && !between(t.length, 1, 7) && !between(r.length, 1, 7)) return e.removeClass("alert-danger"), a.removeClass("alert-danger"), $("#contact-error").text(""), !0;
        e.addClass("alert-danger"), a.addClass("alert-danger"), $("#contact-error").text("Please check your entry")
    }
    return !1
}

function agreementChecker() {
    return $("#agreement").is(":checked") ? ($("#agreement-error").addClass("d-none"), !0) : ($("#agreement-error").removeClass("d-none"), !1)
}

function leasingFormChecker() {
    var e = event.target.name;
    switch (e) {
        case "enquiry":
            enquiryChecker();
            break;
        case "applicant-prefix":
            salutationChecker();
            break;
        case "email":
            emailChecker();
            break;
        case "mobile-number":
        case "office-number":
            contactChecker();
            break;
        default:
            GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS.hasOwnProperty(e) && emptyFieldChecker(e)
    }
}

function submitLeasing() {
    event.preventDefault();
    var e = Object.keys(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS),
        a = !0;
    a &= enquiryChecker(), a &= salutationChecker(), a &= emailChecker(), a &= contactChecker(), a &= agreementChecker();
    for (var t = 0; t < e.length; t++) a &= emptyFieldChecker(e[t]);
    a && $("#submit-success").removeClass("d-none")
}

function openNav() {
    $(".sidenav").width("100%"), $("body").addClass("modal-open")
}

function closeNav() {
    $(".sidenav").width("0"), $("body").removeClass("modal-open"), $(".select-mall .sub").removeClass("show")
}

function openSearch() {
    $(".sidesearch").addClass("show")
}

function closeSearch() {
    $(".sidesearch").removeClass("show")
}

function showResults() {
    $(".searchresults").slideDown()
}

function openOverlay() {
    document.getElementById("full-overlay").style.height = "100%"
}

function closeOverlay() {
    document.getElementById("full-overlay").style.height = "0"
}

function selectProperty() {
    event.stopPropagation(), $(".select-mall .sub").hasClass("show") ? ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up")) : ($(".select-mall .sub").addClass("show"), $(".select-mall .selected").addClass("arrow-up"))
}

function addSub() {
    event.stopPropagation(), $(".filter-dropdown .sub").hasClass("show") ? ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up")) : ($(".filter-dropdown .sub").addClass("show"), $(".filter-dropdown .selected").addClass("arrow-up"))
}

function addSecondary() {
    event.stopPropagation(), $(".filter-dropdown.secondary .sub-secondary").hasClass("show") ? ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up")) : ($(".filter-dropdown.secondary .sub-secondary").addClass("show"), $(".filter-dropdown.secondary .selected-secondary").addClass("arrow-up"))
}

function openNav() {
    $(".sidenav").width("100%"), $("body").addClass("modal-open")
}

function closeNav() {
    $(".sidenav").width("0"), $("body").removeClass("modal-open"), $(".select-mall .sub").removeClass("show")
}

function openSearch() {
    $(".sidesearch").addClass("show")
}

function closeSearch() {
    $(".sidesearch").removeClass("show")
}

function showResults() {
    $(".searchresults").slideDown()
}

function openOverlay() {
    document.getElementById("full-overlay").style.height = "100%"
}

function closeOverlay() {
    document.getElementById("full-overlay").style.height = "0"
}

function selectProperty() {
    event.stopPropagation(), $(".select-mall .sub").hasClass("show") ? ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up")) : ($(".select-mall .sub").addClass("show"), $(".select-mall .selected").addClass("arrow-up"))
}

function addSub() {
    event.stopPropagation(), $(".filter-dropdown .sub").hasClass("show") ? ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up")) : ($(".filter-dropdown .sub").addClass("show"), $(".filter-dropdown .selected").addClass("arrow-up"))
}

function addSecondary() {
    event.stopPropagation(), $(".filter-dropdown.secondary .sub-secondary").hasClass("show") ? ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up")) : ($(".filter-dropdown.secondary .sub-secondary").addClass("show"), $(".filter-dropdown.secondary .selected-secondary").addClass("arrow-up"))
}

function openNav() {
    $(".sidenav").width("100%"), $("body").addClass("modal-open")
}

function closeNav() {
    $(".sidenav").width("0"), $("body").removeClass("modal-open"), $(".select-mall .sub").removeClass("show")
}

function openSearch() {
    $(".sidesearch").addClass("show")
}

function closeSearch() {
    $(".sidesearch").removeClass("show")
}

function showResults() {
    $(".searchresults").slideDown()
}

function openOverlay() {
    document.getElementById("full-overlay").style.height = "100%"
}

function closeOverlay() {
    document.getElementById("full-overlay").style.height = "0"
}

function selectProperty() {
    event.stopPropagation(), $(".select-mall .sub").hasClass("show") ? ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up")) : ($(".select-mall .sub").addClass("show"), $(".select-mall .selected").addClass("arrow-up"))
}

function addSub() {
    event.stopPropagation(), $(".filter-dropdown .sub").hasClass("show") ? ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up")) : ($(".filter-dropdown .sub").addClass("show"), $(".filter-dropdown .selected").addClass("arrow-up"))
}

function addSecondary() {
    event.stopPropagation(), $(".filter-dropdown.secondary .sub-secondary").hasClass("show") ? ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up")) : ($(".filter-dropdown.secondary .sub-secondary").addClass("show"), $(".filter-dropdown.secondary .selected-secondary").addClass("arrow-up"))
}
$("#full-overlay").on("click", closeOverlay), $("#full-overlay .search-wrapper .row").on("click", function () {
    event.stopPropagation()
}), $(document).keyup(function (e) {
    27 == e.keyCode && "100%" === document.getElementById("full-overlay").style.height && closeOverlay()
}), $(".leasingform").on("focusout", leasingFormChecker), $("input[name=enquiry]").change(enquiryChecker), $("select[name=applicant-prefix]").change(salutationChecker), $("#agreement").on("click", agreementChecker), $(window).scroll(function () {
    $(document).scrollTop() > 30 ? $("header").addClass("shrink") : $("header").removeClass("shrink")
}), $(document).on("click", function (e) {
    $(".select-mall .sub").hasClass("show") && ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown .sub").hasClass("show") && ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown.secondary .sub-secondary").hasClass("show") && ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up"))
}), $(".legend").find(".item").click(function () {
    $(this).hasClass("active") ? $(this).removeClass("active") : $(this).addClass("active")
}), $(".loadmore").click(function (e) {
    $(".hidden-items").slideDown(), e.preventDefault()
}), -1 === document.cookie.indexOf("hide_cookie_tray=1") && $(".cookie-tray").addClass("show"), $(".cookie-tray").find(".btn").click(function () {
    $(".cookie-tray").removeClass("show"), document.cookie = "hide_cookie_tray=1; expires=Fri, 31 Dec 9999 23:59:59 GMT"
}), $(".wing-open").length > 0 ? ($(".category-select").removeClass("full"), $(".wing-select").removeClass("full")) : ($(".category-select").addClass("full"), $(".wing-select").addClass("full")), $(".textbody table").removeAttr("style cellspacing cellpadding border"), $(".textbody td").removeAttr("style nowrap"), $(".textbody tr").removeAttr("style"), $(".textbody img").removeAttr("style"), $('.textbody [style*="font-size"]').css("font-size", ""), $(".textbody table").wrap('<div class="table-wrap"></div>'), $("#enquiry-type").change(function () {
    var e = $("option:selected").val();
    "General Enquiry" === e ? ($("section.general-enquiry").show(), $("section.leasing-enquiry").hide()) : "Leasing Enquiry" === e && ($("section.general-enquiry").hide(), $("section.leasing-enquiry").show())
}), $(".last-steps .btn").click(function () {
    $(".submit-status").fadeIn()
}), $(window).scroll(function () {
    $(document).scrollTop() > 30 ? $("header").addClass("shrink") : $("header").removeClass("shrink")
}), $(document).on("click", function (e) {
    $(".select-mall .sub").hasClass("show") && ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown .sub").hasClass("show") && ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown.secondary .sub-secondary").hasClass("show") && ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up"))
}), $(".legend").find(".item").click(function () {
    $(this).hasClass("active") ? $(this).removeClass("active") : $(this).addClass("active")
}), $(".loadmore").click(function (e) {
    $(".hidden-items").slideDown(), e.preventDefault()
}), -1 === document.cookie.indexOf("hide_cookie_tray=1") && $(".cookie-tray").addClass("show"), $(".cookie-tray").find(".btn").click(function () {
    $(".cookie-tray").removeClass("show"), document.cookie = "hide_cookie_tray=1; expires=Fri, 31 Dec 9999 23:59:59 GMT"
}), $(".wing-open").length > 0 ? ($(".category-select").removeClass("full"), $(".wing-select").removeClass("full")) : ($(".category-select").addClass("full"), $(".wing-select").addClass("full")), $(".textbody table").removeAttr("style cellspacing cellpadding border"), $(".textbody td").removeAttr("style nowrap"), $(".textbody tr").removeAttr("style"), $(".textbody img").removeAttr("style"), $('.textbody [style*="font-size"]').css("font-size", ""), $(".textbody table").wrap('<div class="table-wrap"></div>'), $("#enquiry-type").change(function () {
    var e = $("option:selected").val();
    "General Enquiry" === e ? ($("section.general-enquiry").show(), $("section.leasing-enquiry").hide()) : "Leasing Enquiry" === e && ($("section.general-enquiry").hide(), $("section.leasing-enquiry").show())
}), $(".last-steps .btn").click(function () {
    $(".submit-status").fadeIn()
}), $(window).scroll(function () {
    $(document).scrollTop() > 30 ? $("header").addClass("shrink") : $("header").removeClass("shrink")
}), $(document).on("click", function (e) {
    $(".select-mall .sub").hasClass("show") && ($(".select-mall .sub").removeClass("show"), $(".select-mall .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown .sub").hasClass("show") && ($(".filter-dropdown .sub").removeClass("show"), $(".filter-dropdown .selected").removeClass("arrow-up"))
}), $(document).on("click", function (e) {
    $(".filter-dropdown.secondary .sub-secondary").hasClass("show") && ($(".filter-dropdown.secondary .sub-secondary").removeClass("show"), $(".filter-dropdown.secondary .selected-secondary").removeClass("arrow-up"))
}), $(".legend").find(".item").click(function () {
    $(this).hasClass("active") ? $(this).removeClass("active") : $(this).addClass("active")
}), $(".loadmore").click(function (e) {
    $(".hidden-items").slideDown(), e.preventDefault()
}), -1 === document.cookie.indexOf("hide_cookie_tray=1") && $(".cookie-tray").addClass("show"), $(".cookie-tray").find(".btn").click(function () {
    $(".cookie-tray").removeClass("show"), document.cookie = "hide_cookie_tray=1; expires=Fri, 31 Dec 9999 23:59:59 GMT"
}), $(".wing-open").length > 0 ? ($(".category-select").removeClass("full"), $(".wing-select").removeClass("full")) : ($(".category-select").addClass("full"), $(".wing-select").addClass("full")), $(".textbody table").removeAttr("style cellspacing cellpadding border"), $(".textbody td").removeAttr("style nowrap"), $(".textbody tr").removeAttr("style"), $(".textbody img").removeAttr("style"), $('.textbody [style*="font-size"]').css("font-size", ""), $(".textbody table").wrap('<div class="table-wrap"></div>'), $("#enquiry-type").change(function () {
    var e = $("option:selected").val();
    "General Enquiry" === e ? ($("section.general-enquiry").show(), $("section.leasing-enquiry").hide()) : "Leasing Enquiry" === e && ($("section.general-enquiry").hide(), $("section.leasing-enquiry").show())
}), $(".last-steps .btn").click(function () {
    $(".submit-status").fadeIn()
});
var url = window.location.origin + window.location.pathname,
    happeningParams = getUrlVars();

function toTitleCase(e) {
    return e.replace(/\w\S*/g, function (e) {
        return e.charAt(0).toUpperCase() + e.substr(1).toLowerCase()
    })
}
$("#tab-category-filter, #tab-category-filter-mobile-dropdown").click(function () {
    event.preventDefault(), $(event.target).is("a") && (happeningParams.category = event.target.dataset.name, happeningParams.pageIndex = 1, void 0 !== happeningParams.mallId && (happeningParams.mallId = ""), navigateWithParams(happeningParams))
}), $(document).ready(function () {
    if (happeningParams.category) {
        $("#tab-category-filter").children().removeClass("active");
        var e = toTitleCase(happeningParams.category);
        $('#tab-category-filter > a:contains("' + e + '")').addClass("active"), $("#tab-category-filter-mobile-text").text(e)
    }
});
url = window.location.origin + window.location.pathname, happeningParams = getUrlVars();

function toTitleCase(e) {
    return e.replace(/\w\S*/g, function (e) {
        return e.charAt(0).toUpperCase() + e.substr(1).toLowerCase()
    })
}
$("#tab-category-filter, #tab-category-filter-mobile-dropdown").click(function () {
    event.preventDefault(), $(event.target).is("a") && (happeningParams.category = event.target.dataset.name, happeningParams.pageIndex = 1, void 0 !== happeningParams.mallId && (happeningParams.mallId = ""), navigateWithParams(happeningParams))
}), $(document).ready(function () {
    if (happeningParams.category) {
        $("#tab-category-filter").children().removeClass("active");
        var e = toTitleCase(happeningParams.category);
        $('#tab-category-filter > a:contains("' + e + '")').addClass("active"), $("#tab-category-filter-mobile-text").text(e)
    }
});
url = window.location.origin + window.location.pathname, happeningParams = getUrlVars();

function toTitleCase(e) {
    return e.replace(/\w\S*/g, function (e) {
        return e.charAt(0).toUpperCase() + e.substr(1).toLowerCase()
    })
}
$("#tab-category-filter, #tab-category-filter-mobile-dropdown").click(function () {
    event.preventDefault(), $(event.target).is("a") && (happeningParams.category = event.target.dataset.name, happeningParams.pageIndex = 1, void 0 !== happeningParams.mallId && (happeningParams.mallId = ""), navigateWithParams(happeningParams))
}), $(document).ready(function () {
    if (happeningParams.category) {
        $("#tab-category-filter").children().removeClass("active");
        var e = toTitleCase(happeningParams.category);
        $('#tab-category-filter > a:contains("' + e + '")').addClass("active"), $("#tab-category-filter-mobile-text").text(e)
    }
});