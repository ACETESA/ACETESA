/// <reference path="../lib/Ajax.js" />

//function blockScreen() {
//    $.blockUI({ message: null, overlayCSS: { backgroundColor: '#000', opacity: 0.1, zIndex: 99999 } });
//}

//function unBlockScreen() {
//    $.unblockUI();
//}

function OnBegin() {
    blockScreen();
}
function OnFailure(data) {
    var errorMessages = JSON.parse(JSON.stringify(data.responseText));
    if (errorMessages.errorMessage) {
        alert(errorMessages.errorMessage);
    } else {
        alert(errorMessages);
    }
}

function OnComplete() {
    scrollToScreen("#result", 1000);
    EmpresaAux = $("#Empresa").val();
    TiendaAux = $("#Tienda").val();
    FamiliaAux = $("#Familia").val();
    TipoAux = $("#Tipo").val();
    FechaInicioAux = $("#FechaInicio").val();
    SubGrupoAux = $("#SubGrupo").val();
    unBlockScreen();
}
//(function ($) {
//    var $Familia = $("#Familia");
//    var $SubFamilia = $("#SubGrupo");
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
//}
//)(jQuery);

var EmpresaAux = null;
var TiendaAux = null;
var FamiliaAux = null;
var SubGrupoAux = null;
var TipoAux = null;
var FechaInicioAux = null;

function ExportarExcelStock() {
    var sUrl = "descargar";
    
    $.fileDownload(sUrl, {
        httpMethod: "POST",
        data: {
            Empresa: EmpresaAux,
            Tienda: TiendaAux,
            Familia: FamiliaAux,
            Tipo: TipoAux,
            FechaInicio: FechaInicioAux,
            SubGrupo: SubGrupoAux
        },
        failMessageHtml: "There was a problem generating your report, please try again."
    });
}