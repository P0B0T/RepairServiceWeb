function isAdmin() {
    var permissionId = getCookie('permissions');

    var isAdmin = false;

    $.ajax({
        url: '/Autorization/GetRoleName',
        method: 'GET',
        data: { permissionId: permissionId },
        async: false,
        success: function (response) {
            if (response.success) {
                var permissionValue = response.data.toLowerCase();
                isAdmin = permissionValue.includes('admin') || permissionValue.includes('админ');
            }
        }
    });

    return isAdmin;
}

$('#filterAccessories').on('submit', function (event) {
    event.preventDefault();

    var name = $('input[name="Name"]').val();
    var manufacturer = $('select[name="manufacturer"]').val();
    var supplier = $('select[name="supplier"]').val();

    $.ajax({
        url: '/Accessories/GetFilteredAccessories',
        method: 'GET',
        data: { name: name, manufacturer: manufacturer, supplier: supplier },

    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 30em">');
                arr.push(`<h3>${item.name}</h3>`);
                if (item.description !== null) {
                    arr.push(`<p>${item.description}</p>`);
                } else {
                    arr.push('<p></p>');
                }
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 16em">');
                arr.push('<label><b>Производитель:</b></label>');
                arr.push(`<label>${item.manufacturer}</label> <br />`);
                arr.push('<label><b>Цена:</b></label>');
                arr.push(`<label>${item.cost} руб.</label> <br />`);
                arr.push('<label><b>Поставщик:</b></label>');
                arr.push(`<label>${item.supplier.companyName}</label>`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Accessories/GetAccessories', data: '${item.id}', title: 'Информация о запчасти' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterSuppliers').on('submit', function (event) {
    event.preventDefault();

    var address = $('input[name="address"]').val();

    $.ajax({
        url: '/Suppliers/GetFilteredSuppliers',
        method: 'GET',
        data: { address: address },

    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 22em">');
                arr.push(`<h3>${item.companyName}</h3>`);
                if (item.contactPerson != null) {
                    arr.push('<label><b>Контактное лицо:</b></label>');
                    arr.push(`<label>${item.contactPerson}</label> <br />`)
                } else {
                    arr.push('<label></label>');
                }
                if (item.phoneNumber != null) {
                    arr.push('<label><b>Номер телефона:</b></label>');
                    arr.push(`<label>${item.phoneNumber}</label >`)
                } else {
                    arr.push('<label></label>');
                }
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 22em">');
                arr.push('<label><b>Адрес:</b></label>');
                arr.push(`<label>${item.address}</label> <br />`);
                arr.push('<label><b>Email:</b></label>');
                arr.push(`<label>${item.email}</label >`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Suppliers/GetSuppliers', data: '${item.id}', title: 'Информация о поставщике' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterOrderAccessories').on('submit', function (event) {
    event.preventDefault();

    var clientFullName = $('select[name="clientFullName"]').val();
    var accessoryName = $('select[name="accessoryName"]').val();
    var count = $('input[name="count"]').val();
    var date = $('input[name="date"]').val();
    var status = $('select[name="status"]').val();

    $.ajax({
        url: '/OrderAccessories/GetFilteredOrderAccessories',
        method: 'GET',
        data: { clientFullName: clientFullName, accessoryName: accessoryName, count: count, date: date, status: status },
    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 30em">');
                arr.push(`<h3>${item.accessories.name}</h3>`);
                arr.push(`<p><b>Клиент:</b> ${item.client.fullName}</p>`)
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 12em">');
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
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/OrderAccessories/GetOrderAccessories', data: '${item.id}', title: 'Информация о заказе' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterClients').on('submit', function (event) {
    event.preventDefault();

    var fullName = $('input[name="fullName"]').val();
    var address = $('input[name="address"]').val();

    $.ajax({
        url: '/Clients/GetFilteredClients',
        method: 'GET',
        data: { fullName: fullName, address: address },
    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 30em">');
                arr.push(`<h3>${item.fullName}</h3>`);
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 18em">');
                arr.push('<label><b>Адрес:</b></label>');
                arr.push(`<label>${item.address}</label> <br />`);
                arr.push('<label><b>Номер телефона:</b></label>');
                arr.push(`<label>${item.phoneNumber}</label> <br />`);
                arr.push('<label><b>Email:</b></label>');
                arr.push(`<label>${item.email}</label> <br />`);
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 12em">');
                arr.push('<label><b>Логин:</b></label>');
                arr.push(`<label>${item.login}</label> <br />`);
                arr.push('<label><b>Пароль:</b></label>');
                arr.push(`<label>${item.password}</label> <br />`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Clients/GetClients', data: '${item.id}', title: 'Информация о клиенте' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterStaff').on('submit', function (event) {
    event.preventDefault();

    var fullName = $('input[name="fullName"]').val();
    var experiance = $('input[name="experiance"]').val();
    var post = $('select[name="post"]').val();
    var role = $('select[name="role"]').val();

    $.ajax({
        url: '/Staff/GetFilteredStaff',
        method: 'GET',
        data: { fullName: fullName, experiance: experiance, post: post, role: role },
    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 27em">');
                if (item.photo != null) {
                    arr.push(`<img src="/images/StaffPhoto/${item.photo}" alt="Фото сотрудника">`);
                }
                arr.push('</div>');
                arr.push('<div style="width: 19em">');
                arr.push(`<h3>${item.fullName}</h3>`);
                arr.push(`<p>${item.post}</p>`)
                arr.push('<label><b>Стаж:</b></label>');
                arr.push(`<label>${item.experianceWithWord}</label> <br />`);
                arr.push('<label><b>Зар. плата:</b></label>');
                arr.push(`<label>${item.salary} руб.</label> <br />`);
                arr.push('<label><b>Дата принятия на работу:</b></label>');
                var date = new Date(item.dateOfEmployment);
                var formattedDate = date.toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });
                arr.push(`<label>${formattedDate}</label> <br />`);
                arr.push('<label><b>Роль:</b></label>');
                arr.push(`<label>${item.role.role1}</label> <br /> <br />`);
                arr.push('<label><b>Логин:</b></label>');
                arr.push(`<label>${item.login}</label> <br />`);
                arr.push('<label><b>Пароль:</b></label>');
                arr.push(`<label>${item.password}</label> <br />`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Staff/GetStaff', data: '${item.id}', title: 'Информация о сотруднике' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterDevices').on('submit', function (event) {
    event.preventDefault();

    var manufacturer = $('select[name="manufacturer"]').val();
    var type = $('input[name="type"]').val();
    var clientFullName = $('input[name="clientFullName"]').val();

    $.ajax({
        url: '/Devices/GetFilteredDevices',
        method: 'GET',
        data: { manufacturer: manufacturer, type: type, clientFullName: clientFullName },
    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 27em">');
                if (item.photo != null) {
                    arr.push(`<img src="/images/DevicesPhoto/${item.photo}">`);
                }
                arr.push('</div>');
                arr.push('<div style="width: 19em">');
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
                arr.push('<label><b>Клиент:</b></label>');
                arr.push(`<label>${item.client.fullName}</label> <br />`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Devices/GetDevices', data: '${item.id}', title: 'Информация об устройстве' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});

$('#filterRepairs').on('submit', function (event) {
    event.preventDefault();

    var clientFullName = $('select[name="clientFullName"]').val();
    var staffFullName = $('select[name="staffFullName"]').val();

    $.ajax({
        url: '/Repairs/GetFilteredRepairs',
        method: 'GET',
        data: { clientFullName: clientFullName, staffFullName: staffFullName },
    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows counter">');
                arr.push('<div style="width: 20em">');
                arr.push(`<h3>${item.device.model}</h3>`);
                arr.push('<label><b>Клиент:</b></label>');
                arr.push(`<label>${item.device.client.fullName}</label> <br />`);
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
                arr.push('<div style="margin-top: 2em; width: 20em">');
                arr.push('<label><b>Описание проблемы:</b></label>');
                arr.push(`<label>${item.descriptionOfProblem}</label> <br />`);
                arr.push('<label><b>Описание проделанной работы:</b></label>');
                arr.push(`<label>${item.descriprionOfWorkDone}</label> <br />`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                if (isAdmin()) {
                    arr.push(`<button type="button" onclick="openModal({ url: '/Repairs/GetRepairs', data: '${item.id}', title: 'Информация о ремонте' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                }
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});