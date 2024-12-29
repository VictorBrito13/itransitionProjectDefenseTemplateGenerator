import insertLogOutButton from "../../UI/components/btnLogOut.js";
import { getLatestTemplates, resetPagesForLatestTemplates } from "../utils/templates/getLatestTemplates.js";
import { getTemplatesByUserId, resetPagesForUserTemplates } from "../utils/templates/getTemplatesByUserId.js";
import printTemplates from "../utils/templates/printTemplates.js";

const $btnToggleTemplates = document.getElementById("btn-toggle-templates");
const $templatesHeader = document.getElementById("templates-header");
const $templatesContainer = document.getElementById("latest-templates-container");

try {
    insertLogOutButton();
} catch(e) {
    console.error(e);
}

const latestTemplates = await getLatestTemplates();
console.log(latestTemplates);

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
