var userId = getCookie('userId');
var login = getCookie('login');
var password = getCookie('password');

$('.myRepairsButton').click(function (e) {
    e.preventDefault();

    $.ajax({
        url: '/PersonalCabinet/GetRepairs',
        method: 'GET',
        data: {
            userId: userId,
            login: login,
            password: password
        },
        success: function (data) {
            if (data.success) {
                $('#divOutput').empty();
                var arr = [];

                if (data.filteredData == null) {
                    arr.push('<p>Ничего нет (</p>')
                } else {
                    data.filteredData.forEach(function (item) {
                        arr.push('<div class="divRows counter">');
                        arr.push('<div style="width: 20em">');
                        arr.push(`<h3>${item.device.model}</h3>`);
                        arr.push('<label><b>Сотрудник:</b></label>');
                        arr.push(`<label>${item.staff.fullName}</label> <br />`);
                        arr.push('</div>');
                        arr.push('<div style="margin-top: 2em; width: 15em">');
                        arr.push('<label><b>Дата поступления:</b></label>');
                        var date = new Date(item.dateOfAdmission);
                        var formattedDate = date.toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });
                        arr.push(`<label>${formattedDate}</label> <br />`);
                        arr.push('<label><b>Дата завершения:</b></label>');
                        var date = new Date(item.endDate);
                        var formattedDate = date.toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });
                        arr.push(`<label>${formattedDate}</label> <br />`);
                        arr.push('<label><b>Цена:</b></label>');
                        arr.push(`<label>${item.cost} руб.</label> <br />`);
                        arr.push('</div>');
                        arr.push('<div style="margin-top: 2em; width: 40em">');
                        arr.push('<label><b>Описание проблемы:</b></label>');
                        arr.push(`<label>${item.descriptionOfProblem}</label> <br />`);
                        arr.push('<label><b>Описание проделанной работы:</b></label>');
                        arr.push(`<label>${item.descriprionOfWorkDone}</label> <br />`);
                        arr.push('</div>');
                        arr.push('<div>');
                        arr.push('</div>');
                        arr.push('</div>');
                        arr.push('<hr />');
                    });
                }
                $('#divOutput').append(arr.join(' '));
            }
        }
    });
});

$('#myDevicesButton').click(function (e) {
    e.preventDefault();

    $.ajax({
        url: '/PersonalCabinet/GetDevices',
        method: 'GET',
        data: {
            userId: userId,
            login: login,
            password: password
        },
        success: function (data) {
            if (data.success) {
                $('#divOutput').empty();
                var arr = [];

                if (data.filteredData == null) {
                    arr.push('<p>Ничего нет (</p>')
                } else {
                    data.filteredData.forEach(function (item) {
                        arr.push('<div class="divRows counter">');
                        arr.push('<div style="width: 27em">');
                        if (item.photo != null) {
                            arr.push(`<img src="/images/DevicesPhoto/${item.photo}">`);
                        }
                        arr.push('</div>');
                        arr.push('<div>');
                        arr.push(`<h3>${item.model}</h3>`);
                        arr.push('<label><b>Производитель:</b></label>');
                        arr.push(`<label>${item.manufacturer}</label> <br />`);
                        arr.push('<label><b>Тип:</b></label>');
                        arr.push(`<label>${item.type}</label> <br />`);
                        arr.push('<label><b>Год производства:</b></label>');
                        arr.push(`<label>${item.yearOfRelease}</label> <br />`);
                        if (item.serialNumber != null) {
                            arr.push('<label><b>Серийный номер:</b></label>');
                            arr.push(`<label>${item.serialNumber}</label> <br />`);
                        }
                        arr.push('<br />');
                        arr.push('</div>');
                        arr.push('<div>');
                        arr.push('</div>');
                        arr.push('</div>');
                        arr.push('<hr />');
                    });
                }
                $('#divOutput').append(arr.join(' '));
            }
        }
    });
});

$('#myOrderAccessories').click(function (e) {
    e.preventDefault();

    $.ajax({
        url: '/PersonalCabinet/GetOrderAccessories',
        method: 'GET',
        data: {
            userId: userId,
            login: login,
            password: password
        },
        success: function (data) {
            if (data.success) {
                $('#divOutput').empty();
                var arr = [];

                if (data.filteredData == null) {
                    arr.push('<p>Ничего нет (</p>')
                } else {
                    data.filteredData.forEach(function (item) {
                        arr.push(`<h3>${item.accessories.name}</h3>`);
                        arr.push('<div class="divRows counter">');
                        arr.push('<div style="width: 12em">');
                        arr.push('<label><b>Количество:</b></label>');
                        arr.push(`<label>${item.count}</label> <br />`);
                        arr.push('<label><b>Цена:</b></label>');
                        arr.push(`<label>${item.cost} руб.</label> <br />`);
                        arr.push('<label><b>Дата заказа:</b></label>');
                        var date = new Date(item.dateOrder);
                        var formattedDate = date.toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });
                        arr.push(`<label>${formattedDate}</label> <br />`);
                        arr.push('<label><b>Статус:</b></label>');
                        arr.push(`<label>${item.statusOrder}</label>`);
                        arr.push('</div>');
                        arr.push('<div>');
                        arr.push('</div>');
                        arr.push('</div>');
                        arr.push('<hr />');
                    });
                }
                $('#divOutput').append(arr.join(' '));
            }
        }
    });
});