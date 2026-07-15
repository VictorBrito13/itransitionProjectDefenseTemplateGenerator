import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultilineQuestion {
    constructor(label, editionMode, questionId) {
        this.label = label ?? "Add a label";
        this.editionMode = editionMode ?? true;
        this.questionId = questionId;
    }

    getQuestionHTML() {
        //Question container
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $textarea = document.createElement("textarea");

        $textarea.required = true;

        $div.className = "mt-4";
        $textarea.classList.add(..."w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm".split(" "));
        $textarea.dataset["questionId"] = this.questionId;
        $label.textContent = this.label;

        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "1";

        $div.appendChild($label);

        //Edition properties
        if(this.editionMode === true) {
            const $btnDeleteQuestion = document.createElement("button");
            $label.contentEditable =  true;
            $label.className = "me-3";
            $btnDeleteQuestion.className = "px-3 py-1.5 bg-red-500 text-white text-sm font-medium rounded-lg hover:bg-red-600 focus:ring-2 focus:ring-red-500 transition-colors";
            $btnDeleteQuestion.textContent = "delete the question";
            deleteElementOnClick($btnDeleteQuestion, $div);
            $div.appendChild($btnDeleteQuestion);
        }

        $div.appendChild($textarea);
        

        return $div;
    }
}