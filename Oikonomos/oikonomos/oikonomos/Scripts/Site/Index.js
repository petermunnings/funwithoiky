

$(document).ready(function () {

    $("#accordionIndex").accordion({
        heightStyle: "content"
    });

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
        if (data.ChurchEvents.length == 0) {
            var $accordion = $("#accordionIndex").accordion();
            var current = $accordion.accordion("option", "active"),
                maximum = $accordion.find("h3").length,
                next = current + 1 === maximum ? 0 : current + 1;
                // $accordion.accordion("activate",next); // pre jQuery UI 1.10
            $accordion.accordion("option", "active", next);
            $("#churchEventsTab").next().hide();
            $("#churchEventsTab").hide();
        } else {
            $("#churchEventsTemplate")
                .tmpl(data.ChurchEvents)
                .appendTo("#churchEventsList");
        }

    });
    


})