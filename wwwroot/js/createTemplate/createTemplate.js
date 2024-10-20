import throttle from "../utils/throttle.js";
import { getTemplate } from "../utils/buildForm.js";
import { buildForm } from "../utils/buildForm.js";
import insertQuestions from "../utils/createTemplate/insertQuestions.js";
import saveSelectOptions from "../utils/createTemplate/saveSelectOptions.js";

insertQuestions();
saveSelectOptions();

//---------------------------------------------------Update template functions
const urlParamsSearcher = new URLSearchParams(location.search);
let admins = [];
let usersAllowedToAnswer = []

if(urlParamsSearcher.get("templateId")) {
    const template = await getTemplate();
    admins = template.Admins || [];
    usersAllowedToAnswer = template.usersAllowedToAnswer || [];
    const $btnSaveUser = document.getElementById("btn-save-user");
    console.log(template);

    const $pageTitle = document.getElementById("page-title");
    const $btnCreateTemplate = document.getElementById("btn-create-template");
    $pageTitle.textContent = "Edit the template";
    $btnCreateTemplate.textContent = "Edit template";
    const $formTemplate = document.getElementById("template-questions");
    const $usersContainer = document.getElementById("users-container");

    $btnSaveUser.addEventListener("click", e => {
        const $users = $usersContainer.querySelectorAll("p");

        admins = []
        $users.forEach($user => {
            admins.push(
            {
                UserId: $user.dataset["userid"],
                TemplateId : template.TemplateId,
            });
        });

        console.log(admins);
    });

    buildForm($formTemplate, template, true);

    //template's visibility switcher
    const $visbilitySelect = document.getElementById("template-visibility-switch");
    const $btnAddAllowedUsers = document.getElementById("btn-add-allowed-users");

    $visbilitySelect.addEventListener("change", e => {
        const currentState = parseInt($visbilitySelect.value);

        //It means it is public
        if(currentState === 1) {
            $btnAddAllowedUsers.style.display = "none";        
        } else {
            $btnAddAllowedUsers.style.display = "inline-block";
        }
    });

    //Change the modal's title for add admins or users that are able to answer the form
    const $modal_Add_Admins_Or_Users_Allowed_To_Answer = document.getElementById("admin_allowed-users");

    $modal_Add_Admins_Or_Users_Allowed_To_Answer.addEventListener("show.bs.modal", e => {
        $usersContainer.innerHTML = null;
        const $trigger = e.relatedTarget;
        const title = $trigger.dataset["bsTitle"];

        //Insert the admins or user allowed to answer the form
        if($trigger.id === "btn-add-admin") {
            console.log("admin");
            let adminsHmtl = ``;
            admins.forEach(admin => {
                const { User } = admin;
                adminsHmtl += `<p data-userid="${User.UserId}">${User.Email}</p>`;
                console.log(User);
            });
            $usersContainer.innerHTML = adminsHmtl;
        } else if($trigger.id === "btn-add-allowed-users") {
            console.log("users");
            let adminsHmtl = ``;
            usersAllowedToAnswer.forEach(userAllowedToAnswer => {
                const { User } = userAllowedToAnswer;
                adminsHmtl += `<p data-userid="${User.UserId}">${User.Email}</p>`;
                console.log(User);
            });
            $usersContainer.innerHTML = adminsHmtl;
        }

        document.getElementById("modal-admins-users-allowed-title").textContent = title
    });

    //Search a user by his username
    const $formControlSearchUser = document.getElementById("form-control-search-user");
    const $searchResult = document.getElementById("search-result");

    const getUserByEmail = async (username) => {
        const url = `${location.origin}/user/get-by-username?username=${username}`;
        const json = await (await fetch(url)).json();

        $searchResult.innerHTML = null;
        
        if(json.errorMsg) {
            const $p = document.createElement("p");
            $p.textContent = json.errorMsg;
            $searchResult.appendChild($p);
        } else {
            console.log(json);
            const $btn = document.createElement("button");
            $btn.textContent = json.user.Email;
            $btn.className = "btn btn-primary";

            $btn.addEventListener("click", e => {
                const $p = document.createElement("p");
                $p.textContent = json.user.Email;
                $p.dataset["userid"] = json.user.UserId;
                $usersContainer.appendChild($p);
            });
            $searchResult.appendChild($btn);
        }

    }
    const throttleFunc = throttle(getUserByEmail, 200);

    $formControlSearchUser.addEventListener("input", e => {
        console.log($formControlSearchUser.value);
        throttleFunc($formControlSearchUser.value);
    });
}

//Questions for the database
const $btnCreateTemplate = document.getElementById("btn-create-template");

$btnCreateTemplate.addEventListener("click", async e => {

    const questions = [];
    const $questionsContainer = document.getElementById("template-questions");
    const $questions = Array.from($questionsContainer.children).splice(2);
    let questionOptions = [];

    //Create the questions based on the html elements
    //Create the options for questions if is necessary
    $questions.forEach($question => {

        //This means the question is of type multioption
        if(parseInt($question.dataset["QuestionType"]) == 4) {
            const $select = $question.querySelector("select");
            const $options = $select.querySelectorAll("option");

            $options.forEach($option => questionOptions.push({ option: $option.value }));
        }

        const question = {
            questionString: $question.querySelector("label").textContent,
            questionType: parseInt($question.dataset["QuestionType"]),
            questionOptions: [...questionOptions]
        }
        
        questionOptions = [];
        
        questions.push(question);
    });

    //Template creation
    const templateConfig = {
        title: document.getElementById("setting-template-title").textContent.trim(),
        description: document.getElementById("setting-template-description").textContent.trim(),
        image_url: document.getElementById("setting-template-image").value.trim() || "default.png",
        topicId: document.getElementById("setting-template-topic").value.trim(),
        isPublic: parseInt(document.getElementById("template-visibility-switch").value) === 1,
        questions,
        admins,
        usersAllowedToAnswer
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
        
        console.log(isTemplateSavedJSON);
    }
});
