$(function () {
    // Deleting a Tournament
    $('#TournamentList').on('click', 'li i', function () {
        var $li = $(this).parent();
        var id = $li.attr('data-id');

        fYPTourneyPro.services.organizer.tournament.delete(id).then(function () {
            $li.remove();
            abp.notify.info('Deleted the tournament.');
        });
    });

    // Handle tournament name click ( edit )
    $('#TournamentList').on('click', '.tournament-link', function () {
        var tournamentId = $(this).parent().attr('data-id');
        window.location.href = '/Category/Index?tournamentId=' + tournamentId;
    });

    // Adding a New Tournament
    $('#NewTournamentForm').submit(function (e) {
        e.preventDefault();

        var tournamentData = {
            name: $('#TournamentName').val(),
            description: $('#TournamentDescription').val(),
            registrationStartDate: $('#RegistrationStartDate').val(),
            registrationEndDate: $('#RegistrationEndDate').val(),
            startDate: $('#StartDate').val(),
            endDate: $('#EndDate').val()
        };

        //fYPTourneyPro.services.organizer.tournament.create(tournamentData).then(function (result) {
        //    $('<li data-id="' + result.id + '">')
        //        .html('<i class="fa fa-trash-o"></i> ' + result.name)
        //        .appendTo($('#TournamentList'));


        //}
        //);
        fYPTourneyPro.services.organizer.tournament.create(tournamentData).then(function (result) {
            // Redirect to the Category page with the tournamentId as a URL parameter
            window.location.href = '/Category/Index?tournamentId=' + result.id;
        });
    });
});