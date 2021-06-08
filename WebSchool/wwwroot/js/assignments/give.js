window.onload = () => {
    fetch('/apiAssignments/All')
        .then(response => response.json())
        .then(data => attachAssignments(data))
        .catch(() => attachAssignments([]));
}

const attachAssignments = (assignments) => {
    const assignmentsSelect = document.getElementById('assignmentsSelect');
    assignmentsSelect.innerHTML = '';
    assignments.forEach(assignment => {
        const optionElement = document.createElement('option');
        optionElement.textContent = assignment.title;
        optionElement.value = assignment.id;

        assignmentsSelect.appendChild(optionElement);
    });
}