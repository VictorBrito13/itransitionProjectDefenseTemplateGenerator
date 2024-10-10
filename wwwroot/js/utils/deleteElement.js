export default function deleteElementOnClick(btn, element) {
    btn.addEventListener("click", e => {
        element.remove();
    });
}