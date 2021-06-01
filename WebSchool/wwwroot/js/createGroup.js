const nameInput = document.getElementById('nameInput');
const customError = document.getElementById('customError');
const createButton = document.getElementById('createButton');

nameInput.addEventListener('input', async () => {
    const groupName = nameInput.value;
    if (groupName === '') {
        return;
    }

    if (groupName.length < 5) return;

    const isGroupNameAvailable = await checkGroupName(groupName);
    if (isGroupNameAvailable === false) {
        nameInput.style.borderColor = 'red';
        customError.textContent = 'This group name is already taken!';
        createButton.setAttribute('disabled', 'true');
    } else {
        nameInput.style.borderColor = '#5885AF';
        customError.textContent = '';
        createButton.removeAttribute('disabled');
    }
});

const checkGroupName = async (groupName) => {
    return await fetch(`/apiGroups/IsNameValid?groupName=${groupName}`)
        .then(response => response.json())
        .then(data => data)
        .catch(error => false);
}
