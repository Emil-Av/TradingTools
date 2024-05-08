//$.ajax({
//    url: '/papertrades/index',
//    type: 'GET',
//    beforeSend: function () {
//        ShowHideLoadingIndicator();
//    },
//    success: function (response) {
//    },
//    error: function (jqXHR, textStatus, errorThrown) {

//    },
//    complete: function () {
//        ShowHideLoadingIndicator();
//    }
//})

$(document).ready(function () {

    var currentTimeFrame = $('#currentTimeFrame').text().trim();
    $('#menuTimeFrame a').each(function () {
        if ($(this).text() === currentTimeFrame) {
            $(this).addClass('bg-gray-400');
        }
    });

    var currentStrategy = $('#currentStrategy').text().trim();
    $('#menuStrategy a').each(function () {
        if ($(this).text() === currentStrategy) {
            $(this).addClass('bg-gray-400');
        }
    });

    var currentSampleSize = $('#currentSampleSize').text().trim();
    $('#menuSampleSize a').each(function () {
        if ($(this).text() === currentSampleSize) {
            $(this).addClass('bg-gray-400');
        }
    });

    var currentTrade = $('#currentTrade').text().trim();
    $('#menuTrade a').each(function () {
        if ($(this).text() === currentTrade) {
            $(this).addClass('bg-gray-400');
        }
    });

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
});



