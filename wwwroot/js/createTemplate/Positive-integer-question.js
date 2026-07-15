import BaseQuestion from "./BaseQuestion.js";

export default class PositiveIntegerQuestion extends BaseQuestion {
    createInput() {
        const $input = document.createElement("input");
        $input.className = "w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm";
        $input.type = "number";
        $input.dataset["questionId"] = this.questionId;
        $input.min = 0;
        $input.required = true;
        return $input;
    }

    getQuestionType() {
        return "2";
    }
}
