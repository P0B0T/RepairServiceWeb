﻿function openModal(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const title = parameters.title;
    const modal = $('#modal');  

    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id },
        success: function (response) {
            modal.find(".modal-title").text(title);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        }
    });
}

function openConfirm(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#confirmDelete');
    const parentModal = $('#modal');

    $.ajax({
        type: 'GET',
        data: { "id": id },
        success: function (responce) {
            modal.modal('show')
        }
    })

    $(function () {
        modal.find('#buttonYes').click(function () {
            $.ajax({
                url: url,
                type: 'POST',
                data: { id: id },
                success: function (response) {
                    modal.modal('hide');
                    parentModal.modal('hide');
                    location.reload();
                }
            });
        });
    });
}

function openAutorization() {
    var userId = getCookie('userId');
    var login = getCookie('login');
    var password = getCookie('password');

    if (userId) {
        window.location.href = '/PersonalCabinet/PersonalCabinet?userId=' + userId + '&login=' + login + '&password=' + password
    } else {
        const modal = $('#autorization');

        $.ajax({
            type: 'GET',
            success: function (response) {
                modal.modal('show');
            }
        });
    }
}