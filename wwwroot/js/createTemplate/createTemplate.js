import SingleLineQuestion from "./Single-string-question.js";
import MultilineQuestion from "./Multiline-question.js";
import PositiveIntegerQuestion from "./Positive-integer-question.js";
import CheckboxQuestion from "./Checkbox-question.js";
import MultipleOptionsQuestion from "./Multiple-options-question.js";
import updateTemplateHTML from "../UpdateTemplate/UpdateTemplate.js";

//Questions container
const $questionsContainer = document.getElementById("template-questions");
//Buttons to add questions
const $singleLineQuestionBtn = document.getElementById("q-sl"),
$multiLineQuestionBtn = document.getElementById("q-ml"),
$positiveIntegerQuestionBtn = document.getElementById("q-pi"),
$checkboxQuestionBtn = document.getElementById("q-cb"),
$multipleOptionsQuestionBtn = document.getElementById("q-mo");

$singleLineQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new SingleLineQuestion().getQuestionHTML());
});
$multiLineQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new MultilineQuestion().getQuestionHTML())
});
$positiveIntegerQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new PositiveIntegerQuestion().getQuestionHTML())
});
$checkboxQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new CheckboxQuestion().getQuestionHTML())
});
$multipleOptionsQuestionBtn.addEventListener("click", e => {
    $questionsContainer.appendChild(new MultipleOptionsQuestion().getQuestionHTML())
});

//Add options to the select
const $btnSaveOptionsChanges = document.getElementById("btn-save-options-changes");
const $btnCloseOptionsChanges = document.getElementById("btn-close-options-changes");
const $optionsList = document.getElementById("options-list");
const $textAreaAddOpts = document.getElementById("textAreaAddOpts");


$btnSaveOptionsChanges.addEventListener("click", e => {
    const $select = document.getElementById($btnSaveOptionsChanges.dataset["selectId"]);
    $select.innerHTML = "";

    //This variable contains all the h4 in the option list element, dont mix it up with 'option' html element
    const $previousOptions = $optionsList.querySelectorAll(".option");
    const newOpts = $textAreaAddOpts.value.split("\n").filter(opt => opt);//this filter ensure that there are not null options
    const prevOpts = Array.from($previousOptions).map($el => $el.textContent);
    const opts = [...prevOpts, ...newOpts]

    opts.forEach(opt => {
        const $option = document.createElement("option");
        $option.value = opt;
        $option.textContent = opt;
        
        $select.appendChild($option);
    });

    $textAreaAddOpts.value = "";
    $optionsList.innerHTML = null;
});

$btnCloseOptionsChanges.addEventListener("click", e => {
    $textAreaAddOpts.value = "";
    $optionsList.innerHTML = null;
})

//---------------------------------------------------Update template functions
const urlParamsSearcher = new URLSearchParams(location.search);

if(urlParamsSearcher.get("templateId")) {
    updateTemplateHTML()
}

//Questions for the database
const $btnCreateTemplate = document.getElementById("btn-create-template");

$btnCreateTemplate.addEventListener("click", async e => {

    const questions = [];
    const $questions = Array.from($questionsContainer.children).splice(2);
    let questionOptions = [];

    //Create the questions based on the html elements
    //Create the options for questions if is necessary
    $questions.forEach($question => {
        // const ID = Math.round(Math.random() * 10_000);

        //This means the question is of type multioption
        if(parseInt($question.dataset["QuestionType"]) == 4) {
            const $select = $question.querySelector("select");
            const $options = $select.querySelectorAll("option");

            $options.forEach($option => {
                questionOptions.push({ option: $option.value });
            });
        }

        const question = {
            //questionId: ID,
            questionString: $question.querySelector("label").textContent,
            // templateId: isTemplateSavedJSON.templateId,
            questionType: parseInt($question.dataset["QuestionType"]),
            questionOptions: [...questionOptions]
        }
        console.log(question);
        
        questionOptions = [];
        console.log(questionOptions);
        
        questions.push(question);
    }); 

    //Template creation
    const templateConfig = {
        title: document.getElementById("setting-template-title").textContent.trim(),
        description: document.getElementById("setting-template-description").textContent.trim(),
        image_url: document.getElementById("setting-template-image").value.trim() || "default.png",
        topicId: document.getElementById("setting-template-topic").value.trim(),
        isPublic: parseInt(document.getElementById("template-visibility").value) === 1,
        questions
    };

    //Create or update a new teplate
    if(urlParamsSearcher.get("templateId")) {
        //Petition to update a template
        const isTemplateUpdated = await fetch(`${location.origin}/template/update?templateId=${urlParamsSearcher.get("templateId")}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(templateConfig)
        });
        const isTemplateUpdatedJSON = await isTemplateUpdated.json();
        console.log("Trying to update a template.....");
        console.log(isTemplateUpdatedJSON);

    } else {
        //Petition to save the template
        const isTemplateSaved = await fetch(`${location.origin}/template/create`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(templateConfig)
        });
        const isTemplateSavedJSON = await isTemplateSaved.json();
        
        console.log("Trying to create a template.....");
        
        console.log(isTemplateSavedJSON);
    }
});
