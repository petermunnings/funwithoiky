var familyMembers;

function ClearForm() {
    var searchText = $("#text_personSearch").val();
    $("input:text").val("");
    $("#hidden_personId").val("0");
    $("#hidden_familyId").val("0");
    $("#hidden_groupId").val("0");
    $("#group_Name").text("None");
    $("#hidden_lat").val("0");
    $("#hidden_lng").val("0"); 
    var genderRadio = $('input:radio[name=Gender]');
    genderRadio.filter('[value=Male]').prop('checked', false);
    genderRadio.filter('[value=Female]').prop('checked', false);
    $("#warning_firstname").hide();
    $("#warning_surname").hide();

    $("#text_personSearch").val(searchText);
    familyMembers = [];
    $("#family_members").empty();
    $("#Site").val("Select site...");
    $("#row_image").hide();
    $("#img_person").prop("src", " ");
    $("#GroupId").val("0"); 
    }

function PopulatePerson(person) {
    $("#hidden_personId").val(person.PersonId);

    $("#hidden_familyId").val(person.FamilyId);
    $("#hidden_groupId").val(person.GroupId);
    $("#group_Name").text(person.GroupName);
    $("#hidden_roleId").val(person.RoleId);
    $("#text_firstname").val(person.Firstname);
    $("#warning_firstname").hide();
    $("#text_surname").val(person.Surname);
    $("#warning_surname").hide();
    $("#text_email").val(person.Email);
    $("#text_dateOfBirth").val(person.DateOfBirth);
    $("#text_anniversary").val(person.Anniversary);
    $("#text_homePhone").val(person.HomePhone);
    $("#text_cellPhone").val(person.CellPhone);
    $("#text_workPhone").val(person.WorkPhone);
    $("#text_skype").val(person.Skype);
    $("#text_twitter").val(person.Twitter);
    $("#text_occupation").val(person.Occupation);
    var genderRadio = $('input:radio[name=Gender]');
    genderRadio.filter('[value=Male]').prop('checked', false);
    genderRadio.filter('[value=Female]').prop('checked', false);
    if (person.Gender == "Male") {
        genderRadio.filter('[value=Male]').prop('checked', true);
    }
    if (person.Gender == "Female") {
        genderRadio.filter('[value=Female]').prop('checked', true);
    }

    $("#text_address1").val(person.Address1);
    $("#text_address2").val(person.Address2);
    $("#text_address3").val(person.Address3);
    $("#text_address4").val(person.Address4);
    $("#hidden_lat").val(person.Lat);
    $("#hidden_lng").val(person.Lng);
    $("#RoleId").val(person.RoleId);
    if ($('#RoleId option[value=' + person.RoleId + ']').length == 0) {
        $("#row_roleDropDown").hide();
    } else {
        $("#row_roleDropDown").show();
    }

    $("#GroupId").val(person.GroupId);

    $("#Site").val(person.Site);
    $("#text_heardAbout").val(person.HeardAbout);
    $("#family_members").empty();
    $("#familyMemberTemplate")
            .tmpl(person.FamilyMembers)
            .appendTo("#family_members");
    familyMembers = person.FamilyMembers;
    if (!person.HasUsername) {
        $("#button_sendWelcomeMail").show();
    }
    else {
        $("#button_sendWelcomeMail").hide();
    }
    $("#div_saveSuccess").hide();
    if (person.FacebookId != null) {
        $("#img_person").prop("src", "https://graph.facebook.com/" + person.FacebookId + "/picture");
        $("#row_image").show();
    }
    else {
        $("#row_image").hide();
        $("#img_person").prop("src", " ");
    }

    try {
        $("#jqgEventList").jqGrid("setGridParam", { "postData": { personId: person.PersonId } });
        $("#jqgEventList").trigger("reloadGrid");
        $("#jqgCommentList").jqGrid("setGridParam", { "postData": { personId: person.PersonId } });
        $("#jqgCommentList").trigger("reloadGrid");
        ReloadGroups(person.PersonId);
        ReloadMessages(person.PersonId);
    }
    catch(err) {
        SendErrorEmail("Error updating jqgrids", err);
        
    }
}

function ReloadGroups(personId) {
    $("#jqgGroups").jqGrid("setGridParam", { "postData": { personId: personId } });
    $("#jqgGroups").trigger("reloadGrid");
    $("#jqgGroupsPersonIsIn").jqGrid("setGridParam", { "postData": { personId: personId } });
    $("#jqgGroupsPersonIsIn").trigger("reloadGrid");
    $("#jqgGroupsPersonIsNotIn").jqGrid("setGridParam", { "postData": { personId: personId } });
    $("#jqgGroupsPersonIsNotIn").trigger("reloadGrid");
}

function ReloadMessages(personId) {
    $("#jqgMessages").jqGrid("setGridParam", { "postData": { personId: personId } });
    $("#jqgMessages").trigger("reloadGrid");
}

function FetchPersonData(personId) {
    $("#ajax_personSearch").show();
    var postData = { personId: personId };

    var jqxhr = $.post("/Ajax/FetchPerson", $.postify(postData), function (data) {
        if (data.Person == null) {
            window.location = "/Home/Person";
        }
        else {
            PopulatePerson(data.Person);
        }
        $("#ajax_personSearch").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_personSearch").hide();
        SendErrorEmail("Error calling FetchPerson", jqXHR.responseText);
    });
}

function SavePersonAjaxCall(postData, refreshAfterSave) {

    var jqxhr = $.post("/Ajax/SavePerson", $.postify(postData), function(data) {
        if (data.PersonId == 0) {
            var errMessage = "There was a problem saving the person";
            ShowErrorMessage("Cannot save person", errMessage);
        } else {
            $("#hidden_personId").val(data.PersonId + "");
            $("#span_message").html("Person saved succesfully");
            $("#span_message").show();
            //Refetch the person
            if (refreshAfterSave) {
                FetchPersonData(data.PersonId);
            } else {
                $("#ajax_personSearch").hide();
            }
            pageIsDirty = false;
        }
    })
        .error(function(jqXHR, textStatus, errorThrown) {
            $("#ajax_personSearch").hide();
            SendErrorEmail("Error calling SavePerson", jqXHR.responseText);
        });
}

function SavePerson(refreshAfterSave) {
    var canSave = true;
    if ($("#text_firstname").val() == "") {
        canSave = false;
        $("#warning_firstname").show();
    }
    else {
        $("#warning_firstname").hide();
    }

    if ($("#text_surname").val() == "") {
        canSave = false;
        $("#warning_surname").show();
    }
    else {
        $("#warning_surname").hide();
    }

    if (!canSave) {
        return;
    }

    $("#ajax_personSearch").show();
    if (familyMembers != null) {
        $.each(familyMembers, function (index, value) {
            if (value.Relationship == null) {
                value.Relationship = "";
            }
        });
    }

    var groupId = $("#hidden_groupId").val();
    if ($("#row_groupDropDown").length > 0) {
        if ($('#GroupId option[value=' + $("#hidden_groupId").val() + ']').length != 0) {
            groupId = $("#GroupId").val();
        }
    }

    var roleId = $("#hidden_roleId").val();
    if ($("#row_roleDropDown").length > 0) {
        if ($('#RoleId option[value=' + $("#hidden_roleId").val() + ']').length != 0) {
            roleId = $("#RoleId").val();
        }
    }

    var postData = { PersonId: $("#hidden_personId").val(),
        FamilyId: $("#hidden_familyId").val(),
        Firstname: $("#text_firstname").val(),
        Surname: $("#text_surname").val(),
        Email: $("#text_email").val(),
        DateOfBirth_Value: $("#text_dateOfBirth").val(),
        Anniversary_Value: $("#text_anniversary").val(),
        HomePhone: $("#text_homePhone").val(),
        CellPhone: $("#text_cellPhone").val(),
        WorkPhone: $("#text_workPhone").val(),
        Skype: $("#text_skype").val(),
        Twitter: $("#text_twitter").val(),
        Occupation: $("#text_occupation").val(),
        Gender: $("input[name=Gender]:checked").val(),
        Address1: $("#text_address1").val(),
        Address2: $("#text_address2").val(),
        Address3: $("#text_address3").val(),
        Address4: $("#text_address4").val(),
        Lat: $("#hidden_lat").val(),
        Lng: $("#hidden_lng").val(),
        RoleId: roleId,
        Site: $("#Site").val(),
        FamilyMembers: familyMembers,
        HeardAbout: $("#text_heardAbout").val(),
        GroupId: groupId
    };

    SavePersonAjaxCall(postData, refreshAfterSave);

}

function LinkToFamily() {
    var postData = {
        personId: $("#hidden_personId").val(),
        familyId: $("#hidden_newFamilyId").val()
    };
    $.post("/Ajax/LinkPersonToFamily", $.postify(postData), function (data) {
        if (data.personId == 0) {
            var errMessage = "There was a problem linking the person to a new family";
            ShowErrorMessage("Cannot link person to new family", errMessage);
        } else {
            window.location = "/Home/Person?PersonId=" + data.personId;
        }
    })
        .error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling LinkPersonToFamily", jqXHR.responseText);
        });
}

function FetchFamilyMembers() {
    if ($("#hidden_familyId").val() != "0" &&
        $("#hidden_personId").val() != "0") {
        var postData = { personId: $("#hidden_personId").val(),
                         familyId: $("#hidden_familyId").val() };

                         var jqxhr = $.post("/Ajax/FetchFamilyMembers", $.postify(postData), function (data) {
                             $("#family_members").empty();
                             $("#familyMemberTemplate")
                                .tmpl(data.FamilyMembers)
                                .appendTo("#family_members");
                             familyMembers = data.FamilyMembers;
                         })
        .error(function (jqXHR, textStatus, errorThrown) { SendErrorEmail("Error calling FetchFamilyMembers", jqXHR.responseText); });
    }
}

function DeletePerson(){
    if($("#hidden_personId").val() != "0") {
        var postData = { PersonId: $("#hidden_personId").val() };

        var jqxhr = $.post("/Ajax/DeletePerson", $.postify(postData), function (data) {
            $("#span_message").show();
            if (data.Message == "") {
                $("#span_message").html("Person deleted succesfully");
                ClearForm();
                $("#family_members").empty();
                pageIsDirty = false;
            }
            else {
                $("#span_message").html(data.Message);
            }
        })
        .error(function (jqXHR, textStatus, errorThrown) { SendErrorEmail("Error calling DeletePerson", jqXHR.responseText); });
    }
}

function UpdateFamilyMemberRelationship(relatedPersonId, relationship) {
    $.each(familyMembers, function (index, value) {
        if (value.PersonId == relatedPersonId) {
            if (relationship == "Select...") {
                value.Relationship = "";
            }
            else {
                value.Relationship = relationship;
            }
        }
    });
}

function AddFamilyMember() {
    $("#span_message").hide();
    var familyId = $("#hidden_familyId").val();
    var surname = $("#text_surname").val();
    var homephone = $("#text_homePhone").val();
    var address1 = $("#text_address1").val();
    var address2 = $("#text_address2").val();
    var address3 = $("#text_address3").val();
    var address4 = $("#text_address4").val();
    var lat = $("#hidden_lat").val();
    var lng = $("#hidden_lng").val();
    var groupId = $("#hidden_groupId").val();
    //var roleId = $("#hidden_roleId").val();

    ClearForm();

    $("#RoleId").val($("#RoleId option:first").val());
    $("#hidden_roleId").val($("#RoleId option:first").val());
    $("#row_roleDropDown").show();
    $("#hidden_familyId").val(familyId);
    $("#hidden_groupId").val(groupId);
    $("#text_surname").val(surname);
    $("#text_homePhone").val(homephone);
    $("#text_address1").val(address1);
    $("#text_address2").val(address2);
    $("#text_address3").val(address3);
    $("#text_address4").val(address4);
    $("#hidden_lat").val(lat);
    $("#hidden_lng").val(lng);
}

function SendWelcomeMail() {
    if ($("#hidden_personId").val() == "0") {
        $("#span_message").html("Please save the person before sending them their username and password");
        $("#span_message").show();
        return;
    }

    var postData = { personId: $("#hidden_personId").val() };
    $("#ajax_personSearch").show();
    var jqxhr = $.post("/Ajax/SendEmailAndPassword", $.postify(postData), function (data) {
        $("#span_message").show();

        $("#span_message").html(data.Message);
        if (data.EmailSent) {
            $("#button_sendWelcomeMail").hide();
        }
        $("#ajax_personSearch").hide();
    })
    .error(function (jqXHR, textStatus, errorThrown) { SendErrorEmail("Error calling SendEmailAndPassword", jqXHR.responseText); });
}

PageAlert = {
    UnsavedChanges: function() {
        var dfr = $.Deferred();
        if (pageIsDirty && $("#text_surname").val() != '' && $("#text_firstname").val() != '') {
            $("#div_warningUnsavedChanges").dialog({
                autoOpen: true,
                modal: true,
                height: 200,
                width: 280,
                resizable: false,
                buttons: {
                    "Yes": function() {
                        $(this).dialog('close');
                        SavePerson(false);
                        dfr.resolve();
                    },
                    "No": function() {
                        $(this).dialog('close');
                        pageIsDirty = false;
                        dfr.resolve();
                    }
                }
            });
        } else {
            dfr.resolve();
        }

        return dfr.promise();
    }
};

function SetPrimaryGroup(groupId) {
    var postData = {
        groupId: groupId,
        personId: $("#hidden_personId").val()
    };

    var jqxhr = $.post("/Ajax/SetPrimaryGroup", $.postify(postData), function (data) {
        ReloadGroups($("#hidden_personId").val());
    })
    .error(function (jqXHR, textStatus, errorThrown) { SendErrorEmail("Error calling SetPrimaryGroup", jqXHR.responseText); });

}

var ViewMessage = function(rowid) {
    var rowData = $("#jqgMessages").jqGrid().getRowData(rowid);
    var messageType = rowData['MessageType'];
    var postData;
    if (messageType == 'Sms') {
        OpenSmsDialog();
        postData = { messageRecepientId: rowid };
        $.post("/Ajax/SetCellNosFromMessageRecepientId", $.postify(postData), function (data) {
            if (data.Message == "") {
                SetupSmsDialog(data.NoNumbers, data.Subject);
            }
            else {
                ShowErrorMessage("Error opening message", data.Message);
            }
        });
    } else {
        OpenEmailDialog();
        postData = { messageRecepientId: rowid };
        $.post("/Ajax/SetEmailAddressesFromMessageRecepientId", $.postify(postData), function (data) {
            if (data.Message == "") {
                SetupEmailDialog(data.Subject, data.Body);
            }
            else {
                ShowErrorMessage("Error opening message", data.Message);
            }
        });
    }
    
};

var CreateNewMessage = function(personId, messageType) {
    var postData;
    if (messageType == "Sms") {
        OpenSmsDialog();
        postData = { personId: personId };
        $.post("/Ajax/SetCellNoFromPersonId", $.postify(postData), function (data) {
            if (data.Message == "") {
                SetupSmsDialog(1);
            } else {
                ShowErrorMessage("Error opening message", data.Message);
            }
        });
    } else {
        OpenEmailDialog();
        postData = { personId: personId };
        $.post("/Ajax/SetEmailAddressesFromPersonId", $.postify(postData), function(data) {
            if (data.Message == "") {
                SetupEmailDialog();
            } else {
                ShowErrorMessage("Error opening message", data.Message);
            }
        });
    }
};

var pageIsDirty = false;
$(document).ready(function() {
    
    FetchFamilyMembers();

    //Set up masks

    $("#text_homePhone").mask("(999) 9999999");
    $("#text_cellPhone").mask("(999) 9999999");
    $("#text_workPhone").mask("(999) 9999999");

    $("input").change(function() {
        pageIsDirty = true;
    });

    $("select").change(function() {
        pageIsDirty = true;
    });

    $("#family_members").delegate("select", "change", function() {
        pageIsDirty = true;
    });

    $("#button_addPerson").click(function() {
        PageAlert.UnsavedChanges().then(function() {
            $("#span_message").hide();
            ClearForm()
            $("#family_members").empty();
            $("#text_personSearch").val('');
        });
    });

    $("#button_addFamilyMember").click(function() {
        PageAlert.UnsavedChanges().then(function() {
            AddFamilyMember();
        });
    });

    $("#button_addFamilyMember2").click(function() {
        PageAlert.UnsavedChanges().then(function() {
            AddFamilyMember();
        });
    });

    $("#family_members").delegate(".selectPerson", "mouseover mouseout", function(event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
            $(this).parent().addClass("ui-state-hover");
        } else {
            $(this).css("cursor", "default");
            $(this).parent().removeClass("ui-state-hover");
        }
    });

    $("#family_members").delegate(".select_relationship", "change", function(event) {
        UpdateFamilyMemberRelationship($.tmplItem(this).data.PersonId, $(this).val());
    });

    $("#family_members").delegate(".selectPerson", "click", function() {
        var personId = $.tmplItem(this).data.PersonId;
        PageAlert.UnsavedChanges().then(function() {
            $("#span_message").hide();
            $("#text_personSearch").val('');
            FetchPersonData(personId);
        });
    });

    $("#text_personSearch").focus(function() {
        PageAlert.UnsavedChanges().then(function() {
            //Don't have to do anything
        });
    });

    $("#button_search").click(function() {
        if ($("#text_personSearch").val().length > 0) {
            PageAlert.UnsavedChanges().then(function() {
                pageIsDirty = false;
                $("#ajax_personSearch").show();
                var postData = { term: $("#text_personSearch").val() };

                var jqxhr = $.post("/Ajax/PersonAutoComplete", $.postify(postData), function(data) {
                    $("#ajax_personSearch").hide();
                    if (data.length > 0) {
                        $("#hidden_personId").val(data[0] ? data[0].id : "0");
                        FetchPersonData($("#hidden_personId").val());
                    }
                }).error(function(jqXHR, textStatus, errorThrown) {
                    $("#ajax_familySearch").hide();
                    SendErrorEmail("Error calling PersonAutoComplete", jqXHR.responseText);
                });
            });
        }
    });

    $("#text_personSearch").autocomplete({
        source: function(request, response) {
            //At this point - need to make sure that alert doesn't fire
            pageIsDirty = false;
            $("#span_message").hide();
            $("#ajax_personSearch").show();
            ClearForm();
            $("#family_members").empty();
            var postData = { term: request.term };

            var jqxhr = $.post("/Ajax/PersonAutoComplete", $.postify(postData), function(data) {
                $("#ajax_personSearch").hide();
                response(data);
            }).error(function(jqXHR, textStatus, errorThrown) {
                $("#ajax_familySearch").hide();
                SendErrorEmail("Error calling PersonAutoComplete", jqXHR.responseText);
            });
        },
        minLength: 1,
        select: function(event, ui) {
            PageAlert.UnsavedChanges().then(function() {
                $("#hidden_personId").val(ui.item ? ui.item.id : "0");
                FetchPersonData($("#hidden_personId").val());
            });
        }
    });

    $("#text_newFamilySearch").autocomplete({
        source: function(request, response) {
            var postData = { term: request.term };
            var jqxhr = $.post("/Ajax/FamilyAutoComplete", $.postify(postData), function(data) {
                response(data);
            }).error(function(jqXHR, textStatus, errorThrown) {
                SendErrorEmail("Error calling FamilyAutoComplete", jqXHR.responseText);
            });
        },
        minLength: 1,
        select: function(event, ui) {
            $("#hidden_newFamilyId").val(ui.item.id);
        }
    });

    $("#text_dateOfBirth").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd MM yy',
        yearRange: '-120:+0'
    });

    $("#text_anniversary").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd MM yy',
        yearRange: '-120:+0'
    });

    $("#button_savePerson").click(function() {
        $("#span_message").hide();
        SavePerson(true);
    });

    $("#edit_Groups").click(function() {
        if ($("#hidden_personId").val() == "0") {
            ShowErrorMessage("Cannot edit groups", "Please save person before editing groups");
            return;
        }
        $("#div_editGroups").dialog('open');
    });

    $("#div_editGroups").dialog({
        autoOpen: false,
        modal: true,
        height: 640,
        width: 700,
        resizable: false,
        buttons: {
            "Done": function () {
                $(this).dialog('close');
            }
        }
    });

    $("#button_deletePerson").click(function () {
        if ($("#hidden_personId").val() != "0") {
            $("#div_warningDelete").dialog('open');
        }
    });

    $("#button_linkPersonToNewFamily").click(function () {
        $("#text_newFamilySearch").val('');
        $("#hidden_newFamilyId").val('0');
        if ($("#hidden_personId").val() != "0") {
            $("#div_linkPersonToAnotherFamily").dialog('open');
        }
    });

    $("#div_warningDelete").dialog({
        autoOpen: false,
        modal: true,
        height: 200,
        width: 280,
        resizable: false,
        buttons: {
            "Yes": function() {
                $(this).dialog('close');
                DeletePerson();
            },
            "No": function() {
                $(this).dialog('close');
            }
        }
    });
    
    $("#div_linkPersonToAnotherFamily").dialog({
        autoOpen: false,
        modal: true,
        height: 300,
        width: 580,
        resizable: false,
        buttons: {
            "Link to family": function () {
                if ($("#hidden_newFamilyId").val() != '0') {
                    LinkToFamily();
                    $(this).dialog('close');
                }
            },
            "Create new family": function () {
                $(this).dialog('close');
            },
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });

    $("#text_address1").keypress(function() {
        $("#hidden_addressChosen").val("");
    });

    $("#text_address1").autocomplete({
        source: function(request, response) {
            $("#ajax_gpsCoordinates").show();
            var address = $("#text_address1").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function(data) {
                $("#ajax_gpsCoordinates").hide();
                response(data);
            });
        },
        minLength: 4,
        select: function(event, ui) {
            AddressSelected(ui.item.id, 2, '', false);
        },
        open: function() {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function() {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#text_address2").autocomplete({
        source: function(request, response) {
            if ($("#hidden_addressChosen").val() == "selected") {
                response("");
                return;
            }
            $("#ajax_gpsCoordinates2").show();
            var address = $("#text_address2").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function(data) {
                $("#ajax_gpsCoordinates2").hide();
                response(data);
            });
        },
        minLength: 4,
        select: function(event, ui) {
            AddressSelected(ui.item.id, 3, '', false);
        },
        open: function() {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function() {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#button_sendWelcomeMail").click(function() {
        SendWelcomeMail();
    });

    $('#jqgEventList').jqGrid({
        url: '/Ajax/FetchEventListForPerson/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['PersonId', 'Date', 'Event', 'Created By'],
        colModel: [
            { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
            { name: 'Date', index: 'Date', align: 'left', width: 100, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } },
            { name: 'Event', index: 'Event', align: 'left', width: 427, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;' } },
            { name: 'CreatedBy', index: 'CreatedBy', align: 'left', width: 120, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } }
        ],
        pager: $('#jqgpEventList'),
        rowNum: 20,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto'
    }).navGrid('#jqgpEventList', { edit: false, add: false, del: false, search: false });


    $('#jqgCommentList').jqGrid({
        url: '/Ajax/FetchCommentListForPerson/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['CommentId', 'Date', 'Comment', 'Created By'],
        colModel: [
            { name: 'CommentId', index: 'PersonId', hidden: true, search: false },
            { name: 'Date', index: 'Date', align: 'left', width: 100, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } },
            { name: 'Comment', index: 'Comment', align: 'left', width: 437, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal"' } },
            { name: 'CreatedBy', index: 'CreatedBy', align: 'left', width: 120, search: true, cellattr: function(rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } }
        ],
        pager: $('#jqgpCommentList'),
        rowNum: 20,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto'
    }).navGrid('#jqgpCommentList', { edit: false, add: false, del: false, search: false });

    $('#jqgGroups').jqGrid({
        url: '/Ajax/FetchGroupsForPerson/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['GroupId', 'Name', 'Type', 'Last Attended', 'Leader', 'Administrator', 'Primary'],
        colModel: [
            { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
            { name: 'Name', index: 'Name', align: 'left', width: 160, search: true },
            { name: 'Type', index: 'Type', align: 'left', width: 120, search: true },
            { name: 'LastAttended', index: 'LastAttended', align: 'left', width: 100, search: true },
            { name: 'Leader', index: 'Leader', align: 'left', width: 150, search: true },
            { name: 'Administrator', index: 'Administrator', align: 'left', width: 150, search: true },
            { name: 'PrimaryGroup', index: 'PrimaryGroup', align: 'left', width: 60, search: false }
        ],
        pager: $('#jqgpGroups'),
        rowNum: 20,
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function(rowid, iRow, iCol, e) {
            window.location = "/Home/Homegroups?groupId=" + rowid;
        }
    }).navGrid('#jqgpGroups', { edit: false, add: false, del: false, search: false });
    
    $('#jqgGroupsPersonIsIn').jqGrid({
        url: '/Ajax/FetchGroupsPersonIsIn/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['GroupId', 'Groups Person Is In', 'Primary'],
        colModel: [
            { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
            { name: 'GroupName', index: 'GroupName', align: 'left', width: 160, search: true },
            { name: 'PrimaryGroup', index: 'PrimaryGroup', align: 'left', width: 60, search: false }
        ],
        pager: $('#jqgpGroupsPersonIsIn'),
        rowNum: 20,
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            SetPrimaryGroup(rowid);
        },
        loadComplete: function(data) {
            var rowIds = $("#jqgGroupsPersonIsIn").getDataIDs();
            if (rowIds.length == 0) {
                $("#group_Name").text("None");
                $("#hidden_groupId").val(0);
            } else {
                for (var rowId = 0; rowId < rowIds.length; rowId++) {
                    var rowData = $("#jqgGroupsPersonIsIn").getRowData(rowIds[rowId]);
                    if (rowData.PrimaryGroup == "True") {
                        $("#group_Name").text(rowData.GroupName);
                        $("#hidden_groupId").val(rowIds[rowId]);
                    }
                }
            }
        }
    }).navGrid('#jqgpGroupsPersonIsIn', { edit: false, add: false, del: false, search: false })
        .navButtonAdd('#jqgpGroupsPersonIsIn', {
            caption: "",
            title: "Set selected group as primary care group",
            buttonicon: "ui-icon-home",
            onClickButton: function () {
                SetPrimaryGroup($('#jqgGroupsPersonIsIn').jqGrid('getGridParam', 'selrow'));
            },
            position: "first"
        });
    
    $('#jqgGroupsPersonIsNotIn').jqGrid({
        url: '/Ajax/FetchGroupsPersonIsNotIn/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['GroupId', 'Groups Person Is Not In'],
        colModel: [
            { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
            { name: 'GroupName', index: 'GroupName', align: 'left', width: 260, search: true }
        ],
        pager: $('#jqgpGroupsPersonIsNotIn'),
        rowNum: 20,
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto'
    }).navGrid('#jqgpGroupsPersonIsNotIn', { edit: false, add: false, del: false, search: false });

    $('#jqgMessages').jqGrid({
        url: '/Ajax/FetchMessagesForPerson/',
        datatype: 'json',
        mtype: 'POST',
        postData: { personId: $("#hidden_personId").val() },
        colNames: ['MessageId', 'Subject', 'Sent', 'Status', 'Type', 'Sent By'],
        colModel: [
            { name: 'MessageId', index: 'MessageId', hidden: true, search: false },
            { name: 'Subject', index: 'Subject', align: 'left', width: 300, search: true },
            { name: 'Sent', index: 'Sent', align: 'left', width: 90, search: true },
            { name: 'Status', index: 'Status', align: 'left', width: 90, search: true },
            { name: 'MessageType', index: 'MessageType', align: 'left', width: 80, search: true },
            { name: 'SentBy', index: 'SentBy', align: 'left', width: 150, search: true }
        ],
        pager: $('#jqgpMessages'),
        rowNum: 20,
        sortname: 'Sent',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            ViewMessage(rowid);
        },
    }).navGrid('#jqgpMessages', { edit: false, add: false, del: false, search: false });

    $("#saveComment_button").click(function() {
        var postData = {
            personId: $("#hidden_personId").val(),
            comments: []
        };

        if ($("#comment_detail").val() != "") {
            postData.comments.push($("#comment_detail").val());
        }
        
        if ($("#hidden_personId").val() == "0") {
            ShowErrorMessage("Cannot save comment", "Please save person before adding comments");
            return;
        }

        if (postData.comments.length > 0) {
            $.post("/Ajax/SaveComments", $.postify(postData), function (data) {
                $("#comment_detail").val("");
                $("#jqgCommentList").trigger("reloadGrid");
            }).error(function (jqXhr) {

                SendErrorEmail("Error calling SaveComments", jqXhr.responseText);
            });
        }
    });

    $("#addToGroup").click(function() {
        var selr = $('#jqgGroupsPersonIsNotIn').jqGrid('getGridParam', 'selrow');
        if (selr == null) {
            ShowErrorMessage("No group selected", "Please select a group to add the person to");
            return;
        }
        
        var postData = {
            groupId: selr,
            personId: $("#hidden_personId").val()
        };

        $.post("/Ajax/AddPersonToGroup", $.postify(postData), function (data) {
            ReloadGroups($("#hidden_personId").val());
        }).error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling Add To Group", jqXHR.responseText);
        });
    });
    
    $("#removeFromGroup").click(function () {
        var selr = $('#jqgGroupsPersonIsIn').jqGrid('getGridParam', 'selrow');
        if (selr == null) {
            ShowErrorMessage("No group selected", "Please select a group to remove the person from");
            return;
        }

        var postData = {
            groupId: selr,
            personId: $("#hidden_personId").val()
        };

        $.post("/Ajax/RemovePersonFromGroup", $.postify(postData), function (data) {
            ReloadGroups($("#hidden_personId").val());
        }).error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling Remove from Group", jqXHR.responseText);
        });
    });

    $("#cancelComment_button").click(function () {
        $("#comment_detail").val("");
    });

    $("#email_sendNew").button({ icons: { primary: "ui-icon-mail-closed" } })
        .click(function () {
            CreateNewMessage($("#hidden_personId").val(), "Email");
        });
    
    $("#sms_sendNew").button({ icons: { primary: "ui-icon-comment" } })
    .click(function () {
        CreateNewMessage($("#hidden_personId").val(), "Sms");
    });

});




