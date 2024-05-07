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



