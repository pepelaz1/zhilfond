
function ShowGroupReportsSubdetails() {
    $('.div_title_subdetails').text(g_currsubform);
    buildGroupReport();
}

function buildGroupReport() {

    $.get("/api/reporttemplatesapi?type=2", function (data) {

        $('#div_detailgroupreports_buttons').html(
            '<table><tbody></tbody></table>' +
            '<div id="SaveReportDialog" style="width:580">' +
            '<table style="width:100%">' +
            '<tr>' +
            '<td style="width:100%"><span style="vertical-align:central">Введите название отчета</span></td>' +
            '</tr><tr>' +
            '<td><input id="ReportFileName" type="text" style="width:100%"/></td>' +
            '</tr><tr>' +
            '<td><button style="font-size: 0.9em" id="btn_report_save" type="button">Сохранить</button>' +
            '<button style="font-size: 0.9em" id="btn_report_cancel" type="button">Отмена</button></td>' +
            '</tr>' +
            '</table>' +
            '</div>'
            );

        $("#SaveReportDialog").dialog({
            autoOpen: false,
            width: 600,
            title: "Сохранить как"
        });

        data.forEach(function (entry) {

            if (entry.Reportname != null) {

                $('#div_detailgroupreports_buttons table tbody').append(
                            '<tr><td>' +
                            '<button style="font-size: 0.9em; height:auto" id="btn_report_' + entry.Id + '" type="button">' + entry.Reportname + '</button>' +
                            '</td></tr>'
                         );

                $("#btn_report_" + entry.Id).button().click(function (e) {
                    var rowData = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));
                    var reportName = $(this).text();
                    var templateId = entry.Id;
                    //templateId = 4;
                    if (rowData != undefined && rowData.Id != undefined && rowData.Id != 0) {
                        e.preventDefault();

                        $('#ReportFileName').val(
                        //'Сведения о состоянии объектов коммунальной и инженерной инфраструктуры-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
                        reportName + /*'-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter +*/ '.xlsx'
                        );

                        $("#btn_report_cancel").button().click(function (e) {
                            $("#SaveReportDialog").dialog("close");
                        });

                        $("#btn_report_save").button().click(function (e) {
                            window.location = "/api/detailreportsapi?type=2&token=" + $.cookie('token') + "&elemId=" + rowData.Id + "&fileName=" + $('#ReportFileName').val() + "&tmplId=" + templateId;
                            $("#SaveReportDialog").dialog("close");
                        });

                        $("#SaveReportDialog").dialog("open");

                        //window.location = "/api/detailreportsapi?type=3&token=" + $.cookie('token') + "&houseId=" + rowData.Id;
                    }
                    else {
                        alert("Перед формированием отчёта необходимо выбрать группу");
                    }
                });
            }
        });
    });
}

//function buildGroupReport() {
//    $('#div_detailgroupreports_buttons').html(
//    '<table><tbody><tr><td>' +
//    '<button style="font-size: 0.9em; height:40px" id="btn_report_F4" type="button">Сведения о предоставлении гражданам жилых помещений (Форма №4-жилфонд)</button>' +
//    '</td></tr><tr><td>' +
//    '<button style="font-size: 0.9em; height:40px" id="btn_report_F1" type="button">Сведения об объектах инфраструктуры муниципального образования (Форма № 1-МО)</button>' +
//    '</td></tr><tr><td>' +
//    '<button style="font-size: 0.9em; height:40px" id="btn_report_F1_control" type="button">Сведения об осуществлении государственного контроля (надзора) и муниципального контроля (Форма №1-контроль)</button>' +
//    '</td></tr><tr><td>' +
//    '<button style="font-size: 0.9em; height:40px" id="btn_report_F22" type="button">Сведения о работе жилищно-коммунальных организаций в условиях реформы (Форма №22-ЖКХ (сводная))</button>' +
//    '</td></tr></tbody></table>' +
//    '<div id="SaveReportDialog" style="width:580">' +
//    '<table style="width:100%">' +
//    '<tr>' +
//    '<td style="width:100%"><span style="vertical-align:central">Введите название отчета</span></td>' +
//    '</tr><tr>' +
//    '<td><input id="ReportFileName" type="text" style="width:100%"/></td>' +
//    '</tr><tr>' +
//    '<td><button style="font-size: 0.9em" id="btn_report_save" type="button">Сохранить</button>' +
//    '<button style="font-size: 0.9em" id="btn_report_cancel" type="button">Отмена</button></td>' +
//    '</tr>' +
//    '</table>' +
//    '</div>'
//    );

//    $("#SaveReportDialog").dialog({
//        autoOpen: false,
//        width: 600,
//        title: "Сохранить как"
//    });

//    $("#btn_report_F4").button().click(function (e) {
//        e.preventDefault();

//        $('#ReportFileName').val(
//        'Сведения о предоставлении гражданам жилых помещений (Форма №4-жилфонд).xlsx'
//        );

//        $("#btn_report_cancel").button().click(function (e) {
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#btn_report_save").button().click(function (e) {
//            window.location = "/api/detailgroupreportsapi?token=" + $.cookie('token') + "&fileName=" + $('#ReportFileName').val();
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#SaveReportDialog").dialog("open");
//    });

//    $("#btn_report_F1").button().click(function (e) {
//        e.preventDefault();

//        $('#ReportFileName').val(
//        'Сведения об объектах инфраструктуры муниципального образования (Форма № 1-МО).xlsx'
//        );

//        $("#btn_report_cancel").button().click(function (e) {
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#btn_report_save").button().click(function (e) {
//            window.location = "/api/detailgroupreportsapi?token=" + $.cookie('token') + "&fileName=" + $('#ReportFileName').val();
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#SaveReportDialog").dialog("open");
//    });

//    $("#btn_report_F1_control").button().click(function (e) {
//        e.preventDefault();

//        $('#ReportFileName').val(
//        'Сведения об осуществлении государственного контроля (надзора) и муниципального контроля (Форма №1-контроль).xlsx'
//        );

//        $("#btn_report_cancel").button().click(function (e) {
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#btn_report_save").button().click(function (e) {
//            window.location = "/api/detailgroupreportsapi?token=" + $.cookie('token') + "&fileName=" + $('#ReportFileName').val();
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#SaveReportDialog").dialog("open");
//    });

//    $("#btn_report_F22").button().click(function (e) {
//        e.preventDefault();

//        $('#ReportFileName').val(
//        'Сведения о работе жилищно-коммунальных организаций в условиях реформы (Форма №22-ЖКХ (сводная)).xlsx'
//        );

//        $("#btn_report_cancel").button().click(function (e) {
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#btn_report_save").button().click(function (e) {
//            window.location = "/api/detailgroupreportsapi?token=" + $.cookie('token') + "&fileName=" + $('#ReportFileName').val();
//            $("#SaveReportDialog").dialog("close");
//        });

//        $("#SaveReportDialog").dialog("open");
//    });
//}