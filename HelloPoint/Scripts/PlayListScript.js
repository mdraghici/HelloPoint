var selecteditemindex = 0;
var currentliplaying = -1;
var oldindex = -1;
var isplaying = false;
window.onload = function () {
    removeInfo("playlist");
    var playlistdiv = document.getElementById("playlist");
    var htmldata = '<div style="vertical-align:bottom; text-align:center;height:400px;">\
                            <img src="../Content/loading.gif" width="120" height="120">\
                        </div>';
    playlistdiv.insertAdjacentHTML('afterbegin', htmldata);
    getPlayList();
    selecteditemindex = -1;
    oldindex = -1;
};

function getPlayList() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/GetPlayList',
        data: {},
        success: function (data) {
            RenderPlayList(data, selecteditemindex);
        },
        error: {}
    });

}


function RenderPlayList(data, selecteditemindex) {
    removeInfo("playlist");
    var playlistdiv = document.getElementById("playlist");
    var result = JSON.parse(data);
    var htmldata = "";
    isplaying = false;
    if (result.length > 0) {
        htmldata += '<table class="playlistTableClass" id="playlistTableID' + '">';
        for (var i = 0; i < result.length; i++) {
            htmldata += '<tr id="playlistElementID' + i + '" onclick="changeBackgroundPlaylist(' + i + ');getDetails(' + i + ',' + result[i].PlayTime + ',' + result[i].Repetitions + ')">';
            if (result[i].IsSelected) {
                if (result[i].IsPaused) {
                    htmldata += '<td><img src="../Content/pauseIcon.gif" width="30" height="30" /></td>';
                    isplaying = true;
                    currentliplaying = i;
                }
                else {
                    htmldata += '<td><img src="../Content/playIcon.png" width="30" height="30" /></td>';
                    isplaying = true;
                    currentliplaying = i;
                }
            } else {

                if (contains([".avi", ".mpg", ".mp4", ".mkv", ".wmv"], result[i].Type)) {
                    htmldata += '<td><img src="../Content/video.png" width="30" height="30" /></td>';
                }
                else if (contains([".jpg", ".bmp", ".png"], result[i].Type)) {
                    htmldata += '<td><img src="../Content/image.png" width="30" height="30" /></td>';
                }
                else if (".gif" === result[i].Type) {
                    htmldata += '<td><img src="../Content/gif.ico" width="30" height="30" /></td>';
                }
                else if (contains([".ppt", ".pptx"], result[i].Type)) {
                    htmldata += '<td><img src="../Content/ppt.png" width="30" height="30" /></td>';
                }
            }
            htmldata += '<td class="descrioption">' + result[i].Description + '</td>';
            htmldata += '<td><button id="upelement" name="upelement" title="Move up" onclick="moveUpElement(' + i + ')"><img src="../Content/upArrow.png" width="15" height="15"/></button></td>\
                       <td><button id="downelement" name="downelement" title="Move down" onclick="moveDownElement(' + i + ')"><img src="../Content/downArrow.png" width="15" height="15" /></button></td>\
                       <td><button id="deleteplayelement" name="deleteplayelement" title="Delete from playlist" onclick="deleteElement(' + i + ')"><img src="../Content/cancelBtn.png" width="15" height="15"/></button></td>\
                      </tr>';
        }
        htmldata += '</table>';
    }
    playlistdiv.insertAdjacentHTML('afterbegin', htmldata);
    if (selecteditemindex !== -1) {
        for (var j = 0; j < result.length; j++) {
            if (j === selecteditemindex) {
                document.getElementById("playlistElementID" + j).click(changeBackgroundPlaylist(j));
            }
        }
    }
    else {
        if (isplaying === true && oldindex !== selecteditemindex)
            removeInfo("playElementDetailsContent");
        else if (isplaying === false)
            removeInfo("playElementDetailsContent");
    }
}

function contains(a, obj) {
    var i = 0;
    while (i < a.length) {

        if (a[i] === obj) {
            return true;
        }
        i++;
    }
    return false;
}


function removeInfo(obj) {
    var myNode = document.getElementById(obj);
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }
}


function changeBackground() {
    var rows = document.querySelectorAll("#filesTableID tr");

    for (var i = 0; i < rows.length; i++) {
        rows[i].addEventListener('click', function () {
            [].forEach.call(rows, function (el) {
                el.classList.remove("highlight");
            });
            this.className += ' highlight';
        }, false);
    }
}


function changeBackgroundPlaylist(ind) {
    selecteditemindex = ind;
    var rows = document.querySelectorAll("#playlistTableID tr");

    for (var i = 0; i < rows.length; i++) {
        rows[i].addEventListener('click', function () {

            [].forEach.call(rows, function (el) {
                el.classList.remove("highlight");
            });
            this.className += ' highlight';
        }, false);
    }

}




function addElementToPlayList(_guid, _type, _description) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    if (_type === ".ppt" || _type === ".pptx") {
        $.ajax({
            url: VirtualDirectory + '/VerifyElementIsDone',
            data: { 'guid': _guid, 'type': _type },
            success: function (data) {
                if (data === '0')
                    alert("Loading ... please wait");
                else addPlaylist(_guid, '.mp4', _description);
            },
            error: function () {
            }
        });
    } else {
        addPlaylist(_guid, _type, _description);
    }
}


function addPlaylist(_guid, _type, _description) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/AddElementToPlayList',
        data: { 'guid': _guid, 'type': _type, 'description': _description },
        success: function (data) {
            // put result of action into element with class "result"
            RenderPlayList(data);
        },
        error: function () {
        }
    });
}

function play() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    isplaying = true;
    var pi = selecteditemindex;
    if (selecteditemindex === -1)
        pi = 0;
    $.ajax({
        url: VirtualDirectory + '/Play',
        data: { 'id': pi },
        success: function (data) {
            RenderPlayList(data, selecteditemindex);
            oldindex = selecteditemindex - 1;
            document.getElementById("playlistElementID" + selecteditemindex).click(changeBackgroundPlaylist(pi));
        },
        error: function () {
        }
    });
    //Refresh();
}

function pause() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    isplaying = true;
    var pi = selecteditemindex;
    if (selecteditemindex === -1)
        pi = 0;
    $.ajax({
        url: VirtualDirectory + '/Pause',
        data: {},
        success: function (data) {
            RenderPlayList(data, selecteditemindex);
            oldindex = selecteditemindex - 1;
            document.getElementById("playlistElementID" + selecteditemindex).click(changeBackgroundPlaylist(pi));
        },
        error: function () {
        }
    });
    //Refresh();
}

function stop() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    isplaying = false;
    currentliplaying = -1;
    $.ajax({
        url: VirtualDirectory + '/Stop',
        data: {},
        success: function (data) {
            RenderPlayList(data, selecteditemindex);
            oldindex = selecteditemindex - 1;
            document.getElementById("playlistElementID" + selecteditemindex).click(changeBackgroundPlaylist(selecteditemindex));
        },
        error: function () {
        }
    });
}

function clearPlaylist(data) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    currentliplaying = -1;
    $.ajax({
        url: VirtualDirectory + '/ClearPlaylist',
        data: {},
        success: function () {
            RenderPlayList(data, -1);
        },
        error: function () {
        }
    });
}

function moveUpElement(i) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/MoveUp',
        data: { 'id': i },
        success: function (data) {
            selecteditemindex = i - 1;
            RenderPlayList(data, selecteditemindex);
        },
        error: function () {
        }
    });
}


function moveDownElement(i) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;

    $.ajax({
        url: VirtualDirectory + '/MoveDown',
        data: { 'id': i },
        success: function (data) {
            selecteditemindex = i + 1;
            RenderPlayList(data, selecteditemindex);
        },
        error: function () {
        }
    });
}


function deleteElement(i) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    removeInfo("playElementDetailsContent");
    $.ajax({
        url: VirtualDirectory + '/RemoveFromPlaylist',
        data: { 'id': i },
        success: function (data) {
            RenderPlayList(data, i - 1);
        },
        error: function () {
        }
    });
}


function getDetails(id, playtime, repeatnumber) {
    if (oldindex !== selecteditemindex) {
        oldindex = selecteditemindex;
        removeInfo("playElementDetailsContent");
        var proplistdiv = document.getElementById("playElementDetailsContent");
        var htmldata = "";
        if (isplaying === false && id === selecteditemindex) {
            htmldata = 'PlayTime : <label>' + playtime + ' </label> (seconds)<br />';
            htmldata += 'Repetitions : <input type="number" id="repeatnumber" name="repeatnumber" value=' + repeatnumber + ' min="1" max="100" >';
            htmldata += '<button id="upDateElement" name="upDateElement" title="UpDate the current element from playlist" onclick="upDateElement(' + id + ')"><img src="../Content/saveIcon.png" width="30" height="30"/></button>';
        }
        else if (isplaying === true && id === selecteditemindex) {
            if (currentliplaying === id) {
                htmldata = 'PlayTime  :  <label>' + playtime + '</label> (seconds)<br />';
                htmldata += 'Repetitions : <input type="number" id="repeatnumber" name="repetations" value=' + repeatnumber + ' min="1" max="100" >';
            }
            else {
                htmldata = 'PlayTime  :<label>' + playtime + '</label> (seconds)<br />';
                htmldata += 'Repetitions : <input type="number" id="repeatnumber" name="repetations" value=' + repeatnumber + ' min="1" max="100" >';
                htmldata += '<button id="upDateElement" name="upDateElement" title="UpDate the current element from playlist" onclick="upDateElement(' + id + ')"><img src="../Content/saveIcon.png" width="30" height="30"/></button>';
            }
        }
        proplistdiv.insertAdjacentHTML('afterbegin', htmldata);
    }
}

function upDateElement(id) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    val = document.getElementById('repeatnumber').value;
    $.ajax({
        url: VirtualDirectory + '/ModifyRepeatNumber',
        data: { 'id': id, 'repeatnumber': val },
        success: function (data) {
            RenderPlayList(data, selecteditemindex);
        },
        error: function () {
        }
    });
}

function editElement(guid) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    var isinlist = false;
    $.ajax({
        url: VirtualDirectory + '/GetPlayList',
        data: {},
        success: function (data) {
            var result = JSON.parse(data);
            for (var i = 0; i < result.length; i++) {
                if (result[i].Guid === guid) {
                    alert("The selected item is in playlist therefor it can`t be modified");
                    isinlist = true;
                    break;
                }
            }
            if (isinlist === false)
                location.href = VirtualDirectory + "/EditUploadedFile?guid=" + guid;
        },
        error: function () {
        }
    });
}

function deleteFromDbDisk(guid, type) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    var isinlist = false;
    $.ajax({
        url: VirtualDirectory+'/GetPlayList',
        data: {},
        success: function (data) {
            var result = JSON.parse(data);
            for (var i = 0; i < result.length; i++) {
                if (result[i].Guid === guid) {
                    alert("The selected item is in playlist therefor it can`t be modified");
                    isinlist = true;
                    break;
                }
            }
            if (isinlist === false)
                location.href = VirtualDirectory + "/Delete?guid=" + guid + "&type=" + type;
        },
        error: function () {
        }
    });
}

setInterval(function () { getPlayList(); }, 20000);
