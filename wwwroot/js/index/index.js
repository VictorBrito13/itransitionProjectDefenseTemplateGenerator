import { getLatestTemplates, resetPagesForLatestTemplates } from "../utils/templates/getLatestTemplates.js";
import getTemplatesByQuery from "../utils/templates/getTemplateByQuery.js";
import { getTemplatesByUserId, resetPagesForUserTemplates } from "../utils/templates/getTemplatesByUserId.js";
import printTemplates, { createSkeletonCard } from "../utils/templates/printTemplates.js";
import debounce from "../utils/debounce.js";

const $btnToggleTemplates = document.getElementById("btn-toggle-templates");
const $templatesHeader = document.getElementById("templates-header");
const $templatesContainer = document.getElementById("latest-templates-container");
const $inputSearchTemplatesByQuery = document.getElementById("search-template-control");
const $searchResultsContainer = document.getElementById("search-result-contianer");
const $loadMoreContainer = document.getElementById("load-more-container");

let currentMode = "latest";

// --- Skeleton Management ---
function showSkeletons() {
    $templatesContainer.innerHTML = "";
    for (let i = 0; i < 6; i++) {
        $templatesContainer.insertAdjacentHTML('beforeend', createSkeletonCard());
    }
    $templatesContainer.classList.remove('hidden');
}

function hideSkeletons() {
    const skeletons = $templatesContainer.querySelectorAll('.skeleton-card');
    skeletons.forEach(el => el.remove());
}

// --- Template Loading ---
async function loadTemplates() {
    await loadAndRenderTemplates(getLatestTemplates, "latest");
}

/**
 * Shared helper: shows skeletons, fetches templates via the given function,
 * hides skeletons, then renders results or shows empty state.
 * @param {Function} fetchFn - Async function that returns { data: [...] }
 * @param {string} mode - "latest" or "user" for printTemplates
 */
async function loadAndRenderTemplates(fetchFn, mode) {
    showSkeletons();
    try {
        const result = await fetchFn();
        hideSkeletons();

        if (result && result.data && result.data.length > 0) {
            $templatesContainer.classList.remove('hidden');
            const $emptyState = document.getElementById('empty-state');
            if ($emptyState) $emptyState.classList.add('hidden');
            await printTemplates(result.data, $templatesContainer, mode);
        } else {
            $templatesContainer.classList.add('hidden');
            const $emptyState = document.getElementById('empty-state');
            if ($emptyState) $emptyState.classList.remove('hidden');
        }
    } catch (e) {
        hideSkeletons();
        console.error(`Failed to load ${mode} templates:`, e);
    }
}

// --- Search Functionality ---
const debouncedSearch = debounce(async (query) => {
    if (!query || query.trim().length === 0) {
        $searchResultsContainer.classList.add('hidden');
        $searchResultsContainer.innerHTML = "";
        return;
    }

    try {
        const result = await getTemplatesByQuery(query);
        const { data } = result;

        if (!data || !(data instanceof Object)) {
            $searchResultsContainer.innerHTML = `
                <div class="p-4 text-center text-gray-500">
                    <p>No templates found</p>
                </div>
            `;
            $searchResultsContainer.classList.remove('hidden');
        } else if (Array.isArray(data) && data.length === 0) {
            $searchResultsContainer.innerHTML = `
                <div class="p-4 text-center text-gray-500">
                    <p>No templates match your search</p>
                </div>
            `;
            $searchResultsContainer.classList.remove('hidden');
        } else if (Array.isArray(data)) {
            let content = "";
            data.forEach(template => {
                const topicName = template.Topic?.Name || "Uncategorized";
                const creatorName = template.Admins?.[0]?.User?.Username ?? "Unknown";
                content += `
                    <a href="/template/template?templateId=${template.TemplateId}" class="block px-4 py-3 hover:bg-gray-50 transition-colors border-b border-gray-100 last:border-b-0">
                        <div class="flex items-center justify-between">
                            <div>
                                <h3 class="text-sm font-semibold text-gray-900">${template.Title}</h3>
                                <p class="text-xs text-gray-500 mt-0.5">${topicName} &middot; by ${creatorName}</p>
                            </div>
                            <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/>
                            </svg>
                        </div>
                    </a>
                `;
            });
            $searchResultsContainer.innerHTML = content;
            $searchResultsContainer.classList.remove('hidden');
        } else {
            // Handle case where data is a message string (from 404 response)
            $searchResultsContainer.innerHTML = `
                <div class="p-4 text-center text-gray-500">
                    <p>${data}</p>
                </div>
            `;
            $searchResultsContainer.classList.remove('hidden');
        }
    } catch (e) {
        console.error("Search error:", e);
    }
}, 300);

$inputSearchTemplatesByQuery.addEventListener("input", (e) => {
    debouncedSearch(e.target.value);
});

// Close search dropdown when clicking outside
document.addEventListener("click", (e) => {
    if (!$inputSearchTemplatesByQuery.contains(e.target) && !$searchResultsContainer.contains(e.target)) {
        $searchResultsContainer.classList.add('hidden');
    }
});

// Show dropdown when focusing search input
$inputSearchTemplatesByQuery.addEventListener("focus", (e) => {
    if (e.target.value && e.target.value.trim().length > 0) {
        $searchResultsContainer.classList.remove('hidden');
    }
});

// --- User Templates Toggle ---
if ($btnToggleTemplates) {
    $btnToggleTemplates.addEventListener("click", async () => {
        if (currentMode === "latest") {
            currentMode = "user";
            $btnToggleTemplates.textContent = "Latest Templates";
            $templatesHeader.textContent = "Your Templates";
            resetPagesForLatestTemplates();
            await loadAndRenderTemplates(getTemplatesByUserId, "user");
        } else {
            currentMode = "latest";
            $btnToggleTemplates.textContent = "Your Templates";
            $templatesHeader.textContent = "Latest Templates";
            resetPagesForUserTemplates();
            await loadAndRenderTemplates(getLatestTemplates, "latest");
        }
    });
}

// --- Initial Load ---
await loadTemplates();

export { getLatestTemplates, getTemplatesByUserId }
