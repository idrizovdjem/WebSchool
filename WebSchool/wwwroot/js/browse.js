const groupNameInput = document.getElementById('groupNameInput');
const searchButton = document.getElementById('searchButton');
const tableBody = document.getElementById('tableBody');

searchButton.addEventListener('click', async () => {
    const groupName = groupNameInput.value.trim();
    const foundGroups = await fetchGroupsNames(groupName);
    attachGroups(foundGroups);
});

const fetchGroupsNames = async (groupName) => {
    return await fetch(`apiGroups/GetGroupsByName?groupName=${groupName}`)
        .then(response => response.json())
        .then(data => data)
        .catch(error => []);
}

const attachGroups = (groups) => {
    tableBody.innerHTML = '';

    groups.forEach((group, index) => {
        const tableRow = document.createElement('tr');

        const nameData = document.createElement('td');
        nameData.textContent = group.name;
        nameData.classList.add('h5', 'pt-2');

        const linkData = document.createElement('td');
        const requestLink = document.createElement('a');
        

        if (group.requestStatus === 'NotApplied') {
            requestLink.textContent = 'Send Request';
            requestLink.href = '/Applications/Apply?groupId=' + group.id;
            requestLink.classList.add('btn', 'btn-success', 'w-100', 'text-white');
        } else if (group.requestStatus === 'WaitingApproval') {
            requestLink.textContent = 'Waiting approval';
            requestLink.classList.add('btn', 'btn-warning', 'w-100', 'text-black');
        } else if (group.requestStatus === 'InGroup') {
            requestLink.textContent = 'Joined';
            requestLink.classList.add('btn', 'btn-primary', 'w-100', 'text-white');
        }

        linkData.appendChild(requestLink);
        tableRow.appendChild(nameData);
        tableRow.appendChild(linkData);
        tableBody.appendChild(tableRow);
    });
}