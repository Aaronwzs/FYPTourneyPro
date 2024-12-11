$(function () {

    


    // Submit Application Form
    $('#NewApplicationForm').submit(function (e) {
        e.preventDefault();

        var requestedRole = $('#RequestedRole').val();
        var description = $('#Description').val();

        var applicationData = {
            requestedRole: requestedRole,
            description: description
        };

        console.log(fYPTourneyPro.services.users.appForm)

        // Send the data to the backend to create a new application
        fYPTourneyPro.services.users.appForm.create(applicationData).then(function () {
            abp.notify.success('Application submitted successfully.');
            location.reload();  // Refresh the page to show the updated list of applications
        }).catch(function (error) {
            abp.notify.error('An error occurred while submitting the application.');
        });
    });

    // Approve Application
    window.approveApplication = function (id) {
        fYPTourneyPro.services.users.appForm.approve(id).then(() => {
            abp.notify.success('Application approved');
            location.reload();
        }).catch((error) => {
            abp.notify.error('An error occurred while approving the application');
        });
    };

    // Reject Application
    window.rejectApplication = function (id) {
        fYPTourneyPro.services.users.appForm.reject(id).then(() => {
            abp.notify.success('Application rejected');
            location.reload();
        }).catch((error) => {
            abp.notify.error('An error occurred while rejecting the application');
        });
    };

    // Update the application status in the table and hide the action buttons
    function updateApplicationStatus(id, status) {
        var row = $('#application-' + id);
        var statusCell = row.find('.status-cell');
        var actionsCell = row.find('td:last-child');

        // Update the status text
        statusCell.html('<span>' + status + '</span>');

        // Hide the action buttons
        actionsCell.html('');
    }
});
