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

//This function is used to display a form
//if editionMode is true the questions within will have configuration elements, in this mode the send button will not be displayed
//if editionMode is false when send button gets clicked is goint to send the from
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
                formControl = new SingleLineQuestion(question.QuestionString, editionMode, question.QuestionId);
            //Multiline-question
            } else if(question.QuestionType === 1) {
                formControl = new MultilineQuestion(question.QuestionString, editionMode, question.QuestionId);
            //Positive integer
            } else if(question.QuestionType === 2) {
                formControl = new PositiveIntegerQuestion(question.QuestionString, editionMode, question.QuestionId);
            //Checkbox
            } else if(question.QuestionType === 3) {
                formControl = new CheckboxQuestion(question.QuestionString, editionMode, question.QuestionId);
            //Multiple options
            } else if(question.QuestionType === 4) {
                const opts = question.QuestionOptions.map(option => option.Option)
                formControl = new MultipleOptionsQuestion(question.QuestionString, opts, editionMode, question.QuestionId);
            }
            $parentElement.appendChild(formControl.getQuestionHTML())
        });

        if(!editionMode) {
            const $btnSubmit = document.createElement("button");
            $btnSubmit.type = "submit";
            $btnSubmit.className = "btn btn-success"
            $btnSubmit.textContent = "Send";

            $btnSubmit.addEventListener("click", async e => {
                e.preventDefault();
                const date = document.getElementById("form-date").value;
                const userId = document.getElementById("form-user-email").dataset["userid"];
                //Build responses objects
                const responses = [];

                const $questions = Array.from($parentElement.querySelectorAll("div")).slice(2) || [];

                console.log($questions);
                

                $questions.forEach($question => {   
                    
                    const responseString =
                    $question.querySelector("input")?.value ||
                    $question.querySelector("textarea")?.value ||
                    $question.querySelector("select")?.value || "";

                    const questionId =
                    $question.querySelector("input")?.dataset["questionId"] ||
                    $question.querySelector("textarea")?.dataset["questionId"] ||
                    $question.querySelector("select")?.dataset["questionId"] || "";
                    
                    responses.push({
                        Date: date.replace("T", " "),
                        ResponseString: responseString,
                        UserId: userId,
                        QuestionId: questionId,
                    });
                });

                console.log(responses);

                const responsesRes = await fetch(`${location.origin}/response/add`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(responses)
                });
                console.log(responsesRes);
            });
            $parentElement.appendChild($btnSubmit);
        }
    }
    
}

export { getTemplate, buildForm }