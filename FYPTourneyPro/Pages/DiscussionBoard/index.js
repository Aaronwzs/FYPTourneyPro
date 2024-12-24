document.getElementById('createPostBtn').addEventListener('click', function () {
    const form = document.getElementById('createPostForm');
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
});

document.getElementById('cancelPostBtn').addEventListener('click', function () {
    const form = document.getElementById('createPostForm');
    form.style.display = 'none';
});


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

    // Show confirmation prompt
    if (confirm('Are you sure you want to delete this post?')) {
        // Proceed with deletion if confirmed
        fYPTourneyPro.services.posts.post.delete(id).then(function () {
            abp.notify.info('Deleted the post.');
            // Optionally reload the list or remove the deleted post dynamically
        }).catch(function (error) {
            abp.notify.error('Failed to delete the post.');
        });
    }
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
    const noPostsMessage = document.getElementById('noPostsMessage');

    postContainer.innerHTML = '';
    noPostsMessage.style.display = 'none';

    if (result.length === 0) {
        noPostsMessage.style.display = 'block';
        return;
    }

    // Add rows for the new posts
    result.forEach(post => {
        console.log(post);
        const postCard = document.createElement('div');
        postCard.className = 'col';

        postCard.innerHTML = `
                        <div class="card h-100">
                            <div class="card-body">
                                <h5 class="card-title">${post.title}</h5>
                                <p class="card-text">${post.content}</p>
                                <p class="text-muted small mb-1">
                                    Posted by <strong>${post.creator}</strong> at ${formatDate(post.creationTime)}
                                </p>
                                 <button class="btn btn-primary btn-sm")">View</button>
                            </div>
                        </div>
                    `;

        // Add click event listener to the "View" button
        const viewButton = postCard.querySelector('button');
        viewButton.addEventListener('click', () => {
            viewPost(post.postId);
        });
        postContainer.appendChild(postCard);
    });

    function viewPost(postId) {
        window.location.href = `/DiscussionBoard/Comments?postId=${postId}`;
    }
}

document.getElementById('searchInput').addEventListener('input', function () {
    const searchQuery = this.value.toLowerCase().trim(); // Get the search query
    const postContainer = document.getElementById('postContainer'); // The container for discussions
    const posts = postContainer.getElementsByClassName('col'); // Select all post cards

    // Loop through all posts and filter
    Array.from(posts).forEach((post) => {
        const title = post.querySelector('.card-title').innerText.toLowerCase();
        const content = post.querySelector('.card-text').innerText.toLowerCase();

        // Show posts that match the query, hide others
        if (title.includes(searchQuery) || content.includes(searchQuery)) {
            post.style.display = ''; // Show matching post
        } else {
            post.style.display = 'none'; // Hide non-matching post
        }
    });
});


// Load posts with the default filter when the page loads
document.addEventListener('DOMContentLoaded', applyFilter);