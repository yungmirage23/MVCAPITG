$("#save-password").click(function () {
    var a = 0;
    if (!$("#OldPassword").val()) {
        $("#OldPassword").css("border-color", "red");
    }
    else {
        $("#OldPassword").css("border-color", "#EBEBEB");
        a++;
    }
    if (!$("#NewPassword").val() || $("#confirm-pass").val() !== $("#NewPassword").val()) {
        $("#NewPassword").css("border-color", "red");
    }
    else {
        $("#NewPassword").css("border-color", "#EBEBEB");
        a++;
    }

    if (!$("#confirm-pass").val() || $("#confirm-pass").val() !== $("#NewPassword").val()) {
        $("#confirm-pass").css("border-color", "red");
    }
    else {
        $("#confirm-pass").css("border-color", "#EBEBEB");
        a++;
    }
    if (a == 3) {
        var oldPasswor = $("#OldPassword").val();
        var newPassword = $("#NewPassword").val();
        $.post("/Account/ChangePassword",
            { "_newPassword": newPassword, "_oldPassword": oldPasswor }, function (response) {
                if (response.success) {
                    location.reload();
                }
                else {
                    location.reload();
                    $("#OldPassword").css("border-color", "red");
                }
            });
    }
});