﻿// Extract the postId from the URL
const urlParams = new URLSearchParams(window.location.search);
const postId = urlParams.get('postId'); // Get the postId query parameter


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

    // Call the RegistrationAppService to create a new registration
    fYPTourneyPro.services.posts.comment.create(commentData).then((result) => {
        // On success, show an alert and reset the form
        alert('Comment added!');
        $('#addCommentForm')[0].reset();
    }).fail(function (error) {
        // On failure, show an error alert
        alert('Comment Failed! ' + error.message);
    });
});

//Button vote and downvote click
async function vote(voteType, postId) {
    console.log(`Vote Type: ${voteType}, Post ID: ${postId}`);
    var voteData = {
        PostId: postId,
        VoteType: voteType
    };

    //Only can choose one of the votes
    if (voteType === 'Upvote') {
        if (upvoteButton.classList.contains('active')) {
            // If Upvote is already active, cancel the vote
            await fYPTourneyPro.services.posts.postVote.deleteVote(postId).then((result) => {
                upvoteButton.classList.remove('active'); // Remove active state
            });
        } else {
            // Add Upvote and deactivate Downvote
            await fYPTourneyPro.services.posts.postVote.createOrUpdateVote(voteData).then((result) => {
                upvoteButton.classList.add('active'); // Add active state to Upvote
                downvoteButton.classList.remove('active'); // Remove active state from Downvote
            });
        }
    } else if (voteType === 'Downvote') {
        if (downvoteButton.classList.contains('active')) {
            // If Downvote is already active, cancel the vote
            await fYPTourneyPro.services.posts.postVote.deleteVote(postId).then((result) => {
                downvoteButton.classList.remove('active'); // Remove active state
            });
        } else {
            // Add Downvote and deactivate Upvote
            await fYPTourneyPro.services.posts.postVote.createOrUpdateVote(voteData).then((result) => {
                downvoteButton.classList.add('active'); // Add active state to Downvote
                upvoteButton.classList.remove('active'); // Remove active state from Upvote
            });
        }

    }

    getVoteCounts(postId);

    };

function getVoteCounts(postId) {
    fYPTourneyPro.services.posts.postVote.getVoteCount(postId).then((result) => {
        console.log(result);
        document.getElementById('upvoteCount').innerHTML = result.upvotes;
        document.getElementById('downvoteCount').innerHTML = result.downvotes;
    });
}

getVoteCounts(postId);


function navigateToChat(userId) {
    const participantIds = [userId];
    console.log("boboga");
    fYPTourneyPro.services.chat.chat.createGroupChat("", participantIds).then((result) => {
        console.log("hihihihi");
        console.log(result);
        window.location.href = `/Chat/Chatroom?chatRoomId=${result.id}`;
    });
    

}