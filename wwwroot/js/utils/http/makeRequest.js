function getCsrfToken() {
    const meta = document.querySelector('meta[name="csrf-token"]');
    return meta?.getAttribute('content') || '';
}

export default async function makeRequest(path, options = {}) {
    try {
        const { method, headers, body } = options;

        const fetchOptions = {
            method: method || "GET",
            headers: headers || {
                "Content-Type": "application/json"
            }
        };

        if (body) {
            fetchOptions.body = JSON.stringify(body);
        }

        const stateChangingMethods = ['POST', 'PUT', 'DELETE', 'PATCH'];
        if (stateChangingMethods.includes(fetchOptions.method.toUpperCase())) {
            fetchOptions.headers['X-CSRF-TOKEN'] = getCsrfToken();
        }

        const response = await fetch(`${location.origin}/${path}`, fetchOptions);
        const json = await response.json();

        // Handle HTTP error responses (4xx, 5xx)
        if (!response.ok) {
            const errorMessage = parseErrorMessage(json);
            showError(errorMessage);
            return { error: { code: response.status, message: errorMessage }, data: null };
        }

        return json;
    } catch (err) {
        console.error('Request failed:', err);
        showError('Network error — please check your connection and try again');
        return { error: { code: 0, message: 'Network error' }, data: null };
    }
}
