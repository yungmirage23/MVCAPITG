$(".profile ,.sing-in").click(function () {
    var url = "/Account/Login";
    var ReturnUrl = $(this).attr("returnUrl");
    var fulladrs = url + '?returnUrl=' + ReturnUrl;
    $.ajax({
        url: fulladrs,
        type: "Get",
        success: function (response) {
            $(".modalwindow").html(response);
            $('#login').modal('show');  
        }
    });
});