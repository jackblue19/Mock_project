let currentPage = 1;
const itemsPerPage = 6;
const items = document.querySelectorAll('.services-wrap');

function showPage(page) {
    const startIndex = (page - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;

    // Ẩn tất cả các món ăn
    items.forEach(item => item.style.display = 'none');

    // Hiển thị các món ăn trong trang hiện tại
    for (let i = startIndex; i < endIndex && i < items.length; i++) {
        items[i].style.display = 'flex';
    }
}

// Khi người dùng nhấp vào phân trang
document.querySelectorAll('.pagination .page-link').forEach(button => {
    button.addEventListener('click', function (event) {
        const page = parseInt(event.target.innerText, 10);
        currentPage = page;
        showPage(currentPage);
    });
});

// Hiển thị trang đầu tiên khi tải trang
showPage(currentPage);