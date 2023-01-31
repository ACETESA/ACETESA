/// <reference path="../jquery-ui-1.11.4.js" />
/// <reference path="GlobalScript.js" />
/// <reference path="../lib/Ajax.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="~/Scripts/toastr.js" />
/// <reference path="~/Scripts/jquery-2.1.3.js" />
/// <reference path="~/Scripts/jquery.validate-vsdoc.js" />

var _sessionType = "Nuevo";

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
            $("#txtArticulo").val("");
            $("#CotizacionDetailViewModel_fq_cantidad").val("");
            $("#CotizacionDetailViewModel_fm_precio").val("");
            $("#CotizacionDetailViewModel_fm_precio2").val("");
            $("#CotizacionDetailViewModel_fm_precio_fin").val("");
            $("#CotizacionDetailViewModel_fm_precio_tonelada").val("");
            $("#CotizacionDetailViewModel_fq_peso_teorico").val("");
            scrollToScreen("#articulo", 1000);
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
    var errorMessages = $.parseJSON(data.responseText);
    if (errorMessages.errorMessage) {
        var errors = errorMessages.errorMessage;
        toastr.error(errors);
    } else {
        toastr.error(errorMessages);
    }
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
                    source: $.proxy(this, "_source"),
                    position: { collision: "flip" }
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
                    // Cerrar si ya es visible
                    if (wasOpen) {
                        return;
                    }
                    // Pasar string vacio como valor para la busqueda, muestra todos los resultados
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
            // Selecciona un item, no hace nada
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
                    //alert(key);
                    if (key === "1") {
                        toastr.warning(data[key]);
                        $("#txtRazonSocial").val("");
                        $("#cc_analis").val("");
                        return;
                    } else {
                        if (key === "2") {
                            toastr.info(data[key]);
                        }
                        //Cargar Select Sucursal y Contacto
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
                                    $("#cn_contacto").html("<option value=''>Ninguno</option>");
                                    var option = "";
                                    for (var i = 0; i < result.length; i++) {
                                        option += "<option value='" + result[i].value + "'>" + result[i].text + "</option>";
                                    }
                                    sSucursal.html(option);
                                    document.getElementById("cn_suc").setAttribute("onchange", "LlenarContactoSelect();");
                                    OcultarMostrarSucursal(result.length);
                                },
                                error: function (result) {
                                    alert("Error en javascript...");
                                }
                            });
                        } catch (e) {
                            alert(e);
                        }
                        //Cargar Select Condiciones de venta
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
                        //Cargar Select Visitas
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
                var request = new Ajax();
                var url = urlGetClientesByParam;
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

    var callbackPrecioLista = function (ccArtic) {
        //Obtenemos la lista seleccionada
        var cc_tienda = $('#Tienda option:selected').val();
        var igv_bo = $('#igv_bo option:selected').val();

        var $moneda = $("#cc_moneda");
        if ($.trim($moneda.val()).length === 0) {
            toastr.warning("Para mostrar Precio de Lista, debe seleccionar la Moneda.");
            $("#txtArticulo").val("");
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
        var request = new Ajax("");
        var params = {
            codGrupo: $(this).val()
        };
        $SubGrupo.html("");

        request.JsonPost("GetSubGrupos", params, function (data) {
            $.each(data, function (index, item) {
                var html = "<option value='" + item.Value + "'>";
                html += item.Text;
                html += "</option>";
                $SubGrupo.append(html);
            });
            if (datosEdicion != null) {
                $SubGrupo.val(datosEdicion.idSubgrupo);
            }
        }, true, false);//Pantalla bloqueada , asincrono
    });

    var $Articulo = $("#CotizacionDetailViewModel_cc_artic");
    var $msgeArticulo = $("#msgeArticulo");

    $Articulo.combobox({
        id: "txtArticulo",
        messageRequired: "Debe seleccionar un Artículo.",
        objMessage: $msgeArticulo,
        objCallback: callbackPrecioLista
    });

    var $txtArticulo = $("#txtArticulo");
    $txtArticulo.click(function () {
        var input = this;
        input.focus();
        input.setSelectionRange(0, 999);
    });
    $txtArticulo.on("keydown keypress", function (e) {
        var keyCode = e.keyCode || e.which;

        if ((keyCode === 9 || keyCode === 13) && ($Articulo.val() == null || $Articulo.val() == "")) {
            e.preventDefault();
            e.stopPropagation();

            var grupo = $("#CotizacionDetailViewModel_cc_grupo").val();
            var subGrupo = $("#CotizacionDetailViewModel_cc_subgrupo").val();
            var cc_tienda = $("#Tienda").val();

            var $component = $(this);
            var criterio = $component.val();
            var longitud = $.trim($component.val()).length;
            if (longitud === 0) {
                return;
            }

            try {
                var request = new Ajax();
                var url = urlGetArticulosByGrupoParam;
                var params = {
                    grupo: grupo,
                    subGrupo: subGrupo,
                    param: criterio,
                    cc_tienda: cc_tienda
                };
                request.JsonPost(url, params, function (response) {
                    if (response.estado && response.estado === 1) {
                        $Articulo.children().remove();
                        $.each(response.data, function (i, state) {
                            $("<option>", {
                                value: $.trim(state.value)
                            }).html($.trim(state.text)).appendTo($Articulo);
                        });
                        $Articulo.combobox();
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

    var $btnAdd = $("#btnAdd");
    var $cotizacionForm = $("#cotizacionForm");

    $btnAdd.on("click", function (e) {

        var bValidRazonSocial = false;
        var bValidArticulo = false;

        var $cdRazsoc = $("#cd_razsoc");
        var $cdArtic = $("#CotizacionDetailViewModel_cd_artic");
        $cdRazsoc.val($txtRazonSocial.val());
        $cdArtic.val($txtArticulo.val());

        $cotizacionForm = $("#cotizacionForm");
        $cotizacionForm.valid();

        if ($.trim($txtRazonSocial.val()).length === 0) {
            $msgeCliente.text("Debe seleccionar un Cliente.");
            $msgeCliente.show();
        } else {
            bValidRazonSocial = true;
            $msgeCliente.hide();
        }

        if ($.trim($txtArticulo.val()).length === 0) {
            $msgeArticulo.text("Debe seleccionar un Artículo.");
            $msgeArticulo.show();
        } else {
            bValidArticulo = true;
            $msgeArticulo.hide();
        }

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

    var $btnSave = $("#btnSave");
    var $btnSaveAndSend = $("#btnSaveAndSend");
    var $formNewCotizacion = $("#formNewCotizacion");
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
    var emailService = new SendEmailService();

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

    $btnSave.on("click", function (e) {
        _tipoFormulario = "Cotizacion";
        if ($.trim($txtRazonSocial.val()).length === 0) {
            $Cliente.val("");
        }
        if ($.trim($txtArticulo.val()).length === 0) {
            $Articulo.val("");
        }

        $sCcAnalis.val($("#cc_analis").val());
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
        //alert("GUARDAR: " + gs_editando);
        //Hoy
        var hoy = new Date();
        var month = hoy.getMonth() + 1;
        var day = hoy.getDate();
        var hoy = (day < 10 ? '0' : '') + day + '/' +
                     (month < 10 ? '0' : '') + month + '/'+
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
    $btnSaveAndSend.on("click", function () {
        _tipoFormulario = "Cotizacion";
        if ($.trim($txtRazonSocial.val()).length === 0) {
            $Cliente.val("");
        }
        if ($.trim($txtArticulo.val()).length === 0) {
            $Articulo.val("");
        }
        $sCcAnalis.val($("#cc_analis").val());
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

        if (!Validaciones()) {
            return false;
        }

        emailService.GetEmailByCliente("cc_analis");

    });

    var $btnSendMailModal = $("#btnSendMailModal");
    $btnSendMailModal.on("click", function () {
        emailService.Send(function () {
            $formNewCotizacion.submit();
        });
    });

    if (emailModel !== null && emailModel.para) {
        try {
            var request = new Ajax();
            var url = urlEnviarMail;
            var params = {
                Para: emailModel.para,
                ConCopia: emailModel.conCopia,
                Asunto: emailModel.asunto,
                Mensaje: emailModel.mensaje,
                id: emailModel.cnProforma,
            };
            request.JsonPost(url, params, function (response) {

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
            var $Ruc = $("#Ruc");
            var $RazonSocial = $("#RazonSocial");
            var $Domicilio = $("#Domicilio");
            var $Email = $("#Email");
            var $btnCloseModal = $("#btnCloseModal");
            var $Telefono = $("#Telefono");
            var $cc_sector = $("#cc_sector");
            var $cc_departamento = $("#cc_departamento");
            var $cc_provincia = $("#cc_provincia");
            var $cc_distrito = $("#cc_distrito");
            if ($cc_departamento.val() == "00") {
                toastr.error("Debe seleccionar un Departamento válido");
                return false;
            }
            if ($cc_provincia.val() == "00") {
                toastr.error("Debe seleccionar una Provincia válido");
                return false;
            }
            if ($cc_distrito.val() == "00") {
                toastr.error("Debe seleccionar un Distrito válido");
                return false;
            }
            if (!ValidarSunat($Ruc.val())) {
                return false;
            }
            try {
                var request = new Ajax();
                var url = urlClienteNew;
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
                } else {
                    $("#result").html(response);
                }
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

    var $CotizacionDetailViewModel_cc_grupo = $("#CotizacionDetailViewModel_cc_grupo");
    $CotizacionDetailViewModel_cc_grupo.on("change", function () {
        try {
            $("#CotizacionDetailViewModel_cc_artic").val("");
            $("#txtArticulo").val("");
            $Articulo.children().remove();
        } catch (e) {
            toastr.error(e);
        }
    });

    var $CotizacionDetailViewModel_cc_subgrupo = $("#CotizacionDetailViewModel_cc_subgrupo");
    $CotizacionDetailViewModel_cc_subgrupo.on("change", function () {
        try {
            $("#CotizacionDetailViewModel_cc_artic").val("");
            $("#txtArticulo").val("");
            $Articulo.children().remove();
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
    //SeleccionarTiendaSegunVendedor();
    ListarSectores();
    ListarDepartamentos();
}

//function SeleccionarTiendaSegunVendedor() {
//    $.ajax({
//        destroy: true,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        url: urlTiendaSegunVendedor,
//        data: JSON.stringify({
//        }),
//        dataType: "json",
//        success: function (result) {
//            var stienda = $("#Tienda");
//            var cc_tienda = result.cc_tienda;
//            stienda.val(cc_tienda);
//        },
//        error: function (result) {
//            alert("Error en javascript...");
//        }
//    });
//}

$('#igv_bo').on('change', function () {
    $("#txtArticulo").val("");
});

$('#Tienda').on('change', function () {
    $("#txtArticulo").val("");
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
        success: function (result) {
            var sOption = "<option value=''>Ninguno</option>";
            for (var i = 0; i < result.length; i++) {
                sOption+="<option value='" + result[i].cn_contacto + "'>" + result[i].cd_contacto + "</option>";
            }
            sContacto.html(sOption);
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function OcultarMostrarSucursal(filas) {
    var ddlCnSuc = document.getElementById("ddlCnSuc");
    var ddlCnContacto = document.getElementById("ddlCnContacto");
    if (filas > 2) {
        //Visibilidad (si)
        ddlCnSuc.setAttribute("style", "display:normal;");
        //Tamaño
        ddlCnContacto.setAttribute("class", "col-lg-4 mb-3");
    } else {
        //Visibilidad (no)
        ddlCnSuc.setAttribute("style", "display:none;");
        //Tamaño
        ddlCnContacto.setAttribute("class", "col-lg-12 mb-3");
        //Autoseleccionar
        $('#cn_suc :nth-child(2)').prop('selected', true);
        $('#cn_suc').trigger('change');
    }
}