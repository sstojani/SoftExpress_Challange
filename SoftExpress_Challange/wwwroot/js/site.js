$(document).ready(function () {
    var token = localStorage.getItem("jwtToken");


    if (token) {
        // Decode the JWT token
        var decodedToken = jwt_decode(token);
        var notificationMessage = localStorage.getItem("notificationMessage");

        console.log("Token found:", token);
        console.log("Decoded token:", decodedToken);

        if (notificationMessage) {
            showNotification(notificationMessage);
            localStorage.removeItem("notificationMessage");
        }

        var username = decodedToken['nameid'];  // Check for 'nameid' in your token

        // Display the username in the navbar
        $('#username-display').text(username);

        // Hide login and show the username dropdown with logout
        $('#login-link').hide();
        $('#user-dropdown').show();
        $('#statistika-link').show();
        $('#report-link').show();

    } else {
        $('#username-placeholder').text('');
        $('#login-link').show();
        $('#user-dropdown').hide();
        $('#statistika-link').hide();
        $('#report-link').hide();

    }
});

function showNotification(message) {
    var notification = $('#notification');
    notification.text(message);
    notification.fadeIn();

    // Hide the notification after 3 seconds
    setTimeout(function () {
        notification.fadeOut();
    }, 3000);
}

// Logout function
function logout() {

    localStorage.setItem("notificationMessage", "Logged out successfully!");

    localStorage.removeItem("jwtToken"); // Remove token from localStorage
    window.location.href = "/"; // Redirect to home after logout
}

function redirectToCreate() {
    // Get the JWT token from localStorage
    var token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("You are not logged in. Please log in to continue.");
        return;
    }

    // Decode the token using jwt_decode
    var decodedToken = jwt_decode(token);

    // Extract userId and username (adjust claim keys based on your token structure)
    var userId = decodedToken['unique_name']; // Typically 'sub' represents the user ID
    var username = decodedToken['nameid']; // Adjust to match your token claims for username

    if (!userId || !username) {
        alert("Invalid token data. Please log in again.");
        return;
    }

    // Redirect to the Create view, passing the userId and username as query parameters
    window.location.href = `/Actions/Create?username=${encodeURIComponent(username)}&userId=${encodeURIComponent(userId)}`;
}
