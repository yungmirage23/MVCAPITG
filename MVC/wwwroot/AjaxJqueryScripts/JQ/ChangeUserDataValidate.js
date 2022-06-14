$(document).ready(function () {
    jQuery.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[\u0400-\u04FF]+$/i.test(value);
    }, "Letters only please");
    $(".save-changes").click(function () {
        $("#form-changedata").validate({
            rules: {
                FirstName: { lettersonly: true },
                LastName: { lettersonly: true },
                PatronymicName: { lettersonly: true },
                Email: { email: true },
                AdditionalPhone: {
                },
                PhoneNumber: {
                    minlength: 10,
                }
            },
            messages: {
                FirstName: { lettersonly: "Пожалуйста, используйте кирилицу" },
                LastName: { lettersonly: "Пожалуйста, используйте кирилицу" },
                PatronymicName: { lettersonly: "Пожалуйста, используйте кирилицу" },
                Email: { email: "Введите корректный Email адресс" },
                AdditionalPhone: {
                },
                PhoneNumber: {
                    minlength: "Номер телефона должен состоять минимум из 10 цифр",
                }
            },
        });
        if ($("#form-changedata").valid()) {
            $("#form-changedata").submit();
        }
    });
});