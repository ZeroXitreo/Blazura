var downloadBase64 = function (filename, contentType: string, data) {
    console.log(filename, contentType, data);
    // Create the URL
    const file = new File([data], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    URL.revokeObjectURL(exportUrl);
    a.remove();
}

var openBase64Blob = function (contentType: string, data) {
    var file = new Blob([data], { type: `${contentType};base64` });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
}

var openBase64Url = function (contentType: string, data) {
    window.open(`data:${contentType};base64,` + data);
}