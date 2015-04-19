function showReportsDetails() {
    g_currform = 'Типовые отчеты';
    $('.div_title_details').text(g_currform);

    buidDetailReport();
}

function buidDetailReport() {
    $.get("/api/reporttemplatesapi?type=1", function (data) {

            $('#div_detailreports_buttons').html(
                '<table><tbody><!--<tr><td>' +
                '<button style="font-size: 0.9em" id="btn_report_ODS_AH" type="button">Электронный паспорт многоквартирного дома</button>' +
                '</td></tr><tr><td>' +
                '<button style="font-size: 0.9em" id="btn_report_ODS_SFH" type="button">Электронный паспорт жилого дома</button>' +
                '</td></tr><tr><td>' +
                '<button style="font-size: 0.9em" id="btn_report_II" type="button">Сведения о состоянии объектов коммунальной и инженерной инфраструктуры</button>' +
                '</td></tr>--></tbody></table>' +
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

                    $('#div_detailreports_buttons table tbody').append(
                                '<tr><td>' +
                                '<button style="font-size: 0.9em" id="btn_report_' + entry.Id + '" type="button">' + entry.Reportname + '</button>' +
                                '</td></tr>'
                             );

                    $("#btn_report_" + entry.Id).button().click(function (e) {
                        var list = $("#gridMain");
                        var selectedRow = list.getGridParam("selrow");
                        var rowData = list.getRowData(selectedRow);
                        var reportName = $(this).text();
                        var templateId = entry.Id;
                        //templateId = 4;
                        if (rowData != undefined && rowData.Id != undefined && rowData.Id != 0) {
                            e.preventDefault();

                            $('#ReportFileName').val(
                            //'Сведения о состоянии объектов коммунальной и инженерной инфраструктуры-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
                            reportName + '-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
                            );

                            $("#btn_report_cancel").button().click(function (e) {
                                $("#SaveReportDialog").dialog("close");
                            });

                            $("#btn_report_save").button().click(function (e) {
                                window.location = "/api/detailreportsapi?type=1&token=" + $.cookie('token') + "&elemId=" + rowData.Id + "&fileName=" + $('#ReportFileName').val() + "&tmplId=" + templateId;
                                $("#SaveReportDialog").dialog("close");
                            });

                            $("#SaveReportDialog").dialog("open");

                            //window.location = "/api/detailreportsapi?type=3&token=" + $.cookie('token') + "&houseId=" + rowData.Id;
                        }
                        else {
                            alert("Перед формированием отчёта необходимо выбрать дом");
                        }
                    });
                }
            });
    });

    //$('#div_detailreports_buttons').html(
    //'<table><tbody><tr><td>' +
    //'<button style="font-size: 0.9em" id="btn_report_ODS_AH" type="button">Электронный паспорт многоквартирного дома</button>' +
    //'</td></tr><tr><td>' +
    //'<button style="font-size: 0.9em" id="btn_report_ODS_SFH" type="button">Электронный паспорт жилого дома</button>' +
    //'</td></tr><tr><td>' +
    //'<button style="font-size: 0.9em" id="btn_report_II" type="button">Сведения о состоянии объектов коммунальной и инженерной инфраструктуры</button>' +
    //'</td></tr></tbody></table>' +
    //'<div id="SaveReportDialog" style="width:580">' +
    //'<table style="width:100%">' +
    //'<tr>' +
    //'<td style="width:100%"><span style="vertical-align:central">Введите название отчета</span></td>' +
    //'</tr><tr>' +
    //'<td><input id="ReportFileName" type="text" style="width:100%"/></td>' +
    //'</tr><tr>' +
    //'<td><button style="font-size: 0.9em" id="btn_report_save" type="button">Сохранить</button>'+
    //'<button style="font-size: 0.9em" id="btn_report_cancel" type="button">Отмена</button></td>' +
    //'</tr>' +
    //'</table>'+
    //'</div>'
    //);

    //$("#btn_report_ODS_AH").button().click(function (e) {
    //    var list = $("#gridMain");
    //    var selectedRow = list.getGridParam("selrow");
    //    var rowData = list.getRowData(selectedRow);        
    //    if (rowData != undefined && rowData.Id != 0) {
    //        e.preventDefault();
            
    //        $('#ReportFileName').val(
    //        'Электронный паспорт многоквартирного дома-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
    //        );

    //        $("#btn_report_cancel").button().click(function (e) {
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#btn_report_save").button().click(function (e) {
    //            window.location = "/api/detailreportsapi?type=1&token=" + $.cookie('token') + "&houseId=" + rowData.Id + "&fileName=" + $('#ReportFileName').val();
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#SaveReportDialog").dialog("open");
            
    //        //window.location = "/api/detailreportsapi?type=1&token=" + $.cookie('token') + "&houseId=" + rowData.Id;
    //    }
    //    else {
    //        alert("Перед формированием отчёта необходимо выбрать дом");
    //    }
    //});

    //$("#btn_report_ODS_SFH").button().click(function (e) {
    //    var list = $("#gridMain");
    //    var selectedRow = list.getGridParam("selrow");
    //    var rowData = list.getRowData(selectedRow);
    //    if (rowData != undefined && rowData.Id != 0) {
    //        e.preventDefault();

    //        $('#ReportFileName').val(
    //        'Электронный паспорт жилого дома-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
    //        );

    //        $("#btn_report_cancel").button().click(function (e) {
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#btn_report_save").button().click(function (e) {
    //            window.location = "/api/detailreportsapi?type=2&token=" + $.cookie('token') + "&houseId=" + rowData.Id + "&fileName=" + $('#ReportFileName').val();
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#SaveReportDialog").dialog("open");

    //        //window.location = "/api/detailreportsapi?type=2&token=" + $.cookie('token') + "&houseId=" + rowData.Id;
    //    }
    //    else {
    //        alert("Перед формированием отчёта необходимо выбрать дом");
    //    }
    //});

    //$("#btn_report_II").button().click(function (e) {
    //    var list = $("#gridMain");
    //    var selectedRow = list.getGridParam("selrow");
    //    var rowData = list.getRowData(selectedRow);
    //    var reportName = $(this).text();
    //    var templateId = $(this).attr("tmplId");
    //    templateId = 4;
    //    if (rowData != undefined && rowData.Id != 0) {
    //        e.preventDefault();

    //        $('#ReportFileName').val(
    //        //'Сведения о состоянии объектов коммунальной и инженерной инфраструктуры-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
    //        reportName + '-' + rowData.nasp + ' ' + rowData.raion + ' ' + rowData.street + ' ' + rowData.number + ' ' + rowData.letter + '.xlsx'
    //        );

    //        $("#btn_report_cancel").button().click(function (e) {
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#btn_report_save").button().click(function (e) {
    //            window.location = "/api/detailreportsapi?type=1&token=" + $.cookie('token') + "&elemId=" + rowData.Id + "&fileName=" + $('#ReportFileName').val() + "&tmplId=" + templateId;
    //            $("#SaveReportDialog").dialog("close");
    //        });

    //        $("#SaveReportDialog").dialog("open");

    //        //window.location = "/api/detailreportsapi?type=3&token=" + $.cookie('token') + "&houseId=" + rowData.Id;
    //    }
    //    else {
    //        alert("Перед формированием отчёта необходимо выбрать дом");
    //    }
    //});
}

