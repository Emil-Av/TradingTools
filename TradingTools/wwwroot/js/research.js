
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
    // Global index for the currently displayed trade. Can be used in the 'trades' variable
    var tradeIndex = 0;
    var lastTradeIndex = 0;
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


    function setMenuValues(researchVM) {
        // Menu Buttons
        var numberSampleSizes = researchVM.NumberSampleSizes;
        var tradesInSampleSize = researchVM.TradesInSampleSize;
        $('#tradesInSampleSize').val('/' + tradesInSampleSize);
        // Set the SampleSize menu
        $('#spanSampleSize').text(researchVM.CurrentSampleSizeNumber);
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
            setSelectedItemClass(key);
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
                loadResearchSampleSizeAsync($('#spanTimeFrame').text(),
                    $('#spanStrategy').text(),
                    $('#spanSampleSize').text(),
                    sampleSizeChanged);
            });
        })(key);
    }

    // Mark the selected drop down item of the buttons on the top
    function setSelectedItemClass() {
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
     * Region even handlers begins
     * ***************************
     */

    // Card button 'Update' click event handler 
    $('#btnUpdate').on('click', function () {
        updateTradeData(tradeIndex);
    });

    // Card button 'Delete' click event handler 
    $('#btnDelete').on('click', function () {
        Swal.fire({
            title: "Are you sure?",
            text: "All data incl. screenshots will be gone.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes"
        }).then((result) => {
            if (result.isConfirmed) {
                deleteTrade();
            }
        });

    });

    // Menu button "Next" click event handler
    $('#btnNext').on('click', function () {
        showNextTrade(1);
    });

    // Menu button "Previous" click event handler
    $('#btnPrev').on('click', function () {
        showPrevTrade(-1);
    });

    // Event handler when the left or the right arrow is pressed. Displays the trade accordingly.
    $(document).on('keydown', function (event) {
        // Left arrow key pressed
        if (event.which === 37) {
            showPrevTrade(-1);
            // Right arrow key pressed
        } else if (event.which === 39) {
            showNextTrade(1);
        }
    });

    // User enters trade number and presses enter
    $('#tradeNumberInput').on('keypress', function (event) {
        if (event.which === 13) {
            userInput = Number(event.target.value);
            if (Number.isInteger(userInput)) {
                tradeIndex = userInput - 1;
                displayTradeData(tradeIndex, true);
            }
            else {
                toastr.error("Please enter a whole number.");
                return
            }
        }
    });


    /**
     * ***************************
     * Region even handlers ends
     * ***************************
     */

    /**
     * ***************************
     * Region methods begins
     * ***************************
     */

    // Toggles to the next trade
    function showNextTrade(index) {
        displayTradeData(index, false);
    }
    // Toggles to the previous trade
    function showPrevTrade(index) {
        displayTradeData(index, false);
    }

    // Loads the screenshots and the values in the input/select elements in the card
    function displayTradeData(indexToShow, canShowToastr) {
        // Buttons 'prev' or 'next'
        if (indexToShow == -1 || indexToShow == 1) {
            tradeIndex += indexToShow;
        }
        // User input of the trade to be shown
        else {
            tradeIndex = indexToShow;
        }

        if (tradeIndex >= trades.length) {
            if (canShowToastr) {
                toastr.error('Trade ' + (tradeIndex + 1) + ' doesn\'t exist.');
                canShowToastr = false;
            }
            else {
                toastr.info('The last trade is being displayed');
            }
            tradeIndex = lastTradeIndex;
            return;
        }
        else if (tradeIndex < 0) {
            if (canShowToastr) {
                toastr.error('Trade number can\'t be smaller then 1.');
                canShowToastr = false;
            }
            else {
                toastr.info('The first trade is being displayed');
            }
            tradeIndex = lastTradeIndex;
            return;

        }
        lastTradeIndex = tradeIndex;
        $('#tradeNumberInput').val(tradeIndex + 1);
        loadImages();
        loadTradeData();
    }

    function deleteTrade() {
        var currentTradeNumber = parseInt($('#tradeNumberInput').val());
        var id = trades[currentTradeNumber - 1]['IdDisplay'];
        if (currentTradeNumber - 1 <= 0) {
            tradeIndex = 0;
        }
        else {
            tradeIndex = currentTradeNumber - 1;
        }
        $.ajax({
            method: 'DELETE',
            url: '/research/delete',
            dataType: 'JSON',
            data: {
                id: id
            },
            success: function (response) {
                if (response['error'] !== undefined) {
                    toastr.error(response['error']);
                }
                loadViewData(response);
            },
            error: function (jqXHR, exception) // code for exceptions
            {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            }

        });
    }

    // Updates the database with the values from the card for the displayed trade
    function updateTradeData(index) {
        var updatedTrade = {};
        // Get all data from the input and select fields in the card
        $('#cardBodyResearch [data-research]').each(function () {
            var bindProperty = $(this).data('research');
            updatedTrade[bindProperty] = $(this).val();
        });
        // Add the Id and the Screenshots
        updatedTrade['TradeRatingDisplay'] = parseInt($('#TradeRatingInput').val());
        updatedTrade['IdDisplay'] = trades[index]['IdDisplay'];
        updatedTrade['ScreenshotsUrlsDisplay'] = trades[index]['ScreenshotsUrlsDisplay'];
        // make the API call
        $.ajax({
            method: 'POST',
            url: '/research/updatetrade',
            contentType: 'application/json; charset=utf-8',
            dataType: 'JSON',
            data: JSON.stringify(updatedTrade),
            success: function (response) {
                if (response['success'] !== undefined) {
                    toastr.success(response['success']);
                }
                else if (response['error'] !== undefined) {
                    toastr.error(response['error']);
                }
            },
            error: function (response) {
                console.error(response);
            }
        });
    }
    // Loads the trade data into the input/select elements. Used in the prev/next buttons or the key combination
    function loadTradeData() {
        var trade = trades[tradeIndex];
        $('#cardBodyResearch [data-research]').each(function () {
            var bindProperty = $(this).data('research');
            if (trade.hasOwnProperty(bindProperty)) {
                $(this).val(trade[bindProperty]);
            }
        });
        $('#currentTradeId').val(trade['IdDisplay']);
    }
    // Loads the images into the carousel
    function loadImages() {
        var screenshots = trades[tradeIndex]['ScreenshotsUrlsDisplay'];
        if (screenshots === null) {
            toastr.error("No screenshots for the selected trade.");
            return;
        }

        var newCarouselHtml = '<ol class="carousel-indicators">';
        for (var i = 0; i < screenshots.length; i++) {
            var url = screenshots[i];
            if (i == 0) {
                newCarouselHtml += '<li data-bs-target="#carouselTrades" data-bs-slide-to="' + i + '" class="active"></li >';
            }
            else {
                newCarouselHtml += '<li data-bs-target="#carouselTrades" data-bs-slide-to="' + i + '" ></li >';
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
        newCarouselHtml += '</div>';
        $('#imageContainer').empty();
        $('#imageContainer').html(newCarouselHtml);
        $('#tradeNumberInput').val(tradeIndex + 1);

        console.log(newCarouselHtml);
    }

    function loadResearchSampleSizeAsync(timeFrame, strategy, sampleSizeNumber, isSampleSizeChanged) {
        // make the API call
        $.ajax({
            method: 'POST',
            url: '/research/loadResearchSampleSize',
            dataType: 'JSON',
            data: {
                timeFrame: timeFrame,
                strategy: strategy,
                sampleSizeNumber: sampleSizeNumber,
                isSampleSizeChanged: isSampleSizeChanged
            },
            success: function (response) {
                if (response['error'] !== undefined) {
                    toastr.error(response['error']);
                }
                loadViewData(response);
            },
            error: function (jqXHR, exception) // code for exceptions
            {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            }
        });


    }

    function loadViewData(response) {
        researchVM = JSON.parse(response.researchVM);
        tradeIndex = 0;
        trades = researchVM.AllTrades;
        loadTradeData();
        loadImages();
        setMenuValues(researchVM);
        setSelectedItemClass();
    }


    /**
    * ***************************
    * Region methods
    * ***************************
    */
});