$(document).ready(function () {
    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').submit();
    });

    // Get all element
    var elements =
    {
        '#menuTimeFrame': '#currentTimeFrame',
        '#menuStrategy': '#currentStrategy',
        '#menuSampleSize': '#currentSampleSize',
        '#menuTrade': '#currentTrade'
    };

    // Attach the event for each element
    for (var key in elements) {
        (function (key) {
            // Set the "selected item" color when the page is loaded
            $(key + ' a').each(function () {
                if ($(this).text() ===  $(elements[key]).text()) {
                    $(this).addClass('bg-gray-400');
                }
            })
            // Change the value of the span in the button
            $(key).on('click', '.dropdown-item', function () {
                var value = $(this).text();
                $(elements[key]).text(value);

                $('#timeFrameInput').val($('#currentTimeFrame').text());
                $('#strategyInput').val($('#currentStrategy').text());
                $('#sampleSizeInput').val($('#currentSampleSize').text());
                $('#tradeInput').val($('#currentTrade').text());
                $('#formButtonsInput').submit();
            });
        })(key);
    }
});



