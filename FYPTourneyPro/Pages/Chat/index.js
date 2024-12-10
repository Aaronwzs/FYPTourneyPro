const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

let currentChatRoomId = null;
// Receive messages from the server
connection.on("ReceiveMessage", function (userId, message, creationTime) {
    console.log(`Message received from user ${userId}: ${message} at ${creationTime}`);
    const msg = `${userId}: ${message} at ${creationTime}`;
    const li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});


// Start the connection
connection.start().then(function () {
    console.log("Connected to SignalR Hub!");
    loadChatRooms();
    fYPTourneyPro.services.chat.chat.getUserChatRooms().then((chatRooms) => {
        chatRooms.forEach(room => {
            connection.invoke("JoinRoom", room.chatRoomId)
                .then(() => {
                    console.log(`Joined room ${room.chatRoomId}`);
                })
                .catch(function (err) {
                    console.error("Error joining room:", err.toString());
                });
        });
    }).catch(function (err) {
        console.error("Error connecting to SignalR Hub:", err.toString());
    });
});

//CREATE GROUP
    document.getElementById("createGroupForm").addEventListener("submit", async function (event) {
        event.preventDefault();

        const participantsContainer = document.getElementById("participantsContainer");
        const checkboxes = participantsContainer.querySelectorAll("input[type='checkbox']:checked");
        console.log(checkboxes);

        // Gather selected user IDs
        const participantIds = [];
        checkboxes.forEach(checkbox => {
            participantIds.push(checkbox.value);
        });
        console.log("Selected Participant IDs:", participantIds);
        // Validate the input
        const groupName = document.getElementById("groupName").value;
        if (!groupName || participantIds.length === 0) {
            alert("Please enter a group name and select at least one participant.");
            return;
        }

        fYPTourneyPro.services.chat.chat.createGroupChat(groupName, participantIds).then((result) => {
            connection.invoke("JoinRoom", result)
                .then(() => {
                    console.log(`Joined room ${result}`);
                    alert(`Group chat created successfully with ID: ${result}`);
                })
                .catch(function (err) {
                    console.error("Error joining room:", err.toString());
                });
        });

    });


    function joinChatRoom(chatRoomId) {
        // Join the room using SignalR once it's been created
        connection.start().then(function () {
            connection.invoke("JoinRoom", chatRoomId)
                .then(() => {
                    console.log(`Joined room ${chatRoomId}`);
                    loadChatRooms();

                })
                .catch(function (err) {
                    console.error("Error joining room:", err.toString());
                });
        }).catch(function (err) {
            console.error("Error connecting to SignalR Hub:", err.toString());
        });
    }

    // LOAD ALL PARTCIPANTS TO BE INCLUDED INTO GROUP CHAT
    async function loadGroupParticipants() {
        try {
            const participantsContainer = document.getElementById("participantsContainer");

            // Call the service to get all users
            fYPTourneyPro.services.chat.chat.getAllUsers().then((users) => {
                users.forEach(user => {
                    // Create a div for each checkbox and label
                    const userDiv = document.createElement("div");

                    // Create the checkbox
                    const checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.value = user.id;
                    checkbox.id = `participant_${user.id}`;  // Assign unique ID for each checkbox
                    // Create the label for the checkbox
                    const label = document.createElement("label");
                    label.htmlFor = `participant_${user.id}`;
                    label.textContent = user.userName;

                    // Add the checkbox and label to the div
                    userDiv.appendChild(checkbox);
                    userDiv.appendChild(label);

                    // Append the div to the container
                    participantsContainer.appendChild(userDiv);
                });
            });
        } catch (error) {
            console.error("Error loading participants:", error);
        }
    }

//Load all individual participants(chats)
//async function loadParticipants() {
//    try {
//        const participantsContainer = document.getElementById("participantsContainer");

//        fYPTourneyPro.services.chat.chat.getAllUsers().then((users) => {
//            users.forEach(user => {
//                const userDiv = document.createElement("div");
//                userDiv.value = user.id;
//                userDiv.textContent = user.userName;
//                userDiv.addEventListener("click", () => {
//                    openChatRoom();
//                })
//            }

//        });
//    } catch (error) {
//        console.error("Error loading participants:", error);
//}

    // Call loadParticipants on page load
    document.addEventListener("DOMContentLoaded", loadGroupParticipants);


    // LOAD ALL THE CHAT ROOMS INVOLVED BY THE CURRENT USER
    async function loadChatRooms() {
        try {
            const chatRoomsContainer = document.getElementById("chatRoomsList");
            // Make an API call to get the user's chat rooms
            fYPTourneyPro.services.chat.chat.getUserChatRooms().then((chatRooms) => {

                chatRooms.forEach(room => {
                    const roomElement = document.createElement("div");
                    roomElement.classList.add("chat-room");
                    roomElement.textContent = `${room.name} - Last message:${room.lastMessage}`;
                    roomElement.addEventListener("click", () => {
                        openChatRoom(room.chatRoomId, room.name);
                    });
                    chatRoomsContainer.appendChild(roomElement);
                });
            });
        } catch (error) {
            console.error("Error loading chat rooms:", error);
        }
    }


    // OPEN THE CHATROOM SELECTED
async function openChatRoom(chatRoomId, chatRoomName) {

    // Update the UI to show the chat room name and make the messages section visible
    document.getElementById("chatRoomTitle").textContent = chatRoomName;
    document.getElementById("chatMessagesContainer").style.display = "block";

    currentChatRoomId = chatRoomId;
    // Load the chat messages
    loadChatMessages(chatRoomId);
}

// Function to load the messages of the selected chat room
async function loadChatMessages(chatRoomId) {
    try {
        fYPTourneyPro.services.chat.chat.getChatMessages(chatRoomId).then((messages) => {
            const messagesContainer = document.getElementById("messagesList");

            // Clear the existing messages
            messagesContainer.innerHTML = "";

            // Render messages
            messages.forEach(message => {
                const messageDiv = document.createElement("div");
                messageDiv.classList.add("message");
                messageDiv.textContent = `${message.creatorId}: ${message.content} at ${message.creationTime}`;
                messagesContainer.appendChild(messageDiv);
            });
        });
      

    } catch (error) {
        console.error("Error loading chat messages:", error);
    }
}


//CHAT FUNCTION//////////////////
document.getElementById("sendMessageButton").addEventListener("click", async function (event) {
    event.preventDefault();  // Prevent any default form submission behavior

    // Get the message content from the input field
    const messageInput = document.getElementById("messageInput");
    const content = messageInput.value.trim();

    if (!content) {
        alert("Please enter a message.");
        return;
    }

    // Prepare message data to be sent
    const messageData = {
        chatRoomId: currentChatRoomId,
        content: content,
    };

    try {
        // Call the backend service to save the message
        var chatMessage = await fYPTourneyPro.services.chat.chat.createChatMessages(messageData);
        console.log(`Attempting to send message: content = ${content}, currentChatRoomId = ${currentChatRoomId}, creatorId = ${chatMessage.creatorId}`);
        // Optionally, invoke SignalR to broadcast the message to other users
        connection.invoke("SendMessage", currentChatRoomId, chatMessage.creatorId, content, chatMessage.creationTime)
            .then(() => {
                console.log("Message broadcasted successfully.");
            })
            .catch(function (err) {
                console.error("Error broadcasting message:", err.toString());
            });

        console.log("Message saved in the backend.");

        // Clear the input field after the message is sent
        messageInput.value = '';
    } catch (error) {
        console.error("Error saving message to backend:", error);
    }
});