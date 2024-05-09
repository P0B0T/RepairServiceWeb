$(function () {

    var permissionId = getCookie('permissions');    // Получение значения куки 'permissions'

    // Отправка GET-запроса на сервер для получения имени роли пользователя
    $.get('/Autorization/GetRoleName', { permissionId: permissionId }, function (response) {

        // Если запрос неуспешен, прекращаем выполнение функции
        if (response.success === false) {
            return;
        }

        var permissionValue = response.data.toLowerCase();  // Преобразование значения роли пользователя в нижний регистр

        // Проверка роли пользователя и отображение соответствующих элементов интерфейса
        if (permissionValue.includes('admin') || permissionValue.includes('админ')) {

            // Если пользователь - администратор, показываем все элементы управления
            $('.navbar-nav').show();
            $('.buttonAdd').show();
            $('#addDevicesButton').show();
            $('.buttonInf').show();
            $('.elemInf').show();
            $('.staffInf').show();
            $('.confInf').show();
            $('#workersEl').show();
            $('.theme').show();
        }
        else if (permissionValue.includes('ресепшен') || permissionValue.includes('reception')) {

            // Если пользователь - ресепшен, показываем определенные элементы управления
            $("#staffCssLink").attr("disabled", false);
            $('.navbar-nav').show();
            $('.elemInf').show();
            $('#addDevicesButton').show();
        }
        else if (permissionValue.includes('human resources') || permissionValue.includes('отдел кадров')) {

            // Если пользователь - отдел кадров, показываем определенные элементы управления
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/staff.css');
            $('.navbar-nav').show();
            $('.staffInf').show();
            $('.buttonAdd').show();
            $('#addDevicesButton').show();
            $('.buttonInf').show();
        }
        else if (permissionValue.includes('staff') || permissionValue.includes('сотрудник')) {

            // Если пользователь - сотрудник, показываем определенные элементы управления
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/staff.css');
            $('#workersEl').show();
            $('#infAccessories').show();
            $('#infDevice').show();
        }
        else {
            $("#clientCssLink").attr("disabled", false);    // Если пользователь - клиент, показываем определенные элементы управления
        }
    })
});