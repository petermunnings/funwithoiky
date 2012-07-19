function PopulatePerson(person) {
    $("#span_firstname").html(person.Firstname);
    $("#span_surname").html(person.Surname);
    $("#span_email").html(person.Email);
    if (person.DateOfBirth_Short == '') {
        $("#row_dateOfBirth").hide();
    }
    else {
        $("#row_dateOfBirth").show();
    }
    $("#span_dateOfBirth").html(person.DateOfBirth_Short);
    if (person.Anniversary_Short == '') {
        $("#row_anniversary").hide();
    }
    else {
        $("#row_anniversary").show();
    }
    $("#span_anniversary").html(person.Anniversary_Short);
    $("#span_homePhone").html(person.HomePhone);
    $("#span_cellPhone").html(person.CellPhone);
    $("#span_workPhone").html(person.WorkPhone);
    $("#span_skype").html(person.Skype);
    $("#span_twitter").html(person.Twitter);
    $("#span_occupation").html(person.Occupation);
    $("#span_address1").html(person.Address1);
    $("#span_address2").html(person.Address2);
    $("#span_address3").html(person.Address3);
    $("#span_address4").html(person.Address4);
    if (person.FacebookId != null) {
        $("#img_person").prop("src", "https://graph.facebook.com/" + person.FacebookId + "/picture?type=large");
        $("#row_image").show();
    }
    else {
        $("#row_image").hide();
        $("#img_person").prop("src", " ");
    }

    $("#display_person").dialog(
        {
            modal: true,
            height: 560,
            width: 440,
            resizable: false,
            buttons: {
                "OK": function () {
                    $("#img_person").prop("src", "");
                    $(this).dialog("close");
                }
            }
        })
}

$(document).ready(function () {
    $('#jqgChurchList').jqGrid({
        //url from wich data should be requested
        url: '/Ajax/FetchChurchList',
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
                    { name: 'Email', index: 'Email', align: 'left', width: 254, sortable: false, search: true }
                  ],
        //pager for grid
        pager: $('#jqgpChurchList'),
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
        onSelectRow: function (id) {
            //alert('person ' + $('#jqgChurchList').jqGrid('getCell', id, 'Firstname') + ' selected');
            var postData = { personId: id };

            $.post("/Ajax/FetchPerson", $.postify(postData), function (data) {
                PopulatePerson(data.Person);
            });


        }
    }).navGrid('#jqgpChurchList', { edit: false, add: false, del: false, search: false });



    $("#searchButton").click(function () {
        var grid = $('#jqgChurchList');
        var postdata = grid.jqGrid('getGridParam', 'postData');
        jQuery.extend(postdata,
               { filters: '',
                   searchField: 'search',
                   searchOper: 'cn',
                   searchString: $("#searchText").val()
               });
        grid.jqGrid('setGridParam', { search: true, postData: postdata });
        grid.trigger("reloadGrid", [{ page: 1}]);
    });

    var timeOut;
    $("#searchText").keyup(function () {
        clearTimeout(timeOut);
        if ($("#searchText").val().length > 0) {
            timeOut = setTimeout(function () {
                var grid = $('#jqgChurchList');
                var postdata = grid.jqGrid('getGridParam', 'postData');
                jQuery.extend(postdata,
               { filters: '',
                   searchField: 'search',
                   searchOper: 'cn',
                   searchString: $("#searchText").val()
               });
                grid.jqGrid('setGridParam', { search: true, postData: postdata });
                grid.trigger("reloadGrid", [{ page: 1}]);
            }, 600);
        }
    });

    $("#clearButton").click(function () {
        $("#searchText").val('');
        var grid = $('#jqgChurchList');
        var postdata = grid.jqGrid('getGridParam', 'postData');
        jQuery.extend(postdata,
               { filters: '',
                   searchField: '',
                   searchOper: '',
                   searchString: ''
               });
        grid.jqGrid('setGridParam', { search: false, postData: postdata });
        grid.trigger("reloadGrid", [{ page: 1}]);
    });

    $("#onlyMyHomeGroup").click(function () {
        var grid = $('#jqgChurchList');
        var postdata = grid.jqGrid('getGridParam', 'postData');
        jQuery.extend(postdata,
               { filters: '',
                   searchField: 'homegroup',
                   searchOper: 'cn',
                   searchString: 'mine'
               });
        grid.jqGrid('setGridParam', { search: true, postData: postdata });
        grid.trigger("reloadGrid", [{ page: 1}]);
    });

    $("#button_viewChurchList").click(function () {
        var postdata = $("#jqgChurchList").jqGrid('getGridParam', 'postData');
        window.location = "/Report/ChurchList?search=" + postdata._search + "&searchField=" + postdata.searchField + "&searchString=" + postdata.searchString;
    });

    $("#button_sendEmail").click(function () {
        var gridFilter = $("#jqgChurchList").jqGrid('getGridParam', 'postData');
        var postData = {
            search: gridFilter._search,
            searchField: gridFilter.searchField,
            searchString: gridFilter.searchString
        };

        OpenEmailDialog();
        var jqxhr = $.post("/Ajax/FetchChurchEmailAddresses", $.postify(postData), function (data) {
            if (data.Message == "") {
                SetEmailList();
            }
            else {
                $("#responseMessage_text").html(data.Message);
                $("#response_Message").dialog({
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
    });

    $("#button_sendSms").click(function () {
        var gridFilter = $("#jqgChurchList").jqGrid('getGridParam', 'postData');
        var postData = {
            search: gridFilter._search,
            searchField: gridFilter.searchField,
            searchString: gridFilter.searchString
        };

        OpenSmsDialog();
        var jqxhr = $.post("/Ajax/FetchChurchCellPhoneNos", $.postify(postData), function (data) {
            if (data.Message == "") {
                SetSmsList(data.NoNos);
            }
            else {
                $("#responseMessage_text").html(data.Message);
                $("#response_Message").dialog({
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
    });
});