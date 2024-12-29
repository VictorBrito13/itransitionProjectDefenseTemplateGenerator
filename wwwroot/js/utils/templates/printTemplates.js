export default async function printTemplates(templates, $templatesContainer) {
    if(templates) {
        let content = "";
        templates.forEach(template => {
            content +=
            `
            <a href="/template/template?templateId=${template.TemplateId}" class="col-12 text-dark">
                <div class="card d-flex justify-content-center background-image-conainer">
                    <img
                        src="/images/${template.Image_url}"
                        class="card-image-top background-image"
                        alt="${template.Title}"
                        width="150" height="150"
                    >
                    <div class="bg-light bg-opacity-50 rounded-3 p-3 text-center">
                        <h3 class="card-title">${template.Title}</h3>
                        <p class="card-text">By <b>${template.Admins[0]?.User?.Username ?? "you"}</b></p>
                        <p class="card-text">${template.Description}</p>
                    </div>
                </div>
            </a>
            `;
        });
        $templatesContainer.innerHTML += content;

        const cb = (entries) => {
            entries.forEach(entry => {
                console.log(entry);
                if(entry.isIntersecting) {
                    
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
    } else if(!templates) {
        const $p = document.createElement("p");
        $p.textContent = "There is no templates here";
        $templatesContainer.appendChild($p);
    }

}