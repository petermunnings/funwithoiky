

$(document).ready(function () {

    $("#accordion").accordion();

    $("#SelectedChurchId").change(function () {

        var postData = { churchId: $("#SelectedChurchId").val() };

        var jqxhr = $.post("/Ajax/SelectNewChurch", $.postify(postData), function (data) {
            window.location = "/Home/Index";
        }).error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling SelectNewChurch", jqXHR.responseText);
        });
    });


})