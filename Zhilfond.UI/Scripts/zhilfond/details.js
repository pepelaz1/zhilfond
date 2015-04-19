var apiUrl = '/api/housesapi/?token='+$.cookie('token');
var api_values = '/api/valuesapi/';
var g_id_parent = 0;
var g_parent = "";
var g_aidx = 1;
var g_search_frommap = false;


$("#gridMain").jqGrid({
    url: apiUrl,
    datatype: 'json',
    mtype: 'GET',
    pager: '#pagernav',
    pagerpos: 'center',
    sortable: true,
    height: 146,
    width: 1245,
    shrinkToFit: false,
    viewrecords: true,
    gridview: true,
    scrollrows : true,
    rowList: [100, 1000, 5000],
    rowNum: 100,
    colNames: ['ИД', 'Населенный пункт', 'Район', 'Улица', 'Номер', 'Литера', 'Обслуж. орг.', 'Тип дома', 'Сервис', 'Дата ввода в эксплуатацию', 'Наиб. кол-во этажей'
        , 'Кол-во мун. Квартир', 'Общая площадь квартир', 'Зарегистрировано постоянно', 'Форма управления'],
    colModel: [{ name: 'Id', index: 'Id', editable: false, width: 50, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'nasp', index: 'nasp', editable: true, edittype: 'text', width: 120, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'raion', index: 'raion', editable: true, edittype: 'text', width: 80, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'street', index: 'street', editable: true, edittype: 'text', width: 100, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'number', index: 'number', editable: true, edittype: 'text', width: 50, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'letter', index: 'letter', editable: true, edittype: 'text', width: 50, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'oorg', index: 'oorg', editable: true, edittype: 'text', width: 100, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'tdoma', index: 'tdoma', editable: true, edittype: 'text', width: 80, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'seriya', index: 'seriya', editable: true, edittype: 'text', width: 80, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'date_exp', index: 'date_exp', editable: false, edittype: 'text', align: 'center', width: 180, editrules: { required: false }, searchoptions: { sopt: ['cn'] } },
               { name: 'etaz_max', index: 'etaz_max', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'kolmunkv', index: 'kolmunkv', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'ploo', index: 'ploo', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'kolzp', index: 'kolzp', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
               { name: 'fupr', index: 'fupr', editable: true, edittype: 'text', width: 130, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }

    ],
    caption: "Список домов",
    //autowidth: true,
    loadError: function (xhr, st, err) {
        alert(err);
        if (xhr.status == 401) {
            //window.location.replace("/Home/Index");
            alert(xhr);
        }
    },
    loadComplete: function (data) {
        found = false;
        var rowIds = $(this).jqGrid('getDataIDs');
        for (i = 1; i <= rowIds.length; i++) {
            rowData = $(this).jqGrid('getRowData', i);
            //console.log(rowData.Id);
            if (rowData.Id == g_id_house) {
                found = true;
                g_search_frommap = false;
                $(this).jqGrid('setSelection', i);
                $("#gridMain").jqGrid('setCaption', "Список домов: ИД=" + rowData.Id);
            } //if
        }

        var totalPages = $("#sp_1_pagernav").text();
        var currentPage = $(this).getGridParam('page');

        if (currentPage >= totalPages) {
            g_search_frommap = false;
        }
      
        //console.log(g_search_frommap + " " + found + " " + totalPages + " " + currentPage);
        if (g_search_frommap && !found && currentPage < totalPages) {
           // console.log(found + " " + totalPages + " " + currentPage);
            setTimeout(function () { $("#gridMain").trigger('reloadGrid', [{ page: currentPage + 1 }]); }, 10);
        }
    },
    onSelectRow: function (ids) {
        var rowData = $("#gridMain").getRowData(ids);
        $("#gridMain").jqGrid('setCaption', "Список домов: ИД=" + rowData.Id);
     /*   if (g_currform == 'Основные данные') {
            displayDetailsDiv('details_main');
        } else {
            displayDetailsDiv('details');
        }
        g_id_house = rowData.Id;
        showDetails(g_currform);*/
        g_id_house = rowData.Id;
        refreshDetails(g_currform);       
        //  $("#grid_groups").jqGrid('setGridParam', { url: api_groups + "?id_house =" + rowData.Id }).trigger('reloadGrid');

        showAttachDetails();
    }
});

$("#gridMain").jqGrid('filterToolbar');

function updateDialog(action) {
    return {
        url: apiUrl
            , closeAfterAdd: true
            , closeAfterEdit: true
            , afterShowForm: function (formId) { }
            , modal: true
            , onclickSubmit: function (params) {
                var list = $("#gridMain");
                var selectedRow = list.getGridParam("selrow");
                rowData = list.getRowData(selectedRow);
                params.url += rowData.Id;
                params.mtype = action;
            }
            , width: "500"
    };
}


$("#gridMain").jqGrid('navGrid', '#pagernav',
    { add: false, edit: false, del: false, refresh: true, search: false },
    updateDialog('PUT'),
    updateDialog('POST'),
    updateDialog('DELETE'),
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


var grid = $("#gridMain");
grid.jqGrid('navGrid', '#pagernav', {}, {}, {}, {},
{
    onReset: function () {
        var jqModal = true, gridid = grid[0].id;
        $.jgrid.hideModal("#searchmodfbox_" + gridid,
            { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
    }
});


function createDetailsControls() {

    
    $("#details_accordion").accordion({
        collapsible: true,
        header: "> div > h3",
        heightStyle: "fill"
    }).sortable({
        axis: "y",
        handle: "h3",
        stop: function (event, ui) {
            // IE doesn't register the blur when sorting
            // so trigger focusout handlers to remove .ui-state-focus
            ui.item.children("h3").triggerHandler("focusout");
        }
    });

    $('#details_accordion').keypress(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            save("", "manual", "");
            //submitKeyDlg(data, sourceType, id_source);
        }
    });


    $("#btn_new").button().click(function (event) {
        g_isnew = true;
        $("#btn_save").attr('style', 'display:block;color:black');
        $("#btn_cancel").attr('style', 'display:block;color:black');
        // enable and clear all controls
        $("*[class^='fld_']").each(function (i, val) {
            //console.log($(this).attr('tag'));
            if (canEdit($.cookie('token'), g_currform, $(this).attr('category'))) {
                $(this).attr('disabled', false);
                $(this).siblings().removeClass('ui-state-disabled');
                $(this).val('');
            }
        });
        //$("*[class^='fld_']").attr('disabled', false);
        //$("*[class^='fld_integer']").siblings().removeClass('ui-state-disabled');
        //$("*[class^='fld_']").val('');

    });

    $("#btn_edit").button().click(function (event) {
        $("#btn_save").attr('style', 'display:block;color:black');
        $("#btn_cancel").attr('style', 'display:block;color:black');
        // enable all controls
        $("*[class^='fld_']").each(function (i, val) {
            //console.log($(this).attr('tag'));
            if (canEdit($.cookie('token'), g_currform, $(this).attr('category'))) {
                $(this).attr('disabled', false);
                $(this).siblings().removeClass('ui-state-disabled');
            }
        });
        //$("*[class^='fld_']").attr('disabled', false);
      //  $("*[class^='fld_integer']").siblings().removeClass('ui-state-disabled');

    });
    $("#btn_save").button().click(function (event) {
        save("","manual","");
    });

    $("#btn_cancel").button().click(function (event) {
        showDetails(g_currform);
    });

    $(".fld_datepicker").datepicker($.datepicker.regional["ru"]);
    $(".fld_integer").spinner({ icons: { down: "ui-icon-triangle-1-s", up: "ui-icon-triangle-1-n" } });

    $("*[class^='fld_']").attr('disabled', true);
    $("*[class^='fld_integer']").siblings().addClass('ui-state-disabled');


    ///console.log('g_currform = ' + g_currform + ' g_id_house = ' + g_id_house);

    // recreate values grid
    $("#grid_values").jqGrid({
        url: api_values + "?form=" + encodeURIComponent(g_currform) + "&id_house=" + g_id_house,
        datatype: 'json',
        mtype: 'GET',
        pager: '#pager_values',
        pagerpos: 'center',
        sortable: true,
        height: 410,
        width: 257,
        shrinkToFit: false,
        viewrecords: true,
        gridview: true,
        rowList: [100, 1000, 5000],
        rowNum: 10000,
        colNames: ['ИД', 'Выводимое имя'],
        colModel: [{ name: 'Id', index: 'Id', editable: false, width: 50, editrules: { required: true }, searchoptions: { sopt: ['cn'] } },
                   { name: 'Name', index: 'Name', editable: true, edittype: 'text', width: 180, editrules: { required: true }, searchoptions: { sopt: ['cn'] } }
        ],
        caption: g_currform,
        //autowidth: true,
        loadError: function (xhr, st, err) {
            if (xhr.status == 401) {
                //window.location.replace("/Home/Index");
                alert(xhr);
            }
        },
        loadComplete: function () {       
        },
        onSelectRow: function (ids) {
            var rowData = $("#grid_values").getRowData(ids);
            g_id_parent = rowData.Id;
            g_parent = rowData.Name;
            if (g_currform == 'Основные данные') {
                displayDetailsDiv('details_main');
            } else {
                displayDetailsDiv('details');
            }
            showDetails(g_currform);
        },
    });

    $(function () {
        $("#accordion-resizer").resizable({
            minHeight: 140,
            minWidth: 200,
            resize: function () {
                $("#details_accordion").accordion("refresh");
            }
        });
    });

    //$("#grid_values").jqGrid('filterToolbar');

    function updateDialog_values(action) {
        return {
            url: api_values
                , closeAfterAdd: true
                , closeAfterEdit: true
                , afterShowForm: function (formId) { }
                , modal: true
                , onclickSubmit: function (params) {
                    /*    var list = $("#gridMain");
                        var selectedRow = list.getGridParam("selrow");
                        rowData = list.getRowData(selectedRow);
                        params.url += rowData.Id;
                        params.mtype = action;*/
                }
                , width: "500"
        };
    }


    $("#grid_values").jqGrid('navGrid', '#pager_values',
        { add: false, edit: false, del: false, refresh: true, search: false },
        updateDialog_values('PUT'),
        updateDialog_values('POST'),
        updateDialog_values('DELETE'),
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


    var grid = $("#grid_values");
    grid.jqGrid('navGrid', '#pager_values', {}, {}, {}, {},
    {
        onReset: function () {
            var jqModal = true, gridid = grid[0].id;
            $.jgrid.hideModal("#searchmodfbox_" + gridid,
                { gb: "#gbox_" + gridid, jqm: jqModal, onClose: null });
        }
    });


    $("#pager_values").find("#pager_values_center").hide();
}


function showDetails(formname) {
 
 
    if (g_currform != formname)
        g_aidx = 0;
    else if (!!$("#details_accordion").data("ui-accordion") == false)
        g_aidx = 0;
    else
        g_aidx = $('#details_accordion').accordion("option", "active");
    


    g_currform = formname;

    if (g_currform == 'Основные данные') {
        $('#details').html('');
        $('#details_main').html(
            '<div style="float:left;width:770px">'
          +  '<div class="div_title_details">'
          +   '<div style="float:left">' + g_currform + '</div>'
          +   '<div style="float:right"><button id="btn_cancel" style="display:none;color:black">Отмена</button></div>'
          +   '<div style="float:right"><button id="btn_save" style="display:none;color:black">Сохранить</button></div>'
          +   '<div style="float:right"><button id="btn_edit" style="color:black">Редактировать</button></div>'
          +   '<div style="float:right"><button id="btn_new" style="color:black">Новый</button></div>'
          +   '</div>'
          +   '<div id="accordion-resizer" class="ui-widget-content" style="width:100%; height: 500px;"><div id="details_accordion">'
          +   '</div></div>'
          + '</div>'
          + '<div style="float:left;width:200px;margin-left:10px">'
          + '<div class="div_title_details">Координаты</div>'
          +  '<div id="dlg_coords" style="margin-top:3px;font-size:0.8em">'
          +   '<table style="padding:0px; margin:0px;">'
          +   '<tr>'
          +    '<td>'
          +     'Широта'
          +    '</td>'
          +    '<td>'
          +     '<input id="input_lat" type="text" style="width:140px;height:10px;font-size:1.1em;fontname:verdana" readonly="true" ></input>'
          +    '</td>'
          +   '</tr>'
          +   '<tr>'
          +    '<td>'
          +     'Долгота'
          +    '</td>'
          +    '<td>'
          +     '<input id="input_long" type="text" style="width:140px;height:10px;font-size:1.1em;fontname:verdana" readonly="true"></input>'
          +    '</td>'
          +   '</tr>'
          +   '<tr>'
          +    '<td colspan="2">'
          +     '<button id="btn_apply_coord" style="color:black;font-size:1.0em;display:none">Применить</button>'
          +    '</td>'
          +   '</tr>'
          + '</table>'
          + '</div>'
          + '</div>'
        );
    } else {
        $('#details_main').html('');
        $('#details').html(
            '<table style="margin-top:0px">'
              + '<tr>'
              +  '<td style="width:770px; vertical-align:top; padding-right:7px">'
              +   '<div class="div_title_details">'
              +   '<div style="float:left">' + g_currform + '</div>'
              + '<div style="float:right"><button id="btn_cancel" style="display:none;color:black">Отмена</button></div>'
              + '<div style="float:right"><button id="btn_save" style="display:none;color:black">Сохранить</button></div>'
              + '<div style="float:right"><button id="btn_edit" style="color:black">Редактировать</button></div>'
              + '<div style="float:right"><button id="btn_new" style="color:black">Новый</button></div>'
              +   '</div>'
              + '<div id="accordion-resizer" class="ui-widget-content" style="width:100%; height: 500px;"><div id="details_accordion">'
              + '</div></div>'
              +  '</td>'
             + '<td style="width:180px;padding-right:0px">'
                  + '<table id="grid_values" style="padding-right:0px"></table>'
                  + '<div id="pager_values"></div>'
              +   '</td>'
              + '</tr>'
         + '</table>'
        );
    }

    var grid = $("#gridMain");
    var idx = grid.getGridParam("selrow");
    var rowData = grid.getRowData(idx);
        
  //  console.log(g_id_parent);
    $.get("/api/valuesapi/?token="+$.cookie('token')+"&form=" + encodeURIComponent(g_currform) + "&id_house=" + rowData.Id + "&id_parent=" + g_id_parent, function (data) {
        // $('#details_accordion').html('');
        // $('#details_accordion').removeAttr('class');
        var curcat = "";
        var n = 0;
        //var kk = 0;
        g_hash.clear();
        data.forEach(function (entry) {
            //console.log(entry);
            // add to hash table
            var o = new Object();
            o.value = entry.Value;
            o.type = entry.Dictname;
            g_hash.put(entry.Title, o);

            if (entry.Category != curcat) {
                curcat = entry.Category;
                n = n + 1;
                $('#details_accordion').append(
                       '<div class="group">'
                     + '  <h3>' + entry.Category + '</h3>'
                     + '  <div>'
                     + '     <table style="margin-top:0px" id="field-table-' + n + '">'
                     + '     </table>'
                     + '  </div>'
                     + '</div>'
                    );
            }

            var v = '';

            if (entry.Dictname == 'Строковое') {
                v = '<td style="vertical-align:middle"><input category="' + curcat + '" tag="' + entry.Title + '" tag1="' + entry.Dictname + '" style="padding-top:0.5px;padding-bottom:0.75px" type="text" class="fld_string" value="' + entry.Value + '"></input></td>';
            }
            else if (entry.Dictname == 'Числовое' || entry.Dictname == 'С плавающей точкой') {
                v = '<td style="vertical-align:middle"><input category="' + curcat + '" tag="' + entry.Title + '" tag1="' + entry.Dictname + '" style="padding-top:0.5px;padding-bottom:0.75px" type="text" class="fld_integer" value="' + entry.Value + '"></input></td>';
            }
            else if (entry.Dictname == 'Дата и время') {
                v = '<td style="vertical-align:middle"><input category="' + curcat + '" tag="' + entry.Title + '" tag1="' + entry.Dictname + '" style="padding-top:0.5px;padding-bottom:0.75px" type="text" class="fld_datepicker" value="' + entry.Value + '"></input></td>';
            }
            else {
                // справочники
                v = '<td style="vertical-align:middle"><select category="' + curcat + '" tag="' + entry.Title + '" tag1="' + entry.Dictname + '" class="fld_dict">';
                v = v + '<option></option>';
                $.ajax(
                    {
                        type: 'GET',
                        async: false,
                        url: '/api/dictsvaluesapi/?id_dict=' + entry.Id_dict,
                        success: function (data) {
                            data.rows.forEach(function (value) {
                               // console.log(value);
                                if (value.cell[0] == entry.Value)
                                    v += '<option selected value="' + value.cell[0] + '">' + value.cell[1] + '</option>';
                                else
                                    v += '<option value="' + value.cell[0] + '">' + value.cell[1] + '</option>';
                            });
                        }
                    });
                v = v + '<td></select>';
            }


            if (entry.Warning == "" || entry.Warning == undefined || entry.Warning == null) {
                $('#field-table-' + n + '').append(
                    '<tr style="height:30px; vertical-align:top">'
                    + '<td  style="vertical-align:middle">' + entry.Title + '</td>'
                    + v
                    + '</tr>'
                );
            } else {
                $('#field-table-' + n + '').append(
                 '<tr style="height:40px; vertical-align:top">'
                 + '<td style="vertical-align:middle">' + entry.Title + '</td>'
                 + v
                 + '</tr>'
                 + '<tr style="font-size:0.9em; color:#ff0000">'
                 + '<td colspan="3"><div style="margin-left:10px; margin-top:-12px;">'+entry.Warning+'</div></td>'
                 + '</tr>'
               );
            }
        });

        createDetailsControls();
        $("#details_accordion").accordion({ active: g_aidx,  heightStyle: "content" });
        $("#btn_apply_coord").button().click(function (e) {
            houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
            if (houserow != undefined) {

                var o = {};
                o.Token = $.cookie("token");
                o.Id_house = houserow.Id;;
                o.Latitude = $("#input_lat").val();
                o.Longitude = $("#input_long").val();

                $.ajax({
                    url: "/api/coordsapi/",
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    data: JSON.stringify(o),
                    dataType: "json",
                    async: true,
                    success: function (response) {
                    },
                    error: function (x, e) {
                    }
                });
            }
        });

        if (canEditHouse($.cookie('token'), g_id_house)) {
            
            $("#input_lat").prop("readonly", false);
            $("#input_long").prop("readonly", false);
            $("#btn_apply_coord").css("display", "block");
        }

        loadHouseCoords();
    });
}

function showAttachDetails() {
    var api_attachments = "/api/attachmentsapi/";
    // g_currform = 'Прикрепленные файлы';
    $('.div_title_attach').text('Прикрепленные файлы');

    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
    $("#grid_attachments").jqGrid('setGridParam', { url: api_attachments + '?id_house=' + houserow.Id }).trigger('reloadGrid');

    $('#btn_attach').attr('disabled', !canEditHouse($.cookie('token'), g_id_house));

    var $td = $('#del_grid_attachments');
    if (canEditHouse($.cookie('token'), g_id_house))
        $td.show();
    else
        $td.hide();
}


function loadHouseCoords() {
    houserow = $("#gridMain").getRowData($("#gridMain").getGridParam("selrow"));
    if (houserow != undefined) {
        $.get("/api/coordsapi/?id_house=" + houserow.Id, function (data) {
            if (data.Latitude == null || data.Longitude == null || data.Latitude == '' || data.Longitude == '') {
                ymaps.geocode(houserow.nasp + ' ' + houserow.street +' ' + houserow.number + ' ' + houserow.letter, ymaps.util.extend({}, self._options)).then(
                  function (response) {
                      var obj = response.geoObjects.get(0);

                      // Обновляем координаты дома
                      var o = {};
                      o.Token = $.cookie("token");
                      o.Id_house = houserow.Id;
                      o.Latitude = obj.geometry.getCoordinates()[0];
                      o.Longitude = obj.geometry.getCoordinates()[1];
                      //console.log(o);

                      $.ajax({
                          url: "/api/coordsapi/",
                          type: "POST",
                          contentType: "application/json;charset=utf-8",
                          data: JSON.stringify(o),
                          dataType: "json",
                          async: false,
                          success: function (response) {
                              $("#input_lat").val(o.Latitude);
                              $("#input_long").val(o.Longitude);
                          },
                          error: function (x, e) { 
                          }
                      });       
                  },
                  function (err) {
                      promise.reject(err);
                  }
              );
            }
            else {
                $("#input_lat").val(data.Latitude);
                $("#input_long").val(data.Longitude);
            }
        });
    }
}