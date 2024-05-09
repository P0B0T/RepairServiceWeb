$(function () {

    // Обработчик события изменения выбранного клиента
    $(document).on('change', '#selectClient', function () {
        
        var clientId = $(this).val();   // Получение идентификатора выбранного клиента

        // Отправка GET-запроса на сервер для получения устройств выбранного клиента
        $.get("/Repairs/GetDevices", { clientId: clientId }, function (data) {
            
            var devicesSelect = $("#selectDevice"); // Получение элемента выпадающего списка устройств

            devicesSelect.empty();  // Очистка списка устройств

            // Добавление каждого устройства в список
            $.each(data, function (index, row) {
                devicesSelect.append("<option value='" + row.id + "'>" + row.model + "</option>")
            });
        });
    });
});