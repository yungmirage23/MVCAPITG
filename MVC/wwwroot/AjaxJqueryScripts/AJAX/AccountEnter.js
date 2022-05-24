$("#enter").click(function () {
    var phoneReplaced = $(".phone").val();
    phoneReplaced = phoneReplaced.replace("+", "").replace("(", "").replace(")", "").replace("-", "").replaceAll("_", "");
    if ($("#form-log").valid()) {
        $("#valid").css("display", "none");
        var dataObject = {
            'Phone': phoneReplaced,
            'Password': $(".pass").val(),
            'ReturnUrl': $("#return").val()
        };
        $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: JSON.stringify(dataObject),
            contentType: "application/json; charset=utf-8",
            success: successFunc
        });
        return false;
        function successFunc(response) {
            if (response.success) {
                $("#login").modal('toggle');
                location.reload();
            }
            else
                $("#valid").css("display", "block");
        };
    }
});