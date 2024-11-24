$('#discussionForm').submit(function (e) {
    e.preventDefault();

    // Get form values
    var title = $('#Title').val();
    var content = $('#Content').val() || null;

    // Validate the form
    if ( !title || !content) {
        alert('Please fill all the required fields!');
        return;
    }

    // Prepare the registration data
    var postData = {
        Title: title,
        Content: content,
        Comments: []
    };

    console.log("Reg Data: ", postData)
    console.log("fYPTourneyPro.services.organizer.registration: ", fYPTourneyPro.services.posts.post)
    // Call the RegistrationAppService to create a new registration
    fYPTourneyPro.services.posts.post.create(postData).then((result) => {
        console.log("response: ", result)
        // On success, show an alert and reset the form
        alert('Registration Successful!');
        $('#discussionForm')[0].reset();
    }).fail(function (error) {
        // On failure, show an error alert
        alert('Registration Failed! ' + error.message);
    });
});