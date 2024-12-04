// Replace jQuery scroll event with window.addEventListener
window.addEventListener('scroll', function () {
    // Scroll handler for the navbar
    if (window.scrollY > 200) { // Điều kiện cuộn xuống
        const navbarCollapse = document.querySelector('.navbar-collapse');
        if (navbarCollapse && navbarCollapse.classList.contains('show')) {
            navbarCollapse.classList.remove('show'); // Đóng navbar mà không thay đổi giao diện
        }
    }

    // Change navbar style on scroll
    const navbar = document.querySelector('.ftco-navbar-light');
    if (window.scrollY > 50) { // Thay đổi giá trị 50 tùy theo nhu cầu
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
});
