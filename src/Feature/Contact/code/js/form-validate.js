// Wait for the DOM to be ready
$(function() {
  // Initialize form validation on the registration form.
  
  // It has the name attribute "registration"
  $("form.contactform").validate({
    // Specify validation rules
    rules: {
      // The key name on the left side is the name attribute
      // of an input field. Validation rules are defined
      // on the right side
      enquirytype: "required",
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
      space: "required"
    },
    // Specify validation error messages
    messages: {
      enquirytype: "Please select an enquiry type",
      name: "Please enter your name",
      email: "Please enter a valid email address",
      contactno: "Please enter a valid contact number",
      message: "Please enter your message",
      space: "Please select an option"
    },
    // Make sure the form is submitted to the destination defined
    // in the "action" attribute of the form when valid
    submitHandler: function(form) {
      form.submit();
    }
  });
});

$('#fileupload').bind('change', function() {

  var fileSize = this.files[0].size;
  if (fileSize > 5000000) {
    $('.file-upload .error').html('File size too big. Please ensure your file is 5mb and below.');
    $('#fileupload').css('border','1px solid red');
    $('#fileupload').css('padding','10px');
  }

});
