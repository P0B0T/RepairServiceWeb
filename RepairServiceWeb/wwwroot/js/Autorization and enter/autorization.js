$(function () {
    // Обработчик события отправки формы авторизации
    $("#autorizationForm").on("submit", function (event) {
        event.preventDefault();     // Предотвращение стандартного поведения формы

        // Отправка данных формы на сервер
        $.post('/Autorization/Enter', $(this).serialize(), (data) => {

            // Установка куки после успешной авторизации
            setCookie('auth_key', data.auth_key);
            setCookie('permissions', data.permissions);
            setCookie('userId', data.userId);
            setCookie('login', data.login);
            setCookie('password', data.password);

            window.location.href = '/PersonalCabinet/PersonalCabinet'   // Перенаправление пользователя в личный кабинет
        }).fail((error) => {
            $("#error-message").text(error.responseText);   // Вывод сообщения об ошибке
        });
    });
});

// Функция для установки куки
function setCookie(name, val) {
    const date = new Date();
    const value = val;

    date.setTime(date.getTime() + (7 * 24 * 60 * 60 * 1000));   // Установка времени истечения куки через 7 дней

    document.cookie = name + "=" + value + "; expires=" + date.toUTCString() + "; path=/";  // Установка куки
}