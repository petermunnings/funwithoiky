﻿function Search() {
    var grid = $('#jqgEventList');
    if ($("#searchText").val().length > 0) {
        var postdata = grid.jqGrid('getGridParam', 'postData');
        jQuery.extend(postdata,
               { filters: '',
                   searchField: 'search',
                   searchOper: 'cn',
                   searchString: $("#searchText").val()
               });
        grid.jqGrid('setGridParam', { search: true, postData: postdata });
    }
    else {
        grid.jqGrid('setGridParam', { search: false });
    }
    grid.trigger("reloadGrid", [{ page: 1}]);
}

function SendReportEmail(sourceOfEmails, postData) {
    OpenEmailDialog();
    $.post(sourceOfEmails, $.postify(postData), function (data) {
        if (data.Message == "") {
            SetupEmailDialog();
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

function FetchGroupLeadersEmailList() {
    var gridFilter = $("#jqgHomegroups").jqGrid('getGridParam', 'postData');
    var postData = {
        search: gridFilter._search,
        filters: gridFilter.filters,
        includeMembers: $("#checkBox_IncludeMembers").prop('checked') 
    };

    SendReportEmail("/Ajax/FetchGroupLeaderEmails", postData);
}


function FetchPeopleInARoleEmailList() {
    var postData = { roleId: $("#RoleId").val() };

    SendReportEmail("/Ajax/FetchPeopleInARoleEmails", postData);
}

function SendReportSms(sourceOfCellPhoneNos, postData) {
    OpenSmsDialog();
    $.post(sourceOfCellPhoneNos, postData, function (data) {
        if (data.Message == "") {
            SetupSmsDialog(data.NoNos);
        } else {
            $("#responseMessage_text").html(data.Message);
            $("#response_Message").dialog(
                {
                    modal: true,
                    height: 200,
                    width: 500,
                    resizable: true,
                    buttons: {
                        "Close": function() {
                            $(this).dialog("close");
                        }
                    }
                });
        }
    });
}

function FetchGroupLeadersSmsList() {
    var gridFilter = $("#jqgHomegroups").jqGrid('getGridParam', 'postData');
    var postData = {
        search: gridFilter._search,
        filters: gridFilter.filters,
        includeMembers: $("#checkBox_IncludeMembers").prop('checked')
    };

    SendReportSms("/Ajax/FetchGroupLeaderCellPhoneNos", postData);
}

function FetchPeopleInARoleSmsList() {
    var postData = {roleId: $("#RoleId").val()};

    SendReportSms("/Ajax/FetchPeopleInARoleCellPhoneNos", postData);
}

function GetSelectedRolesForChildrenReport() {
    var selectedRoles = new Array();
    $('input[name="ChurchRolesForChildrenReport"]:checked').each(function() {
        selectedRoles.push(this.value);
    });
    return selectedRoles;
}

function GetSelectedBirthdayRolesForReport() {
    var selectedRoles = new Array();
    $('input[name="BirthdayRoles"]:checked').each(function () {
        selectedRoles.push(this.value);
    });
    return selectedRoles;
}

function GetSelectedAnniversaryRolesForReport() {
    var selectedRoles = new Array();
    $('input[name="AnniversaryRoles"]:checked').each(function () {
        selectedRoles.push(this.value);
    });
    return selectedRoles;
}

$(document).ready(function () {

    $("#fromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd MM yy',
        yearRange: '-1:+0'
    });

    $("#toDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd MM yy',
        yearRange: '-1:+0'
    });


    $('#jqgHomegroups').jqGrid({
        //url from wich data should be requested
        url: '/Ajax/FetchGroupList',
        //type of data
        datatype: 'json',
        //url access method type
        mtype: 'POST',
        //columns names
        colNames: ['GroupId', 'Group Name', 'Leader', 'Administrator', 'Suburb', 'Group Classification'],
        //columns model
        colModel: [
                    { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
                    { name: 'GroupName', index: 'GroupName', align: 'left', width: 150, search: true },
                    { name: 'LeaderName', index: 'LeaderName', align: 'left', width: 130, search: true },
                    { name: 'Administrator', index: 'Administrator', align: 'left', width: 130, search: true },
                    { name: 'Suburb', index: 'Suburb', align: 'left', width: 130, search: true },
                    { name: 'GroupClassification', index: 'GroupClassification', align: 'left', width: 150, search: true }
                  ],
        //pager for grid
        pager: $('#jqgpHomegroups'),
        //number of rows per page
        rowNum: 15,
        //initial sorting column
        sortname: 'GroupName',
        //initial sorting direction
        sortorder: 'asc',
        //we want to display total records count
        viewrecords: true,
        //grid width
        width: 'auto',
        //grid height
        height: 'auto'
    }).navGrid('#jqgpHomegroups', { edit: false, add: false, del: false, search: false });

    $('#jqgHomegroups').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#jqgNotInGroup').jqGrid({
        //url from wich data should be requested
        url: '/Ajax/FetchPeopleNotInHomeGroup',
        //type of data
        datatype: 'json',
        //url access method type
        mtype: 'POST',
        //columns names
        colNames: ['PersonId', 'Firstname', 'Surname', 'HomePhone', 'CellPhone', 'Email', 'Site'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 140, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 140, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120, sortable: false, search: true },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120, sortable: false, search: true },
                    { name: 'Email', index: 'Email', align: 'left', width: 194, sortable: false, search: true },
                    { name: 'Site', index: 'Site', align: 'left', width: 150, sortable: true, search: false }
                  ],
        //pager for grid
        pager: $('#jqgpNotInGroup'),
        //number of rows per page
        rowNum: 15,
        //initial sorting column
        sortname: 'Surname',
        //initial sorting direction
        sortorder: 'asc',
        //we want to display total records count
        viewrecords: true,
        //grid width
        width: 'auto',
        //grid height
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location = "/Home/Person?PersonId=" + rowid;
        }
    }).navGrid('#jqgpNotInGroup', { edit: false, add: false, del: false, search: false });

    $("#button_sendHomeGroupLeaderEmail").click(function () {
        FetchGroupLeadersEmailList();
    });

    $("#button_sendHomeGroupLeaderSms").click(function () {
        FetchGroupLeadersSmsList();
    });
    
    $("#button_sendEmail").click(function () {
        FetchPeopleInARoleEmailList();
    });
    
    $("#button_sendSms").click(function () {
        FetchPeopleInARoleSmsList();
    });
    
    $('#jqgEventList').jqGrid({
        //url from wich data should be requested
        url: '/Ajax/FetchEventList',
        //type of data
        datatype: 'json',
        //url access method type
        mtype: 'POST',
        postData: { fromDate: $("#fromDate").val(), toDate: $("#toDate").val() },
        colNames: ['PersonId', 'Person', 'Event Date', 'Event', 'Created By'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Person', index: 'Person', align: 'left', width: 120, search: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } },
                    { name: 'Date', index: 'Date', align: 'left', width: 140, search: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } },
                    { name: 'Event', index: 'Event', align: 'left', width: 300, search: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;' } },
                    { name: 'CreatedBy', index: 'CreatedBy', align: 'left', width: 170, search: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="vertical-align:top"' } }
                  ],
        //pager for grid
        pager: $('#jqgpEventList'),
        //number of rows per page
        rowNum: 15,
        //initial sorting column
        sortname: 'Date',
        //initial sorting direction
        sortorder: 'desc',
        //we want to display total records count
        viewrecords: true,
        //grid width
        width: 'auto',
        //grid height
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            var personId = $('#jqgEventList').jqGrid('getCell', rowid, 'PersonId');
            window.location = "/Home/Person?PersonId=" + personId;
        }
    }).navGrid('#jqgpEventList', { edit: false, add: false, del: false, search: false });

    $(".dateControl").change(function () {
        $("#jqgEventList").jqGrid("setGridParam", { "postData": { fromDate: $("#fromDate").val(), toDate: $("#toDate").val()} });
        Search();
    });

    var timeOut;
    $("#searchText").keyup(function () {
        clearTimeout(timeOut);

        timeOut = setTimeout(function () {
            Search();
        }, 600);
    });

    $('#jqgBirthdays').jqGrid({
        url: '/Ajax/FetchBirthdays',
        postData: {
            monthId: function () { return $("#BirthdayMonthId").val(); },
            selectedRoles: function () { return GetSelectedBirthdayRolesForReport(); }
        },
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Day', 'Firstname', 'Surname', 'MemberStatus', 'HomePhone', 'CellPhone', 'Email'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Day', index: 'Day', align: 'left', width: 40 },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 130, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 130, search: true },
                    { name: 'MemberStatus', index: 'MemberStatus', align: 'left', width: 148, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120 },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120 },
                    { name: 'Email', index: 'Email', align: 'left', width: 230 }
        ],
        pager: $('#jqgpBirthdays'),
        rowNum: 20,
        sortname: 'Birthday',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location = "/Home/Person?PersonId=" + rowid;
        }
    }).navGrid('#jqgpBirthdays', { edit: false, add: false, del: false, search: false });

    $("#BirthdayMonthId").change(function () {
        $('#jqgBirthdays').trigger("reloadGrid");
    });

    $('input[name="BirthdayRoles"]').change(function() {
        $('#jqgBirthdays').trigger("reloadGrid");
    });

    $('input[name="AnniversaryRoles"]').change(function () {
        $('#jqgAnniversaries').trigger("reloadGrid");
    });
    
    $('#jqgAnniversaries').jqGrid({
        url: '/Ajax/FetchAnniversaries',
        postData: {
            monthId: function () { return $("#AnniversaryMonthId").val(); },
            selectedRoles: function () { return GetSelectedAnniversaryRolesForReport(); }
        },
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Day', 'Firstname', 'Surname', 'MemberStatus', 'HomePhone', 'CellPhone', 'Email'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Day', index: 'Day', align: 'left', width: 40 },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 130, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 130, search: true },
                    { name: 'MemberStatus', index: 'MemberStatus', align: 'left', width: 148, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120 },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120 },
                    { name: 'Email', index: 'Email', align: 'left', width: 230 }
        ],
        pager: $('#jqgpAnniversaries'),
        rowNum: 20,
        sortname: 'Birthday',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location = "/Home/Person?PersonId=" + rowid;
        }
    }).navGrid('#jqgpAnniversaries', { edit: false, add: false, del: false, search: false });

    $("#AnniversaryMonthId").change(function () {
        $('#jqgAnniversaries').trigger("reloadGrid");
    });

    $('#jqgChildren').jqGrid({
        url: '/Ajax/FetchListOfChildren',
        postData: {
            selectedRoles: function() { return GetSelectedRolesForChildrenReport(); }
        },
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Age', 'Firstname', 'Surname', 'CellNo', 'Group', 'Father', 'Mother'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Age', index: 'Age', align: 'left', width: 40 },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 130, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 130, search: true },
                    { name: 'CellNo', index: 'CellNo', align: 'left', width: 120 },
                    { name: 'Group', index: 'Group', align: 'left', width: 120 },
                    { name: 'Father', index: 'Father', align: 'left', width: 120 },
                    { name: 'Mother', index: 'Mother', align: 'left', width: 120 }
        ],
        pager: $('#jqgpChildren'),
        rowNum: 20,
        sortname: 'Age',
        sortorder: 'asc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location = "/Home/Person?PersonId=" + rowid;
        }
    }).navGrid('#jqgpChildren', { edit: false, add: false, del: false, search: false });

    $('input[name="ChurchRolesForChildrenReport"]').change(function () {
        $('#jqgChildren').trigger("reloadGrid");
    });

    $('#jqgPeopleInARole').jqGrid({
        url: '/Ajax/FetchPeople',
        postData: { roleId: function () { return $("#RoleId").val(); }},
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Firstname', 'Surname', 'HomePhone', 'CellPhone', 'Email', 'Date First Visit', 'Group', 'Site'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 180, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 180, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120 },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120 },
                    { name: 'Email', index: 'Email', align: 'left', width: 160 },
                    { name: 'Date', index: 'Date', align: 'left', width: 100 },
                    { name: 'Group', index: 'Group', align: 'left', width: 140 },
                    { name: 'Site', index: 'Site', align: 'left', width: 120 }
                  ],
        pager: $('#jqgpPeopleInARole'),
        rowNum: 20,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location = "/Home/Person?PersonId=" + rowid;
        }
    }).navGrid('#jqgpPeopleInARole', { edit: false, add: false, del: false, search: false });

    $('#jqgPeopleInARole').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#RoleId").change(function () {
        $('#jqgPeopleInARole').trigger("reloadGrid");
    });

    //Need to do an asynch post to fetch the columns for this next one
    $.post("/Ajax/FetchGroupAttendanceGridSetup", function (data) {
        $('#jqgGroupAttendance').jqGrid({
            url: '/Ajax/FetchGroupAttendance',
            datatype: 'json',
            mtype: 'POST',
            colNames: data.colNames,
            colModel: data.colModel,
            pager: $('#jqgpGroupAttendance'),
            rowNum: 15,
            sortname: 'Name',
            sortorder: 'asc',
            viewrecords: true,
            width: 'auto',
            height: 'auto'
        }).navGrid('#jqgpGroupAttendance', { edit: false, add: false, del: false, search: false });
    })
    .success(function () {
        $("#jqgGroupAttendance").jqGrid('setGridParam', { datatype: 'json' });
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_familySearch").hide();
        SendErrorEmail("Error calling FetchGroupAttendanceGridSetup", jqXHR.responseText);
    });

    $("#button_viewPeopleInARole").click(function () {
        var roleName = $("#RoleId option[value='" + $("#RoleId").val() + "']").text();
        window.location = "/Report/PeopleList?roleId=" + $("#RoleId").val() + "&roleName=" + roleName;
    });


    $("#button_viewPeopleNotInAGroup").click(function () {
        window.location = "/Report/PeopleNotInAGroup";
    });
    

    $("#button_exportChurchData").click(function () {
        window.location = "/Report/ExportChurchData";
    });

    $("#button_exportChildrenData").click(function () {
        window.location = "/Report/ExportChildrenData?selectedRoles=" + GetSelectedRolesForChildrenReport();
    });

    $("#button_exportBirthdayData").click(function () {
        window.location = "/Report/ExportBirthdayData?selectedRoles=" + GetSelectedBirthdayRolesForReport() + "&selectedMonth=" + $("#BirthdayMonthId").val();
    });

    $("#button_exportAnniversaryData").click(function () {
        window.location = "/Report/ExportAnniversaryData?selectedRoles=" + GetSelectedAnniversaryRolesForReport() + "&selectedMonth=" + $("#AnniversaryMonthId").val();
    });
    
});