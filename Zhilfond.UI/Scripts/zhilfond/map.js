
function ShowMapSubdetails() {
    $('.div_title_subdetails').text(g_currsubform);
}


var g_map;
var bMapEdited = false;

function moveToHouse(Id_house) {

    g_id_house = Id_house;
    $("#gridMain").jqGrid('setGridParam', { "page": 1 }).trigger("reloadGrid");
    g_search_frommap = true;
}

function placeHouses(houses) {
    houses.forEach(function (h, index) {
       // console.log(h);
        var c = [];
        c[0] = h.Latitude.replace(',','.');
        c[1] = h.Longitude.replace(',', '.');

        var pmark = new ymaps.Placemark(c,
                                        {
                                            balloonContentBody: h.Baloon,
                                            balloonContentFooter: '<button type="button" id="btn_ballon_' + index + '" onclick="moveToHouse(' + h.Id_house + ')">Перейти</button>',
                                            Id_house: h.Id_house
                                        },
                                        {
                                            preset: 'twirl#blueIcon'
                                        });

      //  console.log(pmark);
        // вешаем события
        pmark.events.add('dragend', function (e) {
            //alert(pmark.geometry.getCoordinates());
            var thisPlacemark = e.get('target');
            // Определение координат метки 
            var coords = thisPlacemark.geometry.getCoordinates();
          //  console.log(coords);
            /////////////////////////////////
            // Обновляем координаты дома            
            var thisObj = {};
            thisObj.Token = $.cookie("token");
            thisObj.Id_house = thisPlacemark.properties.get('Id_house');
            thisObj.Latitude = thisPlacemark.geometry.getCoordinates()[0];
            thisObj.Longitude = thisPlacemark.geometry.getCoordinates()[1];

            $.ajax({
                url: "/api/coordsapi/",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(thisObj),
                dataType: "json",
                async: false,
                success: function (response) {
                    //alert('success');
                    /*
                    var o1 = {};
                    o1.Id_house = o.Id_house;
                    o1.Latitude = o.Latitude;
                    o1.Longitude = o.Longitude;
                    o1.Baloon = address.Baloon;
                    houses.push(o1);
                    */
                },
                error: function (x, e) {
                   // alert('error');
                }
            });            
            ////////////////////////////////
        });
        
        g_map.geoObjects.add(pmark);
    });
}

function showMap() {
   // console.log('0');
    if (ymaps != undefined) {

        if (g_map != undefined)
            g_map.destroy();


        g_map = new ymaps.Map('ymap', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: [56.486768, 84.946407], // Томск
            zoom: 10,
            behaviors: ['default', 'scrollZoom']
        });

        g_map.options.set('scrollZoomSpeed', 1.0);
        g_map.controls
            // Кнопка изменения масштаба.
            .add('zoomControl', { left: 5, top: 5 })
            // Список типов карты
            .add('typeSelector')
            // Стандартный набор кнопок
            .add('mapTools', { left: 35, top: 5 });

        updateMap();

        // добавили кнопку редактирования
        var btnEditHousesCoord = new ymaps.control.Button('Переместить метки');
        btnEditHousesCoord.events
            .add('select', function () {
                var allPmarks = ymaps.geoQuery(g_map.geoObjects);
                allPmarks.setOptions({
                    preset: 'twirl#redIcon',
                    draggable: true
                });
            })
            .add('deselect', function () {
                var allPmarks = ymaps.geoQuery(g_map.geoObjects);
                allPmarks.setOptions({
                    preset: 'twirl#blueIcon',
                    draggable: false
                });
            });

        g_map.controls.add(btnEditHousesCoord, { right: 90 });     
    }
}

function updateMap() {
    //console.log('1');
    if (ymaps != undefined && g_map != undefined) {
        var addresses = [];
        var houses = [];

        var lista = $('#grid_group_houses').getDataIDs();
        for (i = 0; i < lista.length; i++) {
            rowData = $('#grid_group_houses').getRowData(lista[i]);
            if (rowData.Latitude == undefined || rowData.Latitude == "" || rowData.Longitude == undefined || rowData.Longitude == "") {
              //  console.log(rowData);
                var o = {};
                o.Address = rowData.Address;
                o.Id_house = rowData.Id_house;
                o.Baloon = rowData.Baloon;
                addresses.push(o);

            } else {
              //  console.log(rowData);
                var o = {};
                o.Id_house = rowData.Id_house;
                o.Latitude = rowData.Latitude;
                o.Longitude = rowData.Longitude;
                o.Baloon = rowData.Baloon;
                houses.push(o);
            }
        }

       // console.log('2');
        var n = 0;
        addresses.forEach(function (address, index) {
         //   console.log(address);
            ymaps.geocode('Томск ' + address.Address, ymaps.util.extend({}, self._options))
                .then(
                    function (response) {
                        n++;
                        var obj = response.geoObjects.get(0);

                        // Обновляем координаты дома
                        var o = {};
                        o.Token = $.cookie("token");
                        o.Id_house = address.Id_house;
                        o.Latitude = obj.geometry.getCoordinates()[0];
                        o.Longitude = obj.geometry.getCoordinates()[1];

                        $.ajax({
                            url: "/api/coordsapi/",
                            type: "POST",
                            contentType: "application/json;charset=utf-8",
                            data: JSON.stringify(o),
                            dataType: "json",
                            async: false,
                            success: function (response) {
                                var o1 = {};
                                o1.Id_house = o.Id_house;
                                o1.Latitude = o.Latitude;
                                o1.Longitude = o.Longitude;
                                o1.Baloon = address.Baloon;
                                houses.push(o1);
                                //console.log('Координаты обновлены - ' + o1);
                            },
                            error: function (x, e) {
                                //console.log('Ошибка обновления координат');
                            }
                        });

                        if (n == addresses.length) {
                            placeHouses(houses);
                        }
                    },
                    function (err) {
                        promise.reject(err);
                    }
                );
        });

        if (addresses.length == 0) {
            placeHouses(houses);
        }
    }
}

function refreshMap() {
    setTimeout(showMap, 300);
}