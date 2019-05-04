function addOrder() {
    var requestData = {

    };
    $.ajax({
        url: 'Orders/CheckOut',
        type: 'POST',
        data: JSON.stringify(requestData),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (result) {
            CheckIfInvoiceFound(result);
        },
        async: true,
        processData: false
    });
}