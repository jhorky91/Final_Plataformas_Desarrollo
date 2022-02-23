
var modal = document.getElementById("myModalEmergente");

var span = document.getElementsByClassName("closeEmergente")[0];

span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}