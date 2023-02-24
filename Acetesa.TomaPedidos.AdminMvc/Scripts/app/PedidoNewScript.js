var _sessionType = "Nuevo";
var params;
var url;
var request;

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

    if ($EsProforma.val().toLowerCase() === "true") {
        disableSelect();
    }
}
function OnComplete() {
    unBlockScreen();
}
function OnFailure(data) {
    var errorMessages = JSON.parse(JSON.stringify(data.responseText));
    //var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        var errors = errorMessages.errorMessage;
        toastr.error(errors);
    } else {
        toastr.error(errorMessages);
    }
}

function limpiarCamposArticulo() {
    //$('#PedidoDetailViewModel_cc_grupo').prop('selectedIndex', 0);
    //$('#PedidoDetailViewModel_cc_subgrupo').prop('selectedIndex', 0);
    UnselectArticles();
    LlenarArticulosSelect();
    $("#PedidoDetailViewModel_fq_cantidad").val("");
    $("#PedidoDetailViewModel_fm_precio").val("");
    $("#PedidoDetailViewModel_fm_precio2").val("");
    $("#PedidoDetailViewModel_fm_precio_fin").val("");
    $("#PedidoDetailViewModel_fm_precio_tonelada").val("");
    $("#PedidoDetailViewModel_fq_peso_teorico").val("");
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
                .addClass("form-control col-md-6")
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
        ///Added lately


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
                    // Close if already visible
                    if (wasOpen) {
                        return;
                    }
                    // Pass empty string as value to search for, displaying all results
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

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
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
        //allows programmatic selection of combo using the option value
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

    var callbackCliente = function (ccAnalis, callbackSucursal) {
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
                    } else if (key === "2") {
                        toastr.info(data[key]);
                    } else {
                        try {
                            var request = new Ajax();
                            var url = _baseUrl + "Pedido/GetSucursalesJson";
                            params = {
                                ccAnalis: ccAnalis
                            };
                            request.JsonPost(url, params, function (response) {
                                var sOption = "";
                                $.each(response, function (inx, item) {
                                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                                });
                                $("#cn_suc").html(sOption);
                                if (callbackSucursal !== undefined) {
                                    callbackSucursal();
                                }
                            });
                        } catch (e) {
                            alert(e);
                        }

                        try {
                            request = new Ajax();
                            url = _baseUrl + "Pedido/GetCondicionesVentasJson";
                            params = {
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
                            request = new Ajax();
                            url = _baseUrl + "Pedido/GetLugarEntregaJson";
                            params = {
                                ccAnalis: ccAnalis
                            };
                            request.JsonPost(url, params, function (response) {
                                var sOption = "";
                                $.each(response, function (inx, item) {
                                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                                });
                                $("#Cn_lug").html(sOption);
                            });
                        } catch (e) {
                            alert(e);
                        }

                        try {
                            request = new Ajax();
                            url = _baseUrl + "Pedido/GetContactoEntregaDirectaJson";
                            params = {
                                ccAnalis: ccAnalis
                            };
                            request.JsonPost(url, params, function (response) {
                                var sOption = "";
                                $.each(response, function (inx, item) {
                                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                                });
                                $("#IdContactoEntregaDirecta").html(sOption);
                            });
                        } catch (e) {
                            alert(e);
                        }

                        try {
                            request = new Ajax();
                            url = _baseUrl + "Pedido/GetTransporteJson";
                            params = {
                                ccAnalis: ccAnalis
                            };
                            request.JsonPost(url, params, function (response) {
                                var sOption = "";
                                $.each(response, function (inx, item) {
                                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                                });
                                $("#CC_transp").html(sOption);
                            });
                        } catch (e) {
                            alert(e);
                        }
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
            var datos = $txtRazonSocial.length
            if (longitud === 0) {
                return;
            }
            try {
                var request = new Ajax(_baseUrl);
                var url = "pedido/getclientesbyparam";
                params = {
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

    var $fechaEntrega = $("#FechaEntrega");
    if ($.trim($fechaEntrega.val()).length === 0) {
        $fechaEntrega.val(today);
    }
    $fechaEntrega.datepicker({ dateFormat: "dd/mm/yy" });
    $fechaEntrega.mask("99/99/9999", { placeholder: "dd/mm/yyyy" });

    function CargarPrecioLista(ccArtic) {
        //Obtenemos la lista seleccionada
        var cc_tienda = $('#Tienda option:selected').val();
        var igv_bo = $('#igv_bo option:selected').val();

        //Validando producto
        var cc_tienda = $("#Tienda").val();
        var cn_proforma = $("#cn_proforma").val();
        var cantidadSolicitado = $("#PedidoDetailViewModel_fq_cantidad").val();
        var EsProforma = $("#EsProforma").val();

        if (cantidadSolicitado.length === 0) {
            cantidadSolicitado = 0;
        }

        $.ajax({
            destroy: true,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: _baseUrl + "Pedido/ValidaStockArticuloPedido",
            data: JSON.stringify({
                cc_artic: ccArtic,
                cc_tienda: cc_tienda,
                StockSolicitado: cantidadSolicitado,
                EsProforma: EsProforma,
                cn_proforma: cn_proforma,
                cn_pedido: cn_pedido

            }),
            dataType: "json",
            success: function (result) {
                var id = result.id;
                var mensaje = result.mensaje;
                if (id == 0) {
                    toastr.warning(mensaje);
                    UnselectArticles();
                } else {
                    var $moneda = $("#cc_moneda");
                    var $tienda = $("#Tienda");
                    if ($.trim($tienda.val()).length === 0) {
                            toastr.warning("Para mostrar Precio de Lista, debe seleccionar la Tienda.");
                            UnselectArticles();
                        } else if ($.trim($moneda.val()).length === 0) {
                            toastr.warning("Para mostrar Precio de Lista, debe seleccionar la Moneda.");
                            UnselectArticles();
                    } else {
                            try {
                                var request = new Ajax();
                                var url = _baseUrl + "Pedido/GetPrecioLista";
                                params = {
                                    ccArtic: ccArtic,
                                    cc_tienda: cc_tienda,
                                    igv_bo: igv_bo
                                };
                                request.JsonPost(url, params, function (response) {
                                    var $precioLista = $("#PedidoDetailViewModel_fm_precio");
                                    var $precioLista2 = $("#PedidoDetailViewModel_fm_precio2");
                                    var $precioListaFin = $("#PedidoDetailViewModel_fm_precio_fin");
                                    var $pesoUnitario = $("#PedidoDetailViewModel_fq_peso_teorico");
                                    var $precioTM = $("#PedidoDetailViewModel_fm_precio_tonelada");
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
                }
            },
            error: function (result) {
                alert("Error en javascript...");
            }
        });
    }

    var $Grupo = $("#PedidoDetailViewModel_cc_grupo");
    var $SubGrupo = $("#PedidoDetailViewModel_cc_subgrupo");
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
                    $("#PedidoDetailViewModel_cc_subgrupo").val("Ninguno");
                }

                LlenarArticulosSelect();
            },
            error: function (result) {
                alert("Error en javascript...");
            }
        });
    });

    //var $Articulo = $("#PedidoDetailViewModel_cc_artic");
    var $msgeArticulo = $("#msgeArticulo");

    //Ajax para validar el stock de articulo disponible
    function ValidaStockArticuloPedido() {
        return new Promise(function (resolve, reject) {
            var cc_tienda = $('#Tienda option:selected').val();
            var cantidadSolicitado = $("#PedidoDetailViewModel_fq_cantidad").val();
            var EsProforma = $("#EsProforma").val();
            var cn_proforma = $("#cn_proforma").val();
            var cc_tienda = $("#Tienda").val();

            if (gl_IdArtic === undefined) {
               var gl_IdArtic = $("#PedidoDetailViewModel_cc_artic").val();
            }

            $.ajax({
                destroy: true,
                async: false,
                url: _baseUrl + "Pedido/ValidaStockArticuloPedido",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    cc_artic: gl_IdArtic,
                    cc_tienda: cc_tienda,
                    StockSolicitado: cantidadSolicitado,
                    EsProforma: EsProforma,
                    cn_proforma: cn_proforma,
                    cn_pedido: cn_pedido

                }),
                dataType: "json",
                beforeSend: function () {
                },
                success: function (data) {
                    var id = data.id;
                    var mensaje = data.mensaje;

                    if (id == 0) {
                        toastr.warning(mensaje);
                        $("#PedidoDetailViewModel_fq_cantidad").val("");
                        return false;
                    }
                    else {
                        return true;
                    }
                     resolve(data);
                },
                error: function (err) {
                    reject(err)
                }
            });
        });
    }

    //Llamada del evento submit validar si continua o no
    $('#pedidoForm').submit(async function (e) {
        try {
            const result = await ValidaStockArticuloPedido();
        } catch (e) {
            console.log(e);
        }
    });
   
    //Evento clic del boton añadir detalle
    var $btnAdd = $("#btnAdd");
    $btnAdd.on("click", async function (e) {
        if ($EsProforma.val().toLowerCase() === "true") {
            enableSelect();
        }
        var bValidRazonSocial = false;
        var bValidArticulo = false;
        var bValidCantidad = false;


        var $cdRazsoc = $("#cd_razsoc");
        var $cdArtic = $("#PedidoDetailViewModel_cd_artic");
        $cdRazsoc.val($txtRazonSocial.val());
        $cdArtic.val($('#PedidoDetailViewModel_cc_artic option:selected').text());

        var cantidadArtic = parseFloat($cantidad.val());
        cantidadArtic = cantidadArtic.toFixed(2);

        $("#pedidoForm").valid();

        if ($.trim($txtRazonSocial.val()).length === 0) {
            $msgeCliente.text("Debe seleccionar un cliente.");
            $msgeCliente.show();
        } else {
            bValidRazonSocial = true;
            $msgeCliente.hide();
        }

        if (cantidadArtic == 0.00) {
            toastr.error("La cantidad del artículo debe ser mayor a 0");
        } else {
            bValidCantidad = true;
        }

        //Valida si articulo esta seleccionado
        var selected = 0;
        $('#PedidoDetailViewModel_cc_artic option').each(function () {
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


        if (!bValidRazonSocial || !bValidArticulo || !bValidCantidad) {
            e.preventDefault();
            e.stopPropagation();
        }
        else {
            gs_editando = 0;
        }
    });


    var $cantidad = $("#PedidoDetailViewModel_fq_cantidad");
    var $precioLista = $("#PedidoDetailViewModel_fm_precio");
    var $precioLista2 = $("#PedidoDetailViewModel_fm_precio2");
    var $precioFin = $("#PedidoDetailViewModel_fm_precio_fin");
    var $precioTM = $("#PedidoDetailViewModel_fm_precio_tonelada");
    var $pesoTM = $("#PedidoDetailViewModel_fq_peso_teorico");
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

    var $btnSave = $("#btnSave");
    var $btnSaveAndSend = $("#btnSaveAndSend");
    var $formNew = $("#formNewPedido");
    var $sCcAnalis = $("#scc_analis");
    var $sCcMoneda = $("#scc_moneda");
    var $sCcVta = $("#scc_vta");
    var $sCnSuc = $("#scn_suc");
    var $sCn_lug = $("#sCn_lug");
    var $sIdContactoEntregaDirecta = $("#sIdContactoEntregaDirecta");
    var $sTienda = $("#sTienda");
    var $sIgv_bo = $("#sIgv_bo");
    var $sZonaLiberada = $("#sZonaLiberada");
    var $sCC_transp = $("#sCC_transp");
    var $sContactoTransporte = $("#sContactoTransporte");
    var $sVt_observacion = $("#sVt_observacion");
    var $sCn_ocompra = $("#sCn_ocompra");
    var $sCbRecojo = $("#scb_recojo");
    var $sFechaEmision = $("#sfecha_emision");
    var $sFechaEntrega = $("#sfecha_entrega");
    var $sFmTipCam = $("#sfm_tipcam");
    var emailService = new SendEmailService();

    $(document).on('keyup keypress', 'form input[id="PedidoDetailViewModel_fq_cantidad"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            e.preventDefault();
            return false;
        }
    });
    $(document).on('keyup keypress', 'form input[id="PedidoDetailViewModel_fm_precio_tonelada"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            calcularPrecioFinalPrecioTM(0);
            e.preventDefault();
            return false;
        }
    });
    $(document).on('keyup keypress', 'form input[id="PedidoDetailViewModel_fm_precio_fin"]', function (e) {//Previene agregar detalle con enter en input text
        if (e.which == 13) {
            calcularPrecioFinalPrecioTM(1);
            e.preventDefault();
            return false;
        }
    });

    $btnSave.on("click", async function (e) {

        _tipoFormulario = "Pedido";
        if ($.trim($txtRazonSocial.val()).length === 0) {
            $Cliente.val("");
        }

        UnselectArticles();

        $sCcAnalis.val($("#cc_analis").val());
        $sCcMoneda.val($("#cc_moneda").val());
        $sCcVta.val($("#cc_vta").val());
        $sFechaEmision.val($("#FechaEmision").val());
        $sFechaEntrega.val($("#FechaEntrega").val());
        $sFmTipCam.val($("#n_i_paralelo_venta").val());
        $sCnSuc.val($("#cn_suc").val());
        $sCC_transp.val($("#CC_transp").val());
        $sContactoTransporte.val($("#ContactoTransporte").val());
        $sCn_lug.val($("#Cn_lug").val());
        $sIdContactoEntregaDirecta.val($("#IdContactoEntregaDirecta").val());
        $sTienda.val($("#Tienda").val());
        $sIgv_bo.val($("#igv_bo").val());
        $sZonaLiberada.val($("#zonaLiberada_bo").val());
        $sVt_observacion.val($("#Vt_observacion").val());
        $sCn_ocompra.val($("#cn_ocompra").val().trim());
        $sCbRecojo.val($("#cb_recojo").val());

        if (!Validaciones()) {
            e.preventDefault();
            return false;
        }
        else {
            return true;
        }
    });


    function Validaciones() {
        var parteFecha = $sFechaEmision.val().split("/");
        var fecEmision = new Date(+parteFecha[2], parteFecha[1] - 1, +parteFecha[0]); 
        var parteFecha = $sFechaEntrega.val().split("/");
        var fecEntrega = new Date(+parteFecha[2], parteFecha[1] - 1, +parteFecha[0]); 


        if (gs_editando == 1) {
            if (confirm("Se encuentra editando un artículo. ¿Desea continuar de todas formas?")) {

            }
            else {
                return false;
            }
        }
        else if ($sFechaEmision.val() == '') {
            toastr.warning("Debe ingresar una fecha de emisión válida.");
            return false;
        } else if ($sFechaEntrega.val() == '') {
            toastr.warning("Debe ingresar una fecha de entrega válida.");
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
        } else if (fecEmision > fecEntrega) {
            toastr.warning("Debe ingresar una fecha de entrega posterior a la de emisión.");
            return false;
        } else if (!ValidarSunat($sCcAnalis.val())) {
            return false;
        } else if ($("#cb_recojo").val() == "2") {
            if ($("#Cn_lug").val() == "" || $("#Cn_lug").val() == "Ninguno") {
                toastr.error("Debe seleccionar un Lugar de Entrega");
                return false;
            } else if ($("#IdContactoEntregaDirecta").val() == "" || $("#IdContactoEntregaDirecta").val() == "Ninguno") {
                toastr.error("Debe seleccionar un Contacto de Entrega");
                return false;
            }
        }

        return true;
    }

    
    $btnSaveAndSend.on("click", function (e) {
        var table = document.getElementById("table-detail-pedidos");
        var r = 1; //start counting rows in table
        var filasNoInvolucradas = 4;
        var filas = $('#table-detail-pedidos tr').length - filasNoInvolucradas;
        var lista_ccArtic = "";
        var lista_stockSolicitado = "";
        while (r <= filas) {
            //Codigos de articulos
            var cell = table.rows[r].cells[3];
            lista_ccArtic += cell.innerHTML + ",";
            //Stock solicitado de articulos
            var cell = table.rows[r].cells[6];
            lista_stockSolicitado += cell.innerHTML + ",";
            r++;
        }
        lista_ccArtic = lista_ccArtic.substring(0, lista_ccArtic.length - 1);
        lista_stockSolicitado = lista_stockSolicitado.substring(0, lista_stockSolicitado.length - 1);

        //Primera validacion de stock
        $.ajax({
            destroy: true,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: urlValidarStockPedido,
            data: JSON.stringify({
                listaArtic: lista_ccArtic,
                listaStockSolicitado: lista_stockSolicitado,
                ccTienda: $("#Tienda").val()
            }),
            dataType: "json",
            success: function (result) {
                id = result.id;
                if (id == "0") {
                    toastr.error(result.mensaje);
                }
                else {
                    //Valida Linea de Credito del Cliente
                    $.ajax({
                        destroy: true,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: urlValidaCreditoSobregiroPorPedido,
                        data: JSON.stringify({
                            ruc: $("#cc_analis").val(),
                            totalPedido: document.getElementById("TD_TotalDetallePedido").innerText.replace(",",""),
                            //totalPedido: document.getElementById("aTotVta").innerText,
                            monedaPedido: $("#cc_moneda").val()
                        }),
                        dataType: "json",
                        success: function (resultVLC) {
                            //Ver si la Condicion Vta es Credito
                            var EsCredito;
                            var $CondicionVta = $("#cc_vta option:selected").text();
                            if (~$CondicionVta.indexOf("CREDITO")) {
                                EsCredito = "1";
                            }
                            else {
                                EsCredito = "0";
                            }

                            //Si el mensaje es error: return
                            var mensajeID = resultVLC.mensajeID;
                            var mensajeVLC = resultVLC.mensaje;


                            if (mensajeID == "2" && EsCredito == "1") {//El cliente tiene problemas con la Linea de Credito
                                //Mensaje de confirmación
                                const modal = new Promise(function (resolve, reject) {
                                    $("#ModalMensajeConfirmacion .modal-title").html("Validación de Linea de Crédito");
                                    $("#ModalMensajeConfirmacion .modal-body").html(mensajeVLC);
                                    $('#ModalMensajeConfirmacion').modal('show');
                                    $('#ModalMensajeConfirmacion .btn-confirm').click(function () {
                                        $("#ModalMensajeConfirmacion .modal-title").html("Confirmar");
                                        $("#ModalMensajeConfirmacion .modal-body").html("[Mensaje de confirmación]");
                                        $('#ModalMensajeConfirmacion').modal('hide');
                                        resolve("Usuario desea continuar...");
                                    });
                                    $('#ModalMensajeConfirmacion .btn-cancel').click(function () {
                                        reject("Usuario NO desea continuar...");
                                    });
                                }).then(function (val) {
                                    //RESOLVE
                                    console.log(val);
                                    SaveNSendPedido();
                                }).catch(function (err) {
                                    //REJECT
                                    console.log("Usuario canceló la operación.", err)
                                    toastr.error("No se han guardado los cambios.", "Validación de Linea de Crédito", {
                                        closeButton: true,
                                        timeOut: 15000,
                                        progressBar: true 
                                    });
                                });
                            }
                            else if (mensajeID == "0" && EsCredito == "1") {
                                toastr.error(mensajeVLC, "Validación de Linea de Crédito", {
                                    closeButton: true,
                                    timeOut: 15000,
                                    progressBar: true
                                });
                            }
                            else {
                                SaveNSendPedido();
                            }
                        },
                        error: function (resultVLC) {
                            alert("Error en javascript...");
                        }
                    });
                }
            },
            error: function (result) {
                alert("Error en javascript...");
            }
        });
    });


    function SaveNSendPedido() {
        //Inicio: Save N Send
        _tipoFormulario = "Pedido";
        if ($.trim($txtRazonSocial.val()).length === 0) {
            $Cliente.val("");
        }

        UnselectArticles();

        $sCcAnalis.val($("#cc_analis").val());
        $sCcMoneda.val($("#cc_moneda").val());
        $sCcVta.val($("#cc_vta").val());
        $sFechaEmision.val($("#FechaEmision").val());
        $sFechaEntrega.val($("#FechaEntrega").val());
        $sFmTipCam.val($("#n_i_paralelo_venta").val());
        $sCnSuc.val($("#cn_suc").val());
        $sCC_transp.val($("#CC_transp").val());
        $sContactoTransporte.val($("#ContactoTransporte").val());
        $sCn_lug.val($("#Cn_lug").val());
        $sIdContactoEntregaDirecta.val($("#IdContactoEntregaDirecta").val());
        $sTienda.val($("#Tienda").val());
        $sIgv_bo.val($("#igv_bo").val());
        $sZonaLiberada.val($("#zonaLiberada_bo").val());
        $sVt_observacion.val($("#Vt_observacion").val());
        $sCn_ocompra.val($("#cn_ocompra").val());
        $sCbRecojo.val($("#cb_recojo").val());
        if (!Validaciones()) {
            return false;
        }
        var $detallePedidos = $("#table-detail-pedidos");
        $detallePedidos.find("tbody tr");

        if ($detallePedidos.length === 0) {
            toastr.warning("Debe seleccionar los articulos");
            e.preventDefault();
            return;
        }
        if ($formNew.valid()) {
            emailService.GetEmailByCliente("cc_analis");
        }
        //Fin: Save N Send
    }

    var $btnSendMailModal = $("#btnSendMailModal");
    $btnSendMailModal.on("click", function () {
        emailService.Send(function () {
            $formNew.submit();
        });
    });

    if (emailModel !== null && emailModel.para) {
        toastr.info("Enviando mail. Espere por favor.");
        try {
            var request = new Ajax();
            var url = _baseUrl + "Pedido/EnviarMail";
            params = {
                Para: emailModel.para,
                ConCopia: emailModel.conCopia,
                Asunto: emailModel.asunto,
                Mensaje: emailModel.mensaje,
                id: emailModel.cnPedido
            };
            request.JsonPost(url, params, function (response) {
                toastr.success("Mail enviado.");
            }).fail(function (data) {
                OnFailure(data);
            });
        } catch (e) {
            toastr.error(e);
        }
    }

    var $btnClienteNew = $("#btnClienteNew");
    $btnClienteNew.on("click", function (e) {
        $("#modal-cliente").modal("show");
    });

    var $btnSendClienteNew = $("#btnSendClienteNew");
    $btnSendClienteNew.on("click", function (e) {
        var $clienteForm = $("#clienteForm");
        if ($clienteForm.valid()) {
            var $Ruc = $(".ruc-sunat");
            var $RazonSocial = $(".razon-social-sunat");
            var $Domicilio = $(".domicilio-sunat");
            var $Telefono = $(".telefono-sunat");
            var $Email = $(".email-sunat");
            var $btnCloseModal = $("#btnCloseModal");
            var $cc_sector = $("#cc_sector");
            var $cc_departamento = $("#cc_departamento");
            var $cc_provincia = $("#cc_provincia");
            var $cc_distrito = $("#cc_distrito");
            if ($cc_departamento.val()=="00") {
                toastr.error("Debe seleccionar un Departamento válido");
                return false;
            }
            if ($cc_provincia.val()=="00") {
                toastr.error("Debe seleccionar una Provincia válido");
                return false;
            }
            if ($cc_distrito.val()=="00") {
                toastr.error("Debe seleccionar un Distrito válido");
                return false;
            }
            if (!ValidarSunat($Ruc.val())) {
                return false;
            }
            try {
                var request = new Ajax();
                var url = _baseUrl + "Pedido/ClienteNew";
                var params = {
                    Ruc: $Ruc.val(),
                    RazonSocial: $RazonSocial.val(),
                    Email: $Email.val(),
                    Domicilio: $Domicilio.val(),
                    Telefono: $Telefono.val(),
                    cc_sector: $cc_sector.val(),
                    cc_departamento: $cc_departamento.val(),
                    cc_provincia: $cc_provincia.val(),
                    cc_distrito: $cc_distrito.val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response === 1) {
                        var $ccAnalisNew = $("#cc_analis");
                        var option = "<option value='" + $Ruc.val() + "' selected>";
                        option += $RazonSocial.val();
                        option += "</option>";
                        $ccAnalisNew.append(option);

                        $ccAnalisNew.combobox("setValue", $Ruc.val());
                        callbackCliente($Ruc.val());
                        $btnCloseModal.trigger("click");
                        toastr.success("Cliente Registrado.");
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
                toastr.warning("Debe seleccionar la moneda.");
                return;
            }
            var $tipoCambio = $("#n_i_paralelo_venta");
            var request = new Ajax();
            var url = _baseUrl + "Pedido/ChangePreciosByMoneda";
            var params = {
                tipoMoneda: $tipoMoneda.val(),
                tipoCambio: $tipoCambio.val(),
                sessionType: _sessionType
            };
            request.HtmlPost(url, params, function (response) {
                if (response === "0") {
                } else {
                    $("#result").html(response);
                }
                limpiarCamposArticulo();
            });
        } catch (e) {
            toastr.error(e);
        }
    });

    var $cb_recojo = $("#cb_recojo");
    $cb_recojo.on("change", function (e) {
        var $tipoRecojo = $(this);
        $("#pnl_Cn_lug").hide();
        $("#pnl_cb_contactoEntregaDirecta").hide();
        $(".pnl_CC_transp").hide();
        if ($tipoRecojo.val() === "2") {
            $("#pnl_Cn_lug").show();
            $("#pnl_cb_contactoEntregaDirecta").show();
        } else if ($tipoRecojo.val() === "3") {
            $(".pnl_CC_transp").show();
        }
    });
    $("#cb_recojo").trigger("change");
    
    var $FechaEmision = $("#FechaEmision");
    $FechaEmision.on("change", function () {
        try {
            var $fecha = $(this);
            var request = new Ajax();
            var url = _baseUrl + "Pedido/ChangeTipoCambio";
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
                        $("#n_i_paralelo_venta").val(response);
                        break;
                }
            });
        } catch (e) {
            toastr.error(e);
        }
    });

    var $PedidoDetailViewModel_cc_subgrupo = $("#PedidoDetailViewModel_cc_subgrupo");
    $PedidoDetailViewModel_cc_subgrupo.on("change", function () {
        try {
            LlenarArticulosSelect();
        } catch (e) {
            toastr.error(e);
        }
    });

    var PedidoDetailViewModel_cc_artic = $("#PedidoDetailViewModel_cc_artic");
    PedidoDetailViewModel_cc_artic.on("change", function () {
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
    var $precioListaFin = $("#PedidoDetailViewModel_fm_precio_fin");
    var $pesoUnitario = $("#PedidoDetailViewModel_fq_peso_teorico");
    var $precioTM = $("#PedidoDetailViewModel_fm_precio_tonelada");
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
    ListarSectores();
    ListarDepartamentos();
}


$('#igv_bo').on('change', function () {
    limpiarCamposArticulo();
});

$('#Tienda').on('change', function () {
    limpiarCamposArticulo();
});

function ListarSectores() {
    var Ssector = $("#cc_sector");
    Ssector.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListarSectores,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            //Llenando combo sector
            for (var i = 0; i < result.length; i++) {
                Ssector.append(
                    "<option value=" + result[i].cc_sector + ">" + result[i].cd_sector + "</option>"
                );
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function ListarDepartamentos() {
    var Sdepartamentos = $("#cc_departamento");
    Sdepartamentos.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListarDepartamentos,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            //Llenando combo sector
            for (var i = 0; i < result.length; i++) {
                Sdepartamentos.append(
                    "<option value=" + result[i].cc_dpto + ">" + result[i].cd_dpto + "</option>"
                );
            }
            //Seleccionamos Lima por defecto
            Sdepartamentos.val(15);
            ListarProvincias();
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function ListarProvincias() {
    var Sprovincias = $("#cc_provincia");
    Sprovincias.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListarProvincias,
        data: JSON.stringify({
            cc_dpto: $("#cc_departamento").val()
        }),
        dataType: "json",
        success: function (result) {
            //Llenando combo sector
            for (var i = 0; i < result.length; i++) {
                Sprovincias.append(
                    "<option value=" + result[i].cc_prov + ">" + result[i].cd_prov + "</option>"
                );
            }
            ListarDistritos();
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function ListarDistritos() {
    var Sdistritos = $("#cc_distrito");
    Sdistritos.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlListarDistritos,
        data: JSON.stringify({
            cc_dpto: $("#cc_departamento").val(),
            cc_prov: $("#cc_provincia").val()
        }),
        dataType: "json",
        success: function (result) {
            //Llenando combo sector
            for (var i = 0; i < result.length; i++) {
                Sdistritos.append(
                    "<option value=" + result[i].cc_distrito + ">" + result[i].cd_distrito + "</option>"
                );
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

var $ccDepartamento = $("#cc_departamento");
$ccDepartamento.on("change", function (e) {
    ListarProvincias();
});

var $ccProvincia = $("#cc_provincia");
$ccProvincia.on("change", function (e) {
    ListarDistritos();
});

$('#modal-cliente').on('hidden.bs.modal', function () {
    //Limpiando formulario cada vez que se cierra
    $("#Ruc").val("");
    $("#cc_sector").val(02);
    $("#cc_departamento").val(15);
    ListarProvincias();
    $("#RazonSocial").val("");
    $("#Domicilio").val("");
    $("#Telefono").val("");
    $("#Email").val("");
})

//Llena Select Articulos
function LlenarArticulosSelect() {

    sCcArtic = $("#PedidoDetailViewModel_cc_artic");

    var grupo = $("#PedidoDetailViewModel_cc_grupo").val();
    var subGrupo = $("#PedidoDetailViewModel_cc_subgrupo").val();
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

            $('#PedidoDetailViewModel_cc_artic').selectpicker('refresh');
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
    $('#PedidoDetailViewModel_cc_artic').val('default').selectpicker('deselectAll');
    $('#PedidoDetailViewModel_cc_artic').selectpicker('refresh');
}