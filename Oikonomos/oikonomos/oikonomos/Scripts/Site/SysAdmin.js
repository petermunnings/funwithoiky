
var ChurchViewModel = function () {
    var self = this;

    self.church = ko.observable();
    self.queryString = ko.observable();

    $.get("/Ajax/InitializeChurchSettingsViewModel", self.church);

    self.createNewChurch = function () {
        try {
            if (self.church().ChurchName == null || self.church().ChurchName == '') {
                ShowErrorMessage("Error Creating New Church", "Church name is required");
                return;
            }
            if (self.church().ContactFirstname == null || self.church().ContactFirstname == '') {
                ShowErrorMessage("Error Creating New Church", "Contact Firstname is required");
                return;
            }
            if (self.church().ContactSurname == null || self.church().ContactSurname == '') {
                ShowErrorMessage("Error Creating New Church", "Contact Surname is required");
                return;
            }
            if (self.church().OfficePhone == null || self.church().OfficePhone == '') {
                ShowErrorMessage("Error Creating New Church", "Office Phone No is required");
                return;
            }
            if (self.church().OfficeEmail == null || self.church().OfficeEmail == '') {
                ShowErrorMessage("Error Creating New Church", "Office email is required");
                return;
            }
            if (self.church().Url == null)
                self.church().Url = '';
            if (self.church().Address1 == null)
                self.church().Address1 = '';
            if (self.church().Address2 == null)
                self.church().Address2 = '';
            if (self.church().Address3 == null)
                self.church().Address3 = '';
            if (self.church().Address4 == null)
                self.church().Address4 = '';
            if (self.church().Province == null)
                self.church().Province = 'Gauteng';
            var postData = ko.toJS(self.church);
            $.post("/Ajax/CreateNewChurch", $.postify(postData), function (data) {
                if (data.Message == "Church was succesfully created...") {
                    self.showChurchFields = false;
                    ShowInfoMessage('Success', data.Message);
                } else {
                    ShowErrorMessage("Could not create church", data.Message);
                }
            }).error(function (jqXhr, textStatus, errorThrown) {
                SendErrorEmail("Error calling CreateNewChurch", jqXhr.responseText);
                self.showChurchFields = false;
            });
        } catch (e) {
            ShowErrorMessage("Unexpected error occured", e.message);
            return;
        } 
    };


    self.runQuery = function () {
        $("#jqgQueryResults").GridUnload();
        populateGrid(ko.toJSON(self.queryString));
    };
};


function populateGrid(queryString) {
    $.ajax({
        type: "GET",
        url: "/SysAdmin/RunSql?queryString=" + queryString,
        dataType: "json",
        success: function (result) {
            var rowData = result.RowValues,
                columnModel = result.ColumnModel,
                message = result.Message;

            if (message != null) {
                alert(message);
                return;
            }
            
            $("#jqgQueryResults").jqGrid({
                datatype: 'local',
                data: rowData,
                gridview: true,
                rowNum: 15,
                colModel: columnModel,
                height: "auto",
                loadError: function (xhr, status, error) {
                    alert('error');
                }
            })
            .navGrid('#jqgpQueryResults', { edit: false, add: false, del: false, search: false });
        },
        error: function (x, e) {
            alert(x.readyState + " " + x.status + " " + e.msg);
        }
    });
}


EmailTemplate = {
    Fetch: function () {
        var postData = { churchId: $("#SelectedChurchId").val(), emailTemplateId: $("#EmailTemplateId").val() };
        var dfr = $.Deferred();
        $.post("/Ajax/FetchChurchEmailTemplate", $.postify(postData), function (data) {
            dfr.resolve(data.EmailTemplate);
        })
            .error(function (jqXhr, textStatus, errorThrown) {
                SendErrorEmail("Error calling DeleteStandardComment", jqXhr.responseText);
                dfr.reject();
            });
        return dfr.promise();
    },
    Save: function () {
        var postData = {
            churchId: $("#SelectedChurchId").val(),
            emailTemplateId: $("#EmailTemplateId").val(),
            template: tinyMCE.get('elm1').getContent()
        };
        $.post("/Ajax/SaveChurchEmailTemplate", $.postify(postData), function () {
                ShowInfoMessage("Success", "Email template was succesfully saved");
            })
            .error(function (jqXhr) {
                SendErrorEmail("Error calling DeleteStandardComment", jqXhr.responseText);
            });
    }
};

function SetupMessageStatusGrid() {
    $('#jqgMessageStatusResults').jqGrid({
        url: '/Ajax/FetchMessageStatusResults',
        datatype: 'json',
        mtype: 'POST',
        colNames: ['MessageRecepientId', 'Sent', 'Status', 'From', 'To', 'Subject', 'StatusDetail'],
        colModel: [
                    { name: 'MessageRecepientId', index: 'MessageRecepientId', align: 'left', width: 0, hidden: true },
                    { name: 'Sent', index: 'Sent', align: 'left', width: 150, search: true },
                    { name: 'Status', index: 'Status', align: 'left', width: 150, search: true },
                    { name: 'From', index: 'From', align: 'left', width: 125, search: true },
                    { name: 'To', index: 'To', align: 'left', width: 125, search: true },
                    { name: 'Subject', index: 'Subject', align: 'left', width: 170, search: true },
                    { name: 'StatusMessage', index: 'StatusMessage', align: 'left', width: 250, search: false }
        ],
        rowNum: 25,
        sortname: 'Sent',
        sortorder: 'desc',
        width: 'auto',
        height: 'auto'
    }).navGrid('#jqgpMessageStatusResults', { edit: false, add: false, del: false, search: true });

    $('#jqgMessageStatusResults').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

}

$(document).ready(function () {
    $("#accordion").accordion();
    $("#ChurchId").change(function () {
        var postData = { churchId: $(this).val() };
        $.post("/Ajax/ChangeChurchTo", $.postify(postData));
    });

    ko.applyBindings(new ChurchViewModel());

    tinymce.init(
    {
        mode: "specific_textareas",
        editor_selector: "mceSysAdminEditor",
        plugins: 'link image code',
        convert_urls: false
    });


    EmailTemplate.Fetch().then(function (emailTemplate) {
        tinyMCE.get('elm1').setContent(emailTemplate);
        return false;
    });

    $("#EmailTemplateId").change(function () {
        EmailTemplate.Fetch().then(function (emailTemplate) {
            tinyMCE.get('elm1').setContent(emailTemplate);
            return false;
        });
    });

    $("#SelectedChurchId").change(function () {
        EmailTemplate.Fetch().then(function (emailTemplate) {
            tinyMCE.get('elm1').setContent(emailTemplate);
            return false;
        });
    });

    $("#SaveEmailTemplate").click(function () {
        EmailTemplate.Save();
    });

    SetupMessageStatusGrid();

});