import insertLogOutButton from "../../UI/components/btnLogOut.js";
import { buildForm, getTemplate } from "../utils/buildForm.js";
import makeRequest from "../utils/http/makeRequest.js";

const $form = document.getElementById("form-questions");
const $formTitle = document.getElementById("form-title");
const $formDescription = document.getElementById("form-description");
const $btnLikeTemplate = document.getElementById("btn-like-template");
const $iconStar = document.getElementById("icon-star");
const $iconStarFill = document.getElementById("icon-star-fill");
const userId = document.getElementById("form-user-email").dataset["userid"];
const $likesNumber = document.getElementById("likes-number");
const $serverMsgs = document.getElementById("server-msgs");
const $progressBar = document.getElementById("progress-bar");
const $progressText = document.getElementById("progress-text");
const $btnSubmit = document.getElementById("btn-submit");
const $successState = document.getElementById("success-state");
const $submitSection = document.getElementById("submit-section");

const template = await getTemplate();

//Inser the log  out button in the html
try {
    insertLogOutButton();
} catch(e) {
    console.error(e);
}

console.log(template);

if(template.error) {
    showErrorInContainer(template.error.message, $serverMsgs);

    document.getElementById("form-header").classList.add("hidden");
    $form.closest('.bg-white').classList.add("hidden");
} else {
    const templateLikes = template.Likes;
    $likesNumber.textContent = templateLikes.length;

    templateLikes.forEach(templateLike => {
        //It means the user have given a like to this tempalte
        if(templateLike.UserId == userId) {
            $iconStarFill.classList.remove("hidden");
            $iconStar.classList.add("hidden");
            $btnLikeTemplate.dataset["likeAction"] = "unlike";
        }
    });

    $btnLikeTemplate.addEventListener("click", async e => {
        //Petition to give a like to this template

        if($btnLikeTemplate.dataset["likeAction"] === "like") {
            
            const likedRes = await (await fetch(`${location.origin}/template/like?userId=${userId}&templateId=${template.TemplateId}&action=like`)).json();

            if(likedRes.error) {
                showError(likedRes.error.message);
                return;
            }
            
            if(likedRes) {
                $iconStarFill.classList.remove("hidden");
                $iconStar.classList.add("hidden");
                $btnLikeTemplate.dataset["likeAction"] = "unlike";
                $likesNumber.textContent = likedRes.data;
            }
        } else if($btnLikeTemplate.dataset["likeAction"] === "unlike") {
            const unlikedRes = await (await fetch(`${location.origin}/template/like?userId=${userId}&templateId=${template.TemplateId}&action=unlike`)).json();

            if(unlikedRes.error) {
                showError(unlikedRes.error.message);
                return;
            }

            if(unlikedRes) {
                $iconStarFill.classList.add("hidden");
                $iconStar.classList.remove("hidden");
                $btnLikeTemplate.dataset["likeAction"] = "like";
                $likesNumber.textContent = unlikedRes.data;
            }
        }
    });

    //Request to get the template's likes
    setInterval( async () => {
        const likes = await (await fetch(`${location.origin}/template/likes?templateId=${template.TemplateId}`)).json();

        if(likes.data) {
            $likesNumber.textContent = likes.data.length;
        }
        
    }, 3000);

    // Build form and render questions
    buildForm($form, template, false);

    // Remove the dynamically added submit button from buildForm (we have our own)
    const dynamicSubmitBtn = $form.querySelector('button[type="submit"]');
    if(dynamicSubmitBtn) dynamicSubmitBtn.remove();

    // Apply Tailwind classes to dynamically generated question elements
    applyTailwindToQuestions();

    // Setup progress tracking
    setupProgressTracking();

    // Setup custom submit handler
    setupSubmitHandler(template);

    $formTitle.textContent = template.Title;
    $formDescription.textContent = template.Description;

    //Default value for the date input
    const $formControlDate = document.getElementById("form-date");
    const now = new Date();
    console.log(now.toISOString().slice(0, 19));

    const formattedDateTime = now.toISOString().slice(0, 19);

    $formControlDate.value = formattedDateTime;

}

/**
 * Apply Tailwind classes to dynamically generated form elements from buildForm.
 * The question type classes use Bootstrap classes; we replace them here.
 */
function applyTailwindToQuestions() {
    // Style all inputs, textareas, and selects in the form
    $form.querySelectorAll('input:not([type="hidden"]):not([type="checkbox"]):not([type="radio"]):not([disabled]), textarea, select').forEach(el => {
        el.classList.remove('form-control', 'form-select');
        el.classList.add('w-full', 'px-4', 'py-3', 'border', 'border-gray-200', 'rounded-lg', 'focus:ring-2', 'focus:ring-primary', 'focus:border-primary', 'transition-colors');
    });

    // Style checkboxes
    $form.querySelectorAll('input[type="checkbox"]').forEach(el => {
        el.classList.remove('form-check-input');
        el.classList.add('w-5', 'h-5', 'text-primary', 'rounded', 'border-gray-300', 'focus:ring-primary');
    });

    // Style radio buttons
    $form.querySelectorAll('input[type="radio"]').forEach(el => {
        el.classList.remove('form-check-input');
        el.classList.add('w-5', 'h-5', 'text-primary', 'border-gray-300', 'focus:ring-primary');
    });

    // Style labels
    $form.querySelectorAll('label').forEach(el => {
        el.classList.add('block', 'text-sm', 'font-medium', 'text-gray-700', 'mb-1');
    });

    // Style question containers (divs with data-question-type)
    $form.querySelectorAll('div[data-question-type]').forEach(el => {
        el.classList.add('p-4', 'border', 'border-gray-200', 'rounded-xl', 'bg-gray-50/50');
        // Remove old Bootstrap classes
        el.classList.remove('mt-4', 'd-flex', 'gap-3', 'align-items-center');
    });

    // Style delete buttons within question containers
    $form.querySelectorAll('div[data-question-type] > button').forEach(el => {
        if(el.textContent.toLowerCase().includes('delete')) {
            el.classList.remove('btn', 'btn-danger');
            el.classList.add('text-sm', 'px-3', 'py-1', 'text-red-600', 'hover:bg-red-50', 'rounded-lg', 'transition-colors');
        }
    });

    // Style "Edit options" buttons
    $form.querySelectorAll('button[data-bs-target="#editOptionsModal"]').forEach(el => {
        el.classList.remove('btn', 'btn-primary');
        el.classList.add('text-sm', 'px-3', 'py-1', 'text-primary', 'border', 'border-primary', 'hover:bg-primary/5', 'rounded-lg', 'transition-colors');
        // Replace Bootstrap modal toggle with custom event dispatch
        el.removeAttribute('data-bs-toggle');
        el.removeAttribute('data-bs-target');
        el.addEventListener('click', () => {
            // Open options modal via Alpine.js
            document.querySelector('[x-data]').__x.$data.optionsModalOpen = true;
        });
    });

    // Style select elements for multiple choice
    $form.querySelectorAll('select').forEach(el => {
        el.classList.remove('form-select');
        el.classList.add('w-full', 'px-4', 'py-3', 'border', 'border-gray-200', 'rounded-lg', 'focus:ring-2', 'focus:ring-primary', 'focus:border-primary');
    });
}

/**
 * Setup progress bar tracking based on answered questions.
 */
function setupProgressTracking() {
    const totalQuestions = template.Questions ? template.Questions.length : 0;
    if(totalQuestions === 0) return;

    function updateProgress() {
        let answered = 0;
        const questions = $form.querySelectorAll('div[data-question-type]');
        
        questions.forEach($q => {
            const input = $q.querySelector('input:not([type="hidden"]):not([disabled])') ||
                          $q.querySelector('textarea') ||
                          $q.querySelector('select');
            if(input) {
                if(input.type === 'checkbox') {
                    // Count checkboxes as answered if any are checked
                    const checkboxes = $q.querySelectorAll('input[type="checkbox"]');
                    if(Array.from(checkboxes).some(cb => cb.checked)) answered++;
                } else if(input.type === 'radio') {
                    const radios = $q.querySelectorAll('input[type="radio"]');
                    if(Array.from(radios).some(r => r.checked)) answered++;
                } else if(input.value && input.value.trim() !== '') {
                    answered++;
                }
            }
        });

        const percent = totalQuestions > 0 ? Math.round((answered / totalQuestions) * 100) : 0;
        $progressBar.style.width = percent + '%';
        $progressText.textContent = `${answered} of ${totalQuestions} questions answered`;
    }

    // Listen for input changes on all form elements
    $form.addEventListener('input', updateProgress);
    $form.addEventListener('change', updateProgress);
    
    // Initial update
    updateProgress();
}

/**
 * Setup custom submit handler with loading and success states.
 * This replaces buildForm's default submit handler.
 */
function setupSubmitHandler(templateData) {
    // Prevent buildForm's submit handler from firing
    $form.addEventListener("submit", async e => {
        e.stopImmediatePropagation();
        e.preventDefault();

        // Show loading state
        $btnSubmit.disabled = true;
        $btnSubmit.innerHTML = `
            <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white inline" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Submitting...`;

        // Build responses
        const date = document.getElementById("form-date").value;
        const responses = [];
        const $questions = $form.querySelectorAll('div[data-question-type]');

        $questions.forEach($question => {
            let responseString = '';
            let questionId = '';

            const input = $question.querySelector('input:not([type="hidden"]):not([disabled])');
            const textarea = $question.querySelector('textarea');
            const select = $question.querySelector('select');

            if(input) {
                if(input.type === 'checkbox') {
                    // For checkboxes, collect all checked values
                    const checked = $question.querySelectorAll('input[type="checkbox"]:checked');
                    responseString = Array.from(checked).map(cb => cb.value || cb.parentElement?.textContent?.trim() || 'on').join(', ');
                } else if(input.type === 'radio') {
                    const checked = $question.querySelector('input[type="radio"]:checked');
                    responseString = checked ? (checked.value || checked.parentElement?.textContent?.trim() || '') : '';
                } else {
                    responseString = input.value;
                }
                questionId = input.dataset["questionId"] || '';
            } else if(textarea) {
                responseString = textarea.value;
                questionId = textarea.dataset["questionId"] || '';
            } else if(select) {
                responseString = select.value;
                questionId = select.dataset["questionId"] || '';
            }

            responses.push({
                ResponseString: responseString,
                UserId: parseInt(userId),
                QuestionId: parseInt(questionId)
            });
        });

        try {
            const json = await makeRequest("response/add", {
                method: "POST",
                body: responses
            });
            console.log(json);

            const { data } = json;

            if(data) {
                // Show success state
                $form.classList.add('hidden');
                $submitSection.classList.add('hidden');
                $progressBar.parentElement.classList.add('hidden');
                $progressText.classList.add('hidden');
                $successState.classList.remove('hidden');
            } else {
                // Show error
                showErrorInContainer('Failed to submit response — please try again', $serverMsgs);
                $btnSubmit.disabled = false;
                $btnSubmit.textContent = 'Submit Response';
            }
        } catch(err) {
            console.error(err);
            showErrorInContainer('An error occurred — please try again', $serverMsgs);
            $btnSubmit.disabled = false;
            $btnSubmit.textContent = 'Submit Response';
        }
    }, true); // capture phase to fire before buildForm's handler

    // Wire up our custom submit button to trigger form submission
    $btnSubmit.addEventListener('click', () => {
        $form.requestSubmit();
    });
}
