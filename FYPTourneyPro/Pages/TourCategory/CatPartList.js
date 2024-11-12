
$(function () {
    // Redirect when clicking a category participant row if needed
    $('#ParticipantList').on('click', 'tr', function () {
        var participantId = $(this).attr('data-id');
        // Redirect or perform action with participantId
        window.location.href = '/Participant/Details?participantId=' + participantId;
    });
});