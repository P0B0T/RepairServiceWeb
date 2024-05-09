// Получение сообщения об успешном выполнении операции из TempData
var successMessage = '@TempData["Successfully"]';

// Если сообщение об успешном выполнении операции существует
if (successMessage) {
    
    var overlay = document.getElementById('overlay'); // Получение элемента overlay по его идентификатору

    overlay.style.display = 'flex'; // Установка стиля display элемента overlay в значение 'flex', что делает его видимым на странице

    // Установка таймера, который через 3 секунды устанавливает стиль display элемента overlay в значение 'none', что делает его невидимым на странице
    setTimeout(function () {
        overlay.style.display = 'none';
    }, 3000);
}