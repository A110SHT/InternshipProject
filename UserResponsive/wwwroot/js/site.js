// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.


DetailsPopup = (url) => {
    GetView(url, "User Details");
}


DeletePopup = (url) => {
    GetView(url, "Are you Sure You Want To Delete??");   
}


GetView = (link, title) => {
    $.ajax({
        type: "GET",
        url: link,
        success: function (res) {
            $("#show-view .modal-title").html(title);
            $("#show-view .modal-body").html(res);
            $("#show-view").modal('show');

        }
    })

}
 ValidateImage=()=>
 {
     let img = document.getElementById('pic').value;
     var ValidExtension = ["jpg", "png", "jpeg"];
     if (img != '') {
         var imgext = img.substr(img.lastIndexOf('.') + 1);
         var lwr_ext = imgext.toLowerCase();
         var result = ValidExtension.includes(lwr_ext);
         if (result != true) {
             document.getElementById('img-error').innerHTML = " enter valid files";
             return false;
         }
         
         return true;
     } 
     return true;
}



$("#pic").on('change', function () {
    var countPics = $(this)[0].files.length;
    var imgPath = $(this)[0].value;
    var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();

    var image_holder = $("#imgholder");
    image_holder.empty();

    if (extn == "png" || extn == "jpg" || extn == "jpeg") {

        if (typeof (FileReader) != "undefined") {
            for (var i = 0; i < countPics; i++) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $("<img />", {
                        "src": e.target.result,
                        "class": "thumb-image",
                        "height": 100,
                        "width": 100,
                    }).appendTo(image_holder);
                }
                //showing image preview
                image_holder.show();
                reader.readAsDataURL($(this)[0].files[i]);
            }
        }
        else {
            alert("This browser does not support FileReader.");
        }
    }
    else {
        alert("Please select valid image only");
    }
});




