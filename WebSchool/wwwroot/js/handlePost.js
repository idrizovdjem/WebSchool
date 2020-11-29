const postForm = document.getElementById("postForm");
const addButton = document.getElementById("addButton");

let isCreatePostOpened = true;
function handlePost() {
    if (!isCreatePostOpened) {
        postForm.style.display = "none";
        isCreatePostOpened = true;
        addButton.innerText = "Add new post";
    }
    else {
        postForm.style.display = "block";
        isCreatePostOpened = false;
        addButton.innerText = "Close form";
    }
}

const comments = document.getElementById("commentSection");

$("#post1, #post2, #post3, #post4, #post5, #post6, #post7, #post8, #post9, #post10").click(function (event) {
    const hiddenValue = $(this).find("#hidden").val();
    const formHidden = document.getElementById("formHidden");
    formHidden.value = hiddenValue;

    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let data = JSON.parse(this.responseText);
            comments.innerHTML = "";
            if (data.length == 0) {
                comments.innerHTML = "<p>Be the first one to write a comment ;)</p>";
                return;
            }

            for (let i = 0; i < data.length; i++) {
                comments.innerHTML += `<li class="media mt-3">` + `<img src='https://i.pinimg.com/originals/0c/3b/3a/0c3b3adb1a7530892e55ef36d3be6cb8.png' class='mr-3'  width='64' height='64'/>` + `<div class="media-body"><h5 class="mt-0">` + data[i].creator + `</h5><p>` + data[i].content + `</p></div></li>`;
            }
        }
    };
    xhttp.open("GET", '/Comment/GetComments?postId=' + hiddenValue, true);
    xhttp.send();
});

$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})