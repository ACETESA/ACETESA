function OnBegin() {
    blockScreen();
}
function OnFailure(data) {
    //var errorMessages = JSON.parse(JSON.stringify(data.responseText));
    //if (errorMessages.errorMessage) {
    //    alert(errorMessages.errorMessage);
    //} else {
    //    alert(errorMessages);
    //}
}
function OnComplete() {
    unBlockScreen();
}

function OnSuccess() {

    if ($('#result').length) {
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#result").offset().top
        }, 2000);
    }

}



(function ($) {

    //var urlDetalle = "";
    //var urlResumen = "";
    //function addUrlDetalle(object, id) {
    //    var ancla = $(object);
    //    if ($.trim(urlDetalle) == "") {
    //        urlDetalle = ancla.attr("href");
    //    }
    //    var urlRedirect = urlDetalle + "/" + id;
    //    ancla.attr("href", urlRedirect);
    //}
    //function addUrlResumen(object, id) {
    //    var ancla = $(object);
    //    if ($.trim(urlResumen) == "") {
    //        urlResumen = ancla.attr("href");
    //    }
    //    var urlRedirect = urlResumen + "/" + id;
    //    ancla.attr("href", urlRedirect);
    //}

    //var $Cliente = $("#Cliente");
   

    //$("#btnDetalle").click(function (e) {
    //    if ($.trim($Cliente.val()).length > 0) {
    //        addUrlDetalle(this, $Cliente.val());
    //    } else {
    //        e.preventDefault();
    //        $msgeCliente.text("Campo requerido");
    //    }
    //});
    //$("#btnResumen").click(function (e) {
    //    if ($.trim($Cliente.val()).length > 0) {
    //        addUrlResumen(this, $Cliente.val());
    //    } else {
    //        e.preventDefault();
    //        $msgeCliente.text("Campo requerido");
    //    }
    //});

})(jQuery);
function descargar() {
    var $Cliente = $("#Cliente");
    if ($.trim($Cliente.val()).length > 0) {
        var sUrl = urlDescargarEC + "/?id=" + $Cliente.val() + "&razSoc=" + $("#Cliente option:selected").text();// "/EstadoCuenta/descargar/?id=" + $Cliente.val() + "&razSoc=" + $("#Cliente option:selected").text();
        $.fileDownload(sUrl, {
            failMessageHtml: "There was a problem generating your report, please try again."
        });
        return false;
    } else {
        var $msgeCliente = $("#msgeCliente");
        $msgeCliente.text("Campo requerido");
    }
}
//Para enviar los correos
var emailService = new SendEmailEstadoCuentaService();

function onclickEnviarCorreo() {
    var $Cliente = $("#Cliente");
    if ($.trim($Cliente.val()).length > 0) {
        _tipoFormulario = "EstadoCuenta";
        emailService.GetEmailByClienteEstadoCuenta($Cliente.val());
    } else {
        var $msgeCliente = $("#msgeCliente");
        $msgeCliente.text("Campo requerido");
    }
}

var $btnSendMailModal = $("#btnSendMailModal");
$btnSendMailModal.on("click", function () {
    emailService.Send(function () {
    });
});

//Enviando correos
$(document).on("click", "#btnSendMailModal", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var $Para = $("#Para");
    var $ConCopia = $("#ConCopia");
    var $Asunto = $("#Asunto");
    var $Mensaje = $("#Mensaje");
    var $Cliente = $("#Cliente");

    var request = new Ajax();
    var url = urlEnviarMail;
    var params = {
        Para: $Para.val(),
        ConCopia: $ConCopia.val(),
        Asunto: $Asunto.val(),
        Mensaje: $Mensaje.val(),
        idCliente: $Cliente.val(),
    };
    if ($Para.val() == "") {
        $Para.parent().addClass("has-error");
        return;
    }
    if ($Mensaje.val()=="") {
        return;
    }
    toastr.info("Enviado mail. Espere por favor.");
    request.JsonPost(url, params, function (response) {
        toastr.success("Mail enviado.");
        $Para.val("");
        $ConCopia.val("");
        $Asunto.val("");
        $Mensaje.val("");
        $Cliente.val("");
        $("#btnCloseModal").trigger("click");
    }).fail(function (data) {
        //OnFailure(data);
        //alert(data);
        alert(JSON.stringify(data));
    });
});


function LlenarClientesSelect() {
    Cliente = $("#Cliente");
    Cliente.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectClientesSegunCarteraVendedor,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                Cliente.append("<option value='" + result[i].cc_analis + "'>" + result[i].cd_razsoc + "</option>");
            }
            $('#Cliente').selectpicker('refresh');
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}