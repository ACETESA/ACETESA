//function blockScreen() {
//    $.blockUI({ message: null, overlayCSS: { backgroundColor: "#000", opacity: 0.1, zIndex: 99999 } });
//}

//function unBlockScreen() {
//    $.unblockUI();
//}
function scrollToScreen(idLabel, delay) {
    var heightMenuHeader = $(".navbar-header").height();
    $("html, body").animate({ scrollTop: $(idLabel).offset().top - heightMenuHeader }, delay);
}

var _baseUrl = "/";
var _tipoFormulario = "";

$("input#btnRegresar").on("click", function() {
    history.back();
});
$("li#btnRegresar").on("click", function () {
    history.back();
});

DatosConfigTabla = function(){
    return {
        language: {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No hay items para mostrar",
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
                "sNext": " Siguiente",
                "sPrevious": " Anterior"
            },
            "oAria": {
                "sSortAscending": "Activar para ordenar la columna de manera ascendente",
                "sSortDescending": "Activar para ordenar la columna de manera descendente"
            },
            "Aceptar": "Aceptar",
            "Cancelar": "Cancelar"
        }
    }
}
function DestruirDataTabla(id) {
    if ($.fn.dataTable.isDataTable(id)) {
        $(id).DataTable().destroy();
        $(id).empty();
    }
}
function getConfiguracionDataTable(lColumnas, datos) {
    oConfig = {
        columnDefs: procesarColumnasRenderizablesDataTable(lColumnas),
        columns: lColumnas,
        language: DatosConfigTabla().language,
        destroy: true,
        processing: true,
        order: [],
        bAutoWidth: false,
        bLengthChange: false,
        paging: false,
        info: false
    };

    if (datos != null) {
        oConfig.data = datos;
    };

    return oConfig;
};

function renderizarHtml(data) {
    var sAttr = '',
        sCierreTag = "/>";
    for (var sParam in data) {
        if (sParam == "innerText") {
            sCierreTag = ">" + data[sParam] + "</" + data.tag + ">";
        } else if (sParam != "tag" && data[sParam]) {
            sAttr += " " + sParam + "=\"" + data[sParam] + "\"";
        } else if (sParam != "tag" && data[sParam] != undefined) {
            sAttr += " " + sParam + " ";
        }
    }
    return "<" + data.tag + sAttr + sCierreTag;
};

function procesarColumnasRenderizablesDataTable(lColumnas) {
    var lColumnDefs = [];
    $(lColumnas).each(function (iIndice) {
        if (this.renderizable) {
            lColumnDefs[lColumnDefs.length] = {
                targets: iIndice,
                searchable: false,
                render: function (data, type, full, meta) {
                    var html = "";
                    if (data) {
                        if ($.isArray(data)) {
                            for (var i = 0; i < data.length; i++) {
                                html += renderizarHtml(data[i]);
                            }
                        } else {
                            html = renderizarHtml(data);
                        }
                    }
                    return html;
                }
            }
        }
    });
    return lColumnDefs;
};

EsFecha = function (sFecha) {
    var EsCorrecto = true;
    try {
        $.datepicker.parseDate('dd/mm/yy', sFecha);
    } catch (e) {
        EsCorrecto = false;
    }
    return EsCorrecto;
}

ComparaFechas = function (startDate, endDate) {
    startDate = $.datepicker.parseDate('dd/mm/yy', startDate);
    endDate = $.datepicker.parseDate('dd/mm/yy', endDate);

    var difference = (endDate - startDate) / (86400000);
    
    if (difference < 0) {
        return false;
    }
    return true;

}
ExportarExcel = function (tabla) {
    var oTabla = jQuery("<table>");
    oTabla.append($('#' + tabla).clone());

    oTabla.find(".excluir-excel").remove();
    oTabla.find("a").remove();

    var sTablaHTML = "<table>" + oTabla.html() + "</table>";
    sTablaHTML = sTablaHTML.replace(/[áéíóúñÁÉÍÓÚÑ¡¿]/g,
        function (a) {
            return '&#' + a.charCodeAt(0) + ';';
        });
    sTablaHTML = CabeceraExportExcel("") + sTablaHTML;
    sTablaHTML += "</body></html>";
    if (navigator.userAgent.indexOf("MSIE") > 0 || navigator.userAgent.indexOf("rv:11.0") > 0) {
        var oWin = window.open('about:blank', "_blank");
        oWin.document.write(sTablaHTML);
        oWin.document.close();
        oWin.document.execCommand('SaveAs', false, document.title + ".xls");
        oWin.close();
    } else {
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent(sTablaHTML));
    }
}
function CabeceraExportExcel(titulo) {
    var style = "<html><head><style>";
    style += "table {font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;border-collapse: collapse;width: 100%;}";
    style += "table td, table th {border: 1px solid #ddd;padding: 8px;}";
    style += "table tr:nth-child(even){background-color: #f2f2f2;}";
    style += "table tr:hover {background-color: #ddd;}";
    style += "table th {padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #4CAF50;color: white;}";
    style += "/<style></head><body>";
    return style;
}

function TraerDatosSUNAT(css, SUNAT) {
    if (SUNAT === 1) {
        $("#VALIDA_RUC").val("S");
    }
    if ($("#VALIDA_RUC").val() != "S") {
        toastr.warning("Servicio Inactivo");
        //Regresando al valor predefinido en el webconfig
        $("#VALIDA_RUC").val(localStorage.getItem('LSI_HABILITAR_SUNAT'));
        return false;
    }
    if (css == null) {
        css = "";
    }
    if ($.trim($(".ruc-sunat" + css).val()) === "") {
        toastr.warning("Ingrese un número de RUC.");
        //Regresando al valor predefinido en el webconfig
        $("#VALIDA_RUC").val(localStorage.getItem('LSI_HABILITAR_SUNAT'));
        return false;
    }
    ValidaDatosSUNAT($(".ruc-sunat" + css).val(), function (empresa) {
        if (empresa != null) {
            if (empresa.error == null) {
                $(".razon-social-sunat" + css).val(empresa.razon_social);
                $(".domicilio-sunat" + css).val(empresa.domicilio_fiscal);
            }
        } else {
            toastr.warning("RUC no encontrado en SUNAT");
        }
    });
    //Regresando al valor predefinido en el webconfig
    $("#VALIDA_RUC").val(localStorage.getItem('LSI_HABILITAR_SUNAT'));
}

function ValidarSunat(ruc) {
    if ($("#VALIDA_RUC").val() != "S") {
        return true;
    }
    var activo = true;
    if (ruc.length >0 && ruc.substring(0, 1) != '0') {
        ValidaDatosSUNAT(ruc, function (empresa) {
            if (empresa == null) {
                activo = false;
            } else {
                if (empresa.error != null) {
                    activo = true;
                } else {
                    activo = (empresa.contribuyente_estado == "ACTIVO");
                }
            }
        }, false);
    }
    if (!activo) {
        toastr.warning("El cliente no se encuentra activo en SUNAT");
        return false;
    } else {
        return true;
    }
}

function ValidaDatosSUNAT(RUC, fnc_callback, async) {
    var url = _baseUrl + "Pedido/verificaRUC";
    var request = new Ajax();
    var params = {
        ruc: RUC
    };
    request.JsonPost(url, params, function (response) {
        var retorno = null;
        try {
            retorno = JSON.parse(response);
        } catch (e) {
            retorno = null;
        }
        if (retorno != null && retorno.error) {
            toastr.warning(retorno.error);
        }
        fnc_callback(retorno);
    }, null, async).fail(function (data) {
        OnFailure(data);
        });
}

//Cuando inicia una solicitud Ajax Bloquea la pantalla
//$(document).ajaxSend(function (event, request, settings) {
//    blockScreen();
//});

//Desbloquea la pantalla cuando la solicitud Ajax terminó
//$(document).ajaxComplete(function (event, request, settings) {
//    unBlockScreen();
//});

$(document).ajaxStart(function () {
    blockScreen();
});

$(document).ajaxStop(function () {
    unBlockScreen();
});


//Scroll to Element
function ScrollToElement(element, time) {
    $([document.documentElement, document.body]).animate({
        scrollTop: $(element).offset().top - 100
    }, time);
}

function blockScreen() {
    if ($('#Loading').length > 0) {
        $('#Loading').modal('show');
    }
}

function unBlockScreen() {
    setTimeout(
        function () {
            document.getElementById("btnCloseModalLoading").click();
        }, 1000);
}
var sRutaRaiz;
function getRutaAbsoluta(sPath) {
    if (!sRutaRaiz) {
        sRutaRaiz = $("#VIRTUAL_PATH").val();
    }
    if (sPath[0] != "/") {
        sPath = "/" + sPath;
    }
    if (sPath.indexOf(sRutaRaiz) == -1) {
        sPath = (sRutaRaiz[sRutaRaiz.length - 1] === "/" ? sRutaRaiz.substring(0, sRutaRaiz.length - 1) : sRutaRaiz) + sPath;
    }
    return sPath;
};