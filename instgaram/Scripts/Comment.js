

function sendcomment(id, idstu) {
    var x = document.getElementById(id);
    var fa = new FormData();

    fa.append("idpost", x.id);
    fa.append("comment", x.value);
    fa.append("iduser", idstu)
    
    $.ajax({
        url: 'https://localhost:44393/api/Comments',
        type: 'POST',
        data: fa,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            console.log(data);
            x.value = "";
        }
    });
}