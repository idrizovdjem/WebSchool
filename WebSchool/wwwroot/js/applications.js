Array.from(document.getElementsByClassName('role-select')).forEach(select => {
    const parentNode = select.parentElement;
    const roleField = parentNode.nextElementSibling.children[0].children[2];

    select.addEventListener('change', (event) => {
        const role = event.target.value;
        roleField.setAttribute('value', role);
    });
});
