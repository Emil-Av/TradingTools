
$(function () {
    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').trigger('submit');
    });

    /**
    * ******************************
    * Region global variables starts
    * ******************************
    */

    // menuClicked, clickedMenuValue: When a new trade has to be loaded, one of the buttons has to be clicked (either TimeFrame, Strategy..). In case no trade exists for the selection, set the last value. Used in LoadTradeAsync()
    var menuClicked;
    var clickedMenuValue;
    var showLastTrade;
    var sampleSizeChanged;
    // The model
    var researchVM;

    /**
    * ******************************
    * Region global variables ends
    * ******************************
    */


    function SetMenuValues(displayedTrade) {
        // Menu Buttons
        var numberSampleSizes = paperTradesVM['paperTradesVM']['numberSampleSizes'];
        var tradesInSampleSize = paperTradesVM['paperTradesVM']['tradesInSampleSize'];
        // Set the SampleSize menu
        $('#spanSampleSize').text(paperTradesVM['paperTradesVM']['currentSampleSize']);
        $('#dropdownBtnSampleSize').empty();
        var sampleSizes = '';
        for (var i = numberSampleSizes; i > 0; i--) {
            sampleSizes += '<a class="dropdown-item" role="button">' + i + '</a>';
        }

        $('#dropdownBtnSampleSize').html(sampleSizes);
        // Set the Trades menu
        if (showLastTrade === true) {
            $('#spanTrade').text(tradesInSampleSize);
        }
        else {
            $('#spanTrade').text(displayedTrade);
        }

        $('#dropdownBtnTrade').empty();
        var trades = '';
        for (var i = tradesInSampleSize; i > 0; i--) {
            trades += '<a class="dropdown-item" role="button">' + i + '</a>'
        }
        $('#dropdownBtnTrade').html(trades);
    }

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
        '#dropdownBtnSampleSize': '#spanSampleSize',
        '#dropdownBtnTrade': '#spanTrade'
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
                // If the time frame,the strategy or the sample size has changed, then the latest trade must always be displayed. Used in SetMenuValues()
                if (key != '#dropdownBtnTrade') {
                    showLastTrade = true;
                }
                else {
                    showLastTrade = false;
                }

                if (key == '#dropdownBtnSampleSize') {
                    sampleSizeChanged = true;
                }
                else {
                    sampleSizeChanged = false;
                }

                // Set the new value
                var value = $(this).text();
                $(menuButtons[key]).text(value);
                LoadTradeAsync($('#spanTimeFrame').text(),
                    $('#spanStrategy').text(),
                    $('#spanSampleSize').text(),
                    $('#spanTrade').text(),
                    showLastTrade,
                    sampleSizeChanged);
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
});