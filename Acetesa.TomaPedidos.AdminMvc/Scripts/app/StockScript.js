/// <reference path="../lib/Ajax.js" />

function blockScreen() {
    $.blockUI({ message: null, overlayCSS: { backgroundColor: '#000', opacity: 0.1, zIndex: 99999 } });
}

function unBlockScreen() {
    $.unblockUI();
}

function scrollToScreen(idLabel, delay) {
    var heightMenuHeader = $(".navbar-header").height();
    $("html, body").animate({ scrollTop: $(idLabel).offset().top - heightMenuHeader }, delay);
}

function OnBegin() {
    blockScreen();
}

function OnFailure(data) {
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        alert(errorMessages.errorMessage);
    } else {
        alert(errorMessages);
    }
}

function OnComplete() {
    scrollToScreen("#result", 1000);
    unBlockScreen();
}

(function ($) {
    var $Familia = $("#Familia");
    var $SubFamilia = $("#SubFamilia");
    $Familia.change(function () {
        var request = new Ajax("");
        var params = {
            codFamilia: $(this).val()
        };
        $SubFamilia.html("");
        request.JsonPost("GetSubFamilias", params, function (data) {
            $.each(data, function (index, item) {
                var html = "<option value='" + item.Value + "'>";
                html += item.Text;
                html += "</option>";
                $SubFamilia.append(html);
            });
        });
    });
})(jQuery);