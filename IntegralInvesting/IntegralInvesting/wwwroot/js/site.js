// Opens the modal where users can enter the number of shares they want to purchase of the selected stock
function OpenPurchaseSharesModal(stockNameValue, latestPriceValue, symbolValue) {
    var data = { stockName: stockNameValue, latestPrice: latestPriceValue, symbol: symbolValue };
    $.ajax(
        {
            type: 'GET',
            url: '/PurchaseStock/OpenPurchaseModal',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-purchase-content').html(result);
                $('#purchase-modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

// Onclick event for Purchase button inside the modal that hides the modal when clicked
$('#btnModalPurchaseSubmit').click(function (event) {
    event.preventDefault();
    $('#purchase-modal-placeholder').modal('hide');
    Add();
});

// Opens the modal where users can enter the number of shares they want to sell of the selected stock
function OpenSellSharesModal(symbolValue, currentPriceValue, numberOfSharesValue) {
    var data = { symbol: symbolValue, currentPrice: currentPriceValue, numberOfShares: numberOfSharesValue };
    $.ajax(
        {
            type: 'GET',
            url: '/Portfolio/OpenSellModal',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-sell-content').html(result);
                $('#sell-modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        }); 
}

// Onclick event for Sell button inside the modal that hides the modal when clicked
$('#btnModalSellSubmit').click(function (event) {
    event.preventDefault();
    $('#sell-modal-placeholder').modal('hide');
    Add();
});

// Opens the modal where users can link a new bank account to their app account
function OpenLinkNewBankAccModal() {
    $.ajax(
        {
            type: 'GET',
            url: '/BankAccount/OpenLinkNewAccountModal',
            contentType: 'application/json; charset=utf=8',
            success: function (result) {
                $('#modal-linkNewBankAcc-content').html(result);
                $('#linkNewBankAcc-modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

// Onclick event for Link Account button inside the modal that hides the modal when clicked
$('#btnModalLinkNewBankAccSubmit').click(function (event) {
    event.preventDefault();
    $('#linkNewBankAcc-modal-placeholder').modal('hide');
    Add();
});

// Opens the modal where users can deposit funds into their linked bank accounts
function OpenDepositFundsModal() {
    $.ajax(
        {
            type: 'GET',
            url: '/BankAccount/OpenDepositFundsModal',
            contentType: 'application/json; charset=utf=8',
            success: function (result) {
                $('#modal-depositFunds-content').html(result);
                $('#depositFunds-modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

// Onclick event for Link Account button inside the modal that hides the modal when clicked
$('#btnModalDepositFundsSubmit').click(function (event) {
    event.preventDefault();
    $('#depositFunds-modal-placeholder').modal('hide');
    Add();
});

// Opens the modal where users can withdraw funds from their linked bank accounts
function OpenWithdrawFundsModal() {
    $.ajax(
        {
            type: 'GET',
            url: '/BankAccount/OpenWithdrawFundsModal',
            contentType: 'application/json; charset=utf=8',
            success: function (result) {
                $('#modal-withdrawFunds-content').html(result);
                $('#withdrawFunds-modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

// Onclick event for Link Account button inside the modal that hides the modal when clicked
$('#btnModalWithdrawFundsSubmit').click(function (event) {
    event.preventDefault();
    $('#withdrawFunds-modal-placeholder').modal('hide');
    Add();
});

// Calculate and set the PurchaseTotal value on the modal where users buy their shares

function CalculatePurchaseTotal(purchasePrice) {
    var purchaseQuantity = $('#purchaseQuantityField').val();
    var purchaseTotal = parseFloat(purchaseQuantity * purchasePrice).toFixed(2);

    $('#purchaseTotalField').val(String(purchaseTotal));
}

// Calculate and set the SaleTotal value on the modal where users sell their shares
function CalculateSaleTotal(currentPrice) {
    var sellQuantity = $('#sellQuantityField').val();
    var saleTotal = parseFloat(sellQuantity * currentPrice).toFixed(2);

    $('#saleTotalField').val(String(saleTotal));
}
