
toastr.options.closeButton = true;
toastr.options.preventDuplicates = true;

function OnBegin() {
    blockScreen();
}
function OnSuccess(data) {
    if ($.trim(data).length === 0) {
        toastr.info("No existen datos.", "Información", {
            timeOut: 3000
        });
    } else {
        ScrollToElement("#result",2000);
    }
}
function OnComplete() {
    unBlockScreen();
}
function OnFailure(data) {
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        toastr.warning(errorMessages.errorMessage);
    } else {
        toastr.warning(errorMessages);
    }
}

function OnBeginAnular() {
    blockScreen();
}

function OnCompleteAnular() {
    unBlockScreen();
}

function OnSuccessAnular(response) {
    if (response.estado) {
        toastr.success(response.estado, {
            timeOut: 3000
        });
        var $btnAnular = $("a[id='btnAnular" + response.id + "']");
        var $parenttr = $btnAnular.closest("tr");
        $parenttr.addClass("alert-danger");
        var $lasttd = $parenttr.find("td:last-child");
        $lasttd.text("ANULADO");
        $btnAnular.remove();
    }
}

function OnFailureAnular(data) {
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        toastr.warning(errorMessages.errorMessage);
    } else {
        toastr.warning(errorMessages);
    }
}

(function ($) {
    $.datepicker.regional["es"] = {
        closeText: "Cerrar",
        prevText: "Anterior",
        nextText: "Siguiente",
        currentText: "Hoy",
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
        dayNamesShort: ["Dom", "Lun", "Mar", "Mié", "Juv", "Vie", "Sáb"],
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sá"],
        weekHeader: "Sm",
        firstDay: 1,
        numberOfMonths: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    };
    $.datepicker.setDefaults($.datepicker.regional["es"]);
    $.validator.methods["date"] = function (value, element) {
        var shortDateFormat = "dd/mm/yy";
        var res = true;
        try {
            $.datepicker.parseDate(shortDateFormat, value);
        } catch (error) {
            res = false;
        }
        return res;
    };
    var fechaActual = new Date();
    var yearFinal = fechaActual.getFullYear();
    var yearInicio = yearFinal - 2;
    $("#FechaInicio,#FechaFinal").datepicker({
        dateFormat: "dd/mm/yy",
        changeYear: true,
        yearRange: yearInicio + ":" + yearFinal
    });
    $("#FechaInicio,#FechaFinal").mask("99/99/9999", { placeholder: "dd/mm/yyyy" });
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
              .insertAfter(this.element);

            this.element.hide();
        },

    });


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
        _tipoFormulario = "Pedido";
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

    var $btnsSendMailList = $("a[class *= 'send-email']");
    var idPedido = 0;
    $(document).on("click", "a[class *= 'send-email']", function (e) {
        e.preventDefault();
        e.stopPropagation();
        idPedido = $(this).attr("id");
        var idcliente = $.trim($(this).attr("idcliente"));

        if (idcliente.length === 0) {
            toastr.warning("Debe seleccionar un cliente.");
            return;
        }
        var request = new Ajax();
        var url = _baseUrl + "Pedido/GetEmailCliente";
        var params = {
            id: idcliente,
            tipo: 'P',
            Nro: idPedido
        };
        var $Para = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        request.JsonPost(url, params, function (response) {
            $Para.val($.trim(response.para));
            $ConCopia.val($.trim(response.conCopia));
            $Asunto.val($.trim(response.asunto));
        }).fail(function (data) {
            OnFailure(data);
        });
        $("#modal-send-mail").modal("show");
    });

    $(document).on("click", "#btnSendMailModal", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var $Para  = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        
        var request = new Ajax();
        var url = _baseUrl + "Pedido/EnviarMail";
        var params = {
            Para: $Para.val(),
            ConCopia: $ConCopia.val(),
            Asunto: $Asunto.val(),
            Mensaje: $Mensaje.val(),
            id: idPedido
        };
        toastr.info("Enviado mail. Espere por favor.");
        request.JsonPost(url, params, function (response) {
            toastr.success("Mail enviado.");
            $Para.val("");
            $ConCopia.val("");
            $Asunto.val("");
            $Mensaje.val("");
            var $parenttr = $("a#" + idPedido).closest("tr");
            $parenttr.addClass("alert-info");
            var $lasttd = $parenttr.find("td:last-child");
            $lasttd.text("ENVIADO");
            idPedido = 0;
            $parenttr.remove();
            $("#btnCloseModal").trigger("click");
        }).fail(function (data) {
            OnFailure(data);
        });
    });
})(jQuery);


//Propiedad selectPicker Articulos
$(document).ready(function () {
    LlenarClientesSelect();
    $('.selectpicker').selectpicker({
        size: 5,
        title: 'Seleccione una opción'
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
            Cliente.append("<option class='fw-bold' value=''>Todos los clientes</option>");
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
