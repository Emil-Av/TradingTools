
// Left bar (menu 'Trades'): Hide the menu if it's clicked outside of it
$(document).on('click', function (event) {
    var menu = $('#dropdownBtnTrades');
    var target = $(event.target);
    var isClickInsideMenu = target.closest('#dropdownBtnTrades').length > 0;

    if (!isClickInsideMenu && menu.hasClass('show')) {
        // If the click is outside of the menu and the menu is open, collapse it
        menu.removeClass('show');
    }
});


// Toggles the visibility of the loading indicator and the main content of a page
var loadingIndicator = $('#loadingIndicator');
var content = $('#myContent');
function ShowLoadingIndicator() {
    content.removeClass('showMyContent').addClass('hideMyContent');
    loadingIndicator.css('display', 'block');
}

function HideLoadingIndicator() {
    content.removeClass('hideMyContent').addClass('showMyContent');
    loadingIndicator.css('display', 'none');
}

// Gets the data from the elements which have data-trade-data attribute. Used in multiple views.
function GetTradeData() {
    var tradeData = {};
    $('#cardBody [data-trade-data]').each(function () {
        var bindProperty = $(this).data('trade-data');
        tradeData[bindProperty] = $(this).val();
    });

    const targetsArray = tradeData['TargetsDisplay'].split(',').map(value => value.trim());
    if (tradeData['TargetsDisplay'].length == 0) {
        tradeData['TargetsDisplay'] = [];
    }
    else {
        tradeData['TargetsDisplay'] = targetsArray.map(value => parseFloat(value));
    }

    return tradeData;
}
