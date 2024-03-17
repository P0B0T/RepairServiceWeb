$(function () {
    var permissionId = getCookie('permissions');

    $.get('/Autorization/GetRoleName', { permissionId: permissionId }, function (response) {

        if (response.success === false) {
            return;
        }

        var permissionValue = response.data.toLowerCase();

        if (permissionValue.includes('admin') || permissionValue.includes('админ')) {
            $('.navbar-nav').show();
            $('.buttonAdd').show();
            $('.buttonInf').show();
            $('.elemInf').show();
            $('.staffInf').show();
        }

        if (permissionValue.includes('ресепшен') || permissionValue.includes('reception')) {
            $('.navbar-nav').show();
            $('.elemInf').show();
        }

        if (permissionValue.includes('human resources department') || permissionValue.includes('отдел кадров')) {
            $('.navbar-nav').show();
            $('.staffInf').show();
            $('.buttonAdd').show();
            $('.buttonInf').show();
        }
    })
});