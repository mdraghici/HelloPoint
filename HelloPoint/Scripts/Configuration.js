monitorindex = 2;
isMoitorTurnedOn = false;
window.onload = function () {
    getMonitorState();
    changemonitor(2);
};

function changemonitor(i) {
    monitorindex = i;
    getMonitorData(i);
}

function changeOnOffButton() {
    //alert(isMoitorTurnedOn);
    if (isMoitorTurnedOn === true) {
        document.getElementById("monitorturnonoff").src = "../Content/turnOnIcon.png";
    } else {
        document.getElementById("monitorturnonoff").src = "../Content/turnOffIcon.png";
    }
}

function getMonitorData(i) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory+'/UserManagement/GetMonitorData',
        data: { 'monitorindex': i },
        success: function (data) {
            RenderData(data);
        },
        error: function () {
        }
    });
}



function moveUp() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
     $.ajax({
         url: VirtualDirectory+'/UserManagement/MoveUpMonitor',
         data: { 'monitorindex': monitorindex },
         success: function (data) {
             RenderData(data);
         },
         error: function () {
         }
     });
 }

function moveDown() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/MoveDownMonitor',
        data: { 'monitorindex': monitorindex },
        success: function (data) {
            RenderData(data);
        },
        error: function () {
        }
    });
}

function moveLeft() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/MoveLeftMonitor',
        data: { 'monitorindex': monitorindex },
        success: function (data) {           
            RenderData(data);
        },
        error: function () {
        }
    });
}


function moveRight() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/MoveRightMonitor',
        data: { 'monitorindex': monitorindex },
        success: function (data) {
            RenderData(data);
        },
        error: function () {
        }
    });
}


function scaleUp() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/ScaleUpMonitor',
        data: { 'monitorindex': monitorindex },
        success: function (data) {
            RenderData(data);
        },
        error: function () {
        }
    });
}

function scaleDown() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/ScaleDownMonitor',
        data: { 'monitorindex': monitorindex },
        success: function (data) {
            RenderData(data);
        },
        error: function () {
        }
    });
}

function resetMonitor() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/ResetMonitor',
        data: {},
        success: function (data) {            
            RenderData(data);
        },
        error: function () {
        }
    });
}

function restart() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/RestartWfp',
        data: {},
        success: function (data) {
            if (data === 'True') {
                alert("App restarted succesfull");
            } else {
                alert("App restarted unsuccesfull");
            }            
        },
        error: function () {
        }
    });
}

function getMonitorState() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    $.ajax({
        url: VirtualDirectory + '/UserManagement/GetMonitorState',
        data: {},
        success: function (data) {
            setisMoitorTurnedOn(data);
        },
        error: function () {
        }
    });
}


function turnOnOff() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    var cmd = "On";
    if (isMoitorTurnedOn === true) {
        cmd = "Off";
    }
    $.ajax({
        url: VirtualDirectory+'/UserManagement/TornMonitor'+cmd,
        data: {},
        success: function (data) {
            setisMoitorTurnedOn(data);
        },
        error: function () {
        }
    });
}

function setisMoitorTurnedOn(data) {
    isMoitorTurnedOn = JSON.parse(data).ON;
    changeOnOffButton();
}

function RenderData(data) {
    var result = JSON.parse(data);
    document.getElementById('Upvalue').innerHTML = result.MarginY;
    document.getElementById('Downvalue').innerHTML = result.MarginY;
    document.getElementById('Leftvalue').innerHTML = result.MarginX;
    document.getElementById('Rightvalue').innerHTML = result.MarginX;
    document.getElementById('Zoomvalue').innerHTML = result.Scale;
}

setInterval(function () { changemonitor(monitorindex); getMonitorState(); }, 2000);