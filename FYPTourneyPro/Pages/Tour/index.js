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

    $('#UserTournamentList').on('click', 'li i', function () {
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
        window.location.href = '/TourCategory/Index?tournamentId=' + tournamentId;
    });

    $('#UserTournamentList').on('click', '.tournament-link', function () {
        var tournamentId = $(this).parent().attr('data-id');
        window.location.href = '/TourCategory/Index?tournamentId=' + tournamentId;
    });

    // Adding a New Tournament
    $('#NewTournamentForm').submit(function (e) {
        e.preventDefault();

        var regStartDate = new Date($('#RegistrationStartDate').val());
        var regEndDate = new Date($('#RegistrationEndDate').val());

        var startDate = new Date($('#StartDate').val());
        var endDate = new Date($('#EndDate').val());

        if (regStartDate > regEndDate) {
            alert('Registration start date cannot be later than registration end date.');
            return; // Prevent form submission
        }

        if (startDate <= regEndDate) {
            alert('Start date must be later than the registration end date.');
            return; // Prevent form submission
        }

        if (endDate <= startDate) {
            alert('End date must be later than the start date.');
            return; // Prevent form submission
        }

        var tournamentData = {
            name: $('#TournamentName').val(),
            description: $('#TournamentDescription').val(),
            registrationStartDate: $('#RegistrationStartDate').val(),
            registrationEndDate: $('#RegistrationEndDate').val(),
            startDate: $('#StartDate').val(),
            endDate: $('#EndDate').val()
        };

       
        fYPTourneyPro.services.organizer.tournament.create(tournamentData).then(function (result) {
            // Redirect to the Category page with the tournamentId as a URL parameter
            window.location.href = '/TourCategory/Index?tournamentId=' + result.id;
        });
    });
});