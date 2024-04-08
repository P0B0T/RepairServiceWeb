$(function () {
    var savedTheme = localStorage.getItem('theme');

    if (savedTheme) {
        switchTheme(savedTheme);
    }

    $(".btnTheme").click(function () {
        var theme = $(this).text();

        switchTheme(theme);

        localStorage.setItem('theme', theme);
    });

    function switchTheme(theme) {
        switch (theme) {
            case "Стандартная":
                $("#clientCssLink").attr("disabled", true);
                $("#staffCssLink").attr("disabled", true);
                break;
            case "Клиент":
                $("#clientCssLink").attr("disabled", false);
                $("#staffCssLink").attr("disabled", true);
                break;
            case "Сотрудник":
                $("#clientCssLink").attr("disabled", true);
                $("#staffCssLink").attr("disabled", false);
                break;
        }
    }
});