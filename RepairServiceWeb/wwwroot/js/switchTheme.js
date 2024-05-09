// Функция, которая выполняется при загрузке страницы
$(function () {
    
    var savedTheme = localStorage.getItem('theme'); // Получение сохраненной темы из локального хранилища

    // Если сохраненная тема существует, применяем ее
    if (savedTheme) {
        switchTheme(savedTheme);
    }

    // Обработчик события клика на кнопку смены темы
    $(".btnTheme").click(function () {
        
        var theme = $(this).text(); // Получение названия темы из текста кнопки

        switchTheme(theme); // Применение выбранной темы

        localStorage.setItem('theme', theme); // Сохранение выбранной темы в локальное хранилище
    });

    // Функция для смены темы
    function switchTheme(theme) {

        // В зависимости от выбранной темы включаем или отключаем соответствующие CSS-файлы
        switch (theme) {
            case "Стандартная":

                // Для стандартной темы отключаем все дополнительные CSS-файлы
                $("#clientCssLink").attr("disabled", true);
                $("#staffCssLink").attr("disabled", true);
                break;
            case "Клиент":

                // Для темы "Клиент" включаем CSS-файл для клиентов и отключаем CSS-файл для сотрудников
                $("#clientCssLink").attr("disabled", false);
                $("#staffCssLink").attr("disabled", true);
                break;
            case "Сотрудник":

                // Для темы "Сотрудник" включаем CSS-файл для сотрудников и отключаем CSS-файл для клиентов
                $("#clientCssLink").attr("disabled", true);
                $("#staffCssLink").attr("disabled", false);
                break;
        }
    }
});