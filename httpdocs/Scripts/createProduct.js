window.onload = function () {
    showCategoryList();
}
function showCategoryList() {
    $.ajax({
        url: '/Products/showCategories',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            appendOnSelect('CategoryId',result)
        }
    });
}
function appendOnSelect(entry,data) {
    for (var i = 0; i < data.length; i++) {
        $(`#${entry}`).append(`<option value="${data[i].category.Id}">${data[i].highercategoryName} / ${data[i].highcategoryName} / ${data[i].category.Name}</option>`)
    }
}