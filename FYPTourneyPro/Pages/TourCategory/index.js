$(function () {

    // Collapse and uncollapse Category form
    $('#createCategoryButton').on('click', function () {
        const tournamentForm = document.getElementById('NewCategoryForm');
        if (tournamentForm.style.display === 'none' || tournamentForm.style.display === '') {
            tournamentForm.style.display = 'block'; // Show the form
        } else {
            tournamentForm.style.display = 'none'; // Hide the form
        }
    });


    // Adding a New Category  
    $('#NewCategoryForm').submit(function (e) {
        e.preventDefault();
        // Capture the value of the "Is Pair" checkbox
        var isPair = $('#IsPair').is(':checked');
        var categoryData = {
            name: $('#CategoryName').val(),
            description: $('#CategoryDescription').val(),
            tournamentId: $('#TournamentId').val(),
            isPair: isPair // Include the isPair flag
        };
        // Send the data to the backend to create a new category
        fYPTourneyPro.services.organizer.category.create(categoryData).then(function (result) {
            // Create the hyperlink with the same format as the server-rendered list items
            $('<li data-id="' + (result.id || '') + '">')
                .html(
                    '<i class="fa fa-trash-o"></i> ' +
                    '<a href="/TourCategory/CatPartList?categoryId=' + result.id + '">' +
                    (result.name || 'Unnamed Category') +
                    '</a>' +
                    (result.isPair ? ' <span class="badge bg-info">Doubles</span>' : '')
                )
                .appendTo($('#CategoryList'));

            // Add the new category to the dropdown
            $('#CategorySelect').append(new Option(result.name, result.id));
            // Clear the form fields
            $('#NewCategoryForm')[0].reset();
            abp.notify.success('Category created successfully!');
        }).catch(function (error) {
            abp.notify.error('Error creating category: ' + error.message);
        });
    });

    // Deleting a Category
    $('#CategoryList').on('click', 'li i', function () {
        var $li = $(this).parent();
        var id = $li.attr('data-id');
        fYPTourneyPro.services.organizer.category.delete(id).then(function () {
            $li.remove();
            abp.notify.info('Deleted the category.');
        });
    });
});