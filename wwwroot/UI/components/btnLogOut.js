export default function insertLogOutButton() {
    const $btnLogOutContainer = document.getElementById("btn-log-out-container");
    // $btnLogOutContainer.className = "col-3";

    const $a = document.createElement("a");
    
    $a.className = "inline-flex items-center gap-2 px-4 py-2 bg-red-500 text-white text-sm font-medium rounded-lg hover:bg-red-600 focus:ring-2 focus:ring-red-500 transition-colors";
    $a.href = "/user/log-out";
    $a.innerHTML = 'Log out <svg class="w-4 h-4 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/></svg>';

    $btnLogOutContainer.appendChild($a);
}