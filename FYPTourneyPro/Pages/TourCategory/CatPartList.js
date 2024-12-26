
$(function () {


    //// Initialize sorting for the Start Time column
    //$('.sort').on('click', function () {
    //    const sortOrder = $(this).data('sort');
    //    const rows = $('#matchesTable tbody tr').get();

    //    rows.sort(function (a, b) {
    //        const keyA = $(a).children('td').eq(3).text(); // Start Time column index
    //        const keyB = $(b).children('td').eq(3).text();

    //        // Convert date strings to Date objects for comparison
    //        const dateA = new Date(keyA);
    //        const dateB = new Date(keyB);

    //        return sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    //    });

    //    $.each(rows, function (index, row) {
    //        $('#matchesTable tbody').append(row);
    //    });
    //});

    







    console.log("Initializing...");
    const generateDrawBtn = $('#generateDrawBtn');
    const matchListContainer = $('#matchListContainer');
    const scoreModal = $('#scoreModal');
    const modal = new bootstrap.Modal(scoreModal[0]);

    console.log(fYPTourneyPro.services.organizer);

    // Redirect when clicking a category participant row if needed
    $('#ParticipantList').on('click', 'tr', function () {
        var participantId = $(this).attr('data-id');
        // Redirect or perform action with participantId
        window.location.href = '/Participant/Details?participantId=' + participantId;
    });


    $('#generateDrawBtn').on('click', async function () {

        // Get the categoryId from the hidden input

        var data = {
            categoryId: $('#categoryId').val()
        }

        // Call the generateDraw service and handle the response with .then()
        fYPTourneyPro.services.organizer.matchParticipant.generateDraw(data)
            .then(function (result) {
                // Check if the result is valid and has an ID

                console.log("result:", result);

               
                //if (result) {
                //    // Redirect to the DrawList page with the categoryId parameter
                //    window.location.href = '/TourCategory/DrawList/' + data.categoryId;
                //} else {
                //    // Show an error message if the result is not valid
                //    alert('Failed to generate draw. Please try again.');
                //}
            })
            .catch(function (error) {
                // Handle any errors that occurred during the API call
                console.error('Error generating draw:', error);
                alert('An error occurred while generating the draw.');
            });


            // Make the API call to generate the draw
            //const response = await fetch(`/TourCategory/GenerateDraw/${categoryId}`, {
            //    method: 'POST',
            //    headers: {
            //        'Content-Type': 'application/json'
            //    }
            //});

            //if (response.ok) {
            //    // Parse the response
            //    const matchData = await response.json();

            //    // Clear the previous match list, if any
            //    $('#matchListContainer').html('');

            //    // Dynamically build and display the match list
            //    var matchListHtml = "<h3>Generated Matches</h3><ul>";

            //    matchData.matches.forEach(match => {
            //        matchListHtml += `<li><strong>Match ID: ${match.id}</strong> - Round: ${match.round}<br />`;

            //        // Loop through participants and display them
            //        match.matchParticipants.forEach(participant => {
            //            matchListHtml += `Player ID: ${participant.userId}<br />`;
            //        });

            //        matchListHtml += "</li><hr />";
            //    });

            //    matchListHtml += "</ul>";

            //    // Insert the generated match list into the matchListContainer
            //    $('#matchListContainer').html(matchListHtml);
            //} else {
            //    // Handle error response from the API call
            //    alert('Failed to generate draw. Please try again.');
            //}




    });


    
});

console.log("fYPTourneyPro.services.organizer.matchParticipant: ", fYPTourneyPro.services.organizer.matchParticipant)

var matchParticipants = [];  // empty list
function openScoreModal(matchId) {
    document.getElementById('matchId').value = matchId;

    fYPTourneyPro.services.organizer.matchParticipant.getMatchParticipantsByMatchId(matchId) //  fetch the match participants
        .then(function (participants) {
            console.log('participants: ', participants)


            //put 2 participants into this matchParticipant list
            matchParticipants = participants;

            


            //const participantsList = document.getElementById('participantsList');
            const winnerDropdown = document.getElementById('winnerId');
            //participantsList.innerHTML = '';
            //winnerDropdown.innerHTML = '';

            participants.forEach(function (participant) {
                // Add participant to winner dropdown
                const option = document.createElement('option');
                option.value = participant.userId;
                //option.textContent = participant.userName; // Replace with a proper display name
                option.textContent = participant.fullName;
                winnerDropdown.appendChild(option);
            });
            //get 2 data in list
            //cater for doubles
            // option.value = participant.pairId;

            var mpFullName = participants[0].fullName;
            console.log('mpFullName: ', mpFullName)



            $('#player1').text(participants[0].fullName);
            $('#player1').data('id', participants[0].id);
            $('#player2').text(participants[1].fullName);
            $('#player2').data('id', participants[1].id);



            //Cater for pairs
            const participantGroups = {};
            participants.forEach(function (participant) {
                if (!participantGroups[participant.pairId]) {
                    participantGroups[participant.pairId] = [];
                }
                participantGroups[participant.pairId].push(participant);
            });

            //// Populate winner dropdown with pair names
            //Object.values(participantGroups).forEach(function (pairParticipants) {
            //    const option = document.createElement('option');

            //    // Create pair name by combining participant names
            //    const pairName = pairParticipants.map(p => p.userName).join(' & ');

            //    // Use first participant's pairId as the value
            //    option.value = pairParticipants[0].pairId;
            //    option.textContent = pairName;
            //    winnerDropdown.appendChild(option);
            //});

            //// Set player labels (works for both singles and doubles)
            //if (participants.length >= 2) {
            //    const firstPair = participantGroups[participants[0].pairId];
            //    const secondPair = participantGroups[participants[1].pairId];

            //    // Display pair names
            //    $('#player1').text(firstPair.map(p => p.userName).join(' & '));
            //    $('#player1').data('pairId', participants[0].pairId);

            //    $('#player2').text(secondPair.map(p => p.userName).join(' & '));
            //    $('#player2').data('pairId', participants[1].pairId);
            //}




            // Show the modal
            //new bootstrap.Modal(document.getElementById('scoreModal')).show();
            $('#scoreModal').show();
        })
        .catch(function (error) {
            console.error('Error fetching match participants:', error);
            alert('Failed to load match participants. Please try again.');
        });
}

// function closeMo() {
//    modal.hide();
//    $('#scoreForm')[0].reset();
//};

function submitScore() {
    // todo: validation

    console.log("matchParticipants: ", matchParticipants)

    //fYPTourneyPro.services.organizer.matchParticipant.matchParticipants

    const matchId = document.getElementById('matchId').value;
    var player1Id = $('#player1').data('id');
    const player1_set1 = document.getElementById('player1_set1').value;
    const player1_set2 = document.getElementById('player1_set2').value;
    const player1_set3 = document.getElementById('player1_set3').value;
    const player2_set1 = document.getElementById('player2_set1').value;
    var player2Id = $('#player2').data('id');
    const player2_set2 = document.getElementById('player2_set2').value;
    const player2_set3 = document.getElementById('player2_set3').value;
    const winnerId = document.getElementById('winnerId').value;

    //const scores = [
    //    { Sets: 1, Points: parseInt(set1), MatchParticipantId: matchId },
    //    { Sets: 2, Points: parseInt(set2), MatchParticipantId: matchId },
    //    { Sets: 3, Points: parseInt(set3), MatchParticipantId: matchId }
    //];

    //const data = {
    //    matchId: matchId,
    //    scores: scores,
    //    winnerId: winnerId
    //};

    const playerScoreData = {
        [player1Id]: {
            Set1Score: player1_set1,
            Set2Score: player1_set2,
            Set3Score: player1_set3
        },
        [player2Id]: {
            Set1Score: player2_set1,
            Set2Score: player2_set2,
            Set3Score: player2_set3
        }
    }

    const players = [player1Id, player2Id];

    matchParticipants.forEach((mParticipants) => {

        if (players.includes(mParticipants.id)) {
            console.log("playerScoreData:", playerScoreData)
            var data = playerScoreData[mParticipants.id]
            console.log("participant: ", mParticipants.id)

            data.winnerId = winnerId;
            data.matchId = matchId;
            data.matchParticipantId = mParticipants.id

            console.log("data: ", data)
            fYPTourneyPro.services.organizer.matchScore.saveScore(data)
                .then(function (result) {
                    console.log("result: ", result)
                    //if (result.success) { 
                    //    alert('Score submitted successfully!');
                    //    // Reload the page or redirect if necessary
                    //    location.reload();
                    //} else {
                    //    alert('Failed to submit score. Please try again.');
                    //}
                })
                .catch(function (error) {
                    console.error('Error submitting score:', error);
                    alert('An error occurred while submitting the score.');
                });
        }
    })


}


function closeModal() {
    //const modalElement = document.getElementById('scoreModal');
    //const modal = new bootstrap.Modal(modalElement);
    //modal.hide();  // This will close the modal programmatically
    $('#scoreModal').hide();
    $('#scoreForm')[0].reset();
}
