document.addEventListener("DOMContentLoaded", function () {
    // Handle Error    
    var elements = document.getElementsByTagName("INPUT");
    for (var i = 0; i < elements.length; i++) {
        elements[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity("This field cannot be left blank");
            }
        };
        elements[i].oninput = function (e) {
            e.target.setCustomValidity("");
        };
    }
    //                      HANDLE PSW UPPER CASE
    // Handle Caps Lock notification for password input

    var psw = document.getElementById("psw");

    // checkUpper.addEventListener("keyup", function (event) {
    //     // Check if Caps Lock is on
    //     var isCapsLock = event.getModifierState && event.getModifierState('CapsLock');

    //     if (isCapsLock) {
    //         checkUpper.setCustomValidity("Caps Lock is ON!");
    //     } else {
    //         checkUpper.setCustomValidity(""); // Clear the error message
    //     }

    //     // Update the validity state
    //     checkUpper.reportValidity(); // Show the custom validity message if any
    // });

    // // Optional: Clear the message on input
    // checkUpper.addEventListener("input", function () {
    //     checkUpper.setCustomValidity(""); // Clear the error message
    // });
    InvalidPsw(psw);
    function InvalidPsw(psw) {
        if (psw.value === '') {
            psw.setCustomValidity('Password is required!');
        } else if (psw.getModifierState && psw.getModifierState('CapsLock')) {
            psw.setCustomValidity("Capslock is ON!");
        } else {
            psw.setCustomValidity("");
        }
        return true;
    }

});

// fetch về các email sẵn có => check exist => báo lỗi =>  áp dụng cho trang Sign Up

// <input id="myInput" value="Some text..">
// <p id="text">WARNING! Caps lock is ON.</p>

// <script>
// var input = document.getElementById("myInput");
// var text = document.getElementById("text");
// input.addEventListener("keyup", function(event) {

// if (event.getModifierState("CapsLock")) {
//     text.style.display = "block";
//   } else {
//     text.style.display = "none"
//   }
// });
// </script>