

$(document).ready(function () {

    $("#accordion").accordion();

    $("#SelectedChurchId").change(function () {

        var postData = { churchId: $("#SelectedChurchId").val() };

        $.post("/Ajax/SelectNewChurch", $.postify(postData), function () {
            window.location = "/Home/Index";
        }).error(function (jqXhr) {
            SendErrorEmail("Error calling SelectNewChurch", jqXhr.responseText);
        });
    });

    $.get("/Ajax/FetchChurchEvents", function(data) {
        $("#churchEventsList").empty();
        $("#churchEventsTemplate")
                .tmpl(data.ChurchEvents)
                .appendTo("#churchEventsList");
    });
    


})