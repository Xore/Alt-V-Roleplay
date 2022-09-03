function resizePseudo() {
    var e = document.getElementById("pseudo-box-hudlist"),
        n = window.innerHeight;
    e.style.height = n, e.style.paddingTop = .25 * n, e.style.paddingBottom = .25 * n
}

window.addEventListener("resize", resizePseudo), window.onload = function() { resizePseudo() };