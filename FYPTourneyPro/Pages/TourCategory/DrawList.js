
console.log("fYPTourneyPro.services.organizer.matchParticipant: ", fYPTourneyPro.services.organizer.matchParticipant)

var matchParticipants = [];
function openScoreModal(matchId) {
    document.getElementById('matchId').value = matchId;

    fYPTourneyPro.services.organizer.matchParticipant.getMatchParticipantsByMatchId(matchId)
        .then(function (participants) {
            console.log('participants: ', participants)
            matchParticipants = participants;
            //const participantsList = document.getElementById('participantsList');
            const winnerDropdown = document.getElementById('winnerId');
            //participantsList.innerHTML = '';
            //winnerDropdown.innerHTML = '';

            participants.forEach(function (participant) {
                // Add participant to winner dropdown
                const option = document.createElement('option');
                option.value = participant.userId;
                option.textContent = participant.userName; // Replace with a proper display name
                winnerDropdown.appendChild(option);
            });

            $('#player1').text(participants[0].userName);
            $('#player1').data('id', participants[0].id);
            $('#player2').text(participants[1].userName);
            $('#player2').data('id', participants[1].id);


            // Show the modal
            new bootstrap.Modal(document.getElementById('scoreModal')).show();
        })
        .catch(function (error) {
            console.error('Error fetching match participants:', error);
            alert('Failed to load match participants. Please try again.');
        });
}

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

    const participants = [player1Id, player2Id];

    matchParticipants.forEach((participant) => {

        if (participants.includes(participant.id)) {
            console.log("playerScoreData:", playerScoreData)
            var data = playerScoreData[participant.id]
            console.log("participant: ", participant.id)
            
            data.winnerId = winnerId;
            data.matchid = matchId;
            console.log("data: ", data)
            fYPTourneyPro.services.organizer.matchScore.saveScore(data)
                .then(function (result) {
                    if (result.success) {
                        alert('Score submitted successfully!');
                        // Reload the page or redirect if necessary
                        location.reload();
                    } else {
                        alert('Failed to submit score. Please try again.');
                    }
                })
                .catch(function (error) {
                    console.error('Error submitting score:', error);
                    alert('An error occurred while submitting the score.');
                });
        }
    })

    
}