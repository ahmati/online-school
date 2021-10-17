$(document).ready(function () {
    var stripe = Stripe($("#publishableKey").val());
    var elements = stripe.elements();
    var style = {
        base: {
            'lineHeight': '1.35',
            'fontSize': '1.11rem',
            'color': '#495057',
            'fontFamily': 'apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif'
        }
    };

    var card = elements.create('cardNumber', {
        'placeholder': '',
        'style': style
    });
    card.mount('#card-number');
    
    var cvc = elements.create('cardCvc', {
        'placeholder': '',
        'style': style
    });
    cvc.mount('#card-cvc');

    var exp = elements.create('cardExpiry', {
        'placeholder': '',
        'style': style
    });
    exp.mount('#card-exp');


    function displayLoading(formName, isLoading) {
        let form = $(`#${formName}`);

        if (form) {
            try {
                kendo.ui.progress(form, isLoading);
            }
            catch (e) {
                console.log(e);
            }
        }
    }

    $('#payment-submit').on('click', function (e) {
        var urlAction = $('#ActionName').val();
        e.preventDefault();
        var cardData = {
            name: $('#card-name').val()
        };

        var id = $('#id').val();
        var purchasableItemType = $('#purchasableItemType').val();
        var nameOnCard = document.getElementById('card-name').value;
        var regName = /^[a-zA-Z]+ [a-zA-Z]+$/;
        
        if (!nameOnCard.match(regName) || nameOnCard.length == 0) {
            ShowError("Please enter your full name (FirstName/space/LastName).");
            return false;
        }
            
        stripe.createToken(card, cardData).then(function (result) {
            
            if (result.error && result.error.message) {
                ShowError(result.error.message);
            } else {
                displayLoading("payment-form", true);
                $.ajax({
                    type: "POST",
                    url: "/Payment/Charge",
                    data: { stripeToken: result.token.id, id: id, purchasableItemType: purchasableItemType},
                    dataType: "json",
                    success: function (result) {
                        ShowSuccess("Payment completed successfully");
                        displayLoading("payment-form", false);
                        window.location = urlAction;
                    },
                    error: function (req, status, error) {
                        ShowError("Payment unsuccessful !");
                        displayLoading("payment-form", false);
                        console.log(error);
                    }
                });
            }
        });
    });
});