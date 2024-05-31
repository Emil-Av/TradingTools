
$(function () {
    // After a .zip file is uploaded, the 'change' event is triggered, this submits the form and sends the .zip file to the controller
    $('#fileInput').on('change', function () {
        $('#formUploadFile').trigger('submit');
    });
});