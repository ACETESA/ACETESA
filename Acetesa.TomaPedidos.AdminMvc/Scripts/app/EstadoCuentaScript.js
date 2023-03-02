(function ($) {
    //$.widget("custom.combobox", {
    //    _create: function () {
    //        this.wrapper = $("<span>")
    //          .insertAfter(this.element);
    //        this.element.hide();
    //        this._createAutocomplete();
    //    },

    //    _createAutocomplete: function () {
    //        var selected = this.element.children(":selected"),
    //          value = selected.val() ? selected.text() : "";

    //        this.input = $("<input>")
    //          .appendTo(this.wrapper)
    //          .val(value)
    //          .attr("title", "")
    //          .attr("id", "txtRazon")
    //          .addClass("form-control col-md-10")
    //          .autocomplete({
    //              delay: 0,
    //              minLength: 3,
    //              source: $.proxy(this, "_source")
    //          })
    //          .tooltip({
    //              tooltipClass: "ui-state-highlight"
    //          });

    //        this._on(this.input, {
    //            autocompleteselect: function (event, ui) {
    //                ui.item.option.selected = true;
    //                $msgeCliente.text("");
    //                this._trigger("select", event, {
    //                    item: ui.item.option
    //                });
    //            },

    //            autocompletechange: "_removeIfInvalid"
    //        });
    //    },

    //    _createShowAllButton: function () {
    //        var input = this.input,
    //          wasOpen = false;

    //        $("<a>")
    //          .attr("tabIndex", -1)
    //          .tooltip()
    //          .appendTo(this.wrapper)
    //          .button({
    //              icons: {
    //                  primary: "ui-icon-triangle-1-s"
    //              },
    //              text: false
    //          })
    //          .removeClass("ui-corner-all")
    //          .addClass("custom-combobox-toggle ui-corner-right")
    //          .mousedown(function () {
    //              wasOpen = input.autocomplete("widget").is(":visible");
    //          })
    //          .click(function () {
    //              input.focus();
    //              // Close if already visible
    //              if (wasOpen) {
    //                  return;
    //              }
    //              // Pass empty string as value to search for, displaying all results
    //              input.autocomplete("search", "");
    //          });
    //    },

    //    _source: function (request, response) {
    //        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
    //        response(this.element.children("option").map(function () {
    //            var text = $(this).text();
    //            var val = $(this).val();
    //            if (this.value && (!request.term || matcher.test(text) || matcher.test(val)))
    //                return {
    //                    label: text,
    //                    value: text,
    //                    option: this
    //                };
    //        }));
    //    },

    //    _removeIfInvalid: function (event, ui) {
    //        // Selected an item, nothing to do
    //        if (ui.item) {
    //            return;
    //        }
    //        // Search for a match (case-insensitive)
    //        var value = this.input.val(),
    //          valueLowerCase = value.toLowerCase(),
    //          valid = false;
    //        this.element.children("option").each(function () {
    //            if ($(this).text().toLowerCase() === valueLowerCase) {
    //                this.selected = valid = true;
    //                return false;
    //            }
    //        });
    //        // Found a match, nothing to do
    //        if (valid) {
    //            return;
    //        }
    //        // Remove invalid value
    //        this.input
    //          .val("")
    //          .attr("title", value + " no encontró ningún elemento")
    //          .tooltip("open");
    //        this.element.val("");
    //        this._delay(function () {
    //            this.input.tooltip("close").attr("title", "");
    //        }, 2500);
    //        this.input.autocomplete("instance").term = "";
    //    },
    //    _destroy: function () {
    //        this.wrapper.remove();
    //        this.element.show();
    //    }
    //});

    var urlDetalle = "";
    var urlResumen = "";
    function addUrlDetalle(object, id) {
        var ancla = $(object);
        if ($.trim(urlDetalle) == "") {
            urlDetalle = ancla.attr("href");
        }
        var urlRedirect = urlDetalle + "/" + id;
        ancla.attr("href", urlRedirect);
    }
    function addUrlResumen(object, id) {
        var ancla = $(object);
        if ($.trim(urlResumen) == "") {
            urlResumen = ancla.attr("href");
        }
        var urlRedirect = urlResumen + "/" + id;
        ancla.attr("href", urlRedirect);
    }

    var $Cliente = $("#Cliente");
    //var $ClienteInput = $("input.custom-combobox-input");
    //$Cliente.combobox();
    //var $tipoEc = $("#tipoEc");
    //var $form = $("#formEstadoCuenta");
    //var $msgeCliente = $("#msgeCliente");
    //$Cliente.change(function () {
    //    if ($.trim($Cliente.val()).length > 0) {
    //        $msgeCliente.text("");
    //    }
    //});
    

    $("#btnDetalle").click(function (e) {
        if ($.trim($Cliente.val()).length > 0) {
            addUrlDetalle(this, $Cliente.val());
        } else {
            e.preventDefault();
            $msgeCliente.text("Campo requerido");
        }
    });
    $("#btnResumen").click(function (e) {
        if ($.trim($Cliente.val()).length > 0) {
            addUrlResumen(this, $Cliente.val());
        } else {
            e.preventDefault();
            $msgeCliente.text("Campo requerido");
        }
    });
    //var $txtRazon = $("#txtRazon");
    //$txtRazon.click(function () {
    //    var input = this;
    //    input.focus();
    //    input.setSelectionRange(0, 999);
    //});
    //$txtRazon.on("keypress", function (e) {
    //    if (e.which === 13) {
    //        e.preventDefault();
    //        e.stopPropagation();

    //        var $component = $(this);
    //        var criterio = $component.val();
    //        var longitud = $.trim($component.val()).length;
    //        if (longitud === 0) {
    //            return;
    //        }

    //        try {
    //            var request = new Ajax(_baseUrl);
    //            var url = "EstadoCuenta/getclientesbyparam";
    //            var params = {
    //                param: criterio
    //            };
    //            request.JsonPost(url, params, function (response) {
    //                if (response.estado && response.estado === 1) {
    //                    $Cliente.children().remove();
    //                    $.each(response.data, function (i, state) {
    //                        $("<option>", {
    //                            value: $.trim(state.value)
    //                        }).html($.trim(state.text)).appendTo($Cliente);
    //                    });
    //                    $Cliente.combobox();
    //                    var event = jQuery.Event("keypress");
    //                    event.which = 40;
    //                    event.keyCode = 40; //keycode to trigger this for simulating arrow bottom
    //                    $component.trigger(event);
    //                }
    //            });
    //        } catch (ex) {
    //            toastr.error(ex);
    //        }
    //    }
    //});
})(jQuery);
function descargar() {
    var $Cliente = $("#Cliente");
    if ($.trim($Cliente.val()).length > 0) {
        var sUrl = "EstadoCuenta/descargar/?id=" + $Cliente.val() + "&razSoc=" + $("#Cliente option:selected").text();
        $.fileDownload(sUrl, {
            failMessageHtml: "There was a problem generating your report, please try again."
        });
        return false;
    } else {
        var $msgeCliente = $("#msgeCliente");
        $msgeCliente.text("Campo requerido");
    }
}
//Para enviar los correos
var emailService = new SendEmailEstadoCuentaService();

function onclickEnviarCorreo() {
    var $Cliente = $("#Cliente");
    if ($.trim($Cliente.val()).length > 0) {
        _tipoFormulario = "EstadoCuenta";
        emailService.GetEmailByClienteEstadoCuenta($Cliente.val());
    } else {
        var $msgeCliente = $("#msgeCliente");
        $msgeCliente.text("Campo requerido");
    }
}

var $btnSendMailModal = $("#btnSendMailModal");
$btnSendMailModal.on("click", function () {
    emailService.Send(function () {
    });
});

//Enviando correos
$(document).on("click", "#btnSendMailModal", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var $Para = $("#Para");
    var $ConCopia = $("#ConCopia");
    var $Asunto = $("#Asunto");
    var $Mensaje = $("#Mensaje");
    var $Cliente = $("#Cliente");

    var request = new Ajax();
    var url = _baseUrl + "EstadoCuenta/EnviarMail";
    var params = {
        Para: $Para.val(),
        ConCopia: $ConCopia.val(),
        Asunto: $Asunto.val(),
        Mensaje: $Mensaje.val(),
        idCliente: $Cliente.val(),
    };
    if ($Para.val() == "") {
        $Para.parent().addClass("has-error");
        return;
    }
    if ($Mensaje.val()=="") {
        return;
    }
    toastr.info("Enviado mail. Espere por favor.");
    request.JsonPost(url, params, function (response) {
        toastr.success("Mail enviado.");
        $Para.val("");
        $ConCopia.val("");
        $Asunto.val("");
        $Mensaje.val("");
        $Cliente.val("");
        $("#btnCloseModal").trigger("click");
    }).fail(function (data) {
        //OnFailure(data);
        alert(data);
    });
});


function LlenarClientesSelect() {
    Cliente = $("#Cliente");
    Cliente.html("");
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlSelectClientesSegunCarteraVendedor,
        data: JSON.stringify({
        }),
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                Cliente.append("<option value='" + result[i].cc_analis + "'>" + result[i].cd_razsoc + "</option>");
            }
            $('#Cliente').selectpicker('refresh');
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}