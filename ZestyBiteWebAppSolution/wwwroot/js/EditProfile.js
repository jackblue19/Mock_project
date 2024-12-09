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

function changePassword() {
    const dto = {
        currentPassword: document.getElementById('current-password').value,
        newPassword: document.getElementById('new-password').value,
        repeatNewPassword: document.getElementById('repeat-new-password').value
    };

    fetch('/Account/ChangePassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(dto)
    })
        .then(response => response.json())
        .then(data => {
            if (data.Message) {
                alert(data.Message);
            }
        })
        .catch(error => console.error('Error changing password:', error));
}

function loadGeneralTab() {
    fetch('/Account/ViewProfile')
        .then(response => response.text())  // Đọc nội dung trả về dưới dạng HTML
        .then(html => {
            document.getElementById('account-general').innerHTML = html;  // Cập nhật nội dung vào phần tử
        })
        .catch(error => console.error('Error loading general information:', error));
}

// Hàm tải form thay đổi mật khẩu khi nhấn vào tab
function loadChangePasswordTab() {
    fetch('/Account/ChangePassword')
        .then(response => response.text())  // Đọc nội dung trả về dưới dạng HTML
        .then(html => {
            document.getElementById('account-change-password').innerHTML = html;  // Cập nhật nội dung vào phần tử
        })
        .catch(error => console.error('Error loading change password form:', error));
}

// Thêm sự kiện khi chọn file
document.getElementById('file-input').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const iconPlaceholder = document.getElementById('icon-placeholder');

            // Đặt ảnh tải lên làm src của thẻ img
            iconPlaceholder.src = e.target.result; // Đặt ảnh tải lên vào icon


            iconPlaceholder.style.width = '150px';
            iconPlaceholder.style.height = '150px';
        }
        reader.readAsDataURL(file); // Đọc file ảnh
    }
});

function enableEdit() {
    // Enable fields for editing
    document.getElementById("phoneNumberField").removeAttribute("readonly");
    document.getElementById("addressField").removeAttribute("readonly");

    // Enable gender options
    document.getElementById("male").removeAttribute("disabled");
    document.getElementById("female").removeAttribute("disabled");

    // Show Save and Cancel buttons, hide Edit button
    document.getElementById("editBtn").style.display = "none";
    document.getElementById("saveBtn").style.display = "inline-block";
    document.getElementById("cancelBtn").style.display = "inline-block";
}
function cancelChanges() {
    // Reset form (reload page for simplicity)
    location.reload();
}