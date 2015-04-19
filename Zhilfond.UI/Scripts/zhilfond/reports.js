function ShowReportsSubdetails() {
   // $('.div_title_subdetails').text(g_currsubform);
    $('#gbox_grid_groupreptemplates').css('display', 'block');
    buildReportParams();
}

function buildReportParams() {
    $('#subdetails_report').html(
         '<div style="float:left" class="div_title_subdetails">' + g_currsubform + '</div>'
        + '<button style="margin-left:50px; margin-top:-5px" id="btn_build">Сформировать</button>'
        + '<div id="report_accordion" style="width:525px"></div>'
        + '<div id="SaveReportDialog" style="width:580">' +
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

    $('#btn_build').button().click(function (e) {
        groupreptemplaterow = $("#grid_groupreptemplates").getRowData($("#grid_groupreptemplates").getGridParam("selrow"));
        grouprow = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));
        if (grouprow.Id == null)
        {
            alert("Не выбрана группа");
            return false;
        }
        if (groupreptemplaterow.Id == null) {
            alert("Не выбран шаблон отчета");
            return false;
        }
        e.preventDefault();

        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }

        today = dd + '.' + mm + '.' + yyyy;

        $('#ReportFileName').val(
        groupreptemplaterow.template_name + '-' + today + '.xlsx'
        );

        $("#btn_report_cancel").button().click(function (e) {
            $("#SaveReportDialog").dialog("close");
        });

        $("#btn_report_save").button().click(function (e) {
            window.location = "/api/reportbuilderapi?token=" + $.cookie('token') + "&id_group=" + grouprow.Id + "&id_template=" + groupreptemplaterow.Id + "&fileName=" + $('#ReportFileName').val();
            $("#SaveReportDialog").dialog("close");
        });

        $("#SaveReportDialog").dialog("open");

        //window.location = "/api/reportbuilderapi?token=" + $.cookie('token') + "&id_group=" + grouprow.Id + "&id_template=" + groupreptemplaterow.Id;
        return false;
    });

    groupreptemplaterow = $("#grid_groupreptemplates").getRowData($("#grid_groupreptemplates").getGridParam("selrow"));
    grouprow = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));
  
    $.get("/api/reportparamsapi/?id_group=" + groupreptemplaterow.Id, function (data) {
        var curform = "";
        var curcat = "";
        var n = 0;
        var m = 0;
        var k = 0;
        data.forEach(function (entry) {
            if (entry.Form != curform) {
                curform = entry.Form;
                n++;
                $('#report_accordion').append(
                       '<div class="group" style="margin-top:5px">'
                     + '  <h3>' + entry.Form + '</h3>'
                     + '  <div id="rep_form_accordion_' + n + '" style="height:207px">'
                     + '  </div>'
                     + '</div>'
                    );
                curcat = "";
            }
            if (entry.Category != curcat) {
                curcat = entry.Category;
                m++;
                $('#rep_form_accordion_' + n).append(
                    '<div class="group">'
                    + '  <h3>' + entry.Category + '</h3>'
                    + '  <div id="rep_field_' + m + '" style="height:100px">'
                    + '  </div>'
                + '</div>'
                );             
            }

            var chosen = "";
            if (entry.Chosen == true)
                chosen = "checked";
              
            k++;
            $("#rep_field_" + m).append(
                '<div style="margin-bottom:3px">'
                + '<input style="float:left" type="checkbox" id="check_' + k + '" id_group="' + groupreptemplaterow.Id + '" id_form="' + entry.Id_form
                + '" id_field="' + entry.Id_field + '" id_category ="' + entry.Id_category + '" '+chosen+'/><label for="check_' + k + '"></label>'
                + '<span  style="margin-left:3px">' + entry.Field + '</span>'
                +'</div>'
                );
                

        });

        $("#report_accordion").accordion({
            collapsible: true,
            header: "> div > h3",
            heightStyle: "content"
        }).sortable({
            axis: "y",
            handle: "h3",
            stop: function (event, ui) {
                // IE doesn't register the blur when sorting
                // so trigger focusout handlers to remove .ui-state-focus
                ui.item.children("h3").triggerHandler("focusout");
            }
        });

        for (var i = 0; i <= n; i++) {
            $("#rep_form_accordion_" + i).accordion({
                collapsible: true,
                header: "> div > h3",
                heightStyle: "content"
           //     heightStyle: "content"
            }).sortable({
                axis: "y",
                handle: "h3",
                stop: function (event, ui) {
                    // IE doesn't register the blur when sorting
                    // so trigger focusout handlers to remove .ui-state-focus
                    ui.item.children("h3").triggerHandler("focusout");
                }
            });
        }

        for (var i = 0; i <= k; i++) {
            $("#check_" + i).button().click(function (event) {
            
                var rp = {};
                rp.Id_group = $(this).attr("id_group");
                rp.Id_form = $(this).attr("id_form");
                rp.Id_field = $(this).attr("id_field");
                rp.Id_category = $(this).attr("id_category");
                rp.Chosen = $(this).prop("checked");
            
                $.ajax({
                    url: "/api/reportparamsapi/",
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    data: JSON.stringify(rp),
                    dataType: "json",
                //    async: false,
                    success: function (response) {
                    //    console.log('ok');
                    },
                    error: function (x, e) {
                        alert('Ошибка изменения значения параметра');
                    }
                });
            });
        }
    }); 
}