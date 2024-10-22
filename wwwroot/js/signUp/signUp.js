const $password = document.getElementById("password");
const $confirmPassword = document.getElementById("confirm-password");
const $errors = document.getElementById("errors");
const $signUpForm = document.getElementById("signUpForm");
const $btnCreateAccount = document.getElementById("btn-create-account");

$btnCreateAccount.disabled = true;

$password.addEventListener("input", e => {
    if($password.value !== $confirmPassword.value) {
        $errors.textContent = "The passwords does not match";
        $errors.style.display = "block";
        $btnCreateAccount.disabled = true;
    } else {
        $errors.style.display = "none";
        $btnCreateAccount.disabled = false;
    }
});

$confirmPassword.addEventListener("input", e => {
    if($password.value !== $confirmPassword.value) {
        $errors.textContent = "The passwords does not match";
        $errors.style.display = "block";
        $btnCreateAccount.disabled = true;
    } else {
        $errors.style.display = "none";
        $btnCreateAccount.disabled = false;
    }
});