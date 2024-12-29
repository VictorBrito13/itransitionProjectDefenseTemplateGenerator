import makeRequest from "../http/makeRequest.js";

//Get user's templates
let pageForTemplatesByUser = 0;
const limit = 2;

async function getTemplatesByUserId() {
    const userId = document.getElementById("userId").value;
    const json = await makeRequest(`template/template/user?page=${pageForTemplatesByUser}&limit=${limit}&userId=${userId}`);
    ++pageForTemplatesByUser;

    return json;
}

function resetPagesForUserTemplates() {
    pageForTemplatesByUser = 0;
}

export { getTemplatesByUserId, resetPagesForUserTemplates }
