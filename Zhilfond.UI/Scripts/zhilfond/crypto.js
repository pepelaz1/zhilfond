
function buildXml() {
    var hash = new Hashtable();

    $("*[class^='fld_']").each(function (idx) {
        //console.log('123');
        if (g_isnew == true || g_hash.get($(this).attr('tag')).value != $(this).val()) {
            var o = new Object();
            if ($(this).is('select')) {
                o.value = $("option:selected", this).text();
            } else {
                o.value = $(this).val();
            }
            o.type = $(this).attr('tag1');
            //console.log(o.value + '-' + $(this).val());
            hash.put($(this).attr('tag'), o);
            // console.log($(this).attr('tag') + ' было ' + g_hash.get($(this).attr('tag')) + ' стало ' + $(this).is('select') ? $("option:selected", this).text() : $(this).val());
        }
    });

    if (hash.size() == 0)
        return "";

    var grid = $("#gridMain");
    var idx = grid.getGridParam("selrow");
    var rowData = grid.getRowData(idx);
    var house = "";

    if (g_isnew && g_currform == "Основные данные") {
        house = '<House nasp="" district="" street="" number="" letter=""/>';
    }
    else {
        house = '<House nasp="' + rowData.nasp + '" district="' + rowData.raion + '" street="' + rowData.street + '" number="' + rowData.number + '" letter="' + rowData.letter + '"/>'
    }

    //var operation = "";
    //if (g_isnew) {
    //    operation = 'Insert';
    //} else {
    //    operation = 'Update';
    //}


    var inner = "";
    hash.entries().forEach(function (e) {
       // console.log(e);
        inner += '<Field name="' + e[0] + '" type="' + e[1].type + '">' + e[1].value + '</Field>';
    });


    var xml = '<?xml version="1.0" encoding="utf-8"?>'
    xml += '<ImportData>'
         + '<Element>'
         + house
         //+ '<Form operation="' + operation + '" parent="' + g_parent + '">' + g_currform + '</Form>'
         + '<Form parent="' + g_parent + '">' + g_currform + '</Form>'
         + inner
         + '</Element>'
         + '</ImportData>';

    console.log(xml);
    return xml;
}


function signData(message, key, pass) {
    var signature = message.toString(CryptoJS.enc.Base64);
    //console.log('signature=' + signature);

    key = hex2a(key);
    // console.log('key=' + key);

    var deckey = CryptoJS.AES.decrypt(key, pass);
    var dkey = hex2a(deckey.toString());
   // console.log('dkey='+dkey);

    var rsa = new RSAKey();
    try {
        rsa.readPrivateKeyFromPEMString(dkey);
    } catch (e) {
        alert('Ошибка дешифрации закрытого ключа пользователя. Возможно неправильный пароль.');
        return null;
    }

    try {
        var hsig = rsa.signString(signature, "sha256");
       // console.log("hsig=" + hsig);
        return hsig;
    }
    catch (e) {
        alert('Ошибка создания электронной подписи');
    }   
    return null;
}


function removeKey() {
    $.removeCookie('PrivateKey', { path: '/' });
    $.removeCookie('PrivateKeyPass', { path: '/' });
}

function saveData(key, pass, data, sourceType, id_source) {

    var xml = "";
    if (data == "")
        // create full xml
        xml = buildXml();
    else
        xml = data;
    //  console.log('message='+ xml);

    var sha = CryptoJS.SHA256(xml);
    // console.log('sha='+sha);

    var sign = signData(sha, key, pass);
    if (sign == null) {
        removeKey();
        return ;
    }
   // console.log('sign=' + sign);
  //  return;

    var token = $.cookie("token");

    //console.log(g_isnew);
    // if (g_isnew) {
    $.ajax({
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        async: false,
        data: JSON.stringify({ Token: token, Xml: xml, Signature: sign, SourceType: sourceType,  Id_source: id_source }),
        url: '/api/valuesapi/',
        dataType: 'json',
        success: function (data) {
           // console.log(data);
            var grid = $("#gridMain");
            var idx = grid.getGridParam("selrow");
            var rowData = grid.getRowData(idx);
            g_id_house = rowData.Id;
            grid.trigger('reloadGrid');

            $("#btn_save").attr('style', 'display:none');
            $("#btn_cancel").attr('style', 'display:none');
        },
        error: function (data) {
           // console.log(data);
         //   alert(data.responseText.replace('"', '').replace('"', ''));
            removeKey();
            return;
        }
    });

    g_isnew = false;
}


function submitKeyDlg(data, sourceType, id_source) {
    if ($('#key_filename').val() == '') {
        alert('Необходимо выбрать ключ');
        return;
    }
    if ($('#key_password').val() == '') {
        alert('Необходимо ввести пароль к ключу');
        return;
    }

    $('#dlg_key_selection').dialog("close");


    $.cookie('PrivateKey', $('#key_filename').val(), { path: '/' });
    $.cookie('PrivateKeyPass', $('#key_password').val(), { path: '/' });

    saveData($.cookie('PrivateKey'), $.cookie('PrivateKeyPass'), data, sourceType, id_source);
}

function save(data,sourceType, id_source) {
    var key = $.cookie('PrivateKey');
    var pass = $.cookie('PrivateKeyPass');

    //console.log("key=" + key);
    //console.log("pass=" + pass);
 
    if (key == undefined || key == '' || pass == undefined || pass == '') {

        $("#dlg_key_selection").dialog({
            height: 230,
            width: 500,
            modal: true,
            buttons: {
                "Продолжить": function () {
                    submitKeyDlg(data, sourceType, id_source);
                }
            }
        });

        $('#dlg_key_selection').keypress(function (e) {
            if (e.keyCode == $.ui.keyCode.ENTER) {
                submitKeyDlg(data, sourceType, id_source);
            }
        });
    } else {
        saveData(key, pass, data, sourceType, id_source);
    }
}

//function validateData(data, sign) {
//    var res = false;
//    $.ajax(
//       {
//           type: 'GET',
//           async: false,
//           url: '/api/keysapi/?login=admin',
//           success: function (result) {
//               var x509 = new X509();
//               var key = result.KeyValue;
//               //console.log(key);
//               x509.readCertPEM(key);
//               res = x509.subjectPublicKeyRSA.verifyString(data, sign);
//           },
//           error: function (x, e) {
//               res = false;
//           }
//       });
//    return res;
//}