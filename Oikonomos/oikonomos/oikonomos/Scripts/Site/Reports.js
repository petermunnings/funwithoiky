
$(document).ready(function () {

    $("#button_churchList").click(function () {
        window.location.replace("/Home/ReportGrid");
    });
    $("#button_churchMap").click(function () {
        window.location.replace("/Home/ReportsMap");
    });
    $("#button_adminReports").click(function () {
        window.location.replace("/Home/ReportsAdmin");
    });

})