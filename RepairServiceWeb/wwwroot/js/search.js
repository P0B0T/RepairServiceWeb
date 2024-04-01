var currentIndex = -1;
var highlightedItems = [];

function searchHighlight(url) {
    $('.search').on('submit', function (event) {
        event.preventDefault();
        var highlightedName = $('input[name="name"]').val().toLowerCase().trim();
        if (highlightedName) {
            $.ajax({
                url: url,
                type: 'GET',
                data: { name: highlightedName },
                success: function (data) {
                    if (data.success) {
                        highlightedItems = [];
                        currentIndex = -1;
                        $('.divRows').each(function () {
                            var name = $(this).find('h3').text().toLowerCase().trim();
                            if (name.includes(data.highlightedName)) {
                                highlightedItems.push(this);
                            }
                        });
                        if (highlightedItems.length > 0) {
                            currentIndex = 0;
                            $('.divRows').removeClass('searchRes');
                            $(highlightedItems[currentIndex]).addClass('searchRes');
                            highlightedItems[currentIndex].scrollIntoView();
                            $('#upButton, #downButton, #counter').show();
                            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
                        }
                    }
                }
            });
        }
    });

    $('input[name="name"]').on('input', function () {
        $('.divRows').removeClass('searchRes');
        highlightedItems = [];
        currentIndex = -1;
        $('#upButton, #downButton, #counter').hide();
        $('#counter').text('');
    });

    $('#upButton').on('click', function (event) {
        event.preventDefault();
        if (highlightedItems.length > 0) {
            $('.divRows').removeClass('searchRes');
            currentIndex = (currentIndex - 1 + highlightedItems.length) % highlightedItems.length;
            $(highlightedItems[currentIndex]).addClass('searchRes');
            highlightedItems[currentIndex].scrollIntoView({ behavior: "smooth" });
            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
        }
    });

    $('#downButton').on('click', function (event) {
        event.preventDefault();
        if (highlightedItems.length > 0) {
            $('.divRows').removeClass('searchRes');
            currentIndex = (currentIndex + 1) % highlightedItems.length;
            $(highlightedItems[currentIndex]).addClass('searchRes');
            highlightedItems[currentIndex].scrollIntoView({ behavior: "smooth" });
            $('#counter').text((currentIndex + 1) + ' из ' + highlightedItems.length);
        }
    });
}

$(function () {
    $('#upButton, #downButton, #counter').hide();
});
