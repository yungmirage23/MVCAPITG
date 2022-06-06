$(document).ready(function () {
	jQuery.validator.addMethod("usPhoneFormat", function (value, element) {
		var aaa = value.replace("+", "").replace("(", "").replace(")", "").replace("-", "").replace(/_/g, "");
		return this.optional(element) || /^\d{12}$/.test(aaa);
	}, "Пожалуйста введите корректный номер телефона");
	$("#enter").click(function () {
		$("#form_reg").validate({
			errorLabelContainer: ".validation",
			rules: {
				Phone: {
					usPhoneFormat: true,
					required: true,
				},
				Password: {
					required: true,
					minlength: 8,
					maxlength: 13,
				},
				Confirmpass: {
					required: true,
					equalTo: Password,
				},
				Policy: {
					required: true,
				},
			},
			messages: {
				Phone: {
					required: "Номер телефона обязателен для заполнения",
				},
				Password: {
					required: "Поле пароль обязательно для заполнения",
					minlength: "Минимальная длина пароля 8 символов",
					maxlength: "Длина пароля не должна быть больше 12 символов",
				},
				Confirmpass: {
					required: "Подтвердите свой пароль",
					equalTo: "Пароли не совпадают",
				},
				Policy: {
					required: "Пожалуйста, прочтите и примите пользовательское соглашение",
				},
			},
			success: "valid",
			submitHandler: function (event) {
				var dataObject = {
					'Phone': $(".phone").val(),
					'Password': $(".pass").val(),
					'ReturnUrl': $("#return").val()
				};
				$.ajax({
					type: "POST",
					url: "/Account/RegistrationPartial",
					data: JSON.stringify(dataObject),
					contentType: "application/json; charset=utf-8",
					success: successFunc
				});
				function successFunc(response) {
					switch (response.status) {
						case true:
							location.reload();
							$("#registration").modal('toggle');
							break;
						case false:
							$("#valid").css("display", "block");
							break;
					}
				};
			}
		});
	});
})


