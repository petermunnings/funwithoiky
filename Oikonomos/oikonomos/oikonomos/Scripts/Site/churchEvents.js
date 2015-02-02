
function setupCropChurchEventPic() {
    var d = new Date();
    $("#td_churchEventImageContainer").html('');
    $("#td_churchEventImageContainer").html('<img class="cropper" style="width:600px"/>');
    $(".cropper").attr("src", "/Images/Index?dateTime=" + d.getTime());
    $(".cropper").cropper({
        aspectRatio: 1,
        done: function (data) {
            picCrop.X1 = data.x1;
            picCrop.X2 = data.x2;
            picCrop.Y1 = data.y1;
            picCrop.Y2 = data.y2;
            picCrop.Height = data.height;
            picCrop.Width = data.width;

        }
    });
}



$(document).ready(function () {

    $("#button_addNewChurchEvent").click(function () {
        $("#div_addNewEvent").dialog('open');
    });

    $("#div_addNewEvent").dialog({
        autoOpen: false,
        modal: true,
        height: 800,
        width: 680,
        resizable: false,
        buttons: {
            "Save church event image": function () {
                $.ajax({
                    url: "/Images/SaveCroppedImage",
                    data: JSON.stringify(picCrop),
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {
                        var d = new Date();
                        $.ajax({
                            url: "/Images/DeleteDefaultImage",
                            type: "POST",
                            success: function (result) {
                                alert('saved');
                            }
                        });
                    }
                });
                $(this).dialog('close');
            },
            "Cancel": function () {
                $(this).dialog('close');
            }
        },
        open: function (event, ui) {
            setupCropChurchEventPic();
        }
    });
});