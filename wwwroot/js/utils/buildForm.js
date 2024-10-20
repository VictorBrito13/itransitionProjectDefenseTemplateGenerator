import CheckboxQuestion from "../createTemplate/Checkbox-question.js";
import MultilineQuestion from "../createTemplate/Multiline-question.js";
import MultipleOptionsQuestion from "../createTemplate/Multiple-options-question.js";
import PositiveIntegerQuestion from "../createTemplate/Positive-integer-question.js";
import SingleLineQuestion from "../createTemplate/Single-string-question.js";


async function getTemplate() {
    const searcherParams = new URLSearchParams(location.search);
    const templateId = searcherParams.get("templateId");
    
    const template = await (await fetch(`${location.origin}/template/get-template?templateId=${templateId}`)).json();
    return template;
}

async function buildForm($parentElement, json, editionMode = false) {

    if(json.errorMsg) {
        $parentElement.innerHTML = "<h1>This form does not exists</h1>";
    } else {
        //Print the questions
        const questions = json.Questions;
        questions.forEach(question => {
            let formControl = null;

            //Single line string
            if(question.QuestionType === 0) {
                formControl = new SingleLineQuestion(question.QuestionString, editionMode);
            //Multiline-question
            } else if(question.QuestionType === 1) {
                formControl = new MultilineQuestion(question.QuestionString, editionMode);
            //Positive integer
            } else if(question.QuestionType === 2) {
                formControl = new PositiveIntegerQuestion(question.QuestionString, editionMode);
            //Checkbox
            } else if(question.QuestionType === 3) {
                formControl = new CheckboxQuestion(question.QuestionString, editionMode);
            //Multiple options
            } else if(question.QuestionType === 4) {
                const opts = question.QuestionOptions.map(option => option.Option)
                formControl = new MultipleOptionsQuestion(question.QuestionString, opts, editionMode);
            }
            $parentElement.appendChild(formControl.getQuestionHTML())
        });

        if(!editionMode) {
            const $btnSubmit = document.createElement("button");
            $btnSubmit.type = "submit";
            $btnSubmit.className = "btn btn-success"
            $btnSubmit.textContent = "Send";
            $parentElement.appendChild($btnSubmit);
        }
    }
    
}

export { getTemplate, buildForm }