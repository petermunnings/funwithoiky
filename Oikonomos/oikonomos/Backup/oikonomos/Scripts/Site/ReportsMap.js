function FetchPeople() {
    var jqxhr = $.post("/Ajax/FetchPeopleInChurch", function (data) {
        var churchLatlng = new google.maps.LatLng(data.MapData.ChurchLat, data.MapData.ChurchLng);
        var mapOptions = {
            center: churchLatlng,
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("peopleInChurchMap"), mapOptions);
        var bounds = new google.maps.LatLngBounds();

        var churchImage = '/Content/images/chapel.png';
        var homeImage = '/Content/images/home.png';
        var hgImage = '/Content/images/homegroup.png';

        var churchMarker = new google.maps.Marker({
            map: map,
            draggable: false,
            title: data.MapData.ChurchName,
            icon: churchImage
        });

        churchMarker.setPosition(churchLatlng);
        bounds.extend(churchLatlng);

        
        $.each(data.MapData.Members, function (i, item) {
            var memberLatLng = new google.maps.LatLng(item.Lat, item.Lng);
            var memberMarker = new google.maps.Marker({
                map: map,
                draggable: false,
                title: item.Surname,
                icon: homeImage
            });

            memberMarker.setPosition(memberLatLng);
            bounds.extend(memberLatLng);
        });

        $.each(data.MapData.HomeGroups, function (i, item) {
            var hgLatLng = new google.maps.LatLng(item.Lat, item.Lng);
            var hgMarker = new google.maps.Marker({
                map: map,
                draggable: false,
                title: item.GroupName,
                icon: hgImage
            });

            hgMarker.setPosition(hgLatLng);
            bounds.extend(hgLatLng);
        });

        map.fitBounds(bounds);





    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#ajax_loader").hide();
        SendErrorEmail("Error calling FetchPeopleInChurch", jqXHR.responseText);
    });
}

$(document).ready(function () {

    FetchPeople();

})