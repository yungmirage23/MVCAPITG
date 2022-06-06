$(document).ready(function () {
    jQuery.validator.addMethod("usPhoneFormat", function (value, element) {
        var aaa = value.replace("+", "").replace("(", "").replace(")", "").replace("-", "").replace(/_/g, "");
        return this.optional(element) || /^\d{12}$/.test(aaa);
    }, "Пожалуйста введите корректный номер телефона");
    $("#enter").click(function () {
        $("#form-log").validate({
            errorLabelContainer: ".validation",
            rules: {
                Phone: {
                    usPhoneFormat: true,
                    required: true,
                },
                Password: {
                    required: true,
                    minlength: 8,
                    maxlength: 17,
                }
            },
            messages: {
                Phone: {
                    required: "Номер телефона обязателен для заполнения",
                },
                Password: {
                    required: "Поле пароль обязательно для заполнения",
                    minlength: "Минимальная длина пароля 8 символов",
                    maxlength: "Длина пароля не должна быть больше 12 символов",
                }
            },
            success: "valid",
            submitHandler: function (event) {

                var phoneReplaced = $(".phone").val();
                phoneReplaced = phoneReplaced.replace("+", "").replace("(", "").replace(")", "").replace("-", "").replaceAll("_", "");
                var dataObject = {
                    'Phone': phoneReplaced,
                    'Password': $(".pass").val(),
                    'ReturnUrl': $("#return").val()
                };
                $("#valid").css("display", "none");
                $.ajax({
                    type: "POST",
                    url: "/Account/Login",
                    data: JSON.stringify(dataObject),
                    contentType: "application/json; charset=utf-8",
                    success: successFunc
                });
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

    });
})


