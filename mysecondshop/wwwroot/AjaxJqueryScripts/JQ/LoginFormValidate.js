jQuery.validator.addMethod("usPhoneFormat", function (value, element) {
    var aaa = value.replace("+", "").replace("(", "").replace(")", "").replace("-", "").replace(/_/g, "");
    return this.optional(element) || /^\d{12}$/.test(aaa);
}, "Пожалуйста введите корректный номер телефона");
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
});
