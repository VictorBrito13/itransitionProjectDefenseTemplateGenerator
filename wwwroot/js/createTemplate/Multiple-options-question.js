import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultipleOptionsQuestion {
    #selectId;

    constructor(label, opts, editionMode, questionId) {
        this.label = label ?? "Add a label";
        this.opts = opts ?? [];
        this.editionMode = editionMode ?? true;
        this.#selectId = crypto.randomUUID();
        this.questionId = questionId;
    }

    getId() {
        return this.#selectId;
    }

    getQuestionHTML() {
        //Question container
        
        const $div = document.createElement("div");
        const $label = document.createElement("label");
        const $btnEditOptions = document.createElement("button");
        const $select = document.createElement("select");

        $select.required = true;

        this.opts.forEach(opt => {
            const $option = document.createElement("option");
            $option.textContent = opt;
            $select.appendChild($option);
        });

        $div.className = "mt-4";
        $select.className = "w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm bg-white";
        $select.dataset["questionId"] = this.questionId;
        $select.id = this.#selectId;
        //label configuration
        $label.textContent = this.label;

        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "4";

        $div.appendChild($label);

        //Edition properties
        if(this.editionMode) {
            const $btnDeleteQuestion = document.createElement("button");
            $label.contentEditable =  true;
            //edit select options button configuration
            $btnEditOptions.textContent = "Edit options";
            $btnEditOptions.type = "button";
            $label.className = "me-3";
            $btnEditOptions.className = "px-3 py-1.5 bg-primary text-white text-sm font-medium rounded-lg hover:bg-primary-600 focus:ring-2 focus:ring-primary transition-colors";
            $btnEditOptions.dataset["bsToggle"] = "modal";
            $btnEditOptions.dataset["bsTarget"] = "#editOptionsModal";

            $btnEditOptions.addEventListener("click", e => {
                document.getElementById("btn-save-options-changes").dataset["selectId"] = this.#selectId;
    
                //Print the existing options in the select into the option list element
                $select.querySelectorAll("option").forEach(opt => {
                    const $div = document.createElement("div");
                    const $h4 = document.createElement("h4");
                    const $btnDeleteOption = document.createElement("button");
    
                    $div.className = "d-flex gap-3 mb-3";
                    $h4.textContent = opt.value;
                    $h4.className = "option";
                    $h4.contentEditable;
                    $btnDeleteOption.type = "button";
                    $btnDeleteOption.className = "px-3 py-1 bg-red-500 text-white text-xs font-medium rounded-lg hover:bg-red-600 transition-colors";
                    $btnDeleteOption.textContent = "Delete";
                    deleteElementOnClick($btnDeleteOption, $div);
    
                    $div.appendChild($h4);
                    $div.appendChild($btnDeleteOption);
                    document.getElementById("options-list").appendChild($div);
                });
            });

                //button to delete the question configuration
            $btnDeleteQuestion.className = "px-3 py-1.5 bg-red-500 text-white text-sm font-medium rounded-lg hover:bg-red-600 focus:ring-2 focus:ring-red-500 transition-colors";
            $btnDeleteQuestion.textContent = "delete the question";
            deleteElementOnClick($btnDeleteQuestion, $div);

            $div.appendChild($btnEditOptions);
            $div.appendChild($btnDeleteQuestion);
        }

        $div.appendChild($select);
        return $div;
    }
}