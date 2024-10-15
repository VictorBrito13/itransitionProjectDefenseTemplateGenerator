import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultilineQuestion {
    constructor(label = "Add a label") {
        this.label = label;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $textarea = document.createElement("textarea");
        const $btnDeleteQuestion = document.createElement("button");

        $div.className = "mt-4";
        $textarea.classList.add("form-control");
        $label.textContent = this.label;
        $label.contentEditable =  true;
        $btnDeleteQuestion.className = "btn btn-danger ms-3";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, $div);
        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "1";

        $div.appendChild($label);
        $div.appendChild($btnDeleteQuestion);
        $div.appendChild($textarea);

        return $div;
    }
}