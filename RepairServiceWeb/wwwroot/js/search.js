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
                        $('.divRows').each(function () {
                            var name = $(this).find('h3').text().toLowerCase().trim();
                            if (name.includes(data.highlightedName)) {
                                $(this).addClass('searchRes');
                                this.scrollIntoView();
                                return false;
                            }
                        });
                    }
                }
            });
        }
    });
    $('input[name="name"]').on('input', function () {
        $('.divRows').removeClass('searchRes');
    });
}