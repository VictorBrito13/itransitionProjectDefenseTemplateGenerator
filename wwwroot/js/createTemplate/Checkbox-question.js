import BaseQuestion from "./BaseQuestion.js";

export default class CheckboxQuestion extends BaseQuestion {
    createContainer() {
        const $div = super.createContainer();
        $div.classList.add("flex", "gap-3", "items-center");
        return $div;
    }

    createInput() {
        const $input = document.createElement("input");
        $input.type = "checkbox";
        $input.required = true;
        $input.dataset["questionId"] = this.questionId;
        $input.className = "w-5 h-5 text-primary border-gray-300 rounded focus:ring-primary";
        return $input;
    }

    getQuestionType() {
        return "3";
    }
}
