function showCheckSignatureDetails() {
    g_currform = 'Проверка ЭЦП';
    $('.div_title_details').text(g_currform);

    $("#successCheckSignDialog").dialog({
        autoOpen: false,
        title: "Уведомление"
    });
    $("#failureCheckSignDialog").dialog({
        autoOpen: false,
        title: "Уведомление"
    });
  
   // $("#grid_genreports").trigger('reloadGrid');
}

$('#btn_check_signature').button().click(function () {
    //if ($("#ta_signature").val() == '') {
    //    alert('Необходимо указать подпись')
    //    return;
    //}
   // console.log('123');
    var res = false;
    var files = $("#id_file_check").get(0).files;
    if (files && files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/api/validateapi?type=signature&token=" + $.cookie('token'),
                contentType: false,
                processData: false,
                data: data,
                async: false,
                success: function (result) {
                    //alert(result);
                    res = result;
                    //$.each(results, function (key, data) {
                    //    alert(data);
                    //    res = data;
                    //    // res = validateData(data.File, $("#ta_signature").val());
                    //    //res = validateData(data.File);
                    //});
                }, error: function (x, e) {
                 //   alert('Ошибка проверки');
                }
            });
        } else {
            alert("This browser doesn't support HTML5 multiple file uploads!");
        }
    }

    if (res == true) {
        $("#successCheckSignDialog").dialog("open");
        //alert('Файл подписан публичным ключом ЕРКЦ');
    } else {
        $("#failureCheckSignDialog").dialog("open");
        //alert('Файл подписан НЕ публичным ключом ЕРКЦ');
        //alert('Проверка ЭЦП файла прошла неуспешно\nФайл не подписан публичным ключом ЕРКЦ');
    }

    return res;
});
