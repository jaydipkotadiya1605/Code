// header shrink
$(window).scroll(function() {
  if ($(document).scrollTop() > 30) {
    $('header').addClass('shrink');
  } else {
    $('header').removeClass('shrink');
  }
});

// side nav
function openNav() {
    $('.sidenav').width('100%');
    $('body').addClass('modal-open');
}

function closeNav() {
    $('.sidenav').width('0');
    $('body').removeClass('modal-open');
    $('.select-mall .sub').removeClass('show');
}

// side search
function openSearch() {
    $('.sidesearch').addClass('show');
}

function closeSearch() {
    $('.sidesearch').removeClass('show');
}

function showResults() {
	$('.searchresults').slideDown();
}

// full overlay
function openOverlay() {
    document.getElementById("full-overlay").style.height = "100%";
}
function closeOverlay() {
    document.getElementById("full-overlay").style.height = "0";
}

// property sub
function selectProperty() {
    event.stopPropagation();
    if ( $('.select-mall .sub').hasClass('show') ){
        $('.select-mall .sub').removeClass('show');
        $('.select-mall .selected').removeClass('arrow-up');
    } else {
        $('.select-mall .sub').addClass('show');
        $('.select-mall .selected').addClass('arrow-up');
    }
}

// click outside of sub to close (property sub)
$(document).on('click', function(e) {
    if ($('.select-mall .sub').hasClass('show')) {
        $('.select-mall .sub').removeClass('show');
        $('.select-mall .selected').removeClass('arrow-up');
    }
});
  
// sub for filters (mobile)
function addSub() {
    event.stopPropagation();
    if ( $('.filter-dropdown .sub').hasClass('show') ){
        $('.filter-dropdown .sub').removeClass('show');
        $('.filter-dropdown .selected').removeClass('arrow-up');
    } else {
        $('.filter-dropdown .sub').addClass('show');
        $('.filter-dropdown .selected').addClass('arrow-up');
    }
}

// click outside of sub to close (mobile)
$(document).on('click', function(e) {
    if ($('.filter-dropdown .sub').hasClass('show')) {
        $('.filter-dropdown .sub').removeClass('show');
        $('.filter-dropdown .selected').removeClass('arrow-up');
    }
});

//sub for filters (secondary)
function addSecondary() {
    event.stopPropagation();
    if ( $('.filter-dropdown.secondary .sub-secondary').hasClass('show') ){
        $('.filter-dropdown.secondary .sub-secondary').removeClass('show');
        $('.filter-dropdown.secondary .selected-secondary').removeClass('arrow-up');
    } else {
        $('.filter-dropdown.secondary .sub-secondary').addClass('show');
        $('.filter-dropdown.secondary .selected-secondary').addClass('arrow-up');
    }
}

// click outside of sub to close (secondary)
$(document).on('click', function(e) {
    if ($('.filter-dropdown.secondary .sub-secondary').hasClass('show')) {
        $('.filter-dropdown.secondary .sub-secondary').removeClass('show');
        $('.filter-dropdown.secondary .selected-secondary').removeClass('arrow-up');
    }
});

// store filter
$('.legend').find('.item').click(function(){
	if ($(this).hasClass('active')) {
		$(this).removeClass('active');
	} else {
		$(this).addClass('active');
	}
});

// show hidden items
$('.loadmore').click(function (e) {
	$('.hidden-items').slideDown();
	e.preventDefault();
});

// show cookie tray
if (document.cookie.indexOf('hide_cookie_tray=1') === -1) {
    $('.cookie-tray').addClass('show');
}

$('.cookie-tray').find('.btn').click(function(){
    $('.cookie-tray').removeClass('show');
    document.cookie = 'hide_cookie_tray=1; expires=Fri, 31 Dec 9999 23:59:59 GMT';
});

// check if there is wing
if ( $('.wing-open').length > 0 ){
    $('.category-select').removeClass('full');
    $('.wing-select').removeClass('full');
} else {
    $('.category-select').addClass('full');
    $('.wing-select').addClass('full');
}

// textbody CSS strip
$('.textbody table').removeAttr('style cellspacing cellpadding border');
$('.textbody td').removeAttr('style nowrap');
$('.textbody tr').removeAttr('style');
$('.textbody img').removeAttr('style');
$('.textbody [style*="font-size"]').css('font-size', '');

// textbody table wrap
$('.textbody table').wrap( '<div class="table-wrap"></div>' );

// enquiry type
$("#enquiry-type").change(function () {

    var option = $('option:selected').val();

    if ( option === 'General Enquiry' ) {
        $('section.general-enquiry').show();
        $('section.leasing-enquiry').hide();

    } else if ( option === 'Leasing Enquiry' ) {
        $('section.general-enquiry').hide();
        $('section.leasing-enquiry').show();
    }
});

// submit
$('.last-steps .btn').click(function () {
	$('.submit-status').fadeIn();
});