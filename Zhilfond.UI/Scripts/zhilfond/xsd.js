function showXsdDetails() {
    g_currform = 'Создать XSD';
    $('.div_title_details').text(g_currform);
}

$("#btn_xsd").button().click(function (e) {
    window.location = "/api/xsdgeneratorapi?token=" + $.cookie('token');
});