function LlenarSelectTipoDocumento(tipoDocumentoID) {
    $sTipoDocumento = $("#sTipoDocumento");
    $sTipoDocumento
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectTipoDocumentoIdentidad,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var TipoDocumentoID = value["tipoDocumentoID"];
                var TipoDocumento = value["tipoDocumento"];

                $sTipoDocumento.append($("<option />").val(TipoDocumentoID).text(TipoDocumento));
            });
            //Selecciona Tipo Documento
            $('#sTipoDocumento').val(tipoDocumentoID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


function LlenarSelectProcedencias(procedenciaID) {
    $sProcedencias = $("#sProcedencias");
    $sProcedencias
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectProcedencias,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var idProcedencia = value["idProcedencia"];
                var procedencia = value["procedencia"];

                $sProcedencias.append($("<option />").val(idProcedencia).text(procedencia));
            });

            //Selecciona Procededencia
            $('#sProcedencias').val(procedenciaID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


function LlenarSelectUbigeo(departamentoID, provinciaID, distritoID, zonaID) {
    $sDepartamento = $("#sDepartamento");
    $sDepartamento
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectDepartamento,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                //Carga departamento
                var cc_dpto = value["cc_dpto"];
                var cd_dpto = value["cd_dpto"];

                $sDepartamento.append($("<option />").val(cc_dpto).text(cd_dpto));
            });
            //Selecciona departamento
            $('#sDepartamento').val(departamentoID);

            //Carga Provincia
            $sProvincia = $("#sProvincia");
            $sProvincia
                .find('option')
                .remove()
                .end();

            $.ajax({
                destroy: true,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: urlSelectProvincia,
                data: JSON.stringify({
                    departamentoID: $("#sDepartamento").val()
                }),
                dataType: "json",
                success: function (result) {
                    $.each(result, function (index, value) {
                        var cc_prov = value["cc_prov"];
                        var cd_prov = value["cd_prov"];

                        $sProvincia.append($("<option />").val(cc_prov).text(cd_prov));
                    });
                    //Selecciona Provincia
                    $('#sProvincia').val(provinciaID);

                    //Carga Distrito
                    $sDistrito = $("#sDistrito");
                    $sDistrito
                        .find('option')
                        .remove()
                        .end();

                    $.ajax({
                        destroy: true,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: urlSelectDistrito,
                        data: JSON.stringify({
                            departamentoID: $("#sDepartamento").val(),
                            provinciaID: $("#sProvincia").val()
                        }),
                        dataType: "json",
                        success: function (result) {
                            $.each(result, function (index, value) {
                                var cc_distrito = value["cc_distrito"];
                                var cd_distrito = value["cd_distrito"];

                                $sDistrito.append($("<option />").val(cc_distrito).text(cd_distrito));
                            });
                            //Selecciona Distrito
                            $('#sDistrito').val(distritoID);

                            //Carga Zonas
                            $sZona = $("#sZona");
                            $sZona
                                .find('option')
                                .remove()
                                .end();

                            $.ajax({
                                destroy: true,
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: urlSelectZonas,
                                data: JSON.stringify({
                                    cc_distrito: $("#sDistrito").val(),
                                    cc_prov: $("#sProvincia").val(),
                                    cc_dpto: $("#sDepartamento").val()

                                }),
                                dataType: "json",
                                success: function (result) {
                                    $.each(result, function (index, value) {
                                        var cc_zona = value["cc_zona"];
                                        var cd_zona = value["cd_zona"];

                                        $sZona.append($("<option />").val(cc_zona).text(cd_zona));
                                    });

                                    if (zonaID != null) {
                                        ////Selecciona Zona
                                        $('#sZona').val(zonaID);
                                    }


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

function LlenarSelectPaises(PaisID) {
    $sPais = $("#sPais");
    $sPais
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectPais,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var cc_pais = value["cc_pais"];
                var cd_pais = value["cd_pais"];

                $sPais.append($("<option />").val(cc_pais).text(cd_pais));
            });
            //Selecciona Pais
            $('#sPais').val(PaisID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


function LlenarSelectSector(SectorID) {
    $sSector = $("#sSector");
    $sSector
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectSector,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var cc_sector = value["cc_sector"];
                var cd_sector = value["cd_sector"];

                $sSector.append($("<option />").val(cc_sector).text(cd_sector));
            });
            //Selecciona Sector
            $('#sSector').val(SectorID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


function LlenarSelectCategoria(CategoriaID) {
    $sCategoria = $("#sCategoria");
    $sCategoria
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectCategorias,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var cc_catclie = value["cc_catclie"];
                var cd_catclie = value["cd_catclie"];

                $sCategoria.append($("<option />").val(cc_catclie).text(cd_catclie));
            });
            //Selecciona Categoria
            $('#sCategoria').val(CategoriaID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function LlenarSelectEstado(EstadoID) {
    $sEstado = $("#sEstado");
    $sEstado
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectEstadosCliente,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var estadoID = value["estadoID"];
                var estado = value["estado"];

                $sEstado.append($("<option />").val(estadoID).text(estado));
            });
            //Selecciona Estado
            $('#sEstado').val(EstadoID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function LlenarSelectMonedaFacturacion(MonedaFactID) {
    $sMonedaFacturacion = $("#sMonedaFacturacion");
    $sMonedaFacturacion
        .find('option')
        .remove()
        .end();

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectMonedaFacturacion,
        data: JSON.stringify({}),
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                var cb_monfac = value["cb_monfac"];
                var cd_monfac = value["cd_monfac"];
                $sMonedaFacturacion.append($("<option />").val(cb_monfac).text(cd_monfac));
            });
            //Selecciona Moneda Fact
            $sMonedaFacturacion.val(MonedaFactID);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}



//Campos del formulario
$iNumeroDocumento = $("#iNumeroDocumento");
$sPais = $("#sPais");
$sDepartamento = $("#sDepartamento");
$sProvincia = $("#sProvincia");
$sSector = $("#sSector");
$sCategoria = $("#sCategoria");
$sDistrito = $("#sDistrito");
$iRazonSocial = $("#iRazonSocial");
$sZona = $("#sZona");
$sProcedencias = $("#sProcedencias");
$iDirecccion = $("#iDirecccion");
$iGiroNegocio = $("#iGiroNegocio");
$iFechaConstitucion = $("#iFechaConstitucion");
$sMonedaFacturacion = $("#sMonedaFacturacion");
$cSucursal = $("#cSucursal");
$sEstado = $("#sEstado");
$iNombreComercial = $("#iNombreComercial");
$iApellidoPaterno = $("#iApellidoPaterno");
$iApellidoMaterno = $("#iApellidoMaterno");
$iNombre1 = $("#iNombre1");
$iNombre2 = $("#iNombre2");
$cAgentePercExon = $("#cAgentePercExon");
$sTipoDocumento = $("#sTipoDocumento");
$cVinculado = $("#cVinculado");
$cCorporativo = $("#cCorporativo");
$rTipoSector = $("input[name='rTipoSector']:checked");



function GuardarClienteNuevo() {
    //Value of fields in form
    $iNumeroDocumentoVal = $iNumeroDocumento.val();
    $sPaisVal = $sPais.val();
    $sDepartamentoVal = $sDepartamento.val();
    $sProvinciaVal = $sProvincia.val();
    $sSectorVal = $sSector.val();
    $sCategoriaVal = $sCategoria.val();
    $sDistritoVal = $sDistrito.val();
    $iRazonSocialVal = $iRazonSocial.val();
    $sZonaVal = $sZona.val();
    $sProcedenciasVal = $sProcedencias.val();
    $iDirecccionVal = $iDirecccion.val();
    $iGiroNegocioVal = $iGiroNegocio.val();
    $iFechaConstitucionVal = $iFechaConstitucion.val();
    $sMonedaFacturacionVal = $sMonedaFacturacion.val();
    $cSucursalVal = $cSucursal.is(':checked');
    $rTipoSectorVal = $rTipoSector.val();
    $sEstadoVal = $sEstado.val();
    $iNombreComercialVal = $iNombreComercial.val();
    $iApellidoPaternoVal = $iApellidoPaterno.val();
    $iApellidoMaternoVal = $iApellidoMaterno.val();
    $iNombre1Val = $iNombre1.val();
    $iNombre2Val = $iNombre2.val();
    $cAgentePercExonVal = $cAgentePercExon.is(':checked');
    $sTipoDocumentoVal = $sTipoDocumento.val();
    $cVinculadoVal = $cVinculado.is(':checked');
    $cCorporativoVal = $cCorporativo.is(':checked');

    //Ajax to Save
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlRegistrarCliente,
        data: JSON.stringify({
            cc_analis: $iNumeroDocumentoVal	   
           ,cc_pais: $sPaisVal
           ,cc_dpto: $sDepartamentoVal 
           ,cc_prov: $sProvinciaVal
           ,cc_sector: $sSectorVal
           ,cc_catclie: $sCategoriaVal
           ,cc_distrito: $sDistritoVal
           ,cd_razsoc: $iRazonSocialVal				   
           ,cc_zona: $sZonaVal
           ,cb_proced: $sProcedenciasVal
           ,cd_direc: $iDirecccionVal
           ,ct_giro: $iGiroNegocioVal
           ,dt_constit: $iFechaConstitucionVal   			   
           ,cb_monfac: $sMonedaFacturacionVal		   
           ,cb_sucursal: $cSucursalVal
           ,cb_sector: $rTipoSectorVal
           ,cb_activo: $sEstadoVal   		   
           ,cd_nomcom: $iNombreComercialVal			   
           ,cd_appaterno: $iApellidoPaternoVal
           ,cd_apmaterno: $iApellidoMaternoVal
           ,cd_nombre1: $iNombre1Val
           ,cd_nombre2: $iNombre2Val
           ,c_fl_agente_percepcion: $cAgentePercExonVal
           ,c_cod_documento_identidad: $sTipoDocumentoVal
           ,c_fl_vinculacion: $cVinculadoVal
           ,Corporativo: $cCorporativoVal
        }),
        dataType: "json",
        success: function (result) {
                var mensajeID = result["mensajeID"];
                var mensaje = result["mensaje"];

                if (mensajeID === "1") {
                    toastr.success("Registro de Cliente", mensaje);
                }
                else {
                    toastr.error("Registro de Cliente", mensaje);
                }

                LimpiarFormulario();
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function validarFormulario() {
    var passValidation = true;
    //Value of fields in form
    $iNumeroDocumentoVal = $iNumeroDocumento.val();
    $sPaisVal = $sPais.val();
    $sDepartamentoVal = $sDepartamento.val();
    $sProvinciaVal = $sProvincia.val();
    $sSectorVal = $sSector.val();
    $sCategoriaVal = $sCategoria.val();
    $sDistritoVal = $sDistrito.val();
    $iRazonSocialVal = $iRazonSocial.val();
    $sZonaVal = $sZona.val();
    $sProcedenciasVal = $sProcedencias.val();
    $iDirecccionVal = $iDirecccion.val();
    $iGiroNegocioVal = $iGiroNegocio.val();
    $iFechaConstitucionVal = $iFechaConstitucion.val();
    $sMonedaFacturacionVal = $sMonedaFacturacion.val();
    $cSucursalVal = $cSucursal.is(':checked');
    $rTipoSectorVal = $rTipoSector.val();
    $sEstadoVal = $sEstado.val();
    $iNombreComercialVal = $iNombreComercial.val();
    $iApellidoPaternoVal = $iApellidoPaterno.val();
    $iApellidoMaternoVal = $iApellidoMaterno.val();
    $iNombre1Val = $iNombre1.val();
    $iNombre2Val = $iNombre2.val();
    $cAgentePercExonVal = $cAgentePercExon.val();
    $sTipoDocumentoVal = $sTipoDocumento.val();
    $cVinculadoVal = $cVinculado.val();
    $cCorporativoVal = $cCorporativo.val();

    toastrTittle = "Registrar de Cliente";
    toastrMessage = "Debe completar los campos obligatorios.";
    if ($iNumeroDocumentoVal === "") {
        toastr.error(toastrMessage, toastrTittle);
        passValidation = false;
    } else if ($iGiroNegocioVal === "") {
        toastr.error(toastrMessage, toastrTittle);
        passValidation = false;
    } else if ($iDirecccionVal === "") {
        toastr.error(toastrMessage, toastrTittle);
        passValidation = false;
    } else if ($iRazonSocialVal === "") {
        toastr.error(toastrMessage, toastrTittle);
        passValidation = false;
    } else if ($iFechaConstitucionVal == '') {
        toastr.error(toastrMessage, toastrTittle);
        passValidation = false;
    }
    return passValidation;
}

function LimpiarFormulario() {
    //Selecciona Tipo Documento
    $sTipoDocumento.val('6');
    $("#divDatosPersonaNatural").hide();
    //Limpia Nro Documento
    $iNumeroDocumento.val("");
    //Limpia fecha constitucion
    $iFechaConstitucion.val("");
    //Selecciona Estado
    $sEstado.val('1');
    //Limpia Razon Social
    $iRazonSocial.val("");
    //Limpia Nombre Comercial
    $iNombreComercial.val("");
    //Limpia Apellido Paterno
    $iApellidoPaterno.val("");
    //Limpia Apellido Materno
    $iApellidoMaterno.val("");
    //Limpia Nombre 1
    $iNombre1.val("");
    //Limpia Nombre 2
    $iNombre2.val("");
    //Selecciona Procedencia
    $('#sProcedencias').val('N');
    //Selecciona Pais
    $('#sPais').val('01');
    //Selecciona departamento
    $('#sDepartamento').val('15');
    //Selecciona Provincia
    $('#sProvincia').val('01');
    //Selecciona Distrito
    $('#sDistrito').val('01');
    //Selecciona Zona
    $('#sZona').val('02');
    //Limpia Direccion
    $iDirecccion.val("");
    //Limpia Giro del Negocio
    $iGiroNegocio.val("");
    //Selecciona Categoria
    $sCategoria.val('A1');
    //Limpia Tipo de Sector
    $rTipoSector.val("0");
    //Selecciona Sector
    $sSector.val("00");
    //Selecciona Moneda de Facturacion
    $sMonedaFacturacion.val('E');
    //Limpia Sucrusal
    $cSucursal.prop("checked", false);
    //Limpia Vinculado
    $cVinculado.prop("checked", false);
    //Limpia Agente Percepcion Exoneracion
    $cAgentePercExon.prop("checked", false);
    //Limpia Cliente Corporativo
    $cCorporativo.prop("checked", false);


    }

$(document).ready(function () {
    $("#btnGuardarCliente").click(function () {

        if (validarFormulario()) {
            GuardarClienteNuevo();
        }

    });
});

function OpcionesAbrirNuevoModal() {
    $("#divDatosPersonaNatural").hide();

    LlenarSelectUbigeo("15", "01", "01", null); //Por defecto
    LlenarSelectTipoDocumento("6"); //Por defecto
    LlenarSelectProcedencias("N"); //Por defecto
    LlenarSelectPaises("01"); // Por defecto
    LlenarSelectSector("00"); // Por defecto
    LlenarSelectCategoria("A1"); //Por defecto
    LlenarSelectEstado("1"); //Por defecto
    LlenarSelectMonedaFacturacion("E"); //Por defecto


}

$sTipoDocumento.on('change', function () {
    if ($sTipoDocumento.val() != "6") {
        $("#divDatosPersonaNatural").show();
    }
    else {
        $("#divDatosPersonaNatural").hide();
    }
});


$sDepartamento.on('change', function () {
    LlenarSelectUbigeo($sDepartamento.val(), "01", "01", null);
});

$sProvincia.on('change', function () {
    LlenarSelectUbigeo($sDepartamento.val(), $sProvincia.val(), "01", null);
});

$sDistrito.on('change', function () {
    LlenarSelectUbigeo($sDepartamento.val(), $sProvincia.val(), $sDistrito.val(), null);
});