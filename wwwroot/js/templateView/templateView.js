import CheckboxQuestion from "../createTemplate/Checkbox-question.js";
import MultilineQuestion from "../createTemplate/Multiline-question.js";
import MultipleOptionsQuestion from "../createTemplate/Multiple-options-question.js";
import PositiveIntegerQuestion from "../createTemplate/Positive-integer-question.js";
import SingleLineQuestion from "../createTemplate/Single-string-question.js";

const $form = document.getElementById("form-questions");

async function getTemplate() {
    const searcherParams = new URLSearchParams(location.search);
    const templateId = searcherParams.get("templateId");
    
    const template = await (await fetch(`${location.origin}/template/get-template?templateId=${templateId}`)).json();
    return template;
}

async function printTemplate() {
    const json = await getTemplate();
    const $form = document.getElementById("form-questions");

    if(json.errorMsg) {
        $form.innerHTML = "<h1>This form does not exists</h1>";
    } else {
        const $formTitle = document.getElementById("form-title");
        const $formDescription = document.getElementById("form-description");

        $formTitle.textContent = json.Title;
        $formDescription.textContent = json.Description;

        //Print the questions
        const questions = json.Questions;

        questions.forEach(question => {
            let formControl = null;

            //Single line string
            if(question.QuestionType === 0) {
                formControl = new SingleLineQuestion(question.QuestionString, false);
            //Multiline-question
            } else if(question.QuestionType === 1) {
                formControl = new MultilineQuestion(question.QuestionString, false);
            //Positive integer
            } else if(question.QuestionType === 2) {
                formControl = new PositiveIntegerQuestion(question.QuestionString, false);
            //Checkbox
            } else if(question.QuestionType === 3) {
                formControl = new CheckboxQuestion(question.QuestionString, false);
            //Multiple options
            } else if(question.QuestionType === 4) {
                const opts = question.QuestionOptions.map(option => option.Option)
                formControl = new MultipleOptionsQuestion(question.QuestionString, opts, false);
            }
            $form.appendChild(formControl.getQuestionHTML())
        });

        const $btnSubmit = document.createElement("button");
        $btnSubmit.type = "submit";
        $btnSubmit.className = "btn btn-success"
        $btnSubmit.textContent = "Send";
        $form.appendChild($btnSubmit);

    }

    console.log(json);
    
}

printTemplate();
