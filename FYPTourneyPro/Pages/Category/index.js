$(function () {

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

            

            // Clear the form fields
            $('#NewCategoryForm')[0].reset();

            abp.notify.success('Category created successfully!');

        });
    });

    
});
