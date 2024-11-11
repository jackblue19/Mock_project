document.addEventListener("DOMContentLoaded", function () {
    const itemsPerPage = 10;
    let currentPage = 1;
    let selectedCategory = ''; // Initialize the selected category filter
    let cart = [];

    const menuItems = [
        {
            C04_Item_Image: "../Content/images/bg_2.png",
            C04_Item_Name: "Pizza Margherita",
            C04_Item_Description: "Classic Italian pizza with tomatoes, mozzarella cheese, and fresh basil.",
            C04_Price: 12.99,
            C04_Item_Category: "Main course",
            C04_Item_Status: "Available"
        },

        {
            C04_Item_Image: "../Content/images/drink-6.jpg",
            C04_Item_Name: "Spaghetti Carbonara",
            C04_Item_Description: "Traditional Roman pasta dish made with eggs, cheese, pancetta, and pepper.",
            C04_Price: 14.99,
            C04_Item_Category: "Main course",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/burger-1.jpg",
            C04_Item_Name: "Tiramisu",
            C04_Item_Description: "Delicious coffee-flavored Italian dessert made with mascarpone cheese and cocoa.",
            C04_Price: 6.99,
            C04_Item_Category: "Dessert",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-7.jpg",
            C04_Item_Name: "Caesar Salad",
            C04_Item_Description: "Crisp romaine lettuce with Caesar dressing, croutons, and parmesan cheese.",
            C04_Price: 8.99,
            C04_Item_Category: "Salad",
            C04_Item_Status: "Unavailable"
        },
        {
            C04_Item_Image: "../Content/images/drink-1.jpg",
            C04_Item_Name: "Margarita",
            C04_Item_Description: "Classic cocktail made with tequila, lime juice, and triple sec.",
            C04_Price: 10.99,
            C04_Item_Category: "Drink",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-9.jpg",
            C04_Item_Name: "Chocolate Cake",
            C04_Item_Description: "Rich and moist chocolate cake topped with creamy chocolate frosting.",
            C04_Price: 5.99,
            C04_Item_Category: "Dessert",
            C04_Item_Status: "Unavailable"
        },
        {
            C04_Item_Image: "../Content/images/drink-1.jpg",
            C04_Item_Name: "Fruit Salad",
            C04_Item_Description: "A refreshing mix of seasonal fruits, served chilled.",
            C04_Price: 7.99,
            C04_Item_Category: "Fruit",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/burger-3.jpg",
            C04_Item_Name: "Grilled Chicken",
            C04_Item_Description: "Juicy grilled chicken breast served with a side of vegetables.",
            C04_Price: 15.99,
            C04_Item_Category: "Main course",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-2.jpg",
            C04_Item_Name: "Lemonade",
            C04_Item_Description: "Refreshing homemade lemonade, perfect for a hot day.",
            C04_Price: 3.99,
            C04_Item_Category: "Drink",
            C04_Item_Status: "Unavailable"
        },
        {
            C04_Item_Image: "../Content/images/burger-3.jpg",
            C04_Item_Name: "Lasagna",
            C04_Item_Description: "Layers of pasta, meat, cheese, and rich tomato sauce baked to perfection.",
            C04_Price: 13.99,
            C04_Item_Category: "Main course",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-5.jpg",
            C04_Item_Name: "Cheesecake",
            C04_Item_Description: "Creamy cheesecake with a graham cracker crust and fruit topping.",
            C04_Price: 7.49,
            C04_Item_Category: "Dessert",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-4.jpg",
            C04_Item_Name: "Greek Salad",
            C04_Item_Description: "A fresh mix of tomatoes, cucumbers, olives, and feta cheese.",
            C04_Price: 9.99,
            C04_Item_Category: "Salad",
            C04_Item_Status: "Unavailable"
        },
        {
            C04_Item_Image: "../Content/images/drink-3.jpg",
            C04_Item_Name: "Iced Tea",
            C04_Item_Description: "Chilled tea served with lemon and mint for a refreshing drink.",
            C04_Price: 2.99,
            C04_Item_Category: "Drink",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-7.jpg",
            C04_Item_Name: "Pavlova",
            C04_Item_Description: "Light meringue dessert topped with whipped cream and fresh fruits.",
            C04_Price: 8.49,
            C04_Item_Category: "Dessert",
            C04_Item_Status: "Unavailable"
        },
        {
            C04_Item_Image: "../Content/images/drink-9.jpg",
            C04_Item_Name: "Fruit Smoothie",
            C04_Item_Description: "A blend of fresh fruits and yogurt, perfect for a healthy snack.",
            C04_Price: 5.99,
            C04_Item_Category: "Fruit",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/burger-1.jpg",
            C04_Item_Name: "Beef Stroganoff",
            C04_Item_Description: "Tender beef strips cooked in a creamy mushroom sauce served over egg noodles.",
            C04_Price: 16.99,
            C04_Item_Category: "Main course",
            C04_Item_Status: "Available"
        },
        {
            C04_Item_Image: "../Content/images/drink-4.jpg",
            C04_Item_Name: "Soda",
            C04_Item_Description: "Chilled soft drink available in various flavors.",
            C04_Price: 1.99,
            C04_Item_Category: "Drink",
            C04_Item_Status: "Unavailable"
        }
        // Add more items here with appropriate categories and statuses
    ];

    function searchMenuItems() {
        const searchValue = document.getElementById('search-box').value.toLowerCase();
        const filteredItems = menuItems.filter(item =>
            (item.C04_Item_Name.toLowerCase().includes(searchValue) ||
                item.C04_Item_Description.toLowerCase().includes(searchValue)) &&
            (selectedCategory === '' || item.C04_Item_Category === selectedCategory) &&
            item.C04_Item_Status === "Available"
        );

        const leftColumn = document.getElementById('left-column');
        const rightColumn = document.getElementById('right-column');
        leftColumn.innerHTML = '';
        rightColumn.innerHTML = '';

        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        const currentItems = filteredItems.slice(startIndex, endIndex);

        currentItems.forEach((item, index) => {
            const menuItemHTML = `
                <div class="menu-item ">
                    <div class="menu-item-left">
                        <img src="${item.C04_Item_Image}" alt="${item.C04_Item_Name}" class="C04_Item_Image"/>
                    </div>
                    <div class="menu-item-right">
                        <div class="menu-item-top">
                            <h2 class="C04_Item_Name">${item.C04_Item_Name}</h2>
                            <p class="C04_Price">$${item.C04_Price.toFixed(2)}</p>
                        </div>
                        <p class="C04_Item_Description">${item.C04_Item_Description}</p>
                    </div>
                </div>
            `;

            if (index < currentItems.length / 2) {
                leftColumn.innerHTML += menuItemHTML;
            } else {
                rightColumn.innerHTML += menuItemHTML;
            }
        });

        updatePagination(filteredItems.length);
        currentPage = 1; // Reset to first page after search
    }

    // Add this event listener in the DOMContentLoaded function
    document.getElementById('search-box').addEventListener('input', searchMenuItems);

    // add event listener for filter
    document.querySelectorAll('.category-btn').forEach(button => {
        button.addEventListener('click', function () {
            // Loại bỏ class active từ tất cả các nút
            document.querySelectorAll('.category-btn').forEach(btn => {
                btn.classList.remove('active');
            });

            // Thêm class active cho nút được chọn
            this.classList.add('active');

            // Lấy danh mục từ thuộc tính data-category
            const category = this.getAttribute('data-category');

            // Gọi hàm lọc
            filterByCategory(category);
        });
    });

    function filterMenuItems() {
        // Nếu không có danh mục được chọn, trả về toàn bộ danh sách
        if (!selectedCategory) {
            return menuItems;
        }

        // Lọc các mục theo danh mục đã chọn
        return menuItems.filter(item =>
            item.C04_Item_Category === selectedCategory &&
            item.C04_Item_Status === "Available"
        );
    }

    // Function to add item to cart
    function addToCart(item) {
        // Add item to cart array
        cart.push(item);

        // Show a success notification
        showNotification("Successfully added to your cart!");
    }

    // Show a success notification
    function showNotification(message) {
        const notification = document.createElement("div");
        notification.textContent = message;
        notification.style.position = "fixed";
        notification.style.bottom = "20px";
        notification.style.right = "20px";
        notification.style.backgroundColor = "#4CAF50";
        notification.style.color = "white";
        notification.style.padding = "10px";
        notification.style.borderRadius = "5px";
        notification.style.fontSize = "16px";

        document.body.appendChild(notification);

        // Remove notification after 3 seconds
        setTimeout(() => {
            notification.remove();
        }, 3000);
    }

    // Function to open popup
    function openItemPopup(item) {
        const popup = document.getElementById('item-popup');
        const popupImage = document.getElementById('popup-image');
        const popupName = document.getElementById('popup-name');
        const popupPrice = document.getElementById('popup-price');
        const popupDescription = document.getElementById('popup-description');
        const popupCategory = document.getElementById('popup-category');
        const addToCartButton = document.getElementById('add-to-cart');

        // Set popup content
        popupImage.src = item.C04_Item_Image;
        popupName.textContent = item.C04_Item_Name;
        popupPrice.textContent = `Price: $${item.C04_Price.toFixed(2)}`;
        popupDescription.textContent = item.C04_Item_Description;
        popupCategory.textContent = item.C04_Item_Category;

        // Show popup
        popup.style.display = 'flex';

        // Add click event for "Add to cart" button
        addToCartButton.onclick = function () {
            addToCart(item);
            closeItemPopup(); // Close the popup after adding to cart
        };
    }

    // Function to close popup
    function closeItemPopup() {
        const popup = document.getElementById('item-popup');
        popup.style.display = 'none';

        if (popupCloseBtn) {
            popupCloseBtn.addEventListener('click', closeItemPopup);
        }

    }

    // Close popup when close button is clicked
    const popupCloseBtn = document.querySelector('.popup-close');
    const popup = document.getElementById('item-popup');

    if (popupCloseBtn) {
        popupCloseBtn.addEventListener('click', function () {
            popup.style.display = 'none';
        });
    }

    // Optional: Close popup when clicking outside of the popup content
    popup.addEventListener('click', function (event) {
        if (event.target === popup) {
            popup.style.display = 'none';
        }
    });

    // Modify your renderMenuItems function to add click event to menu items
    function renderMenuItems() {
        const filteredItems = filterMenuItems();
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        const currentItems = filteredItems.slice(startIndex, endIndex);

        const leftColumn = document.getElementById('left-column');
        const rightColumn = document.getElementById('right-column');
        leftColumn.innerHTML = '';
        rightColumn.innerHTML = '';

        currentItems.forEach((item, index) => {
            const menuItemElement = document.createElement('div');
            menuItemElement.className = 'menu-item';
            menuItemElement.innerHTML = `
            <div class="menu-item-left">
                <img src="${item.C04_Item_Image}" alt="${item.C04_Item_Name}" class="C04_Item_Image"/>
            </div>
            <div class="menu-item-right">
                <div class="menu-item-top">
                    <h2 class="C04_Item_Name">${item.C04_Item_Name}</h2>
                    <p class="C04_Price">$${item.C04_Price.toFixed(2)}</p>
                </div>
                <p class="C04_Item_Description">${item.C04_Item_Description}</p>
            </div>
        `;

            // Add click event to open popup
            menuItemElement.addEventListener('click', () => openItemPopup(item));

            if (index < currentItems.length / 2) {
                leftColumn.appendChild(menuItemElement);
            } else {
                rightColumn.appendChild(menuItemElement);
            }
        });

        updatePagination(filteredItems.length);

        // Add event listener to close popup
        document.querySelector('.popup-close').addEventListener('click', closeItemPopup);

        // Close popup when clicking outside of it
        document.getElementById('item-popup').addEventListener('click', (e) => {
            if (e.target === e.currentTarget) {
                closeItemPopup();
            }
        });
    }

    document.getElementById('prev-page').addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            renderMenuItems();
        }
    });

    document.getElementById('next-page').addEventListener('click', () => {
        const filteredItems = filterMenuItems();
        const totalPages = Math.ceil(filteredItems.length / itemsPerPage);
        if (currentPage < totalPages) {
            currentPage++;
            renderMenuItems();
        }
    });

    function filterByCategory(category) {
        selectedCategory = category;
        currentPage = 1; // Reset to the first page after filter change
        renderMenuItems();
    }

    // Initial render
    renderMenuItems();
});
