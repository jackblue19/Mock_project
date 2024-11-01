// Constants
const PAGE_SIZE = 9;

// Sample Data Structure
const sampleData = [
    {
        id: 1,
        feedback: "Đồ ăn rất ngon, nhưng phục vụ hơi chậm",
        response: "Cảm ơn phản hồi của bạn. Chúng tôi sẽ cải thiện tốc độ phục vụ!",
        dateTime: "2023-06-15T14:30:00"
    },
    {
        id: 2,
        feedback: "Pizza hơi mặn",
        response: "Xin lỗi về trải nghiệm của bạn. Chúng tôi sẽ điều chỉnh lại công thức.",
        dateTime: "2023-06-14T18:45:00"
    },
    {
        id: 3,
        feedback: "Nhân viên rất thân thiện",
        response: "Cảm ơn bạn đã góp ý tích cực!",
        dateTime: "2023-06-16T12:15:00"
    },
    {
        id: 4,
        feedback: "Cần thêm các loại topping",
        response: null,
        dateTime: "2023-06-13T20:00:00"
    },
    {
        id: 5,
        feedback: "Không gian quán rất đẹp và thoáng mát",
        response: "Cảm ơn bạn đã đánh giá tích cực về không gian của chúng tôi!",
        dateTime: "2023-06-17T15:20:00"
    },
    {
        id: 6,
        feedback: "Giá hơi cao so với mặt bằng chung",
        response: "Chúng tôi luôn cố gắng đảm bảo chất lượng tương xứng với giá thành.",
        dateTime: "2023-06-18T11:30:00"
    },
    {
        id: 7,
        feedback: "Mì Ý hơi nhạt",
        response: null,
        dateTime: "2023-06-19T19:45:00"
    },
    {
        id: 8,
        feedback: "Wifi không ổn định",
        response: "Chúng tôi sẽ kiểm tra và nâng cấp hệ thống wifi.",
        dateTime: "2023-06-20T13:15:00"
    },
    {
        id: 9,
        feedback: "Salad rất tươi ngon",
        response: "Cảm ơn bạn! Chúng tôi luôn chọn nguyên liệu tươi nhất.",
        dateTime: "2023-06-21T12:00:00"
    },
    {
        id: 10,
        feedback: "Nước uống ngon nhưng hơi ít đá",
        response: "Chúng tôi sẽ lưu ý điều này với nhân viên pha chế.",
        dateTime: "2023-06-22T16:30:00"
    },
    {
        id: 11,
        feedback: "Bánh pizza đế dày quá",
        response: null,
        dateTime: "2023-06-23T18:20:00"
    },
    {
        id: 12,
        feedback: "Nhạc nền hay nhưng hơi to",
        response: "Cảm ơn góp ý của bạn, chúng tôi sẽ điều chỉnh âm lượng phù hợp hơn.",
        dateTime: "2023-06-24T20:45:00"
    },
    {
        id: 13,
        feedback: "Món tráng miệng rất ngon",
        response: "Rất vui khi bạn thích món tráng miệng của chúng tôi!",
        dateTime: "2023-06-25T21:10:00"
    },
    {
        id: 14,
        feedback: "Cần thêm menu cho trẻ em",
        response: "Chúng tôi đang phát triển menu đặc biệt cho trẻ em.",
        dateTime: "2023-06-26T14:25:00"
    },
    {
        id: 15,
        feedback: "Bàn ghế hơi cứng",
        response: null,
        dateTime: "2023-06-27T17:40:00"
    },
    {
        id: 16,
        feedback: "Phục vụ rất nhanh và chu đáo",
        response: "Cảm ơn bạn đã ghi nhận nỗ lực của đội ngũ nhân viên!",
        dateTime: "2023-06-28T19:15:00"
    },
    {
        id: 17,
        feedback: "Nên có thêm combo cho nhóm",
        response: "Chúng tôi sẽ cân nhắc thêm các combo cho nhóm đông người.",
        dateTime: "2023-06-29T12:50:00"
    },
    {
        id: 18,
        feedback: "Khuyến mãi hấp dẫn",
        response: "Cảm ơn bạn đã ủng hộ!",
        dateTime: "2023-06-30T16:05:00"
    },
    {
        id: 19,
        feedback: "Cần cải thiện việc đặt bàn online",
        response: null,
        dateTime: "2023-07-01T13:30:00"
    },
    {
        id: 20,
        feedback: "Món ăn trình bày đẹp mắt",
        response: "Cảm ơn bạn! Chúng tôi luôn chú trọng đến việc trình bày món ăn.",
        dateTime: "2023-07-02T18:00:00"
    },
    {
        id: 21,
        feedback: "Nhà vệ sinh sạch sẽ",
        response: "Cảm ơn bạn đã ghi nhận. Vệ sinh là ưu tiên hàng đầu của chúng tôi.",
        dateTime: "2023-07-03T15:45:00"
    },
    {
        id: 22,
        feedback: "Nên có thêm món chay",
        response: null,
        dateTime: "2023-07-04T11:20:00"
    },
    {
        id: 23,
        feedback: "Không gian phù hợp để làm việc",
        response: "Cảm ơn bạn đã chọn quán chúng tôi làm không gian làm việc!",
        dateTime: "2023-07-05T14:10:00"
    },
    {
        id: 24,
        feedback: "Cần thêm ổ cắm điện cho khách",
        response: "Chúng tôi sẽ bổ sung thêm ổ cắm điện để phục vụ khách hàng tốt hơn.",
        dateTime: "2023-07-06T17:25:00"
    },
    {
        id: 25,
        feedback: "Nước sốt pizza rất đặc biệt",
        response: "Cảm ơn bạn đã ửng hộ",
        dateTime: "2024-07-06T17:25:00"
    }
];

// State Management
let currentPage = 1;
const totalPages = Math.ceil(sampleData.length / PAGE_SIZE);

// Utility Functions
const formatDateTime = (dateTimeString) => {
    const dateTime = new Date(dateTimeString);
    return dateTime.toLocaleString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
};

const createFeedbackElement = (item) => {
    const itemContainer = document.createElement('div');
    itemContainer.className = 'feedback-container';
    
    const feedbackDiv = document.createElement('div');
    feedbackDiv.className = 'feedback-item';
    feedbackDiv.setAttribute('data-id', item.id);
    feedbackDiv.setAttribute('data-date', item.dateTime);
    
    feedbackDiv.innerHTML = `
        <p>${item.feedback}</p>
        <small>${formatDateTime(item.dateTime)}</small>
    `;
    
    feedbackDiv.onclick = () => showPopup(item.id, item.feedback);
    itemContainer.appendChild(feedbackDiv);

    const responseDiv = document.createElement('div');
    responseDiv.className = 'saved-response';
    responseDiv.textContent = item.response ? `Response: ${item.response}` : 'No response yet';
    itemContainer.appendChild(responseDiv);

    return itemContainer;
};

// Page Loading Functions
const loadPage = (pageNumber) => {
    currentPage = pageNumber;
    const contentContainer = document.getElementById('view');
    contentContainer.innerHTML = '';

    const start = (pageNumber - 1) * PAGE_SIZE;
    const end = Math.min(start + PAGE_SIZE, sampleData.length);

    sampleData.slice(start, end).forEach(item => {
        contentContainer.appendChild(createFeedbackElement(item));
    });

    updatePagination();
};

const updatePagination = () => {
    const paginationContainer = document.getElementById('pagination');
    paginationContainer.innerHTML = '';

    Array.from({ length: totalPages }, (_, i) => i + 1).forEach(pageNum => {
        const li = document.createElement('li');
        li.className = `page-item${pageNum === currentPage ? ' active' : ''}`;
        li.innerHTML = `<a class="page-link" href="#" onclick="loadPage(${pageNum})">${pageNum}</a>`;
        paginationContainer.appendChild(li);
    });
};

// Popup Management
const showPopup = (feedbackId, feedbackMessage) => {
    const feedbackMessageEl = document.getElementById('feedbackMessage');
    const responseTextEl = document.getElementById('responseText');
    const existingResponse = sampleData.find(item => item.id === feedbackId)?.response;

    feedbackMessageEl.innerText = feedbackMessage;
    feedbackMessageEl.setAttribute('data-id', feedbackId);
    responseTextEl.value = existingResponse || '';

    document.getElementById('popupOverlay').style.display = 'block';
    document.getElementById('infoPopup').style.display = 'block';
};

const closePopup = () => {
    document.getElementById('popupOverlay').style.display = 'none';
    document.getElementById('infoPopup').style.display = 'none';
};

// Response Handling
const sendResponse = () => {
    const feedbackId = parseInt(document.getElementById('feedbackMessage').getAttribute('data-id'));
    const responseText = document.getElementById('responseText').value;

    if (!responseText) {
        alert('Vui lòng nhập response trước khi gửi.');
        return;
    }

    const feedbackItem = sampleData.find(item => item.id === feedbackId);
    if (feedbackItem) {
        feedbackItem.response = responseText;
        alert('Response đã được lưu thành công!');
        closePopup();
        loadPage(currentPage);
    } else {
        alert('Không tìm thấy feedback tương ứng.');
    }
};

// Sorting Functions
const sortFeedbackByDate = (sortOrder) => {
    const feedbackContainer = document.getElementById('view');
    const feedbackItems = Array.from(feedbackContainer.getElementsByClassName('feedback-container'));

    feedbackItems.sort((a, b) => {
        const dateA = new Date(a.querySelector('.feedback-item').dataset.date);
        const dateB = new Date(b.querySelector('.feedback-item').dataset.date);
        return sortOrder === 'newest' ? dateB - dateA : dateA - dateB;
    });

    feedbackContainer.innerHTML = '';
    feedbackItems.forEach(item => feedbackContainer.appendChild(item));
};

// Event Listeners
document.addEventListener('DOMContentLoaded', () => {
    loadPage(1);

    document.getElementById('dateSortFilter').addEventListener('change', (e) => {
        sortFeedbackByDate(e.target.value);
    });
});