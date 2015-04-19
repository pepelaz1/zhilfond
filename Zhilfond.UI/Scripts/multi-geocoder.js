/**
 * @fileOverview
 * ������ ���������� ���������������� �������������� ��������������.
 * ����������� ���������� ��� ������ ������ ���.
 * @see http://api.yandex.ru/maps/doc/jsapi/1.x/examples/multiplygeocoding.html
 * @example

   var multiGeocoder = new MultiGeocoder({ boundedBy : map.getBounds() });

   multiGeocoder
       .geocode(['������, ���� �������� 16', [55.7, 37.5], '�����-���������'])
           .then(
               function (res) {
                   map.geoObjects.add(res.geoObjects);
               },
               function (err) {
                   console.log(err);
               }
           );
 */

/**
 * ����� ��� �������������� ������ ������� ��� ���������.
 * @class
 * @name MultiGeocoder
 * @param {Object} [options={}] ��������� ����� ���������������.
 */
function MultiGeocoder(options) {
    this._options = options || {};
}

/**
 * ������� ��������������� ��������������.
 * @function
 * @requires ymaps.util.extend
 * @see http://api.yandex.ru/maps/doc/jsapi/2.x/ref/reference/util.extend.xml
 * @requires ymaps.util.Promise
 * @see http://api.yandex.ru/maps/doc/jsapi/2.x/ref/reference/util.Promise.xml
 * @name MultiGeocoder.geocode
 * @param {Array} requests ������ �����-���� ��������� �/��� ��������� ����� (�������� ��������������)
 * @returns {Object} ��� � � ������� ���������, ������ ������-��������.
 */
MultiGeocoder.prototype.geocode = function (requests, options) {
    var self = this,
        size = requests.length,
        promise = new ymaps.util.Promise(),
        geoObjects = new ListCollection();

    requests.forEach(function (request, index) {
        ymaps.geocode(request, ymaps.util.extend({}, self._options, options))
            .then(
                function (response) {
                    var geoObject = response.geoObjects.get(0);

                    geoObject && geoObjects.add(geoObject, index);
                    --size || promise.resolve({ geoObjects : geoObjects });
                },
                function (err) {
                    promise.reject(err);
                }
            );
    });

    return promise;
};