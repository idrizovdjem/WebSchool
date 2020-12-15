const select = document.getElementById('classesSelect');
const xhttp = new XMLHttpRequest();

xhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        let data = JSON.parse(this.responseText);
        console.log(data);
        for (let i = 0; i < data.length; i++) {
            select.innerHTML += `<option value='` + data[i].signature + `'>` + data[i].signature + `</option>`;
        }
    }
}

xhttp.open("GET", "/Teacher/Classes/GetClasses", true);
xhttp.send();