$(function () {

    /**
    * ***************************
    * Region menu buttons starts
    * ***************************
    */
    // Create key, value array: key is the button menu, value is the span element. The span element is the selected value from the dropdown menu.
    var menuButtons =
    {
        '#dropdownBtnTimeFrame': '#spanTimeFrame',
        '#dropdownBtnStrategy': '#spanStrategy',
        '#dropdownBtnTradeType': '#spanTradeType',
        '#dropdownBtnTradeSide': '#spanTradeSide'
    };

    // Attach a click event for each <a> element of each menu.
    for (var key in menuButtons) {
        (function (key) {
            SetSelectedItemClass(key);
            // Change the value of the span in the button
            $(key).on('click', '.dropdown-item', function () {
                // Save the old value. If there is no trade in the DB for the selected trade, the menu's old value should be displayed.
                menuClicked = $(menuButtons[key]);
                clickedMenuValue = $(menuButtons[key]).text();
                // Set the new value
                var value = $(this).text();
                $(menuButtons[key]).text(value);
            });
        })(key);
    }

    // Mark the selected drop down item of the buttons on the top
    function SetSelectedItemClass() {
        // Set the "selected item" color
        for (var key in menuButtons) {
            $(key + ' a').each(function () {
                if ($(this).text() === $(menuButtons[key]).text()) {
                    $(this).addClass('bg-gray-400');
                }
                else {
                    $(this).removeClass('bg-gray-400');
                }
            })
        }
    }

    /**
    * ***************************
    * Region menu buttons ends
    * ***************************
    */

    // After uploading screenshots, the 'change' event is triggered, this submits the form and sends the files to the controller
    $('input[type="file"]').change(function (e) {
        var formData = new FormData();
        var files = $(this)[0].files; // Get the files array from the input element

        // Iterate through each selected file to append it to FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        var tradeData = {};
        tradeData['timeFrame'] = $('#spanTimeFrame').text();
        tradeData['strategy'] = $('#spanStrategy').text();
        tradeData['tradeType'] = $('#spanTradeType').text();
        tradeData['tradeSide'] = $('#spanTradeSide').text();

        formData.append('tradeData', JSON.stringify(tradeData));

        $.ajax({
            url: '/newtrade/uploadscreenshots', // Replace with your controller action URL
            type: 'POST',
            data: formData,
            processData: false, // Don't process the files
            contentType: false, // Set content type to false as FormData will handle it
            success: function (response) {
                console.log('Files uploaded successfully');
                // Handle success response
            },
            error: function (error) {
                console.error('Error uploading files:', error);
                // Handle error
            }
        });
    })

})