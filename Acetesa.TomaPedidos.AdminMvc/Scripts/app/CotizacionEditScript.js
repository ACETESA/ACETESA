var _sessionType = "Editar";

function OnBegin() {
    blockScreen();
}

function AddParameterEliminarFila() {
    $("a[id ^= 'btnEliminarFila']").each(function (ix, item) {
        var ancla = $(item);
        var href = ancla.attr("href");
        ancla.attr("href", href + "?SessionTypes=" + _sessionType);
    });
}

function OnSuccessEliminar(data) {
    if ($.trim(data).length === 0) {
    } else {
        AddParameterEliminarFila();
    }
}
function OnSuccess(data) {
    if ($.trim(data).length === 0) {
    } else {
        AddParameterEliminarFila();
        limpiarCamposArticulo();
    }
    //Bloqueamos cada que se añade detalle
    $("#Tienda option:not(:selected)").prop("disabled", true);
    $("#igv_bo option:not(:selected)").prop("disabled", true);
    $("#cc_moneda option:not(:selected)").prop("disabled", true);

}
function OnComplete() {
    unBlockScreen();
}
function OnFailure(data) {
    var errorMessages = JSON.parse(JSON.stringify(data.responseText));
    if (errorMessages.errorMessage) {
        var errors = errorMessages.errorMessage;
        toastr.error(errors);
    } else {
        toastr.error(errorMessages);
    }
}

function OnBeginRechazado() { blockScreen(); }
function OnSuccessRechazado(data) {
    toastr.success(data);
    setTimeout(function() {
        self.location.reload();
    }, 1000);
}
function OnCompleteRechazado() { unBlockScreen(); }
function OnFailureRechazado(data) {
    var errorMessages = "";
    try {
        errorMessages = $.parseJSON(data.responseText);
    } catch (error) {
        errorMessages = "El Código ya existe en el detalle ...";
    }

    if (errorMessages.errorMessage) {
        var errors = errorMessages.errorMessage.split("\n");
        $.each(errors, function (idx, item) {
            toastr.error(item);
        });
    } else {
        toastr.error(errorMessages);
    }
}

function limpiarCamposArticulo() {
    //$('#CotizacionDetailViewModel_cc_grupo').prop('selectedIndex', 0);
    //$('#CotizacionDetailViewModel_cc_subgrupo').prop('selectedIndex', 0);
    UnselectArticles();
    LlenarArticulosSelect();
    $("#CotizacionDetailViewModel_fq_cantidad").val("");
    $("#CotizacionDetailViewModel_fm_precio").val("");
    $("#CotizacionDetailViewModel_fm_precio2").val("");
    $("#CotizacionDetailViewModel_fm_precio_fin").val("");
    $("#CotizacionDetailViewModel_fm_precio_tonelada").val("");
    $("#CotizacionDetailViewModel_fq_peso_teorico").val("");
    $("#datosStockTodasTiendas").html('');
}

(function ($, toastr) {
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
                .insertAfter(this.element);
            this.element.hide();
            this._createAutocomplete(this.options.id);
        },

        _createAutocomplete: function (id) {
            var selected = this.element.children(":selected"),
                value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
                .appendTo(this.wrapper)
                .val(value)
                .attr("title", "")
                .attr("id", id)
                .attr("data-val-required", this.options.messageRequired)
                .addClass("form-control col-md-10")
                .autocomplete({
                    delay: 0,
                    minLength: 3,
                    source: $.proxy(this, "_source")
                })
                .tooltip({
                    tooltipClass: "ui-state-highlight"
                });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this.options.objMessage.text("");
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                    if (this.options.objCallback) {
                        this.options.objCallback(ui.item.option.value);
                    }
                },
                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
                wasOpen = false;

            $("<a>")
                .attr("tabIndex", -1)
                .tooltip()
                .appendTo(this.wrapper)
                .button({
                    icons: {
                        primary: "ui-icon-triangle-1-s"
                    },
                    text: false
                })
                .removeClass("ui-corner-all")
                .addClass("custom-combobox-toggle ui-corner-right")
                .mousedown(function () {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .click(function () {
                    input.focus();

                    // Cerrar si es visible
                    if (wasOpen) {
                        return;
                    }
                    // Pasa vacio como valor de busquda para mostrar todos los resultados.
                    input.autocomplete("search", "");
                });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                var val = $(this).val();
                if (this.value && (!request.term || matcher.test(text) || matcher.test(val)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {
            // Item seeleccionado, no hacer nada
            if (ui.item) {
                return;
            }
            // Busqueda (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });
            // Encontro coincidencia nada que hacer
            if (valid) {
                return;
            }
            // Elimina valor invalido
            this.input
                .val("")
                .attr("title", value + " no encontró ningún elemento")
                .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },
        //Permite seleccion del select segun el valor.
        setValue: function (value) {
            var $input = this.input;
            $("option", this.element).each(function () {
                if ($(this).val() === value) {
                    this.selected = true;
                    $input.val(this.text);
                    return false;
                }
            });
        },
        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
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

    function llenarSelectVisitas(ccAnalis) {
        //Cargar Select Condiciones de venta
        try {
            var request = new Ajax();
            var url = urlGetVisitasClientesJson;
            var params = {
                ccAnalis: ccAnalis,
                fechaEmision: $("#FechaEmision").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#VisitaClienteID").html(sOption);
            });
        } catch (e) {
            alert(e);
        }
    }

    var callbackCliente = function (ccAnalis) {
        validarClienteZonaLiberadaXRUC();

        $.ajax({
            url: urlValidarVendedorCliente,
            data: JSON.stringify({
                cc_analis: ccAnalis
            }),
            type: "POST",
            contentType: 'application/json',
            success: function (data) {
                for (var key in data) {
                    if (key === "1") {
                        toastr.warning(data[key]);
                        $("#txtRazonSocial").val("");
                        $("#cc_analis").val("");
                        return;
                    } else {
                        if (key === "2") {
                            toastr.info(data[key]);
                        }
                        try {
                            var request = new Ajax();
                            var url = urlGetCondicionesVentasJson;
                            var params = {
                                ccAnalis: ccAnalis
                            };
                            request.JsonPost(url, params, function (response) {
                                var sOption = "";
                                $.each(response, function (inx, item) {
                                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                                });
                                $("#cc_vta").html(sOption);
                            });
                        } catch (e) {
                            alert(e);
                        }
                        try {
                            sSucursal = $("#cn_suc");
                            $.ajax({
                                destroy: true,
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: urlSucursales,
                                data: JSON.stringify({
                                    ccAnalis: ccAnalis
                                }),
                                dataType: "json",
                                success: function (result) {
                                    var sOption = "";
                                    for (var i = 0; i < result.length; i++) {
                                        sOption += "<option value='" + result[i].value + "'>" + result[i].text + "</option>";
                                    }
                                    sSucursal.html(sOption);
                                    document.getElementById("cn_suc").setAttribute("onchange", "LlenarContactoSelect();");
                                    OcultarMostrarSucursal(result.length);
                                    $('#cn_suc').trigger('change');
                                },
                                error: function (result) {
                                    alert("Error en javascript...");
                                }
                            });
                        } catch (e) {
                            alert(e);
                        }
                        //Llenar Select Visita Clientes
                        llenarSelectVisitas(ccAnalis);
                    }
                }
            }
        });
    }

    var $Cliente = $("#cc_analis");
    var $msgeCliente = $("#msgeCliente");
    $Cliente.combobox({
        id: "txtRazonSocial",
        messageRequired: "Debe ingresar un Cliente.",
        objMessage: $msgeCliente,
        objCallback: callbackCliente
    });
    
    var $txtRazonSocial = $("#txtRazonSocial");
    $txtRazonSocial.click(function () {
        var input = this;
        input.focus();
        input.setSelectionRange(0, 999);
    });
    $txtRazonSocial.on("keypress", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            e.stopPropagation();

            var $component = $(this);
            var criterio = $component.val();
            var longitud = $.trim($component.val()).length;
            if (longitud === 0) {
                return;
            }

            try {
                var request = new Ajax("");
                var url = urlgetclientesbyparam;
                var params = {
                    param: criterio
                };
                request.JsonPost(url, params, function (response) {
                    if (response.estado && response.estado === 1) {
                        $Cliente.children().remove();
                        $.each(response.data, function (i, state) {
                            $("<option>", {
                                value: $.trim(state.value)
                            }).html($.trim(state.text)).appendTo($Cliente);
                        });
                        $Cliente.combobox();
                        var event = jQuery.Event("keypress");
                        event.which = 40;
                        event.keyCode = 40; //keycode to trigger this for simulating arrow bottom
                        $component.trigger(event);
                    }
                });
            } catch (ex) {
                toastr.error(ex);
            }
        }
    });   

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = "0" + dd;
    }

    if (mm < 10) {
        mm = "0" + mm;
    }

    today = dd + "/" + mm + "/" + yyyy;
    var $fechaEmision = $("#FechaEmision");
    if ($.trim($fechaEmision.val()).length === 0) {
        $fechaEmision.val(today);
    }
    $fechaEmision.datepicker({ dateFormat: "dd/mm/yy" });
    $fechaEmision.mask("99/99/9999", { placeholder: "dd/mm/yyyy" });


    function CargarPrecioLista(ccArtic) {
        //Obtenemos la lista seleccionada
        var cc_tienda = $('#Tienda option:selected').val();
        var igv_bo = $('#igv_bo option:selected').val();

        var $moneda = $("#cc_moneda");
        if ($.trim($moneda.val()).length === 0) {
            toastr.warning("Para mostrar Precio de Lista, debe seleccionar la Moneda.");
            UnselectArticles();
        } else {
            try {
                var request = new Ajax();
                var url = urlGetPrecioLista;
                var params = {
                    ccArtic: ccArtic,
                    cc_tienda: cc_tienda,
                    igv_bo: igv_bo
                };
                request.JsonPost(url, params, function (response) {
                    var $precioLista = $("#CotizacionDetailViewModel_fm_precio");
                    var $precioLista2 = $("#CotizacionDetailViewModel_fm_precio2");
                    var $precioListaFin = $("#CotizacionDetailViewModel_fm_precio_fin");
                    var $pesoUnitario = $("#CotizacionDetailViewModel_fq_peso_teorico");
                    var $precioTM = $("#CotizacionDetailViewModel_fm_precio_tonelada");
                    if (response !== null) {
                        $pesoUnitario.val(response.fq_peso_teorico);
                        switch ($moneda.find("option:selected").text()) {
                            case "S/.":
                                $precioLista.val(response.fm_precio_mn);
                                $precioLista2.val(response.fm_precio2_mn);
                                $precioListaFin.val(response.fm_precio_mn);
                                break;
                            case "US$":
                                $precioLista.val(response.fm_precio_me);
                                $precioLista2.val(response.fm_precio2_me);
                                $precioListaFin.val(response.fm_precio_me);
                                break;
                        }
                        $precioTM.val((($precioLista.val() / $pesoUnitario.val()) * 1000).toFixed(4));
                    } else {
                        $precioLista.val("");
                        $precioLista2.val("");
                        toastr.info("Artículo sin precio de lista.");
                    }
                });
            } catch (e) {
                alert(e);
            }
        }
    };

    var $Grupo = $("#CotizacionDetailViewModel_cc_grupo");
    var $SubGrupo = $("#CotizacionDetailViewModel_cc_subgrupo");
    $Grupo.change(function () {

        $SubGrupo.html("");
        $.ajax({
            destroy: true,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: urlGetSubGrupos,
            data: JSON.stringify({
                codGrupo: $(this).val(),
            }),
            dataType: "json",
            success: function (result) {
                $.each(result, function (index, item) {
                    var html = "<option value='" + item.Value + "'>";
                    html += item.Text;
                    html += "</option>";
                    $SubGrupo.append(html);
                });
                if (datosEdicion != null) {
                    $SubGrupo.val(datosEdicion.idSubgrupo);
                }

                if (result.length == 1) {
                    $("#CotizacionDetailViewModel_cc_subgrupo").val("Ninguno");
                }

                LlenarArticulosSelect();
            },
            error: function (result) {
                alert("Error en javascript...");
            }
        });

    });

    var $msgeArticulo = $("#msgeArticulo");


    var $btnAdd = $("#btnAdd");
    $btnAdd.on("click", function (e) {

        var bValidRazonSocial = false;
        var bValidArticulo = false;
        var $txtRazonSocial = $("#txtRazonSocial");

        var $cdRazsoc = $("#cd_razsoc");
        var $cdArtic = $("#CotizacionDetailViewModel_cd_artic");
        $cdRazsoc.val($txtRazonSocial.val());
        $cdArtic.val($('#CotizacionDetailViewModel_cc_artic option:selected').text());

        $("#cotizacionForm").valid();

        if ($.trim($txtRazonSocial.val()).length === 0) {
            $msgeCliente.text("Debe seleccionar un Cliente.");
            $msgeCliente.show();
        } else {
            bValidRazonSocial = true;
            $msgeCliente.hide();
        }

        //Valida si articulo esta seleccionado
        var selected = 0;
        $('#CotizacionDetailViewModel_cc_artic option').each(function () {
            if (this.selected) {
                selected = 1;
            }
        });

        if (selected == 1) {
            bValidArticulo = true;
            $msgeArticulo.hide();
        }
        else {
            $msgeArticulo.text("Debe seleccionar un Artículo.");
            $msgeArticulo.show();
        }

        //Verifica todas las validaciones
        if (!bValidRazonSocial || !bValidArticulo) {
            e.preventDefault();
            e.stopPropagation();
        }
        else {
            gs_editando = 0;
        }

    });

    var $cantidad = $("#CotizacionDetailViewModel_fq_cantidad");
    var $precioLista = $("#CotizacionDetailViewModel_fm_precio");
    var $precioLista2 = $("#CotizacionDetailViewModel_fm_precio2");
    var $precioFin = $("#CotizacionDetailViewModel_fm_precio_fin");
    var $precioTM = $("#CotizacionDetailViewModel_fm_precio_tonelada");
    var $pesoTM = $("#CotizacionDetailViewModel_fq_peso_teorico");
    if ($.trim($cantidad.val()) === "0.00") {
        $cantidad.val("");
    }
    if ($.trim($precioLista.val()) === "0.00") {
        $precioLista.val("");
    }
    if ($.trim($precioLista2.val()) === "0.00") {
        $precioLista2.val("");
    }
    if ($.trim($precioFin.val()) === "0.00") {
        $precioFin.val("");
    }
    if ($.trim($precioTM.val()) === "0.00") {
        $precioTM.val("");
    }
    if ($.trim($pesoTM.val()) === "0.00") {
        $pesoTM.val("");
    }

    $(document).on('keyup keypress', 'form input[id="CotizacionDetailViewModel_fq_cantidad"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            e.preventDefault();
            return false;
        }
    });
    $(document).on('keyup keypress', 'form input[id="CotizacionDetailViewModel_fm_precio_tonelada"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            calcularPrecioFinalPrecioTM(0);
            e.preventDefault();
            return false;
        }
    });
    $(document).on('keyup keypress', 'form input[id="CotizacionDetailViewModel_fm_precio_fin"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            calcularPrecioFinalPrecioTM(1);
            e.preventDefault();
            return false;
        }
    });

    var $btnSave = $("#btnSave");
    var $sCcAnalis = $("#scc_analis");
    var $sCcMoneda = $("#scc_moneda");
    var $sCcVta = $("#scc_vta");
    var $sFechaEmision = $("#sfecha_emision");
    var $sCnSuc = $("#scn_suc");
    var $sVisitaClienteID = $("#sVisitaClienteID");
    var $sCnContacto = $("#scn_contacto");
    var $sFmTipCam = $("#sfm_tipcam");
    var $sTienda = $("#sTienda");
    var $sIgv_bo = $("#sIgv_bo");
    var $sZonaLiberada = $("#sZonaLiberada");
    var $sImprimirPrecioTN = $("#sImprimirPrecioTN");
    var $sObservacion = $("#sObservacion");


    var $ccAnalis = $("#cc_analis");

    $btnSave.on("click", function () {
        _tipoFormulario = "Cotizacion";
        $sCcAnalis.val($ccAnalis.val());
        $sCcMoneda.val($("#cc_moneda").val());
        $sCcVta.val($("#cc_vta").val());
        $sFechaEmision.val($("#FechaEmision").val());
        $sCnSuc.val($("#cn_suc").val());
        $sVisitaClienteID.val($("#VisitaClienteID").val());
        $sCnContacto.val($("#cn_contacto").val());
        $sFmTipCam.val($("#n_i_val_venta").val());
        $sTienda.val($("#Tienda").val());
        $sIgv_bo.val($("#igv_bo").val());
        $sZonaLiberada.val($("#zonaLiberada_bo").val());
        $sImprimirPrecioTN.val($("#imprimirPrecioTN").val());
        $sObservacion.val($("#observacion").val());

        return Validaciones();
    });
    function Validaciones() {
        //Hoy
        var hoy = new Date();
        var month = hoy.getMonth() + 1;
        var day = hoy.getDate();
        var hoy = (day < 10 ? '0' : '') + day + '/' +
            (month < 10 ? '0' : '') + month + '/' +
            hoy.getFullYear();
        //
        if ($sFechaEmision.val() == '') {
            toastr.warning("Debe ingresar una fecha válida.");
            return false;
        } else if ($sFechaEmision.val() < hoy) {
            toastr.error("La 'Fecha de emision' debe ser igual o posterior a la fecha de hoy.");
            return false;
        } else if ($sCcAnalis.val() == '') {
            toastr.warning("Debe seleccionar un cliente.");
            return false;
        } else if ($sCcMoneda.val() == '') {
            toastr.warning("Debe seleccionar una moneda.");
            return false;
        } else if ($sCcVta.val() == '') {
            toastr.warning("Debe seleccionar una condición de venta.");
            return false;
        } else if ($sCnSuc.val() == '' || $sCnSuc.val() == 'Ni') {
            toastr.warning("Debe seleccionar una sucursal.");
            return false;
        } else if ($sCnContacto.val() == '' || $sCnContacto.val() == 'Ni') {
            toastr.warning("Debe seleccionar un contacto.");
            return false;
        } else if (!ValidarSunat($sCcAnalis.val())) {
            return false;
        } else if (gs_editando == 1) {
            if (confirm("Se encuentra editando un artículo. ¿Desea continuar de todas formas?")) {

            }
            else {
                return false;
            }
        }

        return true;
    }
    var $btnSendEmail = $("#btnSendEmail");
    $btnSendEmail.on("click", function (e) {
        _tipoFormulario = "Cotizacion";
        $sCcAnalis.val($ccAnalis.val());
        $sCcMoneda.val($("#cc_moneda").val());
        $sCcVta.val($("#cc_vta").val());
        $sFechaEmision.val($("#FechaEmision").val());
        $sCnSuc.val($("#cn_suc").val());
        $sVisitaClienteID.val($("#VisitaClienteID").val());
        $sCnContacto.val($("#cn_contacto").val());
        $sFmTipCam.val($("#n_i_val_venta").val());
        $sTienda.val($("#Tienda").val());
        $sIgv_bo.val($("#igv_bo").val());
        $sZonaLiberada.val($("#zonaLiberada_bo").val());
        $sImprimirPrecioTN.val($("#imprimirPrecioTN").val());
        $sObservacion.val($("#observacion").val());
        var $cnProforma = $("#cn_proforma");
        if (!Validaciones()) {
            return false;
        }

        var form = $("#scc_analis").parent("form").serialize();

        $.ajax({
            type: 'POST',
            url: urlEditarAjax,
            data: form,
            async: false,
            cache: false,
            dataType: 'json',
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseJSON.errorMessage);
            },
            success: function (data) {
                if (data.result == "Error") {
                    alert(data.message);
                    return false;
                } else {
                    var idCliente = $.trim($Cliente.val());
                    if (idCliente.length === 0) {
                        toastr.warning("Debe seleccionar un cliente.");
                        return;
                    }
                    var cn_contacto = $("#cn_contacto").val();
                    var cn_suc = $("#cn_suc").val();
                    var request = new Ajax();
                    var url = urlGetEmailCliente;
                    var params = {
                        id: idCliente,
                        tipo: 'C',
                        Nro: $cnProforma.val(),
                        cn_suc: cn_suc,
                        cn_contacto: cn_contacto
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
                }
            }
        });
    });

    var $btnRechazar = $("#btnRechazar");
    $btnRechazar.on("click", function (e) {
        var bConfirm = confirm("Confirmar cambio de estado a Rechazado.");
        if (!bConfirm) {
            e.preventDefault();
            e.stopPropagation();
        } 
    });

    var $btnSendMailModal = $("#btnSendMailModal");
    $btnSendMailModal.on("click", function (e) {
        var $Para = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        var $cnProforma = $("#cn_proforma");

        var $btnCloseModal = $("#btnCloseModal");
        var $enviarMailForm = $("#enviarMailForm");
        if ($enviarMailForm.valid()) {
            toastr.info("Enviando mail");
            try {
                var request = new Ajax();
                var url = urlEnviarMail;
                var params = {
                    Para: $Para.val(),
                    ConCopia: $ConCopia.val(),
                    Asunto: $Asunto.val(),
                    Mensaje: $Mensaje.val(),
                    id: $cnProforma.val(),
                    idCliente: $ccAnalis.val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response === 1) {
                        $Para.val("");
                        $Mensaje.val("");
                        $btnCloseModal.trigger("click");
                        toastr.success("Cotización enviada.");
                        setTimeout(function() {
                            self.location.reload();
                        }, 1000);
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            } 
        }
    });

    var $ccMoneda = $("#cc_moneda");
    $ccMoneda.on("change", function (e) {
        try {
            var $tipoMoneda = $(this);
            if ($.trim($tipoMoneda.val()).length === 0) {
                e.preventDefault();
                e.stopPropagation();
                toastr.warning("Debe seleccionar la Moneda.");
                return;
            }
            var $tipoCambio = $("#n_i_val_venta");
            var request = new Ajax();
            var url = urlChangePreciosByMoneda;
            var params = {
                tipoMoneda: $tipoMoneda.val(),
                tipoCambio: $tipoCambio.val(),
                sessionType: _sessionType
            };
            request.HtmlPost(url, params, function (response) {
                if (response === "0") {
                    //toastr.info("No existe Tipo de Cambio.");
                } else {
                    $("#result").html(response);
                }
                limpiarCamposArticulo();
            });
        } catch (e) {
            toastr.error(e);
        }
    });

    var $FechaEmision = $("#FechaEmision");
    $FechaEmision.on("change", function () {
        try {
            var $fecha = $(this);
            var request = new Ajax();
            var url = urlChangeTipoCambio;
            var params = {
                fechaEmision: $fecha.val(),
                sessionType: _sessionType
            };
            request.JsonPost(url, params, function (response) {
                switch (response) {
                    case -1:
                        toastr.warning("Debe seleccionar o ingresar la Fecha de Emisión.");
                        break;
                    case -2:
                        toastr.warning("Fecha de Emisión no válido (dd/mm/yyyy).");
                        break;
                    default:
                        $("#n_i_val_venta").val(response);
                        break;
                }
            });
        } catch (e) {
            toastr.error(e);
        }
    });

    var $CotizacionDetailViewModel_cc_subgrupo = $("#CotizacionDetailViewModel_cc_subgrupo");
    $CotizacionDetailViewModel_cc_subgrupo.on("change", function () {
        try {
            LlenarArticulosSelect();
        } catch (e) {
            toastr.error(e);
        }
    });

    var $CotizacionDetailViewModel_cc_artic = $("#CotizacionDetailViewModel_cc_artic");
    $CotizacionDetailViewModel_cc_artic.on("change", function () {
        try {
            CargarPrecioLista(this.value);
        } catch (e) {
            toastr.error(e);
        }
    });

    AddParameterEliminarFila();

})(jQuery, toastr);

function calcularPrecioFinalPrecioTM(tipo) {
    //0 - Lista
    //1 - Tonelada
    var $precioListaFin = $("#CotizacionDetailViewModel_fm_precio_fin");
    var $pesoUnitario = $("#CotizacionDetailViewModel_fq_peso_teorico");
    var $precioTM = $("#CotizacionDetailViewModel_fm_precio_tonelada");
    //Validaciones
    if (IsNumeric($precioListaFin.val()) !== true) {
        $precioTM.val(0.0000);
        $precioListaFin.val(0.0000);
        toastr.error("Debe ingresar un valor numérico en el Precio Lista");
        return;
    } else if (IsNumeric($pesoUnitario.val()) !== true) {
        $precioTM.val(0.0000);
        $precioListaFin.val(0.0000);
        toastr.error("Debe ingresar un valor numérico en el Peso Unitario");
        return;
    } else if (IsNumeric($precioTM.val()) !== true) {
        $precioTM.val(0.0000);
        $precioListaFin.val(0.0000);
        toastr.error("Debe ingresar un valor numérico en el Precio TM");
        return;
    }

    if (tipo === 0) {
        $precioListaFin.val((($precioTM.val() / 1000) * $pesoUnitario.val()).toFixed(4));
    } else {
        $precioTM.val((($precioListaFin.val() / $pesoUnitario.val()) * 1000).toFixed(4));
    }
}

function IsNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


window.onload = function () {
    //Bloqueamos cada que se añade detalle
    $("#Tienda option:not(:selected)").prop("disabled", true);
    $("#igv_bo option:not(:selected)").prop("disabled", true);
    $("#cc_moneda option:not(:selected)").prop("disabled", true);
    document.getElementById("cn_suc").setAttribute("onchange", "LlenarContactoSelect();");
}

$('#igv_bo').on('change', function () {
    limpiarCamposArticulo();
});

$('#Tienda').on('change', function () {
    limpiarCamposArticulo();
});

sContacto = $("#cn_contacto");
function LlenarContactoSelect() {
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlContactoPorSucursal,
        data: JSON.stringify({
            cn_suc: $("#cn_suc").val(),
            cc_analis: $("#cc_analis").val()
        }),
        dataType: "json",
        beforeSend: function() {
            blockScreen();
        },
        success: function (result) {
            var sOption = "<option value=''>Ninguno</option>";
            for (var i = 0; i < result.length; i++) {
                sOption += "<option value='" + result[i].cn_contacto + "'>" + result[i].cd_contacto + "</option>";
            }
            sContacto.html(sOption);
            unBlockScreen();
        },
        error: function (result) {
            alert("Error en javascript...");
            unBlockScreen();
        }
    });
}

function OcultarMostrarSucursal(filas) {
    var ddlCnSuc = document.getElementById("ddlCnSuc");
    var lblCnContacto = document.getElementById("lblCnContacto");
    var ddlCnContacto = document.getElementById("ddlCnContacto");
    if (filas > 2) {
        //Visibilidad (si)
        ddlCnSuc.setAttribute("style", "display:normal;");
        //Tamaño
        ddlCnContacto.setAttribute("class", "col-12 mb-3");
    } else {
        //Visibilidad (no)
        ddlCnSuc.setAttribute("style", "display:none;");
        //Tamaño
        ddlCnContacto.setAttribute("class", "col-12 mb-3");
        //Autoseleccionar
        $('#cn_suc :nth-child(2)').prop('selected', true);
        $('#cn_suc').trigger('change');
    }
}


//Llena Select Articulos
function LlenarArticulosSelect() {

    sCcArtic = $("#CotizacionDetailViewModel_cc_artic");

    var grupo = $("#CotizacionDetailViewModel_cc_grupo").val();
    var subGrupo = $("#CotizacionDetailViewModel_cc_subgrupo").val();
    var cc_tienda = $("#Tienda").val();

    sCcArtic.html("");

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlGetArticulosByGrupoParam,
        data: JSON.stringify({
            grupo: grupo,
            subGrupo: subGrupo,
            //param: '',
            cc_tienda: cc_tienda
        }),
        dataType: "json",
        success: function (result) {
            $.each(result.data, function (i, item) {
                sCcArtic.append("<option value='" + item.value + "'>" + item.text + "</option>");
            });

            $('#CotizacionDetailViewModel_cc_artic').selectpicker('refresh');
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}


//Propiedad selectPicker Articulos
$(document).ready(function () {
    LlenarArticulosSelect();
    $('.selectpicker').selectpicker({
        size: 5,
        title: 'Seleccione una opción'
    });
});


function UnselectArticles() {
    $('#CotizacionDetailViewModel_cc_artic').val('default').selectpicker('deselectAll');
    $('#CotizacionDetailViewModel_cc_artic').selectpicker('refresh');
}