const tableBody = document.getElementById('tableBody');
const groupNameInput = document.getElementById('groupNameInput');

window.onload = () => fetchMostPopularGroups();

groupNameInput.oninput = (event) => {
    const groupName = event.target.value.trim();
    if (groupName === '') {
        fetchMostPopularGroups();
        return;
    }

    fetch('/apiGroups/GetGroupsByName?groupName=' + groupName)
        .then(response => response.json())
        .then(data => attachGroups(data))
        .catch(error => attachGroups([]));
}

const fetchMostPopularGroups = () => {
    fetch('/apiGroups/GetMostPopular')
        .then(response => response.json())
        .then(data => attachGroups(data))
        .catch(error => attachGroups([]));
}

const attachGroups = (groups) => {
    tableBody.innerHTML = '';

    for (const group of groups) {
        const row = document.createElement('tr');
        const name = document.createElement('td');
        name.classList.add('align-middle');
        name.textContent = group.name;

        const buttonData = document.createElement('td');
        const button = document.createElement('button');

        if (group.requestStatus === "NotApplied") {
            button.classList.add('btn', 'btn-primary', 'w-100');
            button.textContent = 'Join';
            button.addEventListener('click', (event) => applyToGroup(event, group.id));
        } else if (group.requestStatus === "WaitingApproval") {
            button.classList.add('btn', 'btn-warning', 'w-100');
            button.textContent = 'Waiting approval';
        } else if (group.requestStatus === 'InGroup') {
            button.classList.add('btn', 'btn-success', 'w-100');
            button.textContent = 'In group';
        }

        buttonData.appendChild(button);

        row.appendChild(name);
        row.appendChild(buttonData);
        tableBody.appendChild(row);
    }
}

const applyToGroup = (event, groupId) => {
    fetch('/apiApplications/Apply?groupId=' + groupId);
    event.target.classList.remove('btn-primary');
    event.target.classList.add('btn-warning');
    event.target.textContent = 'Waiting approval';
}