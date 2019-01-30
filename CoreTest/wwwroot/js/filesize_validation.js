
$.validator.addMethod("filesize", function (value, element, params) {
    var val_file_size = element.attributes[6].value;
    var file_size = element.files[0].size;

    if (val_file_size > file_size) {
        return true;
    }
    return false;
});

$.validator.unobtrusive.adapters.add('filesize', ['value'], function (options) {
    options.rules['filesize'] = { targetvalue: options.params.value };
    options.messages['filesize'] = options.message;
});