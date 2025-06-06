document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("registerForm");

    form.addEventListener("submit", function (e) {
        let valid = true;

        form.querySelectorAll("input").forEach(input => {
            input.classList.remove("is-invalid", "is-valid");
        });

        const username = form.querySelector("input[name='Username']");
        const phone = form.querySelector("input[name='PhoneNumber']");
        const password = form.querySelector("input[name='Password']");

        
        const usernameValue = username.value.trim();
        const usernameRegex = /^[a-zA-Z0-9_-]+$/;

        if (usernameValue.length < 3) {
            markInvalid(username, "Ім’я користувача має містити щонайменше 3 символи");
            valid = false;
        } else if (!usernameRegex.test(usernameValue)) {
            markInvalid(username, "Ім’я користувача може містити лише літери, цифри, тире та підкреслення");
            valid = false;
        } else {
            markValid(username);
        }

       
        const phoneValue = phone.value.trim();
        const phoneRegex = /^\+380\d{9}$/;

        if (!phoneValue) {
            markInvalid(phone, "Номер телефону є обов’язковим");
            valid = false;
        } else if (!phoneRegex.test(phoneValue)) {
            markInvalid(phone, "Номер має бути у форматі +380XXXXXXXXX");
            valid = false;
        } else {
            markValid(phone);
        }

        
        const passwordValue = password.value.trim();
        if (!passwordValue) {
            markInvalid(password, "Пароль є обов’язковим");
            valid = false;
        } else if (passwordValue.length < 8) {
            markInvalid(password, "Пароль має містити щонайменше 8 символів");
            valid = false;
        } else {
            markValid(password);
        }

        clearValidationMessages();

        if (!valid) {
            e.preventDefault();

            form.classList.remove("shake");
            void form.offsetWidth;
            form.classList.add("shake");

            return;
        }
    });

    function markInvalid(input, message) {
        input.classList.add("is-invalid");
        let feedback = input.nextElementSibling;
        if (feedback && feedback.classList.contains("text-danger")) {
            feedback.innerText = message;
        }
    }

    function markValid(input) {
        input.classList.add("is-valid");
    }

    function clearValidationMessages() {
        form.querySelectorAll("input").forEach(input => {
            if (input.classList.contains("is-valid")) {
                let feedback = input.nextElementSibling;
                if (feedback && feedback.classList.contains("text-danger")) {
                    feedback.innerText = "";
                }
            }
        });
    }

});
