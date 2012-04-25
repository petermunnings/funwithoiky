

$(document).ready(function () {

    $("#forgotPassword").click(function () {
        $("#td_message").html("");
        if ($("#email").val() == "") {
            $("#td_message").html("Please enter your email address");
        }
        else {
            $("#td_message").html("Checking email address...&nbsp;<img src='/Content/images/ajax-loader.gif' />");
            var postData = { emailAddress: $("#email").val()  };

            $.post("/Ajax/ResetPassword", $.postify(postData), function (data) {
                $("#td_message").html(data.Message);
            }).error(function (jqXHR, textStatus, errorThrown) {
                SendErrorEmail("Error calling ResetPassword", jqXHR.responseText);
            });
        }
        return false;
    });
})