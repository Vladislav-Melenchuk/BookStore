document.addEventListener("DOMContentLoaded", () => {
    const cartItemsContainer = document.getElementById("cartItemsContainer");
    const cartTotal = document.getElementById("cartTotal");
    const checkoutBtn = document.getElementById("checkoutBtn");
    const clearCartBtn = document.getElementById("clearCartBtn");

    function getCart() {
        const cartStr = localStorage.getItem("cart");
        if (cartStr) return JSON.parse(cartStr);

        const match = document.cookie.match(/(?:^|; )cart=([^;]*)/);
        if (match) {
            try {
                return JSON.parse(decodeURIComponent(match[1]));
            } catch (e) {
                console.warn("Невірний формат корзини в cookie");
            }
        }
        return [];
    }

 
    function saveCart(cart) {
        localStorage.setItem("cart", JSON.stringify(cart));
        document.cookie = "cart=" + encodeURIComponent(JSON.stringify(cart)) + "; path=/";
    }

    function renderCart() {
        const cart = getCart();
        cartItemsContainer.innerHTML = "";

        if (cart.length === 0) {
            cartItemsContainer.innerHTML = `
                <div class="empty-cart text-center p-4">
                     <img src="/img/cart.png" alt="Порожній кошик" style="max-width: 200px; opacity: 0.6;">
                     <h5 class="mt-4 text-muted">Ваш кошик порожній</h5>
                     <p class="text-secondary">Схоже, ви ще не додали жодної книги</p>
                     <a href="/Catalog" class="btn btn-outline-secondary mt-2">Перейти до каталогу</a>
                </div>
            `;

            cartTotal.textContent = "0.00 грн";
            checkoutBtn.disabled = true;
            clearCartBtn.style.display = "none";
            return;
        }

        clearCartBtn.style.display = "block";
        checkoutBtn.disabled = false;

        let total = 0;

        cart.forEach((item, index) => {
            total += item.price * item.qty;

            const itemElem = document.createElement("div");
            itemElem.className = "card mb-3 p-3 d-flex flex-row align-items-center gap-3";

            itemElem.innerHTML = `
                <img src="${item.image}" alt="${item.title}" class="img-thumbnail" style="width: 100px; height: auto; object-fit: contain;">
                <div class="flex-grow-1">
                    <h5>${item.title}</h5>
                    <p class="mb-1 text-muted">${item.author}</p>
                    <p class="mb-1 fw-bold">${item.price.toFixed(2)} грн</p>
                    <div class="d-flex align-items-center gap-2">
                        <label for="qty-${index}" class="form-label mb-0">Кількість:</label>
                        <input type="number" id="qty-${index}" class="form-control qty-input" value="${item.qty}" min="1" style="width: 70px;">
                    </div>
                </div>
                <button class="btn btn-outline-danger btn-sm remove-btn" title="Видалити">
                    <i class="bi bi-trash"></i>
                </button>
            `;

            
            const qtyInput = itemElem.querySelector(".qty-input");
            qtyInput.addEventListener("change", (e) => {
                let val = parseInt(e.target.value);
                if (isNaN(val) || val < 1) val = 1;
                e.target.value = val;

                const cart = getCart();
                cart[index].qty = val;
                saveCart(cart);
                renderCart();
            });

            
            const removeBtn = itemElem.querySelector(".remove-btn");
            removeBtn.addEventListener("click", () => {
                let cart = getCart();
                cart.splice(index, 1);
                saveCart(cart);
                renderCart();
            });

            cartItemsContainer.appendChild(itemElem);
        });

        cartTotal.textContent = total.toFixed(2) + " грн";
    }

    
    clearCartBtn.addEventListener("click", () => {
        localStorage.removeItem("cart");
        renderCart();
    });

 


    
    checkoutBtn.addEventListener("click", () => {
        const cart = getCart();
        

        if (cart.length === 0) {
            alert("Ваш кошик порожній!");
            return;
        }

        if (window.isUserLoggedIn !== true && window.isUserLoggedIn !== "true") {
            if (confirm("Щоб оформити замовлення, потрібно увійти або зареєструватись. Перейти до входу?")) {
                window.location.href = "/Account/Login";
            }
            return;
        }

        fetch('/api/cart/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cart.map(item => ({
                productId: item.id,
                title: item.title,
                price: item.price,
                quantity: item.qty
            })))
        }).then(() => {
            window.location.href = "/Order/Checkout";
        }).catch(() => {
            alert("Помилка збереження корзини. Спробуйте ще раз.");
        });
    });


    
    renderCart();
});

