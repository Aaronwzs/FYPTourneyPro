$(function () {

    // Collapse and uncollapse tournament creation form
    $('#createTournamentButton').on('click', function () {
        const tournamentForm = document.getElementById('NewTournamentForm');
        if (tournamentForm.style.display === 'none' || tournamentForm.style.display === '') {
            tournamentForm.style.display = 'block'; // Show the form
        } else {
            tournamentForm.style.display = 'none'; // Hide the form
        }
    });


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
            endDate: $('#EndDate').val(),
            hotlineNum: $('#HotlineNum').val(),
            isMalaysian: $('#IsMalaysian').prop('checked'),  // Checkbox value
            rulesAndRegulations: $('#RulesAndRegulations').val()
        };

       
        fYPTourneyPro.services.organizer.tournament.create(tournamentData).then(function (result) {
            // Redirect to the Category page with the tournamentId as a URL parameter
            window.location.href = '/TourCategory/Index?tournamentId=' + result.id;
        });
    });
});

$(function () {

    // Edit Tournament Button - Show the modal with pre-filled data
    $('#TournamentList').on('click', '.edit-tournament', function () {
        var $li = $(this).parent();
        var tournamentId = $li.attr('data-id');

        // Fetch tournament data from the backend (use API or your own method to get the tournament details)
        fYPTourneyPro.services.organizer.tournament.get(tournamentId).then(function (result) {
            // Populate the modal with tournament data
            $('#EditTournamentId').val(result.id);
            $('#EditTournamentName').val(result.name);
            $('#EditTournamentDescription').val(result.description);
            $('#EditRegistrationStartDate').val(result.registrationStartDate.split('T')[0]);
            $('#EditRegistrationEndDate').val(result.registrationEndDate.split('T')[0]);
            $('#EditStartDate').val(result.startDate.split('T')[0]);
            $('#EditEndDate').val(result.endDate.split('T')[0]);
            $('#EditHotlineNum').val(result.hotlineNum);
            $('#EditIsMalaysian').prop('checked', result.isMalaysian);
            $('#EditRulesAndRegulations').val(result.rulesAndRegulations);

            // Show the modal
            $('#EditTournamentModal').modal('show');
        });
    });

    // Remove Tournament Button - Delete tournament
    //$('#TournamentList').on('click', '.remove-tournament', function () {
    //    var $li = $(this).parent();
    //    var tournamentId = $li.attr('data-id');

    //    // Confirm before deletion
    //    if (confirm('Are you sure you want to delete this tournament?')) {
    //        // Call the delete API to remove the tournament
    //        fYPTourneyPro.services.organizer.tournament.delete(id).then(function () {
    //            // Remove the tournament from the list in the UI
    //            $li.remove();
    //            abp.notify.info('Tournament removed successfully.');
    //        }).catch(function (error) {
    //            abp.notify.error('Error deleting the tournament.');
    //        });
    //    }
    //});

    // Handle the Edit Tournament Form Submission
    $('#EditTournamentForm').submit(function (e) {
        e.preventDefault();

        var tournamentData = {
            id: $('#EditTournamentId').val(),
            name: $('#EditTournamentName').val(),
            description: $('#EditTournamentDescription').val(),
            registrationStartDate: $('#EditRegistrationStartDate').val(),
            registrationEndDate: $('#EditRegistrationEndDate').val(),
            startDate: $('#EditStartDate').val(),
            endDate: $('#EditEndDate').val(),
            hotlineNum: $('#EditHotlineNum').val(),
            isMalaysian: $('#EditIsMalaysian').prop('checked'),
            rulesAndRegulations: $('#EditRulesAndRegulations').val()
        };

        // Send the updated tournament data to the backend
        fYPTourneyPro.services.organizer.tournament.update(tournamentData.id, tournamentData).then(function (result) {
            // Close the modal and update the UI
            $('#EditTournamentModal').modal('hide');
            abp.notify.info('Tournament updated successfully.');
            location.reload(); // Reload the page or update the list dynamically
        });
    });
});
