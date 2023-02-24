$(document).ready(function () {
    $("#popup-overlay").fadeIn();

    // Hide the popup overlay after 1.5 seconds
    setTimeout(function () {
        $("#popup-overlay").fadeOut();
    }, 1500);
});