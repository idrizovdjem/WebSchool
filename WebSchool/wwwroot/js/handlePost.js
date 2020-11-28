const postForm = document.getElementById("postForm");

let isCreatePostOpened = true;
function handlePost() {
    if (!isCreatePostOpened) {
        postForm.style.display = "none";
        isCreatePostOpened = true;
    }
    else {
        postForm.style.display = "block";
        isCreatePostOpened = false;
    }
}