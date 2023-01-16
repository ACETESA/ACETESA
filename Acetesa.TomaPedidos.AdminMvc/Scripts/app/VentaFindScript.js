/// <reference path="GlobalScript.js" />
/// <reference path="~/Scripts/jquery-ui-1.11.4.js" />
/// <reference path="~/Scripts/jquery.validate-vsdoc.js" />
/// <reference path="~/Scripts/toastr.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />

$(document).on("click", "#btnSendMailModal", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var $Para = $("#Para");
    var $ConCopia = $("#ConCopia");
    var $Asunto = $("#Asunto");
    var $Mensaje = $("#Mensaje");

    var request = new Ajax();
    var url = _baseUrl + "Venta/EnviarMail";
    var params = {
        Para: $Para.val(),
        ConCopia: $ConCopia.val(),
        Asunto: $Asunto.val(),
        Mensaje: $Mensaje.val(),
        id: gs_nroDocumento,
        rutaPDF: gs_rutaPDF,
    };
    toastr.info("Enviado mail. Espere por favor.");
    request.JsonPost(url, params, function (response) {
        toastr.success("Mail enviado.");
        $Para.val("");
        $ConCopia.val("");
        $Asunto.val("");
        $Mensaje.val("");
        $("#btnCloseModal").trigger("click");
    }).fail(function (data) {
        OnFailure(data);
    });
});