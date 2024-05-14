$(document).ready(function () {
    // When a new trade has to be loaded, one of the buttons has to be clicked (either TimeFrame, Strategy..). In case no trade exists for the selection, set the prior value. Used in LoadTradeAsync()
    var menuClicked;
    var clickedMenuValue;
    var showLatestTrade;
    var paperTradesVM;
    var currentJournalTab;

    $('#tabContent').on('dblclick', function () {
        // Hide the tabContent of the journal and show the summernote instead
        $('#show' + currentJournalTab).css('display', 'none');
        $('.summernote').summernote('code', 'my text');
        $('#edit' + currentJournalTab).css('display', 'block')
    });

    $('button[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        currentJournalTab = $(e.target).attr("aria-controls"); // activated tab
        currentJournalTab = capitalizeFirstLetter(currentJournalTab);
        console.log(currentJournalTab);
    });

    //$('#menuJournal').on('click', '.dropdown-item', function () {
    //    console.log(paperTradesVM);
    //    var journal = $(this).text();
    //})
    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').submit();
    });

    // Get all element
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

    function capitalizeFirstLetter(text) {
        return text.charAt(0).toUpperCase() + text.slice(1)
    }

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
});


