import deleteElementOnClick from "../utils/deleteElement.js";

export default class CheckboxQuestion {
    constructor(label, editionMode) {
        this.label = label ?? "Add a label";
        this.editionMode = editionMode ?? true;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $input = document.createElement("input");
        
        $div.className = "mt-4 d-flex gap-3 align-items-center";
        $input.type = "checkbox";
        $input.className = "form-check-input";
        $label.className = "form-check-label";
        $label.textContent = this.label;

        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "3";

        $div.appendChild($label);
        //Edition properties
        if(this.editionMode) {
            const $btnDeleteQuestion = document.createElement("button");
            $label.contentEditable =  true;
            $btnDeleteQuestion.className = "btn btn-danger ms-3";
            $btnDeleteQuestion.textContent = "delete the question";
            deleteElementOnClick($btnDeleteQuestion, $div);
            $div.appendChild($btnDeleteQuestion);
        }
        $div.appendChild($input);

        return $div;
    }
}