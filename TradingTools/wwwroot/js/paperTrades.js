// Summernote sometimes opens with the cursor in the middle, then that causes errors. Check Review
$(function () {

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
    var paperTradesVM;
    var currentTab = '#pre'; // Always the start value
    var isEditorShown = false;
    var currentCardMenu = 'Journal';

    /**
    * ******************************
    * Region global variables ends
    * ******************************
    */

    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').trigger('submit');
    });


    /**
    * *************************
    * Region summernote starts
    * *************************
    */

    // Send the data to the controller
    function UpdateReview() {
        let dataToSend =
        {
            CurrentTrade: {
                SampleSizeId: $('#spanSampleSizeIdInput').val()
            },
            Review: {
                First: $('#first').html(),
                Second: $('#second').html(),
                Third: $('#third').html(),
                Forth: $('#forth').html(),
                summary: $('#summary').html()
            }
        };
        $.ajax({
            method: 'POST',
            url: '/papertrades/updatereview',
            contentType: "application/json; charset=utf-8",
            dataType: 'JSON',
            data: JSON.stringify(dataToSend),
            success: function (response) {
                if (response['success'] !== undefined) {

                    toastr.success(response['success']);
                }
                else {
                    toastr.error(response['error']);
                }
            }
        });
    }

    // Send the data to the controller
    function UpdateJournal() {
        let dataToSend =
        {
            CurrentTrade: {
                Id: $('#spanTradeIdInput').val()
            },
            Journal: {
                Pre: $('#pre').html(),
                During: $('#during').html(),
                Exit: $('#exit').html(),
                Post: $('#post').html()
            }
        };
        $.ajax({
            method: 'POST',
            url: '/papertrades/updatejournal',
            contentType: "application/json; charset=utf-8",
            dataType: 'JSON',
            data: JSON.stringify(dataToSend),
            success: function (response) {
                if (response['success'] !== undefined) {
                    toastr.success(response['success']);
                }
                else {
                    toastr.error(response['error']);
                }
            }
        });
    }

    // When the content is double clicked, it can be edited (summernote is displayed)
    $('#tabContentJournal').on('dblclick', function () {
        OpenEditor();
    });

    $('#tabContentReview').on('dblclick', function () {
        OpenEditor();
    });
    // On tab change
    $('button[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        if (isEditorShown) {
            SaveEditorText();
        }
        currentTab = '#' + $(e.target).attr('aria-controls');
    });

    // Open editor and show the buttons
    $('#btnEdit').on('click', function () {
        OpenEditor();
    });

    // Save the journal changes
    $('#btnSave').on('click', function () {
        SaveEditorText();
    });

    // Close the editor and show 'Edit' button
    // TODO: Changes are saved in the contentPage when they shouldn't. (Changes aren't save in the DB as expected)
    $('#btnCancel').on('click', function () {
        $('#summernote').summernote('destroy');
        $(currentTab).removeClass('d-none');
        $('#cardBody').addClass('card-body');
        isEditorShown = false;
        ToggleFooterButtons();
    });

    function ToggleFooterButtons() {
        if ($('#btnEdit').hasClass('d-none')) {
            $('#btnEdit').removeClass('d-none');
            $('.editorOnBtns').addClass('d-none');
        }
        else {
            $('#btnEdit').addClass('d-none');
            $('.editorOnBtns').removeClass('d-none');
        }
    }

    // Save the journal in the DB and toggle the buttons
    function SaveEditorText() {
        $('#cardBody').addClass('card-body');
        isEditorShown = false;
        // Save the text from the editor
        var editorText = $($('#summernote').summernote('code')).text().trim();
        // Save the text from the tab
        var oldTabContent = $(currentTab).text().trim();
        // Set the content of the tab to the value of the editor
        $(currentTab).html($('#summernote').summernote('code'));
        // Show the tab content
        $(currentTab).removeClass('d-none');
        // Close the editor
        $('#summernote').summernote('code', '');
        $('#summernote').summernote('destroy');
        ToggleFooterButtons();
        // If a change has been made, save it
        if (editorText !== oldTabContent) {
            if (currentCardMenu === 'Journal') {
                UpdateJournal();
            }
            else {
                UpdateReview();
            }
        }
    }

    // Open the summernote editor
    function OpenEditor() {
        $('#cardBody').removeClass('card-body');
        ToggleFooterButtons();
        // Hide the tabContent of the journal and show the summernote instead
        // Get the text from the tabContent
        let currentTabContent = $(currentTab).html();
        // Hide the tabContent
        $(currentTab).addClass('d-none');
        // Set the text into the editor
        $('#summernote').summernote('code', currentTabContent);
        $('#summernote').summernote('justifyLeft');
        // Display the editor
        isEditorShown = true;
    }

    /**
    * ************************
    * Region summernote ends
    * ************************
    */


    /**
     * ***************************
     * Card dropdown menu starts
     * ***************************
     */

    $('#headerMenu').on('click', '.dropdown-item', function () {
        if (isEditorShown) {
            SaveEditorText();
        }
        currentCardMenu = $(this).text();
        // Display Journal tabs
        if (currentCardMenu == 'Journal') {
            currentTab = '#pre';
            $('#journalTabHeaders').removeClass('d-none');
            $('#journalTabContent').removeClass('d-none');
            $('#reviewTabHeaders').addClass('d-none');
            $('#reviewTabContent').addClass('d-none');
        }
        // Display Review tabs
        else {
            currentTab = '#first';
            $('#journalTabHeaders').addClass('d-none');
            $('#journalTabContent').addClass('d-none');
            $('#reviewTabHeaders').removeClass('d-none');
            $('#reviewTabContent').removeClass('d-none');
        }

        // Set the text of the card menu
        if (currentCardMenu !== $('#currentMenu').text()) {
            $('#currentMenu').text(currentCardMenu);
            $(this).addClass('bg-gray-400');

            // Remove the bg color of the last selected item
            $('#headerMenu a').each(function () {
                // if a dropdown item has the bg-gray-400 class and the text of the current item being iterated is different than the text in the #currentMenu, than that was the previously selected option
                if ($(this).hasClass('bg-gray-400') && $(this).text() !== $('#currentMenu').text()) {
                    $(this).removeClass('bg-gray-400');
                }
            });
        }
    });

    /**
    * ***************************
    * Card dropdown menu ends
    * ***************************
    */


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
                LoadTradeAsync( $('#spanTimeFrame').text(),
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
    // API call to load the selected trade
    function LoadTradeAsync(timeFrame, strategy, sampleSize, trade, showLastTrade, sampleSizeChanged) {
        $.ajax({
            method: 'POST',
            url: '/papertrades/loadtrade',
            dataType: 'JSON',
            data: {
                timeFrame: timeFrame,
                strategy: strategy,
                sampleSize: sampleSize,
                trade: trade,
                showLastTrade: showLastTrade,
                sampleSizeChanged: sampleSizeChanged

            },
            success: function (response) {
                if (response['error'] !== undefined) {
                    toastr.error(response['error']);
                    // Set the old value
                    menuClicked.text(clickedMenuValue);
                    return;
                }
                else if (response['info'] !== undefined) {
                    toastr.info(response['info']);
                    // Set the old value
                    menuClicked.text(clickedMenuValue);
                    return;
                }
                paperTradesVM = response;
                // Set the new trade id
                $("#spanTradeIdInput").val(response['paperTradesVM']['currentTrade']['id']);
                // Set the sample size id
                $("#spanSampleSizeIdInput").val(response['paperTradesVM']['currentTrade']['samplesizeid']);
                SetMenuValues(trade);
                SetSelectedItemClass();
                LoadImages();
                LoadReview();
                LoadJournal();
            }
        });
    }

    // Loads the review of the sample size
    function LoadReview() {
        // Activate the 'First' tab
        $('#first-tab').trigger('click');
        // Set the values
        $('#first').html(paperTradesVM['paperTradesVM']['review']['first']);
        $('#second').html(paperTradesVM['paperTradesVM']['review']['second']);
        $('#third').html(paperTradesVM['paperTradesVM']['review']['third']);
        $('#forth').html(paperTradesVM['paperTradesVM']['review']['forth']);
        $('#summary').html(paperTradesVM['paperTradesVM']['review']['summary']);
    }

    // Loads the journal of the trade
    function LoadJournal() {
        // Activate the 'Pre' tab
        $('#pre-tab').trigger('click');
        // Set the values
        $('#pre').html(paperTradesVM['paperTradesVM']['journal']['pre']);
        $('#during').html(paperTradesVM['paperTradesVM']['journal']['during']);
        $('#exit').html(paperTradesVM['paperTradesVM']['journal']['exit']);
        $('#post').html(paperTradesVM['paperTradesVM']['journal']['post']);
    }

    // Populate the drop down items after a new trade has been selected and set the values in the spans.
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

        // Menu card header
        currentCardMenu = 'Journal';
        $('#cardMenuJournal').trigger('click');
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
})

