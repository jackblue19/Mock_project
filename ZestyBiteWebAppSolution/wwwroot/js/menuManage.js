
async function getMenu() {
    try {
        const response = await fetch('/api/item/viewmenu');
        const menuItems = await response.json();

        console.log(menuItems); 

        const dataTable = document.getElementById('data');
        dataTable.innerHTML = ''; 

        if (Array.isArray(menuItems) && menuItems.length > 0) {
            menuItems.forEach((item, index) => {
                // Ánh xạ itemStatus từ số thành chuỗi
                const statusText = item.itemStatus === 1 ? 'Still' : 'Sold Out';
                const { text, badgeClass } = getStatusAndBadgeClass(statusText);

                
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

    // Kiểm tra giá
    if (parseFloat(originalPrice) > parseFloat(price)) {
        originalPriceError.style.display = 'block';
        return;
    }

    if (parseFloat(price) <= 0) {
        priceError.style.display = 'block';
        return;
    }

    // Mặc định ảnh nếu không có
    const imageInput = document.getElementById('image');
    const imageFile = imageInput.files[0];
    let imageUrl = '/images/default-image.jpg';  

    
    if (imageFile) {
        try {
            imageUrl = await convertImageToBase64(imageFile);  
        } catch (error) {
            alert('Lỗi khi chuyển đổi ảnh!');
            console.error(error);
            return;  
        }
    }

    const newDish = {
        itemName: productName,
        itemCategory: category,
        originalPrice: parseFloat(originalPrice),
        itemStatus: status === 'Still' ? 1 : 0,
        itemDescription: description,
        suggestedPrice: parseFloat(price),
        itemImage: imageUrl,
        isServed: 1
    };

    
    try {
        const response = await fetch('/api/item/newdish', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newDish)
        });

        if (response.ok) {
            alert('Món ăn đã được tạo thành công!');
            $('#ItemForm').modal('hide');
            getMenu();  
        } else {
            alert('Tạo món ăn không thành công!');
        }
    } catch (error) {
        console.error('Lỗi khi tạo món ăn:', error);
        alert('Có lỗi xảy ra khi tạo món ăn.');
    }
}

// Hiển thị ảnh người dùng đã chọn
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

// Hàm chuyển ảnh thành Base64
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


document.getElementById('myForm').addEventListener('submit', createMenu);

//UPDATE
function openUpdateModal(id) {
    fetch(`/api/item/viewdish/${id}`)
        .then(response => response.json())
        .then(item => {
            document.getElementById('itemIdUpdate').value = item.itemId;
            document.getElementById('productNameUpdate').value = item.itemName || '';
            document.getElementById('categoryUpdate').value = item.itemCategory || '';
            document.getElementById('originalPriceUpdate').value = item.originalPrice ? item.originalPrice.toFixed(2) : '';
            document.getElementById('priceUpdate').value = item.suggestedPrice ? item.suggestedPrice.toFixed(2) : '';
            document.getElementById('descriptionUpdate').value = item.itemDescription || '';
            document.getElementById('isServedUpdate').value = item.isServed;
            document.getElementById('statusUpdate').value = item.itemStatus === 1 ? 'Still' : 'Sold Out';

            const imagePreview = document.getElementById('imagePreviewUpdate');
            imagePreview.src = item.itemImage || '/images/default-image.jpg';  
            imagePreview.style.display = 'block';

            
            
            const imageFileName = getFileNameFromPath(item.itemImage || '/images/default-image.jpg');
            

            
            $('#updateItemForm').modal('show');
        })
        .catch(error => {
            console.error('Error fetching item data:', error);
            alert('Failed to load item data!');
        });
}

// Hàm lấy tên file từ đường dẫn ảnh
function getFileNameFromPath(path) {
    if (path) {
        const pathParts = path.split('/');
        const fileName = pathParts[pathParts.length - 1]; 
        return fileName || 'No file chosen';  
    }
    return 'No file chosen'; 
}

// Xử lý sự kiện chọn ảnh mới
document.getElementById('imageUpdate').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const fileNameLabel = document.getElementById('fileNameLabel');

    if (file) {
        
        fileNameLabel.textContent = `Selected file: ${file.name}`;

        
        const reader = new FileReader();
        reader.onload = function (e) {
            const imagePreview = document.getElementById('imagePreviewUpdate');
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    } else {
       
        fileNameLabel.textContent = 'No file chosen';
    }
});


async function updateMenu(event) {
    event.preventDefault();

    const itemId = document.getElementById('itemIdUpdate').value;
    const itemName = document.getElementById('productNameUpdate').value;
    const category = document.getElementById('categoryUpdate').value;
    const originalPrice = document.getElementById('originalPriceUpdate').value;
    const price = document.getElementById('priceUpdate').value;
    const description = document.getElementById('descriptionUpdate').value;
    const status = document.getElementById('statusUpdate').value;
    const isServed = document.getElementById('isServedUpdate').value === '1' ? 1 : 0;

    const currentImage = document.getElementById('imagePreviewUpdate').src; // Lấy src của ảnh hiện tại

    // Kiểm tra điều kiện ảnh mới
    let imageFile = document.getElementById('imageUpdate').files[0];
    let itemImage = currentImage; 

    if (imageFile) {
        itemImage = await convertImageToBase64(imageFile); 
    }

    // Kiểm tra giá cả hợp lệ
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
        itemImage: itemImage,  
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
// Trong getMenu() - phần hiển thị danh sách món ăn
menuItems.forEach((item, index) => {
    const row = document.createElement('tr');
    row.id = `menu-row-${item.itemId}`;

    row.innerHTML = `
        <td>${index + 1}</td>
        <td><img src="${item.itemImage || '/images/default-image.jpg'}" alt="${item.itemName}" class="item-image" style="width: 50px; height: 50px;"></td>
        <td>${item.itemName || 'N/A'}</td>
        <td>${item.itemCategory || 'N/A'}</td>
        <td>${item.originalPrice ? item.originalPrice.toFixed(2) : 'N/A'}</td>
        <td>${item.suggestedPrice ? item.suggestedPrice.toFixed(2) : 'N/A'}</td>
        <td>${item.itemDescription || 'N/A'}</td>
        <td><span class="badge ${item.itemStatus === 1 ? 'badge-success' : 'badge-danger'}">${item.itemStatus === 1 ? 'Available' : 'Sold Out'}</span></td>
        <td>
            <button class="btn btn-success" onclick="viewMenu(${item.itemId})" data-toggle="modal" data-target="#readData"><i class="bi bi-eye"></i></button>
            <button class="btn btn-primary" onclick="openUpdateModal(${item.itemId})" data-toggle="modal" data-target="#updateItemForm"><i class="bi bi-pencil-square"></i></button>
            <button class="btn btn-danger" onclick="deleteMenu(${item.itemId})"><i class="bi bi-trash3"></i></button>
        </td>
    `;
    dataTable.appendChild(row);
});








