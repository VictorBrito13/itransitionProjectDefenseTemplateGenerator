import { buildForm } from "../utils/buildForm.js";

const $form = document.getElementById("form-questions");
const $formTitle = document.getElementById("form-title");
const $formDescription = document.getElementById("form-description");

buildForm($form, (json) => {
    $formTitle.textContent = json.Title;
    $formDescription.textContent = json.Description;
}, false);
