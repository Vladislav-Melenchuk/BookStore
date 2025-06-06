document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("loginForm");

    if (!form) return;

    form.addEventListener("submit", function (e) {
        const username = form.querySelector("input[name='Username']");
        const password = form.querySelector("input[name='Password']");
        let valid = true;

        [username, password].forEach(input => {
            input.classList.remove("is-invalid", "is-valid");
        });

        if (!username.value.trim()) {
            username.classList.add("is-invalid");
            valid = false;
        } else {
            username.classList.add("is-valid");
        }

        if (!password.value.trim()) {
            password.classList.add("is-invalid");
            valid = false;
        } else {
            password.classList.add("is-valid");
        }

        if (!valid) {
            e.preventDefault();
            form.classList.remove("shake");
            void form.offsetWidth;
            form.classList.add("shake");
        }
    });
});
