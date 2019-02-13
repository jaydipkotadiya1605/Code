// Wait for the DOM to be ready
$(function () {
    var classNames = ["general-enquiry", "leasing-enquiry"];
    if ($('#enquirytypeselected').val() !== null)
    {
        var className = $('#enquirytypeselected').val().toLowerCase().replace(" ", "-");
        classNames.forEach(function (element) {
            if (element === className) {
                $('section.' + element).css("display", "block");
            } else {
                $('section.' + element).css("display", "none");
            }
        });
    }

    $('#enquirytypeselected').change(function (e) {
        e.preventDefault();
        var className = $(this).val().toLowerCase().replace(" ", "-");
        classNames.forEach(function (element) {
            if (element === className) {
                $('section.' + element).css("display", "block");
            } else {
                $('section.' + element).css("display", "none");
            }
        });
    });
  // Initialize form validation on the registration form.
  // It has the name attribute "registration"
  $("form.contactform").validate({
    // Specify validation rules
    rules: {
      // The key name on the left side is the name attribute
      // of an input field. Validation rules are defined
      // on the right side
        enquirytypeselected: {
            required: true
        },
      name: "required",
      email: {
        required: true,
        // Specify that email should be validated
        // by the built-in "email" rule
        email: true
      },
      contactno: {
          required: true,
          number: true
      },
      message: "required",
      enquiryspaceselected: "required"
    },
    // Specify validation error messages
    messages: {
      enquirytypeselected: "Please select an enquiry type",
      name: "Please enter your name",
      email: {
          required: "Please enter your email address",
          email: "Please enter a valid email address"
      },
      contactno: {
          required: "Please enter a valid contact number"
      },
      message: "Please enter your message",
      enquiryspaceselected: "Please select an option"
    },
    // Make sure the form is submitted to the destination defined
    // in the "action" attribute of the form when valid
    submitHandler: function(form) {
      form.submit();
    }
  });
});