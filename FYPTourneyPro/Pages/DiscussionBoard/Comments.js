// Add a new comment
$('#addCommentForm').submit(function (e) {
    e.preventDefault();

    // Get form values
    var content = $('#newCommentContent').val();

    // Validate the form
    if (!content) {
        alert('Please fill all the required fields!');
        return;
    }

    // Extract the postId from the URL
    const urlParams = new URLSearchParams(window.location.search);
    const postId = urlParams.get('postId'); // Get the postId query parameter

    // Validate postId
    if (!postId) {
        alert('Post ID is missing.');
        return;
    }

    // Prepare the comment data
    var commentData = {
        PostId: postId, // Include the postId in the data
        Content: content,
    };

    console.log(commentData);

    // Call the RegistrationAppService to create a new registration
    fYPTourneyPro.services.posts.comment.create(commentData).then((result) => {
        // On success, show an alert and reset the form
        alert('Comment added!');
        $('#addCommentForm')[0].reset();
    }).fail(function (error) {
        // On failure, show an error alert
        alert('Registration Failed! ' + error.message);
    });
});