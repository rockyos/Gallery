$.validator.addMethod("fileextensions", function (value, element, params) {
    var ext_string = element.attributes[4].value;
    var ext_array = ext_string.split(',');
    var file_ext = value.substring(value.indexOf('.') + 1);

    if (ext_array.includes(file_ext)) {
        return true;
    }
    return false;
});

$.validator.unobtrusive.adapters.add('fileextensions', ['value'], function (options) {
    options.rules['fileextensions'] = { targetvalue: options.params.value };
    options.messages['fileextensions'] = options.message;
});

