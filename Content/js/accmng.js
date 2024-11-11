let accounts = [];
// Render account table
function renderAccountTable() {
    const tbody = document.getElementById('accountTableBody');
    tbody.innerHTML = accounts.map(account => `
        <tr onclick="showPopup(${account.id})">
            <td>${account.id}</td>
            <td>${account.username}</td>
            <td>${account.password}</td>
            <td>${account.name}</td>
            <td>${account.phone}</td>
            <td>${account.address}</td>
            <td>${account.gender}</td>
            <td>${account.email}</td>
            <td>${account.verificationCode}</td>
            <td>${account.roleId}</td>
        </tr>
    `).join('');
}

// Show popup for editing or creating an account
function showPopup(id) {
    const account = id ? accounts.find(a => a.id === id) : {};
    const isNewAccount = !id;

    document.getElementById('popupTitle').textContent = isNewAccount ? 'Create New Account' : 'Edit Account';

    const fields = [
        { name: 'id', label: 'ID', type: 'text', readonly: true },
        { name: 'username', label: 'Username', type: 'text' },
        { name: 'password', label: 'Password', type: 'password' },
        { name: 'name', label: "User's Name", type: 'text' },
        { name: 'phone', label: 'Phone Number', type: 'text' },
        { name: 'address', label: 'Address', type: 'text' },
        { name: 'gender', label: 'Gender', type: 'select', options: ['Male', 'Female', 'Other'] },
        { name: 'email', label: 'Email', type: 'text' },
        { name: 'verificationCode', label: 'Verification Code', type: 'text' },
        { name: 'roleId', label: 'Role ID', type: 'text' }
    ];

    const infoTableBody = document.getElementById('infoTableBody');
    infoTableBody.innerHTML = fields.map(field => `
        <tr>
            <td><strong>${field.label}</strong></td>
            <td>
                ${field.type === 'select' 
                    ? `<select id="${field.name}" ${field.readonly ? 'readonly' : ''}>
                        ${field.options.map(option => `<option value="${option}" ${account[field.name] === option ? 'selected' : ''}>${option}</option>`).join('')}
                       </select>`
                    : `<input type="${field.type}" id="${field.name}" value="${account[field.name] || ''}" ${field.readonly ? 'readonly' : ''}>`
                }
            </td>
        </tr>
    `).join('');

    document.getElementById('submitButton').textContent = isNewAccount ? 'Create' : 'Update';
    document.getElementById('deleteButton').style.display = isNewAccount ? 'none' : 'block';

    document.getElementById('popupOverlay').style.display = 'block';
    document.getElementById('infoPopup').style.display = 'block';
}

// Show create popup
function showCreatePopup() {
    showPopup();
}

// Close popup
function closePopup() {
    document.getElementById('popupOverlay').style.display = 'none';
    document.getElementById('infoPopup').style.display = 'none';
}

// Submit form (create or update account)
function submitForm() {
    const formData = {};
    document.querySelectorAll('#infoTableBody input, #infoTableBody select').forEach(input => {
        formData[input.id] = input.value;
    });

    if (formData.id) {
        const index = accounts.findIndex(a => a.id === parseInt(formData.id));
        if (index !== -1) {
            accounts[index] = { ...accounts[index], ...formData };
        }
    } else {
        formData.id = accounts.length + 1;
        accounts.push(formData);
    }

    localStorage.setItem('accounts', JSON.stringify(accounts));
    renderAccountTable();
    closePopup();
}

// Delete account
function deleteAccount() {
    const accountId = parseInt(document.getElementById('id').value);
    accounts = accounts.filter(account => account.id !== accountId);

    localStorage.setItem('accounts', JSON.stringify(accounts));
    renderAccountTable();
    closePopup();
}

// Search accounts
function searchAccounts() {
    const searchValue = document.getElementById('search').value.toLowerCase();
    const rows = document.querySelectorAll('#accountTableBody tr');

    rows.forEach(row => {
        const rowText = Array.from(row.cells).map(cell => cell.innerText.toLowerCase()).join(' ');
        row.style.display = rowText.includes(searchValue) ? '' : 'none';
    });
}

// Initialize page and reset accounts
document.addEventListener('DOMContentLoaded', loadAccounts);

// Load accounts
function loadAccounts() {
    accounts = [
        { id: 1, username: "user1", password: "pass123", name: "John Doe", phone: "123456789", address: "123 Main St", gender: "Male", email: "john@example.com", verificationCode: "666161", roleId: 2 },
        { id: 2, username: "user2", password: "pass456", name: "Jane Smith", phone: "987654321", address: "456 Elm St", gender: "Female", email: "jane@example.com", verificationCode: "123456", roleId: 1 },
        { id: 3, username: "user3", password: "pass789", name: "Alice Johnson", phone: "555123456", address: "789 Maple Ave", gender: "Female", email: "alice@example.com", verificationCode: "987654", roleId: 1 },
        { id: 4, username: "user4", password: "pass321", name: "Bob Brown", phone: "555987654", address: "321 Oak St", gender: "Male", email: "bob@example.com", verificationCode: "456789", roleId: 2 },
        { id: 5, username: "user5", password: "pass654", name: "Charlie Green", phone: "555000111", address: "987 Pine St", gender: "Other", email: "charlie@example.com", verificationCode: "654321", roleId: 3 },
        { id: 6, username: "user6", password: "pass963", name: "Dave White", phone: "555444222", address: "654 Cedar St", gender: "Male", email: "dave@example.com", verificationCode: "963852", roleId: 2 },
        { id: 7, username: "user7", password: "pass852", name: "Eve Black", phone: "555333111", address: "852 Willow Rd", gender: "Female", email: "eve@example.com", verificationCode: "852741", roleId: 1 },
        { id: 8, username: "user8", password: "pass147", name: "Frank Grey", phone: "555222000", address: "147 Birch Rd", gender: "Male", email: "frank@example.com", verificationCode: "147258", roleId: 3 },
        { id: 9, username: "user9", password: "pass369", name: "Grace Blue", phone: "555111333", address: "369 Spruce Rd", gender: "Female", email: "grace@example.com", verificationCode: "369258", roleId: 2 },
        { id: 10, username: "user10", password: "pass753", name: "Henry Orange", phone: "555888777", address: "753 Ash Ln", gender: "Male", email: "henry@example.com", verificationCode: "753951", roleId: 1 },
        { id: 11, username: "user11", password: "pass111", name: "Isabella White", phone: "555111222", address: "111 Cherry St", gender: "Female", email: "isabella@example.com", verificationCode: "111222", roleId: 2 },
        { id: 12, username: "user12", password: "pass222", name: "Jack Black", phone: "555222333", address: "222 Peach St", gender: "Male", email: "jack@example.com", verificationCode: "222333", roleId: 1 },
        { id: 13, username: "user13", password: "pass333", name: "Liam Brown", phone: "555333444", address: "333 Plum St", gender: "Male", email: "liam@example.com", verificationCode: "333444", roleId: 3 },
        { id: 14, username: "user14", password: "pass444", name: "Mia Green", phone: "555444555", address: "444 Apricot St", gender: "Female", email: "mia@example.com", verificationCode: "444555", roleId: 2 },
        { id: 15, username: "user15", password: "pass555", name: "Noah Blue", phone: "555555666", address: "555 Fig St", gender: "Male", email: "noah@example.com", verificationCode: "555666", roleId: 1 },
    ];

    const savedAccounts = localStorage.getItem('accounts');
    if (savedAccounts) {
        accounts = JSON.parse(savedAccounts);
    }
    renderAccountTable();
}

