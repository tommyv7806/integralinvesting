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
function OpenSellSharesModal(numberOfSharesValue, currentPriceValue) {
    var data = { numberOfShares: numberOfSharesValue, currentPrice: currentPriceValue };
    $.ajax(
        {
            type: 'GET',
            url: '/PurchaseStock/OpenSellModal',
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

function CalculatePurchaseTotal(purchasePrice) {
    var purchaseQuantity = $('#purchaseQuantityField').val();
    var purchaseTotal = parseFloat(purchaseQuantity * purchasePrice).toFixed(2);

    $('#purchaseTotalField').val(String(purchaseTotal));
}
