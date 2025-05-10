function sendPasswordChange() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    var name = document.getElementById("username").innerHTML;
    var password = document.getElementById("pass1").value;
    var password1 = document.getElementById("pass2").value;
    if (password1 == "")
        alert("The fields are empty");
    else
        {
    if (password !== password1)
        alert("The passwords doesn't match!Try again!");
    else {
       $.ajax({
           url: VirtualDirectory + '/Account/ChangePassword',
           data: { 'username': name, 'newPassword': password },
           success: function (data) {
               alert(data);
               window.location.href = VirtualDirectory + "/Account/AdminPage";
           },
           error: function () {
               alert('Error');
           }
        });        
    }
    }
}

function sendNewRole() {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    var role = document.getElementById("roleselecttag").value;
    var name = document.getElementById("username").innerHTML;
    $.ajax({
        url: VirtualDirectory + '/Account/ChangeRole',
        data: { 'username': name, 'roleid': role },
        success: function (data) {
            alert(data);
            window.location.href = VirtualDirectory + "/Account/AdminPage";
        },
        error: function () {
            alert('Error');
        }
    });
}

function deleteConfirmation(Username, Role) {
    var pathname = window.location.pathname;
    var VirtualDir = pathname.split('/');
    VirtualDirectory = VirtualDir[1];
    VirtualDirectory = '/' + VirtualDirectory;
    if (confirm("Are you sure you want to delete user: " + Username + " who is an :" + Role) == true) {
        $.ajax({
            url: VirtualDirectory + '/Account/DeleteUser',
            data: { 'username': Username, 'rolename': Role },
            success: function (data) {

                window.location.href = VirtualDirectory + "/Account/AdminPage";
            },
            error: function () {
                alert('Error');
            }
        });


    }
    else {
        window.location.href = VirtualDirectory + "/Account/AdminPage";
    }

}