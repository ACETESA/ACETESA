/// <reference path="GlobalScript.js" />
/// <reference path="~/Scripts/jquery-ui-1.11.4.js" />
/// <reference path="~/Scripts/jquery.validate-vsdoc.js" />
/// <reference path="~/Scripts/toastr.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />

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
        scrollToScreen("#result", 1000);
    }
}
function OnComplete() {
    unBlockScreen();
}
function OnFailure(data) {
    var errorMessages = $.parseJSON(data.responseText);
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
        var $btnAnular = $("a[id='btnAnular" + response.id + "']");
        var $parenttr = $btnAnular.closest("tr");
        $parenttr.addClass("alert-danger");
        var $lasttd = $parenttr.find("td:last-child");
        $lasttd.text("ANULADO");
        $btnAnular.remove();
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
        changeYear: true,
        yearRange: yearInicio + ":" + yearFinal
    });
    $("#FechaInicio,#FechaFinal").mask("99/99/9999", { placeholder: "dd/mm/yyyy" });
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
              .insertAfter(this.element);

            this.element.hide();
            this._createAutocomplete();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
              value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
              .appendTo(this.wrapper)
              .val(value)
              .attr("title", "")
              .attr("id", "txtRazon")
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
                    $msgeCliente.text("");
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
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
        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });

    var $Cliente = $("#Cliente");
    var $msgeCliente = $("#msgeCliente");
    $Cliente.combobox();
    var $txtRazon = $("#txtRazon");
    $txtRazon.click(function () {
        var input = this;
        input.focus();
        input.setSelectionRange(0, 999);

    });
    $txtRazon.on("keypress", function (e) {
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
                var request = new Ajax(_baseUrl);
                var url = "pedido/getclientesbyparam";
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
        _tipoFormulario = "Pedido";
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
        if ($.trim($txtRazon.val()).length === 0) {
            $Cliente.val("");
        }
    });

    var $btnsSendMailList = $("a[class *= 'send-email']");
    var idPedido = 0;
    $(document).on("click", "a[class *= 'send-email']", function (e) {
        e.preventDefault();
        e.stopPropagation();
        idPedido = $(this).attr("id");
        var idcliente = $.trim($(this).attr("idcliente"));

        if (idcliente.length === 0) {
            toastr.warning("Debe seleccionar un cliente.");
            return;
        }
        var request = new Ajax();
        var url = _baseUrl + "Pedido/GetEmailCliente";
        var params = {
            id: idcliente,
            tipo: 'P',
            Nro: idPedido
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
    });

    $(document).on("click", "#btnSendMailModal", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var $Para  = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        
        var request = new Ajax();
        var url = _baseUrl + "Pedido/EnviarMail";
        var params = {
            Para: $Para.val(),
            ConCopia: $ConCopia.val(),
            Asunto: $Asunto.val(),
            Mensaje: $Mensaje.val(),
            id: idPedido
        };
        toastr.info("Enviado mail. Espere por favor.");
        request.JsonPost(url, params, function (response) {
            toastr.success("Mail enviado.");
            $Para.val("");
            $ConCopia.val("");
            $Asunto.val("");
            $Mensaje.val("");
            var $parenttr = $("a#" + idPedido).closest("tr");
            $parenttr.addClass("alert-info");
            var $lasttd = $parenttr.find("td:last-child");
            $lasttd.text("ENVIADO");
            idPedido = 0;
            $parenttr.remove();
            $("#btnCloseModal").trigger("click");
        }).fail(function (data) {
            OnFailure(data);
        });
    });
})(jQuery);