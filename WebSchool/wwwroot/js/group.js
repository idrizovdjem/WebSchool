const toastMessage = document.getElementById('toastMessage');

const deletePost = (event, postId) => {
    event.preventDefault();

    const deleteConfirm = confirm('Are you sure you want to delete this post ?');
    if (deleteConfirm === false) {
        return;
    }

    const postElement = event.currentTarget.parentNode.parentNode;
    const postLine = postElement.nextElementSibling;

    fetch('/apiPosts/Remove?postId=' + postId)
        .then(response => response.json())
        .then(data => showDeleteMessage(data))
        .catch(error => showDeleteMessage(false));

    postElement.remove();
    postLine.remove();
}

const showDeleteMessage = (isSuccessfull) => {
    if (isSuccessfull) {
        toastMessage.textContent = 'Post removed successfully!';
    } else {
        toastMessage.textContent = 'Something went wrong while removing the post!';
    }

    $('.toast').toast('show');
}