import BaseQuestion from "./BaseQuestion.js";

export default class MultilineQuestion extends BaseQuestion {
    createInput() {
        const $textarea = document.createElement("textarea");
        $textarea.required = true;
        $textarea.classList.add(..."w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all text-sm".split(" "));
        $textarea.dataset["questionId"] = this.questionId;
        return $textarea;
    }

    getQuestionType() {
        return "1";
    }
}
