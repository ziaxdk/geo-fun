(function () {
    var m = angular.module('geofun', ['ngAnimate']);

    m.controller("c", ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        var t = this;


        t.latLon = null;
        t.newest = null;
        t.newestFlag = false;
        t.msg = [];

        var map = L.map('map').setView([56, 11], 8);
        L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png').addTo(map);
        map.on('click', function (e) {
            $scope.$apply(function () {
                t.latLon = e.latlng;
            });
        });

        var jsonGroup = L.featureGroup().addTo(map);
        var num = 1;

        $scope.$watch(function () { return t.latLon }, function (n, o) {
            if (!n) return;
            $http.get('/api/geo', { params: { lat: n.lat, lon: n.lng } }).then(function (res) {
                var data = res.data;
                t.msg.splice(0, 1, { id: num++, val: data.stats.took });
                if (!data.res) return;
                t.newest = data.res.nr + ' ' + data.res.navn;
                t.newestFlag = true;
                $timeout(function () {
                    t.newestFlag = false;
                }, 2000);
                L.geoJson(data.res.polygon, {}).bindPopup(t.newest).addTo(jsonGroup);
            });
        });
    }]);
}());
