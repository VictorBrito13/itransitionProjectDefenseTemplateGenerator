export default async function makeRequest(path, options = {}) {
    try {
        const { method, headers, body } = options;

        const fetchOptions =  {
            method: method || "GET",
            headers: headers || {
                "Content-Type": "application/json"
            }
        }
        
        if(body) {
            fetchOptions.body = JSON.stringify(body);
        }

        const response = await fetch(`${location.origin}/${path}`, fetchOptions);

        return await response.json();
    } catch(err) {
        console.log("An error has ocurred");
        console.error(err);
        return null;
    }
}