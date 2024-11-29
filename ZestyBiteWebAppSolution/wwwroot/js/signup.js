var currentTab = 0; // Current tab is set to be the first tab (0)
showTab(currentTab);

function showTab(n) {
    // Hide all tabs
    var tabs = document.getElementsByClassName("tab");
    tabs[n].style.display = "block";
    // Display the correct navigation buttons
    if (n == 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n == (tabs.length - 1)) {
        document.getElementById("nextBtn").style.display = "none";
        document.getElementById("submit").style.display = "inline"; // Show the submit button on the last tab
    } else {
        document.getElementById("nextBtn").style.display = "inline";
        document.getElementById("submit").style.display = "none";
    }
    // Fix the step indicator
    fixStepIndicator(n);
}

function nextPrev(n) {
    var tabs = document.getElementsByClassName("tab");
    // Hide the current tab
    tabs[currentTab].style.display = "none";
    // Increase or decrease the current tab by 1
    currentTab = currentTab + n;
    // If you have reached the end of the form... 
    if (currentTab >= tabs.length) {
        document.getElementById("signupForm").submit();
        return false;
    }
    showTab(currentTab);
}

function fixStepIndicator(n) {
    var steps = document.getElementsByClassName("step");
    for (var i = 0; i < steps.length; i++) {
        steps[i].classList.remove("active");
    }
    steps[n].classList.add("active");
}