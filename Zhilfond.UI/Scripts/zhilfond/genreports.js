function showGenReportsDetails() {
    g_currform = 'Сформированные отчёты';
    $('.div_title_details').text(g_currform);
  
    $("#main_grid_genreports").trigger('reloadGrid');
}

//----- grid_genreports
var api_genreports = "/api/genreportsapi/";


$("#main_grid_genreports").jqGrid({
    url: api_genreports + "?token=" + $.cookie('token'),
    datatype: 'json',
    mtype: 'GET',
    pager: '#main_pager_genreports',
    pagerpos: 'center',
    sortable: true,
    height: 375,
    width: 1010,
    shrinkToFit: false,
    viewrecords: true,
    gridview: true,
    rowNum: 10000,
    colNames: ['ИД', 'Имя файла','Создан','Тип','Подпись','Автор'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 25, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Filename', index: 'Filename', editable: true, edittype: 'text', width: 570, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Created', index: 'Created', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Type', index: 'Type', editable: true, edittype: 'text', width: 40, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Signature', index: 'Signature', hidden:true, editable: true, edittype: 'text', width: 40, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Author', index: 'Type', editable: true, edittype: 'text', width: 200, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
    ],
    caption: 'Построенные отчеты',
    loadError: function (xhr, st, err) {
        if (xhr.status == 401) {
            alert(xhr);
        }
    },
    loadComplete: function () {
    },
    onSelectRow: function (ids) {
        var rowData = $("#main_grid_genreports").getRowData(ids);
    },
    ondblClickRow: function (rowid, iRow, iCol, e) {
        var row = $("#main_grid_genreports").getRowData($("#main_grid_genreports").getGridParam("selrow"));
        if (row.Id != undefined)
            window.location = "/api/genreportsapi/" + row.Id;
    }
})



//$("#grid_values").jqGrid('filterToolbar');

function updateDialog_gengroups(action) {
    return {
        url: api_gengroups
            , closeAfterAdd: true
            , closeAfterEdit: true
            , afterShowForm: function (formId) { }
            , modal: true
            , onclickSubmit: function (params) {

             /*  var grid = $("#grid_groups")
                var selectedRow = grid.getGridParam("selrow");
                rowData = grid.getRowData(selectedRow);
                params.url += rowData.Id;
                params.url += "?token=" + $.cookie('token')
                //console.log(params.url);
                params.mtype = action;*/
            }
            , width: "600"
    };
}


$("#main_grid_genreports").jqGrid('navGrid', '#main_pager_genreports',
    { add: false, edit: false, del: false, refresh: true, search: false },
    updateDialog_groups('PUT'),
    updateDialog_groups('POST'),
    updateDialog_groups('DELETE'),
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


var grid = $("#main_grid_genreports");
grid.jqGrid('navGrid', '#pager_genreports', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});


$("#main_pager_genreports").find("#main_pager_genreports_center").hide();