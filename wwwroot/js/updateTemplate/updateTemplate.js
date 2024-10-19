import { buildForm } from "../utils/buildForm.js";

const $pageTitle = document.getElementById("page-title");
const $btnCreateTemplate = document.getElementById("btn-create-template");

export default function updateTemplateHTML() {
    $pageTitle.textContent = "Edit the template";
    $btnCreateTemplate.textContent = "Edit template";
    const $formTemplate = document.getElementById("template-questions")

    buildForm($formTemplate, undefined, true)
}

export { updateTemplateHTML }