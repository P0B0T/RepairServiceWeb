// Функция для открытия модального окна с информацией
function openModal(parameters) {

    // Получение данных
    const id = parameters.data;
    const url = parameters.url;
    const title = parameters.title;
    const modal = $('#modal');

    // Отправка AJAX-запроса на сервер
    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id },
        success: function (response) {

            // Если запрос успешен, обновляем содержимое модального окна и открываем его
            modal.find(".modal-title").text(title);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        }
    });
}

// Функция для открытия модального окна подтверждения
function openConfirm(parameters) {

    // Получение данных
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#confirmDelete');
    const parentModal = $('#modal');

    // Отправка AJAX-запроса на сервер
    $.ajax({
        type: 'GET',
        data: { "id": id },
        success: function (responce) {
            modal.modal('show') // Если запрос успешен, открываем модальное окно подтверждения
        }
    })

    // Обработчик события нажатия на кнопку "Да"
    $(function () {
        modal.find('#buttonYes').click(function () {

            // Отправка AJAX-запроса на сервер для удаления элемента
            $.ajax({
                url: url,
                type: 'POST',
                data: { id: id },
                success: function (response) {

                    // Если запрос успешен, закрываем оба модальных окна и обновляем страницу
                    modal.modal('hide'); 
                    parentModal.modal('hide'); 
                    location.reload();
                }
            });
        });
    });
}

// Функция для открытия модального окна авторизации
function openAutorization() {

    // Получение данных из куки
    var userId = getCookie('userId');
    var login = getCookie('login');
    var password = getCookie('password');

    // Если пользователь уже авторизован, перенаправляем его в личный кабинет
    if (userId) {
        window.location.href = '/PersonalCabinet/PersonalCabinet'
    } else {
        // Иначе
        const modal = $('#autorization'); // Получение элемента модального окна авторизации

        // Отправка AJAX-запроса на сервер
        $.ajax({
            type: 'GET',
            success: function (response) {
                modal.modal('show'); // Если запрос успешен, открываем модальное окна авторизации
            }
        });
    }
}