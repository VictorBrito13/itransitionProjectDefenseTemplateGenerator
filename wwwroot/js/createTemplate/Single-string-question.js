import BaseQuestion from "./BaseQuestion.js";

export default class SingleLineQuestion extends BaseQuestion {
    createInput() {
        const $input = document.createElement("input");
        $input.className = "w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm";
        $input.dataset["questionId"] = this.questionId;
        $input.required = true;
        return $input;
    }

    getQuestionType() {
        return "0";
    }
}
