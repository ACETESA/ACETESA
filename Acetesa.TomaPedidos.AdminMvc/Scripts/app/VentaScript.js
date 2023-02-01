
function OnBegin() {
    blockScreen();
}

function OnComplete() {
    unBlockScreen();
}
function OnFailure(data) {
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        alert(errorMessages.errorMessage);
    } else {
        alert(errorMessages);
    }
}

(function ($) {

    var $btnEnviar = $("#btnEnviar");
    $(document).on("click", "ul.pagination a[href]", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var idPage = $(this).attr("href").split("=")[1];
        var $page = $("#page");
        $page.val(idPage);
        $btnEnviar.trigger("click");
    });

    $btnEnviar.on("click", function (e) {
        if (!EsFecha($("#FechaInicio").val())) {
            toastr.info("La fecha inicial es incorrecta.", "Información", {
                timeOut: 3000
            });
            $("#FechaInicio").focus();
            return false;
        }
        if (!EsFecha($("#FechaFinal").val())) {
            toastr.info("La fecha final es incorrecta.", "Información", {
                timeOut: 3000
            });
            $("#FechaFinal").focus();
            return false;
        }
        if (!ComparaFechas($("#FechaInicio").val(), $("#FechaFinal").val())) {
            toastr.info("La fecha inicial no puede ser mayor a la fecha final.", "Información", {
                timeOut: 3000
            });
            $("#FechaFinal").focus();
            return false;
        }
    });

})(jQuery);
function descargarPDF(documento, path) {
    var sUrl = "descargarPDF/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}
function descargarZIP(documento, path) {
    var sUrl = "descargarZIP/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}
function descargarXML(documento, path) {
    var sUrl = "descargarXML/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}


//Envio por correo
var emailService = new SendEmailVentaService();
var gs_rutaPDF;
var gs_nroDocumento;
function onclickEnviarCorreo(ruc, nroDocumento, rutaPDF) {
    gs_rutaPDF = rutaPDF;
    gs_nroDocumento = nroDocumento;
    _tipoFormulario = "Venta";
    emailService.GetEmailByClienteVenta(ruc, nroDocumento);
}

var $btnSendMailModal = $("#btnSendMailModal");
$btnSendMailModal.on("click", function () {
    emailService.Send(function () {
    });
});