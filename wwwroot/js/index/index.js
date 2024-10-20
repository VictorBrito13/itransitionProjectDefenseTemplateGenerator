let pageForTemplates = 0;
let limit = 10;

const $btnToggleTemplates = document.getElementById("btn-toggle-templates");
const $templatesHeader = document.getElementById("templates-header");
const $templatesContainer = document.getElementById("latest-templates-container");

//Get all the templates
async function getLatestTemplates() {
    const res = await fetch(`${location.origin}/template/templates?page=${pageForTemplates}&limit=${limit}`);
    const json = await res.json();
    ++pageForTemplates;
    
    return json;
}

async function printLatestTemplates() {
    const templates = await getLatestTemplates();
    console.log("Lates templates");
    console.log(templates);
    

    if(templates.data) {
        let content = "";
        templates.data.forEach(template => {
            content +=
            `
            <a href="/template/template?templateId=${template.TemplateId}" class="col-4">
                <figure class="text-center">
                    <img src="${template.Image_url}"  alt="${template.Title}" width="150" height="150">
                    <figcaption>Template name By <b>${template.Admins[0].User.Username}</b></figcaption>
                </figure>
            </a>
            `;
        });
        $templatesContainer.innerHTML += content
    } else if(templates === null || templates.data.length === 0) {
        const $p = document.createElement("p");
        $p.textContent = "There is no templates here";
        $templatesContainer.appendChild($p);
    }

}

printLatestTemplates()

//Get user's templates
let pageForTemplatesByUser = 0;
async function getTemplatesByUserId() {
    const userId = document.getElementById("userId").value;
    const res = await fetch(`${location.origin}/template/template/user?page=${pageForTemplatesByUser}&limit=${limit}&userId=${userId}`);
    const json = await res.json();
    ++pageForTemplatesByUser;
    
    return json;
}

async function printUserTemplates() {
    const templates = await getTemplatesByUserId();
    console.log(templates);
    
    
    const username = document.getElementById("username").value;

    if(templates.data) {
        let content = "";
        templates.data.forEach(template => {
            content +=
            `
            <a href="/template/create?templateId=${template.TemplateId}" class="col-4">
                <figure class="text-center">
                    <img src="${template.Image_url}" alt="${template.Title}" width="150" height="150">
                    <figcaption>Template name By <b>${username}</b></figcaption>
                </figure>
            </a>
            `;
        });
        $templatesContainer.innerHTML += content
    } else if(templates === null || templates.data.length === 0) {
        const $p = document.createElement("p");
        $p.textContent = "There is no templates here";
        $templatesContainer.appendChild($p);
    }
}

//Button to toggle the templates between latest templates and the user's templates
try {
    $btnToggleTemplates.addEventListener("click", e => {
        $templatesContainer.innerHTML = null;
        pageForTemplates = 0;
        pageForTemplatesByUser = 0;
        console.log($btnToggleTemplates.textContent)
    
        if($btnToggleTemplates.textContent === "Your Templates") {
            $btnToggleTemplates.textContent = "See the latest templates";
            $templatesHeader.textContent = "Your Templates";
            printUserTemplates();            
        } else {
            $btnToggleTemplates.textContent = "Your Templates";
            $templatesHeader.textContent = "Latest Templates";
            printLatestTemplates();
        }
    });
} catch(e) {
    console.error(e);
}
