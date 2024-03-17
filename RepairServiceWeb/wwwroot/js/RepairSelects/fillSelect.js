$(function () {
    $(document).on('change', '#selectClient', function () {
        var clientId = $(this).val();
        $.get("/Repairs/GetDevices", { clientId: clientId }, function (data) {
            var devicesSelect = $("#selectDevice");
            devicesSelect.empty();
            $.each(data, function (index, row) {
                devicesSelect.append("<option value='" + row.id + "'>" + row.model + "</option>")
            });
        });
    });
});