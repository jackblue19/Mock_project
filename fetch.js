import data from './data.js';

const fetchData = async () => {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve(data);
        }, 1000); // Simulates network delay
    });
};

const displayData = async () => {
    try {
        const fetchedData = await fetchData();
        const container = document.getElementById('data-container');

        fetchedData.forEach(item => {
            const itemElement = document.createElement('div');
            itemElement.classList.add('data-item');
            itemElement.innerHTML = `
                <h3>${item.source_text}</h3>
                <p>Comment: ${item.comment}</p>
                <p>Rating: ${item.translation_rating}</p>
                <p>Suggested Translation: ${item.suggested_translation}</p>
            `;
            container.appendChild(itemElement);
        });
    } catch (error) {
        console.error('Error fetching data:', error);
    }
};

const addCommentFunctionality = () => {
    const addCommentBtn = document.getElementById('add-comment-btn');
    const commentInputContainer = document.getElementById('comment-input-container');
    const submitCommentBtn = document.getElementById('submit-comment-btn');
    const commentInput = document.getElementById('comment-input');

    addCommentBtn.addEventListener('click', () => {
        commentInputContainer.style.display = 'block';
        commentInput.focus();
    });

    submitCommentBtn.addEventListener('click', () => {
        const newComment = commentInput.value.trim();
        if (newComment) {
            const newCommentElement = document.createElement('div');
            newCommentElement.classList.add('data-item');
            newCommentElement.innerHTML = `<p>${newComment}</p>`;
            document.getElementById('data-container').appendChild(newCommentElement);
            commentInput.value = ''; // Clear the input
            commentInputContainer.style.display = 'none'; // Hide the input field
        } else {
            alert('Please enter a comment!');
        }
    });
};

// Call the displayData and addCommentFunctionality functions
displayData();
addCommentFunctionality();
