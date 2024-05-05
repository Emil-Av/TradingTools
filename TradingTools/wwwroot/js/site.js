// Toggles the visibility of the loading indicator and the main content of a page
function ShowHideLoadingIndicator() {
    var loadingIndicator = $('#loadingIndicator');
    var content = $('#myContent');

    if (loadingIndicator.css('display') == 'block') {
        alert('1');
        content.removeClass('hideMyContent');
        content.addClass('showMyContent');
        loadingIndicator.css('display', 'none');
    }
    else {
        alert('2');
        content.removeClass('showMyContent');
        content.addClass('hideMyContent');
        loadingIndicator.css('display', 'block');
    }
}
