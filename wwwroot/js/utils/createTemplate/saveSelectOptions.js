export default async function saveSelectOptions() {

    //Add options to the select
    const $btnSaveOptionsChanges = document.getElementById("btn-save-options-changes");
    const $btnCloseOptionsChanges = document.getElementById("btn-close-options-changes");
    const $optionsList = document.getElementById("options-list");
    const $textAreaAddOpts = document.getElementById("textAreaAddOpts");

    $btnSaveOptionsChanges.addEventListener("click", e => {
        const $select = document.getElementById($btnSaveOptionsChanges.dataset["selectId"]);
        $select.innerHTML = "";
    
        //This variable contains all the h4 in the option list element, dont mix it up with 'option' html element
        const $previousOptions = $optionsList.querySelectorAll(".option");
        const newOpts = $textAreaAddOpts.value.split("\n").filter(opt => opt);//this filter ensure that there are not null options
        const prevOpts = Array.from($previousOptions).map($el => $el.textContent);
        const opts = [...prevOpts, ...newOpts]
    
        opts.forEach(opt => {
            const $option = document.createElement("option");
            $option.value = opt;
            $option.textContent = opt;
            
            $select.appendChild($option);
        });
    
        $textAreaAddOpts.value = "";
        $optionsList.innerHTML = null;
    });
    
    $btnCloseOptionsChanges.addEventListener("click", e => {
        $textAreaAddOpts.value = "";
        $optionsList.innerHTML = null;
    })
}