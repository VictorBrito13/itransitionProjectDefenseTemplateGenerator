import { getLatestTemplates } from "./getLatestTemplates.js";
import { getTemplatesByUserId } from "./getTemplatesByUserId.js";

const topicColors = {
    1: { bg: "bg-blue-100", text: "text-blue-700", gradient: "from-blue-400 to-blue-600" },
    2: { bg: "bg-emerald-100", text: "text-emerald-700", gradient: "from-emerald-400 to-emerald-600" },
    3: { bg: "bg-purple-100", text: "text-purple-700", gradient: "from-purple-400 to-purple-600" },
    4: { bg: "bg-amber-100", text: "text-amber-700", gradient: "from-amber-400 to-amber-600" },
    5: { bg: "bg-rose-100", text: "text-rose-700", gradient: "from-rose-400 to-rose-600" },
    6: { bg: "bg-cyan-100", text: "text-cyan-700", gradient: "from-cyan-400 to-cyan-600" },
    7: { bg: "bg-indigo-100", text: "text-indigo-700", gradient: "from-indigo-400 to-indigo-600" },
    8: { bg: "bg-orange-100", text: "text-orange-700", gradient: "from-orange-400 to-orange-600" },
};

const defaultColor = { bg: "bg-gray-100", text: "text-gray-700", gradient: "from-gray-400 to-gray-600" };

function getTopicColor(topicId) {
    return topicColors[topicId] || defaultColor;
}

export function createSkeletonCard() {
    return `
        <div class="bg-white rounded-2xl shadow-soft overflow-hidden skeleton-card">
            <div class="h-32 bg-gray-200 animate-pulse"></div>
            <div class="p-6">
                <div class="h-6 bg-gray-200 rounded animate-pulse mb-2 w-3/4"></div>
                <div class="h-4 bg-gray-200 rounded animate-pulse mb-2 w-full"></div>
                <div class="h-4 bg-gray-200 rounded animate-pulse mb-4 w-1/2"></div>
                <div class="flex items-center gap-2">
                    <div class="h-3 bg-gray-200 rounded animate-pulse w-20"></div>
                    <div class="h-3 bg-gray-200 rounded animate-pulse w-12"></div>
                </div>
            </div>
        </div>
    `;
}

function createTemplateCard(template, templatesMode) {
    const topicColor = getTopicColor(template.TopicId);
    const topicName = template.Topic?.Name || "Uncategorized";
    const creatorName = template.Admins?.[0]?.User?.Username ?? "Unknown";
    const likeCount = template.Likes?.length ?? 0;
    const href = templatesMode === "user"
        ? `/template/create?templateId=${template.TemplateId}`
        : `/template/template?templateId=${template.TemplateId}`;

    return `
        <a href="${href}" class="block bg-white rounded-2xl shadow-soft overflow-hidden hover:shadow-lg transition-shadow cursor-pointer group">
            <div class="h-32 bg-gradient-to-br ${topicColor.gradient} flex items-center justify-center relative">
                <svg class="w-12 h-12 text-white/60 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
                </svg>
            </div>
            <div class="p-6">
                <h3 class="text-xl font-semibold text-gray-900 mb-2 group-hover:text-primary transition-colors">${template.Title}</h3>
                <p class="text-gray-600 text-sm mb-4 line-clamp-2">${template.Description}</p>
                <span class="inline-block px-3 py-1 ${topicColor.bg} ${topicColor.text} text-xs font-medium rounded-full">${topicName}</span>
            </div>
            <div class="px-6 py-4 border-t border-gray-100 flex justify-between items-center">
                <span class="text-sm text-gray-500">by ${creatorName}</span>
                <span class="flex items-center gap-1 text-sm text-gray-500">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/>
                    </svg>
                    ${likeCount}
                </span>
            </div>
        </a>
    `;
}

export default async function printTemplates(templates, $templatesContainer, templatesMode) {
    // Remove skeleton cards on first load
    const skeletons = $templatesContainer.querySelectorAll('.skeleton-card');
    skeletons.forEach(el => el.remove());

    if (!templates || templates.length === 0) {
        $templatesContainer.classList.add('hidden');
        const $emptyState = document.getElementById('empty-state');
        if ($emptyState) $emptyState.classList.remove('hidden');
        return;
    }

    let content = "";
    templates.forEach((template) => {
        content += createTemplateCard(template, templatesMode);
    });
    $templatesContainer.insertAdjacentHTML('beforeend', content);

    // Infinity scroll
    const cb = (entries) => {
        entries.forEach(async (entry) => {
            if (entry.isIntersecting) {
                let result;
                if (templatesMode === "latest") {
                    result = await getLatestTemplates();
                } else if (templatesMode === "user") {
                    result = await getTemplatesByUserId();
                } else {
                    throw new Error("This option is not valid");
                }

                if (result && result.data && result.data.length > 0) {
                    printTemplates(result.data, $templatesContainer, templatesMode);
                } else {
                    unobserve();
                    const $loadMoreContainer = document.getElementById('load-more-container');
                    if ($loadMoreContainer) $loadMoreContainer.classList.add('hidden');
                }
            }
        });
    };

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
}
