function showImportDetails() {
    g_currform = 'Импорт';
    $('.div_title_details').text(g_currform);
    $("#grid_import").trigger('reloadGrid');
    $("#grid_import_details").trigger('reloadGrid');
}


$("#btn_upload").button().click(function (event) {
    var files = $("#file1").get(0).files;
    if (files && files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (i = 0; i < files.length; i++) {
             //   console.log(files[i]);
                data.append("file" + i, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/api/fileuploadapi?type=import&token=" + $.cookie('token'),
                contentType: false,
                processData: false,
                data: data,
                success: function (results) {
                    $.each(results, function (key, data) {
                        save(data.File, "import", data.Id_import);
                        $("#grid_import").trigger('reloadGrid');
                        $("#grid_import_details").trigger('reloadGrid');
                    });
                }, error: function (x, e) {
                    alert('Ошибка импорта');
                    $("#grid_import").trigger('reloadGrid');
                    $("#grid_import_details").trigger('reloadGrid');
                }
            });
        } else {
            alert("This browser doesn't support HTML5 multiple file uploads!");
        }
    }
    return false;
});

//----- grid_import
var api_import = "/api/impfilesapi/";


$("#grid_import").jqGrid({
    url: api_import + "?token=" + $.cookie('token'),
    datatype: 'json',
    mtype: 'GET',
    pager: '#pager_import',
    pagerpos: 'center',
    sortable: true,
    height: 330,
    width: 360,
    shrinkToFit: false,
    viewrecords: true,
    gridview: true,
    rowNum: 10000,
    colNames: ['ИД', 'Имя файла', 'Импортирован','Кем'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 25, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Filename', index: 'Filename', editable: true, edittype: 'text', width: 110, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Created', index: 'Created', editable: true, edittype: 'text', width: 125, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Username', index: 'Username', editable: true, edittype: 'text', width: 60, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
    ],
    caption: 'Импортированные файлы',
    loadError: function (xhr, st, err) {
        if (xhr.status == 401) {
            alert(xhr);
        }
    },
    loadComplete: function () {
    },
    onSelectRow: function (ids) {
        var rowData = $("#grid_import").getRowData(ids);
        $("#grid_import_details").jqGrid('setGridParam', { url: api_importdetails + "?id_import=" + rowData.Id+ "&token="+$.cookie('token') }).trigger('reloadGrid');
      //  $("#ta_rep_signature").val(rowData.Signature);
    },
})

function updateDialog_import(action) {
    return {
        url: api_import
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

$("#grid_import").jqGrid('navGrid', '#pager_import',
    { add: false, edit: false, del: false, refresh: true, search: false },
    updateDialog_import('PUT'),
    updateDialog_import('POST'),
    updateDialog_import('DELETE'),
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

var grid = $("#grid_import");
grid.jqGrid('navGrid', '#pager_import', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});

$("#pager_import").find("#pager_import_center").hide();



//----- grid_importdetails_api
var api_importdetails = "/api/impdetailsapi/";


$("#grid_import_details").jqGrid({
    url: api_importdetails + "?token=" + $.cookie('token'),
    datatype: 'json',
    mtype: 'GET',
    pager: '#pager_import_details',
    pagerpos: 'center',
    sortable: true,
    height: 330,
    width: 630,
    shrinkToFit: false,
    viewrecords: true,
    gridview: true,
    rowNum: 10000,
    colNames: ['ИД', 'Статус', 'Дом', 'Элемент', 'Ошибка'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 25, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Status', index: 'Status', editable: true, edittype: 'text', width: 60, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'House', index: 'House', editable: true, edittype: 'text', width: 275, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Element', index: 'Element', editable: true, edittype: 'text', width: 300, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Error', index: 'Error', editable: true, edittype: 'text', width: 220, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }

    ],
    caption: 'Детали импорта',
    loadError: function (xhr, st, err) {
        if (xhr.status == 401) {
            alert(xhr);
        }
    },
    loadComplete: function () {
    },
    onSelectRow: function (ids) {

    },
})

function updateDialog_importdetails(action) {
    return {
        url: api_importdetails
            , closeAfterAdd: true
            , closeAfterEdit: true
            , afterShowForm: function (formId) { }
            , modal: true
            , onclickSubmit: function (params) {

            }
            , width: "500"
    };
}

$("#grid_import_details").jqGrid('navGrid', '#pager_import_details',
    { add: false, edit: false, del: false, refresh: true, search: false },
    updateDialog_importdetails('PUT'),
    updateDialog_importdetails('POST'),
    updateDialog_importdetails('DELETE'),
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

var grid = $("#grid_import_details");
grid.jqGrid('navGrid', '#pager_import_details', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});

$("#pager_import_details").find("#pager_import_details_center").hide();