const MAX_DEPTH = 2; // Maximum reply depth
        let currentPage = 1; // Current page of comments

        // Fetch comments for the current page
        function loadComments(page = 1) {
            $.ajax({
                url: `/api/feedback/feedbackpagination?pageNumber=${page}&pageSize=10`,
                method: 'GET',
                success: function (data) {
                    const commentList = document.getElementById("commentList");
                    commentList.innerHTML = ""; // Clear existing comments
                    displayComments(data, commentList); // Render comments
                    updatePaginationControls(page);
                },
                error: function () {
                    console.error("Error loading comments.");
                }
            });
        }

        // Update the state of pagination buttons
        function updatePaginationControls(page) {
            currentPage = page;

            // Enable/disable previous and next buttons
            document.getElementById("prevPage").disabled = currentPage === 1;

            // Update the page status
            document.getElementById("pageStatus").textContent = `Page ${currentPage}`;
        }

        // Event listeners for pagination controls
        document.getElementById("prevPage").addEventListener("click", () => {
            if (currentPage > 1) {
                loadComments(currentPage - 1);
            }
        });

        document.getElementById("nextPage").addEventListener("click", () => {
            loadComments(currentPage + 1);
        });

        document.getElementById("goToPage").addEventListener("click", () => {
            const pageInput = parseInt(document.getElementById("pageInput").value, 10);
            if (!isNaN(pageInput) && pageInput > 0) {
                loadComments(pageInput);
            } else {
                alert("Invalid page number.");
            }
        });

        // Recursive function to display comments and replies
        function displayComments(commentsToDisplay, container, level = 0) {
            commentsToDisplay.forEach(comment => {
                const commentItem = document.createElement("li");
                commentItem.classList.add("comment", "nested-comment");
                commentItem.style.marginLeft = `${level * 15}px`;

                commentItem.innerHTML = `
                    <div class="comment-body">
                        <h3>${comment.username}</h3>
                        <img src="${comment.ProfileImage}" alt="Profile" class="profile-img">
                        <div class="meta">${new Date(comment.DateTime).toLocaleString()}</div>
                        <p>${comment.Content}</p>
                        ${level < MAX_DEPTH ? `<button class="reply-btn" data-id="${comment.Id}" data-level="${level}">Reply</button>` : ""}
                        <div class="reply-form" id="replyBox-${comment.Id}" style="display: none;">
                            <input type="text" placeholder="Write a reply..." class="reply-input">
                            <button class="submit-reply" data-parent-id="${comment.Id}" data-level="${level}">Submit</button>
                        </div>
                        <ul class="nested-comments" id="nested-${comment.Id}"></ul>
                    </div>
        `;

                container.appendChild(commentItem);

                if (comment.Replies && comment.Replies.length > 0) {
                    const repliesContainer = document.getElementById(`nested-${comment.Id}`);
                    displayComments(comment.Replies, repliesContainer, level + 1);
                }
            });

            // Add reply functionality
            setupReplyHandlers();
        }

        // Set up event listeners for reply buttons and forms
        function setupReplyHandlers() {
            document.querySelectorAll('.reply-btn').forEach(button => {
                button.addEventListener('click', event => {
                    const replyBox = document.getElementById(`replyBox-${event.target.dataset.id}`);
                    replyBox.style.display = replyBox.style.display === 'none' ? 'block' : 'none';
                });
            });

            document.querySelectorAll('.submit-reply').forEach(button => {
                button.addEventListener('click', event => {
                    const parentId = event.target.dataset.parentId;
                    const level = parseInt(event.target.dataset.level);
                    const replyInput = event.target.previousElementSibling.value.trim();

                    if (replyInput && level < MAX_DEPTH) {
                        postReply(parentId, replyInput, level);
                    } else {
                        alert("Reply content cannot be empty or exceeds max depth.");
                    }
                });
            });
        }

        // Post a reply to the back-end
        function postReply(parentId, replyContent, level) {
            $.ajax({
                url: '/api/feedback/reply',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Content: replyContent,
                    ParentFb: parentId,
                    Username: "Current User", // Replace dynamically
                }),
                success: function (newReply) {
                    const repliesContainer = document.getElementById(`nested-${parentId}`);
                    appendFeedback(newReply, repliesContainer, level + 1); // Append reply dynamically
                },
                error: function (error) {
                    console.error("Error submitting reply:", error);
                }
            });
        }


        // Append new feedback to the comment list
        function appendFeedback(feedback, container, level) {
            const newComment = document.createElement("li");
            newComment.classList.add("comment", "nested-comment");
            newComment.style.marginLeft = `${level * 15}px`;

            newComment.innerHTML = `
                <div class="comment-body">
                    <h3>${feedback.username}</h3>
                    <div class="meta">${new Date(feedback.createdAt).toLocaleString()}</div>
                    <p>${feedback.content}</p>
                </div>
    `;

            container.appendChild(newComment);
        }

        // Initialize comments on page load
        document.addEventListener('DOMContentLoaded', () => {
            loadComments(currentPage);

            // Pagination controls
            document.getElementById('nextPage').addEventListener('click', () => {
                currentPage++;
                loadComments(currentPage);
            });

            document.getElementById('prevPage').addEventListener('click', () => {
                if (currentPage > 1) {
                    currentPage--;
                    loadComments(currentPage);
                }
            });
        });

        // Initial load
        document.addEventListener("DOMContentLoaded", () => {
            loadComments(currentPage);
        });