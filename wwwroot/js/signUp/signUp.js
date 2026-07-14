const $email = document.getElementById("email");
const $username = document.getElementById("username");
const $password = document.getElementById("password");
const $confirmPassword = document.getElementById("confirm-password");
const $errorContainer = document.getElementById("error-container");
const $signUpForm = document.getElementById("signUpForm");
const $btnCreateAccount = document.getElementById("btn-create-account");
const $btnText = document.getElementById("btn-text");
const $btnSpinner = document.getElementById("btn-spinner");

// Field error elements
const $emailError = document.getElementById("email-error");
const $usernameError = document.getElementById("username-error");
const $passwordError = document.getElementById("password-error");
const $confirmError = document.getElementById("confirm-error");

// Password strength elements
const $strengthBars = [
    document.getElementById("strength-bar-1"),
    document.getElementById("strength-bar-2"),
    document.getElementById("strength-bar-3"),
    document.getElementById("strength-bar-4"),
];
const $strengthText = document.getElementById("strength-text");
const $matchIcon = document.getElementById("match-icon");
const $reqLength = document.getElementById("req-length");

// --- Validation state ---
let validationState = {
    email: false,
    username: false,
    password: false,
    confirm: false,
};

// --- Utility functions ---
function showFieldError(input, errorEl, message) {
    input.classList.add("border-red-500");
    input.classList.remove("border-gray-300");
    errorEl.textContent = message;
    errorEl.classList.remove("hidden");
}

function clearFieldError(input, errorEl) {
    input.classList.remove("border-red-500");
    input.classList.add("border-gray-300");
    errorEl.classList.add("hidden");
}

function updateSubmitButton() {
    const allValid = validationState.email && validationState.username && validationState.password && validationState.confirm;
    $btnCreateAccount.disabled = !allValid;
}

function showGlobalError(message) {
    $errorContainer.textContent = message;
    $errorContainer.classList.remove("hidden");
}

function hideGlobalError() {
    $errorContainer.classList.add("hidden");
}

// --- Password strength calculation ---
function getPasswordStrength(password) {
    let score = 0;
    if (password.length >= 8) score++;
    if (password.length >= 12) score++;
    if (/[A-Z]/.test(password) && /[a-z]/.test(password)) score++;
    if (/\d/.test(password)) score++;
    if (/[^A-Za-z0-9]/.test(password)) score++;
    return Math.min(score, 4);
}

function updatePasswordStrength() {
    const strength = getPasswordStrength($password.value);
    const colors = ["bg-red-400", "bg-orange-400", "bg-yellow-400", "bg-green-500"];
    const labels = ["Weak", "Fair", "Good", "Strong"];
    const textColors = ["text-red-600", "text-orange-600", "text-yellow-600", "text-green-600"];

    // Reset all bars
    $strengthBars.forEach((bar) => {
        bar.className = "flex-1 rounded-full transition-colors bg-gray-200";
    });

    if ($password.value.length === 0) {
        $strengthText.textContent = "";
        $strengthText.className = "mt-1.5 text-xs h-4";
    } else {
        // Fill bars up to strength level
        for (let i = 0; i < strength; i++) {
            $strengthBars[i].classList.remove("bg-gray-200");
            $strengthBars[i].classList.add(colors[strength - 1]);
        }
        $strengthText.textContent = labels[strength - 1];
        $strengthText.className = "mt-1.5 text-xs h-4 " + textColors[strength - 1];
    }

    // Update length requirement
    if ($password.value.length >= 8) {
        $reqLength.classList.remove("text-gray-500");
        $reqLength.classList.add("text-green-600");
        $reqLength.querySelector(".req-icon").textContent = "✓";
    } else {
        $reqLength.classList.remove("text-green-600");
        $reqLength.classList.add("text-gray-500");
        $reqLength.querySelector(".req-icon").textContent = "○";
    }
}

// --- Password match indicator ---
function updateMatchIndicator() {
    if ($confirmPassword.value.length === 0) {
        $matchIcon.classList.add("hidden");
        $matchIcon.innerHTML = "";
        clearFieldError($confirmPassword, $confirmError);
        return;
    }

    if ($password.value === $confirmPassword.value) {
        $matchIcon.classList.remove("hidden");
        $matchIcon.innerHTML = '<svg class="w-5 h-5 text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"/></svg>';
        clearFieldError($confirmPassword, $confirmError);
        validationState.confirm = true;
    } else {
        $matchIcon.classList.remove("hidden");
        $matchIcon.innerHTML = '<svg class="w-5 h-5 text-red-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/></svg>';
        showFieldError($confirmPassword, $confirmError, "Passwords do not match");
        validationState.confirm = false;
    }
    updateSubmitButton();
}

// --- Field validators ---
function validateEmail() {
    if (!$email.value) {
        showFieldError($email, $emailError, "Email is required");
        validationState.email = false;
        updateSubmitButton();
        return false;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test($email.value)) {
        showFieldError($email, $emailError, "Please enter a valid email");
        validationState.email = false;
        updateSubmitButton();
        return false;
    }
    clearFieldError($email, $emailError);
    validationState.email = true;
    updateSubmitButton();
    return true;
}

function validateUsername() {
    if (!$username.value) {
        showFieldError($username, $usernameError, "Username is required");
        validationState.username = false;
        updateSubmitButton();
        return false;
    }
    if ($username.value.length < 3) {
        showFieldError($username, $usernameError, "Username must be at least 3 characters");
        validationState.username = false;
        updateSubmitButton();
        return false;
    }
    clearFieldError($username, $usernameError);
    validationState.username = true;
    updateSubmitButton();
    return true;
}

function validatePassword() {
    if (!$password.value) {
        showFieldError($password, $passwordError, "Password is required");
        validationState.password = false;
        updatePasswordStrength();
        updateSubmitButton();
        return false;
    }
    if ($password.value.length < 8) {
        showFieldError($password, $passwordError, "Password must be at least 8 characters");
        validationState.password = false;
        updatePasswordStrength();
        updateSubmitButton();
        return false;
    }
    clearFieldError($password, $passwordError);
    validationState.password = true;
    updatePasswordStrength();
    // Re-check confirm match if confirm has value
    if ($confirmPassword.value) {
        updateMatchIndicator();
    }
    updateSubmitButton();
    return true;
}

// --- Event listeners ---
$email.addEventListener("blur", validateEmail);
$username.addEventListener("blur", validateUsername);
$password.addEventListener("input", function () {
    validatePassword();
    updatePasswordStrength();
    if ($confirmPassword.value) {
        updateMatchIndicator();
    }
});
$confirmPassword.addEventListener("input", updateMatchIndicator);

// --- Form submission ---
$signUpForm.addEventListener("submit", function (e) {
    const emailValid = validateEmail();
    const usernameValid = validateUsername();
    const passwordValid = validatePassword();

    if (!$confirmPassword.value) {
        showFieldError($confirmPassword, $confirmError, "Please confirm your password");
        validationState.confirm = false;
    } else {
        updateMatchIndicator();
    }

    if (!emailValid || !usernameValid || !passwordValid || !validationState.confirm) {
        e.preventDefault();
        return;
    }

    // Show loading state
    $btnCreateAccount.disabled = true;
    $btnText.textContent = "Creating account...";
    $btnSpinner.classList.remove("hidden");
});

// --- Initial state ---
// Server error is displayed via cshtml TempData block
