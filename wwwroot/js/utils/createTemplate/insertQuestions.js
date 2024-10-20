import CheckboxQuestion from "../../createTemplate/Checkbox-question.js";
import MultilineQuestion from "../../createTemplate/Multiline-question.js";
import MultipleOptionsQuestion from "../../createTemplate/Multiple-options-question.js";
import PositiveIntegerQuestion from "../../createTemplate/Positive-integer-question.js";
import SingleLineQuestion from "../../createTemplate/Single-string-question.js";

export default function insertQuestions() {
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
}