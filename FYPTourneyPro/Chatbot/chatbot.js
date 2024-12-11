$(document).ready(function () {
    $.get("/api/user/current-id", function (data) {

        let userId = null;
        let isLoggedIn = false;

        if (data.UserId) {
            const userId = data.UserId;
            isLoggedIn = true;
            console.log("API Response: ", data);

            console.log(userId, isLoggedIn);
            // Initialize the chatbot
            WebChat.default.init({
                selector: "#webchat",
                customData: { userId: userId, isLoggedIn: isLoggedIn },
                socketUrl: "http://localhost:5006",
                title: "Chatbot",
            });
        } else {
            alert("You need to log in to use the chatbot.");
        }
    }).fail(function () {
        alert("Failed to retrieve user information.");
    });
});