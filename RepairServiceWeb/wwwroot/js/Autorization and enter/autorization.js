$(function () {
    $("#autorizationForm").on("submit", function (event) {
        event.preventDefault();

        $.post('/Autorization/Enter', $(this).serialize(), (data) => {
            setCookie('auth_key', data.auth_key);
            setCookie('permissions', data.permissions);
            setCookie('userId', data.userId);
            setCookie('login', data.login);
            setCookie('password', data.password);

            window.location.href = '/PersonalCabinet/PersonalCabinet?userId=' + data.userId + '&login=' + data.login + '&password=' + data.password;
        }).fail((error) => {
            $("#error-message").text(error.responseText);
        });
    });
});

function setCookie(name, val) {
    const date = new Date();
    const value = val;

    date.setTime(date.getTime() + (7 * 24 * 60 * 60 * 1000));

    document.cookie = name + "=" + value + "; expires=" + date.toUTCString() + "; path=/";
}