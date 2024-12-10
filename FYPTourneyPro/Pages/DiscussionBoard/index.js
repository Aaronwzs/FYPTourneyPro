// Create Form Submission
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
    };

    // Call the RegistrationAppService to create a new registration
    fYPTourneyPro.services.posts.post.create(postData).then((result) => {
        // On success, show an alert and reset the form
        alert('Registration Successful!');
        $('#discussionForm')[0].reset();
    }).fail(function (error) {
        // On failure, show an error alert
        alert('Registration Failed! ' + error.message);
    });
});

// Open Edit Modal
function openEditModal(postId, title, content) {
    // Populate the modal fields
    document.getElementById('editPostId').value = postId;
    document.getElementById('editTitle').value = title;
    document.getElementById('editContent').value = content;

    // Show the modal
    const editModal = new bootstrap.Modal(document.getElementById('editModal'));
    editModal.show();
}

//Edit Form Submission
$('#editDiscussionForm').submit(function (e) {
    e.preventDefault();

    // Get form values
    var id = $('#editPostId').val();
    var title = $('#editTitle').val();
    var content = $('#editContent').val() || null;

    // Validate the form
    if (!editTitle || !editContent) {
        alert('Please fill all the required fields!');
        return;
    }

    // Prepare the registration data
    var postData = {
        Title: title,
        Content: content,
    };

    // Call the RegistrationAppService to create a new registration
    fYPTourneyPro.services.posts.post.update(id, postData).then((result) => {

        // On success, show an alert and reset the form
        alert('Registration Successful!');
        $('#editDiscussionForm')[0].reset();
    }).fail(function (error) {
        // On failure, show an error alert
        alert('Registration Failed! ' + error.message);
    });
});

// Delete Post
$('#postDelete').on('click', function () {
    var $deleteId = $(this);
    var id = $deleteId.attr('data-id');

    fYPTourneyPro.services.posts.post.delete(id).then(function () {
        abp.notify.info('Deleted the post.');
    });
});


//Change filtering
function applyFilter() {
    // Get the selected filter type
    const filterType = document.getElementById('filterSelect').value;

    console.log(fYPTourneyPro.services.posts.postVote);
    fYPTourneyPro.services.posts.postVote.getFilteredPosts(filterType).then((result) => {
        console.log(result);
        updatePosts(result);
    });
}

//Update posts dynamically according to the filters
function updatePosts(result) {
    // Get the tbody element
    const postContainer = document.getElementById('postContainer');

    // Clear the existing rows
    postContainer.innerHTML = '';

    // Add rows for the new posts
    result.forEach(post => {
        console.log(post.postId);
        const row = `
            <tr onclick="window.location.href='/DiscussionBoard/Comments?postId=${post.postId}'" style="cursor: pointer;">
                <td>${post.title}</td>
                <td>${post.content}</td>
                <td>${new Date(post.creationTime).toLocaleString()}</td>
            </tr>
        `;
        postContainer.innerHTML += row; // Append the new row
    });
}

// Load posts with the default filter when the page loads
document.addEventListener('DOMContentLoaded', applyFilter);