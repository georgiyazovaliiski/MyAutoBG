function addProduct(idOfProduct) {
        checkIfInvalidData();
        let quantityOfProduct = $('#quantityOfProduct').val();
        let id = idOfProduct;
        let obj = {
            id: parseInt(id),
            quantity: parseInt(quantityOfProduct)
        };
        $.ajax({
            url: '../../Cart/AddProduct',
            type: 'POST',
            data: JSON.stringify(obj),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                window.location.href = '/Cart'
            }
        });
}


function editProduct(entry) {
    var t = $("#edit-" + entry).val();
    /*alert(t)*/

    $.ajax({
        url: '../../Shared/EditProduct',
        type: 'POST',
        data: JSON.stringify({
            productId: entry,
            val: t
        }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            window.location.href = '/Cart'
        }
    });
}
window.onload = function() {
    $("#quantityOfProduct").on("change", checkIfInvalidData);
}
function checkIfInvalidData() {
    if ($("#quantityOfProduct").val() <= 0) {
        $("#quantityOfProduct").val(1);
    }
    if ($("#quantityOfProduct").val() >= 1000) {
        $("#quantityOfProduct").val(999);
    }
}