import deleteElementOnClick from "../utils/deleteElement.js";

export default class PositiveIntegerQuestion {
    constructor(label = "Add a label") {
        this.label = label;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $input = document.createElement("input");
        const $btnDeleteQuestion = document.createElement("button");

        $div.className = "mt-4";
        $input.className = "form-control";
        $input.type = "number"
        $input.min = 0;
        $label.textContent = this.label;
        $label.contentEditable =  true;
        $btnDeleteQuestion.className = "btn btn-danger ms-3";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, $div);
        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "2";

        $div.appendChild($label);
        $div.appendChild($btnDeleteQuestion);
        $div.appendChild($input);

        return $div;
    }
}