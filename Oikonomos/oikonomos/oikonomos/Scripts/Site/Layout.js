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
    if ($("#email_bodyWithFormatting").val() == "") {
        ShowErrorMessage("No email to send", "Email message is empty");
        return false;
    }

    if ($("#email_subject").val() == "") {
        ShowErrorMessage("Cannot send email", "Email has no subject");
        return false;
    }

    var postData = { subject: $("#email_subject").val(),
        body: $("#email_bodyWithFormatting").val()
    };

    var jqxhr = $.post("/Ajax/SendGroupEmail", $.postify(postData), function (data) {
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
    $("#email_bodyWithFormatting").val(body);
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
    $("#email_bodyWithFormatting").val("");
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

    $('.button').live('mouseover mouseout', function (event) {
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

    $("#email_bodyWithFormatting").tinymce({
        theme: "advanced",
        theme_advanced_buttons1: "bold,italic,underline, strikethrough, separator,justifyleft, justifycenter,justifyright, justifyfull, separator,forecolor,backcolor,separator, bullist,numlist,separator,outdent,indent,separator,undo,redo, code",
        theme_advanced_buttons2: "fontselect,fontsizeselect,formatselect",
        theme_advanced_buttons3: "",
        theme_advanced_buttons4: "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        content_css: '/Content/site.css',
    });

    $("#email_cancel").button({ icons: { primary: "ui-icon-close" } })
        .click(function(event) {
            $("#send_Email").slideUp();
            $("#mainContent").slideDown();
        });
    
    $("#email_send").button({ icons: { primary: "ui-icon-mail-closed" } })
    .click(function (event) {
        if (SendEmail() == true) {
            $("#email_subject").val("");
            $("#email_bodyWithFormatting").val("");
            $("#send_Email").slideUp();
            $("#mainContent").slideDown();
        }
    });


})