$(".emailmodal").click(function () {
    $.ajax({
        url: "/Home/SubscribeEmail",
        type: "Get",
        success: function (response) {
            $(".modalwindow").html(response);
            $('#subscribe').modal('show');
        }
    });
});