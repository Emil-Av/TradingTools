$(document).ready(function () {

    /**
    * ******************************
    * Region global variables starts
    * ******************************
    */

    // menuClicked, clickedMenuValue: When a new trade has to be loaded, one of the buttons has to be clicked (either TimeFrame, Strategy..). In case no trade exists for the selection, set the last value. Used in LoadTradeAsync()
    var menuClicked;
    var clickedMenuValue;
    var showLatestTrade;
    // The model
    var paperTradesVM;
    var showedJournal = '#showPre'; // Always the start value
    var isEditorShown = false;

    /**
    * ******************************
    * Region global variables ends
    * ******************************
    */

    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').submit();
    });


    /**
    * ************************
    * Region summernote starts
    * ************************
    */

    // Send the data to the controller
    function UpdateJournal() {
        let dataToSend =
        {
            CurrentTrade: {
                Id: $('#currentTradeIdInput').val()
            },
            Journal: {
                Pre: $('#showPre').html(),
                During: $('#showDuring').html(),
                Exit: $('#showExit').html(),
                Post: $('#showPost').html()
            }
        };
        $.ajax({
            method: 'POST',
            url: '/papertrades/updatejournal',
            contentType: "application/json; charset=utf-8",
            dataType: 'JSON',
            data: JSON.stringify(dataToSend),
            success: function (response) {
            }
        })
    }

    // When the content is double clicked, it can be edited (summernote is displayed)
    $('#tabContent').on('dblclick', function () {
        OpenEditor();
    });

    // On tab change
    $('button[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        SaveEditorText();
        showedJournal = '#show' + CapitalizeFirstLetter($(e.target).attr('aria-controls')); // activated tab
    });

    // Open editor and show the buttons
    $('#btnEdit').on('click', function () {
        $(this).addClass('d-none');
        $('.editorOnBtns').removeClass('d-none');
        OpenEditor();
    });

    // Save the journal changes
    $('#btnSave').on('click', function () {
        SaveEditorText();
        $('.editorOnBtns').addClass('d-none');
        $('#btnEdit').removeClass('d-none');
    });

    // Close the editor and show 'Edit' button
    // TODO: Changes are saved in the contentPage when they shouldn't. (Changes aren't save in the DB as expected)
    $('#btnCancel').on('click', function () {
        $('#summernote').summernote('destroy');
        $('.editorOnBtns').addClass('d-none');
        $('#btnEdit').removeClass('d-none');
    });

    // Save the journal in the DB
    function SaveEditorText() {
        if (isEditorShown) {
            $(showedJournal).html($('#summernote').summernote('code'));
            $(showedJournal).css('display', 'block');
            $('#summernote').summernote('code', '');
            $('#summernote').summernote('destroy');
            isEditorShown = false;
            UpdateJournal();
        }
    }

    // Open the summernote editor
    function OpenEditor() {
        // Hide the tabContent of the journal and show the summernote instead
        // Get the text from the tabContent
        var journalText = $(showedJournal).html();
        // Hide the tabContent
        $(showedJournal).css('display', 'none');
        // Set the text into the editor
        $('#summernote').summernote('code', journalText);
        // Display the editor
        $('#summernote').summernote('justifyLeft');
        isEditorShown = true;
    }

    // Make the first char of a string upper case
    function CapitalizeFirstLetter(text) {
        return text.charAt(0).toUpperCase() + text.slice(1);
    }

    /**
    * ************************
    * Region summernote ends
    * ************************
    */


    /**
    * ***************************
    * Region menu buttons starts
    * ***************************
    */

    // Create key, value array: key is the button menu, value is the span element. The span element is the selected value from the dropdown menu.
    var menuButtons =
    {
        '#menuTimeFrame': '#currentTimeFrame',
        '#menuStrategy': '#currentStrategy',
        '#menuSampleSize': '#currentSampleSize',
        '#menuTrade': '#currentTrade'
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
                if (key != '#menuTrade') {
                    showLatestTrade = true;
                }
                else {
                    showLatestTrade = false;
                }

                // Set the new value
                var value = $(this).text();
                $(menuButtons[key]).text(value);
                LoadTradeAsync($('#currentTimeFrame').text(),
                    $('#currentStrategy').text(),
                    $('#currentSampleSize').text(),
                    $('#currentTrade').text());
            });
        })(key);
    }

    // Mark the selected drop down item
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
    // API Call to load the selected trade
    function LoadTradeAsync(timeFrame, strategy, sampleSize, trade) {
        $.ajax({
            method: 'POST',
            url: '/papertrades/loadtrade',
            dataType: 'JSON',
            data: {
                timeFrame: timeFrame,
                strategy: strategy,
                sampleSize: sampleSize,
                trade: trade

            },
            success: function (response) {
                paperTradesVM = response;
                if (response == null) {
                    menuClicked.text(clickedMenuValue);
                    return;
                }
                SetMenuValues(trade);
                LoadImages();
                SetSelectedItemClass();
            }
        })
    }
    // Populates the drop down items after a new trade has been selected and sets the values in the spans.
    function SetMenuValues(displayedTrade) {
        var numberSampleSizes = paperTradesVM['paperTradesVM']['numberSampleSizes'];
        var tradesInSampleSize = paperTradesVM['paperTradesVM']['tradesInSampleSize'];
        // Set the SampleSize menu
        $('#currentSampleSize').text(numberSampleSizes);
        $('#menuSampleSize').empty();
        var sampleSizes = '';
        for (var i = numberSampleSizes; i > 0; i--) {
            sampleSizes += '<a class="dropdown-item" role="button">' + i + '</a>';
        }
        $('#menuSampleSize').html(sampleSizes);

        // Set the Trades menu
        if (showLatestTrade === true) {
            $('#currentTrade').text(tradesInSampleSize);
        }
        else {
            $('#currentTrade').text(displayedTrade);
        }
        $('#menuTrade').empty();

        var trades = '';
        for (var i = tradesInSampleSize; i > 0; i--) {
            trades += '<a class="dropdown-item" role="button">' + i + '</a>'
        }
        $('#menuTrade').html(trades);
    }

    // Load the images into the carousel
    function LoadImages() {
        $('#imageContainer').empty();
        var screenshots = paperTradesVM['paperTradesVM']['currentTrade']['screenshotsUrls'];

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
        newCarouselHtml += '</div>';

        $('#imageContainer').html(newCarouselHtml);
    }

    /**
    * ***************************
    * Region menu buttons ends
    * ***************************
    */
});


