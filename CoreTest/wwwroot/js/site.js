// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).on('submit', '#post_file_form', function (event) {
    event.preventDefault();
    $('#loaderimg').show();
    var token = $("input[type='hidden'][name$='__RequestVerificationToken']").val();
    var formdata = new FormData();
    var file = $('#post_file_form').find('#FormFile').get(0).files[0];
    formdata.append('FormFile', file);
    formdata.append('__RequestVerificationToken', token);
    $('#pre_Img').remove();
    $.ajax({
        type: 'POST',
        url: 'Photos/Index',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            $('#g_table').find('tbody:last').append(result);
            $('#btn_reset').click();
            $('#loaderimg').hide();
            $("a#single_image").fancybox();
        },
        error: function (error) {
            $('#loaderimg').hide();
        }
    });
});

$(document).ready(function () {
    $('#loaderimg').hide();
    $("a#single_image").fancybox();
    $('input[type=file]').change(function (event) {
        $('#pre_Img').remove();
        var temp_path = URL.createObjectURL(event.target.files[0]);
        var valid = $('#post_file_form').valid();
        if (valid) {
            $('#pre_img_div').append($('<img>', { id: 'pre_Img', src: temp_path }).addClass('img-thumbnail').css({ 'max-height': '150px', 'max-width': '200px' }));
        }
        valid.form();
    });
    $('#btn_reset').click(function () {
        $('#pre_Img').remove();
    });

});

$(document).on('submit', '.delete_form', function (event) {
    event.preventDefault();
    var token = $("input[type='hidden'][name$='__RequestVerificationToken']").val();
    var formdata = new FormData();
    var my_id = $(this).attr('id');
    var my_id_with = '#' + my_id;
    formdata.append('Id', my_id);
    formdata.append('__RequestVerificationToken', token);
    $.ajax({
        type: 'POST',
        url: 'Photos/Delete',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            var row = $('#g_table').find(my_id_with);
            row.fadeOut(1000, function () {
                row.remove();
            });
        }
    });
});



