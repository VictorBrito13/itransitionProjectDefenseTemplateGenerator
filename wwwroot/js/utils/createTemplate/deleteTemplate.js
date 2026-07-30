const $btnDeleteTemplate = document.getElementById("delete-template");

export default function deleteTemplate(templateId) {
    const $serverMsgs = document.getElementById("server-responses");
    const csrfToken = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content') || '';

    $btnDeleteTemplate.addEventListener("click", async e => {
        const deleteRes = await fetch(`${location.origin}/template/delete?templateId=${templateId}`, {
            method: "DELETE",
            headers: {
                "X-CSRF-TOKEN": csrfToken
            }
        });

        const deleteJSON = await deleteRes.json();

        if(deleteJSON.error?.code === 401) {
            location.assign(`${location.origin}/user/log-in`);
        }

        if(deleteJSON.data) {
            location.assign(`${location.origin}/`);
        } else {
            showError(deleteJSON.error?.message || 'Failed to delete template');
        }
    });
}