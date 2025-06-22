document.addEventListener('DOMContentLoaded', function () {

    // Gerekli Elementler
    const loginForm = document.getElementById('login-form');
    const loginPage = document.getElementById('login-page');
    const adminPanel = document.getElementById('admin-panel');
    const logoutButton = document.getElementById('logout-button');
    const sidebarLinks = document.querySelectorAll('.sidebar-link');
    const pageContents = document.querySelectorAll('.page-content');
    const themeRadios = document.querySelectorAll('input[name="theme"]');
    const sidebarCollapse = document.getElementById('sidebarCollapse');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('main-content');


    // ===================================================
    // 1. GİRİŞ & ÇIKIŞ İŞLEMLERİ (Dummy Logic)
    // ===================================================
    loginForm.addEventListener('submit', function (e) {
        e.preventDefault(); // Formun varsayılan gönderimini engelle
        // Gerçek projede burada kullanıcı adı/şifre kontrolü yapılır
        loginPage.classList.add('d-none');
        adminPanel.classList.remove('d-none');
        // Giriş yapıldığında grafikleri çiz
        initCharts();
    });

    logoutButton.addEventListener('click', function (e) {
        e.preventDefault();
        adminPanel.classList.add('d-none');
        loginPage.classList.remove('d-none');
    });

    // ===================================================
    // 2. SAYFA NAVİGASYONU (SPA Havası)
    // ===================================================
    sidebarLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const pageId = this.getAttribute('data-page');

            // Tüm içerikleri gizle
            pageContents.forEach(content => {
                content.classList.add('d-none');
            });

            // Hedef içeriği göster
            const targetPage = document.getElementById(pageId);
            if (targetPage) {
                targetPage.classList.remove('d-none');
            }

            // Aktif menü öğesini güncelle
            sidebarLinks.forEach(l => l.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // ===================================================
    // 3. GRAFİK OLUŞTURMA (Chart.js)
    // ===================================================
    let salesChartInstance, categoryChartInstance;

    function initCharts() {
        // Bar Chart - Satışlar
        const salesCtx = document.getElementById('salesChart').getContext('2d');
        if (salesChartInstance) salesChartInstance.destroy(); // Önceki grafiği temizle
        salesChartInstance = new Chart(salesCtx, {
            type: 'bar',
            data: {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran'],
                datasets: [{
                    label: 'Aylık Satışlar (₺)',
                    data: [12000, 19000, 8000, 15000, 22000, 30000],
                    backgroundColor: 'rgba(54, 162, 235, 0.6)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: { scales: { y: { beginAtZero: true } } }
        });

        // Pie Chart - Kategoriler
        const categoryCtx = document.getElementById('categoryChart').getContext('2d');
        if (categoryChartInstance) categoryChartInstance.destroy(); // Önceki grafiği temizle
        categoryChartInstance = new Chart(categoryCtx, {
            type: 'pie',
            data: {
                labels: ['Elektronik', 'Giyim', 'Ev & Yaşam', 'Kozmetik'],
                datasets: [{
                    label: 'Kategori Dağılımı',
                    data: [45, 25, 15, 15],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.7)',
                        'rgba(54, 162, 235, 0.7)',
                        'rgba(255, 206, 86, 0.7)',
                        'rgba(75, 192, 192, 0.7)'
                    ]
                }]
            }
        });
    }

    // ===================================================
    // 4. TEMA DEĞİŞTİRME (Açık/Karanlık Mod)
    // ===================================================
    function applyTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('theme', theme); // Tercihi kaydet
    }

    themeRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            applyTheme(this.value);
        });
    });

    // Sayfa yüklendiğinde kayıtlı temayı uygula
    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);
    document.querySelector(`input[value="${savedTheme}"]`).checked = true;


    // ===================================================
    // 5. SIDEBAR GİZLE/GÖSTER
    // ===================================================
    sidebarCollapse.addEventListener('click', function () {
        sidebar.classList.toggle('toggled');
        mainContent.classList.toggle('toggled');
    });

});