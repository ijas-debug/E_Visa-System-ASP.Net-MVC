///checkstatus Email id validation
function validateEmail() {
    var emailField = document.getElementById('EmailID');
    var email = emailField.value.trim();

    if (!email.includes('@')) {
        emailField.setCustomValidity('Email address must contain @ symbol.');
    } else {
        emailField.setCustomValidity('');
    }
}

///Passport number validations
function validatePassportNumber() {
    var passportField = document.getElementById('PassportNumber');
    var passportNumber = passportField.value.trim();

    if (!/^\d+$/.test(passportNumber)) {
        passportField.setCustomValidity('Passport number must only contain numbers.');
    } else {
        passportField.setCustomValidity('');
    }
}
