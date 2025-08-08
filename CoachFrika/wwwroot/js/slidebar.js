document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('toggleBtn');
    const sidebar = document.querySelector('.sidebar');
    const mainContent = document.querySelector('.main-content');
    const logoutBtn = document.getElementById('logoutBtn'); // ✅ Declare logoutBtn safely

    if (toggleBtn && sidebar && mainContent) {
        const icon = toggleBtn.querySelector('i');

        // Set correct icon on initial load
        icon.className = sidebar.classList.contains('collapsed')
            ? 'fas fa-angle-double-right'
            : 'fas fa-angle-double-left';

        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('collapsed');
            mainContent.classList.toggle('collapsed');

            const isCollapsed = sidebar.classList.contains('collapsed');
            icon.className = isCollapsed
                ? 'fas fa-angle-double-right'
                : 'fas fa-angle-double-left';
        });
    }

    const items = document.querySelectorAll('.sidebar-item');
    const pages = document.querySelectorAll('.page-content');

    items.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            const targetPage = this.getAttribute('href').substring(1);
            pages.forEach(page => page.classList.remove('active'));
            const target = document.getElementById(targetPage);
            if (target) {
                target.classList.add('active');
            }
        });
    });

    // ✅ Logout handler added safely
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function () {
            localStorage.removeItem("authToken");
            sessionStorage.clear();
            window.location.href = "/Account/Login";
        });
    }
});
