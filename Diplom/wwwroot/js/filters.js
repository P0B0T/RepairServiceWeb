$('#filterAccessories').on('submit', function (event) {
    event.preventDefault();

    var name = $('input[name="Name"]').val();
    var manufacturer = $('select[name="manufacturer"]').val();
    var cost = $('input[name="cost"]').val();
    var supplier = $('select[name="supplier"]').val();

    $.ajax({
        url: '/Accessories/GetFilteredAccessories',
        method: 'GET',
        data: { name: name, manufacturer: manufacturer, cost: cost, supplier: supplier },

    }).done(function (data) {
        if (data.success) {
            $('#divOutput').empty();
            var arr = [];

            data.filteredData.forEach(function (item) {
                arr.push('<div class="divRows">');
                arr.push('<div style="width: 30em">');
                arr.push(`<h3>${item.name}</h3>`);
                if (item.description !== null) {
                    arr.push(`<p>${item.description}</p>`);
                } else {
                    arr.push('<p></p>');
                }
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 16em">');
                arr.push('<label>Производитель:</label>');
                arr.push(`<label>${item.manufacturer}</label> <br />`);
                arr.push('<label>Цена:</label>');
                arr.push(`<label>${item.cost} руб.</label> <br />`);
                arr.push('<label>Поставщик:</label>');
                arr.push(`<label>${item.supplier.companyName}</label>`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                arr.push(`<button type="button" onclick="openModal({ url: '/Accessories/GetAccessories', data: '${item.id}', title: 'Информация о запчасти' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
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
                console.log(item);
                arr.push('<div class="divRows">');
                arr.push('<div style="width: 22em">');
                arr.push(`<h3>${item.companyName}</h3>`);
                if (item.contactPerson != null) {
                    arr.push('<label>Контактное лицо:</label>');
                    arr.push(`<label>${item.contactPerson}</label> <br />`)
                } else {
                    arr.push('<label></label>');
                }
                if (item.phoneNumber != null) {
                    arr.push('<label>Номер телефона:</label>');
                    arr.push(`<label>${item.phoneNumber}</label >`)
                } else {
                    arr.push('<label></label>');
                }
                arr.push('</div>');
                arr.push('<div style="margin-top: 2em; width: 22em">');
                arr.push('<label>Адрес:</label>');
                arr.push(`<label>${item.address}</label> <br />`);
                arr.push('<label>Email:</label>');
                arr.push(`<label>${item.email}</label >`);
                arr.push('</div>');
                arr.push('<div>');
                arr.push('<form>');
                arr.push(`<button type="button" onclick="openModal({ url: '/Suppliers/GetSuppliers', data: '${item.id}', title: 'Информация о поставщике' })" data-toggle="ajax-modal" data-target="#Modal">Посмотреть</button>`);
                arr.push('</form>');
                arr.push('</div>');
                arr.push('</div>');
                arr.push('<hr />');
            });
            $('#divOutput').append(arr.join(' '));
        }
    });
});