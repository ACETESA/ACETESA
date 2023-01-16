var dtTabla = null;
var msFechaInicio, msFechaFin;
var sEmpresa;
function blockScreen() {
    $.blockUI({ message: null, overlayCSS: { backgroundColor: '#000', opacity: 0.1, zIndex: 99999 } });
}

function unBlockScreen() {
    $.unblockUI();
}

function scrollToScreen(idLabel, delay) {
    var heightMenuHeader = $(".navbar-header").height();
    $("html, body").animate({ scrollTop: $(idLabel).offset().top - heightMenuHeader }, delay);
}

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
    scrollToScreen("#result", 1000);
    unBlockScreen();
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
        changeYear: true
    });
    $("#FechaInicio,#FechaFinal").mask("99/99/9999", { placeholder: "dd/mm/yyyy" });

    jQuery("#btnEnviar").on("click", function (e) {
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
        blockScreen();
        msFechaInicio = $("#FechaInicio").val();
        msFechaFin = $("#FechaFinal").val();
        sEmpresa = $("#Empresa").val();
        CrearTabla();
        unBlockScreen();
        return false;
    });
})(jQuery);

function CrearTabla() {
    var bCargado = false;
    if (jQuery.fn.dataTable.isDataTable("#tblListado")) {
        if (jQuery.fn.dataTable.isDataTable("#tblListado")) {
            jQuery("#tblListado").DataTable().destroy();
            bCargado = true;
        }
    }
    dtTabla = jQuery("#tblListado").DataTable(
    {
        ajax: {
            url: "getDatosReporte",
            type: "POST",
            data: {
                sFechaInicio: msFechaInicio,
                sFechaFinal: msFechaFin,
                sVendedor: '',
                sTienda: '',
                sEmpresa: sEmpresa
            },
            dataSrc: ""
        },
        columns:
            [
                    {
                      "className": 'details-control',
                      "orderable": false,
                      "data": null,
                      "defaultContent": ''
                  },
                  {
                      "title": "Vendedor",
                      "data": "nombre",
                      "searchable": true
                  },
                  {
                      "title": "TM",
                      "data": "vtaPeso",
                      "searchable": true,
                      "className": "Monto"
                  },
                  {
                      "title": "Venta",
                      "data": "vtaSoles",
                      "searchable": true,
                      "className": "Monto"
                  },
                  {
                      "title": "Costo",
                      "data": "ctoSoles",
                      "searchable": true,
                      "className": "Monto"
                  },
                  {
                      "title": "Marg.%",
                      "data": "margenPorc",
                      "searchable": true,
                      "className": "group-margen"
                },
                {
                    "title": "Tienda",
                    "data": "tienda",
                    "searchable": true
                }
                ],
            paging: false,
            info: false,
        iDisplayLength: 100,
        bLengthChange: false,
        bAutoWidth: false,
        columnDefs: [{
                "visible": false,
                "targets": 6
            }],
        destroy: true,
        processing: true,
        scrollX: true,
            order: [6, 'asc'],
            orderFixed: [6, 'asc'],
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No hay ítems para mostrar",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": "Activar para ordenar la columna de manera ascendente",
                "sSortDescending": "Activar para ordenar la columna de manera descendente"
            },
        },
        "fnFooterCallback": function (nRow, aasData, iStart, iEnd, aiDisplay) {
            var api = this.api(), data;
            var intVal = function (i) {
                var bRetoro = 0;

                if ($.isNumeric(i)) {
                    bRetoro = i;
                } else {
                    i = i.replace(",", ".");
                    if ($.trim(i) == "") {
                        bRetoro = 0;
                    } else {
                        bRetoro = parseFloat(i);
                    }
                }
                return bRetoro;
            };
            var columnas = [2, 3, 4];
            var totalCosto =0;
            var totalVenta=0;
            jQuery(columnas).each(function () {
                var j = this;
                var total = api
                    .column(j)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                $(api.column(j).footer()).html(total.toFixed(2));
                if(j == 3)
                {
                    totalVenta = total
                }
                if (j == 4) {
                    totalCosto = total
                }
            });
            $(api.column(0).footer()).html("Total:");
            $(api.column(5).footer()).html(Math.round(((totalVenta - totalCosto) / totalVenta)*100).toFixed(0));
            },
            "drawCallback": function (settings) {
                var COL_TM = 2;
                var COL_VENTA = 3;
                var COL_COSTO = 4;
                var COL_MARGEN = 5;
                var COL_TIENDA = 6;
            var api = this.api();
            var rows = api.rows({
                page: 'all'
            }).nodes();
            var last = null;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

                total_TM = [];
                total_VENTA = [];
                total_COSTO = [];
                api.column(COL_TIENDA, {
                page: 'all'
            }).data().each(function (group, i) {
                group_assoc = group.replace(' ', "_");
                console.log(group_assoc);
                if (typeof total_TM[group_assoc] != 'undefined') {
                    total_TM[group_assoc] = total_TM[group_assoc] + intVal(api.column(COL_TM).data()[i]);
                    total_VENTA[group_assoc] = total_VENTA[group_assoc] + intVal(api.column(COL_VENTA).data()[i]);
                    total_COSTO[group_assoc] = total_COSTO[group_assoc] + intVal(api.column(COL_COSTO).data()[i]);
                } else {
                    total_TM[group_assoc] = intVal(api.column(COL_TM).data()[i]);
                    total_VENTA[group_assoc] = intVal(api.column(COL_VENTA).data()[i]);
                    total_COSTO[group_assoc] = intVal(api.column(COL_COSTO).data()[i]);
                }
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td></td><td>Tienda: ' + group + '</td><td class="' + group_assoc + '_TM group-number"><td class="' + group_assoc + '_VENTA group-number"><td class="' + group_assoc + '_COSTO group-number"><td class="' + group_assoc + '_MARGEN group-margen"></td></tr>'
                    );
                    last = group;
                }
            });
                for (var key in total_TM) {
                    $("." + key + "_TM").html(total_TM[key].toFixed(2));
                    $("." + key + "_VENTA").html(total_VENTA[key].toFixed(2));
                    $("." + key + "_COSTO").html(total_COSTO[key].toFixed(2));
                    var margen = Math.round(((parseFloat(total_VENTA[key]) - parseFloat(total_COSTO[key])) / parseFloat(total_VENTA[key]) * 100)).toFixed(0);
                    $("." + key + "_MARGEN").html(margen);
            }
        }
    });
    if (!bCargado) {
        jQuery('#tblListado tbody').on('click', 'td.details-control', function () {
            var tr = jQuery(this).closest('tr');
            var row = dtTabla.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {

                row.child(format(row.data())).show();
                tr.addClass('shown');
            }
        });
    }

    function format(d) {
        var sRetorno = "";
        jQuery.ajax({
            url: "getDatosReporte",
            type: "POST",
            async: false,
            data: {
                sFechaInicio: msFechaInicio,
                sFechaFinal: msFechaFin,
                sVendedor: d.id,
                sEmpresa: sEmpresa,
                sTienda: d.tienda_ID
            },
            success: function (data) {
                var Titulos = [{ titulo: "Razón Social", campo: "nombre" }, { titulo: "TM", campo: "vtaPeso" }, { titulo: "Venta", campo: "vtaSoles" }, { titulo: "Costo", campo: "ctoSoles" }, { titulo: "Marg.%", campo: "margenPorc" }];
                var oTabla = crearTablaDetalle(data, Titulos);
                sRetorno = oTabla[0].outerHTML;
            },
            cache: false,
            dataType: 'json'
        });

        return sRetorno;
    }
    function crearTablaDetalle(lValores, lColumnas) {
        var oTr = jQuery("<tr>");
        var oTHead = jQuery("<thead class='thead-inverse'>");
        var oTBody = jQuery("<tbody>");
        jQuery(lColumnas).each(function () {
            oTr.append(jQuery("<th>").text(this.titulo));
        });
        oTHead.append(oTr);
        jQuery(lValores).each(function (iIndice) {
            oTr = jQuery("<tr>").addClass(iIndice % 2 == 0 ? "odd" : "even");
            for (var i = 0; i < lColumnas.length; i++) {
                var Celta = jQuery("<td" + (i == 0 ? "" : " class='Monto'") + ">");
                Celta[0].innerHTML = this[lColumnas[i].campo];
                oTr.append(Celta)
            }
            oTBody.append(oTr);
        });

        var oTabla = jQuery("<table>")
        .addClass("table table-hover table-bordered")
        .append(oTHead)
        .append(oTBody);
        return oTabla;
    }
}