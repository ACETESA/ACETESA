
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
        //validarProductos();
    }
}
function OnComplete() {
    unBlockScreen();
}

//function validarProductos() {
//    $(".GenerarPedido").each(function () {
//        var cnProforma = $(this).attr('id');
//        cnProforma = cnProforma.substring(10, cnProforma.length);
//        var btnGenerar = document.getElementById($(this).attr('id'));
//        $.ajax({
//            destroy: true,
//            type: "POST",
//            contentType: "application/json; charset=utf-8",
//            url: urlValidaProductosCotizacion,
//            data: JSON.stringify({
//                cnProforma: cnProforma
//            }),
//            dataType: "json",
//            success: function (result) {
//                id = result.id;
//                if (id == "0") {
//                    btnGenerar.setAttribute("onclick", "toastr.error('" + result.mensaje + "'); event.preventDefault();");
//                }
//                btnGenerar.removeAttribute("disabled");
//            },
//            error: function (result) {
//                alert("Error en javascript...");
//            }
//        });

//    });
//}

function OnFailure(data) {
    var errorMessages = JSON.parse(JSON.stringify(data.responseText));
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
        if (response.estado == "Cambio de estado a Anulado.") {
            var $btnAnular = $("a[id='btnAnular" + response.id + "']");
            $btnAnular.removeClass();
            $btnAnular.addClass("btn btn-default btn-sm glyphicon glyphicon-plus-sign");
            $btnAnular.prop('title', 'Restablecer');
            var $btnRechazar = $("a[id='btnRechazar" + response.id + "']");
            var $parenttr = $btnAnular.closest("tr");
            $parenttr.addClass("alert-danger");
            var $lasttd = $parenttr.find("td:last-child");
            $lasttd.text("ANULADO");
            $btnRechazar.remove();
        }
        else if (response.estado == "Cambio de estado a Emitido.") {
            var $btnAnular = $("a[id='btnAnular" + response.id + "']");
            $btnAnular.removeClass();
            $btnAnular.addClass("btn btn-default btn-sm glyphicon glyphicon-minus-sign");
            $btnAnular.prop('title', 'Anular');
            var $btnRechazar = $("a[id='btnRechazar" + response.id + "']");
            var $parenttr = $btnAnular.closest("tr");
            $parenttr.removeClass("alert-danger");
            var $lasttd = $parenttr.find("td:last-child");
            $lasttd.text("EMITIDO");
            $btnRechazar.remove();
        }
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

function OnBeginRechazar() {
    blockScreen();
}

function OnCompleteRechazar() {
    unBlockScreen();
}

function OnSuccessRechazar(response) {
    if (response.estado) {
        toastr.success(response.estado, {
            timeOut: 3000
        });
        var $btnAnular = $("a[id='btnAnular" + response.id + "']");
        var $btnRechazar = $("a[id='btnRechazar" + response.id + "']");
        var $parenttr = $btnAnular.closest("tr");
        $parenttr.addClass("alert-danger");
        var $lasttd = $parenttr.find("td:last-child");
        $lasttd.text("RECHAZADO");
        $btnAnular.remove();
        $btnRechazar.remove();
    }
}

function OnFailureRechazar(data) {
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        toastr.warning(errorMessages.errorMessage);
    } else {
        toastr.warning(errorMessages);
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
        _tipoFormulario = "Cotizacion";


        //if (!EsFecha($("#FechaInicio").val())) {
        //    toastr.info("La fecha inicial es incorrecta.", "Información", {
        //        timeOut: 3000
        //    });
        //    $("#FechaInicio").focus();
        //    return false;
        //}
        //if (!EsFecha($("#FechaFinal").val())) {
        //    toastr.info("La fecha final es incorrecta.", "Información", {
        //        timeOut: 3000
        //    });
        //    $("#FechaFinal").focus();
        //    return false;
        //}
        //if (!ComparaFechas($("#FechaInicio").val(), $("#FechaFinal").val())) {
        //    toastr.info("La fecha inicial no puede ser mayor a la fecha final.", "Información", {
        //        timeOut: 3000
        //    });
        //    $("#FechaFinal").focus();
        //    return false;
        //}

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



//                    btnGenerar.setAttribute("onclick", "toastr.error('" + result.mensaje + "'); event.preventDefault();");


function mensajeValidacionArticulos(idmensaje, mensaje) {
    toastr.error(mensaje, "Validación de Artículos", {
        closeButton: true,
        timeOut: 30000,
        progressBar: true
    });
}


function ValidarTrnasformacionCotizacionAPedido(CotizacionID) {
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlValidarTransformacionCotizacionAPedido,
        data: JSON.stringify({
            CotizacionID: CotizacionID,
        }),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var mensajeID = value["item1"];
                var mensaje = value["item2"];

                if (mensajeID == "0") {
                    toastr.error(mensaje, "Validación de Stock de Artículos", {
                        closeButton: true,
                        timeOut: 30000,
                        progressBar: true
                    });
                }
                else {
                    var URL = "/Pedido/Nuevo/" + CotizacionID;
                    window.location = URL;
                }
            });

            //alert(JSON.stringify(result));
            //for (var i = 0; i < result.length; i++) {
            //    //Cliente.append("<option value='" + result[i].cc_analis + "'>" + result[i].cd_razsoc + "</option>");
            //}
            
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}