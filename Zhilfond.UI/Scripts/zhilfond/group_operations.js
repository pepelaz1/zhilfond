$("#btn_group").button().click(function (event) {
    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
    grouprow = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));

    var gh = {};
    gh.Id_group = grouprow.Id;
    gh.Id_house = g_id_house;
    if (houserow.letter == "") {
        gh.Address = houserow.street + " " + houserow.number
    } else {
        gh.Address = houserow.street + " " + houserow.number + "/" + houserow.letter;
    }

    $.ajax({
        url: "/api/grouphousesapi/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(gh),
        dataType: "json",
        async: false,
        success: function (response) {
            $("#grid_group_houses").trigger('reloadGrid');
        },
        error: function (x, e) {
            alert('failed');
        }
    });
});

function showGroupOperationsDetails() {
    //console.log(g_id_house);
    g_currform = 'Групповые операции';
    $('.div_title_details').text(g_currform);
}

    //----- grid_groups
    var api_groups = "/api/groupsapi/";
  

    $("#grid_groups").jqGrid({
        url: api_groups + "?token=" + $.cookie('token'),
        datatype: 'json',
        mtype: 'GET',
        pager: '#pager_groups',
        pagerpos: 'center',
        sortable: true,
        height: 80,
        width: 220,
        shrinkToFit: false,
        viewrecords: true,
        rowNum: 10000,
        colNames: ['ИД', 'Название'],
        colModel: [{ name: 'Id', index: 'Id', editable: false, width: 30, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
                   { name: 'Title', index: 'Title', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
        ],
        caption: 'Группы',
        loadError: function (xhr, st, err) {
            if (xhr.status == 401) {
                alert(xhr);
            }
        },
        loadComplete: function () {
        },
        onSelectRow: function (ids) {
            var grid = $("#grid_groups")
            var selectedRow = grid.getGridParam("selrow");
            rowData = grid.getRowData(selectedRow);
          
           // console.log(api_group_houses + "?id_group=" + rowData.Id);
            $("#grid_group_houses").jqGrid('setGridParam', { url: api_group_houses + "?id_group=" + rowData.Id }).trigger('reloadGrid');
            //refreshDetails(g_currsubform);
        },
    })

    //console.log(api_groups + "?token=" + $.cookie('token'));
    $("#grid_groups").jqGrid('setGridParam', { url: api_groups + "?token=" + $.cookie('token') }).trigger('reloadGrid');

    //$("#grid_values").jqGrid('filterToolbar');

    function updateDialog_groups(action) {
        return {
            url: api_groups
                , closeAfterAdd: true
                , closeAfterEdit: true
                , afterShowForm: function (formId) { }
                , modal: true
                , onclickSubmit: function (params) {

                    var grid = $("#grid_groups")
                    var selectedRow = grid.getGridParam("selrow");
                    rowData = grid.getRowData(selectedRow);
                    params.url += rowData.Id;
                    params.url += "?token=" + $.cookie('token')
                    //console.log(params.url);
                    params.mtype = action;
                }
                , width: "500"
        };
    }


    $("#grid_groups").jqGrid('navGrid', '#pager_groups',
        { add: true, edit: true, del: true, refresh: true, search: false },
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


    var grid = $("#grid_groups");
    grid.jqGrid('navGrid', '#pager_groups', {}, {}, {}, {},
    {
        onReset: function () {
            var jqModal = true, gridid = grid[0].id;
            $.jgrid.hideModal("#searchmodfbox_" + gridid,
                { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
        }
    });


    $("#pager_groups").find("#pager_groups_center").hide();

    //----- grid_group_houses
    var api_group_houses = "/api/grouphousesapi/";

    $("#grid_group_houses").jqGrid({
        url: api_group_houses,
        datatype: 'json',
        mtype: 'GET',
        pager: '#pager_group_houses',
        pagerpos: 'center',
        sortable: true,
        height: 80,
        width: 220,
        shrinkToFit: false,
        viewrecords: true,
        rowNum: 10000, 
        colNames: ['ИД', 'ИДДома','Дом', "Широта", "Долгота", "Подсказка"],
        colModel: [{ name: 'Id', index: 'Id', editable: false, width: 30, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
                   { name: 'Id_house', index: 'Id_house', editable: false, width: 30, editrules: { required: true }, searchoptions: { sopt: ['cn'] }, hidden: true  },
                   { name: 'Address', index: 'Address', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
                   { name: 'Latitude', index: 'Latitude', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] }, hidden: true },
                   { name: 'Longitude', index: 'Longitude', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] }, hidden: true },
                   { name: 'Baloon', index: 'Baloon', editable: true, edittype: 'text', width: 160, editrules: { required: true }, searchoptions: { sopt: ['cn'] }, hidden: true }
        ],
        caption: 'Состав группы',
        loadError: function (xhr, st, err) {
            if (xhr.status == 401) {
                alert(xhr);
            }
        },
        loadComplete: function (data) {
            refreshMap();
        },
        gridComplete: function (data) {        
        },
        onSelectRow: function (ids) {
        },
    });


    function updateDialog_group_houses(action) {
        return {
            url: api_group_houses
                , closeAfterAdd: true
                , closeAfterEdit: true
                , afterShowForm: function (formId) { }
                , modal: true
                , onclickSubmit: function (params) {

                    //var grid = $("#grid_group_houses")
                    //var selectedRow = grid.getGridParam("selrow");
                    var ghrow = $("#grid_group_houses").getRowData( $("#grid_group_houses").getGridParam("selrow"));
                    params.url += ghrow.Id;
                    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
      

                    /*var grid = $("#grid_groups")
                    var selectedRow = grid.getGridParam("selrow");
                    rowData = grid.getRowData(selectedRow);
                    params.url += rowData.Id;*/
                    if ( houserow.letter == "") {
                        params.url += "?address=" +houserow.street + " " + houserow.number
                    } else {
                        params.url += "?address=" +houserow.street + " " + houserow.number + "/" + houserow.letter;
                    }
                    //console.log(params.url);
                    params.mtype = action;
                }
                , width: "500"
        };
    }


    $("#grid_group_houses").jqGrid('navGrid', '#pager_group_houses',
        { add: false, edit: false, del: true, refresh: true, search: false },
        updateDialog_group_houses('PUT'),
        updateDialog_group_houses('POST'),
        updateDialog_group_houses('DELETE'),
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


    var grid = $("#grid_group_houses");
    grid.jqGrid('navGrid', '#pager_group_houses', {}, {}, {}, {},
    {
        onReset: function () {
            var jqModal = true, gridid = grid[0].id;
            $.jgrid.hideModal("#searchmodfbox_" + gridid,
                { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
        }
    });
    $("#pager_group_houses").find("#pager_group_houses_center").hide();

    //----- grid_groupreptemplates
    var api_groupreptemplatesapi = "/api/groupreptemplatesapi/";


    $("#grid_groupreptemplates").jqGrid({
        url: api_groupreptemplatesapi,
        datatype: 'json',
        mtype: 'GET',
        pager: '#pager_groupreptemplates',
        pagerpos: 'center',
        sortable: true,
        height: 290,
        width: 220,
        shrinkToFit: false,
        viewrecords: true,
        rowNum: 10000,
        colNames: ['ИД', 'Название'],
        colModel: [{ name: 'Id', index: 'Id', editable: false, width: 30, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
                   { name: 'template_name', index: 'template_name', editable: true, edittype: 'text', width: 180, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
        ],
        caption: 'Шаблоны отчетов',
        loadError: function (xhr, st, err) {
            if (xhr.status == 401) {
                alert(xhr);
            }
        },
        loadComplete: function () {
        },
        onSelectRow: function (ids) {
            var grid = $("#grid_groupreptemplates")
            var selectedRow = grid.getGridParam("selrow");
            rowData = grid.getRowData(selectedRow);

            // console.log(api_group_houses + "?id_group=" + rowData.Id);
            //$("#grid_group_houses").jqGrid('setGridParam', { url: api_group_houses + "?id_group=" + rowData.Id }).trigger('reloadGrid');
            refreshDetails(g_currsubform);
        },
    })


    //$("#grid_values").jqGrid('filterToolbar');

    function updateDialog_groupreptemplates(action) {
        return {
            url: api_groupreptemplatesapi
                , closeAfterAdd: true
                , closeAfterEdit: true
                , afterShowForm: function (formId) { }
                , modal: true
                , onclickSubmit: function (params) {

                    var grid = $("#grid_groupreptemplates")
                    var selectedRow = grid.getGridParam("selrow");
                    rowData = grid.getRowData(selectedRow);
                    params.url += rowData.Id;
                    //console.log(params.url);
                    params.mtype = action;
                }
                , width: "500"
        };
    }


    $("#grid_groupreptemplates").jqGrid('navGrid', '#pager_groupreptemplates',
        { add: true, edit: true, del: true, refresh: true, search: false },
        updateDialog_groupreptemplates('PUT'),
        updateDialog_groupreptemplates('POST'),
        updateDialog_groupreptemplates('DELETE'),
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


    var grid = $("#grid_groupreptemplates");
    grid.jqGrid('navGrid', '#pager_groupreptemplates', {}, {}, {}, {},
    {
        onReset: function () {
            var jqModal = true, gridid = grid[0].id;
            $.jgrid.hideModal("#searchmodfbox_" + gridid,
                { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
        }
    });


    $("#pager_groupreptemplates").find("#pager_groupreptemplates_center").hide();


   