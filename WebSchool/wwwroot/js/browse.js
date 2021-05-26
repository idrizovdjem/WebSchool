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
        console.log(group);
        const tableRow = document.createElement('tr');
        const rowClass = index % 2 == 0 ? "event-row" : "odd-row";
        tableRow.classList.add(rowClass);

        const nameData = document.createElement('td');
        nameData.classList.add('group-name-col');
        nameData.textContent = group.name;

        const linkData = document.createElement('td');
        const requestLink = document.createElement('a');
        requestLink.classList.add('group-join-button');

        if (group.requestStatus === 'NotApplied') {
            requestLink.classList.add('not-joined');
            requestLink.textContent = 'Send Request';
            requestLink.href = '/Applications/Apply?groupId=' + group.id;
        } else if (group.requestStatus === 'WaitingApproval') {
            requestLink.classList.add('waiting-approval');
            requestLink.textContent = 'Waiting approval';
        } else if (group.requestStatus === 'InGroup') {
            requestLink.classList.add('joined');
            requestLink.textContent = 'Joined';
        }

        linkData.appendChild(requestLink);
        tableRow.appendChild(nameData);
        tableRow.appendChild(linkData);
        tableBody.appendChild(tableRow);
    });
}