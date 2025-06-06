document.addEventListener("DOMContentLoaded", function () {
    console.log("загружен");

    let username = getCookie("Username");
    let isAdmin = getCookie("IsAdmin");

    if (!username) {
        username = sessionStorage.getItem("Username");
        console.log("Взято з sessionStorage:", username);
    } else {
        console.log("Взято з Cookie:", username);
    }

    if (!isAdmin) {
        isAdmin = sessionStorage.getItem("IsAdmin");
        console.log("Права з sessionStorage:", isAdmin);
    } else {
        console.log("Права з Cookie:", isAdmin);
    }

    const guestNav = document.querySelectorAll(".guest-only");
    const authNav = document.querySelectorAll(".auth-only");
    const usernameSpan = document.getElementById("nav-username");
    const dropdownMenu = document.querySelector(".auth-only .dropdown-menu");

    if (username) {
        guestNav.forEach(el => el.classList.add("d-none"));
        authNav.forEach(el => el.classList.remove("d-none"));

        if (usernameSpan) {
            usernameSpan.textContent = username;
        }

        if ((isAdmin === "True" || isAdmin === "true") && dropdownMenu) {
            const adminItem = document.createElement("li");
            adminItem.innerHTML = `<a class="dropdown-item text-primary fw-semibold" href="/Admin/AdminPanel"><i class="bi bi-shield-lock"></i> Адмін-панель</a>`;
            dropdownMenu.insertBefore(adminItem, dropdownMenu.firstElementChild);
        }
    }

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return decodeURIComponent(parts.pop().split(';').shift());
        return null;
    }
});