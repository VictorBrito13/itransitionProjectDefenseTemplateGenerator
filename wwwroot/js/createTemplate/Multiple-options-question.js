import BaseQuestion from "./BaseQuestion.js";
import deleteElementOnClick from "../utils/deleteElement.js";

export default class MultipleOptionsQuestion extends BaseQuestion {
    #selectId;

    constructor(label = "Add a label", opts = [], editionMode = true, questionId) {
        super(label, editionMode, questionId);
        this.opts = opts;
        this.#selectId = crypto.randomUUID();
    }

    getId() {
        return this.#selectId;
    }

    createInput() {
        const $select = document.createElement("select");
        $select.required = true;
        $select.className = "w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm bg-white";
        $select.dataset["questionId"] = this.questionId;
        $select.id = this.#selectId;

        this.opts.forEach(opt => {
            const $option = document.createElement("option");
            $option.textContent = opt;
            $select.appendChild($option);
        });

        return $select;
    }

    /**
     * Override to add the unique "Edit options" button alongside the standard
     * delete button. The edit button opens a modal for managing select options.
     */
    addEditionControls(container, label) {
        const $btnDeleteQuestion = document.createElement("button");
        const $btnEditOptions = document.createElement("button");

        label.contentEditable = true;
        label.className = "me-3";

        // Edit options button
        $btnEditOptions.textContent = "Edit options";
        $btnEditOptions.type = "button";
        $btnEditOptions.className = "px-3 py-1.5 bg-primary text-white text-sm font-medium rounded-lg hover:bg-primary-600 focus:ring-2 focus:ring-primary transition-colors";
        $btnEditOptions.dataset["bsToggle"] = "modal";
        $btnEditOptions.dataset["bsTarget"] = "#editOptionsModal";

        const $select = container.querySelector("select");

        $btnEditOptions.addEventListener("click", e => {
            document.getElementById("btn-save-options-changes").dataset["selectId"] = this.#selectId;

            // Print the existing options in the select into the option list element
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

        // Delete question button
        $btnDeleteQuestion.className = "px-3 py-1.5 bg-red-500 text-white text-sm font-medium rounded-lg hover:bg-red-600 focus:ring-2 focus:ring-red-500 transition-colors";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, container);

        container.appendChild($btnEditOptions);
        container.appendChild($btnDeleteQuestion);
    }

    getQuestionType() {
        return "4";
    }
}
