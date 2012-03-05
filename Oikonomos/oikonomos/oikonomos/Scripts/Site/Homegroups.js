var homeGroups;
var members;
var visitors;
var rowId = 0;
var selectedGroupId = 0;

function SaveHomeGroup() {
    $("#message").html("");
    var postData = { GroupId: $("#hidden_homeGroupId").val(),
        HomeGroupName: $("#text_homeGroupName").val(),
        LeaderId: $("#hidden_homeGroupLeaderId").val(),
        LeaderName : $("#text_homeGroupLeader").val(),
        AdministratorId: $("#hidden_homeGroupAdministratorId").val(),
        AdministratorName: $("#text_homeGroupAdministrator").val(),
        AddressId: $("#hidden_addressId").val(),
        Address1: $("#text_address1").val(),
        Address2: $("#text_address2").val(),
        Address3: $("#text_address3").val(),
        Address4: $("#text_address4").val(),
        AddressType: $("#hidden_addressType").val(),
        Lat: $("#hidden_lat").val(),
        Lng: $("#hidden_lng").val(),
        GroupClassification: $("#select_hgClassification").val(),
        Suburb: $("#select_hgSuburb").val()
    };

    $.post("/Ajax/SaveHomeGroup", $.postify(postData), function (data) {
        FetchHomeGroupList($("#div_groupId")[0].innerText);
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        alert(jqXHR.responseText);
    });
}

function FetchHomeGroupList(groupId) {
    $("#message").html("");
    //Fetch the list
    var jqxhr = $.post("/Ajax/FetchHomeGroups", function (data) {
        homeGroups = data.HomeGroups;
        $("#homeGroupList").empty();
        $("#homeGroupTemplate")
                .tmpl(homeGroups)
                .appendTo("#homeGroupList");
        if (homeGroups.length == 0) {
            $("#homeGroupName").html("No Homegroups to Display");
        }
        else {
            $("#homeGroupName").html(homeGroups[0].HomeGroupName);
            selectedGroupId = homeGroups[0].GroupId;
            if (groupId != 0) {
                $.each(homeGroups, function () {
                    if (this.GroupId == groupId) {
                        selectedGroupId = groupId;
                        $("#homeGroupName").html(this.HomeGroupName);
                        return false;
                    }
                });
            }

            PopulatePeople(selectedGroupId, true);
        }
        $("#ajax_loader").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        alert(jqXHR.responseText);
    });
}

function FetchGroupClassifications() {
    var optionList = [{ GroupClassificationId: 0, GroupClassification: "Select..."}];
    $.post("/Ajax/FetchGroupClassifications", function (data) {
        $("#select_hgClassification").empty();
        $("#groupClassificationTemplate")
                .tmpl($.merge(optionList, data.GroupClassifications))
                .appendTo("#select_hgClassification");
    }).error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function FetchSuburbs() {
    var optionList = [{ SuburbId: 0, SuburbName: "Select..."}];
    $.post("/Ajax/FetchSuburbs", function (data) {
        $("#select_hgSuburb").empty();
        $("#suburbTemplate")
                .tmpl($.merge(optionList, data.Suburbs))
                .appendTo("#select_hgSuburb");
    }).error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function FetchEventTypes() {
    var postData = { eventFor: 'Group' };
    $.post("/Ajax/FetchEventTypes", $.postify(postData), function (data) {
        $("#eventsList").empty();
        $("#eventTypeTemplate")
                .tmpl(data.EventTypes)
                .appendTo("#eventsList");
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function PopulatePeopleLists(data) {
    members = data.Members;
    visitors = data.Visitors;
    $("#membersList").empty();
    $("#visitorsList").empty();
    $("#membersListAttendance").empty();
    $("#visitorsListAttendance").empty();
    $("#memberTemplate")
                .tmpl(members)
                .appendTo("#membersList");
    if (visitors.length > 0) {
        $("#visitorsHeader").show();
        $("#visitorsHeaderAttendance").show();
        $("#visitorTemplate")
                .tmpl(visitors)
                .appendTo("#visitorsList");
        $("#attendanceTemplate")
                .tmpl(visitors)
                .appendTo("#visitorsListAttendance");
    }
    else {
        $("#visitorsHeader").hide();
        $("#visitorsHeaderAttendance").hide();
    }
    $("#attendanceTemplate")
                .tmpl(members)
                .appendTo("#membersListAttendance");

    FetchAttendance();
    $("#ajax_loader").hide();
}

function FetchPeople(homeGroupId, onLoad) {
    $("#message").html("");
    var postData = { groupId: homeGroupId };

    var jqxhr = $.post("/Ajax/FetchPeopleInHomeGroup", $.postify(postData), function (data) {
        PopulatePeopleLists(data);
        if (!onLoad) {
            if ($("#tabs").tabs("length") > 2) {
                $("#tabs").tabs("select", 1);
            }
            else {
                $("#tabs").tabs("select", 0);
            }
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        alert(jqXHR.responseText);
    });
}

function PopulatePeople(homeGroupId, onLoad) {
    $("#message").html("");
    $("#ajax_loader").show();
    //Get the people
    FetchPeople(homeGroupId, onLoad);
    $("#table_people").show();
    $("#button_addPerson").show();
}

function AddPersonToHomeGroup(personId, homeGroupId) {
    $("#message").html("");
    $("#message_hg").html("Adding person");
    $("#ajax_loader_hg").show();
    if (personId == "0") {
        //Save the new person
        var names = $("#text_personName").val().split(" ");
        var firstname = "";
        var surname = "";
        if (names.length == 0) {
            //There's a problem, just return
            $("#message").html("Invalid name");
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
            return;
        }
        else if (names.length == 1) {
            //Need a firstname and a surname
            $("#message").html("You need a firstname and a surname");
            return;
        }
        else {
            $.each(names, function (index) {
                if (index > 0) {
                    surname += names[index];
                    if (index < names.length - 1) {
                        surname += " ";
                    }
                }
                else {
                    firstname = names[index];
                }
            });
        }

        var securityRole = "Visitor";
        if ($("#addPerson_securityRole").length > 0) {
            securityRole = $("#addPerson_securityRole").val()
        }

        var personData = { PersonId: personId,
            FamilyId: 0,
            Firstname: firstname,
            Surname: surname,
            Email: "",
            DateOfBirth_Value: "",
            HomePhone: "",
            CellPhone: "",
            WorkPhone: "",
            Skype: "",
            Twitter: "",
            Occupation: "",
            Address1: "",
            Address2: "",
            Address3: "",
            Address4: "",
            Lat: "",
            Lng: "",
            FindFamily: true,
            RoleName: securityRole,
            GroupId: homeGroupId
        };

        $.post("/Ajax/SavePerson", $.postify(personData), function (data) {
            AddPersonToHomeGroup(data.PersonId, homeGroupId);
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        });
    }
    else {

        var postData = { groupId: homeGroupId, personId: personId };

        $.post("/Ajax/AddPersonToHomeGroup", $.postify(postData), function (data) {
            PopulatePeopleLists(data);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        }).error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_loader").hide();
            alert(jqXHR.responseText);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        });
    }
}

function RemovePersonFromHomeGroup(personId, homeGroupId) {
    $("#message").html("");
    $("#ajax_loader_hg").show();
    var postData = { groupId: homeGroupId, personId: personId };

    var jqxhr = $.post("/Ajax/RemovePersonFromHomeGroup", $.postify(postData), function (data) {
        PopulatePeopleLists(data);
        $("#ajax_loader_hg").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader_hg").hide();
        alert(jqXHR.responseText);
    });
}

function SetLeader(personId, homeGroupId) {
    var postData = { groupId: homeGroupId, leaderId: personId };

    $.post("/Ajax/SetHomeGroupLeader", $.postify(postData));
}

function SetAdministrator(personId, homeGroupId) {
    var postData = { groupId: homeGroupId, administratorId: personId };

    $.post("/Ajax/SetHomeGroupAdministrator", $.postify(postData));
}

function DeleteHomeGroup(homeGroupId) {
    $("#message").html("");    
    var postData = { groupId: homeGroupId};

    var jqxhr = $.post("/Ajax/DeleteHomeGroup", $.postify(postData), function (data) {
        $("#message").html(data.Message);
        if (data.Success) {
            members = null;
            visitors = null;
            $("#peopleList").empty();
            selectedGroupId = 0;
            $("#table_people").hide();
            $("#button_addPerson").hide();
            homeGroups = data.HomeGroups;
            $("#homeGroupList").empty();
            $("#homeGroupTemplate")
                    .tmpl(homeGroups)
                    .appendTo("#homeGroupList");
            $("#ajax_loader").hide();
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        alert(jqXHR.responseText);
    });
}

function SaveLeaveEvents(personId) {
    $("#ajax_loader_hg").show();
    var postData = { PersonId: personId,
        Events: []
    };

    var currentDate = new Date();
    var currentDateFormat = currentDate.getFullYear() + '/' + (currentDate.getMonth() + 1) + '/' + (currentDate.getDay());
    $.each($("#add_LeaveEvent input:checked"), function (index, value) {
        if (value.id == "checkbox_leaveOther" && $("#text_leaveOther").val() != "") {
            postData.Events.push({ 
                Name: $("#text_leaveOther").val(),
                Date: currentDateFormat,
                GroupId: selectedGroupId
            });
        }
        else {
            postData.Events.push({ 
                Name: $(this).val(),
                Date: currentDateFormat,
                GroupId: selectedGroupId
            });
        }
    });

    if (postData.Events.length > 0) {
        var jqxhr = $.post("/Ajax/SavePersonEvents", $.postify(postData), function (data) {
            $("#ajax_loader_hg").hide();
        }).error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_loader_hg").hide();
            alert(jqXHR.responseText);
        });
    }
    else {
        $("#ajax_loader_hg").hide();
    }
}

function SaveEvents() {
    $("#ajax_loader").show();
    var postData = { PersonId: $("#hidden_eventPersonId").val(),
        Events: []
    };

    $.each($("#add_Event input:checked"), function (index, value) {
        if (value.id == "checkbox_other" && $("#text_other").val()!="") {
            postData.Events.push({ Name: $("#text_other").val(),
                Date: $("#text_eventDate").val(),
                GroupId: 0
            });
        }
        else {
            postData.Events.push({ Name: $(this).val(),
                Date: $("#text_eventDate").val(),
                GroupId: 0
            });
        }
    });

    if (postData.Events.length > 0) {
        var jqxhr = $.post("/Ajax/SavePersonEvents", $.postify(postData), function (data) {
            $("#ajax_loader").hide();
        }).error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_loader").hide();
            alert(jqXHR.responseText);
        });
    }
    else {
        $("#ajax_loader_hg").hide();
    }
}

function SaveAttendance() {
    $("#ajax_loader_attendance").show();
    var postData = { GroupId: selectedGroupId,
        EventDate: $("#text_eventDate").val(),
        Events: []
    };

    $.each($("#membersListAttendance input.checkbox_didNotAttended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
            IsVisitor: false,
            Events: [{ Name: 'Did not attend Group',
                Date: $("#text_eventDate").val(),
                GroupId: selectedGroupId
            }]
        });
    });

    $.each($("#visitorsListAttendance input.checkbox_didNotAttended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
            IsVisitor: true,
            Events: [{ Name: 'Did not attend Group',
                Date: $("#text_eventDate").val(),
                GroupId: selectedGroupId
            }]
        });
    });

    $.each($("#membersListAttendance input.checkbox_attended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
            IsVisitor: false,
            Events: [{ Name: 'Attended Group',
                Date: $("#text_eventDate").val(),
                GroupId: selectedGroupId
            }]
        });
    });

    $.each($("#visitorsListAttendance input.checkbox_attended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
            IsVisitor: true,
            Events: [{ Name: 'Attended Group',
                Date: $("#text_eventDate").val(),
                GroupId: selectedGroupId
            }]
        });
    });

    var jqxhr = $.post("/Ajax/SaveHomeGroupEvent", $.postify(postData), function (data) {
        $("#ajax_loader_attendance").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader_attendance").hide();
        alert(jqXHR.responseText);
    });
}

function UpdateAttendance(tableSelector, data) {
    //Go through the data and populate the attendance
    $(tableSelector).each(function (index) {
        personId = $.tmplItem(this).data.PersonId;
        var row = this;
        var found = false;
        $.each(data.Attendance, function (index) {
            if (this.PersonId == personId) {
                found = true;
                if (this.Attended) {
                    var attended = row.children[2].children[0];
                    attended.checked = true;
                }
                else {
                    var didNotAttend = row.children[3].children[0];
                    didNotAttend.checked = true;
                }
            }
        });
        if (!found) {
            var didNotAttend = row.children[3].children[0];
            didNotAttend.checked = true;
        }
    });
}

function SaveComment() {
        var postData = { personId: $("#hidden_commentPersonId").val(),
                         comment: $("#comment_detail").val() };

        var jqxhr = $.post("/Ajax/SavePersonComment", $.postify(postData), function (data) {
            $("#ajax_loader").hide();
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
            $("#ajax_loader").hide();
            alert(jqXHR.responseText);
        });
}

function FetchAttendance() {
    //Fetch the attendance on that day
    var postData = { groupId: selectedGroupId,
        date: $("#text_eventDate").val()
    };

    var jqxhr = $.post("/Ajax/FetchAttendance", $.postify(postData), function (data) {
        UpdateAttendance("#membersListAttendance .tableRow", data);
        UpdateAttendance("#visitorsListAttendance .tableRow", data);
    });
}

function FetchEmailList(includeVisitors) {
    var postData = { groupId: selectedGroupId,
                     includeVisitors: includeVisitors };

    OpenEmailDialog();
    var jqxhr = $.post("/Ajax/FetchGroupEmails", $.postify(postData), function (data) {
        if (data.Message == "") {
            SetEmailList();
        }
        else {
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
        }
    });
}

function ShowLeaveEvents(personId) {
    $(".checkbox_eventType").prop("checked", false);
    $("#text_leaveOther").val("");
    $("#add_LeaveEvent").dialog(
    {
        modal: true,
        height: 240,
        width: 350,
        resizable: false,
        buttons: {
            "Remove Person from Group": function () {
                $("#ajax_loader").show();
                SaveLeaveEvents(personId);
                RemovePersonFromHomeGroup(personId, selectedGroupId);
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

$(document).ready(function () {
    FetchHomeGroupList($("#div_groupId")[0].innerText);
    FetchSuburbs();
    FetchEventTypes();
    FetchGroupClassifications();

    $("#ajax_loader").hide();
    $("#ajax_loader_hg").hide();
    $("#button_addHomeGroup").show();
    $("#ajax_loader_attendance").hide();
    $("#button_saveAttendance").show();
    $("#button_printAttendance").show();


    $("#text_eventDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd MM yy',
        yearRange: '-2:+1',
        onSelect: function (date, instance) {
            FetchAttendance();
        }
    });

    $("#homeGroupList").delegate(".selectHomeGroup", "mouseover mouseout", function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
            $(this).parent().addClass("ui-state-hover");
        } else {
            $(this).css("cursor", "default");
            $(this).parent().removeClass("ui-state-hover");
        }
    });

    $("#homeGroupList").delegate(".selectHomeGroup", "click", function () {
        selectedGroupId = $.tmplItem(this).data.GroupId;
        $("#homeGroupName").html($.tmplItem(this).data.HomeGroupName);
        PopulatePeople(selectedGroupId, false);
    });

    $("#table_people").delegate(".selectPerson", "mouseover mouseout", function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
            $(this).parent().addClass("ui-state-hover");
        } else {
            $(this).css("cursor", "default");
            $(this).parent().removeClass("ui-state-hover");
        }
    });

    $("#table_people").delegate(".selectPerson", "click", function () {
        selectedPersonId = $.tmplItem(this).data.PersonId;
        window.location.replace("/Home/Person?PersonId=" + selectedPersonId + "&GroupId=" + selectedGroupId);
    });

    $("#table_peopleAttendance").delegate(".selectPerson", "mouseover mouseout", function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
            $(this).parent().addClass("ui-state-hover");
        } else {
            $(this).css("cursor", "default");
            $(this).parent().removeClass("ui-state-hover");
        }
    });

    $("#table_peopleAttendance").delegate(".selectPerson", "click", function () {
        selectedPersonId = $.tmplItem(this).data.PersonId;
        window.location.replace("/Home/Person?PersonId=" + selectedPersonId);
    });

    $("#homeGroupList").delegate(".editHomeGroup", "click", function () {
        //Populate fields
        $("#hidden_homeGroupId").val($.tmplItem(this).data.GroupId);
        $("#text_homeGroupName").val($.tmplItem(this).data.HomeGroupName);
        $("#text_homeGroupLeader").val($.tmplItem(this).data.LeaderName);
        $("#hidden_homeGroupLeaderId").val($.tmplItem(this).data.LeaderId);
        $("#text_homeGroupAdministrator").val($.tmplItem(this).data.AdministratorName);
        $("#hidden_homeGroupAdministratorId").val($.tmplItem(this).data.AdministratorId);
        $("#text_address1").val($.tmplItem(this).data.Address1);
        $("#text_address2").val($.tmplItem(this).data.Address2);
        $("#text_address3").val($.tmplItem(this).data.Address3);
        $("#text_address4").val($.tmplItem(this).data.Address4);
        $("#hidden_addressType").val($.tmplItem(this).data.AddressType);
        $("#hidden_addressId").val($.tmplItem(this).data.AddressId);
        $("#hidden_lat").val($.tmplItem(this).data.Lat);
        $("#hidden_lng").val($.tmplItem(this).data.Lng);
        var groupClassification = $.tmplItem(this).data.GroupClassification;
        if (groupClassification == "") {
            groupClassification = "Select...";
        }
        $("#select_hgClassification").val(groupClassification);
        var suburb = $.tmplItem(this).data.Suburb;
        if (suburb == "") {
            suburb = "Select...";
        }
        $("#select_hgSuburb").val(suburb);

        $("#edit_homeGroup").dialog(
        {
            modal: true,
            height: 400,
            width: 440,
            resizable: false,
            buttons: {
                "Save": function () {
                    $("#ajax_loader").show();
                    rowId = 0;
                    SaveHomeGroup();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }

        })
    });

    $("#text_homeGroupLeader").autocomplete({
        source: "/Ajax/PersonAutoComplete",
        minLength: 1,
        select: function (event, ui) {
            $("#hidden_homeGroupLeaderId").val(ui.item ? ui.item.id : "0");
        }
    });

    $("#text_homeGroupAdministrator").autocomplete({
        source: "/Ajax/PersonAutoComplete",
        minLength: 1,
        select: function (event, ui) {
            $("#hidden_homeGroupAdministratorId").val(ui.item ? ui.item.id : "0");
        }
    });


    $("#text_personName").autocomplete({
        source: function (request, response) {
            $("#ajax_loader_addPerson").show();
            $("#hidden_personId").val("0");
            $("#addPerson_securityRole").prop('disabled', false);
            var postData = { term: request.term };

            var jqxhr = $.post("/Ajax/PersonAutoComplete", $.postify(postData), function (data) {
                $("#ajax_loader_addPerson").hide();
                response(data);
            }).error(function (jqXHR, textStatus, errorThrown) {
                $("#ajax_loader_addPerson").hide();
                alert(jqXHR.responseText);
            });
        }
        ,
        minLength: 1,
        select: function (event, ui) {
            $("#hidden_personId").val(ui.item ? ui.item.id : "0");
            $("#addPerson_securityRole").val("Member");
            $("#addPerson_securityRole").prop('disabled', true);
        }
    });

    $("#button_addHomeGroup").click(function () {
        //Populate fields
        $("#hidden_homeGroupId").val("0");
        $("#text_homeGroupName").val("");
        $("#text_homeGroupLeader").val("");
        $("#hidden_homeGroupLeaderId").val("0");
        $("#text_homeGroupAdministrator").val("");
        $("#hidden_homeGroupAdministratorId").val("0");
        $("#text_homeGroupAddress").val("");

        $("#edit_homeGroup").dialog(
        {
            modal: true,
            height: 350,
            width: 440,
            resizable: false,
            buttons: {
                "Save": function () {
                    $("#ajax_loader").show();
                    rowId = 0;
                    SaveHomeGroup();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }

        })
    });

    $("#table_peopleAttendance").delegate(".addEvent", "click", function () {
        //Populate fields
        $("#hidden_eventPersonId").val($.tmplItem(this).data.PersonId);
        $("#add_Event input:checkbox").prop("checked", false);

        $("#add_Event").dialog(
        {
            modal: true,
            height: 450,
            width: 425,
            resizable: false,
            buttons: {
                "Save": function () {
                    $("#ajax_loader").show();
                    SaveEvents();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    });

    $("#table_peopleAttendance").delegate(".addComment", "click", function () {
        //Populate fields
        $("#previous_commentsList").empty();
        $("#hidden_commentPersonId").val($.tmplItem(this).data.PersonId);
        $("#comment_detail").val("");
        $("#ajax_loader_comment").show();
        var postData = { personId: $.tmplItem(this).data.PersonId };

        var jqxhr = $.post("/Ajax/FetchPersonCommentHistory", $.postify(postData), function (data) {
            if (data.Comments.length == 0) {
                $("#previous_comments").hide();
            }
            else {
                $("#previous_comments").show();
                $("#previousCommentsTemplate")
                    .tmpl(data.Comments)
                    .appendTo("#previous_commentsList");
            }
            $("#ajax_loader_comment").hide();
        });

        $("#add_Comment").dialog(
        {
            modal: true,
            height: 500,
            width: 500,
            resizable: false,
            buttons: {
                "Save": function () {
                    $("#ajax_loader").show();
                    SaveComment();
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        })
    });

    $("#button_saveAttendance").click(function () {
        SaveAttendance();
    });

    $("#button_addPerson").click(function () {
        //Populate fields
        $("#hidden_personId").val("0");
        $("#text_personName").val("");
        $("#addPerson_securityRole").prop('disabled', false);
        $("#addPerson_securityRole").val("Visitor");
        $("#add_Person").dialog(
        {
            modal: true,
            height: 180,
            width: 440,
            resizable: false,
            buttons: {
                "Add Person": function () {
                    $("#ajax_loader").show();
                    rowId = 0;
                    AddPersonToHomeGroup($("#hidden_personId").val(), selectedGroupId);
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        })
    });

    $("#button_addPersonAttendance").click(function () {
        //Populate fields
        $("#hidden_personId").val("0");
        $("#text_personName").val("");

        $("#add_Person").dialog(
        {
            modal: true,
            height: 150,
            width: 440,
            resizable: false,
            buttons: {
                "Add Person": function () {
                    $("#ajax_loader").show();
                    rowId = 0;
                    AddPersonToHomeGroup($("#hidden_personId").val(), selectedGroupId);
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    });


    $("#button_printList").click(function () {
        window.location.replace("/Report/HomeGroupList/" + selectedGroupId);
    });

    $("#button_printAttendance").click(function () {
        window.location.replace("/Report/HomeGroupAttendance/" + selectedGroupId);
    });

    $("#table_people").delegate(".removePerson", "click", function () {
        ShowLeaveEvents($.tmplItem(this).data.PersonId);
    });

    $("#table_peopleAttendance").delegate(".removePerson", "click", function () {
        ShowLeaveEvents($.tmplItem(this).data.PersonId);
    });

    $("#membersList").delegate(".radio_leader", "click", function () {
        personId = $.tmplItem(this).data.PersonId;
        SetLeader(personId, selectedGroupId);
    });

    $("#membersList").delegate(".radio_administrator", "click", function () {
        personId = $.tmplItem(this).data.PersonId;
        SetAdministrator(personId, selectedGroupId);
    });

    $("#homeGroupList").delegate(".deleteHomeGroup", "click", function () {
        selectedGroupId = $.tmplItem(this).data.GroupId;
        DeleteHomeGroup(selectedGroupId);
    });

    $("#text_address1").keypress(function () {
        $("#message").html("");
        $("#hidden_addressChosen").val("");
    });

    $("#text_address1").autocomplete({
        source: function (request, response) {
            $("#ajax_gpsCoordinates").show();
            var address = $("#text_address1").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_gpsCoordinates").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 2, '', false);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#text_address2").autocomplete({
        source: function (request, response) {
            $("#message").html("");
            if ($("#hidden_addressChosen").val() == "selected") {
                response("");
                return;
            }
            $("#ajax_gpsCoordinates2").show();
            var address = $("#text_address2").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_gpsCoordinates2").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 3, '', false);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#text_other").keyup(function () {
        if ($("#text_other").val() != "") {
            $("#checkbox_other").prop("checked", true);
        }
        else {
            $("#checkbox_other").prop("checked", false);
        }
    });

    $("#text_leaveOther").keyup(function () {
        if ($("#text_leaveOther").val() != "") {
            $("#checkbox_leaveOther").prop("checked", true);
        }
        else {
            $("#checkbox_leaveOther").prop("checked", false);
        }
    });

    $("#button_sendEmail").click(function () {
        //Fetch the email addresses to send the email to
        $("#include_Visitors").dialog(
                {
                    modal: true,
                    height: 200,
                    width: 300,
                    resizable: false,
                    buttons: {
                        "Yes": function () {
                            FetchEmailList(true);
                            $(this).dialog("close");
                        },
                        "No": function () {
                            FetchEmailList(false);
                            $(this).dialog("close");
                        }
                    }
                });
    });


})