var serverfile = [];// список на удаление с БД
var temparr = [];// список для хранения уникальных имен файлов
var filearray = [];///список отправляемых файлов
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
                btnclickregistration();
                $("a#single_image").fancybox();
            },
            error: function (error) {
                $('#loaderimg').hide();
            }
        });
});

function btnclickregistration() { 
    $("button[id^='serverbtn_']").click(function () {
        var btn = $(this).attr('id');
        var id = btn.substring(btn.indexOf('_') + 1);
        serverfile.push(id);
        var row = $('#g_table').find('#serverimg_' + id);
        row.fadeOut(200, function () {
        });
    });
}


$('#sendbuttonex').click(function () {
    if (serverfile.length === 0 || filearray.length > 0) {
       $('#post_file_form').submit();
    }
    if (serverfile.length > 0) {
      $('#delete_form').submit();
   }
    
});

$(document).ready(function () {
    $('#loaderimg').hide();
    $("a#single_image").fancybox();
    $('input[type=file]').change(function (event) {
        var temp_path = URL.createObjectURL(event.target.files[0]);
        var valid = $('#post_file_form').valid();
        if (valid) {
            filearray.push(event.target.files[0]);
            var name_temp = event.target.files[0].name;
            var namefile = name_temp.substring(0, name_temp.indexOf('.')) + "_" + (new Date()).getTime().toString(36);
            temparr.push(namefile);
            var tempimg = $('<img>', { id: 'pre_Img', src: temp_path }).addClass('img-thumbnail mx-auto d-block').css({ 'max-height': '480px', 'max-width': '320px' });
            $('#g_table').find('tbody:last').append('<tr id="addsite_' + namefile + '"><td><a id="single_image" href="' + temp_path +
                '"></a></td><td>' + name_temp + '</td><td><button id="btn_' + namefile + '" class="btn btn-outline-danger font-weight-bold">Delete</button></td></tr>');
            $('#g_table').find('tbody tr:last-child #single_image').append($(tempimg.prop('outerHTML')));
            $("a#single_image").fancybox();

            $('#btn_' + namefile).click(function () {
                var row = $('#g_table').find('#addsite_' + namefile);
                row.fadeOut(200, function () {
                    row.remove();
                });

                var myindex = $.inArray(namefile, temparr);
                filearray.splice(myindex, 1);
                temparr.splice(myindex,1);

                if (filearray.length === 0) {
                    $('#btn_reset').click();
                }
            });
        }
        valid.form();
    });
    btnclickregistration();
    $('#btn_reset').click(function () {
        $(temparr).each(function (index) {
            var name = temparr[index];
            var row = $('#g_table').find('#addsite_' + name);
            row.fadeOut(200, function () {
                row.remove();
            });
        });
        $(serverfile).each(function (index) {
            var row = $('#g_table').find('#serverimg_' + serverfile[index]);
            row.fadeIn(200, function () {
               
            });
        });
        serverfile = [];
        filearray = [];
        temparr = [];
    });
});

$(document).on('submit', '#delete_form', function (event) {
    event.preventDefault();
    var token = $("input[type='hidden'][name$='__RequestVerificationToken']").val();
    var formdata = new FormData();
    $(serverfile).each(function (index) {
        formdata.append('Id', serverfile[index]);
    });
    serverfile = [];
    formdata.append('__RequestVerificationToken', token);
    $.ajax({
        type: 'POST',
        url: 'Photos/Delete',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {

        }
    });
});



