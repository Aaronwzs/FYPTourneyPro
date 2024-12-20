﻿$(document).ready(function () {

    $.get("/api/user/current-id", function (data) {
        let isLoggedIn = false;

        if (data.userId) {
            console.log(`sup ${data.userId}`);
            const userId = data.userId;
            isLoggedIn = true;
            console.log("API Response: ", data);
            console.log(WebChat);

            console.log(userId, isLoggedIn);
            // Initialize the chatbot
            window.WebChat.default({
                selector: "#webchat",
                initPayload: '/greet',
                customData: { userId: userId, isLoggedIn: isLoggedIn },
                socketUrl: "http://localhost:5005",
                title: "Chatbot",
            });
        } else {
            alert("You need to log in to use the chatbot.");
        }
    }).fail(function () {
        alert("Failed to retrieve user information.");
    });
});