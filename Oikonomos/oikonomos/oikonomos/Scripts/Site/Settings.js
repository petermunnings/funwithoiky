﻿var optionalFields;

function DeleteSite(site) {

    var postData = { siteId: site.SiteId };

    $.post("/Ajax/DeleteSite", $.postify(postData), function (data) {
        $("#saveMessage_Sites").html(data.Message);
        var grid = $('#jqgSites');
        grid.trigger("reloadGrid");
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function SaveSite(site) {

    var postData = { SiteId: site.SiteId,
        SiteName: $("#text_sitename").val(),
        AddressId: site.AddressId,
        Address1: $("#text_siteaddress1").val(),
        Address2: $("#text_siteaddress2").val(),
        Address3: $("#text_siteaddress3").val(),
        Address4: $("#text_siteaddress4").val(),
        Lat: $("#hidden_sitelat").val(),
        Lng: $("#hidden_sitelng").val(),
        AddressType: $("#hidden_siteaddressType").val()
    };

    $.post("/Ajax/SaveSite", $.postify(postData), function (data) {
        $("#saveMessage_Sites").html(data.Message);
        var grid = $('#jqgSites');
        grid.trigger("reloadGrid");
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function PopulateSite(site) {
    $("#hidden_siteId").val(site.SiteId);
    $("#text_sitename").val(site.SiteName);
    $("#text_siteaddress1").val(site.Address1);
    $("#text_siteaddress2").val(site.Address2);
    $("#text_siteaddress3").val(site.Address3);
    $("#text_siteaddress4").val(site.Address4);
    $("#hidden_sitelat").val(site.Lat);
    $("#hidden_sitelng").val(site.Lng);
    $("#hidden_siteaddressType").val(site.AddressType);
    $("#hidden_siteaddressId").val(site.AddressId);

    if (site.SiteId == 0) {  //New site
        $("#addEdit_Site").dialog(
        {
            modal: true,
            height: 300,
            width: 300,
            resizable: false,
            buttons: {
                "Save": function () {
                    SaveSite(site);
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
    else {  //Existing site

        $("#addEdit_Site").dialog(
        {
            modal: true,
            height: 300,
            width: 300,
            resizable: false,
            buttons: {
                "Save": function () {
                    SaveSite(site);
                    $(this).dialog("close");
                },
                "Delete": function () {
                    DeleteSite(site);
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function ChangePassword() {
    $("#span_passwordMessage").html("");
    if($("#newPassword1").val()!=$("#newPassword2").val()){
        $("#span_passwordMessage").html("The current passwords must match");
    }
    else{
        $("#ajax_changePassword").show();
        var postData = { currentPassword: $("#currentPassword").val(), 
                         newPassword: $("#newPassword1").val() };

        var jqxhr = $.post("/Ajax/ChangePassword", $.postify(postData), function (data) {
                $("#span_passwordMessage").html(data.Message);
                $("#ajax_changePassword").hide();
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_changePassword").hide();
            alert(jqXHR.responseText); 
        });
    }
}

function SaveAddress() {
    var postData = { GroupId: $("#hidden_groupId").val(),
                     AddressId: $("#hidden_addressId").val(),
                     Address1: $("#text_address1").val(),
                     Address2: $("#text_address2").val(),
                     Address3: $("#text_address3").val(),
                     Address4: $("#text_address4").val(),
                     AddressType: $("#hidden_addressType").val(),
                     Lat: $("#hidden_lat").val(),
                     Lng: $("#hidden_lng").val()
                 };

    $("#ajax_loaderAddress").show();
    var jqxhr = $.post("/Ajax/SaveGroupAddress", $.postify(postData), function (data) {
            $("#saveAddressMessage").html(data.Message);
            $("#ajax_loaderAddress").hide();
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            $("#ajax_loaderAddress").hide();
            alert(jqXHR.responseText);
        });

}

function PopulateSuburbs(suburbs, load) {
    if (suburbs.length == 0) {
        $("#suburbNone").show();
        $("#suburbList").hide();
    }
    else {
        $("#suburbNone").hide();
        $("#suburbList").empty();
        $("#suburbTemplate")
                .tmpl(suburbs)
                .appendTo("#suburbList");
        $("#suburbList").show();
        $(".deleteSuburb").button();
    }

    $("#div_suburbs").show();
    $("#button_expandCollapseSuburbs").prop("title", "Collapse");
    $("#button_expandCollapseSuburbs").removeClass("ui-icon-triangle-1-e");
    $("#button_expandCollapseSuburbs").addClass("ui-icon-triangle-1-s");
    $("#suburbContent").slideDown('medium');
    CollapseOthers($("#button_expandCollapseSuburbs"), load);
}

function FetchSuburbs(load) {
    $.post("/Ajax/FetchSuburbs", function (data) {
        PopulateSuburbs(data.Suburbs, load);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function AddSuburb() {
    if ($("#input_newSuburb").val() != "") {
        var postData = { suburb: $("#input_newSuburb").val() };

        $.post("/Ajax/AddSuburb", $.postify(postData), function (data) {
            $("#input_newSuburb").val('')
            PopulateSuburbs(data.Suburbs);
        })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
    }
}

function DeleteSuburb(suburbId) {
    var postData = { suburbId: suburbId };

    $.post("/Ajax/DeleteSuburb", $.postify(postData), function (data) {
        $("#input_newSuburb").val('')
        PopulateSuburbs(data.Suburbs);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function PopulateHGClassifications(hgClassifications, load) {
    if (hgClassifications.length == 0) {
        $("#hgClassificationNone").show();
        $("#hgClassificationList").hide();
    }
    else {
        $("#hgClassificationNone").hide();
        $("#hgClassificationList").empty();
        $("#hgClassificationTemplate")
                .tmpl(hgClassifications)
                .appendTo("#hgClassificationList");
        $("#hgClassificationList").show();
        $(".deleteHGClassification").button();
    }
    $("#div_hgClassification").show();
    $("#button_expandCollapseHGClassification").prop("title", "Collapse");
    $("#button_expandCollapseHGClassification").removeClass("ui-icon-triangle-1-e");
    $("#button_expandCollapseHGClassification").addClass("ui-icon-triangle-1-s");
    $("#hgClassificationContent").slideDown('medium');
    CollapseOthers($("#button_expandCollapseHGClassification"), load);
}

function FetchHGClassifications(load) {
    $.post("/Ajax/FetchGroupClassifications", function (data) {
        PopulateHGClassifications(data.GroupClassifications, load);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function AddHGClassification() {
    if ($("#input_newHGClassification").val() != "") {
        var postData = { groupClassification: $("#input_newHGClassification").val() };

        $.post("/Ajax/AddGroupClassification", $.postify(postData), function (data) {
            $("#input_newHGClassification").val('')
            PopulateHGClassifications(data.GroupClassifications);
        })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
    }
}

function DeleteHGClassification(hgClassificationId) {
    var postData = { groupClassificationId: hgClassificationId };

    $.post("/Ajax/DeleteGroupClassification", $.postify(postData), function (data) {
        $("#input_newHGClassification").val('')
        PopulateHGClassifications(data.GroupClassifications);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function PopulateHGEventTypes(hgEventTypes, load) {
    if (hgEventTypes.length == 0) {
        $("#hgEventTypeNone").show();
        $("#hgEventTypeList").hide();
    }
    else {
        $("#hgEventTypeNone").hide();
        $("#hgEventTypeList").empty();
        $("#hgEventTypeTemplate")
                .tmpl(hgEventTypes)
                .appendTo("#hgEventTypeList");
        $("#hgEventTypeList").show();
        $(".deleteHGEventType").button();
    }
    $("#div_hgEventType").show();
    $("#button_expandCollapseHGEventType").prop("title", "Collapse");
    $("#button_expandCollapseHGEventType").removeClass("ui-icon-triangle-1-e");
    $("#button_expandCollapseHGEventType").addClass("ui-icon-triangle-1-s");
    $("#hgEventTypeContent").slideDown('medium');
    CollapseOthers($("#button_expandCollapseHGEventType"), load);
}

function FetchHGEventTypes(load) {
    var postData = { eventFor: 'Group' };
    $.post("/Ajax/FetchEventTypes", $.postify(postData), function (data) {
        PopulateHGEventTypes(data.EventTypes, load);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function AddHGEventType() {
    if ($("#input_newHGEventType").val() != "") {
        var postData = { eventType: $("#input_newHGEventType").val(),
                         eventFor: 'Group'
        };

        $.post("/Ajax/AddEventType", $.postify(postData), function (data) {
            $("#input_newHGEventType").val('')
            PopulateHGEventTypes(data.EventTypes);
        })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
    }
}

function DeleteHGEventType(hgEventTypeId) {
    var postData = { eventTypeId: hgEventTypeId,
                     eventFor: 'Group'
    };

    $.post("/Ajax/DeleteEventType", $.postify(postData), function (data) {
        $("#input_newHGEventType").val('')
        PopulateHGEventTypes(data.EventTypes);
    })
    .error(function (jqXHR, textStatus, errorThrown) {
        alert(jqXHR.responseText);
    });
}

function CollapseOthers(button, load) {
    if (button.selector != "#button_expandCollapseHGClassification") {
        $("#button_expandCollapseHGClassification").prop("title", "Expand");
        $("#button_expandCollapseHGClassification").prop("src", "/Content/Images/collapse.png");
        $("#hgClassificationContent").slideUp('medium');
        if (load) {
            $("#hgClassificationContent").hide();
        }
    }
    if (button.selector != "#button_expandCollapseSuburbs") {
        $("#button_expandCollapseSuburbs").prop("title", "Expand");
        $("#button_expandCollapseSuburbs").prop("src", "/Content/Images/collapse.png");
        $("#suburbContent").slideUp('medium');
        if (load) {
            $("#suburbContent").hide();
        }
    }
    if (button.selector != "#button_expandCollapseHGEventType") {
        $("#button_expandCollapseHGEventType").prop("title", "Expand");
        $("#button_expandCollapseHGEventType").prop("src", "/Content/Images/collapse.png");
        $("#hgEventTypeContent").slideUp('medium');
        if (load) {
            $("#hgEventTypeContent").hide();
        }
    }

}

$(document).ready(function () {

    $("#text_officePhone").mask("(999) 9999999");

    $.post("/Ajax/FetchOptionalFields", function (data) {
        optionalFields = data.OptionalFields;
        $("#optionalFieldTemplate")
            .tmpl(optionalFields)
            .appendTo("#optionalFieldList");
        //For some reason the "checked is not working"
        $(".optionalField").each(function (index) {
            $(this).prop("checked", $.tmplItem(this).data.Display);
            if ($.tmplItem(this).data.Name == 'SuburbLookup') {
                if ($.tmplItem(this).data.Display) {
                    FetchSuburbs(true);
                }
                else {
                    $("#div_suburbs").hide();
                }
            }
            if ($.tmplItem(this).data.Name == 'Home Group Classification') {
                if ($.tmplItem(this).data.Display) {
                    FetchHGClassifications(true);
                }
                else {
                    $("#div_hgClassification").hide();
                }
            }
            if ($.tmplItem(this).data.Name == 'Home Group Events') {
                if ($.tmplItem(this).data.Display) {
                    FetchHGEventTypes(true);
                }
                else {
                    $("#div_hgEventType").hide();
                }
            }
        });

    })
    .error(function (jqXHR, textStatus, errorThrown) { alert(jqXHR.responseText); });

    $("#button_saveGeneralSettings").click(function () {
        $("#saveMessage_generalSettings").html("");
        //Send the updated data back to the server
        $("#ajaxLoader_generalSettings").show();
        var postData = { OptionalFields: optionalFields };
        $.post("/Ajax/SaveOptionalFields", $.postify(postData), function (data) {
            $("#saveMessage_generalSettings").html("Settings saved");
            $("#ajaxLoader_generalSettings").hide();
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            $("#saveMessage_generalSettings").html("");
            $("#ajaxLoader_generalSettings").hide();
            alert(jqXHR.responseText);
        });
    });

    $("#button_saveChurch").click(function () {
        $("#saveMessage_Church").html("");
        //Send the updated data back to the server
        $("#ajaxLoader_Church").show();
        var postData = { ChurchName: $("#text_churchName").val(),
            OfficePhone: $("#text_officePhone").val(),
            OfficeEmail: $("#text_officeEmail").val(),
            Url: $("#text_url").val(),
            AddressId: $("#hidden_churchaddressId").val(),
            Address1: $("#text_churchaddress1").val(),
            Address2: $("#text_churchaddress2").val(),
            Address3: $("#text_churchaddress3").val(),
            Address4: $("#text_churchaddress4").val(),
            Lat: $("#hidden_churchlat").val(),
            Lng: $("#hidden_churchlng").val(),
            AddressType: $("#hidden_churchaddressType").val(),
            UITheme: $("#select_uiTheme").val(),
            SystemName: $("#text_systemName").val(),
            Province: $("#hidden_churchProvince").val()
        };


        $.post("/Ajax/SaveChurchContactDetails", $.postify(postData), function (data) {
            $("#saveMessage_Church").html(data.Message);
            $("#ajaxLoader_Church").hide();
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            $("#saveMessage_Church").html("");
            $("#ajaxLoader_Church").hide();
            alert(jqXHR.responseText);
        });
    });

    $("#button_saveBulkSmsDetails").click(function () {
        $("#saveMessage_BulkSmsDetails").html("");
        $("#ajaxLoader_BulkSmsDetails").show();
        var postData = { BulkSmsUsername: $("#text_bulkSmsUsername").val(),
            BulkSmsPassword: $("#text_bulkSmsPassword").val()
        };

        $.post("/Ajax/SaveBulkSmsDetails", $.postify(postData), function (data) {
            $("#saveMessage_BulkSmsDetails").html(data.Message);
            $("#ajaxLoader_BulkSmsDetails").hide();
        })
        .error(function (jqXHR, textStatus, errorThrown) {
            $("#saveMessage_BulkSmsDetails").html("");
            $("#ajaxLoader_BulkSmsDetails").hide();
            alert(jqXHR.responseText);
        });
    });

    $("#optionalFieldList").delegate(".optionalField", "click", function () {
        $("#saveMessage").html("");
        $.tmplItem(this).data.Display = $(this).prop("checked");
        if ($.tmplItem(this).data.Name == 'SuburbLookup') {
            if ($.tmplItem(this).data.Display) {
                FetchSuburbs(false);
            }
            else {
                $("#div_suburbs").hide();
            }
        }
        if ($.tmplItem(this).data.Name == 'Home Group Classification') {
            if ($.tmplItem(this).data.Display) {
                FetchHGClassifications(false);
            }
            else {
                $("#div_hgClassification").hide();
            }
        }
        if ($.tmplItem(this).data.Name == 'Home Group Events') {
            if ($.tmplItem(this).data.Display) {
                FetchHGEventTypes(false);
            }
            else {
                $("#div_hgEventType").hide();
            }
        }
    });

    $("#button_addSuburb").click(function () {
        AddSuburb();
    });

    $("#button_addHGClassification").click(function () {
        AddHGClassification();
    });

    $("#button_addHGEventType").click(function () {
        AddHGEventType();
    });

    $("#suburbList").delegate(".deleteSuburb", "click", function () {
        DeleteSuburb($.tmplItem(this).data.SuburbId);
    });

    $("#hgClassificationList").delegate(".deleteHGClassification", "click", function () {
        DeleteHGClassification($.tmplItem(this).data.GroupClassificationId);
    });

    $("#hgEventTypeList").delegate(".deleteHGEventType", "click", function () {
        DeleteHGEventType($.tmplItem(this).data.EventTypeId);
    });

    $(".optionsButton").live('mouseover mouseout', function (event) {
        if (event.type == 'mouseover') {
            $(this).css("cursor", "pointer");
        } else {
            $(this).css("cursor", "default");
        }
    });

    $("#row_suburbs").click(function () {
        if ($("#button_expandCollapseSuburbs").prop("title") == "Expand") {
            $("#button_expandCollapseSuburbs").prop("title", "Collapse");
            $("#button_expandCollapseSuburbs").prop("src", "/Content/Images/expand.png");
            $("#suburbContent").slideDown('medium');
            CollapseOthers($("#button_expandCollapseSuburbs"), false);
        }
    });

    $("#row_hgClassifications").click(function () {
        if ($("#button_expandCollapseHGClassification").prop("title") == "Expand") {
            $("#button_expandCollapseHGClassification").prop("title", "Collapse");
            $("#button_expandCollapseHGClassification").prop("src", "/Content/Images/expand.png");
            $("#hgClassificationContent").slideDown('medium');
            CollapseOthers($("#button_expandCollapseHGClassification"), false);
        }
    });

    $("#row_hgEventTypes").click(function () {
        if ($("#button_expandCollapseHGEventType").prop("title") == "Expand") {
            $("#button_expandCollapseHGEventType").prop("title", "Collapse");
            $("#button_expandCollapseHGEventType").prop("src", "/Content/Images/expand.png");
            $("#hgEventTypeContent").slideDown('medium');
            CollapseOthers($("#button_expandCollapseHGEventType"), false);
        }
    });

    $("#button_changePassword").click(function () {
        $("#saveAddressMessage").html("");
        ChangePassword();
    });

    $("#text_address1").keypress(function () {
        $("#saveAddressMessage").html("");
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
            $("#saveAddressMessage").html("");
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

    $("#text_churchaddress1").keypress(function () {
        $("#saveMessage_Church").html("");
        $("#hidden_churchaddressChosen").val("");
    });

    $("#text_churchaddress1").autocomplete({
        source: function (request, response) {
            $("#ajax_churchGpsCoordinates").show();
            var address = $("#text_churchaddress1").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_churchGpsCoordinates").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 2, 'church', true);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#text_churchaddress2").autocomplete({
        source: function (request, response) {
            $("#saveMessage_Church").html("");
            if ($("#hidden_churchaddressChosen").val() == "selected") {
                response("");
                return;
            }
            $("#ajax_churchGpsCoordinates2").show();
            var address = $("#text_churchaddress2").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_churchGpsCoordinates2").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 3, 'church', true);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#button_saveAddress").click(function () {
        $("#saveAddressMessage").html("");
        SaveAddress();
    });

    $("#addButton").click(function () {
        var site = { SiteId: 0,
            SiteName: $("#siteText").val(),
            AddressId: 0,
            Address1: '',
            Address2: '',
            Address3: '',
            Address4: '',
            Lat: 0,
            Lng: 0,
            AddressType: ''
        };
        PopulateSite(site);
    });

    $('#jqgSites').jqGrid({
        //url from wich data should be requested
        url: '/Ajax/FetchSites',
        //type of data
        datatype: 'json',
        //url access method type
        mtype: 'POST',
        //columns names
        colNames: ['SiteId', 'SiteName', 'Address1', 'Address2', 'Address3', 'Address4'],
        //columns model
        colModel: [
                    { name: 'SiteId', index: 'PersonId', hidden: true },
                    { name: 'SiteName', index: 'SiteName', align: 'left', width: 180 },
                    { name: 'Address1', index: 'Address1', align: 'left', width: 187 },
                    { name: 'Address2', index: 'Address2', align: 'left', width: 100 },
                    { name: 'Address3', index: 'Address3', align: 'left', width: 100 },
                    { name: 'Address4', index: 'Address4', align: 'left', width: 100 }
                  ],
        //pager for grid
        pager: $('#jqgpSites'),
        //number of rows per page
        rowNum: 15,
        //initial sorting column
        sortname: 'SiteName',
        //initial sorting direction
        sortorder: 'asc',
        //we want to display total records count
        viewrecords: true,
        //grid width
        width: 'auto',
        //grid height
        height: 'auto',
        onSelectRow: function (id) {
            var postData = { siteId: id };
            $.post("/Ajax/FetchSite", $.postify(postData), function (data) {
                PopulateSite(data.Site);
            });
        }
    }).navGrid('#jqgpSites', { edit: false, add: false, del: false, search: false });

    $("#text_siteaddress1").keypress(function () {
        $("#saveMessage_Sites").html("");
        $("#hidden_siteaddressChosen").val("");
    });

    $("#siteText").keypress(function () {
        $("#saveMessage_Sites").html("");
    });


    $("#text_siteaddress1").autocomplete({
        source: function (request, response) {
            $("#ajax_sitegpsCoordinates").show();
            var address = $("#text_siteaddress1").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_sitegpsCoordinates").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 2, 'site', false);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#text_siteaddress2").autocomplete({
        source: function (request, response) {
            $("#saveMessage_Sites").html("");
            if ($("#hidden_siteaddressChosen").val() == "selected") {
                response("");
                return;
            }
            $("#ajax_sitegpsCoordinates2").show();
            var address = $("#text_siteaddress2").val().replace(/ /g, "+") + ", " + $("#hidden_googleSearchRegion").val();
            Google.searchAddress(address).then(function (data) {
                $("#ajax_sitegpsCoordinates2").hide();
                response(data);
            });
        }
        ,
        minLength: 4,
        select: function (event, ui) {
            AddressSelected(ui.item.id, 3, 'site', false);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
})