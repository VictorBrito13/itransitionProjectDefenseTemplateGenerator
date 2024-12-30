import insertLogOutButton from "../../UI/components/btnLogOut.js";
import { getLatestTemplates, resetPagesForLatestTemplates } from "../utils/templates/getLatestTemplates.js";
import getTemplatesByQuery from "../utils/templates/getTemplateByQuery.js";
import { getTemplatesByUserId, resetPagesForUserTemplates } from "../utils/templates/getTemplatesByUserId.js";
import printTemplates from "../utils/templates/printTemplates.js";
import throttle from "../utils/throttle.js";

const $btnToggleTemplates = document.getElementById("btn-toggle-templates");
const $templatesHeader = document.getElementById("templates-header");
const $templatesContainer = document.getElementById("latest-templates-container");
const $inputSearchTemplatesByQuery = document.getElementById("search-template-control");
const $searchResultsContainer = document.getElementById("search-result-contianer");

const throttleGetTemplatesByQuery = throttle(getTemplatesByQuery, 200);

$inputSearchTemplatesByQuery.addEventListener("focus", e => {
    $searchResultsContainer.classList.add("element-visible");
    $searchResultsContainer.classList.remove("element-hidden");
});

$inputSearchTemplatesByQuery.addEventListener("focusout", e => {
    $searchResultsContainer.classList.add("element-hidden");
    $searchResultsContainer.classList.remove("element-visible");
});

$inputSearchTemplatesByQuery.addEventListener("input", async () => {    
    const templates = await throttleGetTemplatesByQuery($inputSearchTemplatesByQuery.value);
    const { data } = templates;
    
    if(!data || !(data instanceof Object)) {
        $searchResultsContainer.innerHTML = `<p class="bg-danger text-light p-3 rounded-3">${data}</p>`;
    } else {
        let content = "";
        data.forEach(template => {
            content +=
            `
            <a class="icon-link link-secondary icon-link-hover" href="/template/template?templateId=${template.TemplateId}">
                <div>
                    <h3>${template.Title}</h3>
                    <hr>
                    <p>${template.Description}</p>
                    <hr class="border border-primary">
                </div>
            </a>
            `;
        });
        $searchResultsContainer.innerHTML = content;
    }
});

try {
    insertLogOutButton();
} catch(e) {
    console.error(e);
}

const latestTemplates = await getLatestTemplates();

printTemplates(latestTemplates.data, $templatesContainer, "latest");

//Button to toggle the templates between latest templates and the user's templates
try {
    $btnToggleTemplates.addEventListener("click", async e => {
        $templatesContainer.innerHTML = null;
    
        if($btnToggleTemplates.textContent === "Your Templates") {
            $btnToggleTemplates.textContent = "See the latest templates";
            $templatesHeader.textContent = "Your Templates";
            const userTemplates = await getTemplatesByUserId();
            resetPagesForLatestTemplates();
            printTemplates(userTemplates.data, $templatesContainer, "user");
        } else {
            $btnToggleTemplates.textContent = "Your Templates";
            $templatesHeader.textContent = "Latest Templates";
            const latestTemplates = await getLatestTemplates();
            resetPagesForUserTemplates();
            printTemplates(latestTemplates.data, $templatesContainer, "latest");
        }
    });
} catch(e) {
    console.error(e);
}

export { getLatestTemplates, getTemplatesByUserId }
