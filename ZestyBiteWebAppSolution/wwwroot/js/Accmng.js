    let accounts = [];
    renderAccountTable(accounts);

    function renderAccountTable(accounts) {
            const tbody = document.getElementById('accountTableBody');
    tbody.innerHTML = ''; // Clear old content

            accounts.forEach(account => {
        console.log(account.status);
    const row = document.createElement('tr');
                row.onclick = () => showPopup(account); // Updated to pass the entire account object

    row.innerHTML = `
    <td>${account.status == 1 ? 'Active' : 'Unactive'}</td>
    <td>${account.username}</td>
    <td>${account.password}</td>
    <td>${account.name}</td>
    <td>${account.phoneNumber}</td>
    <td>${account.address}</td>
    <td>${account.gender == 1 ? 'Male' : 'Female'}</td>
    <td>${account.email}</td>
    <td>${account.roleDescription}</td>
    `;

    tbody.appendChild(row);
            });
        }

    async function loadAccounts() {
            try {
                const response = await fetch('/api/account/getallacc');
    if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
    const accounts = await response.json();
    renderAccountTable(accounts);
            } catch (error) {
        console.error('Error fetching accounts:', error);
            }
        }

    function showPopup(account) { // Updated to take entire account object
        document.getElementById('popupTitle').innerText = 'Account Details';
    document.getElementById('status').value = account.status;
    document.getElementById('username').value = account.username;
    document.getElementById('password').value = account.password;
    document.getElementById('name').value = account.name;
    document.getElementById('phone').value = account.phoneNumber;
    document.getElementById('address').value = account.address;
    document.getElementById('gender').value = account.gender;
    document.getElementById('email').value = account.email;
    document.getElementById('roleId').value = account.roleDescription;

    document.getElementById('popupOverlay').style.display = 'block';
    document.getElementById('infoPopup').style.display = 'block';
        }

    function showCreatePopup() {
        document.getElementById('createPopupOverlay').style.display = 'block';
    document.getElementById('createPopup').style.display = 'block';
        }

    async function updateAccount() {
            const accountData = {
        status: document.getElementById('status').value,
    username: document.getElementById('username').value,
    password: document.getElementById('password').value,
    name: document.getElementById('name').value,
    phoneNumber: document.getElementById('phone').value,
    address: document.getElementById('address').value,
    gender: document.getElementById('gender').value,
    email: document.getElementById('email').value,
    roleId: document.getElementById('roleId').value
            };

    console.log(accountData); // Log the data to verify

    const response = await fetch(`/api/account/updateacc`, {
        method: 'POST',
    headers: {
        'Content-Type': 'application/json'
                },
    body: JSON.stringify(accountData)
            });

    if (response.ok) {
        await loadAccounts();
    closePopup();
            } else {
        console.error('Error updating account:', await response.text());
            }
        }

    async function createNewAccount() {
            const newAccountData = {
        username: document.getElementById('newUsername').value,
    password: document.getElementById('newPassword').value,
    gender: document.getElementById('newGender').value,
    phoneNumber: document.getElementById('newPhone').value,
    email: document.getElementById('newEmail').value,
    address: document.getElementById('newAddress').value,
    roleId: document.getElementById('newRole').value
            };

    const response = await fetch('/api/account/addstaff', {
        method: 'POST',
    headers: {
        'Content-Type': 'application/json'
                },
    body: JSON.stringify(newAccountData)
            });

    if (response.ok) {
        await loadAccounts();
    closeCreatePopup();
            } else {
        console.error('Error creating new account:', await response.text());
            }
        }

    async function manageStatus() {
            const username = document.getElementById('username').value;

    const response = await fetch(`/api/account/status`, {
        method: 'POST',
    headers: {
        'Content-Type': 'application/json'
                },
    body: JSON.stringify({username})
            });

    if (response.ok) {
        await loadAccounts();
    closePopup();
            } else {
        console.error('Error managing status:', await response.text());
            }
        }

    async function deleteAccount() {
            const username = document.getElementById('username').value;

    try {
                const response = await fetch(`/api/account/deleteacc`, {
        method: 'DELETE',
    headers: {
        'Content-Type': 'application/json'
                    },
    body: JSON.stringify({username})
                });

    if (response.ok) {
        await loadAccounts();
    console.error('Delete account successfully!');
    closePopup();
                } else {
                    const errorText = await response.text();
    throw new Error(errorText);
                }
            } catch (error) {
        console.error('Error deleting account:', error);
    alert('Error deleting account: ' + error.message);
            }
        }

    function closePopup() {
        document.getElementById('popupOverlay').style.display = 'none';
    document.getElementById('infoPopup').style.display = 'none';
        }

    function closeCreatePopup() {
        document.getElementById('createPopupOverlay').style.display = 'none';
    document.getElementById('createPopup').style.display = 'none';
        }

    function submitForm() {
            const formData = { };
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

    function searchAccounts() {
            const searchValue = document.getElementById('search').value.toLowerCase();
    const rows = document.querySelectorAll('#accountTableBody tr');

            rows.forEach(row => {
                const rowText = Array.from(row.cells).map(cell => cell.innerText.toLowerCase()).join(' ');
    row.style.display = rowText.includes(searchValue) ? '' : 'none';
            });
        }

    document.addEventListener('DOMContentLoaded', loadAccounts);