document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const genreSelect = document.getElementById("genreSelect");
    const minPrice = document.getElementById("minPrice");
    const maxPrice = document.getElementById("maxPrice");
    const bookCards = document.querySelectorAll(".book-card");
    const clearBtn = document.getElementById("clearFilter");

    function filterBooks() {
        const search = searchInput.value.toLowerCase();
        const genre = genreSelect.value.toLowerCase();
        const min = parseFloat(minPrice.value) || 0;
        const max = parseFloat(maxPrice.value) || Infinity;

        bookCards.forEach(card => {
            const title = card.dataset.title.toLowerCase();
            const bookGenre = (card.dataset.genre || "").toLowerCase();
            const price = parseFloat(card.dataset.price);

            const matchesTitle = title.includes(search);
            const matchesGenre = !genre || bookGenre === genre;
            const matchesPrice = price >= min && price <= max;

            const shouldShow = matchesTitle && matchesGenre && matchesPrice;

            if (shouldShow) {
                if (card.classList.contains("d-none")) {
                    card.classList.remove("d-none", "fade-out");
                    card.classList.add("fade-in");
                }
            } else {
                if (!card.classList.contains("d-none")) {
                    card.classList.remove("fade-in");
                    card.classList.add("fade-out");
                    setTimeout(() => {
                        card.classList.add("d-none");
                    }, 300); 
                }
            }
        });
    }

    searchInput.addEventListener("input", filterBooks);
    genreSelect.addEventListener("change", filterBooks);
    minPrice.addEventListener("input", filterBooks);
    maxPrice.addEventListener("input", filterBooks);
    clearBtn.addEventListener("click", () => {
        searchInput.value = "";
        genreSelect.value = "";
        minPrice.value = "";
        maxPrice.value = "";
        filterBooks();
    });

    filterBooks(); 
});

function showDetails(card) {
    document.getElementById("bookModal").dataset.bookId = card.dataset.id;


    document.getElementById("modalBookTitle").textContent = card.dataset.title;
    document.getElementById("modalBookAuthor").textContent = card.dataset.author;
    document.getElementById("modalBookCategory").textContent = card.dataset.genre;
    document.getElementById("modalBookPrice").textContent = parseFloat(card.dataset.price).toFixed(2);
    document.getElementById("modalBookImage").src = card.dataset.image;

    const fullDesc = card.dataset.description || "";
    const descContainer = document.getElementById("modalBookDescription");

    if (fullDesc.length > 200) {
        descContainer.innerHTML = `
            <span id="shortDesc">${fullDesc.substring(0, 200)}...</span>
            <span id="fullDesc" class="d-none">${fullDesc}</span>
            <br><a href="#" id="toggleDesc" class="text-primary" style="text-decoration: underline;">Читати більше</a>
        `;

        setTimeout(() => {
            document.getElementById("toggleDesc")?.addEventListener("click", function (e) {
                e.preventDefault();
                const short = document.getElementById("shortDesc");
                const full = document.getElementById("fullDesc");

                const isExpanded = !short.classList.contains("d-none");
                short.classList.toggle("d-none", isExpanded);
                full.classList.toggle("d-none", !isExpanded);
                this.textContent = isExpanded ? "Згорнути" : "Читати більше";
            });
        }, 50);
    } else {
        descContainer.textContent = fullDesc;
    }

    const modal = new bootstrap.Modal(document.getElementById("bookModal"));
    modal.show();
}

document.addEventListener("DOMContentLoaded", function () {
    const cartCount = document.getElementById("cartCount");

    
    function updateCartCounter() {
        const cart = JSON.parse(localStorage.getItem("cart") || "[]");
        const totalQty = cart.reduce((sum, item) => sum + item.qty, 0);
        if (cartCount) {
            cartCount.textContent = totalQty;
            cartCount.style.display = totalQty > 0 ? "inline-flex" : "none";
        }
    }

    
    function animateCartIcon() {
        if (!cartCount) return;
        cartCount.classList.add("animate__animated", "animate__bounce");
        setTimeout(() => {
            cartCount.classList.remove("animate__animated", "animate__bounce");
        }, 1000);
    }

   
    function addToCart(book) {
        let cart = JSON.parse(localStorage.getItem("cart") || "[]");
        const existing = cart.find(x => x.id === book.id);
        if (existing) {
            existing.qty += 1;
        } else {
            cart.push({ ...book, qty: 1 });
        }
        localStorage.setItem("cart", JSON.stringify(cart));
        updateCartCounter();
        animateCartIcon();
    }

    
    document.querySelectorAll(".btn-add-to-cart").forEach(button => {
        button.addEventListener("click", function (e) {
            e.stopPropagation(); 
            const card = this.closest(".book-card");
            const book = {
                id: parseInt(card.dataset.id),
                title: card.dataset.title,
                author: card.dataset.author,
                genre: card.dataset.genre,
                price: parseFloat(card.dataset.price),
                image: card.dataset.image
            };
            addToCart(book);
            animateAddButton(this);
        });
    });

    
    const modalBtn = document.getElementById("modalAddToCart");
    if (modalBtn) {
        modalBtn.addEventListener("click", function () {
            const modal = document.getElementById("bookModal");
            const book = {
                id: parseInt(modalBtn.dataset.id),
                title: modalBtn.dataset.title,
                author: modalBtn.dataset.author,
                genre: modalBtn.dataset.genre,
                price: parseFloat(modalBtn.dataset.price),
                image: modalBtn.dataset.image
            };
            animateAddButton(this);
            addToCart(book);
            

          
            const modalInstance = bootstrap.Modal.getInstance(modal);
            if (modalInstance) modalInstance.hide();
        });
    }

    updateCartCounter();
});


function showDetails(card) {
    document.getElementById("modalBookTitle").textContent = card.dataset.title;
    document.getElementById("modalBookAuthor").textContent = card.dataset.author;
    document.getElementById("modalBookCategory").textContent = card.dataset.genre;
    document.getElementById("modalBookPrice").textContent = parseFloat(card.dataset.price).toFixed(2);
    document.getElementById("modalBookImage").src = card.dataset.image;

    const fullDesc = card.dataset.description || "";
    const descContainer = document.getElementById("modalBookDescription");

    if (fullDesc.length > 200) {
        descContainer.innerHTML = `
            <span id="shortDesc">${fullDesc.substring(0, 200)}...</span>
            <span id="fullDesc" class="d-none">${fullDesc}</span>
            <br><a href="#" id="toggleDesc" class="text-primary" style="text-decoration: underline;">Читати більше</a>
        `;

        setTimeout(() => {
            document.getElementById("toggleDesc")?.addEventListener("click", function (e) {
                e.preventDefault();
                const short = document.getElementById("shortDesc");
                const full = document.getElementById("fullDesc");

                const isExpanded = !short.classList.contains("d-none");
                short.classList.toggle("d-none", isExpanded);
                full.classList.toggle("d-none", !isExpanded);
                this.textContent = isExpanded ? "Згорнути" : "Читати більше";
            });
        }, 50);
    } else {
        descContainer.textContent = fullDesc;
    }

    
    const modalBtn = document.getElementById("modalAddToCart");
    modalBtn.dataset.id = card.dataset.id;
    modalBtn.dataset.title = card.dataset.title;
    modalBtn.dataset.author = card.dataset.author;
    modalBtn.dataset.genre = card.dataset.genre;
    modalBtn.dataset.price = card.dataset.price;
    modalBtn.dataset.image = card.dataset.image;

    const modal = new bootstrap.Modal(document.getElementById("bookModal"));
    modal.show();
}


function animateAddButton(button) {
    button.classList.add("animate__animated", "animate__heartBeat");
    setTimeout(() => {
        button.classList.remove("animate__animated", "animate__heartBeat");
    }, 800);

    
    const originalText = button.textContent;
    button.textContent = "✓ Додано";
    setTimeout(() => {
        button.textContent = originalText;
    }, 1200);
}
