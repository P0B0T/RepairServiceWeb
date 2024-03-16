$(function () {
    var shouldResetSelectClient = '@TempData["ResetSelectClient"]';
    if (shouldResetSelectClient === 'True') {
        $("#selectClient").val('');
    }
});