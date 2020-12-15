const xhttp = new XMLHttpRequest();

xhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        let data = JSON.parse(this.responseText);
        const table = document.getElementById("classesTable");
        table.innerHTML = "";
        for (let i = 0; i < data.length; i++) {
            table.innerHTML += `<tr><td>` + (i + 1) + `</td><td>` + data[i].signature + `</td><td>` + data[i].createdOn + `</td><td><a href='/Admin/Classes/Information?signature=` + data[i].signature + `' class='btn btn-primary text-white'>Edit</a></td></tr>`;
        }
    }
};

xhttp.open("GET", "/Admin/Classes/GetClasses", true);
xhttp.send();