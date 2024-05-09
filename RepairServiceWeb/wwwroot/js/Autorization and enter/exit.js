$(function () {

    // Обработчик события клика на кнопку выхода
    $("#exitButton").click(function () {

        // Удаление куки при выходе из системы
        deleteCookie("auth_key");
        deleteCookie("permissions");
        deleteCookie("userId");
        deleteCookie("login");
        deleteCookie("password");

        localStorage.clear();   // Очистка локального хранилища
    });
});

// Функция для удаления куки
function deleteCookie(name) {

    const date = new Date();

    date.setTime(date.getTime() + (-1 * 24 * 60 * 60 * 1000));  // Установка времени истечения куки на прошедшую дату

    document.cookie = name + "=; expires=" + date.toUTCString() + "; path=/";   // Удаление куки
}