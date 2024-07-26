$(function () {
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
        '#dropdownBtnTimeFrame': '#spanTimeFrame',
        '#dropdownBtnStrategy': '#spanStrategy',
        '#dropdownBtnTradeType': '#spanTradeType',
        '#dropdownBtnTradeSide': '#spanTradeSide'
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
     * Region functions begins
     * ***************************
     */

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

        displayNames();
    });

    function displayNames() {
        var fileList = $('#fileList');
        fileList.removeClass('text-center').addClass('text-left');
        fileList.empty();

        for (var i = 0; i < uploadedFiles.length; i++) {
            fileName = (i + 1) + '. ' + uploadedFiles[i].name;
            fileList.append('<p class="text-truncate">' + fileName + '<span class="text-success ml-2">&#10003;</span</p>');
        }
    }

    function saveTrade() {

        var formData = new FormData();

        for (var i = 0; i < uploadedFiles.length; i++) {
            formData.append('files', uploadedFiles[i]);
        }

        var tradeParams = {};
        tradeParams['timeFrame'] = $('#spanTimeFrame').text();
        tradeParams['strategy'] = $('#spanStrategy').text();
        tradeParams['tradeType'] = $('#spanTradeType').text();
        tradeParams['tradeSide'] = $('#spanTradeSide').text();

        formData.append('tradeParams', JSON.stringify(tradeParams));

        var tradeData = {};
        $('#cardBodyResearch [data-bindresearch]').each(function () {
            var bindProperty = $(this).data('bindresearch');
            tradeData[bindProperty] = $(this).val();
        });

        formData.append('tradeData', JSON.stringify(tradeData));

        $.ajax({
            url: '/newtrade/savenewtrade',
            type: 'POST',
            data: formData,
            processData: false, // Don't process the files, otherwise jQuery will transform the data into a query string
            contentType: false, // Set content type to false as FormData will handle it
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
        // Clear fields with data-bindResearch attribute
        $('[data-bindResearch]').each(function () {
            if ($(this).is('input')) {
                // Clear the rest of the input fields
                $(this).val('');
            }
            // Clear the select fields
            else if ($(this).is('select')) {
                $(this).prop('selectedIndex', 1);
            }
        });

        // Clear fields without data-bindResearch attribute
        $('input:not([data-bindResearch])').each(function () {
            // Clear the files input
            $(this).val('');
        });
        $('#fileList').empty();
        $('#fileList').append('<p class="text-truncate">No uploaded files.</p>')
        $('#formUploadFiles').trigger('reset');
        uploadedFiles = [];
        console.log('form resetted');
        $('select:not([data-bindResearch])').each(function () {
            $(this).prop('selectedIndex', 1);
        });
    }

    /**
     * ***************************
     * Region functions ends
     * ***************************
     */

})