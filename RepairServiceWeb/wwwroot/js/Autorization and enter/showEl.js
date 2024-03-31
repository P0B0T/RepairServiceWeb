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
            $('#addDevicesButton').show();
            $('.buttonInf').show();
            $('.elemInf').show();
            $('.staffInf').show();
            $('.confInf').show();
            $('#workersEl').show();
        }
        else if (permissionValue.includes('ресепшен') || permissionValue.includes('reception')) {
            $('.navbar-nav').show();
            $('.elemInf').show();
            $('#addDevicesButton').show();
        }
        else if (permissionValue.includes('human resources') || permissionValue.includes('отдел кадров')) {
            $('.navbar-nav').show();
            $('.staffInf').show();
            $('.buttonAdd').show();
            $('#addDevicesButton').show();
            $('.buttonInf').show();
        }
        else if (permissionValue.includes('staff') || permissionValue.includes('сотрудник')) {
            $('#workersEl').show();
            $('#infAccessories').show();
            $('#infDevice').show();
        }
    })
});