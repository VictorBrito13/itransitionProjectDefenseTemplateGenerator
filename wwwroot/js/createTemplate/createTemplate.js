import throttle from "../utils/throttle.js";
import { getTemplate } from "../utils/buildForm.js";
import { buildForm } from "../utils/buildForm.js";
import insertQuestions from "../utils/createTemplate/insertQuestions.js";
import saveSelectOptions from "../utils/createTemplate/saveSelectOptions.js";
import toggleVisibilityTemplate from "../utils/createTemplate/visibilitySwitcher.js";
import deleteElementOnClick from "../utils/deleteElement.js";
import deleteTemplate from "../utils/createTemplate/deleteTemplate.js";

insertQuestions();
saveSelectOptions();

//---------------------------------------------------Update template functions
const urlParamsSearcher = new URLSearchParams(location.search);
let admins = [];
let usersAllowedToAnswer = []
//Hide the admin's button
document.getElementById("btn-add-admin").style.display = "none";
//Hide the danger zone
document.getElementById("danger-zone-container").style.display = "none";

const $serverMsgs = document.getElementById("server-responses");

if(urlParamsSearcher.get("templateId")) {
    //show the admin's button
    document.getElementById("btn-add-admin").style.display = "inline-block";
    //Show the danger zone
    document.getElementById("danger-zone-container").style.display = "block";

    const $templateTitle = document.getElementById("setting-template-title");
    const $tempalteDescription = document.getElementById("setting-template-description");
    //Make a petition to get the template
    const template = await getTemplate();
    $tempalteDescription.textContent = template.Description;
    $templateTitle.textContent = template.Title;
    console.log(template);

    //button to delete the template
    deleteTemplate(template.TemplateId);
    
    admins = template.Admins || [];
    usersAllowedToAnswer = template.usersAllowedToAnswer || [];


    //When it gets clicked it save the admins or users allowed to answer in the respective array
    const $btnSaveUser = document.getElementById("btn-save-user");

    const $pageTitle = document.getElementById("page-title");
    const $btnCreateTemplate = document.getElementById("btn-create-template");
    $pageTitle.textContent = "Edit the template";
    $btnCreateTemplate.textContent = "Edit template";
    const $formTemplate = document.getElementById("template-questions");
    const $usersContainer = document.getElementById("users-container");

    //Print all the users in the form
    buildForm($formTemplate, template, true);

    //template's visibility switcher    
    toggleVisibilityTemplate(template.IsPublic);

    //Change the modal's title for add admins or users that are able to answer the form
    const $modal_Add_Admins_Or_Users_Allowed_To_Answer = document.getElementById("admin_allowed-users");

    $btnSaveUser.addEventListener("click", e => {
        const $users = $usersContainer.querySelectorAll("p");

        if($modal_Add_Admins_Or_Users_Allowed_To_Answer.dataset["userType"] === "admin") {
            admins = []
            $users.forEach($user => {
                admins.push(
                {
                    UserId: parseInt($user.dataset["userid"]),
                    TemplateId : template.TemplateId,
                    //This property is removed before the template get sended
                    User: {
                        UserId: $user.dataset["userid"],
                        Username: $user.dataset["username"]
                    }
                });
            });
            console.log(admins);
            
        } else if($modal_Add_Admins_Or_Users_Allowed_To_Answer.dataset["userType"] === "allowUser") {
            usersAllowedToAnswer = []
            $users.forEach($user => {
                usersAllowedToAnswer.push(
                {
                    UserId: parseInt($user.dataset["userid"]),
                    TemplateId : template.TemplateId,
                    //This property is removed before the template get sended
                    User: {
                        UserId: $user.dataset["userid"],
                        Username: $user.dataset["username"]
                    }
                });
            });
            console.log(usersAllowedToAnswer);
            
        }
    });

    function generateUserHmtl(user) {
        const $p = document.createElement("p");
        console.log(user.Email)
        $p.textContent = user.Email;
        $p.dataset["userid"] = user.UserId;
        $p.dataset["username"] = user.Username;
        const $btnDelete = document.createElement("button");
        $btnDelete.type = "button";
        $btnDelete.textContent = "Delete"
        $btnDelete.className = "btn btn-danger";

        deleteElementOnClick($btnDelete, $p);

        $p.appendChild($btnDelete);

        return $p;
    }

    $modal_Add_Admins_Or_Users_Allowed_To_Answer.addEventListener("show.bs.modal", e => {
        $usersContainer.innerHTML = null;
        const $trigger = e.relatedTarget;
        const title = $trigger.dataset["bsTitle"];

        //Insert the admins or user allowed to answer the form
        if($trigger.id === "btn-add-admin") {
            $modal_Add_Admins_Or_Users_Allowed_To_Answer.dataset["userType"] = "admin";
            admins.forEach(admin => {
                const { User } = admin;
                $usersContainer.appendChild(generateUserHmtl(User));
            });
        } else if($trigger.id === "btn-add-allowed-users") {
            $modal_Add_Admins_Or_Users_Allowed_To_Answer.dataset["userType"] = "allowUser";
            usersAllowedToAnswer.forEach(userAllowedToAnswer => {
                const { User } = userAllowedToAnswer;
                $usersContainer.appendChild(generateUserHmtl(User));
            });
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
        topicId: document.getElementById("setting-template-topic").value.trim(),
        isPublic: parseInt(document.getElementById("template-visibility-switch").value) === 1,
        questions,
        admins: admins.map(a => { return { UserId: a.UserId, TemplateId: a.TemplateId} }),
        usersAllowedToAnswer: usersAllowedToAnswer.map(a => { return { UserId: a.UserId, TemplateId: a.TemplateId} })
    };

    console.log(templateConfig);

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
        const templateUpdatedJSON = await isTemplateUpdated.json();
        console.log(templateUpdatedJSON);

        if(templateUpdatedJSON.status === 401) {
            location.assign(`${location.origin}/user/log-in`);
        }

        const $p = document.createElement("p");
        if(templateUpdatedJSON.errorMsg) {
            $p.textContent = templateUpdatedJSON.errorMsg;
            $p.className = "p-3 rounded text-light bg-danger";
        } else if(templateUpdatedJSON.data) {
            $p.textContent = templateUpdatedJSON.data;
            $p.className = "p-3 rounded text-light bg-success";
        }

        $serverMsgs.appendChild($p);

        setTimeout(() => {
            $serverMsgs.innerHTML = null;
        }, 3000);

    } else {
        //Petition to save the template
        const templateSaved = await fetch(`${location.origin}/template/create`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(templateConfig)
        });
        const templateSavedJSON = await templateSaved.json();

        if(templateSavedJSON.status === 401) {
            location.assign(`${location.origin}/user/log-in`);
        }
        
        console.log(templateSavedJSON);
        const $p = document.createElement("p");

        if(templateSavedJSON.errorMsg) {
            $p.className = "bg-danger rounded text-light p-3";
            $p.textContent = templateSavedJSON.errorMsg;
        } else {
            $p.className = "bg-success rounded text-light p-3";
            $p.innerHTML =
            `Your template has been saved succesfully you can update your template
            <a class="btn btn-light" href="${location.origin}/template/create?templateId=${templateSavedJSON.templateId}">Here</a>`;
        }

        $serverMsgs.appendChild($p);

        setTimeout(() => {
            $serverMsgs.innerHTML = null;
        }, 3000);
    }
});
