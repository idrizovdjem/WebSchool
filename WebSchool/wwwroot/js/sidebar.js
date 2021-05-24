const groupsContainer = document.getElementById('groupsContainer');
const groupsSection = document.getElementById('groupsSection');

groupsSection.addEventListener('click', () => {
    const groupsVisibility = groupsContainer.style.display;
    const newVisibility = groupsVisibility === 'block' ? 'none' : 'block';
    groupsContainer.style.display = newVisibility;
});

const fetchGroups = () => {
    fetch('/apiGroups/GetUserGroups')
        .then(response => response.json())
        .then(data => addGroups(data))
        .catch(error => console.log(error));
}

const addGroups = (groupNames) => {
    groupNames.forEach(groupName => {
        const groupLabelElement = document.createElement('p');
        groupLabelElement.innerHTML = `<a href="/Groups/Index?groupName=${groupName}">${groupName}</a>`;
        groupLabelElement.classList.add('group-element');
        groupsContainer.appendChild(groupLabelElement);

        groupLabelElement.addEventListener('click', (event) => {
            event.stopImmediatePropagation();
        });
    });
}

window.onload = () => {
    fetchGroups();
}