// Sample data array
const menuData = [
    {  id: 1,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 2,image: "/Content/images/pizza-1.jpg",name: "drink",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 3,image: "/Content/images/pizza-2.jpg",name: "burger",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Sold Out" },
    {  id: 4,image: "/Content/images/pizza-3.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 5,image: "/Content/images/pizza-4.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Sold Out" },
    {  id: 6,image: "/Content/images/pizza-5.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 7,image: "/Content/images/pizza-6.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 8,image: "/Content/images/pizza-7.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 9,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 10,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Sold Out" },
    {  id: 10,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 10,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Sold Out" },
    {  id: 10,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Still" },
    {  id: 10,image: "/Content/images/pizza-1.jpg",name: "Pizza",category: "Food",price: "10.000",description: "Ngon tuyệt đối",status: "Sold Out" },

    
];
// Variables for pagination
let currentPage = 1;
const itemsPerPage = 5; // Adjust as needed
let filteredData = [...menuData]; // Start with all items

// Function to render table data with pagination
function renderTableData() {
    const tbody = document.getElementById("data");
    tbody.innerHTML = ""; // Clear existing data
    // Calculate the start and end indices of items on the current page
    const start = (currentPage - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const paginatedData = filteredData.slice(start, end);

    // Render the current page of items
    paginatedData.forEach(item => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${item.id}</td>
            <td><img src="${item.image}" alt="${item.name}" class="item-image"></td>
            <td>${item.name}</td>
            <td>${item.category}</td>
            <td>${item.price}</td>
            <td>${item.description}</td>
            <td>
                <span class="badge ${item.status === 'Still' ? 'bg-success' : 'bg-danger'}">
                    ${item.status === 'Still' ? 'Still' : 'Sold Out'}
                </span>
            </td>
            <td>
                <button class="btn btn-success" data-toggle="modal" data-target="#readData"><i class="bi bi-eye"></i></button>
                <button class="btn btn-primary"><i class="bi bi-pencil-square"></i></button>
                <button class="btn btn-danger"><i class="bi bi-trash3"></i></button>
            </td>
        `;
        tbody.appendChild(row);
    });

    renderPaginationControls();
}

// Function to render pagination controls with page numbers
function renderPaginationControls() {
    const paginationControls = document.getElementById("pagination");
    paginationControls.innerHTML = ""; // Clear existing controls

    const totalPages = Math.ceil(filteredData.length / itemsPerPage);

    // Previous button
    if (currentPage > 1) {
        const prevButton = document.createElement("button");
        prevButton.textContent = "Previous";
        prevButton.classList.add("btn", "btn-secondary", "me-2");
        prevButton.style.backgroundColor = "rgba(113, 99, 186, 1)";
        prevButton.addEventListener("click", () => {
            currentPage--;
            renderTableData();
        });
        paginationControls.appendChild(prevButton);
    }

    // Page number buttons
    for (let i = 1; i <= totalPages; i++) {
        const pageButton = document.createElement("button");
        pageButton.textContent = i;
        pageButton.classList.add("btn", "me-1");
        pageButton.classList.add(i === currentPage ? "btn-primary" : "btn-light");

        pageButton.addEventListener("click", () => {
            currentPage = i;
            renderTableData();
        });
        paginationControls.appendChild(pageButton);
    }

    // Next button
    if (currentPage < totalPages) {
        const nextButton = document.createElement("button");
        nextButton.textContent = "Next";
        nextButton.classList.add("btn", "btn-secondary");
        nextButton.style.backgroundColor = "rgba(113, 99, 186, 1)";
        nextButton.addEventListener("click", () => {
            currentPage++;
            renderTableData();
        });
        paginationControls.appendChild(nextButton);
    }
}

// Function to handle search and filter menu items
function handleSearch() {
    const query = document.getElementById("query").value.toLowerCase();

    // Filter menuData based on the search query
    filteredData = menuData.filter(item => {
        return (
            item.name.toLowerCase().includes(query) ||
            item.category.toLowerCase().includes(query) ||
            item.description.toLowerCase().includes(query) ||
            item.price.toLowerCase().includes(query) ||
            item.id.toString().includes(query)||
            item.status.toLowerCase().includes(query) // Search by status
        );
    });

    // Reset to first page after a search
    currentPage = 1;

    // Re-render table data and pagination after search
    renderTableData();
}

// Event listener for search input
document.getElementById("query").addEventListener("input", handleSearch);

// Call the function to render the initial data
renderTableData();

// Function to handle form submission and add a new item
document.getElementById('myForm').addEventListener('submit', function(event) {
    event.preventDefault(); // Prevent the default form submission

    // Get values from the form
    const itemId = document.getElementById('itemId').value; // Item ID (read-only)
    const image = document.getElementById('image').files[0]; // Image file
    const productName = document.getElementById('productName').value;
    const category = document.getElementById('category').value;
    const price = document.getElementById('price').value;
    const description = document.getElementById('description').value;
    const status = document.getElementById('status').value;

    // Check if all required fields are filled
    if (!productName || !category || !price || !description || !status) {
        alert("Please fill in all fields.");
        return;
    }

    // Create a new item object
    const newItem = {
        id: menuData.length + 1, // Item ID should be unique, so using menuData length for new ID
        image: URL.createObjectURL(image), // Use URL.createObjectURL to preview the image
        name: productName,
        category: category,
        price: price,
        description: description,
        status: status
    };

    // Add the new item to the menuData array
    menuData.push(newItem);
    filteredData = [...menuData]; // Update filteredData to include the new item

    // Re-render the table data with the updated menuData array
    renderTableData();


    // Close the modal after submitting
    $('#SupplyForm').modal('hide');

    // Reset the form
    document.getElementById('myForm').reset();

    // Clear the image preview
    document.getElementById('imagePreview').style.display = 'none';
});

// Function to preview the image selected in the file input
document.getElementById('image').addEventListener('change', function(event) {
    const imagePreview = document.getElementById('imagePreview');
    const file = event.target.files[0];

    if (file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block'; // Show the image preview
            imagePreview.style.width='200px'; // Show the image preview
        };
        reader.readAsDataURL(file);
    } else {
        imagePreview.style.display = 'none'; // Hide the image preview if no file is selected
    }
});

