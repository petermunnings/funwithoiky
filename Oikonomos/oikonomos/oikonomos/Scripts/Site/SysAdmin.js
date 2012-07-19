
var ChurchViewModel = function () {
    var self = this;

    self.church = ko.observable();
    self.queryString = ko.observable();

    $.get("/Ajax/InitializeChurchSettingsViewModel", self.church);

    self.createNewChurch = function () {
        var postData = ko.toJS(self.church);
        var jqxhr = $.post("/Ajax/CreateNewChurch", $.postify(postData), function (data) {
            if (data.Message == "") {
                self.showChurchFields = false;
            } else {
                ShowErrorMessage("Could not create church", data.Message);
            }
        }).error(function (jqXHR, textStatus, errorThrown) {
            SendErrorEmail("Error calling CreateNewChurch", jqXHR.responseText);
            self.showChurchFields = false;
        });
    };

    self.runQuery = function () {
        $("#jqgQueryResults").GridUnload();
        populateGrid(ko.toJS(self.queryString));
    };
};


function populateGrid(queryString) {
    $.ajax({
        type: "GET",
        url: "/SysAdmin/RunSql?queryString=" + queryString,
        dataType: "json",
        success: function (result) {
            var rowData = result.RowValues,
                columnModel = result.ColumnModel;

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

$(document).ready(function () {
    $("#accordion").accordion();
    $("#ChurchId").change(function () {
        var postData = { churchId: $(this).val() };
        $.post("/Ajax/ChangeChurchTo", $.postify(postData));
    });

    ko.applyBindings(new ChurchViewModel());
});