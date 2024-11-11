const MAX_DEPTH = 2; // Maximum allowable reply depth

const comments = [
    {
        comment_id: 1,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "Absolutely delicious! Would recommend.",
        replies: []
    },
    {
        comment_id: 1,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "Absolutely delicious! Would recommend.",
        replies: []
    },
    {
        comment_id: 2,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "Absolutely phenomenal! The freshness of the fish and the delicate balance of flavors in each bite left me wanting more. Sushi heaven! 🍣",
        replies: []
    },
    {
        comment_id: 3,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "Juicy, flavorful, and cooked to perfection. A burger that hits all the right spots. Thumbs up! 🍔",
        replies: []
    },
    {
        comment_id: 3,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "The perfect blend of sweet, tangy, and spicy. This Pad Thai is a symphony of flavors that transports you straight to Thailand with each bite. Truly a culinary masterpiece!",
        replies: []
    },
    {
        comment_id: 5,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "Perfect balance of crunchy and creamy, with a zing of lemon. This Caesar Salad is a refreshing and satisfying choice for any meal. 🥗",
        replies: []
    },
    {
        comment_id: 6,
        account_name: "John Doe",
        date: "June 27, 2018 at 2:21pm",
        item_id: "Spaghetti Bolognese",
        feedback: "A slice of heaven. The crust is perfectly crispy, the sauce is rich, and the toppings are fresh. This pizza is a true Italian masterpiece. 🍕",
        replies: []
    }
];

let activeReplyBox = null;

// Recursive function to display comments and replies with depth limit
function displayComments(commentsToDisplay, container, level = 0) {
    container.innerHTML = ""; // Clear existing comments in the container

    commentsToDisplay.forEach(comment => {
        const commentItem = document.createElement("li");
        commentItem.classList.add("comment", "nested-comment");
        commentItem.style.marginLeft = `${level * 15}px`; // Indentation based on level

        // Generate comment HTML with conditional reply box
        commentItem.innerHTML = `
            <div class="vcard bio">
                <img src="../../Content/images/person_1.jpg" alt="Image placeholder">
            </div>
            <div class="comment-body">
                <h3>${comment.account_name}</h3>
                <div class="meta">${comment.date}</div>
                <p>Ate: ${comment.item_id}</p>
                <p class="feedback">Feedback: ${comment.feedback}</p>
                ${level < MAX_DEPTH ? `<p><a href="#" class="reply" data-id="${comment.comment_id}">Reply</a></p>` : ""}
                
                <!-- Hidden reply box, only shown when "Reply" is clicked -->
                <div class="reply-box" id="replyBox-${comment.comment_id}" style="display: none;">
                    <input type="text" placeholder="Write a reply..." id="replyInput-${comment.comment_id}">
                    <button onclick="submitReply(${comment.comment_id}, ${level})">Enter</button>
                </div>

                <!-- Replies list container for nested replies -->
                <ul class="replies-list" id="repliesList-${comment.comment_id}"></ul>
            </div>
        `;
        container.appendChild(commentItem);

        // Add click event listener for the reply button
        const replyButton = commentItem.querySelector('.reply');
        const replyBox = document.getElementById(`replyBox-${comment.comment_id}`);
        const replyInput = document.getElementById(`replyInput-${comment.comment_id}`);

        if (replyButton) {
            replyButton.addEventListener('click', function (event) {
                event.preventDefault();

                // Toggle the reply box visibility
                if (activeReplyBox && activeReplyBox !== replyBox) {
                    activeReplyBox.style.display = 'none';
                }
                replyBox.style.display = replyBox.style.display === 'none' ? 'block' : 'none';
                activeReplyBox = replyBox.style.display === 'block' ? replyBox : null;
                replyInput.focus();
            });

            // Add keypress event listener for the Enter key
            replyInput.addEventListener('keypress', function (e) {
                if (e.key === 'Enter') {
                    submitReply(comment.comment_id, level);
                }
            });
        }

        // Recursively display replies, incrementing the level for nested replies
        if (comment.replies.length > 0) {
            const repliesContainer = document.getElementById(`repliesList-${comment.comment_id}`);
            displayComments(comment.replies, repliesContainer, level + 1);
        }
    });

    // Close reply box when clicking outside
    document.addEventListener('mousedown', function (event) {
        if (activeReplyBox && !activeReplyBox.contains(event.target) && !event.target.classList.contains('reply')) {
            activeReplyBox.style.display = 'none';
            activeReplyBox = null;
        }
    });
}

// Temporary function to submit reply and save it in-memory with depth checking
function submitReply(commentId, level) {
    const replyInput = document.getElementById(`replyInput-${commentId}`);
    const replyText = replyInput.value.trim();

    if (replyText === "") return; // Skip empty replies
    if (level >= MAX_DEPTH) return; // Prevent reply submission beyond max depth

    const replyDate = new Date().toLocaleString();
    const newReply = {
        comment_id: Date.now(), // Temporary unique ID
        account_name: "My Name", // Replace with actual user's name
        date: replyDate,
        item_id: "",
        feedback: replyText,
        replies: [] // Allows for nested replies
    };

    // Find the parent comment in the comments array and add the reply
    const addReplyToComment = (comments, id) => {
        for (let comment of comments) {
            if (comment.comment_id === id) {
                comment.replies.push(newReply);
                return true;
            } else if (comment.replies.length > 0) {
                if (addReplyToComment(comment.replies, id)) return true;
            }
        }
        return false;
    };
    addReplyToComment(comments, commentId);

    // Clear the input field and hide the reply box
    replyInput.value = "";
    document.getElementById(`replyBox-${commentId}`).style.display = "none";

    // Re-render comments to show the new reply
    displayComments(comments, document.getElementById("commentList"));
}

// Search function
function searchComments(event) {
    const searchTerm = event.target.value.toLowerCase();
    const filteredComments = searchTerm
        ? comments.filter(comment => commentMatchesSearch(comment, searchTerm))
        : comments;

    displayComments(filteredComments, document.getElementById("commentList"));
}

function commentMatchesSearch(comment, searchTerm) {
    return comment.account_name.toLowerCase().includes(searchTerm) ||
        comment.item_id.toLowerCase().includes(searchTerm) ||
        comment.feedback.toLowerCase().includes(searchTerm) ||
        comment.replies.some(reply => commentMatchesSearch(reply, searchTerm));
}

// Initial load of comments
displayComments(comments, document.getElementById("commentList"));
document.querySelector('.res-ser-bar input[type="text"]').addEventListener('input', searchComments);
