var addressList;

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function SendErrorEmail(subject, body) {
    var postData = { subject: subject, body: htmlEncode(body) };
    var jqxhr = $.post("/Email/SendSystemEmail", $.postify(postData))
    .error(function (jqXHR, textStatus, errorThrown) {
        ShowErrorMessage("Oiky is really battling", "Something really unexpected happened.  We can't even automatically let our developers know.  Please email them as support@oikonomos.co.za and let them know.  " + jqXHR.responseText);
    });
    ShowErrorMessage("A very unexpected error has occured.", "We apologize up front.  We have already sent off an email to our developers and they will start working on this a.s.a.p.");
}

function ShowErrorMessage(title, message) {
    $("#errorMessage").html(message);
    
    $("#dialog_errorMessage").dialog({
        modal: true,
        title: title,
        buttons: {
            Ok: function () {
                $("#errorMessage").html("");
                $(this).dialog("close");
            }
        }
    });
}

function ShowInfoMessage(title, message) {
    $("#infoMessage").html(message);

    $("#dialog_infoMessage").dialog({
        modal: true,
        title: title,
        buttons: {
            Ok: function () {
                $("#infoMessage").html("");
                $(this).dialog("close");
            }
        }
    });
}

function SendEmail() {
    var content = tinyMCE.get('email_bodyWithFormatting').getContent();
    if (content == "") {
        ShowErrorMessage("No email to send", "Email message is empty");
        return false;
    }

    if ($("#email_subject").val() == "") {
        ShowErrorMessage("Cannot send email", "Email has no subject");
        return false;
    }

    var postData = { subject: $("#email_subject").val(),
        body: content
    };

    $.post("/Ajax/SendGroupEmail", $.postify(postData), function (data) {
        $("#responseMessage_text").html(data.Message);
        $("#response_Message").dialog(
            {
                modal: true,
                height: 200,
                width: 500,
                resizable: true,
                buttons: {
                    "Close": function () {
                        $(this).dialog("close");
                    }
                }
            });
    }).error(function (jqXHR, textStatus, errorThrown) {
        SendErrorEmail("Error calling SendGroupEmail", jqXHR.responseText);
    });

    return true;
}

function SendSms() {
    if ($("#sms_message").val() == "") {
        ShowErrorMessage("Cannot send sms", "There is no sms to send");
        return false;
    }
    
    var postData = { message: $("#sms_message").val() };

    var jqxhr = $.post("/Ajax/SendGroupSms", $.postify(postData), function (data) {
        $("#responseMessage_text").html(data.Message);
        $("#response_Message").dialog(
            {
                modal: true,
                height: 200,
                width: 500,
                resizable: true,
                buttons: {
                    "Close": function () {
                        $(this).dialog("close");
                    }
                }
            });
    }).error(function (jqXHR, textStatus, errorThrown) {
        SendErrorEmail("Error calling SendGroupSms", jqXHR.responseText);
    });

    return true;
}

function getDialogButton(dialog_selector, button_name) {
    var buttons = $(dialog_selector + ' .ui-dialog-buttonpane button');
    for (var i = 0; i < buttons.length; ++i) {
        var jButton = $(buttons[i]);
        if (jButton.text() == button_name) {
            return jButton;
        }
    }
    return null; 
}

function SetupEmailDialog(subject, body) {
    $("#ajax_loader_sendEmail").hide();
    var sendButton = getDialogButton( '.email_dialog', 'Send' );
    if (sendButton) {
        sendButton.button("enable");
        sendButton.button("option", "icons", { primary: "ui-icon-mail-closed" });
    }
    var cancelButton = getDialogButton('.email_dialog', 'Cancel');
    if (cancelButton) {
        cancelButton.button("option", "icons", { primary: "ui-icon-close" });
    }
    $("#email_subject").val(subject);
    if (typeof body === "undefined") {
        body = "";
    }
    tinyMCE.get('email_bodyWithFormatting').setContent(body);
}

function SetupSmsDialog(noNos, message) {
    $("#ajax_loader_sendSms").hide();
    var sendButton = getDialogButton('.sms_dialog', 'Submit');
    if (sendButton) {
        sendButton.button("enable");
        sendButton.button("option", "icons", { primary: "ui-icon-mail-closed" });
        sendButton.button().addClass("ui-state-active");
    }
    var cancelButton = getDialogButton('.sms_dialog', 'Cancel');
    if (cancelButton) {
        cancelButton.button("option", "icons", { primary: "ui-icon-close" });
    }
    $("#noNos").html("Sms will be sent to " + noNos + " valid cell phone Nos");
    $("#sms_message").val(message);
}

function OpenEmailDialog() {

    $("#ajax_loader_sendEmail").show();
    $("#email_subject").val("");
    tinyMCE.get('email_bodyWithFormatting').setContent("");
    $("#send_Email").slideDown(200);
    $("#mainContent").slideUp(200);
}

function OpenSmsDialog() {
    $("#noNos").html("");
    $("#ajax_loader_sendSms").show();
    $("#sms_message").val("");

    $("#send_Sms").dialog(
        {
            dialogClass: 'sms_dialog',
            modal: true,
            height: 350,
            width: 600,
            buttons: {
                Submit: function () {
                    if (SendSms() == true) {
                        $("#sms_message").val("");
                        $(this).dialog('close');
                    }
                },
                Cancel: function () {
                    $("#sms_message").val("");
                    $(this).dialog('close');
                }
            }
        });

    // now programmatically get the submit button and disable it
    var button = getDialogButton('.sms_dialog', 'Submit');
    if (button) {
        button.prop('disabled', true).addClass('ui-state-disabled');
    }
}

function stopRKey(evt) { 
  var evt = (evt) ? evt : ((event) ? event : null); 
  var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null); 
  if ((evt.keyCode == 13) && (node.type=="text"))  {return false;} 
}

$(document).ready(function () {

    setTimeout("window.location = '/Home/Index'", 1210000);

    document.onkeypress = stopRKey;

    $('.button').on('mouseover mouseout', function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
        } else {
            $(this).css("cursor", "default");
        }
    });

    $(".button").tooltip({ position: "top right", opacity: 1, tipClass: "tooltip" });
    $("button, input:submit").button();

    $("#button_home").click(function () {
        window.location = "/Home/Index";
    });
    $("#button_person").click(function () {
        window.location = "/Home/Person";
    });
    $("#button_homegroup").click(function () {
        window.location = "/Home/Groups";
    });
    $("#button_settings").click(function () {
        window.location = "/Home/Settings";
    });
    $("#button_lists").click(function () {
        window.location = "/Home/ReportGrid";
    });
    $("#button_reports").click(function () {
        window.location = "/Home/ReportsAdmin";
    });
    $("#button_help").click(function () {
        window.location = "/Home/Help";
    });
    $("#button_logout").click(function () {
        window.location = "/Home/Logout";
    });
    $("#button_sysAdmin").click(function () {
        window.location = "/Home/SysAdmin";
    });

    tinymce.init(
    {
        mode: "specific_textareas",
        editor_selector: "mceEditor"
    });

    $("#email_cancel").button({ icons: { primary: "ui-icon-close" } })
        .click(function() {
            $("#send_Email").slideUp();
            $("#mainContent").slideDown();
            $.post("/Home/RemoveAllAttachments", function (data) {
                var newHtml = [];
                $('.file_name').html(newHtml.join(""));
            });
        });
    
    $("#email_send").button({ icons: { primary: "ui-icon-mail-closed" } })
    .click(function () {
        if (SendEmail() == true) {
            $("#email_subject").val("");
            tinyMCE.get('email_bodyWithFormatting').setContent("");
            $("#send_Email").slideUp();
            $("#mainContent").slideDown();
        }
    });

    $('#fileupload').fileupload({
        dataType: 'json',
        url: '/Home/UploadFiles',
        autoUpload: true,
        maxChunkSize: 4096,
        done: function (e, data) {
            if (data.result.errorMessage)
                ShowErrorMessage("Error", data.result.errorMessage);
            $("#progressbar").progressbar({
                value: 0
            });
            $("#progressbar").hide();
            $("#fileupload").show();
            var newHtml = [];
            $.each(data.result.list, function (index, value) {
                newHtml.push("<tr style='vertical-align:top'><td><span class='ui-icon ui-icon-close removeButton'></span>&nbsp;</td><td>" + value.name + "</td></tr>");
            });

            $('.file_name').html(newHtml.join(""));
        }
    }).on('fileuploadprogressall', function (e, data) {
        $("#progressbar").show();
        $("#fileupload").hide();
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $("#progressbar").progressbar({
            value: progress
        });
    });
    
    $(".file_name").on("click", ".removeButton", function () {
        $.post("/Home/RemoveAttachment?name=" + this.parentElement.parentElement.innerText.trim(), function (data) {
            var newHtml = [];
            $.each(data.list, function (index, value) {
                newHtml.push("<tr style='vertical-align:top'><td><span class='ui-icon ui-icon-close removeButton'></span>&nbsp;</td><td>" + value.name + "</td></tr>");
            });

            $('.file_name').html(newHtml.join(""));
        });

    });


})