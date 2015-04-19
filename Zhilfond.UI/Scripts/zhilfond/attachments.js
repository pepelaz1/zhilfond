var api_attachments = "/api/attachmentsapi/";


//function showAttachDetails() {
//    // g_currform = 'Прикрепленные файлы';
//    $('.div_title_attach').text('Прикрепленные файлы');

//    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
//    $('#details_attach').dialog('option', 'title', houserow.nasp + ", " + houserow.raion + " р-н, " + houserow.street + " " + houserow.number + houserow.letter);
//    $("#grid_attachments").jqGrid('setGridParam', { url: api_attachments + '?id_house=' + houserow.Id }).trigger('reloadGrid');

//    $('#btn_attach').attr('disabled', !canEditHouse($.cookie('token'), g_id_house));

//    var $td = $('#del_grid_attachments');
//    if (canEditHouse($.cookie('token'), g_id_house))
//        $td.show();
//    else
//        $td.hide();
//}

$('#btn_attach').button().click(function () {
    var files = $("#id_file_attach").get(0).files;
    if (files && files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
            }
            houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
            //console.log(houserow.Id);
            $.ajax({
                type: "POST",
                url: "/api/attachmentsapi?id_house=" + houserow.Id,
                contentType: false,
                processData: false,
                data: data,
                success: function (results) {
                    $("#grid_attachments").trigger('reloadGrid');
                }, error: function (x, e) {
                    alert('Ошибка вложения файлов');
                }
            });
        } else {
            alert("This browser doesn't support HTML5 multiple file uploads!");
        }
    }
    return false;
}).attr("disabled", !canEditHouse($.cookie('token'), g_id_house));

//----- grid_attachments
$("#grid_attachments").jqGrid({
    url: api_attachments,
    datatype: 'json',
    mtype: 'GET',
    pager: '#pager_attachments',
    pagerpos: 'center',
    sortable: false,
    height: 150,
    shrinkToFit: true,
    viewrecords: true,
    rowNum: 10000,
    colNames: ['ИД', 'Порядок', 'Имя файла'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 40, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Order', index: 'Order', editable: true, width: 100, editrules: { required: false }, searchoptions: { sopt: ['cn'] } },
               { name: 'Filename', index: 'Filename', editable: true, edittype: 'text', width: 390, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
    ],
    caption: '',
    loadError: function (xhr, st, err) {
        if (xhr.status == 401) {
            alert(xhr);
        }
    },
    loadComplete: function () {
    },
    onSelectRow: function (ids) {
       /* var grid = $("#grid_groups")
        var selectedRow = grid.getGridParam("selrow");
        rowData = grid.getRowData(selectedRow);
        // console.log(api_group_houses + "?id_group=" + rowData.Id);
        $("#grid_group_houses").jqGrid('setGridParam', { url: api_group_houses + "?id_group=" + rowData.Id }).trigger('reloadGrid');
        refreshDetails(g_currsubform);*/
    },
    ondblClickRow: function (rowid, iRow, iCol, e) { 
        var row = $("#grid_attachments").getRowData($("#grid_attachments").getGridParam("selrow"));
        if (row.Id != undefined)
            window.location = "/api/attachmentsapi/" + row.Id;
    }
})


function updateDialog_attachments(action) {
    return {
        url: api_attachments
            , closeAfterAdd: true
            , closeAfterEdit: true
            , afterShowForm: function (formId) { }
            , modal: true
            , onclickSubmit: function (params) {
                var grid = $("#grid_attachments")
                var selectedRow = grid.getGridParam("selrow");
                rowData = grid.getRowData(selectedRow);
                params.url += rowData.Id;
                params.mtype = action;
            }
            , width: "500"
    };
}


$("#grid_attachments").jqGrid('navGrid', '#pager_attachments',
    { add: false, edit: true, del: true, refresh: true, search: false },
    updateDialog_attachments('PUT'),
    updateDialog_attachments('POST'),
    updateDialog_attachments('DELETE'),
     {
         onReset: function () {
             var jqModal = true, gridid = grid[0].id;
             $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
         },
         onSearch: function () {
             var jqModal = true, gridid = grid[0].id;
             $.jgrid.hideModal("#searchmodfbox_" + gridid,
         { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
         }
     }
 );


$("#grid_attachments").jqGrid('navGrid', '#pager_attachments', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});


$("#pager_attachments").find("#pager_attachments_center").hide();