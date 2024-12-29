import makeRequest from "../http/makeRequest.js";

let pageForTemplates = 0;
const limit = 3;

//Get the latest templates
async function getLatestTemplates() {
    const json = await makeRequest(`template/templates?page=${pageForTemplates}&limit=${limit}`);
    ++pageForTemplates;

    return json;
}

function resetPagesForLatestTemplates() {
    pageForTemplates = 0;
}

export { getLatestTemplates, resetPagesForLatestTemplates }
