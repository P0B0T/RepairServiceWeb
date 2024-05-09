$(function () {

    var shouldResetSelectClient = '@TempData["ResetSelectClient"]'; // Получение значения TempData["ResetSelectClient"] из сервера

    // Если значение TempData["ResetSelectClient"] равно 'True', сбрасываем выбранного клиента
    if (shouldResetSelectClient === 'True') {
        $("#selectClient").val('');
    }
});