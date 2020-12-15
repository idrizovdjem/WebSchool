﻿let addedEmails = [];
const suggestBox = document.getElementById('suggestBox');
const searchBar = document.getElementById("searchBar");
const addedBox = document.getElementById('addedBox');
const form = document.getElementById('addStudentsForm');
const urlParams = new URLSearchParams(window.location.search);
const xhttp = new XMLHttpRequest();
form.innerHTML += `<input type="hidden" value="` + urlParams.get('signature') + `" name='signature' />`;

searchBar.addEventListener('input', (event) => {
	let email = event.target.value;
	xhttp.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			let data = JSON.parse(this.responseText);
			suggestBox.innerHTML = "";
			for (let i = 0; i < data.length; i++) {
				if (addedEmails.includes(data[i])) {
					continue;
				}
				suggestBox.innerHTML += `<a href="#" class="list-group-item list-group-item-action" onclick='addStudent("` + data[i] + `")'>` + data[i] + `</a>`;
			}
		}
	}

	xhttp.open("GET", "/Users/GetStudentsWithEmail?email=" + email, true);
	xhttp.send();
});

function addStudent(email) {
	if (addedEmails.includes(email)) {
		return;
	}
	addedEmails.push(email);
	searchBar.value = "";
	suggestBox.innerHTML = "";

	updateStudents();
}

function removeStudent(email) {
	let index = addedEmails.indexOf(email);
	addedEmails.splice(index, 1);

	updateStudents();
}

function updateStudents() {
	addedBox.innerHTML = "";
	let token = document.getElementsByName('__RequestVerificationToken');
	form.innerHTML = `<input type="hidden" value="` + urlParams.get('signature') + `" name='signature' />`;
	let tokenValue = token[0].value;
	form.innerHTML += `<input name='__RequestVerificationToken' type='hidden' value="` + tokenValue + `" />`;

	for (let i = 0; i < addedEmails.length; i++) {
		addedBox.innerHTML += `<a href="#" class="list-group-item list-group-item-action" onclick='removeStudent("` + addedEmails[i] + `")'>` + addedEmails[i] + `</a>`;
		form.innerHTML += `<input type='hidden' value="` + addedEmails[i] + `" name="emails" />`;
	}

	form.innerHTML += `<button class="btn btn-primary w-100" type="submit">Add students</button>`;
}
