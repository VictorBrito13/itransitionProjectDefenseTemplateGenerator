const $btnDeleteTemplate = document.getElementById("delete-template");

export default function deleteTemplate(templateId) {
    const $serverMsgs = document.getElementById("server-responses");

    $btnDeleteTemplate.addEventListener("click", async e => {
        const deleteRes = await fetch(`${location.origin}/template/delete?templateId=${templateId}`, {
            method: "DELETE"
        });

        const deleteJSON = await deleteRes.json();

        if(deleteJSON.status === 401) {
            location.assign(`${location.origin}/user/log-in`);
        }

        //The template was deleted
        if(deleteJSON.data) {
            location.assign(`${location.origin}/`);
        } else {
            $serverMsgs.innerHTML = `<p class="bg-danger p-3 rounded text-light">${deleteJSON.errorMsg}</p>`
        }
    });
}