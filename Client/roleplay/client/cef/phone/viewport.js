function resizePhone() {
    var e = document.getElementById("Smartphone-Box"),
        n = 700,
        t = 345;
    window.innerHeight > 1080 && (t = .5 * (n = .65 * window.innerHeight)), e.style.height = n + "px", e.style.width = t + "px"
}

function getViewportPhoneMarginBot() { var e = document.getElementById("Smartphone-Box").offsetHeight; return height = .85 * e, -height }
window.addEventListener("resize", resizePhone), window.onload = function() { resizePhone() };