﻿// Toggle visibility of the Create Group Chat Form
const createChatButton = document.getElementById("createChatButton");
const createGroupChatForm = document.getElementById("createGroupChatForm");

createChatButton.addEventListener("click", () => {
    if (createGroupChatForm.style.display === "none") {
        createGroupChatForm.style.display = "block";
        createChatButton.textContent = "Cancel Chat Creation";
        createChatButton.classList.add("cancel");
        loadParticipants();
    } else {
        createGroupChatForm.style.display = "none";
        createChatButton.textContent = "Create Chat";
        createChatButton.classList.remove("cancel");
    }
});



const participantsContainer = document.getElementById("participantsContainer");

function loadParticipants(searchTerm = "") {
    participantsContainer.innerHTML = ""; // Clear existing list
    

    fYPTourneyPro.services.chat.chat.getAllUsers().then((participants) => {


        const filteredParticipants = participants.filter((participantName) => {

            return participantName.userName.toLowerCase().includes(searchTerm.toLowerCase());
        });



        filteredParticipants.forEach((participant) => {
            const checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.value = participant.id;
            checkbox.id = `participant-${participant.id}`;
            checkbox.className = "participant";

            const label = document.createElement("label");
            label.htmlFor = `participant-${participant.id}`;
            label.textContent = participant.userName;

            const div = document.createElement("div");
            div.appendChild(checkbox);
            div.appendChild(label);

            participantsContainer.appendChild(div);
    });

   
    });
}

// Add Search Functionality
const searchInput = document.getElementById("searchParticipants");
searchInput.addEventListener("input", (event) => {
    loadParticipants(event.target.value);
});

//Select only 1 
document.getElementById('participantsContainer').addEventListener('change', function () {
    const selectedParticipants = Array.from(
        document.querySelectorAll("#participantsContainer input:checked")
    ).map((checkbox) => checkbox.value);
    const groupNameField = document.getElementById('groupName');

    if (selectedParticipants.length === 1) {
        // Disable the group name field if only one participant is selected
        groupNameField.disabled = true;
        groupNameField.value = ''; // Clear the group name field
    } else {
        // Enable the group name field if more than one participant is selected
        groupNameField.disabled = false;
    }
});
// Handle Form Submission
document.getElementById("createGroupForm").addEventListener("submit", (event) => {
        event.preventDefault(); // Prevent page refresh

        // Gather Group Name and Selected Participants
        const groupName = document.getElementById("groupName").value;
        const selectedParticipants = Array.from(
            document.querySelectorAll("#participantsContainer input:checked")
        ).map((checkbox) => checkbox.value);

        if (!groupName || selectedParticipants.length === 0) {
            alert("Please enter a group name and select at least one participant.");
            return;
    }

    // Simulate API Call
    fYPTourneyPro.services.chat.chat.createGroupChat(groupName, selectedParticipants).then((result) => {
        if (result.isDuplicate) {
            console.log(result);
            // If the chat room already exists
            alert(`Chat room already exists with ID: ${result.id}`);
        } else {
            // If a new chat room is created
            alert(`New chat room created with ID: ${result.id}`);
        }
    });

        // Reset Form and Close
        document.getElementById("createGroupForm").reset();
        createGroupChatForm.style.display = "none";
        createChatButton.textContent = "Create Group Chat";
});


//Chatroom rows
function loadChatRooms() {
    fYPTourneyPro.services.chat.chat.getUserChatRooms().then((chatRooms) => {

        console.log("Original chatRooms:", chatRooms); // Debug: Log the unsorted array

        // Sort chat rooms by last message creation time (newest first)
        chatRooms.sort((a, b) => {
            const timeA = new Date(a.creationTime).getTime(); // Convert to timestamp
            const timeB = new Date(b.creationTime).getTime();
            return timeB - timeA; // Descending order
        });

        console.log("Sorted chatRooms:", chatRooms); // Debug: Log the sorted array
        chatRooms.forEach((chatRoom) => {

            const div = document.createElement("div");
            div.classList.add("chatroom");
            div.value = chatRoom.chatRoomId;

            const title = document.createElement("h3")
            title.textContent = `${chatRoom.name}`;

            const content = document.createElement("p")

            if (chatRoom.username) {
                content.textContent = `${chatRoom.username} - `;
                content.textContent += `${chatRoom.lastMessage}   `;

                const formattedCreationTime = new Date(chatRoom.creationTime).toLocaleString();

                const creationTime = document.createElement("span");
                creationTime.textContent = `Created on: ${formattedCreationTime}`;
                creationTime.classList.add("creation-time"); // Add a CSS class
                content.append(creationTime);
            }

            div.addEventListener("click", () => {
                console.log(`Navigating to chat room: ${chatRoom.name}`);
                // Redirect to chat page with chatRoomId as a query parameter
                window.location.href = `/Chat/Chatroom?chatRoomId=${chatRoom.chatRoomId}`;
            });

            div.append(title)
            div.append(content)

            document.getElementById("chatRoomsList").append(div);
        });

    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadChatRooms(); // Load chat rooms when the page is ready
});