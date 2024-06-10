
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
    var lastTradeIndex = 0;
    var tradeIndex = 0;
    var sampleSizeChanged;
    // The model
    var researchVM;
    var trades = $('#tradesData').data('trades');

    //console.log(trades);

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
        '#dropdownBtnSampleSize': '#spanSampleSize'
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

    /**
     * ***************************
     * Region menu methods
     * ***************************
     */

    // Event handler when the left or the right arrow is pressed. Changes the trade accordingly.
    $(document).keydown(function (event) {
        // Left arrow key pressed
        if (event.which === 37) {
            ShowPrevTrade();
        // Right arrow key pressed
        } else if (event.which === 39) {
            ShowNextTrade();
            // Add your code to handle right arrow key press
        }
    });

    function ShowNextTrade() {
        tradeIndex++;
        ShowScreenshots();
    }

    function ShowPrevTrade() {
        tradeIndex--;
        ShowScreenshots();
    }

    $('#tradeNumberInput').on('keypress', function (event) {
        if (event.which === 13) {
            tradeIndex = event.target.value - 1;
            ShowScreenshots();
        }
    });

    $('#btnNext').on('click', function () {
        ShowNextTrade();
    });

    $('#btnPrev').on('click', function () {
        ShowPrevTrade();
    });

    function ShowScreenshots() {
        if (tradeIndex >= trades.length) {
            toastr.info('Trade ' + (tradeIndex + 1) + ' doesn\'t exist.');
            tradeIndex = lastTradeIndex;
            $('#tradeNumberInput').val(tradeIndex + 1);
            return;
        }
        else if (tradeIndex < 0) {
            toastr.info('Trade number can\'t be smaller then 1.');
            tradeIndex = lastTradeIndex;
            $('#tradeNumberInput').val(tradeIndex + 1);
            return;
        }
        lastTradeIndex = tradeIndex;
        LoadImages();
    }

    function LoadImages() {
        $('#imageContainer').empty();
        var screenshots = trades[tradeIndex]['ScreenshotsUrls'];

        var newCarouselHtml = '<ol class="carousel-indicators">';
        for (var i = 0; i < screenshots.length; i++) {
            var url = screenshots[i];
            if (i == 0) {
                newCarouselHtml += '<li data-bs-target="#carouselTrades" data-slide-to="' + i + '" class="active"></li >';
            }
            else {
                newCarouselHtml += '<li data-bs-target="#carouselTrades" data-slide-to="' + i + '" ></li >';
            }
        }
        newCarouselHtml += '</ol>';

        newCarouselHtml += '<div class="carousel-inner">';
        for (var i = 0; i < screenshots.length; i++) {
            var url = screenshots[i];
            if (i == 0) {
                newCarouselHtml += '<div class="carousel-item active"><img src="' + url + '" class="d-block w-100" alt = "..." ></div>';
            }
            else {
                newCarouselHtml += '<div class="carousel-item"><img src="' + url + '" class="d-block w-100" alt = "..." ></div>';;
            }
        }
        $('#tradeNumberInput').val(tradeIndex + 1);
        newCarouselHtml += '</div>';
        $('#imageContainer').html(newCarouselHtml);
        console.log(newCarouselHtml);
    }

    /**
     * ***************************
     * Region menu methods
     * ***************************
     */
});