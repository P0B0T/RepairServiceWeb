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