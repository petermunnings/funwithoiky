$(document).ready(function () {
    $("#ChurchId").change(function () {
        var postData = { churchId: $(this).val() };
        $.post("/Ajax/ChangeChurchTo", $.postify(postData));
    });
})