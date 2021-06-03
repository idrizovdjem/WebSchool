const toastMessage = document.getElementById('toastMessage');
const deletePostButtons = Array.from(document.getElementsByClassName('delete-post'));

deletePostButtons.forEach(button => {
    button.addEventListener('click', (event) => {
        event.preventDefault();

        const deleteConfirm = confirm('Are you sure you want to delete this post ?');
        if (deleteConfirm === false) {
            return;
        }

        const postElement = event.currentTarget.parentNode.parentNode.parentNode;
        const postIdInput = event.target.parentNode.parentNode.getElementsByClassName('post-id')[0];
        const postId = postIdInput.value;
        const postLine = postElement.nextElementSibling;

        fetch('/apiPosts/Remove?postId=' + postId)
            .then(response => response.json())
            .then(data => showDeleteMessage(data))
            .catch(error => showDeleteMessage(false));

        postElement.remove();
        postLine.remove();
    })
});

const showDeleteMessage = (isSuccessfull) => {
    if (isSuccessfull) {
        toastMessage.textContent = 'Post removed successfully!';
    } else {
        toastMessage.textContent = 'Something went wrong while removing the post!';
    }

    $('.toast').toast('show');
}