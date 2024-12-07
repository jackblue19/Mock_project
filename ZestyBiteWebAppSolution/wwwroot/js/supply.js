// LIST
async function getSupplies() {
    try {
        const response = await fetch('/api/supply');  // Gọi API backend
        const supplies = await response.json();  // Dữ liệu trả về dạng JSON

        // Lấy bảng và thêm dữ liệu vào bảng
        const dataTable = document.getElementById('data');

        supplies.forEach((supply, index) => {
            const { text, badgeClass } = getStatusAndBadgeClass(supply.supplyStatus);
            const row = document.createElement('tr');
            row.id = `supply-row-${supply.supplyId}`;
            row.innerHTML = `
                        <td>${index + 1}</td>
                        <td>${supply.productName}</td>
                        <td>${supply.supplyQuantity}</td>
                        <td>${supply.supplyPrice}</td>
                        <td>${new Date(supply.dateImport).toLocaleDateString()}</td>
                        <td>${new Date(supply.dateExpiration).toLocaleDateString()}</td>
                        <td>${supply.vendorName}</td>
                        <td>${supply.vendorPhone}</td>
                        <td>${supply.vendorAddress}</td>
                         <td>${supply.supplyCategory}</td>
                         <td><span class="badge ${badgeClass}">${text}</span></td>   
                        <td>
                        <button class="btn btn-success" onclick="viewSupply(${supply.supplyId})" data-toggle="modal" data-target="#readData"><i class="bi bi-eye"></i></button>
                        <button class="btn btn-primary" onclick="editSupply(${supply.supplyId})" data-toggle="modal" data-target="#updateSupplyModal">
                                   <i class="bi bi-pencil-square"></i> </button>
                         <button class="btn btn-danger" onclick="deleteSupply(${supply.supplyId})">
            <i class="bi bi-trash3"></i>
                        </td>
                    `;
            dataTable.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching supplies:', error);
    }
}
window.onload = getSupplies;

function getStatusAndBadgeClass(supplyStatus) {
    if (supplyStatus === 1) {
        return { text: "Still", badgeClass: "bg-success" };
    } else if (supplyStatus === 0) {
        return { text: "Expired", badgeClass: "bg-danger" };
    } else {
        return { text: "Unknown", badgeClass: "bg-secondary" };
    }
}
//View 
async function viewSupply(id) {
    try {
        const response = await fetch(`/api/supply/${id}`);
        const supply = await response.json();
        console.log(supply); // Kiểm tra dữ liệu trả về từ API
        if (response.ok) {
            // Kiểm tra nếu productName tồn tại, nếu không sẽ gán 'N/A' để tránh trường hợp bị trống
            document.getElementById('proNamePro').value = supply.productName || 'N/A';
            document.getElementById('supplyQuantity').value = supply.supplyQuantity || '';
            document.getElementById('supplyPrice').value = supply.supplyPrice || '';
            document.getElementById('dateImport').value = supply.dateImport ? supply.dateImport.split('T')[0] : '';
            document.getElementById('dateExpiration').value = supply.dateExpiration ? supply.dateExpiration.split('T')[0] : '';
            document.getElementById('tableId').value = supply.tableId || '';
            document.getElementById('vendorName').value = supply.vendorName || '';
            document.getElementById('vendPhone').value = supply.vendorPhone || '';
            document.getElementById('vendorAddress').value = supply.vendorAddress || '';
            document.getElementById('supplyCategory').value = supply.supplyCategory || '';

            // Hiển thị modal
            const myModal = new bootstrap.Modal(document.getElementById('readData'));
            myModal.show();
        } else {
            alert("Không thể tải chi tiết của supply.");
        }
    } catch (error) {
        console.error('Lỗi khi lấy thông tin chi tiết của supply:', error);
        alert("Đã xảy ra lỗi khi tải thông tin chi tiết của supply.");
    }
}
// CREATE
document.getElementById("supplyForm").addEventListener("submit", async function (event) {
    event.preventDefault();
    // Lấy giá trị của ngày nhập và ngày hết hạn
    const dateImport = document.getElementById("DateI").value;
    const dateExpiration = document.getElementById("DateE").value;

    // Chuyển đổi ngày từ chuỗi sang đối tượng Date
    const importDate = new Date(dateImport);
    const expirationDate = new Date(dateExpiration);

    // Kiểm tra nếu dateExpiration trước dateImport
    if (expirationDate < importDate) {
        document.getElementById("dateExpirationError").style.display = "block";
        return;  // Dừng lại nếu điều kiện không hợp lệ
    } else {
        // Ẩn thông báo lỗi nếu ngày hợp lệ
        document.getElementById("dateExpirationError").style.display = "none";
    }
    // Kiểm tra giá trị của price
    const price = parseFloat(document.getElementById("price").value);
    if (price < 1) {
        document.getElementById("priceError").style.display = "block";
        return;  // Dừng lại nếu điều kiện không hợp lệ
    } else {
        document.getElementById("priceError").style.display = "none";
    }

    const supplyData = {
        productName: document.getElementById("productName").value,
        supplyQuantity: parseInt(document.getElementById("quantity").value),
        supplyPrice: parseFloat(document.getElementById("price").value),
        supplyStatus: 1,
        dateImport: document.getElementById("DateI").value,
        dateExpiration: document.getElementById("DateE").value,
        tableId: document.getElementById("TabID").value,
        vendorName: document.getElementById("vName").value,
        vendorPhone: document.getElementById("vendorPhone").value,
        vendorAddress: document.getElementById("vAdress").value,
        supplyCategory: document.getElementById("category").value
    };

    try {
        const response = await fetch("/api/Supply/create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(supplyData)
        });

        // Kiểm tra nếu phản hồi từ API thành công
        if (!response.ok) {
            // Nếu không thành công, hiển thị thông báo lỗi với dữ liệu trả về
            const error = await response.json();
            throw new Error(error.message || "Unknown error occurred");
        }

        // Nếu thành công, thông báo cho người dùng và làm mới trang
        const result = await response.json();
        alert("Supply created successfully!");
        console.log("Result:", result);
        location.reload();

    } catch (err) {
        // Hiển thị thông báo lỗi cho người dùng
        console.error("Error creating supply:", err);
        alert("Failed to create supply: " + err.message);
    }
});

//DELETE
async function deleteSupply(id) {
    if (!confirm("Bạn có chắc chắn muốn xóa supply này?")) {
        return; // Người dùng hủy xóa
    }

    try {
        const response = await fetch(`/api/supply/delete/${id}`, {
            method: 'DELETE',
        });

        if (response.ok) {
            alert("Xóa supply thành công!");
            // Xóa hàng tương ứng khỏi bảng 
            document.getElementById(`supply-row-${id}`).remove();
        } else {
            alert("Không thể xóa supply. Vui lòng thử lại.");
        }
    } catch (error) {
        console.error("Lỗi khi xóa supply:", error);
        alert("Đã xảy ra lỗi khi xóa supply.");
    }
}

