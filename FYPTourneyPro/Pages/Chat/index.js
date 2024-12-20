// Toggle visibility of the Create Group Chat Form
const createChatButton = document.getElementById("createChatButton");
const createGroupChatForm = document.getElementById("createGroupChatForm");

createChatButton.addEventListener("click", () => {
    if (createGroupChatForm.style.display === "none") {
        createGroupChatForm.style.display = "block";
        createChatButton.textContent = "Cancel Group Creation";
        createChatButton.classList.add("cancel");
    } else {
        createGroupChatForm.style.display = "none";
        createChatButton.textContent = "Create Group Chat";
        createChatButton.classList.remove("cancel");
    }
});

// Dynamically Load Participants (Simulated List)

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

// Initial Load of Participants
loadParticipants();

// Add Search Functionality
const searchInput = document.getElementById("searchParticipants");
searchInput.addEventListener("input", (event) => {
    loadParticipants(event.target.value);
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
            // If the chat room already exists
            alert(`Chat room already exists with ID: ${result.id}`);
        } else {
            // If a new chat room is created
            alert(`New chat room created with ID: ${result.id}`);
        }
    });

        console.log("Group Created:", { groupName, selectedParticipants });
        alert(
            `Group "${groupName}" created with participants: ${selectedParticipants.join(
                ", "
            )}`
        );

        // Reset Form and Close
        document.getElementById("createGroupForm").reset();
        createGroupChatForm.style.display = "none";
        createChatButton.textContent = "Create Group Chat";
    });