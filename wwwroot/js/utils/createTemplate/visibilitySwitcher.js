export default function toggleVisibilityTemplate(defaultValue) {
    const $visbilitySelect = document.getElementById("template-visibility-switch");
    
    $visbilitySelect.querySelectorAll("option").forEach($option => {
        if(parseInt($option.value) === Number(defaultValue)) $option.selected = true;
    });

    
    const $btnAddAllowedUsers = document.getElementById("btn-add-allowed-users");

    //This means the template is private
    if(!defaultValue) $btnAddAllowedUsers.style.display = "inline-block";
    
    $visbilitySelect.addEventListener("change", e => {
        const currentState = parseInt($visbilitySelect.value);

        //It means it is public
        if(currentState === 1) {
            $btnAddAllowedUsers.style.display = "none";        
        } else {
            $btnAddAllowedUsers.style.display = "inline-block";
        }
    });
}