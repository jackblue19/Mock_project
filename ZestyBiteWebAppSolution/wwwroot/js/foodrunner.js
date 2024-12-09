    async function fetchUserData() {
           try {
               // Fetch the user data from the API
               const response = await fetch('/api/account');
    const userData = await response.json();

    // Populate the profile image
    const profileImg = document.querySelector('.profile-img');
    profileImg.src = userData.profileImage;
    profileImg.alt = userData.username;

    // Populate the username
    const usernameDiv = document.getElementById('name');
    usernameDiv.textContent = userData.username;

                   } catch (error) {
        console.error('Error fetching user data:', error);
                   }
               }

    async function fetchData() {
                    try {
                        const realityResponse = await fetch('/api/table/reality');
    const realityData = await realityResponse.json();
    populateTable(realityData, 'table-body');

    const virtualResponse = await fetch('/api/table/virtual');
    const virtualData = await virtualResponse.json();
    populateTable(virtualData, 'online-body');
                    } catch (error) {
        console.error('Error fetching data:', error);
                    }
                }

    function populateTable(data, tableBodyId) {
                    const tableBody = document.getElementById(tableBodyId);
    tableBody.innerHTML = ''; // Clear existing rows

    // Define the order of statuses
    const statusOrder = ['Waiting', 'Served', 'Empty'];

                    // Sort the data based on status order
                    data.sort((a, b) => statusOrder.indexOf(a.tableStatus) - statusOrder.indexOf(b.tableStatus));

                    data.forEach(item => {
                        const row = document.createElement('tr');
                        row.addEventListener('click', () => {
        alert(`Table ID: ${item.tableId}`);
                        });

    // Apply color based on status
    switch (item.tableStatus) {
                            case 'Waiting':
    row.classList.add('status-waiting');
    break;
    case 'Served':
    row.classList.add('status-served');
    break;
    case 'Empty':
    row.classList.add('status-empty');
    break;
    default:
    break;
                        }

    const cell1 = document.createElement('td');
    cell1.textContent = item.tableId;

    const cell2 = document.createElement('td');
    cell2.textContent = item.tableStatus;

    const cell3 = document.createElement('td');
    cell3.textContent = item.tableBill;
    cell3.style.display = 'none';

    row.appendChild(cell1);
    row.appendChild(cell2);
    row.appendChild(cell3);
    tableBody.appendChild(row);
                    });
                }
                 async function fetchTableItems(tableId) {
             try {
                 const response = await fetch(`/api/table/items?tableId=${tableId}`);
                 const items = await response.json();
                 const tableDetailItem = document.getElementById('table-detail-item');
                 tableDetailItem.innerHTML = ''; Clear existing rows

                 items.forEach(item => {
                     const row = document.createElement('tr');
                     const cell = document.createElement('td');
                     cell.textContent = item.itemName;
                     row.appendChild(cell);
                     tableDetailItem.appendChild(row);
                 });
             } catch (error) {
                 console.error('Error fetching table items:', error);
             }
         }

         async function fetchTableNotes(tableId) {
             try {
                 const response = await fetch(`/api/table/notes?tableId=${tableId}`);
                 const notes = await response.json();
                 const tableDetailNote = document.getElementById('table-detail-note');
                 tableDetailNote.innerHTML = ''; Clear existing rows

                 notes.forEach(note => {
                     const row = document.createElement('tr');
                     const cell = document.createElement('td');
                     cell.textContent = note.note;
                     row.appendChild(cell);
                     tableDetailNote.appendChild(row);
                 });
             } catch (error) {
                 console.error('Error fetching table notes:', error);
             }
         }

         Call fetchData when the page loads
window.onload = fetchData;
