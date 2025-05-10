var filename = "";
window.onload = function () {
    filename = document.getElementById("filename").innerHTML;
};

function showFileSize() {
    var input, file;
    document.getElementById('ppthidden').style.visibility = "hidden";
    if (!window.FileReader) {
        bodyAppend("p", "The file API isn't supported on this browser yet.");
        return;
    }

    input = document.getElementById('Files');
    if (!input) {
        bodyAppend("p", "Um, couldn't find the fileinput element.");
    }
    else if (!input.files) {
        bodyAppend("p", "This browser doesn't seem to support the `files` property of file inputs.");
    }
    else if (!input.files[0]) {
        document.getElementById("filename").innerHTML = filename;
        document.getElementById("limit").innerHTML = "";
        document.getElementById("submit").disabled = true;
    }
    else {
        document.getElementById("filename").innerHTML = input.files[0].name;
        var limit = 0;
        limit = limit + input.files[0].size;
        document.getElementById("limit").innerHTML = parseInt(limit / (1024 * 1024)) + " Mb";
        if (limit / (1024 * 1024) > 2048) {
            document.getElementById("limit").style.color = "red";
            document.getElementById("submit").disabled = true;
        }
        else {
            var types = ["jpg", "png", "bmp", "avi", "mkv", "mp4", "gif", "ppt", "pptx"];
            var isvalidfile = 0;
            for (var i = 0; i < types.length; i++) {
                if (getFileExtension(input.files[0].name).startsWith(types[i])) {
                    isvalidfile = 1;
                    if (getFileExtension(input.files[0].name).startsWith('ppt') || getFileExtension(input.files[0].name).startsWith('pptx'))
                    {
                        document.getElementById('ppthidden').style.visibility = "visible";
                    }
                }
            }
            if (isvalidfile === 1) {
                document.getElementById("limit").style.color = "green";
                document.getElementById("submit").disabled = false;
            }
            else {
                document.getElementById("limit").innerHTML = "is not a valid file format";
                document.getElementById("limit").style.color = "red";
                document.getElementById("submit").disabled = true;

            }
        }
    }
}


function bodyAppend(tagName, innerHTML) {
    var elm;

    elm = document.createElement(tagName);
    elm.innerHTML = innerHTML;
    document.body.appendChild(elm);
}


function getFileExtension(filename) {
    var spl = filename.split('.');
    return spl[spl.length - 1];
}