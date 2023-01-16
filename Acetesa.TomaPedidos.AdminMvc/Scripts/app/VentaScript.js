/// <reference path="GlobalScript.js" />
/// <reference path="../jquery-ui-1.11.4.js" />
/// <reference path="../lib/Ajax.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="~/Scripts/toastr.js" />
/// <reference path="~/Scripts/jquery-2.1.3.js" />
/// <reference path="~/Scripts/jquery.validate-vsdoc.js" />

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
        alert(errorMessages.errorMessage);
    } else {
        alert(errorMessages);
    }
}

(function ($) {
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

    var callbackCliente = function (ccAnalis, callbackSucursal) {
        $.ajax({
            url: urlValidarVendedorCliente,
            data: {
                cc_analis: ccAnalis
            },
            type: "post",
            contenttype: 'application/json',
            success: function (data) {
                for (var key in data) {
                    if (key === "1") {
                        toastr.warning(data[key]);
                        $("#txtRazon").val("");
                        $("#Cliente").val("");
                        return;
                    }
                }
            }
        });
    }

    var $Cliente = $("#Cliente");
    var $msgeCliente = $("#msgeCliente");

    $Cliente.combobox({
        id: "txtRazon",
        messageRequired: "Debe ingresar un Cliente.",
        objMessage: $msgeCliente,
        objCallback: callbackCliente
    });

    var $txtRazon = $("#txtRazon");
    $txtRazon.click(function () {
        var input = this;
        input.focus();
        input.setSelectionRange(0, 999);

    });

    $txtRazon.on("keydown", function (e) {
        if (e.which === 13 || e.which === 9) {

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
                var url = "Venta/getclientesbyparam";
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

})(jQuery);
function descargarPDF(documento, path) {
    var sUrl = "descargarPDF/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}
function descargarZIP(documento, path) {
    var sUrl = "descargarZIP/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}
function descargarXML(documento, path) {
    var sUrl = "descargarXML/?documento=" + documento + "&path=" + encodeURI(path);
    $.fileDownload(sUrl, {
        failMessageHtml: "There was a problem generating your report, please try again."
    });
    return false;
}

//Para enviar los correos
var emailService = new SendEmailVentaService();
var gs_rutaPDF;
var gs_nroDocumento;
function onclickEnviarCorreo(ruc, nroDocumento, rutaPDF) {
    gs_rutaPDF = rutaPDF;
    gs_nroDocumento = nroDocumento;
    _tipoFormulario = "Venta";
    emailService.GetEmailByClienteVenta(ruc, nroDocumento);
}

var $btnSendMailModal = $("#btnSendMailModal");
$btnSendMailModal.on("click", function () {
    emailService.Send(function () {
    });
});