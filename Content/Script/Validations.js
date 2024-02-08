//Remove past dates
const dobInputs = document.querySelectorAll('#datepast');
const currentDate = new Date().toISOString().split('T')[0];

dobInputs.forEach(function (input) {
    input.setAttribute('min', currentDate);
});



//RemoveFuture date
const dobInput = document.getElementById('datefuture');
const currentDate = new Date().toISOString().split('T')[0];
dobInput.setAttribute('max', currentDate);



//onfocusout validations
function validateFirstName() {
    var firstNameInput = document.getElementById('FirstName');
    var firstNameErrorMessage = document.getElementById('firstNameError');

    if (firstNameInput.value.trim() === '') {
        firstNameErrorMessage.textContent = 'First name is required.';
    } else {
        firstNameErrorMessage.textContent = '';
    }
}

function validateLastName() {
    var lastNameInput = document.getElementById('LastName');
    var lastNameErrorMessage = document.getElementById('lastNameError');

    if (lastNameInput.value.trim() === '') {
        lastNameErrorMessage.textContent = 'Last name is required.';
    } else {
        lastNameErrorMessage.textContent = '';
    }
}

function validateDateOfBirth() {
    var dateOfBirthInput = document.getElementById('DateOfBirth');
    var dateOfBirthErrorMessage = document.getElementById('dateOfBirthError');

    if (dateOfBirthInput.value.trim() === '') {
        dateOfBirthErrorMessage.textContent = 'Date of birth is required.';
    } else {
        dateOfBirthErrorMessage.textContent = '';
    }
}

function validatePhoneNumber() {
    var phoneNumberInput = document.getElementById('PhoneNumber');
    var phoneNumberErrorMessage = document.getElementById('phoneNumberError');

    if (phoneNumberInput.value.trim() === '') {
        phoneNumberErrorMessage.textContent = 'Phone number is required.';
    } else {
        phoneNumberErrorMessage.textContent = '';
    }
}

function validateEmailAddress() {
    var emailAddressInput = document.getElementById('EmailAddress');
    var emailAddressErrorMessage = document.getElementById('emailAddressError');

    if (emailAddressInput.value.trim() === '') {
        emailAddressErrorMessage.textContent = 'Email address is required.';
    } else {
        emailAddressErrorMessage.textContent = '';
    }
}

function validateAddress() {
    var addressInput = document.getElementById('Address');
    var addressErrorMessage = document.getElementById('addressError');

    if (addressInput.value.trim() === '') {
        addressErrorMessage.textContent = 'Address is required.';
    } else {
        addressErrorMessage.textContent = '';
    }
}

function validateCountry() {
    var countryInput = document.getElementById('Country');
    var countryErrorMessage = document.getElementById('countryError');

    if (countryInput.value.trim() === '') {
        countryErrorMessage.textContent = 'Country is required.';

    } else {
        countryErrorMessage.textContent = '';
    }
}

function validateState() {
    var stateInput = document.getElementById('State');
    var stateErrorMessage = document.getElementById('stateError');

    if (stateInput.value.trim() === '') {
        stateErrorMessage.textContent = 'State is required.';
    } else {
        stateErrorMessage.textContent = '';
    }
}

function validateCity() {
    var cityInput = document.getElementById('City');
    var cityErrorMessage = document.getElementById('cityError');

    if (cityInput.value.trim() === '') {
        cityErrorMessage.textContent = 'City is required.';
    } else {
        cityErrorMessage.textContent = '';
    }
}

function validatePostcode() {
    var postcodeInput = document.getElementById('Postcode');
    var postcodeErrorMessage = document.getElementById('postcodeError');

    if (postcodeInput.value.trim() === '') {
        postcodeErrorMessage.textContent = 'Postcode is required.';
    } else {
        postcodeErrorMessage.textContent = '';
    }
}

function validatePassportNumber() {
    var passportNumberInput = document.getElementById('PassportNumber');
    var passportNumberErrorMessage = document.getElementById('passportNumberError');

    if (passportNumberInput.value.trim() === '') {
        passportNumberErrorMessage.textContent = 'Passport number is required.';
    } else {
        passportNumberErrorMessage.textContent = '';
    }
}

function validateUsername() {
    var usernameInput = document.getElementById('Username');
    var usernameErrorMessage = document.getElementById('usernameError');

    if (usernameInput.value.trim() === '') {
        usernameErrorMessage.textContent = 'Username is required.';
    } else {
        usernameErrorMessage.textContent = '';
    }
}

function validatePassword() {
    var passwordInput = document.getElementById('Password');
    var passwordErrorMessage = document.getElementById('passwordError');

    if (passwordInput.value.trim() === '') {
        passwordErrorMessage.textContent = 'Password is required.';
    } else {
        passwordErrorMessage.textContent = '';
    }
}

function validateConfirmPassword() {
    var confirmPasswordInput = document.getElementById('ConfirmPassword');
    var confirmPasswordErrorMessage = document.getElementById('confirmPasswordError');
    var passwordInput = document.getElementById('Password');

    if (confirmPasswordInput.value.trim() === '') {
        confirmPasswordErrorMessage.textContent = 'Confirm password is required.';
    } else if (confirmPasswordInput.value !== passwordInput.value) {
        confirmPasswordErrorMessage.textContent = 'Passwords do not match.';
    } else {
        confirmPasswordErrorMessage.textContent = '';
    }
}
