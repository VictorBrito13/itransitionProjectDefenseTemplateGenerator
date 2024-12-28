export default function insertLogOutButton() {
    const $btnLogOutContainer = document.getElementById("btn-log-out-container");
    $btnLogOutContainer.className = "col-3";

    const $a = document.createElement("a");
    
    $a.className = "btn btn-danger";
    $a.href = "/user/log-out";
    $a.textContent = "Log out";

    $btnLogOutContainer.appendChild($a);
}