let payHistory = []; // Store payment history data

// Function to render the account table rows
function renderAccountTable() {
    const tbody = document.getElementById('accountTableBody');
    tbody.innerHTML = payHistory.map(payHistorys => `
        <tr onclick="showPopup(${payHistorys.billID})">
            <td>${payHistorys.billID}</td>
            <td>${payHistorys.billStatus}</td>
            <td>${payHistorys.totalCost}</td>
            <td>${payHistorys.billDateTime}</td>
            <td>${payHistorys.billType}</td>
        </tr>
    `).join('');
}

// Function to display popup with selected bill details
function showPopup(billID) {
    const selectedBill = payHistory.find(p => p.billID === billID);

    if (selectedBill) {
        document.getElementById('popupTitle').innerText = "Bill Details";

        // Update the fields with selected bill details
        document.querySelector('.company-info').innerHTML = `
            <p>
                <strong>Account ID:</strong> ${selectedBill.accountId}<br>
                <strong>Account Name:</strong> ${selectedBill.accountId}<br>
                <strong>Table No:</strong> ${selectedBill.tableID}
            </p>
        `;

        document.querySelector('.invoice-info').innerHTML = `
            <p><strong>Bill ID:</strong> ${selectedBill.billID}</p>
            <p><strong>Payment ID:</strong> ${selectedBill.paymentID}</p>
            <p><strong>Bill DateTime:</strong> ${selectedBill.billDateTime}</p>
        `;

        document.getElementById('popupOverlay').style.display = 'block';
        document.getElementById('infoPopup').style.display = 'block';
    }
}

// Close popup function
function closePopup() {
    document.getElementById('popupOverlay').style.display = 'none';
    document.getElementById('infoPopup').style.display = 'none';
}

// Initialize page and load payment history
document.addEventListener('DOMContentLoaded', loadAccounts);

function loadAccounts() {
    payHistory = [
        { billID: 1, billStatus: "Paid", paymentID: "3", accountId: "3", tableID: "2", totalCost: "1000", billDateTime: "21/10/2024", billType: "Deposit" },
        { billID: 2, billStatus: "Unpaid", paymentID: "4", accountId: "5", tableID: "3", totalCost: "1500", billDateTime: "22/10/2024", billType: "Deposit" },
        { billID: 3, billStatus: "Paid", paymentID: "5", accountId: "2", tableID: "1", totalCost: "1200", billDateTime: "23/10/2024", billType: "Withdrawal" },
        { billID: 4, billStatus: "Unpaid", paymentID: "6", accountId: "7", tableID: "4", totalCost: "2000", billDateTime: "24/10/2024", billType: "Deposit" },
        { billID: 5, billStatus: "Paid", paymentID: "7", accountId: "6", tableID: "2", totalCost: "1700", billDateTime: "25/10/2024", billType: "Withdrawal" },
        { billID: 6, billStatus: "Unpaid", paymentID: "8", accountId: "4", tableID: "5", totalCost: "900", billDateTime: "26/10/2024", billType: "Deposit" },
        { billID: 7, billStatus: "Paid", paymentID: "9", accountId: "8", tableID: "3", totalCost: "1400", billDateTime: "27/10/2024", billType: "Deposit" },
        { billID: 8, billStatus: "Unpaid", paymentID: "10", accountId: "9", tableID: "6", totalCost: "1600", billDateTime: "28/10/2024", billType: "Withdrawal" },
        { billID: 9, billStatus: "Paid", paymentID: "11", accountId: "10", tableID: "4", totalCost: "1300", billDateTime: "29/10/2024", billType: "Deposit" },
        { billID: 10, billStatus: "Unpaid", paymentID: "12", accountId: "11", tableID: "1", totalCost: "800", billDateTime: "30/10/2024", billType: "Withdrawal" },
        { billID: 11, billStatus: "Paid", paymentID: "13", accountId: "12", tableID: "7", totalCost: "1100", billDateTime: "31/10/2024", billType: "Deposit" },
        { billID: 12, billStatus: "Paid", paymentID: "13", accountId: "13", tableID: "7", totalCost: "1100", billDateTime: "31/10/2024", billType: "Deposit" },
        { billID: 13, billStatus: "Paid", paymentID: "13", accountId: "13", tableID: "7", totalCost: "1100", billDateTime: "31/10/2024", billType: "Deposit" }
    ];

    const savedPayHistory = localStorage.getItem('payHistory');
    if (savedPayHistory) {
        payHistory = JSON.parse(savedPayHistory);
    }
    renderAccountTable();
}