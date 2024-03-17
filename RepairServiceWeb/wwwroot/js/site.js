function adjustButtonMargin() {
    var navbarHeight = $('.navbar').height();
    $('#exitButton').css('margin-top', navbarHeight + 20);
}

$(document).ready(adjustButtonMargin);
$(window).on('resize', adjustButtonMargin);