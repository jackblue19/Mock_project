
async function getMenu() {
    try {
        const response = await fetch('/api/item/viewmenu');
        const menuItems = await response.json();

        console.log(menuItems); 

        const dataTable = document.getElementById('data');
        dataTable.innerHTML = ''; // Xóa dữ liệu cũ trước khi hiển thị danh sách mới

        if (Array.isArray(menuItems) && menuItems.length > 0) {
            menuItems.forEach((item, index) => {
                // Ánh xạ itemStatus từ số thành chuỗi
                const statusText = item.itemStatus === 1 ? 'Still' : 'Sold Out';
                const { text, badgeClass } = getStatusAndBadgeClass(statusText);

                // Kiểm tra nếu giá trị null và thay thế bằng 'N/A'
                const itemName = item.itemName || 'N/A';
                const itemCategory = item.itemCategory || 'N/A';
                const originalPrice = item.originalPrice ? item.originalPrice.toFixed(2) : 'N/A';
                const suggestedPrice = item.suggestedPrice ? item.suggestedPrice.toFixed(2) : 'N/A';
                const itemDescription = item.itemDescription || 'N/A';
                const itemImage = item.itemImage || 'path/to/default-image.jpg';

                const row = document.createElement('tr');
                row.id = `menu-row-${item.itemId}`;

                row.innerHTML = `
                        <td>${index + 1}</td>
                        <td><img src="${itemImage}" alt="${itemName}" class="item-image" style="width: 50px; height: 50px;"></td>
                        <td>${itemName}</td>
                        <td>${itemCategory}</td>
                            <td>${originalPrice}</td>
                        <td>${suggestedPrice}</td>
                        <td>${itemDescription}</td>
                        <td><span class="badge ${badgeClass}">${text}</span></td>
                        <td>
                            <button class="btn btn-success" onclick="viewMenu(${item.itemId})" data-toggle="modal" data-target="#readData"><i class="bi bi-eye"></i></button>
                            <button class="btn btn-primary" onclick="openUpdateModal(${item.itemId})" data-toggle="modal" data-target="#updateItemForm"><i class="bi bi-pencil-square"></i></button>
                            <button class="btn btn-danger" onclick="deleteMenu(${item.itemId})"><i class="bi bi-trash3"></i></button>
                        </td>
                    `;
                dataTable.appendChild(row);
            });
        } else {
            dataTable.innerHTML = '<tr><td colspan="7">No menu items available.</td></tr>';
        }
    } catch (error) {
        console.error('Error fetching menu items:', error);
        const dataTable = document.getElementById('data');
        dataTable.innerHTML = '<tr><td colspan="7">Failed to load menu items.</td></tr>';
    }
}

// Hàm để xác định trạng thái và badge của món ăn (Still/Sold Out)
function getStatusAndBadgeClass(status) {
    let text = '';
    let badgeClass = '';
    if (status === 'Still') {
        text = 'Available';
        badgeClass = 'badge-success';
    } else if (status === 'Sold Out') {
        text = 'Sold Out';
        badgeClass = 'badge-danger';
    } else {
        text = 'Unknown';
        badgeClass = 'badge-secondary';
    }
    return { text, badgeClass };
}

window.onload = () => {
    getMenu();
}

// Hàm tạo món ăn mới
async function createMenu(event) {
    event.preventDefault();

    const productName = document.getElementById('productName').value;
    const category = document.getElementById('category').value;
    const originalPrice = document.getElementById('originalPrice').value;
    const price = document.getElementById('price').value;
    const description = document.getElementById('description').value;
    const status = document.getElementById('status').value;
    const originalPriceError = document.getElementById('originalPriceError');
    const priceError = document.getElementById('priceError');

    originalPriceError.style.display = 'none';
    priceError.style.display = 'none';

    if (parseFloat(originalPrice) > parseFloat(price)) {
        originalPriceError.style.display = 'block';
        return;
    }

    // Kiểm tra nếu giá bán <= 0
    if (parseFloat(price) <= 0) {
        priceError.style.display = 'block';
        return;
    }

    const imageInput = document.getElementById('image');
    const imageFile = imageInput.files[0];
    let imageUrl = '';

    if (imageFile) {
        imageUrl = await convertImageToBase64(imageFile);
    } else {
        imageUrl = '/images/default-image.jpg';
    }

    const newDish = {
        itemName: productName,
        itemCategory: category,
        originalPrice: parseFloat(price),
        itemStatus: status === 'Still' ? 1 : 0,
        itemDescription: description,
        suggestedPrice: parseFloat(price),
        itemImage: imageUrl,
        isServed: 1
    };

    try {
        // Gửi yêu cầu POST tới API
        const response = await fetch('/api/item/newdish', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newDish)
        });

        if (response.ok) {
            alert('Item created successfully!');
            $('#ItemForm').modal('hide');
            getMenu();
        } else {
            alert('Failed to create item!');
        }
    } catch (error) {
        console.error('Error creating menu item:', error);
        alert('An error occurred while creating the item.');
    }
}

// JavaScript: Hiển thị ảnh người dùng đã chọn
document.getElementById('image').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imagePreview = document.getElementById('imagePreview');
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
});
async function convertImageToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = function (e) {
            resolve(e.target.result);
        };
        reader.onerror = function (error) {
            reject(error);
        };
        reader.readAsDataURL(file);
    });
}

// Gắn hàm createMenu vào sự kiện submit của form
document.getElementById('myForm').addEventListener('submit', createMenu);

//UPDATE
// Hàm để hiển thị thông tin món ăn trong modal cập nhật
function openUpdateModal(itemId) {
    // Lấy thông tin món ăn từ API hoặc từ dữ liệu đã có
    fetch(`/api/item/${itemId}`)
        .then(response => response.json())
        .then(item => {
            // Điền dữ liệu vào các trường trong modal
            document.getElementById('itemIdUpdate').value = item.itemId;
            document.getElementById('productNameUpdate').value = item.itemName;
            document.getElementById('categoryUpdate').value = item.itemCategory;
            document.getElementById('originalPriceUpdate').value = item.originalPrice;
            document.getElementById('priceUpdate').value = item.suggestedPrice;
            document.getElementById('descriptionUpdate').value = item.itemDescription;
            document.getElementById('statusUpdate').value = item.itemStatus === 1 ? 'Still' : 'Sold Out';

            // Hiển thị ảnh hiện tại (nếu có)
            const imagePreview = document.getElementById('imagePreviewUpdate');
            imagePreview.src = item.itemImage || '/images/default-image.jpg';
            imagePreview.style.display = 'block';

            // Mở modal
            $('#updateItemForm').modal('show');
        })
        .catch(error => console.error('Error fetching item data:', error));
}

// Hàm xử lý cập nhật món ăn
// Hàm mở modal cập nhật và điền thông tin món ăn vào form
function openUpdateModal(id) {
    // Gửi yêu cầu lấy thông tin món ăn từ server bằng API của bạn
    fetch(`/api/item/viewdish/${id}`)  // Đảm bảo API đúng với cấu trúc của bạn
        .then(response => response.json())
        .then(item => {
            // Điền thông tin vào các trường trong modal
            document.getElementById('itemIdUpdate').value = item.itemId;
            document.getElementById('productNameUpdate').value = item.itemName || '';  
            document.getElementById('categoryUpdate').value = item.itemCategory || '';
            document.getElementById('originalPriceUpdate').value = item.originalPrice ? item.originalPrice.toFixed(2) : '';
            document.getElementById('priceUpdate').value = item.suggestedPrice ? item.suggestedPrice.toFixed(2) : '';
            document.getElementById('descriptionUpdate').value = item.itemDescription || '';
            document.getElementById('isServedUpdate').value = item.isServed;
            document.getElementById('statusUpdate').value = item.itemStatus === 1 ? 'Still' : 'Sold Out';

            // Hiển thị ảnh món ăn hiện tại
            const imagePreview = document.getElementById('imagePreviewUpdate');
            imagePreview.src = item.itemImage || '/images/default-image.jpg';  
            imagePreview.style.display = 'block';

            // Mở modal
            $('#updateItemForm').modal('show');
        })
        .catch(error => {
            console.error('Error fetching item data:', error);
            alert('Failed to load item data!');
        });
}
async function updateMenu(event) {
    event.preventDefault(); 

    // Lấy thông tin từ form
    const itemId = document.getElementById('itemIdUpdate').value;
    const itemName = document.getElementById('productNameUpdate').value;
    const category = document.getElementById('categoryUpdate').value;
    const originalPrice = document.getElementById('originalPriceUpdate').value;
    const price = document.getElementById('priceUpdate').value;
    const description = document.getElementById('descriptionUpdate').value;
    const status = document.getElementById('statusUpdate').value;
    const isServed = document.getElementById('isServedUpdate').value === '1' ? 1 : 0;

    // Kiểm tra giá trị
    if (parseFloat(originalPrice) > parseFloat(price)) {
        alert('Original price cannot be greater than selling price.');
        return;
    }

    if (parseFloat(price) <= 0) {
        alert('Selling price cannot be less than or equal to zero.');
        return;
    }
    const updatedItem = {
        itemId: itemId,
        itemName: itemName,
        itemCategory: category,
        originalPrice: parseFloat(originalPrice),
        suggestedPrice: parseFloat(price),
        itemDescription: description,
        itemStatus: status === 'Still' ? 1 : 0,  
        itemImage: await convertImageToBase64(document.getElementById('imageUpdate').files[0]) || '/images/default-image.jpg',
        isServed: isServed
    };

    try {
        const response = await fetch('/api/item/editdish', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updatedItem)
        });

        if (response.ok) {
            const updatedDish = await response.json();
            alert('Item updated successfully!');
            $('#updateItemForm').modal('hide');
            getMenu();
        } else {
            const errorData = await response.json();
            alert(`Failed to update item: ${errorData.message || 'Unknown error'}`);
        }
    } catch (error) {
        console.error('Error updating item:', error);
        alert('An error occurred while updating the item.');
    }
}
// Hiển thị ảnh trong form khi người dùng chọn ảnh mới
document.getElementById('imageUpdate').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imagePreview = document.getElementById('imagePreviewUpdate');
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block'; // Hiển thị ảnh đã chọn
        };
        reader.readAsDataURL(file);  // Chuyển ảnh thành Base64
    }
});






