function validateSubmit() {
    const password = document.getElementsByName("password")[0].value;
    const confirmedPassword = document.getElementsByName("confirm")[0].value;
    
    if (password !== confirmedPassword) {
        alert("Lozinke se ne poklapaju!");
        return false;
    }
    
    const jmbg = document.getElementsByName("jmbg")[0].value;

    if (jmbg.length !== 13) {
        alert("JMBG mora imati 13 karaktera!");
        return false;
    } else {
        if (isNaN(parseInt(jmbg, 10))) {
            alert("Uneti JMBG nije broj!");
            return false;
        }
    }
    
    return true;
}

window.onload = () => {
    if (document.URL.includes("index.html?username=")) {
        
        alert("Korisnik je uspesno dodat!");
        const data = document.URL.split("index.html?")[1];
        
        const username = data.split('&')[0].split("=")[1];
        const password = data.split('&')[1].split("=")[1];
        const confirmedPassword = data.split('&')[2].split("=")[1];
        const firstName = data.split('&')[3].split("=")[1];
        const lastName = data.split('&')[4].split("=")[1];
        const jmbg = data.split('&')[5].split("=")[1];

        let htmlCode = "";

        htmlCode += "<h1 style='color: green'>Lista korisnika</h1>";
        htmlCode += "<table border='black'>";
        htmlCode += "<tr> <th>Korisnicko ime</th> <th>Ime</th> <th>Prezime</th> <th>JMBG</th></tr>";
        htmlCode += "<tr> <td>user1</td> <td>ime1</td> <td>prezime1</td> <td>jmbg1</td></tr>";
        htmlCode += "<tr> <td>user2</td> <td>ime2</td> <td>prezime2</td> <td>jmbg2</td></tr>";
        htmlCode += `<tr> <td>${username}</td> <td>${firstName}</td> <td>${lastName}</td> <td>${jmbg}</td></tr>`;
        htmlCode += "</table>";

        document.body.innerHTML = htmlCode;

    }
}