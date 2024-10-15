import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultipleOptionsQuestion {
    #selectId;

    constructor(label = "Add a label", opts = []) {
        this.label = label;
        this.opts = opts;
        this.#selectId = crypto.randomUUID();
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
        const $btnDeleteQuestion = document.createElement("button");

        this.opts.forEach(opt => {
            const $option = document.createElement("option");
            $option.textContent = opt;
            $select.appendChild($option);
        });

        $div.className = "mt-4";
        $select.className = "form-select";
        $select.id = this.#selectId;
        //label configuration
        $label.textContent = this.label;
        $label.contentEditable =  true;
        //edit select options button configuration
        $btnEditOptions.textContent = "Edit options";
        $btnEditOptions.type = "button";
        $btnEditOptions.className = "btn btn-primary ms-3";
        $btnEditOptions.dataset["bsToggle"] = "modal";
        $btnEditOptions.dataset["bsTarget"] = "#editOptionsModal";
        //QuestionType defined for the database
        $div.dataset["QuestionType"] = "4";

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
                $btnDeleteOption.className = "btn btn-danger";
                $btnDeleteOption.textContent = "Delete";
                deleteElementOnClick($btnDeleteOption, $div);

                $div.appendChild($h4);
                $div.appendChild($btnDeleteOption);
                document.getElementById("options-list").appendChild($div);
            });
        });

        //button to delete the question configuration
        $btnDeleteQuestion.className = "btn btn-danger ms-3";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, $div);

        $div.appendChild($label);
        $div.appendChild($btnEditOptions);
        $div.appendChild($btnDeleteQuestion);
        $div.appendChild($select);
        return $div;
    }
}