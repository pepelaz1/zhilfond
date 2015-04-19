function showXmlSubdetails() {
    g_currform = 'Выгрузить XML';
    //$('.div_title_subdetails').text(g_currform);


    $('#subdetails_xml').html(
        '<div style="float:left" class="div_title_subdetails">' + g_currsubform + '</div>'
       + '<button style="margin-left:50px; margin-top:-5px" id="btn_generate_xml">Сгенерировать</button>'
       + '</div>'
    );


    $('#btn_generate_xml').button().click(function (e) {
        e.preventDefault();
        grouprow = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));
        if (grouprow.Id == undefined)
            alert('Необходимо выбрать группу домов');
        else
            window.location = "/api/xmlgeneratorapi?token="+$.cookie('token')+"&id_group=" + grouprow.Id;
    });
}
