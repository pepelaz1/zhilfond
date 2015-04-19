var api_audit = "/api/auditapi/";

function showAuditDetails() {
    g_currform = 'Аудит';
    $('.div_title_details').text(g_currform);

    $("#grid_audit").jqGrid('setGridParam', { url: api_audit + "?token=" + $.cookie('token') }).trigger('reloadGrid');
}

//----- grid_audit

$("#grid_audit").jqGrid({
    url: api_audit + "?token=" + $.cookie('token'),
    datatype: 'json',
    mtype: 'GET',
    pager: '#pager_audit',
    pagerpos: 'center',
    sortable: true,
    height: 365,
    width: 1035,
    shrinkToFit: false,
    viewrecords: true,
    rowNum: 10000,
    gridview: true,
    colNames: ['ИД', 'ИД дома', 'Логин', 'Форма', 'Поле', 'Старое значение', 'Новое значение', 'Дата и время изменения'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 40, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Id_house', index: 'Id_house', editable: false, width: 60, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Login', index: 'Login', editable: true, edittype: 'text', width: 80, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Form', index: 'Form', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Field', index: 'Field', editable: true, edittype: 'text', width: 200, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'OldVal', index: 'OldVal', editable: true, edittype: 'text', width: 140, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'NewVal', index: 'NewVal', editable: true, edittype: 'text', width: 140, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'WhenDateTime', index: 'WhenDateTime', editable: true, edittype: 'text', width: 150, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
    ],
    caption: 'Изменения, сделанные пользователями',
    loadError: function (xhr, st, err) {
        if (xhr.status == 401) {
            alert(xhr);
        }
    },
    loadComplete: function () {
    },
    onSelectRow: function (ids) {
      /*  var grid = $("#grid_groups")
        var selectedRow = grid.getGridParam("selrow");
        rowData = grid.getRowData(selectedRow);

        // console.log(api_group_houses + "?id_group=" + rowData.Id);
        $("#grid_group_houses").jqGrid('setGridParam', { url: api_group_houses + "?id_group=" + rowData.Id }).trigger('reloadGrid');
        refreshDetails(g_currsubform);*/
    },
})

$("#grid_audit").jqGrid('filterToolbar');

function updateDialog_groups(action) {
    return {
        url: api_audit
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
            , width: "500"
    };
}


$("#grid_audit").jqGrid('navGrid', '#pager_audit',
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


var grid = $("#grid_audit");
grid.jqGrid('navGrid', '#pager_audit', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});

$("#grid_audit").jqGrid('setGridParam', { url: api_audit + "?token=" + $.cookie('token') }).trigger('reloadGrid');

