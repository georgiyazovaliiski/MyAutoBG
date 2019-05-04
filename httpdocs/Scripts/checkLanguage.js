window.onload = function(){
    checkCookie();
}

function checkCookie() {
    let id = id1;
    alert("enterd " + id);
    document.getElementById("disp").innerHTML = "hi";
    $.ajax({
        url: "/add.php ",
        type: "GET",
        data: {
            item_id: id,
        },
        success: function (response) {
            //document.getElementById("total_items").value=response;
            document.getElementById("disp").innerHTML = response;
        },
        error: function () {
            alert("error");
        }

    });
}