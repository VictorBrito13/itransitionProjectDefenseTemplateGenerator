import SingleLineQuestion from "./Single-string-question.js";
import MultilineQuestion from "./Multiline-question.js";
import PositiveIntegerQuestion from "./Positive-integer-question.js";
import CheckboxQuestion from "./Checkbox-question.js";
import MultipleOptionsQuestion from "./Multiple-options-question.js";

//Questions container
const $questionsContainer = document.getElementById("template-questions");
//Buttons to add questions
const $singleLineQuestionBtn = document.getElementById("q-sl"),
$multiLineQuestionBtn = document.getElementById("q-ml"),
$positiveIntegerQuestionBtn = document.getElementById("q-pi"),
$checkboxQuestionBtn = document.getElementById("q-cb"),
$multipleOptionsQuestionBtn = document.getElementById("q-mo");

$singleLineQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new SingleLineQuestion().getQuestionHTML());
});
$multiLineQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new MultilineQuestion().getQuestionHTML())
});
$positiveIntegerQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new PositiveIntegerQuestion().getQuestionHTML())
});
$checkboxQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new CheckboxQuestion().getQuestionHTML())
});
$multipleOptionsQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new MultipleOptionsQuestion().getQuestionHTML())
});

//Add options to the select
const $btnSaveOptionsChanges = document.getElementById("btn-save-options-changes");
const $btnCloseOptionsChanges = document.getElementById("btn-close-options-changes");
const $optionsList = document.getElementById("options-list");
const $textAreaAddOpts = document.getElementById("textAreaAddOpts");

$btnSaveOptionsChanges.addEventListener("click", e => {
    const $select = document.getElementById($btnSaveOptionsChanges.dataset["selectId"]);
    $select.innerHTML = "";

    //This variable contains all the h4 in the option list element, dont mix it up with 'option' html element
    const $previousOptions = $optionsList.querySelectorAll(".option");
    const newOpts = $textAreaAddOpts.value.split("\n").filter(opt => opt);//this filter ensure that there is no null options
    const prevOpts = Array.from($previousOptions).map($el => $el.textContent);
    const opts = [...prevOpts, ...newOpts]

    opts.forEach(opt => {
        const $option = document.createElement("option");
        $option.value = opt;
        $option.textContent = opt;
        
        $select.appendChild($option);
    });

    $textAreaAddOpts.value = "";
    $optionsList.innerHTML = null;
});

$btnCloseOptionsChanges.addEventListener("click", e => {
    $textAreaAddOpts.value = "";
    $optionsList.innerHTML = null;
})
