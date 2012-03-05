function Search() {
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

function FetchEmailList() {
    var gridFilter = $("#jqgHomegroups").jqGrid('getGridParam', 'postData');
    var postData = {
        search: gridFilter._search,
        filters: gridFilter.filters,
        includeMembers: $("#checkBox_IncludeMembers").prop('checked') 
    };
    OpenEmailDialog();
    var jqxhr = $.post("/Ajax/FetchGroupLeaderEmails", $.postify(postData), function (data) {
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

function FetchSmsList() {
    var gridFilter = $("#jqgHomegroups").jqGrid('getGridParam', 'postData');
    var postData = {
        search: gridFilter._search,
        filters: gridFilter.filters,
        includeMembers: $("#checkBox_IncludeMembers").prop('checked') 
    };
    OpenSmsDialog();
    var jqxhr = $.post("/Ajax/FetchGroupLeaderCellPhoneNos", postData, function (data) {
        if (data.Message == "") {
            SetSmsList();
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
        url: '/Ajax/FetchHomeGroupList',
        //type of data
        datatype: 'json',
        //url access method type
        mtype: 'POST',
        //columns names
        colNames: ['GroupId', 'HomeGroupName', 'Leader', 'Administrator', 'Suburb', 'GroupClassification'],
        //columns model
        colModel: [
                    { name: 'GroupId', index: 'GroupId', hidden: true, search: false },
                    { name: 'HomeGroupName', index: 'HomeGroupName', align: 'left', width: 150, search: true },
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
        sortname: 'HomeGroupName',
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
        colNames: ['PersonId', 'Firstname', 'Surname', 'HomePhone', 'CellPhone', 'Email'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 140, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 140, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120, sortable: false, search: true },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120, sortable: false, search: true },
                    { name: 'Email', index: 'Email', align: 'left', width: 194, sortable: false, search: true }
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
            window.location.replace("/Home/Person?PersonId=" + rowid);
        }
    }).navGrid('#jqgpNotInGroup', { edit: false, add: false, del: false, search: false });

    $("#button_sendHomeGroupLeaderEmail").click(function () {
        FetchEmailList();
    });

    $("#button_sendHomeGroupLeaderSms").click(function () {
        FetchSmsList();
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
            window.location.replace("/Home/Person?PersonId=" + personId);
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

    $('#jqgVisitors').jqGrid({
        url: '/Ajax/FetchPeople',
        postData: { roleId: 7 }, //Visitors
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Firstname', 'Surname', 'Date First Visit', 'Group', 'Site'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 180, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 180, search: true },
                    { name: 'Date', index: 'Date', align: 'left', width: 100 },
                    { name: 'Group', index: 'Group', align: 'left', width: 140 },
                    { name: 'Site', index: 'Site', align: 'left', width: 120 }
                  ],
        pager: $('#jqgpVisitors'),
        rowNum: 20,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location.replace("/Home/Person?PersonId=" + rowid);
        }
    }).navGrid('#jqgpVisitors', { edit: false, add: false, del: false, search: false });

    $('#jqgVisitors').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#jqgPastMembers').jqGrid({
        url: '/Ajax/FetchPeople',
        postData: { roleId: 6 }, //Past Members
        datatype: 'json',
        mtype: 'POST',
        colNames: ['PersonId', 'Firstname', 'Surname', 'HomePhone', 'CellPhone', 'Email'],
        //columns model
        colModel: [
                    { name: 'PersonId', index: 'PersonId', hidden: true, search: false },
                    { name: 'Firstname', index: 'Firstname', align: 'left', width: 160, search: true },
                    { name: 'Surname', index: 'Surname', align: 'left', width: 160, search: true },
                    { name: 'HomePhone', index: 'HomePhone', align: 'left', width: 120 },
                    { name: 'CellPhone', index: 'CellPhone', align: 'left', width: 120 },
                    { name: 'Email', index: 'Email', align: 'left', width: 160 }
                  ],
        pager: $('#jqgpPastMembers'),
        rowNum: 20,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        width: 'auto',
        height: 'auto',
        ondblClickRow: function (rowid, iRow, iCol, e) {
            window.location.replace("/Home/Person?PersonId=" + rowid);
        }
    }).navGrid('#jqgpPastMembers', { edit: false, add: false, del: false, search: false });

    $('#jqgPastMembers').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
        }).navGrid('#jqgpVisitors', { edit: false, add: false, del: false, search: false });
    })
    .success(function () {
        $("#jqgGroupAttendance").jqGrid('setGridParam', { datatype: 'json' });
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_familySearch").hide();
        alert(jqXHR.responseText);
    });

    $("#button_viewVisitorList").click(function () {
        window.location.replace("/Report/VisitorList");
    });

    $("#button_viewPastMemberList").click(function () {
        window.location.replace("/Report/PastMemberList");
    });

    $("#button_exportChurchData").click(function () {
        window.location.replace("/Report/ExportChurchData");
    });
});