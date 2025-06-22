document.addEventListener('DOMContentLoaded', function() {
    // --- Gece Gündüz Modu ---
    const themeToggleBtn = document.getElementById('theme-toggle');
    const body = document.body;
    const currentTheme = localStorage.getItem('theme'); // Daha önce kaydedilmiş tema

    // Sayfa yüklendiğinde temayı uygula
    if (currentTheme) {
        body.classList.add(currentTheme);
        // Buton ikonunu temaya göre ayarla
        if (currentTheme === 'dark-mode') {
            if (themeToggleBtn) themeToggleBtn.innerHTML = '<i class="fas fa-sun"></i>';
        } else {
            if (themeToggleBtn) themeToggleBtn.innerHTML = '<i class="fas fa-moon"></i>';
        }
    } else {
        // Varsayılan tema (light-mode) ise, butonu ayarla
        if (themeToggleBtn) themeToggleBtn.innerHTML = '<i class="fas fa-moon"></i>';
    }

    if (themeToggleBtn) { // Buton varsa dinleyiciyi ekle
        themeToggleBtn.addEventListener('click', function() {
            if (body.classList.contains('dark-mode')) {
                body.classList.remove('dark-mode');
                localStorage.setItem('theme', 'light-mode');
                this.innerHTML = '<i class="fas fa-moon"></i>'; // Ay ikonu
            } else {
                body.classList.add('dark-mode');
                localStorage.setItem('theme', 'dark-mode');
                this.innerHTML = '<i class="fas fa-sun"></i>'; // Güneş ikonu
            }
        });
    }

    // --- Sepet Yönetimi ---
    // Sepet verilerini localStorage'dan al veya boş bir dizi oluştur
    let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];

    function updateCartCount() {
        const cartBadge = document.querySelector('.navbar-nav .badge');
        let totalCount = cartItems.reduce((sum, item) => sum + item.quantity, 0);
        if (cartBadge) {
            cartBadge.textContent = totalCount;
        }
        localStorage.setItem('cartCount', totalCount); // Genel sepet sayacını da güncelle
    }

    // Sepete Ekle butonlarına tıklama olayı
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', function() {
            const productId = this.dataset.productId;
            const productName = this.dataset.productName;
            const productPrice = parseFloat(this.dataset.productPrice);

            let existingItem = cartItems.find(item => item.id == productId);

            if (existingItem) {
                existingItem.quantity++;
                existingItem.totalPrice = existingItem.quantity * existingItem.price;
            } else {
                cartItems.push({
                    id: productId,
                    name: productName,
                    price: productPrice,
                    quantity: 1,
                    totalPrice: productPrice
                });
            }
            localStorage.setItem('cartItems', JSON.stringify(cartItems)); // Sepeti güncelle
            updateCartCount(); // Sayacı güncelle
            alert(`${productName} sepete eklendi!`);
        });
    });

    // Sayfa yüklendiğinde sepet sayacını ve tema modunu ayarla
    updateCartCount();

    // Ürün detayı sayfasına yönlendirme (Örnek)
    document.querySelectorAll('.card .btn-outline-primary').forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            const productId = this.closest('.card').querySelector('.add-to-cart').dataset.productId;
            window.location.href = `product-detail.html?productId=${productId}`;
        });
    });

    // products.html'de kullanılacak ürün verisi (Bu kısmı products.html scriptine taşıyacağız, burada sadece bilgi amaçlı bırakıldı)
    // const productsData = [ ... ];
    // filterAndSortProducts fonksiyonu products.html içinde olacak.
});