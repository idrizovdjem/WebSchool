const changeNameButton = document.getElementById('changeNameButton');
const groupNameInput = document.getElementById('groupNameInput');
const errorMessagesElement = document.getElementById('errorMessages');
const changeNameForm = document.getElementById('changeNameForm');

changeNameButton.addEventListener('click', async (event) => {
    event.preventDefault();

    const groupName = groupNameInput.value.trim();
    if (groupName.length < 5 || groupName.length > 250) {
        errorMessagesElement.textContent = 'Group name must be between 5 and 250 symbols long';
        return;
    }

    const validationResult = await fetch('/apiGroups/IsNameValid?groupName=' + groupName)
        .then(response => response.json())
        .then(data => data)
        .catch(error => false);

    if (validationResult === false) {
        errorMessagesElement.textContent = 'This group name is already taken';
        return;
    }

    changeNameForm.submit();
});