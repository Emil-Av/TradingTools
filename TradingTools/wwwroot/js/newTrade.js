﻿$(function () {
    /**
   * ******************************
   * Region global variables starts
   * ******************************
   */

    var uploadedFiles = [];

    /**
      * ****************************
      * Region global variables ends
      * ****************************
      */


    /**
    * ***************************
    * Region menu buttons starts
    * ***************************
    */
    // Create key, value array: key is the button menu, value is the span element. The span element is the selected value from the dropdown menu.
    var menuButtons =
    {
        '#dropdownBtnStatus': '#spanStatus',
        '#dropdownBtnTimeFrame': '#spanTimeFrame',
        '#dropdownBtnStrategy': '#spanStrategy',
        '#dropdownBtnTradeType': '#spanTradeType',
        '#dropdownBtnTradeSide': '#spanTradeSide',
        '#dropdownBtnOrderType': '#spanOrderType'
    };

    // Attach a click event for each <a> element of each menu.
    for (var key in menuButtons) {
        (function (key) {
            // Change the value of the span in the button
            $(key).on('click', '.dropdown-item', function () {
                // Set the new value
                var value = $(this).text();
                $(menuButtons[key]).text(value);
                SetSelectedItemClass(key);
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
     * Region event handlers begins
     * ***************************
     */

    $('#headerMenu').on('click', '.dropdown-item', function () {
        var currentCardMenu = $(this).text();
        console.log(currentCardMenu);
        // Display TradeData tabs
        if (currentCardMenu == 'Trade Data') {
            $('#tradeDataTabHeaders').removeClass('d-none');
            $('#tradeDataTabContent').removeClass('d-none');
            $('#researchDataTabHeaders').addClass('d-none');
            $('#researchDataTabContent').addClass('d-none');
        }
        // Display Research Tabs
        else {
            $('#researchDataTabHeaders').removeClass('d-none');
            $('#researchDataTabContent').removeClass('d-none');
            $('#tradeDataTabHeaders').addClass('d-none');
            $('#tradeDataTabContent').addClass('d-none');
        }

        // Set the text of the card menu
        if (currentCardMenu !== $('#currentMenu').text()) {
            $('#currentMenu').text(currentCardMenu);
            $(this).addClass('bg-gray-400');
        }

        // Remove the bg color of the last selected item
        $('#headerMenu a').each(function () {
            // if a dropdown item has the bg-gray-400 class and the text of the current item being iterated is different than the text in the #currentMenu, than that was the previously selected option
            if ($(this).hasClass('bg-gray-400') && $(this).text() !== $('#currentMenu').text()) {
                $(this).removeClass('bg-gray-400');
            }
        });
    });

    $('#btnSave').on('click', function () {
        saveTrade();
    });

    $('#btnClear').on('click', function () {
        clearFields();
    });

    // After uploading screenshots, the 'change' event is triggered. Save the files in the global variable and display the file names
    $('input[type="file"]').change(function (e) {
        var files = $(this)[0].files; // Get the files array from the input element

        // Iterate through each selected file to append it to FormData
        for (var i = 0; i < files.length; i++) {
            uploadedFiles.push(files[i]);
        }
        // Sort the screenshots to be in ascending order based on the lastModified property
        uploadedFiles.sort(function (a, b) {
            return a.lastModified - b.lastModified;
        });

        displayNames();
    });

    /**
     * ***************************
     * Region event handlers ends
     * ***************************
     */

    /**
     * ***************************
     * Region functions begins
     * ***************************
     */
    function displayNames() {
        var fileList = $('#fileList');
        fileList.removeClass('text-center').addClass('text-left');
        fileList.empty();

        for (var i = 0; i < uploadedFiles.length; i++) {
            fileName = (i + 1) + '. ' + uploadedFiles[i].name;
            fileList.append('<p class="text-truncate">' + fileName + '</p>');
        }
    }

    function saveTrade() {

        var formData = new FormData();

        // Trades without screenshots should not be saved
        if (uploadedFiles.length == 0) {
            toastr.error("No screenshots uploaded.");
            return;
        }

        for (var i = 0; i < uploadedFiles.length; i++) {
            formData.append('files', uploadedFiles[i]);
        }

        var tradeParams = {};
        tradeParams['status'] = $('#spanStatus').text();
        tradeParams['timeFrame'] = $('#spanTimeFrame').text();
        tradeParams['strategy'] = $('#spanStrategy').text();
        tradeParams['tradeType'] = $('#spanTradeType').text();
        tradeParams['tradeSide'] = $('#spanTradeSide').text();
        tradeParams['orderType'] = $('#spanOrderType').text();

        formData.append('tradeParams', JSON.stringify(tradeParams));

        var researchData = {};
        $('#cardBody [data-research]').each(function () {
            var bindProperty = $(this).data('research');
            researchData[bindProperty] = $(this).val();
        });

        formData.append('researchData', JSON.stringify(researchData));

        var tradeData = {};
        $('#cardBody [data-trade-data]').each(function () {
            var bindProperty = $(this).data('trade-data');
            tradeData[bindProperty] = $(this).val();
        });

        const targetsArray = tradeData['TargetsDisplay'].split(',').map(value => value.trim());
        if (tradeData['TargetsDisplay'].length == 0) {
            tradeData['TargetsDisplay'] = '';
        }
        else {
            tradeData['TargetsDisplay'] = targetsArray.map(value => parseFloat(value));
        }

        formData.append('tradeData', JSON.stringify(tradeData));

        $.ajax({
            type: 'POST',
            url: '/newtrade/savenewtrade',
            processData: false, // Don't process the files, otherwise jQuery will transform the data into a query string
            contentType: false, // Set content type to false as FormData will handle it
            data: formData,
            success: function (response) {
                if (response['success'] !== undefined) {
                    toastr.success(response['success']);
                    clearFields();
                }
                else if (response['error'] !== undefined) {
                    toastr.error(response['error']);
                }
            },
            error: function (error) {
                // Handle error
                console.error('Error uploading files:', error);
            }
        });
    };

    function clearFields() {
        // Clear fields with data-research attribute
        $('[data-research]').each(function () {
            if ($(this).is('input')) {
                // Clear the rest of the input fields
                $(this).val('');
            }
            // Clear the select fields
            else if ($(this).is('select')) {
                $(this).prop('selectedIndex', 1);
            }
        });

        // Clear fields without data-research attribute
        $('input:not([data-research])').each(function () {
            // Clear the files input
            $(this).val('');
        });
        $('#fileList').empty();
        $('#fileList').append('<p class="text-truncate">No uploaded files.</p>')
        $('#formUploadFiles').trigger('reset');
        uploadedFiles = [];
        $('select:not([data-research])').each(function () {
            $(this).prop('selectedIndex', 1);
        });
    }

    /**
     * ***************************
     * Region functions ends
     * ***************************
     */

})