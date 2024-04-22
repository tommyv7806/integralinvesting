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
                $('#modal-placeholder').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

$('#btnModalPurchaseSubmit').click(function (event) {
    event.preventDefault();
    $('#modal-placeholder').modal('hide');
    Add();
});

function CalculatePurchaseTotal(purchasePrice) {
    var purchaseQuantity = $('#purchaseQuantityField').val();
    var purchaseTotal = parseFloat(purchaseQuantity * purchasePrice).toFixed(2);

    $('#purchaseTotalField').val(String(purchaseTotal));
}
