import insertLogOutButton from "../../UI/components/btnLogOut.js";
import makeRequest from "../utils/http/makeRequest.js";
import printTemplates from "../utils/templates/printTemplates.js";

let pageForTemplates = 0;
let limit = 10;

const $btnToggleTemplates = document.getElementById("btn-toggle-templates");
const $templatesHeader = document.getElementById("templates-header");
const $templatesContainer = document.getElementById("latest-templates-container");

try {
    insertLogOutButton();
} catch(e) {
    console.error(e);
}

//Get the latest templates
async function getLatestTemplates() {
    const json = makeRequest(`template/templates?page=${pageForTemplates}&limit=${limit}`);
    ++pageForTemplates;
    
    return json;
}

const templates = await getLatestTemplates();

printTemplates(templates.data, $templatesContainer);

//Get user's templates
let pageForTemplatesByUser = 0;
async function getTemplatesByUserId() {
    const userId = document.getElementById("userId").value;
    const json = makeRequest(`template/template/user?page=${pageForTemplatesByUser}&limit=${limit}&userId=${userId}`);
    ++pageForTemplatesByUser;
    
    return json;
}

//Button to toggle the templates between latest templates and the user's templates
try {
    $btnToggleTemplates.addEventListener("click", async e => {
        $templatesContainer.innerHTML = null;
        pageForTemplates = 0;
        pageForTemplatesByUser = 0;
    
        if($btnToggleTemplates.textContent === "Your Templates") {
            $btnToggleTemplates.textContent = "See the latest templates";
            $templatesHeader.textContent = "Your Templates";
            const userTemplates = await getTemplatesByUserId();
            printTemplates(userTemplates.data, $templatesContainer);
        } else {
            $btnToggleTemplates.textContent = "Your Templates";
            $templatesHeader.textContent = "Latest Templates";
            const templates = await getTemplatesByUserId();
            printTemplates(templates.data, $templatesContainer);
        }
    });
} catch(e) {
    console.error(e);
}
