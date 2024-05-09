// Функция для корректировки отступа кнопки
function adjustButtonMargin() {

    var navbarHeight = $('.navbar').height();   // Получение высоты навигационной панели


    $('#exitButton').css('margin-top', navbarHeight + 20);  // Установка верхнего отступа для кнопки выхода, равного высоте навигационной панели плюс 20 пикселей
}


$(document).ready(adjustButtonMargin);  // Вызов функции корректировки отступа при загрузке документа

$(window).on('resize', adjustButtonMargin); // Вызов функции корректировки отступа при изменении размера окна