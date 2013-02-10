
var googleResults;

Google = {
    searchAddress: function (address) {
        var dfr = $.Deferred();
        var geocoder = new google.maps.Geocoder();
        if (geocoder) {
            geocoder.geocode({ "address": address, "region": "South Africa" },
                            function (results, status) {
                                if (status == google.maps.GeocoderStatus.OK) {
                                    googleResults = results;

                                    dfr.resolve($.map(results, function (item, index) {
                                        var returnLabel = '';
                                        var returnValue = '';
                                        $.each(item.address_components, function (index, value) {
                                             $.each(value.types, function (i, type) {
                                                if (type.indexOf("street_number") > -1 || type.indexOf("route") > -1 || type.indexOf("locality") > -1 || type.indexOf("sublocality") > -1) {
                                                    if (type.indexOf("street_number") > -1) {
                                                        returnLabel += value.short_name + " ";
                                                    }
                                                    else {
                                                        returnLabel += value.short_name + ", ";
                                                    }
                                                }
                                                if (type.indexOf("street_number") > -1 || type.indexOf("route") > -1) {
                                                    returnValue += value.short_name + " ";
                                                }
                                            });
                                        });

                                        returnValue = returnValue.slice(0, returnValue.length - 1);
                                        returnLabel = returnLabel.slice(0, returnLabel.length - 2);

                                        return {
                                            label: returnLabel,
                                            value: returnValue,
                                            id: index
                                        }
                                    }));
                                }
                                else {
                                    dfr.resolve("");
                                }
                            })
        }
        return dfr.promise();
    }
}

function AddressSelected(id, startIndex, prefix, includeProvince) {

    $("#hidden_" + prefix + "addressType").val(googleResults[id].types);
    for (i = startIndex; i < 5; i++) {
        $("#text_" + prefix + "address" + i).val(" ");
    }


    $.each(googleResults[id].address_components, function (index, value) {
        $.each(value.types, function (i, type) {
            if (includeProvince) {
                if (type.indexOf("administrative_area_level_1") > -1) {
                    $("#hidden_" + prefix + "Province").val(value.long_name);
                }
            }
            if (type.indexOf("sublocality") > -1) {
                $("#text_" + prefix + "address3").val(value.long_name);
            }

            if (type.indexOf("locality") > -1 && type.indexOf("sublocality") == -1) {
                $("#text_" + prefix + "address4").val(value.long_name);
            }
        });
    });
    $("#hidden_" + prefix + "lat").val(googleResults[id].geometry.location.lat());
    $("#hidden_" + prefix + "lng").val(googleResults[id].geometry.location.lng());
    if (startIndex == 1) {
        $("#hidden_" + prefix + "addressChosen").val("selected");
    }
}
