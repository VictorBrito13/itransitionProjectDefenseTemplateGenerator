import insertLogOutButton from "../../UI/components/btnLogOut.js";

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
            <a href="/template/template?templateId=${template.TemplateId}" class="col-12 text-dark">
                <div class="card d-flex justify-content-center background-image-conainer">
                    <img
                        src="/images/${template.Image_url}"
                        class="card-image-top background-image"
                        alt="${template.Title}"
                        width="150" height="150">
                    <div class="bg-light bg-opacity-50 rounded-3 p-3 text-center">
                        <p class="card-text">${template.Title} By <b>${template.Admins[0]?.User.Username}</b></p>
                        <p class="card-text">${template.Description}</p>
                    </div>
                </div>
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
            <a href="/template/create?templateId=${template.TemplateId}" class="col-12">
                <div class="card">
                    <img src="${template.Image_url}" class="card-image-top" alt="${template.Title}" width="150" height="150">
                    <div class"card-body">
                        <card-text>Template name By <b>${username}</b></card-text>
                    </div>
                </div>
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

//Paginations buttons
const $btnPaginationPrev = document.getElementById("pagination-prev");
const $btnPaginationNext = document.getElementById("pagination-next");

$btnPaginationPrev.addEventListener("click", e => {

});

$btnPaginationNext.addEventListener("click", e => {

});

