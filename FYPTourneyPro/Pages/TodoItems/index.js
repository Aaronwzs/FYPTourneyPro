$(function () {

    // DELETING ITEMS /////////////////////////////////////////
    $('#TodoList').on('click', 'li i', function () {
        var $li = $(this).parent();
        var id = $li.attr('data-id');

        fYPTourneyPro.services.todoItems.todo.delete(id).then(function () {
            $li.remove();
            abp.notify.info('Deleted the todo item.');
        });
    });

    // CREATING NEW ITEMS /////////////////////////////////////
    $('#NewItemForm').submit(function (e) {
        e.preventDefault();

        var todoText = $('#NewItemText').val();
        console.log("todoText: ", fYPTourneyPro.services)
        fYPTourneyPro.services.todoItems.todo.create(todoText).then(function (result) {
            $('<li data-id="' + result.id + '">')
                .html('<i class="fa fa-trash-o"></i> ' + result.text)
                .appendTo($('#TodoList'));
            $('#NewItemText').val('');
        });
    });
});
