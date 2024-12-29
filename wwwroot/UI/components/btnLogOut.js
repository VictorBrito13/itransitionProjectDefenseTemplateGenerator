export default function insertLogOutButton() {
    const $btnLogOutContainer = document.getElementById("btn-log-out-container");
    $btnLogOutContainer.className = "col-3";

    const $a = document.createElement("a");
    
    $a.className = "btn btn-danger";
    $a.href = "/user/log-out";
    $a.innerHTML = `Log out <i class="bi bi-box-arrow-right"></i>`;

    $btnLogOutContainer.appendChild($a);
}