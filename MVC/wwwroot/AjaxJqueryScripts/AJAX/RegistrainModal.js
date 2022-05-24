$("#registration").click(function () {
    $(".phone").mask();
    $('#login').modal('toggle');
    $.ajax({
        url: "/Account/RegistrationPartial",
        type: "Get",
        success: function (response) {
            $(".modalwindow").html(response);
            $('#registration').modal('show');
        }
    });
});