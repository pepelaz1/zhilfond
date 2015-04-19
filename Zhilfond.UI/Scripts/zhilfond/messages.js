var api_umessages = "/api/umessagesapi/";

function showMessagesDetails() {
    g_currform = 'Сообщения';
 //   $('.div_title_details').text(g_currform);


    $("#messages_tabs").tabs({
        beforeActivate: function (event, ui) {
            updateUnreadMsgCount();
            buildMessageList();         
            $("#grid_umessages").jqGrid('setGridParam', { url: api_umessages + "?token=" + $.cookie('token') }).trigger('reloadGrid');
        }
    });
    $("#grid_umessages").jqGrid('setGridParam', { url: api_umessages + "?token=" + $.cookie('token') }).trigger('reloadGrid');
    buildMessageList();
}



$('#btn_add_message').button().click(function (e) {
    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
    grouprow = $("#grid_groups").getRowData($("#grid_groups").getGridParam("selrow"));

    var m = {};
    m.Token =  $.cookie("token");
    m.Id_house = g_id_house;
    m.Text = $('#ta_new_message').val();
    
    $.ajax({
        url: "/api/messagesapi/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(m),
        dataType: "json",
        async: false,
        success: function (response) {
            buildMessageList();
        },
        error: function (x, e) {
            alert('Ошибка при добавлении сообщения');
        }
    });
});

function buildMessageList() {

    $.get("/api/messagesapi/?id_house=" + g_id_house + "&token=" + $.cookie('token'), function (data) {
              

        if (!!$("#messages_accordion").data("ui-accordion") == true) {
            $('#messages_accordion').html('');
            $("#messages_accordion").accordion("destroy");
        }

        var n = 0;
        data.forEach(function (entry) {
            //console.log(entry);
            n++;

            if (entry.Unread == false) {
                $('#messages_accordion').append(
                    '<div class="group">'
                  + '  <h3>' + entry.Login + ' ' + entry.Created + '</h3>'
                  + '  <div>'
                  + entry.Text
                  + '  </div>'
                  + '</div>'
                 );
            } else {
                $('#messages_accordion').append(
                    '<div class="group">'
                  + '  <h3 mesId="' + entry.Id + '"><b>' + entry.Login + ' ' + entry.Created + '</b></h3>'
                  + '  <div><b>'
                  +  entry.Text
                  + ' </b></div>'
                  + '</div>'
                 );
            }
        });

        $("#messages_accordion").accordion({
            collapsible: true,
            header: "> div > h3",
            active: 0
        });
    });
}



//----- grid_umessages
$("#grid_umessages").jqGrid({
    url: api_umessages + "?token=" + $.cookie('token'),
    datatype: 'json',
    mtype: 'GET',
    pager: '#pager_umessages',
    pagerpos: 'center',
    sortable: true,
    height: 340,
    width: 1010,
    shrinkToFit: false,
    viewrecords: true,
    rowNum: 10000,
    gridview: true,
    colNames: ['ИД', 'ИД дома', 'Логин', 'Дата и время', 'Текст'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 40, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Id_house', index: 'Id_house', editable: false, width: 60, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Login', index: 'Login', editable: true, edittype: 'text', width: 80, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'WhenDateTime', index: 'WhenDateTime', editable: true, edittype: 'text', width: 150, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'Text', index: 'Text', editable: true, edittype: 'text', width: 630, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
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
        /*  var grid = $("#grid_groups")
          var selectedRow = grid.getGridParam("selrow");
          rowData = grid.getRowData(selectedRow);
  
          // console.log(api_group_houses + "?id_group=" + rowData.Id);
          $("#grid_group_houses").jqGrid('setGridParam', { url: api_group_houses + "?id_group=" + rowData.Id }).trigger('reloadGrid');
          refreshDetails(g_currsubform);*/
    },
})

//$("#grid_audit").jqGrid('filterToolbar');

function updateDialog_groups(action) {
    return {
        url: api_umessages
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


$("#grid_umessages").jqGrid('navGrid', '#pager_umessages',
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


$("#grid_umessages").jqGrid('navGrid', '#pager_umessages', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});


$('#btn_go_house').button().click(function (e) {
    var grid = $("#grid_umessages")
    var selectedRow = grid.getGridParam("selrow");
    rowData = grid.getRowData(selectedRow);

    if (rowData.Id_house != undefined) {
        g_id_house = rowData.Id_house;
        $("#gridMain").jqGrid('setGridParam', { "page": 1 }).trigger("reloadGrid");
        g_search_frommap = true;
    }  
});

$('#btn_mark_read').button().click(function (e) {
    var grid = $("#grid_umessages")
    var selectedRow = grid.getGridParam("selrow");
    rowData = grid.getRowData(selectedRow);
    console.log(rowData);

    if (rowData.Id != undefined) {

        var o = {};
        o.Token = $.cookie("token");

        $.ajax({
            url: "/api/umessagesapi/" + rowData.Id,
            type: "DELETE",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(o),
            dataType: "json",
            async: true,
            success: function (response) {
                console.log(response);
                $("#grid_umessages").trigger("reloadGrid");
                updateUnreadMsgCount();
            },
            error: function (x, e) {
                //console.log('Ошибка обновления координат');
            }
        });
    }    
});


$('#btn_mark_read_mes_by_house').button().click(function (e) {
    var selectedRow = $("#messages_accordion .group h3[aria-selected='true']");
    if (selectedRow.length > 0) {
        rowData = selectedRow[0].attributes.getNamedItem("mesid");
        console.log(rowData);

        if (rowData != null && rowData.value != undefined) {

            var o = {};
            o.Token = $.cookie("token");

            $.ajax({
                url: "/api/umessagesapi/" + rowData.value,
                type: "DELETE",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(o),
                dataType: "json",
                async: true,
                success: function (response) {
                    buildMessageList();
                    updateUnreadMsgCount();
                },
                error: function (x, e) {
                    alert('Ошибка прочтения сообщения');
                }
            });
        }
    }
});

