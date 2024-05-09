// Функция для получения значения куки по имени
function getCookie(name) {

    var nameEQ = name + "=";    // Добавление знака равенства к имени куки

    var ca = document.cookie.split(';');    // Разделение всех куки по символу ';'

    // Перебор всех куки
    for (var i = 0; i < ca.length; i++) {

        var c = ca[i];

        while (c.charAt(0) == ' ') c = c.substring(1, c.length);    // Удаление пробелов в начале куки

        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);    // Если имя куки совпадает с искомым, возвращаем значение куки
    }

    return null;    // Если куки с таким именем не найдено, возвращаем null
}