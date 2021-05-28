Array.from(document.getElementsByClassName('role-select')).forEach(select => {
    const parentNode = select.parentElement;
    const roleField = parentNode.nextElementSibling.children[0].children[2];

    select.addEventListener('change', (event) => {
        const role = event.target.value;
        roleField.setAttribute('value', role);
    });
});


const forms = Array.from(document.getElementsByClassName('application-form'));

Array.from(document.getElementsByClassName('approve-button')).forEach((button, index) => {
    button.addEventListener('click', (event) => applicationButtonHandler(event, index, '/apiApplications/Approve'));
});

Array.from(document.getElementsByClassName('decline-button')).forEach((button, index) => {
    button.addEventListener('click', (event) => applicationButtonHandler(event, index, '/apiApplications/Decline'));
});

const applicationButtonHandler = (event, index, url) => {
    event.preventDefault();

    const formData = new FormData(forms[index]);
    const groupId = formData.get('groupId');
    const applicantId = formData.get('applicantId');
    const role = formData.get('role');

    fetch(url, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ groupId, applicantId, role })
    })
        .then(response => {
            if (response.status === 200) {
                const rowElement = forms[index].parentElement.parentElement;
                rowElement.remove();
            } else {
                console.log('Something went wrong')
            }
        })
        .catch(error => console.log('Something went wrong'));
}