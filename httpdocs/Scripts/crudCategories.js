window.onload = function () {
    showCategoryList();
}
function showCategoryList() {
    $.ajax({
        url: '/Products/showAllCategories',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            appendOnSelect('CategoryId', result)
        }
    });
}
function showNonPromoProductList() {
    $.ajax({
        url: '/Products/showNonPromoProductList',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            appendOnSelect('ProductId', result)
        }
    });
}
function appendOnSelect(entry,data) {
    for (var i = 0; i < data.length; i++) {
        $(`#${entry}`).append(`<option value="${data[i].category.Id}">${data[i].highcategoryName} / ${data[i].category.Name}</option>`)
    }
}