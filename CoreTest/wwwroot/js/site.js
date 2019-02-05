﻿
var filearray = [];///массив отправляемых файлов
$(document).on('submit', '#post_file_form', function (event) {
        event.preventDefault();
        $('#loaderimg').show();
        var token = $("input[type='hidden'][name$='__RequestVerificationToken']").val();
        var formdata = new FormData();
        $(filearray).each(function (index) {
            formdata.append('FormFile', filearray[index]);
        });
        formdata.append('__RequestVerificationToken', token);
        $.ajax({
            type: 'POST',
            url: 'Photos/Index',
            data: formdata,
            processData: false,
            contentType: false,
            success: function (result) {
                $('#btn_reset').click();
                $('#g_table').find('tbody:last').append(result);
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
        var temp_path = URL.createObjectURL(event.target.files[0]);
        var temp_name = event.target.files[0].name;
        var valid = $('#post_file_form').valid();
        if (valid) {
            filearray.push(event.target.files[0]);

            var name_temp = event.target.files[0].name;
            var namefile = name_temp.substring(0, name_temp.indexOf('.'));

            var tempimg = $('<img>', { id: 'pre_Img', src: temp_path }).addClass('img-thumbnail mx-auto d-block').css({ 'max-height': '480px', 'max-width': '320px' });
            $('#g_table').find('tbody:last').append('<tr id="addsite_' + namefile + '"><td><a id="single_image" href="' + temp_path +
                '"></a></td><td>' + temp_name + '</td><td><button id="btn_' + namefile + '" class="btn btn-outline-danger font-weight-bold">Delete</button></td></tr>');
            $('#g_table').find('tbody tr:last-child #single_image').append($(tempimg.prop('outerHTML')));


            $('#btn_' + namefile).click(function () {
                var row = $('#g_table').find('#addsite_' + namefile);
                row.fadeOut(100, function () {
                    row.remove();
                });
                $(filearray).each(function (index) {
                    var temp = filearray[index].name;
                    var namefile1 = temp.substring(0, temp.indexOf('.'));
                    if (namefile1 === namefile) {
                        filearray.splice(index, 1);
                    }
                });
                if (filearray.length === 0) {
                    $('#btn_reset').click();
                }
            });
        }
        valid.form();
    });

    $('#btn_reset').click(function () {
        $(filearray).each(function (index) {
            var temp = filearray[index].name;
            var namefile1 = temp.substring(0, temp.indexOf('.'));
            var row = $('#g_table').find('#addsite_' + namefile1);
            row.fadeOut(200, function () {
                row.remove();
            });
        });
        filearray = [];
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
            row.fadeOut(200, function () {
                row.remove();
            });
        }
    });
});



