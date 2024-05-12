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

function ShowNotification(message) {
        toastr.success(message);
}