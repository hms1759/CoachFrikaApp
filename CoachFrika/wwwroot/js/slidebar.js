document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('toggleBtn');
    const sidebar = document.querySelector('.sidebar');
    const logoutBtn = document.getElementById('logoutBtn');
    const mainContent = document.querySelector('.main-content');

    if (toggleBtn && sidebar && mainContent) {
        //toggleBtn.addEventListener('click', function () {
        //    sidebar.classList.toggle('collapsed');
        //    mainContent.classList.toggle('collapsed');

        //    toggleBtn.textContent = sidebar.classList.contains('collapsed') ? '>>' : '<<';
        //});
        

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

    if (logoutBtn) {
        logoutBtn.addEventListener('click', function () {
            alert('Logging out...');
        });
    }
});
