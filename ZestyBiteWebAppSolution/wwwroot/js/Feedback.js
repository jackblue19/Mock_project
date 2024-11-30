document.addEventListener("DOMContentLoaded", function () {
    const dishSelect = document.getElementById('dish-select');

    // Dummy function to simulate fetching data from the Bill table
    function fetchBillItems() {
        return [
            { item: "Pizza Margherita" },
            { item: "Pepperoni Pizza" },
            { item: "Hawaiian Pizza" },
            { item: "Veggie Pizza" },
            { item: "BBQ Chicken Pizza" }
        ];
    }

    // Populate the select element with fetched items
    function populateDishSelect(items) {
        items.forEach(item => {
            const option = document.createElement('option');
            option.value = item.item;
            option.textContent = item.item;
            dishSelect.appendChild(option);
        });
    }

    // Fetch items and populate select element
    const items = fetchBillItems();
    populateDishSelect(items);

    document.getElementById('submitFeedback').addEventListener('click', function () {
        const feedbackDish = document.getElementById('feedbackDish').value;
        const feedbackMessage = document.getElementById('feedbackMessage').value;

        fetch('https://localhost:7131/Feedback/SubmitFeedback', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                content: feedbackMessage,
                dateTime: new Date().toISOString(),
                accountId: 1, // Replace with the actual account ID
                username: 'john_doe', // Replace with the actual username
                itemId: 1, // Replace with the actual item ID (you might need to retrieve this based on the dish name)
                itemName: feedbackDish
            })
        })
            .then(response => response.json())
            .then(data => {
                alert('Feedback submitted successfully! Redirecting to Blog page...');
                setTimeout(() => {
                    window.location.href = '/Blog';
                }, 1500); // Redirect after 1.5 seconds
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });
});
