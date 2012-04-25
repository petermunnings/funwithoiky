var homeGroups;
var rowId = 0;
var selectedGroupId = 0;

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

function FetchAttendance() {
    //Fetch the attendance on that day
    var postData = { 
        groupId: selectedGroupId,
        date: $("#text_eventDate").val()
    };

    var jqxhr = $.post("/Ajax/FetchAttendance", $.postify(postData), function (data) {
        UpdateAttendance("#attendanceList .tableRow", data);
    });
}

function PopulateAttendance() {
    var postData = { groupId: selectedGroupId };
    var jqxhr = $.post("/Ajax/FetchPeopleInGroupForAttendance", $.postify(postData), function (data) {
        $("#attendanceList").empty();
        //$("#attendanceTemplate").empty();
        $("#attendanceTemplate")
                .tmpl(data.People)
                .appendTo("#attendanceList");

        $(".addEvent").button();
        $(".addComment").button();
        FetchAttendance();
        $("#ajax_loader").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        SendErrorEmail("Error calling FetchPeopleInGroupForAttendance", jqXHR.responseText);
    });


}

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
        GroupClassification: $("#SelectedGroupClassificationId").val(),
        Suburb: $("#SelectedSuburbId").val()
    };

    $.post("/Ajax/SaveHomeGroup", postData, function (data) {
        $("#jqgGroups").trigger("reloadGrid");
        //Set newly create group as the selected group
        //Trigger the rest
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        SendErrorEmail("Error calling SaveHomeGroup", jqXHR.responseText);
    });
}

function AddPersonToGroup(personId, homeGroupId) {
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
            RoleId: $("#RoleId").val(),
            GroupId: homeGroupId
        };

        $.post("/Ajax/SavePerson", $.postify(personData), function (data) {
            AddPersonToGroup(data.PersonId, homeGroupId);
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling SavePerson", jqXHR.responseText);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        });
    }
    else {

        var postData = { groupId: homeGroupId, personId: personId };

        $.post("/Ajax/AddPersonToGroup", $.postify(postData), function () {
            ReloadPeopleGrid(selectedGroupId);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        }).error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_loader").hide();
            SendErrorEmail("Error calling AddPersonToGroup", jqXHR.responseText);
            $("#message_hg").html("");
            $("#ajax_loader_hg").hide();
        });
    }
}

function RemovePersonFromGroup(personId, groupId) {
    $("#message").html("");
    $("#ajax_loader_hg").show();
    var postData = { groupId: groupId, personId: personId };

    var jqxhr = $.post("/Ajax/RemovePersonFromGroup", $.postify(postData), function () {
        ReloadPeopleGrid(groupId);
        $("#ajax_loader_hg").hide();
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader_hg").hide();
        SendErrorEmail("Error calling RemovePersonFromGroup", jqXHR.responseText);
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

function DeleteGroup() {
    $("#message").html("");    
    var postData = { groupId: selectedGroupId};

    var jqxhr = $.post("/Ajax/DeleteHomeGroup", $.postify(postData), function (data) {
        $("#message").html(data.Message);
        if (data.Success) {
            $("#jqgGroups").trigger("reloadGrid");
            $("#ajax_loader").hide();
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        SendErrorEmail("Error calling DeleteHomeGroup", jqXHR.responseText);
    });
}

function SaveLeaveEvents(personId) {
    $("#ajax_loader_hg").show();
    var postData = { PersonId: personId,
        Events: []
    };

    var currentDate = new Date();
    var currentDateFormat = currentDate.getFullYear() + '/' + (currentDate.getMonth() + 1) + '/' + (currentDate.getDate());
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
            SendErrorEmail("Error calling SavePersonEvents", jqXHR.responseText);
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
            SendErrorEmail("Error calling SavePersonEvents", jqXHR.responseText);
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

    $.each($("#attendanceList input.checkbox_didNotAttended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
            Events: [{ Name: 'Did not attend Group',
                Date: $("#text_eventDate").val(),
                GroupId: selectedGroupId
            }]
        });
    });

    $.each($("#attendanceList input.checkbox_attended:checked"), function (index, value) {
        postData.Events.push({ PersonId: $.tmplItem(value).data.PersonId,
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
        SendErrorEmail("Error calling SaveHomeGroupEvent", jqXHR.responseText);
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
            SendErrorEmail("Error calling SavePersonComment", jqXHR.responseText);
        });
}

function FetchEmailList() {
    var selArr = $("#jqgPeople").getGridParam("selarrrow");
    var postData = { groupId: selectedGroupId, selectedIds: selArr };

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
                RemovePersonFromGroup(personId, selectedGroupId);
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

function EditPerson() {
    var selArr = $("#jqgPeople").getGridParam("selarrrow");
    if (selArr.length > 0)
        window.location = "/Home/Person?PersonId=" + selArr[0];
}

function DeletePerson(personId) {
    var selArr = $("#jqgPeople").getGridParam("selarrrow");
    if (selArr.length > 0)
        ShowLeaveEvents(selArr[0]);
}

function AddPerson() {
    //Populate fields
    $("#hidden_personId").val("0");
    $("#text_personName").val("");
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
                    AddPersonToGroup($("#hidden_personId").val(), selectedGroupId);
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function SetupPeopleGrid() {
    $('#jqgPeople').jqGrid({
        url: '/Ajax/FetchPeopleInGroup',
        datatype: 'json',
        mtype: 'POST',
        postData: { groupId: function () { return selectedGroupId; } },
        colNames: ['PersonId', 'Firstname', 'Surname', 'HomePhone', 'CellPhone', 'Email'],
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 150, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 150, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 110, search: false },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 110, search: false },
                    { name: 'Email', index: 'Email', align: 'left', width: 200, search: false }
                  ],
        multiselect: true,
        multiboxonly: true,
        pager: $('#jqgpPeople'),
        rowNum: 25,
        sortname: 'Surname',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto'
    }).navGrid('#jqgpPeople', { edit: false, add: false, del: false, search: false })
    .navButtonAdd('#jqgpPeople', {
        caption: "Delete",
        buttonicon: "ui-icon-trash",
        onClickButton: function () {
            DeletePerson();
        },
        position: "first"
    })
    .navButtonAdd('#jqgpPeople', {
        caption: "Edit",
        buttonicon: "ui-icon-pencil",
        onClickButton: function () {
            EditPerson();
        },
        position: "first"
    })
    .navButtonAdd('#jqgpPeople', {
        caption: "Add",
        buttonicon: "ui-icon-plus",
        onClickButton: function () {
            AddPerson();
        },
        position: "first"
    });

    $('#jqgPeople').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
}

function ReloadPeopleGrid(id) {
    selectedGroupId = id;
    $('#jqgGroups').jqGrid('setSelection', selectedGroupId);
    var ret = $("#jqgGroups").getRowData(selectedGroupId);
    $("#groupName").html(ret.GroupName);
    $("#jqgPeople").trigger("reloadGrid");

    PopulateAttendance();
}

function AddGroup() {
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

        });
}

function EditGroup() {
    var postData = { groupId: selectedGroupId };
    $("#hidden_homeGroupId").val(selectedGroupId);
    var jqxhr = $.post("/Ajax/FetchGroupInfo", $.postify(postData), function (data) {

        $("#text_homeGroupName").val(data.GroupInfo.HomeGroupName);
        $("#text_homeGroupLeader").val(data.GroupInfo.LeaderName);
        $("#hidden_homeGroupLeaderId").val(data.GroupInfo.LeaderId);
        $("#text_homeGroupAdministrator").val(data.GroupInfo.AdministratorName);
        $("#hidden_homeGroupAdministratorId").val(data.GroupInfo.AdministratorId);
        $("#text_address1").val(data.GroupInfo.Address1);
        $("#text_address2").val(data.GroupInfo.Address2);
        $("#text_address3").val(data.GroupInfo.Address3);
        $("#text_address4").val(data.GroupInfo.Address4);
        $("#hidden_addressType").val(data.GroupInfo.AddressType);
        $("#hidden_addressId").val(data.GroupInfo.AddressId);
        $("#hidden_lat").val(data.GroupInfo.Lat);
        $("#hidden_lng").val(data.GroupInfo.Lng);
        var groupClassification = data.GroupInfo.GroupClassification;
        if (groupClassification == "") {
            groupClassification = "Select...";
        }
        $("#SelectedGroupClassificationId").val(groupClassification);
        var suburb = data.GroupInfo.Suburb;
        if (suburb == "") {
            suburb = "Select...";
        }
        $("#SelectedSuburbId").val(suburb);

    });

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

        });
}

$(document).ready(function () {
    selectedGroupId = $("#div_groupId").html();

    SetupPeopleGrid();

    if ($("#div_showList").html() == "True") {
        $('#jqgGroups').jqGrid({
            url: '/Ajax/FetchGroupList',
            datatype: 'json',
            mtype: 'POST',
            colNames: ['GroupId', 'Group Name', 'Leader', 'Administrator', 'Suburb', 'Classification'],
            colModel: [
                    { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
                    { name: 'GroupName', index: 'GroupName', align: 'left', width: 150, search: true },
                    { name: 'LeaderName', index: 'LeaderName', align: 'left', width: 130, search: true },
                    { name: 'Administrator', index: 'Administrator', align: 'left', width: 130, search: true },
                    { name: 'Suburb', index: 'Suburb', align: 'left', width: 130, search: true },
                    { name: 'GroupClassification', index: 'GroupClassification', align: 'left', width: 150, search: true }
                  ],
            pager: $('#jqgpGroups'),
            rowNum: 25,
            sortname: 'GroupName',
            sortorder: 'asc',
            viewrecords: true,
            width: 'auto',
            height: 'auto',
            gridComplete: function () {
                if ($('#jqgGroups').getDataIDs().length > 0) {
                    ReloadPeopleGrid($('#jqgGroups').getDataIDs()[0]);
                }
            },
            onSelectRow: function (id) {
                if (selectedGroupId != id) {
                    ReloadPeopleGrid(id);
                }
            }
        })
        .navGrid('#jqgpGroups', { edit: false, add: false, del: false, search: false });

        if ($("#div_addGroup").html() == "True") {
            $('#jqgGroups').jqGrid().navButtonAdd('#jqgpGroups', {
                caption: "Delete",
                buttonicon: "ui-icon-trash",
                onClickButton: function () {
                    DeleteGroup();
                },
                position: "first"
            });
        }

        if ($("#div_editGroup").html() == "True") {
            $('#jqgGroups').jqGrid().navButtonAdd('#jqgpGroups', {
                caption: "Edit",
                buttonicon: "ui-icon-pencil",
                onClickButton: function () {
                    EditGroup();
                },
                position: "first"
            });
        }

        if ($("#div_deleteGroup").html() == "True") {
            $('#jqgGroups').jqGrid().navButtonAdd('#jqgpGroups', {
                caption: "Add",
                buttonicon: "ui-icon-plus",
                onClickButton: function () {
                    AddGroup();
                },
                position: "first"
            });
        }

        $('#jqgGroups').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    } else {
        PopulateAttendance();
    }

    $("#ajax_loader").hide();
    $("#ajax_loader_hg").hide();
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

    $("#table_people").delegate(".selectPerson", "mouseover mouseout", function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
            $(this).parent().addClass("ui-state-hover");
        } else {
            $(this).css("cursor", "default");
            $(this).parent().removeClass("ui-state-hover");
        }
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
            $("#row_roleId").show();
            var postData = { term: request.term };

            var jqxhr = $.post("/Ajax/PersonAutoComplete", $.postify(postData), function (data) {
                $("#ajax_loader_addPerson").hide();
                response(data);
            }).error(function (jqXHR, textStatus, errorThrown) {
                $("#ajax_loader_addPerson").hide();
                SendErrorEmail("Error calling PersonAutoComplete", jqXHR.responseText);
            });
        }
        ,
        minLength: 1,
        select: function (event, ui) {
            if (ui.item) {
                $("#hidden_personId").val(ui.item.id);
                $("#row_roleId").hide();
            } else {
                $("#hidden_personId").val("0");
                $("#row_roleId").show();
            }
        }
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

    //    $("#button_addPersonAttendance").click(function () {
    //        //Populate fields
    //        $("#hidden_personId").val("0");
    //        $("#text_personName").val("");

    //        $("#add_Person").dialog(
    //        {
    //            modal: true,
    //            height: 150,
    //            width: 440,
    //            resizable: false,
    //            buttons: {
    //                "Add Person": function () {
    //                    $("#ajax_loader").show();
    //                    rowId = 0;
    //                    AddPersonToGroup($("#hidden_personId").val(), selectedGroupId);
    //                    $(this).dialog("close");
    //                },
    //                Cancel: function () {
    //                    $(this).dialog("close");
    //                }
    //            }
    //        });
    //    });


    $("#button_printList").click(function () {
        window.location = "/Report/HomeGroupList/" + selectedGroupId;
    });

    $("#button_printAttendance").click(function () {
        window.location = "/Report/HomeGroupAttendance/" + selectedGroupId;
    });

    $("#membersList").delegate(".radio_leader", "click", function () {
        personId = $.tmplItem(this).data.PersonId;
        SetLeader(personId, selectedGroupId);
    });

    $("#membersList").delegate(".radio_administrator", "click", function () {
        personId = $.tmplItem(this).data.PersonId;
        SetAdministrator(personId, selectedGroupId);
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
        FetchEmailList();
    });


})