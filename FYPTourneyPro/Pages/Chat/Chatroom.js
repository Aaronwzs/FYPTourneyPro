// Get the chatRoomId from the URL
const urlParams = new URLSearchParams(window.location.search);
const chatRoomId = urlParams.get("chatRoomId");
const chatContainer = document.getElementById('chatContainer');
const membersList = document.getElementById('membersList');
const chatRoomTitle = document.getElementById('chatRoomTitle');
const messageInput = document.getElementById('messageInput');
const sendMessageButton = document.getElementById('sendMessageButton');


//open signalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub") // Your SignalR hub endpoint
    .build();

connection.start()
    .then(() => {
        console.log("Connected to SignalR hub");
        // Optionally rejoin any room you need to
        connection.invoke("JoinRoom", chatRoomId);
    })
    .catch((error) => {
        console.error("Failed to connect:", error);
    });

//Receive message real time
connection.on("ReceiveMessage", (username, message, creationTime) => {
    // Create a new message element
    const messageDiv = document.createElement("div");
    messageDiv.classList.add("message");

    // Format the message content
    const formattedTime = new Date(creationTime).toLocaleString();
    messageDiv.innerHTML = `
        <p><strong>${username}:</strong> ${message}</p>
        <p class="timestamp">${formattedTime}</p>
    `;

    // Append the message to the chat container
    const chatContainer = document.getElementById("chatContainer");
    chatContainer.appendChild(messageDiv);

    // Auto-scroll to the bottom of the chat container
    chatContainer.scrollTop = chatContainer.scrollHeight;
});

    //leave room when clsose window
window.onbeforeunload = () => {
    connection.invoke("LeaveRoom", chatRoomId);
    console.log(`left Room ${ chatRoomId }`);
};

async function loadChatRoomDetails() {
    if (chatRoomId) {
        fYPTourneyPro.services.chat.chat.getChatMessages(chatRoomId).then((messages) => {

            chatRoomTitle.textContent = messages[0].chatRoomName;


            messages.forEach((message) => {
                const messageDiv = document.createElement("div");
                messageDiv.classList.add("message");
                messageDiv.innerHTML = `
                <p><strong>${message.username}</strong>: ${message.content}</p>
                <p class="timestamp">${new Date(message.creationTime).toLocaleString()}</p>
            `;
                chatContainer.append(messageDiv);
            });

            //scroll to latest
            chatContainer.scrollTop = chatContainer.scrollHeight;
        }).catch((error) => {
            console.error("Error loading chat messages:", error);
        });

        fYPTourneyPro.services.chat.chat.getUsersInChatroom(chatRoomId).then((users) => {
            users.forEach((user) => {
                const listItem = document.createElement('li');
                listItem.className = "member-item"; // Add a class for styling
                listItem.innerHTML = `
        <span class="member-name">${user.username}</span>
    `;
                membersList.appendChild(listItem);
            });
        });
    } else {
        console.error("Chat Room ID not found in the URL");
    }
}


async function sendMessage() {
    const messageContent = messageInput.value.trim();
    if (!messageContent) {
        alert("Message cannot be empty!");
        return;
    }
    try {
        fYPTourneyPro.services.chat.chat.createChatMessages(chatRoomId, messageContent).then((newMessage) => {


            try {
                console.log("Sending message:", newMessage);
                // Send the message to the SignalR server
                var messageData = {
                    chatRoomId: chatRoomId,
                    content: messageContent,
                    creationTime: newMessage.creationTime,
                    creatorId: newMessage.creatorId,
                    username: newMessage.username
                };


                connection.invoke("SendMessage", messageData);
                messageInput.value = ""; // Clear input after sending
            } catch (error) {
                console.error("Failed to send message:", error);
            }
        });
    } catch (error) {
        console.error("Failed to send message", error);
    }
}


// Event Listeners
sendMessageButton.addEventListener('click', sendMessage);

messageInput.addEventListener("keypress", (event) => {  
    if (event.key === "Enter") {
        event.preventDefault(); // Prevent the default Enter behavior (e.g., form submission)
        sendMessageButton.click(); // Trigger the send button's click event
    }
});

// Call this after loading messages
loadChatRoomDetails()
