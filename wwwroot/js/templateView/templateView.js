import { buildForm, getTemplate } from "../utils/buildForm.js";

const $form = document.getElementById("form-questions");
const $formTitle = document.getElementById("form-title");
const $formDescription = document.getElementById("form-description");

const template = await getTemplate();

buildForm($form, template, false);

$formTitle.textContent = template.Title;
$formDescription.textContent = template.Description;

//Default value for the date intput
const $formControlDate = document.getElementById("form-date");
const now = new Date();
console.log(now.toISOString().slice(0, 19));

const formattedDateTime = now.toISOString().slice(0, 19);

$formControlDate.value = formattedDateTime;
