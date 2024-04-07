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
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/staff.css');
            $('.navbar-nav').show();
            $('.elemInf').show();
            $('#addDevicesButton').show();
        }
        else if (permissionValue.includes('human resources') || permissionValue.includes('отдел кадров')) {
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/staff.css');
            $('.navbar-nav').show();
            $('.staffInf').show();
            $('.buttonAdd').show();
            $('#addDevicesButton').show();
            $('.buttonInf').show();
        }
        else if (permissionValue.includes('staff') || permissionValue.includes('сотрудник')) {
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/staff.css');
            $('#workersEl').show();
            $('#infAccessories').show();
            $('#infDevice').show();
        }
        else {
            $('<link>')
                .appendTo('head')
                .attr({ type: 'text/css', rel: 'stylesheet' })
                .attr('href', '/css/client.css');
        }
    })
});