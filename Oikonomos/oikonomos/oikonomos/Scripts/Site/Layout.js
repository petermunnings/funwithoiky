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
        alert(jqXHR.responseText);
    });
    alert("A very unexpected error has happened.  We apologize up front.  We have already sent off an email to our developers and they will start working on this a.s.a.p.");
}

function SendEmail() {
    var postData = { subject: $("#email_subject").val(),
        body: $("#email_body").val()
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
}

function SendSms() {
    var postData = { message: $("#sms_message").val()
    };

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

function SetEmailList() {
    $("#ajax_loader_sendEmail").hide();
    var button = getDialogButton( '.email_dialog', 'Submit' );
    if (button) {
        button.prop('disabled', false).removeClass('ui-state-disabled');
    }
}

function SetSmsList() {
    $("#ajax_loader_sendSms").hide();
    var button = getDialogButton('.sms_dialog', 'Submit');
    if (button) {
        button.prop('disabled', false).removeClass('ui-state-disabled');
    }
}

function OpenEmailDialog() {
    $("#ajax_loader_sendEmail").show();
    $("#email_subject").val("");
    $("#email_body").val("");
    $("#send_Email").dialog(
        {
            dialogClass: 'email_dialog',
            modal: true,
            height: 540,
            width: 750,
            buttons: {
                Cancel: function () {
                    $("#email_subject").val("");
                    $("#email_body").val("");
                    $(this).dialog('close');
                },
                Submit: function () {

                    SendEmail();

                    $("#email_subject").val("");
                    $("#email_body").val("");
                    $(this).dialog('close');
                }
            }
        });
            
    // now programmatically get the submit button and disable it
    var button = getDialogButton( '.email_dialog', 'Submit' );
    if ( button ){  
        button.prop('disabled', true).addClass('ui-state-disabled');
    }
}

function OpenSmsDialog() {
    $("#ajax_loader_sendSms").show();
    $("#sms_message").val("");

    $("#send_Sms").dialog(
        {
            dialogClass: 'sms_dialog',
            modal: true,
            height: 300,
            width: 600,
            buttons: {
                Cancel: function () {
                    $("#sms_message").val("");
                    $(this).dialog('close');
                },
                Submit: function () {

                    SendSms();

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

    $("#sms_message").keyup(function () {
        var noCharactersLeft = 160 - $("#sms_message").val().length;
        if (noCharactersLeft < 0) {
            $("#sms_message").val($("#sms_message").val().substring(0, 160));
            noCharactersLeft = 0;
        }
        $("#span_noCharactersLeft").html(noCharactersLeft);
    });

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
})