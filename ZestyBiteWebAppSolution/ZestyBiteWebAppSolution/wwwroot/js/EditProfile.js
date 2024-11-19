function setActiveTab(tab) {
    // Xóa lớp active khỏi tất cả các tab
    const tabs = document.querySelectorAll('.list-group-item');
    tabs.forEach(function (item) {
        item.classList.remove('active');
    });
    // Thêm lớp active cho tab được nhấp
    tab.classList.add('active');

    // Ẩn tất cả các tab nội dung
    const tabContents = document.querySelectorAll('.tab-pane');
    tabContents.forEach(function (content) {
        content.classList.remove('active', 'show');
    });

    // Hiển thị nội dung tab tương ứng
    const target = tab.getAttribute('href');
    document.querySelector(target).classList.add('active', 'show');
}

function loadImage(event) {
    const image = document.getElementById('icon-placeholder');
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function (e) {
        image.src = e.target.result;
        currentImageSrc = e.target.result; // Lưu ảnh vào biến
        document.getElementById('remove-image').style.display = 'block'; // Hiện nút xóa
        document.getElementById('save-photo').style.display = 'block'; // Hiện nút lưu
    }

    if (file) {
        reader.readAsDataURL(file);
    }
}

function removeImage() {
    const image = document.getElementById('icon-placeholder');
    image.src = ''; // Xóa ảnh
    document.getElementById('file-input').value = ''; // Đặt lại input file
    document.getElementById('remove-image').style.display = 'none'; // Ẩn nút xóa
    document.getElementById('save-photo').style.display = 'none'; // Ẩn nút lưu
}

let currentImageSrc = ''; // Biến để lưu trữ ảnh hiện tại

function savePhoto() {
    const userDropdown = document.getElementById('userDropdown');
    const saveButton = document.getElementById('save-photo'); // Lấy nút Save
    const removeButton = document.getElementById('remove-image'); // Lấy nút Remove

    if (!currentImageSrc) {
        alert('Chưa có ảnh để lưu!'); // Kiểm tra ảnh có tồn tại không
        return;
    }

    const img = document.createElement('img'); // Tạo phần tử <img>
    img.src = currentImageSrc; // Đặt nguồn ảnh
    img.style.width = '50px'; // Đặt chiều rộng
    img.style.height = '50px'; // Đặt chiều cao
    img.style.borderRadius = '50%'; // Để ảnh tròn nếu cần
    img.style.objectFit = 'cover'; // Đảm bảo ảnh vừa khung

    // Thay thế nội dung của <a> bằng ảnh
    userDropdown.innerHTML = ''; // Xóa nội dung hiện tại
    userDropdown.appendChild(img); // Thêm ảnh vào phần tử <a>
}

function showSuccessAlert() {
    Swal.fire({
        title: "Good job!",
        text: "Đã lưu ảnh thành công!",
        icon: "success"
    });
}
