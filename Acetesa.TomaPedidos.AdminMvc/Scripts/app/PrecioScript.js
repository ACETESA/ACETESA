/// <reference path="../lib/Ajax.js" />
/// <reference path="GlobalScript.js" />

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
    unBlockScreen();
}

function OnSuccess() {
    //scrollToScreen("#tableSector", 1000);

    if ($('#tableSector').length)
    {
        $([document.documentElement, document.body]).animate({
        scrollTop: $("#tableSector").offset().top
        }, 2000);
    }

    ListaPreciosAux = $("#ListaPrecios").val();
    FamiliaAux = $("#Familia").val();
    SubFamiliaAux = $("#SubFamilia").val();
    StocksAux = $("#Stocks").val();
}

//(function ($) {
//    var $Familia = $("#Familia");
//    var $SubFamilia = $("#SubFamilia");
//    $Familia.change(function () {
//        var request = new Ajax("");
//        var params = {
//            codFamilia: $(this).val()
//        };
//        $SubFamilia.html("");
//        request.JsonPost("GetSubFamilias", params, function (data) {
//            $.each(data, function (index, item) {
//                var html = "<option value='" + item.Value + "'>";
//                html += item.Text;
//                html += "</option>";
//                $SubFamilia.append(html);
//            });
//        });
//    });
//})(jQuery);

//var ListaPreciosAux = null;
//var FamiliaAux = null;
//var SubFamiliaAux = null;
//var StocksAux = null;

//function ExportarExcelPrecio() {
//    var sUrl = "descargarExcelPrecio";

//    $.fileDownload(sUrl, {
//        httpMethod: "POST",
//        data: {
//            ListaPrecios: ListaPreciosAux,
//            Familia: FamiliaAux,
//            SubFamilia: SubFamiliaAux,
//            Stocks: StocksAux
//        },
//        failMessageHtml: "There was a problem generating your report, please try again."
//    });
//}