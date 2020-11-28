let isOpened = false;

/* Set the width of the side navigation to 250px */
function openNav() {
    if (!isOpened) {
        document.getElementById("mySidenav").style.width = "250px";
        isOpened = true;
    }
    else {
        document.getElementById("mySidenav").style.width = "0";
        isOpened = false;
    }
}

/* Set the width of the side navigation to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    isOpened = false;
}