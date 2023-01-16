//GLOBAL VARIABLES
var gs_latitud;
var gs_longuitud;


///JS For new Modal


//Clientes
var sClientesNew = $("#sClientesNew");
function clientesActivosListar_New() {
    sClientesNew.html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlClienteAsignado,
        data: JSON.stringify({
            departamentoId: $("#sDepartamento option:selected").val(),
            provinciaId: $("#sProvincia option:selected").val(),
            distritoId: $("#sDistrito option:selected").val(),
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                sClientesNew.append("<option value='" + result[i].ruc + "'>" + result[i].razonSocial + "</option>");
            }
            //Al reselecionar
            document.getElementById("sClientesNew").setAttribute("onchange", "LlenarSelectContactoCliente();");
            $('#sClientesNew').selectpicker({
                maxOptions: 2
            });
            $('#sClientesNew').selectpicker('refresh');

        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


$(document).ready(function () {

    onReadyPreparation();
});

function onReadyPreparation() {
    LlenarSelectDepartamento();

    $('.selectpicker').selectpicker({
        size: 5,
        title: 'Seleccione una opción'
    });

    LlenarSelectNroPlanificacion();
    LlenarSelectMotivoVisita();


    document.getElementById("EsVisitaPlanificada").setAttribute("onchange", "LlenarNroPlanificacionSegunEsPlanificada();");

}


//$('#newVisitaModel').on('shown.bs.modal', function (e) {

//})



//Nro Planificacion
var sNroPlanificacionNew = $("#sNroPlanificacionNew");
function LlenarSelectNroPlanificacion() {
    sNroPlanificacionNew.html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaNumeroPlanificacionActivas,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            if (result.length == 0) {
                //No Planificado
                $('#EsVisitaPlanificada option[value="0"]').attr("selected", "selected");
                //No permite seleccionar si es planificado o no
                $('#EsVisitaPlanificada').attr('disabled', 'disabled');
                //Llenar con NO ACTIVOS
                LlenarSelectNroPlanificacion_NOACTIVAS();
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    sNroPlanificacionNew.append("<option value='" + result[i].planificacionID + "'>" + result[i].descripcion + "</option>");
                    $('#EsVisitaPlanificada option[value="1"]').attr("selected", "selected");
                    //Cambia etiqueta select
                    $("#labelNroPlanificacion").html("Planificación (Abiertos)");
                }
            }

        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function LlenarSelectNroPlanificacion_NOACTIVAS() {
    $("#sNroPlanificacionNew").html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaNumeroPlanificacionNoActivas,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            //No Planificado
            for (var i = 0; i < result.length; i++) {
                $("#sNroPlanificacionNew").append("<option value='" + result[i].planificacionID + "'>" + result[i].descripcion + "</option>");
            }
            //Cambia etiqueta select
            $("#labelNroPlanificacion").html("Planificación (Cerrados)");
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}




function LlenarSelectMotivoVisita() {
    $("#MotivoVisita").html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaMotivosVisita,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("#MotivoVisita").append("<option value='" + result[i].motivoID + "'>" + result[i].motivo + "</option>");
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



//Contacto Cliente segun seleccion
function LlenarSelectContactoCliente() {
    $("#sContactoCliente").html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaContactoCliente,
        data: JSON.stringify({
            clienteID: $("#sClientesNew option:selected").val(),
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("#sContactoCliente").append("<option value='" + result[i].contactoID + "'>" + result[i].contacto + "</option>");
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



function LlenarNroPlanificacionSegunEsPlanificada() {
    var VisitaPlanificada = $("#EsVisitaPlanificada option:selected").val()

    if (VisitaPlanificada == 1) {
        LlenarSelectNroPlanificacion();
    }
    else {
        LlenarSelectNroPlanificacion_NOACTIVAS();
    }
}










function LimpiarFormularioNuevaVisita() {

    //Limpieza y reseteo de los campos
    $("#EsVisitaPlanificada").html("");
    $("#sContactoCliente").html("");
    $("#FechaPlaneacionVisita").val("");
    $("#sClientesNew").html("");
    $("#sContactoCliente").html("");
    $("#sContactoCliente").html("");
    $("#MotivoVisita").html("");
    $("#ObservacionPlanificacion").val("");
    $("#checkEnUbicacion").prop("checked", false);
    $("#FechaHora").val("");
    $("#ubicacion").val("");
    $("#ObservacionVisita").val("");

    //Enabling fields in header
    $('#EsVisitaPlanificada').removeAttr('disabled');
    $('#sNroPlanificacionNew').removeAttr('disabled');
    $('#FechaPlaneacionVisita').removeAttr('disabled');
    $('#sClientesNew').removeAttr('disabled');
    $('#sContactoCliente').removeAttr('disabled');
    $('#MotivoVisita').removeAttr('disabled');
    $('#ObservacionPlanificacion').removeAttr('disabled');
    $('#sDepartamento').removeAttr('disabled');
    $('#sProvincia').removeAttr('disabled');
    $('#sDistrito').removeAttr('disabled');


    //Carga de los campos como si fueran nuevos
    onReadyPreparation();
    //clientesActivosListar_New();
    //$('.selectpicker').selectpicker({
    //    size: 5,
    //    title: 'Seleccione una opción'
    //});

    //LlenarSelectNroPlanificacion();
    //LlenarSelectMotivoVisita();


    $('#EsVisitaPlanificada').append($('<option>', {
        value: 0,
        text: 'No'
    }));
    $('#EsVisitaPlanificada').append($('<option>', {
        value: 1,
        text: 'Si'
    }));

}


$('#newVisitaModel').on('hidden.bs.modal', function () {
    LimpiarFormularioNuevaVisita();

    gs_visitaClienteID = null;
})


Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}
// usage
//=> 3..padLeft() => '03'
//=> 3..padLeft(100,'-') => '--3' 


$("#checkEnUbicacion").click(function () {

    if ($('#checkEnUbicacion').is(":checked")) {
        //Setea la fecha
        //var now = new Date();
        //now = now.toLocaleString()
        //$("#FechaHora").val(now);

        var d = new Date,
            dformat = [d.getDate().padLeft(),
                      (d.getMonth() + 1).padLeft(),
                       d.getFullYear()].join('/') + ' ' +
                      [d.getHours().padLeft(),
                       d.getMinutes().padLeft(),
                       d.getSeconds().padLeft()].join(':');
        //=> dformat => '05/17/2012 10:52:21'
        $("#FechaHora").val(dformat);







        //Setea el ubigeo
        showError = function (error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    //alert("No tiene permiso para obtener la ubicación.");
                    //break;
                case error.POSITION_UNAVAILABLE:
                    //alert("La información de la localización no esta disponible.");
                    //break;
                case error.TIMEOUT:
                    //alert("El tiempo de espera para obtener la información fue demsiado.");
                    //break;
                case error.UNKNOWN_ERROR:
                    //alert("Ocurrio un error desconocido.");
                    //break;
            }
        }

        getGeolocation = function () {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(coordenadas, showError);
            } else {
                alert('Este navegador es algo antiguo, actualiza para usar el API de localización');
            }
        }
        coordenadas = function (position) {
            document.getElementById("ubicacion").value = position.coords.latitude + ";" + position.coords.longitude;
        }
        getGeolocation();
    } else {
        //Setea la fehc hora actual
        var now = new Date();
        $("#FechaHora").val("");

        //Cambia la descripcion del ubigeo
        $("#ubicacion").val("No considerado");
    }




    //OLD

    //if ($('#checkEnUbicacion').is(":checked")) {
    //    //Setea la fehc hora actual
    //    var now = new Date();
    //    now = now.toLocaleString()
    //    $("#FechaHora").val(now);

    //    //Cambia la descripcion del ubigeo
    //    if (navigator.geolocation) {
    //        navigator.geolocation.getCurrentPosition(showPosition);
    //    } else {
    //        toastr.error("Geolocalización no es soportada por el navegador.");
    //    }

    //}
    //else {

    //    //Setea la fehc hora actual
    //    var now = new Date();
    //    $("#FechaHora").val("");

    //    //Cambia la descripcion del ubigeo
    //    $("#ubicacion").val("No considerado");
    //}
});


//function showPosition(position) {
//    gs_latitud = position.coords.latitude;
//    gs_longuitud = position.coords.longitude;
//    $("#ubicacion").val(gs_latitud + " ; " + gs_longuitud);
//}




//UBIGEO

var sDepartamento = $("#sDepartamento");
var sProvincia = $("#sProvincia");
var sDistrito = $("#sDistrito");


//Departamento
function LlenarSelectDepartamento() {
    sDepartamento.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaDepartamentos,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sDepartamento.append("<option value='" + result[i].cc_dpto + "'>" + result[i].cd_dpto + "</option>");
                }
                //Autoselecciona Lima
                $('#sDepartamento option[value="15"]').attr("selected", "selected");

                //On Change
                document.getElementById("sDepartamento").setAttribute("onchange", "LlenarSelectProvincia();");


                LlenarSelectProvincia();
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

//Provincia

function LlenarSelectProvincia() {
    sProvincia.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaProvincia,
        data: JSON.stringify({
            departamentoId: $("#sDepartamento option:selected").val(),
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sProvincia.append("<option value='" + result[i].cc_prov + "'>" + result[i].cd_prov + "</option>");
                }
                //Autoselecciona Lima
                $('#sProvincia option[value="01"]').attr("selected", "selected");

                //On Change
                document.getElementById("sProvincia").setAttribute("onchange", "LlenarSelectDistrito();");

                LlenarSelectDistrito();
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



//Distrito

function LlenarSelectDistrito() {
    sDistrito.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaDistrito,
        data: JSON.stringify({
            departamentoId: $("#sDepartamento option:selected").val(),
            provinciaId: $("#sProvincia option:selected").val(),
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                //Agregar Todos Option
                sDistrito.append("<option value='%'>Todos</option>");
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sDistrito.append("<option value='" + result[i].cc_distrito + "'>" + result[i].cd_distrito + "</option>");
                }
                //Autoselecciona Lima
                $('#sDistrito option[value="%"]').attr("selected", "selected");

                //On Change
                document.getElementById("sDistrito").setAttribute("onchange", "clientesActivosListar_New();");

                //Carga Clientes
                clientesActivosListar_New();
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}











////////////////////////EDITAR PLANIFICACION DE VISITA DE CLIENTES



////UBIGEO
//Departamento
function LlenarSelectDepartamento_EDITAR(departamentoId, provinciaId, distritoId, clienteId, contactoID) {
    sDepartamento.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaDepartamentos,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sDepartamento.append("<option value='" + result[i].cc_dpto + "'>" + result[i].cd_dpto + "</option>");
                }

                $('#sDepartamento option[value="' + departamentoId + '"]').attr("selected", "selected");


                //On Change
                //document.getElementById("sDepartamento").setAttribute("onchange", "LlenarSelectProvincia_EDITAR();");


                //LlenarSelectProvincia();
                LlenarSelectProvincia_EDITAR(departamentoId, provinciaId, distritoId, clienteId, contactoID);
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

//Provincia

function LlenarSelectProvincia_EDITAR(departamentoId, provinciaId, distritoId, clienteId, contactoID) {
    sProvincia.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaProvincia,
        data: JSON.stringify({
            departamentoId: departamentoId,//$("#sDepartamento option:selected").val(),
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sProvincia.append("<option value='" + result[i].cc_prov + "'>" + result[i].cd_prov + "</option>");
                }

                $('#sProvincia option[value="' + provinciaId + '"]').attr("selected", "selected");
                //On Change
                //document.getElementById("sProvincia").setAttribute("onchange", "LlenarSelectDistrito_EDITAR();");

                //LlenarSelectDistrito();
                LlenarSelectDistrito_EDITAR(departamentoId, provinciaId, distritoId, clienteId, contactoID);

            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



//Distrito

function LlenarSelectDistrito_EDITAR(departamentoId, provinciaId, distritoId, clienteId, contactoID) {
    sDistrito.html("");//limpiar todas las filas o rows

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaDistrito,
        data: JSON.stringify({
            departamentoId: departamentoId,
            provinciaId: provinciaId,
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                //Agregar Todos Option
                sDistrito.append("<option value='%'>Todos</option>");
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sDistrito.append("<option value='" + result[i].cc_distrito + "'>" + result[i].cd_distrito + "</option>");
                }

                $('#sDistrito option[value="' + distritoId + '"]').attr("selected", "selected");
                //On Change
                //document.getElementById("sDistrito").setAttribute("onchange", "clientesActivosListar_New();");

                //Carga Clientes
                clientesActivosListar_Editar(departamentoId, provinciaId, distritoId, clienteId, contactoID);
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}
///UBIGEO

function clientesActivosListar_Editar(depId, provId, disId, clienteId, contactoID) {
    sClientesNew.html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlClienteAsignado,
        data: JSON.stringify({
            departamentoId: depId,
            provinciaId: provId,
            distritoId: disId,
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                sClientesNew.append("<option value='" + result[i].ruc + "'>" + result[i].razonSocial + "</option>");
            }
            //Al reselecionar
            document.getElementById("sClientesNew").setAttribute("onchange", "LlenarSelectContactoCliente();");
            $('#sClientesNew').selectpicker({
                maxOptions: 2
            });
            $('#sClientesNew').selectpicker('refresh');

            //Marcar segun query
            $('select[name=sClientesNew]').val(clienteId);
            $('.selectpicker').selectpicker('refresh')




            ///Reseleccionar contacto
            $("#sContactoCliente").html("");//limpiar todas las filas o rows
            $.ajax({
                destroy: true,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlListaContactoCliente,
                data: JSON.stringify({
                    clienteID: $("#sClientesNew option:selected").val(),
                }),
                dataType: "json",
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        $("#sContactoCliente").append("<option value='" + result[i].contactoID + "'>" + result[i].contacto + "</option>");
                    }

                    $('#sContactoCliente option[value="' + contactoID + '"]').attr("selected", "selected");

                    //Disabling fields in header
                    $('#EsVisitaPlanificada').attr('disabled', 'disabled');
                    $('#sNroPlanificacionNew').attr('disabled', 'disabled');
                    $('#FechaPlaneacionVisita').attr('disabled', 'disabled');
                    $('#sClientesNew').attr('disabled', 'disabled');
                    $('#sContactoCliente').attr('disabled', 'disabled');
                    $('#MotivoVisita').attr('disabled', 'disabled');
                    $('#ObservacionPlanificacion').attr('disabled', 'disabled');
                    $('#sDepartamento').attr('disabled', 'disabled');
                    $('#sProvincia').attr('disabled', 'disabled');
                    $('#sDistrito').attr('disabled', 'disabled');
                },
                error: function (result) {
                    alert("Error en javascript...");
                }
            });



        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



function recuperarEditarVisitaCliente(VisitaID) {

    cargarVariableAccion(2, VisitaID);

    $('#newVisitaModel').modal('toggle');

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlRecuperarVisitaCliente,
        data: JSON.stringify({
            visitaClienteID: VisitaID,
        }),
        dataType: "json",
        success: function (result) {

            LlenarSelectDepartamento_EDITAR();

            for (var i = 0; i < result.length; i++) {
                $("#EsVisitaPlanificada").val(result[i].esVisitaPlanificada);
                $('#sNroPlanificacionNew option[value="' + result[i].planificacionID + '"]').attr("selected", "selected");
                $("#FechaPlaneacionVisita").val(result[i].fechaVisitaDT);
                //$('#sClientesNew option[value="' + result[i].clienteID + '"]').attr("selected", "selected");

                LlenarSelectDepartamento_EDITAR(result[i].departamentoId, result[i].provinciaId, result[i].distritoId, result[i].clienteID);

                //$('#sDepartamento option[value="' + result[i].departamentoId+'"]').attr("selected", "selected");
                //$('#sProvincia option[value="' + result[i].provinciaId+'"]').attr("selected", "selected");
                //$('#sDistrito option[value="' + result[i].distritoId+'"]').attr("selected", "selected");


                //Antes reselecionaba aca


                //var ContactoID = result[i].contactoID;




                $('#MotivoVisita option[value="' + result[i].motivoVisitaID + '"]').attr("selected", "selected");

                $("#ObservacionPlanificacion").val(result[i].observacionPlanificacion);
                //$("#checkEnUbicacion").val(result[i].motivoID);
                $("#FechaHora").val(result[i].fechaReal);
                $("#ubicacion").val(result[i].ubicacionLatitud + ";" + result[i].ubicacionLongitud);
                $("#ObservacionVisita").val(result[i].observacionVisita);
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });

}




//Registrar nueva visita de clientes
function RegistrarNuevaVisitaCliente() {
    if ($('#checkEnUbicacion').is(":checked")) {
        var estaUbicado = 1;
    }
    else {
        var estaUbicado = 0;

    }

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlRegistrarVisitaCliente,
        data: JSON.stringify({
            planificacionID: $("#sNroPlanificacionNew option:selected").val(),
            clienteID: $("#sClientesNew option:selected").val(),
            contactoID: $("#sContactoCliente option:selected").val(),
            FechaVisita: $("#FechaPlaneacionVisita").val(),
            motivoVisitaID: $("#MotivoVisita option:selected").val(),
            observacionPlanificacion: $("#ObservacionPlanificacion").val(),
            esVisitaPlanificada: $("#EsVisitaPlanificada option:selected").val(),
            fechaReal: $("#FechaHora").val(),
            observacionVisita: $("#ObservacionVisita").val(),
            ubicacionLatitud: gs_latitud,
            ubicacionLongitud: gs_longuitud,
            esUbicado: estaUbicado
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                if (result[i].mensajeID == 1) {
                    //Mostrar mensaje
                    toastr.success(result[i].mensaje);
                    //Cerar Modal
                    $('#newVisitaModel').modal('toggle');
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

function accionBotonFormularioVisitaClientes() {
    validarCamposObligatorios();



}

function validarCamposObligatorios(valid) {
    var fechaPlan = $("#FechaPlaneacionVisita").val();
    var cliente = $("#sClientesNew option:selected").val();
    var contacto = $("#sContactoCliente option:selected").val();

    var incompleto = 0;

    if (fechaPlan == "") {
        incompleto = 1;
        $("#FechaPlaneacionVisita").attr("style", "border: 1px solid red;")
    }
    else {
        $("#FechaPlaneacionVisita").attr("style", "border: default;")
    }

    if (cliente == "") {
        incompleto = 1;
        $("#sClientesNew").css("border-color", "red");
    }
    else {
        $("#sClientesNew").css("border-color", "default");
    }

    if (contacto == "") {
        incompleto = 1;
        $("#sContactoCliente").css("border-color", "red");
    } else {
        $("#sContactoCliente").css("border-color", "default");
    }


    if (incompleto == 1) {
        toastr.error("Debe completas los campos faltantes.");
    }
    else {
        if (gs_accion == 1) {
            RegistrarNuevaVisitaCliente();
        } else {
            EditarVisitaCliente();
        }
    }
}

//Editar nueva visita de clientes
function EditarVisitaCliente() {
    if ($('#checkEnUbicacion').is(":checked")) {
        var estaUbicado = 1;
    }
    else {
        var estaUbicado = 0;

    }


    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlEditarVisitaCliente,
        data: JSON.stringify({
            visitaClienteID: gs_visitaClienteID,
            planificacionID: $("#sNroPlanificacionNew option:selected").val(),
            clienteID: $("#sClientesNew option:selected").val(),
            contactoID: $("#sContactoCliente option:selected").val(),
            FechaVisita: $("#FechaPlaneacionVisita").val(),
            motivoVisitaID: $("#MotivoVisita option:selected").val(),
            observacionPlanificacion: $("#ObservacionPlanificacion").val(),
            esVisitaPlanificada: $("#EsVisitaPlanificada option:selected").val(),
            fechaReal: $("#FechaHora").val(),
            observacionVisita: $("#ObservacionVisita").val(),
            ubicacionLatitud: gs_latitud,
            ubicacionLongitud: gs_longuitud,
            esUbicado: estaUbicado
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                if (result[i].mensajeID == 1) {
                    //Mostrar mensaje
                    toastr.success(result[i].mensaje);
                    //Cerar Modal
                    $('#newVisitaModel').modal('toggle');
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



