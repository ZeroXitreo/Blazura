var convertBase64ToBlob = function (contentType: string, data: string) {
    var file = new Blob([data], { type: `${contentType};base64` });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
    return fileURL;
}
