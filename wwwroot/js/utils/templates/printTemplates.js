import { getLatestTemplates } from "./getLatestTemplates.js";
import { getTemplatesByUserId } from "./getTemplatesByUserId.js";

export default async function printTemplates(templates, $templatesContainer, templatesMode) {
    if(!templates || templates.length === 0) {
        throw new Error("There is no templates");
    }
    
    if(templates) {
        let content = "";
        templates.forEach((template) => {
            const $a = document.createElement("a");
            if(templatesMode === "latest") {
                $a.href = `/template/template?templateId=${template.TemplateId}`;
            } else if(templatesMode === "user") {
                $a.href = `/template/create?templateId=${template.TemplateId}`;
            }
            $a.className = "col-12 text-dark";
            // <a href="/template/template?templateId=${template.TemplateId}" class="col-12 text-dark">
            $a.innerHTML +=
            `
                <div class="card d-flex justify-content-center background-image-conainer">
                    <img
                        src="/images/${template.Image_url}"
                        class="card-image-top background-image"
                        alt="${template.Title}"
                        width="150" height="150"
                        loading="lazy"
                    >
                    <div class="bg-light bg-opacity-50 rounded-3 p-3 text-center">
                        <span>${template.TemplateId}</span>
                        <h3 class="card-title">${template.Title}</h3>
                        <p class="card-text">By <b>${template.Admins[0]?.User?.Username ?? "you"}</b></p>
                        <p class="card-text">${template.Description}</p>
                    </div>
                </div>
            `;
            // </a>
            $templatesContainer.appendChild($a);
        });
        // $templatesContainer.innerHTML += content;

        // Infinity scroll
        const cb = (entries) => {
            entries.forEach(async (entry) => {
                console.log(entry.target);
                if(entry.isIntersecting) {
                    // make petitions to get more templates
                    if(templatesMode === "latest") {
                        const latestTemplates = await getLatestTemplates();
                        console.log(latestTemplates)
                        printTemplates(latestTemplates.data, $templatesContainer, "latest");
                    } else if(templatesMode === "user") {
                        const userTemplates = await getTemplatesByUserId();
                        printTemplates(userTemplates.data, $templatesContainer, "user");
                        console.log(userTemplates)
                    } else {
                        throw new Error("This option is not valid");
                    }
                    unobserve();
                }
            });
        }

        const observer = new IntersectionObserver(cb, {
            root: document,
            threshold: 0.50,
            rootMargin: "0px"
        });

        const $lastChild = $templatesContainer.children[$templatesContainer.children.length - 1];

        observer.observe($lastChild);

        function unobserve() {
            observer.unobserve($lastChild);
        }

    } else if(!templates) {
        const $p = document.createElement("p");
        $p.textContent = "There is no templates here";
        $templatesContainer.appendChild($p);
    }

}