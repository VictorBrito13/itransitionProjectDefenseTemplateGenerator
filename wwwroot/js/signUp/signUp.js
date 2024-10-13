const $password = document.getElementById("password");
const $confirmPassword = document.getElementById("confirm-password");
const $errors = document.getElementById("errors");
const $signUpForm = document.getElementById("signUpForm");

$password.addEventListener("input", e => {
    if($password.value !== $confirmPassword.value) {
        $errors.textContent = "The passwords does not match";
        $errors.style.display = "block";
    } else {
        $errors.style.display = "none";
    }
});

$confirmPassword.addEventListener("input", e => {
    if($password.value !== $confirmPassword.value) {
        $errors.textContent = "The passwords does not match";
        $errors.style.display = "block";
    } else {
        $errors.style.display = "none";
    }
});