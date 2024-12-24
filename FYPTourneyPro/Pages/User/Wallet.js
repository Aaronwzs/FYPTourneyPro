$(function () {

  

    //Top Up
    $('#topUpForm').submit(function (e) {
        e.preventDefault();

        var topUpData = {
            topUpAmount: parseFloat($('#topUpAmt').val()),
             userId: $('#userId').val()
        };
       
        console.log("topUpData:", topUpData)
        

        console.log(" fYPTourneyPro.services.users.wallet:", fYPTourneyPro.services.users.wallet)
        fYPTourneyPro.services.users.wallet.topUp(topUpData).then(function (result) {
            abp.notify.success('TopUp successfully.');
            //location.reload();
        }).catch(function (error) {
            abp.notify.error('Top up failed: ' + error.message);
        });
    });

    //Withraw
    $('#WithdrawForm').submit(function (e) {
        e.preventDefault();

        var withdrawData = {
            withdrawAmount: parseFloat($('#withdrawAmt').val()),
            userId: $('#userId').val()
        };

        console.log("withdrawData:", withdrawData)


        console.log(" fYPTourneyPro.services.users.wallet:", fYPTourneyPro.services.users.wallet)
        fYPTourneyPro.services.users.wallet.withdraw(withdrawData).then(function (result) {
            abp.notify.success('Withdraw successfully.');
            //location.reload();
        }).catch(function (error) {
            abp.notify.error('Top up failed: ' + error.message);
        });
    });
});
