const toastMessage = document.getElementById('toastMessage');

const deletePost = (event) => {
    const deleteConfirm = confirm('Are you sure you want to delete this post?');
    if (deleteConfirm === false) {
        event.preventDefault();
    }
}

const deleteComment = (event, commentId) => {
    const deleteConfirm = confirm('Are you sure you want to delete this comment?');
    if (deleteConfirm === false) {
        return;
    }

    fetch('/apiComments/Remove?commentId=' + commentId)
        .then(response => response.json())
        .then(data => showDeleteMessage(data))
        .catch(error => showDeleteMessage(false));

    const commentElement = event.currentTarget.parentNode.parentNode;
    const commentLine = commentElement.nextElementSibling;

    commentElement.remove();
    commentLine.remove();
}

const showDeleteMessage = (isSuccessfull) => {
    if (isSuccessfull) {
        toastMessage.textContent = 'Comment removed successfully!';
    } else {
        toastMessage.textContent = 'Something went wrong while removing the comment!';
    }

    $('.toast').toast('show');
}