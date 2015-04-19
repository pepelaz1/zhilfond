if (!('forEach' in Array.prototype)) {
    Array.prototype.forEach = function (action, that /*opt*/) {
        for (var i = 0, n = this.length; i < n; i++)
            if (i in this)
                action.call(that, this[i], i, this);
    };
}


$.datepicker.regional['ru'] = {
    closeText: 'Закрыть',
    prevText: '&#x3c;Пред',
    nextText: 'След&#x3e;',
    currentText: 'Сегодня',
    monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
    'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
    monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
    'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
    dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
    dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
    dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
    weekHeader: 'Не',
    dateFormat: 'dd.mm.yy',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};


function hex2a(hex) {
    var str = '';
    for (var i = 0; i < hex.length; i += 2)
        str += String.fromCharCode(parseInt(hex.substr(i, 2), 16));
    return str;
}


function showTopMenu() {
    $("#top_menu").css("display", "block");
    $(".top_menu_item_all").css("display", "block");

    if (isAdmin($.cookie('token')))
        $(".top_menu_item_admin").css("display", "block");
}

function isAdmin(token) {
    var res = false;
    $.ajax(
        {
            type: 'GET',
            async: false,
            url: '/api/usersapi/?token=' + token,
            success: function (data) {
                res = (data.Role == "Администраторы");
            },
            error: function (x, e) {
                res = false;
            }
        });
    return res;
}

function getUser(token) {
    var res = false;
    $.ajax(
        {
            type: 'GET',
            async: false,
            url: '/api/usersapi/?token=' + token,
            success: function (data) {
                res = data.Login;
            },
            error: function (x, e) {
                res = false;
            }
        });
    return res;
}

function canEdit(token, form, category) {
   // console.log('canEdit');
    var res = false;
    $.ajax(
        {
            type: 'GET',
            async: false,
            url: '/api/accessapi/?token=' + token + '&form=' + encodeURIComponent(form) + '&category=' + encodeURIComponent(category),
            success: function (data) {
                data.forEach(function (entry) {
                    if(entry.Level == 'Полный доступ' || entry.Level == 'Чтение/запись')
                        res = true;
                })
            },
            error: function (x, e) {
                res = false;
            }
        });
    return res;
}

function canEditHouse(token, id_house) {

   // console.log('canEditHouse');
    var res = false;
    $.ajax(
        {
            type: 'GET',
            async: false,
            url: '/api/accessapi/?token=' + token + '&id_house=' + id_house,
            success: function (data) {
                data.forEach(function (entry) {
                    if (entry.Level == 'Полный доступ' || entry.Level == 'Чтение/запись')
                        res = true;
                })
            },
            error: function (x, e) {
                res = false;
            }
        });
    return res;
}