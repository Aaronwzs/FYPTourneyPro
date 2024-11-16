
$(function () {

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

               
                if (result) {
                    // Redirect to the DrawList page with the categoryId parameter
                    window.location.href = '/TourCategory/DrawList/' + data.categoryId;
                } else {
                    // Show an error message if the result is not valid
                    alert('Failed to generate draw. Please try again.');
                }
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
