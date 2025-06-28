document.addEventListener('DOMContentLoaded', function () {
    const canvas = document.getElementById('bkdashboardChart');
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            datasets: [
                {
                    label: 'Teachers',
                    data: [12, 19, 3, 5, 2, 3, 9],
                    borderColor: 'rgba(54, 162, 235, 1)',
                    fill: false,
                    tension: 0.4
                },
                {
                    label: 'Coaches',
                    data: [5, 15, 8, 2, 7, 11, 6],
                    borderColor: 'rgba(255, 99, 132, 1)',
                    fill: false,
                    tension: 0.4
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
});
