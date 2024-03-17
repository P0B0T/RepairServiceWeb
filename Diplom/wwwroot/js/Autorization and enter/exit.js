$(function () {
    $("#exitButton").click(function () {
        deleteCookie("auth_key");
        deleteCookie("permissions");
        deleteCookie("userId");
    });
});

function deleteCookie(name) {
    const date = new Date();

    date.setTime(date.getTime() + (-1 * 24 * 60 * 60 * 1000));


    document.cookie = name + "=; expires=" + date.toUTCString() + "; path=/";
}