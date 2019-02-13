// main-extend.js For Quan
var GLOBAL = {
  LEASING_FORM_NO_EMPTY_FIELDS: {
    'applicant-name': 'Please enter the name of applicant',
    'existing-shop-name':
      'Please enter applicant’s existing shop name. If not applicable, enter NIL',
    'trade-merchandise':
      'Please enter applicant’s trade/merchandise. If not applicable, enter NIL',
    'area-require':
      'Please enter the area required. If not applicable, enter NIL'
  }
};

// Read a page's GET URL variables and return them as an object.
function getUrlVars() {
  if (window.location.href.indexOf('?') === -1) {
    return {};
  }

  var vars = {},
    hash;
  var hashes = window.location.href
    .slice(window.location.href.indexOf('?') + 1)
    .split('&');
  for (var i = 0; i < hashes.length; i++) {
    hash = hashes[i].split('=');
    vars[hash[0]] = decodeURIComponent(hash[1]);
  }

  return vars;
}

// Navigate to another page with query string
function navigateWithParams(params, anchor = "") {
    window.location.assign(
    url + '?' + $.param(params, true).replace(/\+/g, '%20') + anchor
  );
}

function navigateTo(url, params) {
    window.location.assign(
        url + '?' + $.param(params, true).replace(/\+/g, '%20')
    );
}


// full overlay
$('#full-overlay').on('click', closeOverlay);
$('#full-overlay .search-wrapper .row').on('click', function() {
  event.stopPropagation();
});
$(document).keyup(function(e) {
  if (
    e.keyCode == 27 &&
    document.getElementById('full-overlay').style.height === '100%'
  ) {
    closeOverlay();
  }
});

// leasing form handler
function validateInteger(s) {
  // Validate phone number
  return /^\d+$/.test(s);
}

function validateEmail(email) {
  var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(String(email).toLowerCase());
}

function between(x, min, max) {
  return x >= min && x <= max;
}

function enquiryChecker() {
  if ($('input[name=enquiry]:checked').val()) {
    $('#enquiry-error').text('');
    return true;
  } else {
    $('#enquiry-error').text(
      'Please select the space you would like to lease.'
    );
    return false;
  }
}

function salutationChecker() {
  var salutationTag = $('select[name=applicant-prefix]');
  if (salutationTag.find(':selected').text() !== 'Select') {
    salutationTag.removeClass('alert-danger');
    $('#applicant-prefix-error').text('');
    return true;
  } else {
    salutationTag.addClass('alert-danger');
    $('#applicant-prefix-error').text('Please select your salutation.');
    return false;
  }
}

function emailChecker() {
  var emailTag = $('input[name=email]');
  if (validateEmail(emailTag.val())) {
    emailTag.removeClass('alert-danger');
    $('#email-error').text('');
    return true;
  } else {
    emailTag.addClass('alert-danger');
    $('#email-error').text('Please enter a valid email address.');
    return false;
  }
}

function emptyFieldChecker(name) {
  var tag = $('input[name=' + name + ']');
  if (tag.val().length !== 0) {
    $('input[name=' + name + ']').removeClass('alert-danger');
    $('#' + name + '-error').text('');
    return true;
  } else {
    $('input[name=' + name + ']').addClass('alert-danger');
    $('#' + name + '-error').text(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS[name]);
    return false;
  }
}

function contactChecker() {
  var mobile = $('input[name=mobile-number]');
  var office = $('input[name=office-number]');
  var mobileVal = mobile.val();
  var officeVal = office.val();

  if (mobileVal.length + officeVal.length === 0) {
    // Must fill a least one field
    mobile.addClass('alert-danger');
    office.addClass('alert-danger');
    $('#contact-error').text(
      'Please enter at least mobile or office/home number.'
    );
  } else if (
    !validateInteger(mobileVal + officeVal) ||
    between(mobileVal.length, 1, 7) ||
    between(officeVal.length, 1, 7)
  ) {
    // Validate provided value
    mobile.addClass('alert-danger');
    office.addClass('alert-danger');
    $('#contact-error').text('Please check your entry');
  } else {
    mobile.removeClass('alert-danger');
    office.removeClass('alert-danger');
    $('#contact-error').text('');
    return true;
  }
  return false;
}

function agreementChecker() {
  if ($('#agreement').is(':checked')) {
    $('#agreement-error').addClass('d-none');
    return true;
  } else {
    $('#agreement-error').removeClass('d-none');
    return false;
  }
}

function leasingFormChecker() {
  var targetName = event.target.name;
  switch (targetName) {
    case 'enquiry':
      enquiryChecker();
      break;
    case 'applicant-prefix':
      salutationChecker();
      break;
    case 'email':
      emailChecker();
      break;
    case 'mobile-number':
    case 'office-number':
      contactChecker();
      break;
    default: // Other fields, only need to not be empty
      if (GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS.hasOwnProperty(targetName))
        emptyFieldChecker(targetName);
      break;
  }
}

function submitLeasing() {
  event.preventDefault(); // Maybe right in real life situation

  var emptyFields = Object.keys(GLOBAL.LEASING_FORM_NO_EMPTY_FIELDS);
  var isValid = true;
  isValid &= enquiryChecker();
  isValid &= salutationChecker();
  isValid &= emailChecker();
  isValid &= contactChecker();
  isValid &= agreementChecker();

  for (var i = 0; i < emptyFields.length; i++) {
    isValid &= emptyFieldChecker(emptyFields[i]);
  }

  if (isValid) {
    // TODO: Do something like submit to the server
    $('#submit-success').removeClass('d-none');
  }
}

$('.leasingform').on('focusout', leasingFormChecker);
$('input[name=enquiry]').change(enquiryChecker);
$('select[name=applicant-prefix]').change(salutationChecker);
$('#agreement').on('click', agreementChecker);

// End leasing form handler