// Get all element
var elements =
{
    '#menuTimeFrame': '#currentTimeFrame',
    '#menuStrategy': '#currentStrategy',
    '#menuSampleSize': '#currentSampleSize',
    '#menuTradeNumber': '#currentTradeNumber'
};

// Attach the event for each element
for (var key in elements) {
    (function (key) {
        $(key).on('click', '.dropdown-item', function () {
            var value = $(this).text();
            $(elements[key]).text(value);
        });
    })(key);
}


