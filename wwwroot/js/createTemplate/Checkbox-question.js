import deleteElementOnClick from "../utils/deleteElement.js";

export default class CheckboxQuestion {
    constructor(label = "Add a label") {
        this.label = label;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $input = document.createElement("input");
        const $btnDeleteQuestion = document.createElement("button");

        $div.className = "mt-4 d-flex gap-3 align-items-center";
        $input.type = "checkbox";
        $input.className = "form-check-input";
        $label.className = "form-check-label";
        $label.textContent = this.label;
        $label.contentEditable =  true;
        $btnDeleteQuestion.className = "btn btn-danger ms-3";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, $div);
        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "3";

        $div.appendChild($label);
        $div.appendChild($input);
        $div.appendChild($btnDeleteQuestion);

        return $div;
    }
}