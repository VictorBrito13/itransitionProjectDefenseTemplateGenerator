import insertLogOutButton from "../../UI/components/btnLogOut.js";
import { buildForm, getTemplate } from "../utils/buildForm.js";

const $form = document.getElementById("form-questions");
const $formTitle = document.getElementById("form-title");
const $formDescription = document.getElementById("form-description");
const $btnLikeTemplate = document.getElementById("btn-like-template");
const $iconStar = document.getElementById("icon-star");
const $iconStarFill = document.getElementById("icon-star-fill");
const userId = document.getElementById("form-user-email").dataset["userid"];
const $likesNumber = document.getElementById("likes-number");

const template = await getTemplate();

//Inser the log  out button in the html
try {
    insertLogOutButton();
} catch(e) {
    console.error(e);
}

console.log(template);

const templateLikes = template.Likes;
$likesNumber.textContent = templateLikes.length;

console.log(templateLikes);

templateLikes.forEach(templateLike => {
    //It means the user have given a like to this tempalte
    if(templateLike.UserId == userId) {
        $iconStarFill.style.display = "block";
        $iconStar.style.display = "none";
        $btnLikeTemplate.dataset["likeAction"] = "unlike";
    }
});

$btnLikeTemplate.addEventListener("click", async e => {
    //Petition to give a like to this template
    console.log(userId);
    console.log(template.TemplateId);

    if($btnLikeTemplate.dataset["likeAction"] === "like") {
        
        const likedRes = await (await fetch(`${location.origin}/template/like?userId=${userId}&templateId=${template.TemplateId}&action=like`)).json();
        console.log(likedRes);
        
        if(likedRes) {
            $iconStarFill.style.display = "block";
            $iconStar.style.display = "none";
            $btnLikeTemplate.dataset["likeAction"] = "unlike";
            $likesNumber.textContent = likedRes.data;
        }
    } else if($btnLikeTemplate.dataset["likeAction"] === "unlike") {
        const unlikedRes = await (await fetch(`${location.origin}/template/like?userId=${userId}&templateId=${template.TemplateId}&action=unlike`)).json();
        console.log(unlikedRes);

        if(unlikedRes) {
            $iconStarFill.style.display = "none";
            $iconStar.style.display = "block";
            $btnLikeTemplate.dataset["likeAction"] = "like";
            $likesNumber.textContent = unlikedRes.data;
            console.log(unlikedRes)
        }
    }
});

//Request to get the template's likes
setInterval( async () => {
    const likes = await (await fetch(`${location.origin}/template/likes?templateId=${template.TemplateId}`)).json();

    if(likes.data) {
        $likesNumber.textContent = likes.data.length;
    }

    console.log(likes.data);
    
}, 3000);

buildForm($form, template, false);

$formTitle.textContent = template.Title;
$formDescription.textContent = template.Description;

//Default value for the date intput
const $formControlDate = document.getElementById("form-date");
const now = new Date();
console.log(now.toISOString().slice(0, 19));

const formattedDateTime = now.toISOString().slice(0, 19);

$formControlDate.value = formattedDateTime;
