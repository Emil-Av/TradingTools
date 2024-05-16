$(document).click(function (event) {
    // Hide the menu if it's clicked outside of it
    $(document).on('click', function (event) {
        var menu = $('#menuTrades');
        var target = $(event.target);
        var isClickInsideMenu = target.closest('#menuTrades').length > 0;

        if (!isClickInsideMenu && menu.hasClass('show')) {
            // If the click is outside of the menu and the menu is open, collapse it
            menu.removeClass('show');
        }
    });
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
