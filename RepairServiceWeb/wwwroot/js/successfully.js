var successMessage = '@TempData["Successfully"]';
if (successMessage) {
    var overlay = document.getElementById('overlay');

    overlay.style.display = 'flex';
    setTimeout(function () {
        overlay.style.display = 'none';
    }, 3000);
}