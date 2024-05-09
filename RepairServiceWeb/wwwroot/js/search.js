// Инициализация индекса текущего элемента и массива подсвеченных элементов
var currentIndex = -1;
var highlightedItems = [];

// Функция для подсветки результатов поиска
function searchHighlight(url) {

    // Обработчик события отправки формы поиска
    $('.search').on('submit', function (event) {
        event.preventDefault(); // Предотвращение стандартного поведения формы

        var highlightedName = $('input[name="name"]').val().toLowerCase().trim(); // Получение введенного имени для подсветки

        // Если имя для подсветки указано, отправляем AJAX-запрос на сервер
        if (highlightedName) {
            $.ajax({
                url: url,
                type: 'GET',
                data: { name: highlightedName },
                success: function (data) {
                    // Если запрос успешен, обновляем массив подсвеченных элементов и индекс текущего элемента
                    if (data.success) {
                        highlightedItems = [];
                        currentIndex = -1;

                        // Перебираем все элементы с классом "divRows"
                        $('.divRows').each(function () {

                            var name = $(this).find('h3').text().toLowerCase().trim(); // Получаем имя текущего элемента и проверяем, содержит ли оно введенное имя для подсветки

                            // Если имя содержит введенное имя для подсветки, добавляем текущий элемент в массив подсвеченных элементов
                            if (name.includes(data.highlightedName)) {
                                highlightedItems.push(this);
                            }
                        });

                        // Если есть подсвеченные элементы, подсвечиваем первый элемент и показываем кнопки и счетчик
                        if (highlightedItems.length > 0) {
                            currentIndex = 0;
                            $('.divRows').removeClass('searchRes');
                            $(highlightedItems[currentIndex]).addClass('searchRes');
                            highlightedItems[currentIndex].scrollIntoView();
                            $('#upButton, #downButton, #counter').show();
                            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
                        }
                    }
                }
            });
        }
    });

    // Обработчик события ввода в поле "name"
    $('input[name="name"]').on('input', function () {

        // При вводе в поле "name" сбрасываем подсветку, массив подсвеченных элементов и индекс текущего элемента, и скрываем кнопки и счетчик
        $('.divRows').removeClass('searchRes');
        highlightedItems = [];
        currentIndex = -1;
        $('#upButton, #downButton, #counter').hide();
        $('#counter').text('');
    });

    // Обработчик события нажатия на кнопку "upButton"
    $('#upButton').on('click', function (event) {
        event.preventDefault(); // Предотвращение стандартного поведения кнопки

        // Если есть подсвеченные элементы, переходим к предыдущему элементу
        if (highlightedItems.length > 0) {
            $('.divRows').removeClass('searchRes');
            currentIndex = (currentIndex - 1 + highlightedItems.length) % highlightedItems.length;
            $(highlightedItems[currentIndex]).addClass('searchRes');
            highlightedItems[currentIndex].scrollIntoView({ behavior: "smooth" });
            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
        }
    });

    // Обработчик события нажатия на кнопку "downButton"
    $('#downButton').on('click', function (event) {
        event.preventDefault(); // Предотвращение стандартного поведения кнопки

        // Если есть подсвеченные элементы, переходим к следующему элементу
        if (highlightedItems.length > 0) {
            $('.divRows').removeClass('searchRes');
            currentIndex = (currentIndex + 1) % highlightedItems.length;
            $(highlightedItems[currentIndex]).addClass('searchRes');
            highlightedItems[currentIndex].scrollIntoView({ behavior: "smooth" });
            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
        }
    });
}

// Скрытие кнопок и счетчика при загрузке страницы
$(function () {
    $('#upButton, #downButton, #counter').hide();
});