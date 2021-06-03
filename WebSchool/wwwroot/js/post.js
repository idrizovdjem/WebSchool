const deletePost = (event) => {
    const deleteConfirm = confirm('Are you sure you want to delete this post?');
    if (deleteConfirm === false) {
        event.preventDefault();
    }
}