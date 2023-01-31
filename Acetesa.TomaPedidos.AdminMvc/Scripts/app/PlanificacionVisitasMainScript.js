//Objects
var $bodyTableListadoVisitas = $("#TablePlanificacionVisitas > body")
var $btnBuscarVisitas = $("#btnBuscarVisitas");
var sClientes = $("#sClientes");
var $FechaInicio = $("#FechaInicio")
var $FechaFin = $("#FechaFin")

var gs_visitaClienteID;
var gs_accion;


//Todas las planificaciones
var sPlanificacionFilter = $("#sPlanificacionFilter");
function LlenarSelectPlanificacionFilter() {
    sPlanificacionFilter.html("");//limpiar todas las filas o rows
    
    //Opcion todos primero
    sPlanificacionFilter.append("<option value='%'>Todos</option>");
    $('#sPlanificacionFilter option[value="%"]').attr("selected", "selected");

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListaNumeroPlanificacionTodos,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    //Resto de opciones
                    sPlanificacionFilter.append("<option value='" + result[i].planificacionID + "'>" + result[i].descripcion + "</option>");
                }
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



function buscarVisitasMain() {

    $("#TablePlanificacionVisitas_BODY").html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListadoVisitas,
        data: JSON.stringify({
            clienteID: $("#sClientes option:selected").val(),
            fechaInicio: $FechaInicio.val(),
            fechaFin: $FechaFin.val(),
            estadoVisita: $("#sEstadoVisita option:selected").val(),
            estadoVisita: $("#sEstadoVisita option:selected").val(),
            planificacionID: $("#sPlanificacionFilter option:selected").val()
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                var EstadoID;
                var HTML_celdaEstados;
                if (result[i].estado == "Registrado") {
                    EstadoID = 1;
                    HTML_celdaEstados = "<td style='text-align:left'><span class='badge badge-info'>" + result[i].estado+"</span></td>";
                }
                else if (result[i].estado == "Cancelado") {
                    EstadoID = 0;
                    HTML_celdaEstados = "<td style='text-align:left'><span class='badge badge-error'>" + result[i].estado+"</span></td>";
                }
                else if (result[i].estado == "Realizado") {
                    EstadoID = 2;
                    HTML_celdaEstados = "<td style='text-align:left'><span class='badge badge-success'>" + result[i].estado+"</span></td>";
                }

                $("#TablePlanificacionVisitas_BODY").append("<tr>" +
                    +"<td style='display:none; text-align:left'>" + result[i].planificacionID + "</td>"
                    + "<td style='display:none; text-align:left'>" + result[i].visitaClienteID + "</td>"
                    + "<td style='text-align:left'>" + result[i].descripcion + "</td>"
                    + "<td style='text-align:left'>" + result[i].cliente + "</td>"
                    + "<td style='text-align:left'>" + result[i].fechaVisita + "</td>"
                   // + "<td style='text-align:left'>" + result[i].estado + "</td>" +
                    + HTML_celdaEstados +
                    "<td style='text-align:center'><button type='button' class='btn btn-outline-warning' onclick='recuperarEditarVisitaCliente(" + result[i].visitaClienteID + ")'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Editar</button>" +
                    "<button type='button' class='btn btn-outline-danger ms-2' onclick='Set_GSVisitaClienteID(" + result[i].visitaClienteID + "," + EstadoID + ")'><i class='fa fa-times' aria-hidden='true'></i> Anular</button>" +
                    "</tr>");
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function Set_GSVisitaClienteID(visitaClienteID, Estado) {
    if (Estado == 1) {
        gs_visitaClienteID = visitaClienteID;
        $('#cancelVisitaModel').modal('toggle');
    } else {
        toastr.error("No se puede Cancelar una Visita que está en Estado Realizado o Cancelado");
        return;
    }

}

function clientesActivosListar() {
    sClientes.html("");//limpiar todas las filas o rows
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlClienteAsignado,
        data: JSON.stringify({
            departamentoId: "%",
            provinciaId: "%",
            distritoId: "%"
        }),
        dataType: "json",
        success: function (result) {
            sClientes.append("<option value=''>" + "TODOS LOS CLIENTES" + "</option>");
            for (var i = 0; i < result.length; i++) {
                sClientes.append("<option value='" + result[i].ruc + "'>" + result[i].razonSocial + "</option>");
            }
            //document.getElementById("sClientes").setAttribute("onchange", "buscarCliente();");
            $('#sClientes').selectpicker({
                maxOptions: 2
            });
            $('#sClientes').selectpicker('refresh');
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


function setDates() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    var firstDay = new Date(y, m, 1);

    var day = firstDay.getDate(),
        month = firstDay.getMonth() + 1,
        year = firstDay.getFullYear();

    month = (month < 10 ? "0" : "") + month;
    day = (day < 10 ? "0" : "") + day;

    firstDay = year + "-" + month + "-" + day;

    document.getElementById('FechaInicio').value = firstDay;


    var lastDay = new Date(y, m + 1, 0);

    var day = lastDay.getDate(),
        month = lastDay.getMonth() + 1,
        year = lastDay.getFullYear();

    month = (month < 10 ? "0" : "") + month;
    day = (day < 10 ? "0" : "") + day;

    lastDay = year + "-" + month + "-" + day;

    document.getElementById('FechaFin').value = lastDay;
    //alert(firstDay);

}


$(document).ready(function () {

    LlenarSelectPlanificacionFilter();

    setDates();

    clientesActivosListar();

    $('.selectpicker').selectpicker({
        size: 5,
        title: 'Seleccione una opción'
    });

    //buscarVisitasMain();


});


//document.addEventListener("DOMContentLoaded", buscarVisitasMain);


function cargarVariableAccion(x, VisitaID) {
    gs_accion = x;
    gs_visitaClienteID = VisitaID;

}


