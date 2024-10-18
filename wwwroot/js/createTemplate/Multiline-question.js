import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultilineQuestion {
    constructor(label, editionMode) {
        this.label = label ?? "Add a label";
        this.editionMode = editionMode ?? true;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $textarea = document.createElement("textarea");

        $div.className = "mt-4";
        $textarea.classList.add("form-control");
        $label.textContent = this.label;

        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "1";

        $div.appendChild($label);

        //Edition properties
        if(this.editionMode === true) {
            const $btnDeleteQuestion = document.createElement("button");
            $label.contentEditable =  true;
            $btnDeleteQuestion.className = "btn btn-danger ms-3";
            $btnDeleteQuestion.textContent = "delete the question";
            deleteElementOnClick($btnDeleteQuestion, $div);
            $div.appendChild($btnDeleteQuestion);
        }

        $div.appendChild($textarea);
        

        return $div;
    }
}