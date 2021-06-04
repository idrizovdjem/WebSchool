const deletePost = (event, postId) => {
    const deleteConfirm = confirm('Are you sure you want to delete this post?');
    if (deleteConfirm === false) {
        return;
    }

    fetch('/apiPosts/Remove?postId=' + postId)
        .then(response => response.json())
        .then(data => showDeleteMessage(data))
        .catch(error => showDeleteMessage(false));

    const tableRow = event.currentTarget.parentNode.parentNode;
    tableRow.remove();
}

const showDeleteMessage = (isSuccessfull) => {
    if (isSuccessfull) {
        toastMessage.textContent = 'Post removed successfully!';
    } else {
        toastMessage.textContent = 'Something went wrong while removing the post!';
    }

    $('.toast').toast('show');
}