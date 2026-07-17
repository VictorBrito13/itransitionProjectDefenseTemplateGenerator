import deleteElementOnClick from "../utils/deleteElement.js";

/**
 * Base class for all question types in the template builder.
 * Encapsulates shared logic: container creation, label creation,
 * edition controls (contentEditable label + delete button).
 * Subclasses override createInput() to provide the specific input element.
 */
export default class BaseQuestion {
    constructor(label = "Add a label", editionMode = true, questionId) {
        this.label = label;
        this.editionMode = editionMode;
        this.questionId = questionId;
    }

    /**
     * Creates the question container div with base styling.
     * @returns {HTMLDivElement}
     */
    createContainer() {
        const $div = document.createElement("div");
        $div.className = "mt-4";
        return $div;
    }

    /**
     * Creates the label element with the question's text.
     * @returns {HTMLLabelElement}
     */
    createLabel() {
        const $label = document.createElement("label");
        $label.textContent = this.label;
        return $label;
    }

    /**
     * Adds edition controls to the container: contentEditable label,
     * delete button wired to remove the container.
     * @param {HTMLElement} container
     * @param {HTMLLabelElement} label
     */
    addEditionControls(container, label) {
        const $btnDeleteQuestion = document.createElement("button");
        label.contentEditable = true;
        label.className = "me-3";
        $btnDeleteQuestion.dataset.cy = "delete-question-btn";
        $btnDeleteQuestion.className = "px-3 py-1.5 bg-red-500 text-white text-sm font-medium rounded-lg hover:bg-red-600 focus:ring-2 focus:ring-red-500 transition-colors";
        $btnDeleteQuestion.textContent = "delete the question";
        deleteElementOnClick($btnDeleteQuestion, container);
        container.appendChild($btnDeleteQuestion);
    }

    /**
     * Creates the specific input element for this question type.
     * Must be overridden by subclasses.
     * @returns {HTMLElement}
     */
    createInput() {
        throw new Error("createInput() must be overridden by subclass");
    }

    /**
     * Orchestrates the full question HTML assembly:
     * container → label → input → edition controls (if editionMode) → return container.
     * @returns {HTMLDivElement}
     */
    getQuestionHTML() {
        const $container = this.createContainer();
        const $label = this.createLabel();
        const $input = this.createInput();

        $container.dataset["QuestionType"] = this.getQuestionType();

        $container.appendChild($label);

        if (this.editionMode) {
            this.addEditionControls($container, $label);
        }

        $container.appendChild($input);

        return $container;
    }

    /**
     * Returns the QuestionType numeric string for the database.
     * Must be overridden by subclasses.
     * @returns {string}
     */
    getQuestionType() {
        throw new Error("getQuestionType() must be overridden by subclass");
    }
}
