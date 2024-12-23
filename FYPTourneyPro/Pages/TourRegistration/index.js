$(function () {
    // Set current date for registration
    var today = new Date();
    var dateString = today.toISOString().split('T')[0];
    $('#regDate').val(dateString);


    console.log(fYPTourneyPro.services.organizer.registration)

    const categorySelect = document.getElementById('categorySelect');
    const player2Container = document.getElementById('player2Container');
    const player2Select = document.getElementById('player2');

    categorySelect.addEventListener('change', async function () { //using jquery
        const categoryId = categorySelect.value;

        if (categoryId) {
            // Fetch category details to check if "isPair" is true
            fYPTourneyPro.services.organizer.category.getIsPair(categoryId).then((result) => {
                console.log("is pair: ", result)

                if (result) {
                    $('#player2Container').show()
                } else {
                    $('#player2Container').hide()

                }
            })
            
        }
    });

    //// Submit the registration form
    //document.getElementById('registrationForm').addEventListener('submit', async function (event) {
    //    event.preventDefault();

    //    const categoryId = categorySelect.value;
    //    const player1 = '@Model.UserId';  // The logged-in user's ID
    //    const player2 = player2Select.value || null;
    //    const regDate = document.getElementById('regDate').value;
    //    const totalAmount = parseFloat(document.getElementById('totalAmount').value);

    //    // Validate the form
    //    if (!categoryId || !regDate || !totalAmount) {
    //        alert('Please fill all the required fields!');
    //        return;
    //    }

    //    // Send registration request to RegistrationAppService
    //    const registrationData = {
    //        categoryId,
    //        totalAmount,
    //        userId1: player1,
    //        userId2: player2,
    //        tournamentId: '@Model.TournamentId'
    //    };

    //    const response = await fetch('/api/services/app/registration/createAsync', {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json',
    //            'Accept': 'application/json'
    //        },
    //        body: JSON.stringify(registrationData)
    //    });

    //    if (response.ok) {
    //        alert('Registration Successful!');
    //    } else {
    //        alert('Registration Failed!');
    //    }
    //});

    $('#registrationForm').submit(function (e) {
        e.preventDefault();

        // Get form values
        var categoryId = $('#categorySelect').val();
        var player1 = $('#player1').val();
        var player2 = $('#player2').val() || null;
        var regDate = $('#regDate').val();
        var totalAmount = parseFloat($('#totalAmount').val());
        var userId = $('#userId').val();

        console.log("player1: ", player1);
        console.log("player2: ", player2);
        console.log("regDate: ", regDate);
        console.log("totalAmount: ", totalAmount);
        console.log("categoryId: ", categoryId);

        // Validate the form
        if (!categoryId || !regDate || !totalAmount) {
            alert('Please fill all the required fields!');
            return;
        }

        // Prepare the registration data
        var registrationData = {
            categoryId: categoryId,
            totalAmount: totalAmount,
            username1: player1,
            username2: player2,
            tournamentId: $('#tournamentId').val()
        };
        console.log("Reg Data: ", registrationData);
        var paymentData = {
            UserId: userId,
            RegFee: totalAmount
        };
        console.log("paymentData:", paymentData);



        console.log("Reg Data: ", registrationData)
        console.log("fYPTourneyPro.services.organizer.registration: ", fYPTourneyPro.services.organizer.registration)
        console.log("fYPTourneyPro.services.users.wallet: ", fYPTourneyPro.services.users.wallet)

        // Start payment process
        fYPTourneyPro.services.users.wallet.payment(paymentData).then(function (result) {
            console.log("Payment successful:", result);
            abp.notify.success('Paid successfully.');

            // Now proceed to create registration
            return fYPTourneyPro.services.organizer.registration.create(registrationData);
        }).then(function (result) {
            console.log("Registration response: ", result);
            alert('Registration Successful!');
            $('#registrationForm')[0].reset();
        }).catch(function (error) {
            // Catch errors from either payment or registration
            abp.notify.error('Error: ' + error.message);
            console.error("Error during payment or registration:", error);
        });
    });
});