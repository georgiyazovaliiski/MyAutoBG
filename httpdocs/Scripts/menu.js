function showThirdly(id) {
    $.ajax({
        url: '/Shared/SubSubCategories',
        type: 'GET',
        data: { id: id },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            showResults(result);
        }
    });
}
function showResults(result) {
    console.log(result)
    $('#products2').empty(); $('#products2').show(); $('#product-item').remove();
    for (let i = 0; i < result.length; i++) {
        console.log(i)
        $('#products2').append(
            `<div class="col-lg-4">
                <a href='/Catalogue/Category/${result[i].UrlName}'><h1 class="btn btn-primary" style="width:100%; padding-top:40px; padding-bottom:40px; font-family:Arial">${result[i].Name[0].toUpperCase() + result[i].Name.substring(1)}</h1></a>
            </div>`);
    }
    /*@cat.Name[0].ToString().ToUpper()@cat.Name.Substring(1).ToLower()*/
}

