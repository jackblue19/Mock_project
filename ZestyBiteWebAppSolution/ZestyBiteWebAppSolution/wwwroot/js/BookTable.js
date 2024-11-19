$(function () {
    var $reservationDropdown = $('#reservation, .reservationPopup');
    for (var i = 1; i <= 20; i++) {
        $reservationDropdown.append($('<option>', {
            value: i,
            text: 'Bàn ' + i
        }));
    }
    $reservationDropdown.select2();
});

$(function () {
    $.ajax({
        url: 'https://vapi.vnappmob.com/api/province/',
        method: 'GET',
        success: function (data) {
            var provinces = data.results;
            var $provinceDropdown = $('.sDeliveryAddress');
            $.each(provinces, function (index, province) {
                $provinceDropdown.append($('<option>', {
                    value: province.province_name,
                    text: province.province_name
                }));
            });
        },
        error: function (error) {
            console.error('Error fetching provinces:', error);
        }
    });
});

$(function () {
    // Hiển thị popup nếu TempData["ShowPopup"] không null
    if ('@TempData["ShowPopup"]' !== 'null') {
        $('#reservationPopup').show(); // Hiện popup
    }

    // Đóng popup khi nhấn vào nút đóng
    $('.close').on('click', function () {
        $('#reservationPopup').hide(); // Ẩn popup
    });
});
