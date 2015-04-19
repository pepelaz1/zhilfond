var g_hash = new Hashtable();
var g_isnew = false;
var g_currform = 'Основные данные'
var g_id_house = 1905;
var g_currsubform = 'Построить отчет';

function makeFormList() {
  //  console.log($.cookie('token'));
    $.get("/api/formsapi/?token="+$.cookie('token'), function (data) {
        
        data.rows.forEach(function (entry) {
            if (entry.Title != 'Основные данные')
                $(".menu_main_form").append('<li class="menu_main_item"><a href="#">' + entry.Title + '</a></li>');
        });
        

        if (isAdmin($.cookie('token')))
            $("#menu_bottom_part").append('<li class="menu_main_item"><a href="#">Аудит</a></li>');

        $(".menu_main_form").menu();
        $(".menu_main").menu();

        updateUnreadMsgCount();
    });
}

function updateUnreadMsgCount() {
    $.get("/api/umessagesapi/?token=" + $.cookie('token'), function (data) {
        if (data.Count > 0)
            $("#msg_menu").text("Сообщения (" + data.Count + ")");
        else
            $("#msg_menu").text("Сообщения");
    });
}


function displayDetailsDiv(id) {
    $('#details_main').css('display', 'none');
    $('#details').css('display', 'none');
    $('#details_messages').css('display', 'none');
    //$('#details_attach').css('display', 'none');
    $('#details_check_signature').css('display', 'none');
    $('#details_group_operations').css('display', 'none');
    $('#details_import').css('display', 'none');
    $('#details_xsd').css('display', 'none');
    $('#details_audit').css('display', 'none');
    $('#details_reports').css('display', 'none');
    $('#details_genreports').css('display', 'none');
    $('#gbox_grid_groupreptemplates').css('display', 'none');

    $('#' + id).css('display', 'block');
}

function displaySubdetailsDiv(id) {
    $('#subdetails_report').css('display', 'none');
    $('#subdetails_map').css('display', 'none');
    $('#subdetails_xml').css('display', 'none');
    $('#subdetails_group_reports').css('display', 'none');
    $('#gbox_grid_groupreptemplates').css('display', 'none');

    $('#' + id).css('display', 'block');   
}

$(".menu_main").on("menuselect", function (event, ui) {
    //console.log(ui);
    refreshDetails(ui.item.text());
})

$(".menu_main_form").on("menuselect", function (event, ui) {
   // if ($("#gridMain").getGridParam("selrow") == undefined)
  //      return;

    displayDetailsDiv('details');
    showDetails(ui.item.text());
});

function refreshDetails(a) {
   // if ($("#gridMain").getGridParam("selrow") == undefined)
   //     return;
    // var a = ui.item.text();
    if ( a != g_currform)
        g_id_parent = 0;

    if (a == 'Основные данные') {
        displayDetailsDiv('details_main');
        //showMainDetails();
        showDetails(a);
    } else if (a.indexOf("Сообщения") != -1) {      
        displayDetailsDiv('details_messages');
        showMessagesDetails();
    } else if (a == 'Прикрепленные файлы') {

        //console.log(houserow.nasp + ", " + houserow.raion + " р-н, " + houserow.street + " " + houserow.number + houserow.letter);

        var dialogOptions = {
            "title": houserow.nasp + ", " + houserow.raion + " р-н, " + houserow.street + " " + houserow.number + houserow.letter,
            "width": 420,
            "resize": function (event, ui) {
                $('#grid_attachments').setGridWidth($(this).outerWidth() - 30);
                //$('#grid_attachments').setGridWidth($(this).outerWidth() - 30);
            }
        };
        var dialogExtendOptions = {
            //"closable": $("#button-close").is(":checked"),
            //"maximizable": $("#button-maximize").is(":checked"),
            "minimizable": true
            //"minimizeLocation": $("#my-form [name=minimizeLocation]:checked").val() || false,
            //"collapsable": $("#button-collapse").is(":checked"),
            //"dblclick": $("#my-form [name=dblclick]:checked").val() || false,
            //"titlebar": $("#my-form [name=titlebar]:checked").val() || false
        };
        $('#details_attach').dialog(dialogOptions).dialogExtend(dialogExtendOptions); 
        $('#grid_attachments').setGridWidth(390);

        //$('#details_attach').dialog({ width: 420 });
        showAttachDetails();
    } else if (a == 'Проверка ЭЦП') {
        displayDetailsDiv('details_check_signature');
        showCheckSignatureDetails();
    } else if (a == 'Групповые операции') {
        displayDetailsDiv('details_group_operations');
        showGroupOperationsDetails();
    } else if (a == 'Импорт') {
        displayDetailsDiv('details_import');
        showImportDetails();
    } else if (a == 'Выгрузить XML') {
        g_currsubform = a
        displaySubdetailsDiv('subdetails_xml');
        showXmlSubdetails();
    } else if (a == 'Создать XSD') {
        displayDetailsDiv('details_xsd');
        showXsdDetails();
    } else if (a == 'Типовые отчеты') {
        displayDetailsDiv('details_reports');
        showReportsDetails();
    } else if (a == 'Аудит') {
        displayDetailsDiv('details_audit');
        showAuditDetails();
    } else if (a == 'Сформированные отчёты') {
        displayDetailsDiv('details_genreports');
        showGenReportsDetails();
    }
    else if (a == 'Генератор отчетов') {
        g_currsubform = a;
        displaySubdetailsDiv('subdetails_report');
        ShowReportsSubdetails();
    } else if (a == 'Показать на карте') {
        g_currsubform = a;
        displaySubdetailsDiv('subdetails_map');
        ShowMapSubdetails();
    } else if (a == 'Типовые отчеты по группе') {
        g_currsubform = a;
        displaySubdetailsDiv('subdetails_group_reports');
        ShowGroupReportsSubdetails();
    } else {
        displayDetailsDiv('details');
        showDetails(a);
    }

    updateUnreadMsgCount();
}
