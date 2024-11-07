$(function () {

    // Set current date for registration
    var today = new Date();
    var dateString = today.toISOString().split('T')[0];
    $('#RegistrationDate').val(dateString);

    // Deleting a Category
    $('#CategoryList').on('click', 'li i', function () {
        var $li = $(this).parent();
        var id = $li.attr('data-id');

        fYPTourneyPro.services.organizer.category.delete(id).then(function () {
            $li.remove();
            abp.notify.info('Deleted the category.');
        });
    });


    // Adding a New Category  
    $('#NewCategoryForm').submit(function (e) {
        e.preventDefault();

        var categoryData = {
            name: $('#CategoryName').val(),
            description: $('#CategoryDescription').val(),
            tournamentId: $('#TournamentId').val()
        };

        fYPTourneyPro.services.organizer.category.create(categoryData).then(function (result) {
            $('<li data-id="' + (result.id || '') + '">')
                .html('<i class="fa fa-trash-o"></i> ' + (result.name || 'Unnamed Category'))
                .appendTo($('#CategoryList'));

            // Add the new category to the dropdown
            $('#CategorySelect').append(new Option(result.name, result.id));

            // Clear the form fields
            $('#NewCategoryForm')[0].reset();

            abp.notify.success('Category created successfully!');

        });
    });

    
    // Handle Player Registration
    $('#PlayerRegistrationForm').submit(function (e) {
        e.preventDefault();

        // Check if the user is logged in
        if (!abp.currentUser.id) {
            abp.notify.error('You must be logged in to register.');
            return;
        }

        var registrationData = {
            date: $('#RegistrationDate').val(),
            amount: parseFloat($('#RegistrationAmount').val()),
            categoryId: $('#CategorySelect').val(),
            tournamentId: $('#RegTournamentId').val(),
            userName: abp.currentUser.userName,
            userId: abp.currentUser.id
        };

        if (!registrationData.categoryId) {
            abp.notify.error('Please select a category.');
            return;
        }

        fYPTourneyPro.services.organizer.playerRegistration.create(registrationData)
            .then(function (result) {
                abp.notify.success('Registration successful!');
                // Clear the form
                $('#RegistrationAmount').val('');
                $('#CategorySelect').val('');
            })
            .catch(function (error) {
                abp.notify.error('Registration failed. Please try again.');
                console.error('Registration error:', error);
            });
    });

    // Handle Registration Deletion
    $('#RegistrationsList').on('click', '.delete-registration', function () {
        var $row = $(this).closest('tr');
        var id = $row.data('id');

        if (confirm('Are you sure you want to delete this registration?')) {
            fYPTourneyPro.services.organizer.playerRegistration.delete(id)
                .then(function () {
                    $row.remove();
                    abp.notify.info('Registration deleted successfully.');
                })
                .catch(function (error) {
                    abp.notify.error('Failed to delete registration.');
                    console.error('Deletion error:', error);
                });
        }
    });

    // Update the registration list after successful registration
    // Modify the existing registration success handler:
    fYPTourneyPro.services.organizer.playerRegistration.create(registrationData)
        .then(function (result) {
            abp.notify.success('Registration successful!');

            // Add the new registration to the table
            var categoryName = $('#CategorySelect option:selected').text();
            var newRow = `
                <tr data-id="${result.id}">
                    <td>${new Date(result.date).toLocaleDateString()}</td>
                    <td>${result.amount.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}</td>
                    <td>${categoryName}</td>
                    <td>
                        <button class="btn btn-sm btn-danger delete-registration">
                            <i class="fas fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
            $('#RegistrationsList').append(newRow);

            // Clear the form
            $('#RegistrationAmount').val('');
            $('#CategorySelect').val('');
        })
        .catch(function (error) {
            abp.notify.error('Registration failed. Please try again.');
            console.error('Registration error:', error);
        });

});
