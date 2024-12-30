import makeRequest from "../http/makeRequest.js";

export default async function getTemplatesByQuery(text) {
    const templates = await makeRequest(`template/get-by-query?text=${text}`);

    return templates;
}