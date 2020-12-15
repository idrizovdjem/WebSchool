const xhttp = new XMLHttpRequest();

xhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        let data = JSON.parse(this.responseText);
        const table = document.getElementById("linksTable");
        table.innerHTML = "";
        let counter = 1;
        for (let i = 0; i < data.length; i++) {
            let textColor = data[i].isUsed == "Yes" ? "success" : "danger";
            let disabled = data[i].isUsed == "Yes" ? "disabled" : "";
            table.innerHTML += `<tr><th scope="row">` + (counter++) + `</th><td>` + data[i].email + `</td><td>` + data[i].createdOn + `</td><td>` + data[i].role + `</td>
                <td class="text-` + textColor + `">` + data[i].isUsed + `</td>
                <td><a href='/Admin/Administration/DeleteLink?id=` + data[i].id + `' class='btn btn-danger ` + disabled + `'>Delete</a></td></tr>`;
        }
    }
};

xhttp.open("GET", "/Admin/Administration/GetLinks", true);
xhttp.send();