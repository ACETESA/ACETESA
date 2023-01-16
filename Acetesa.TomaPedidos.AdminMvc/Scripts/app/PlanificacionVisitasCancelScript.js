


function LlenarSelectMotivoCancelacion() {
    $("#SMotivoCancelacion").html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaMotivosCancelacion,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("#SMotivoCancelacion").append("<option value='" + result[i].motivoID + "'>" + result[i].motivo + "</option>");
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


$('#cancelVisitaModel').on('shown.bs.modal', function (e) {

    LlenarSelectMotivoCancelacion();
})


$('#cancelVisitaModel').on('hidden.bs.modal', function () {

    $("#SMotivoCancelacion").html("");
    $("#ObservacionCancelación").val("");
    gs_visitaClienteID = null;

})





//Cancelar visita cliente - Registrar
function CancelarVisitaCliente() {

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlCancelarVisitaCliente,
        data: JSON.stringify({
            visitaClienteID: gs_visitaClienteID,
            motivoCancelacionID: $("#SMotivoCancelacion option:selected").val(),
            observacionCancelacion: $("#ObservacionCancelación").val(),
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                if (result[i].mensajeID = 1) {
                    //Mostrar mensaje
                    toastr.success(result[i].mensaje);
                    //Cerar Modal
                    $('#cancelVisitaModel').modal('toggle');
                    //Recargar listado principal
                    buscarVisitasMain();
                }
                else {
                    toastr.error(result[i].mensaje);
                    return;

                }
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}