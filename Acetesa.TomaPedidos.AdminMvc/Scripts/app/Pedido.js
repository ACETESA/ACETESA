function Pedido() {
    function btnMostrarGrillaTransporte_click() {
        if ($("#cc_analis").val() == "") {
            return false;
        }
        if (window.columnagrillatransporte == null) {
            $.ajax({
                url: _baseUrl + "Pedido/GetColumnasGrillaSelect",
                cache: false,
                async: false,
                method: "POST",
                success: function (respuesta) {
                    window.columnagrillatransporte = respuesta;
                }
            });
        }
        DestruirDataTabla("#tblTransportistas");
        $('#PopupGrillaTransportista').modal('show');
        var bCargado = false;
        $('#PopupGrillaTransportista').on('shown.bs.modal', function (e) {
            if (bCargado == false) {
                $("#tblTransportistas").DataTable($.extend(
                    getConfiguracionDataTable(window.columnagrillatransporte), {
                        ajax: {
                            url: _baseUrl + "Pedido/GetTransporteJsonTodos",
                            cache: false,
                            data: {
                                ccAnalis: $("#cc_analis").val()
                            },
                            method: "POST",
                            dataSrc: ""
                        },
                        scrollY: '50vh',
                        scrollCollapse: true
                    }));
                $("#tblTransportistas").on('draw.dt',
                    function () {
                        $("#tblTransportistas").DataTable().$("td")
                            .filter(":not(.acciones,.dataTables_empty,.check-datatable)")
                            .unbind("click")
                            .click(seleccionarTransporte)
                            .end();
                        $("[name=chkTransporte]")
                            .click(function (e) {
                                var transporte = this.value;
                                var estado = (this.checked ? 1 : 0);

                                var request = new Ajax();
                                var url = _baseUrl + "Pedido/setTransporteCliente";
                                var params = {
                                    transporte: transporte,
                                    estado: estado,
                                    cliente: $("#cc_analis").val()
                                };
                                request.HtmlPost(url, params);

                                e.stopPropagation();
                            });
                    }
                );
            }
            bCargado = true;
        });
    }
    function seleccionarTransporte() {
        var transportista = $("#tblTransportistas").DataTable().row($(this).parents("tr")).data();

        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/setTransporteCliente";
            params = {
                transporte: transportista.codigo,
                estado: 1,
                cliente: $("#cc_analis").val()
            };
            request.JsonPost(url, params, function (response) {
                LlenaTransportes(transportista.codigo);
                $('#PopupGrillaTransportista').modal("hide");
            });
        } catch (e) {
            alert(e);
        }
    }
    function btnNuevoTransporte_click() {
        $('#PopupGrillaTransportista').modal("hide");
        $('#PopupNuevoTransportista').modal('show');
        //Para el scroll
        $("#Provincia").html("");
        $("#Distrito").html("");
        $('#TransporteForm')[0].reset();
        Departamento_click();
    }
    function btnCerrarTransporte_click() {
        $('#PopupGrillaTransportista').modal("hide");
        var valor = $("#CC_transp").val();
        LlenaTransportes(valor);
    }
    function LlenaTransportes(valor) {
        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/GetTransporteJson";
            params = {
                ccAnalis: $("#cc_analis").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#CC_transp").html(sOption);
                $("#CC_transp").val(valor);
                LlenaContactoTransportes(null);
            });
        } catch (e) {
            alert(e);
        }
    }
    function inicializar() {
        $(".mostrar-grilla-transporte").on("click", btnMostrarGrillaTransporte_click);
        $(".mostrar-grilla-contacto-transporte").on("click", btnMostrarGrillaContactoTransporte_click);
        $("#btnNuevoTransporte").on("click", btnNuevoTransporte_click);
        $("#btnNuevoContacto").on("click", btnNuevoContacto_click);
        $("#btnGrabarTransporte").on("click", btnGrabarTransporte_click);
        $("#btnGrabarContactoTransporte").on("click", btnGrabarContactoTransporte_click);
        $("#btnCerrarTransporte").on("click", btnCerrarTransporte_click);
        $("#btnCerrarContactoTransporte").on("click", btnCerrarContactoTransporte_click);
        $("#Departamento").change(Departamento_click);
        $("#Provincia").change(Provincia_click);
        $("#Distrito").change(Distrito_click);
        $("#DepartamentoLugarEntrega").change(DepartamentoLugarEntrega_click);
        $("#ProvinciaLugarEntrega").change(ProvinciaLugarEntrega_click);
        $("#DistritoLugarEntrega").change(DistritoLugarEntrega_click);
        $("#CC_transp").change(CC_transp_click);

        $(".mostrar-grilla-lugarEntrega").on("click", btnMostrarGrillalugarEntrega_click);
        $(".mostrar-grilla-contactoEntregaDirecta").on("click", btnMostrarGrillaContactoEntregaDirecta_click);
        $("#btnCerrarLugarEntrega").on("click", btnCerrarLugarEntrega_click);
        $("#btnCerrarContactoEntregaDirecta").on("click", btnCerrarContactoEntregaDirecta_click);
        $("#btnNuevoLugarEntrega").on("click", btnNuevoLugarEntrega_click);
        $("#btnNuevoContactoEntregaDirecta").on("click", btnNuevoContactoEntregaDirecta_click);
        $("#DepartamentoLugarEntrega").html($("#Departamento").html());
        $("#btnGrabarLugarEntrega").on("click", btnGrabarLugarEntrega_click);
        $("#btnGrabarContactoEntregaDirecta").on("click", btnGrabarContactoEntregaDirecta_click);
    }
    function btnGrabarTransporte_click() {
        var $Form = $("#TransporteForm");
        if ($Form.valid()) {
            var $Ruc = $(".ruc-sunat-tran");
            var $RazonSocial = $(".razon-social-sunat-tran");
            var $Domicilio = $(".domicilio-sunat-tran");
            var $Departamento = $(".departamento-tran");
            var $Provincia = $(".provincia-tran");
            var $Distrito = $(".distrito-tran");
            try {
                var request = new Ajax();
                var url = _baseUrl + "Pedido/TransportistaNew";
                var params = {
                    Ruc: $Ruc.val(),
                    RazonSocial: $RazonSocial.val(),
                    Domicilio: $Domicilio.val(),
                    Departamento: $Departamento.val(),
                    Provincia: $Provincia.val(),
                    Distrito: $Distrito.val(),
                    Cliente: $("#cc_analis").val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response.codigo) {
                        $('#PopupNuevoTransportista').modal("hide");
                        LlenaTransportes(response.codigo);
                        toastr.success("Transportista Registrado.");
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            }
        }
    }
    function Departamento_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetProvinciaJSON";
            params = {
                departamento: $("#Departamento").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#Provincia").html(sOption);
                Provincia_click();
            });
        } catch (e) {
            alert(e);
        }
    }
    function Provincia_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetDistritoJSON";
            params = {
                departamento: $("#Departamento").val(),
                provincia: $("#Provincia").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#Distrito").html(sOption);
            });
        } catch (e) {
            alert(e);
        }
    }
    function Distrito_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetZonaJSON";
            params = {
                departamento: $("#Departamento").val(),
                provincia: $("#Provincia").val(),
                distrito: $("#Distrito").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#Zona").html(sOption);
            });
        } catch (e) {
            alert(e);
        }
    }
    function btnMostrarGrillaContactoTransporte_click() {
        if ($("#cc_analis").val() == "") {
            return false;
        }
        if ($("#CC_transp").val() == "") {
            return false;
        }
        if (window.columnagrillacontactotransporte == null) {
            $.ajax({
                url: _baseUrl + "Pedido/GetColumnasGrillaSelectContacto",
                cache: false,
                async: false,
                method: "POST",
                success: function (respuesta) {
                    window.columnagrillacontactotransporte = respuesta;
                }
            });
        }
        DestruirDataTabla("#tblContactoTransportistas");
        $('#PopupContactoTransportista').modal('show');
        var bCargado = false;
        $('#PopupContactoTransportista').on('shown.bs.modal', function (e) {
            if (bCargado == false) {
                $("#tblContactoTransportistas").DataTable($.extend(
                    getConfiguracionDataTable(window.columnagrillacontactotransporte), {
                        ajax: {
                            url: _baseUrl + "Pedido/GetContactoTransporteJsonTodos",
                            cache: false,
                            data: {
                                transporte: $("#CC_transp").val()

                            },
                            method: "POST",
                            dataSrc: ""
                        },
                        scrollY: '50vh',
                        scrollCollapse: true
                    }));
                $("#tblContactoTransportistas").on('draw.dt',
                    function () {
                        $("#tblContactoTransportistas").DataTable().$("td")
                            .filter(":not(.acciones,.dataTables_empty,.check-datatable)")
                            .unbind("click")
                            .click(seleccionarContactoTransporte)
                            .end();
                        $("[name=chkContactoTransporte]")
                            .click(function (e) {
                                var contacto = this.value;
                                var estado = (this.checked ? 1 : 0);

                                var request = new Ajax();
                                var url = _baseUrl + "Pedido/setContactoTransporte";
                                var params = {
                                    contacto: contacto,
                                    estado: estado,
                                    transporte: $("#CC_transp").val()
                                };
                                request.HtmlPost(url, params);

                                e.stopPropagation();
                            });
                    }
                );
            }
            bCargado = true;
        });
    }
    function seleccionarContactoTransporte() {
        var contacto = $("#tblContactoTransportistas").DataTable().row($(this).parents("tr")).data();

        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/setContactoTransporte";
            params = {
                contacto: contacto.codigo,
                estado: 1,
                transporte: $("#CC_transp").val()
            };
            request.JsonPost(url, params, function (response) {
                LlenaContactoTransportes(contacto.codigo);
                $('#PopupContactoTransportista').modal("hide");
            });
        } catch (e) {
            alert(e);
        }
    }
    function LlenaContactoTransportes(valor) {
        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/GetContactoTransporteJson";
            params = {
                transporte: $("#CC_transp").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.codigo + "'>" + item.descri + "</option>";
                });
                $("#ContactoTransporte").html(sOption);
                if (valor != null) {
                    $("#ContactoTransporte").val(valor);
                }
            });
        } catch (e) {
            alert(e);
        }
    }
    function btnNuevoContacto_click() {
        $('#PopupContactoTransportista').modal("hide");
        $('#PopupNuevoContactoTransportista').modal('show');
        $('#ContactoForm')[0].reset();
    }
    function btnCerrarContactoTransporte_click() {
        $('#PopupContactoTransportista').modal("hide");
        var valor = $("#ContactoTransporte").val();
        LlenaContactoTransportes(valor);
    }
    function btnGrabarContactoTransporte_click() {
        var $Form = $("#ContactoForm");
        if ($Form.valid()) {
            var Nombres = $(".nombres-contacto");
            var Telefono1 = $(".telefono1-contacto");
            var Telefono2 = $(".telefono2-contacto");
            var Email = $(".email-contacto");
            try {
                var request = new Ajax();
                var url = _baseUrl + "Pedido/ContactoTransportistaNew";
                var params = {
                    Nombres: Nombres.val(),
                    Telefono1: Telefono1.val(),
                    Telefono2: Telefono2.val(),
                    Email: Email.val(),
                    Transportista: $("#CC_transp").val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response.codigo) {
                        $('#PopupNuevoContactoTransportista').modal("hide");
                        LlenaContactoTransportes(response.codigo);
                        toastr.success("Transportista Registrado.");
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            }
        }
    }
    function CC_transp_click() {
        LlenaContactoTransportes(null);
    }

    function btnMostrarGrillalugarEntrega_click() {
        if ($("#cc_analis").val() == "") {
            return false;
        }
        if (window.columnagrillaLugarEntrega == null) {
            $.ajax({
                url: _baseUrl + "Pedido/GetColumnasLugarEntrega",
                cache: false,
                async: false,
                method: "POST",
                success: function (respuesta) {
                    window.columnagrillaLugarEntrega = respuesta;
                }
            });
        }
        DestruirDataTabla("#tblLugarEntrega");
        $('#PopupGrillaLugarEntrega').modal('show');
        var bCargado = false;
        $('#PopupGrillaLugarEntrega').on('shown.bs.modal', function (e) {
            if (bCargado == false) {
                $("#tblLugarEntrega").DataTable($.extend(
                    getConfiguracionDataTable(window.columnagrillaLugarEntrega), {
                    ajax: {
                        url: _baseUrl + "Pedido/GetLugarEntregaJsonTodos",
                        cache: false,
                        data: {
                            ccAnalis: $("#cc_analis").val()
                        },
                        method: "POST",
                        dataSrc: ""
                    },
                    scrollY: '50vh',
                    scrollCollapse: true
                }));
                $("#tblLugarEntrega").on('draw.dt',
                    function () {
                        $("#tblLugarEntrega").DataTable().$("td")
                            .filter(":not(.acciones,.dataTables_empty,.check-datatable)")
                            .unbind("click")
                            .click(seleccionarLugarEntrega)
                            .end();
                    }
                );
            }
            bCargado = true;
        });
    }

    function btnMostrarGrillaContactoEntregaDirecta_click() {
        if ($("#cc_analis").val() == "") {
            return false;
        }
        if (window.columnagrillaContactoEntregaDirecta == null) {
            $.ajax({
                url: urlGetColumnasContactoEntregaDirecta,
                cache: false,
                async: false,
                method: "POST",
                success: function (respuesta) {
                    window.columnagrillaContactoEntregaDirecta = respuesta;
                }
            });
        }
        DestruirDataTabla("#tblContactoEntregaDirecta");
        $('#PopupGrillaContactoEntregaDirecta').modal('show');
        var bCargado = false;
        $('#PopupGrillaContactoEntregaDirecta').on('shown.bs.modal', function (e) {
            if (bCargado == false) {
                $("#tblContactoEntregaDirecta").DataTable($.extend(
                    getConfiguracionDataTable(window.columnagrillaContactoEntregaDirecta), {
                        ajax: {
                            url: _baseUrl + "Pedido/GetContactoEntregaDirectaJsonTodos",
                            cache: false,
                            data: {
                                ccAnalis: $("#cc_analis").val()
                            },
                            method: "POST",
                            dataSrc: ""
                        },
                        scrollY: '50vh',
                        scrollCollapse: true
                    }));
                $("#tblContactoEntregaDirecta").on('draw.dt',
                    function () {
                        $("#tblContactoEntregaDirecta").DataTable().$("td")
                            .filter(":not(.acciones,.dataTables_empty,.check-datatable)")
                            .unbind("click")
                            .click(seleccionarContactoEntregaDirecta)
                            .end();
                        $("thead tr th.cn_suc").each(function () {
                            this.setAttribute("style", "display:none;");
                        });
                        $("tbody tr td.cn_suc").each(function (i) {
                            this.setAttribute("style", "display:none;");
                        });
                    }
                );
            }
            bCargado = true;
        });
    }

    function btnCerrarLugarEntrega_click() {
        $('#PopupGrillaLugarEntrega').modal("hide");
    }
    function btnCerrarContactoEntregaDirecta_click() {
        $('#PopupGrillaContactoEntregaDirecta').modal("hide");
    }
    function seleccionarLugarEntrega() {
        var lugarEntrega = $("#tblLugarEntrega").DataTable().row($(this).parents("tr")).data();

        try {
            LlenaLugarEntrega(lugarEntrega.codigo);
            $('#PopupGrillaLugarEntrega').modal("hide");
        } catch (e) {
            alert(e);
        }
    }
    function seleccionarContactoEntregaDirecta() {
        var contactoEntrega = $("#tblContactoEntregaDirecta").DataTable().row($(this).parents("tr")).data();

        try {
            LlenaContactoEntregaDirecta(contactoEntrega.idContacto);
            $('#PopupGrillaContactoEntregaDirecta').modal("hide");
        } catch (e) {
            alert(e);
        }
    }
    function LlenaLugarEntrega(valor) {
        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/GetLugarEntregaJson";
            params = {
                ccAnalis: $("#cc_analis").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#Cn_lug").html(sOption);
                if (valor != null) {
                    $("#Cn_lug").val(valor);
                }
            });
        } catch (e) {
            alert(e);
        }
    }
    function LlenaContactoEntregaDirecta(valor) {
        try {
            request = new Ajax();
            var url = _baseUrl + "Pedido/GetContactoEntregaDirectaJson";
            params = {
                ccAnalis: $("#cc_analis").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#IdContactoEntregaDirecta").html(sOption);
                if (valor != null) {
                    $("#IdContactoEntregaDirecta").val(valor);
                }
            });
        } catch (e) {
            alert(e);
        }
    }

    function btnNuevoLugarEntrega_click() {
        $('#PopupGrillaLugarEntrega').modal("hide");
        $('#PopupNuevoLugarEntrega').modal('show');
        $('#LugarEntregaForm')[0].reset();
        $("#Entrega1").attr("checked", true);
        $("#Cobranza1").attr("checked", true);
        $("#DepartamentoLugarEntrega").val("15");
        DepartamentoLugarEntrega_click();
    }
    function btnNuevoContactoEntregaDirecta_click() {
        var ccAnalis = $("#cc_analis").val();
        var ccAnalis = $("#cc_analis").val();
        $('#PopupGrillaContactoEntregaDirecta').modal("hide");
        $('#PopupNuevoContactoEntregaDirecta').modal('show');
        $('#ContactoEntregaDirectaForm')[0].reset();
        $("#EnvioDocumento0").attr("checked", true);
        $("#TipoContacto3").attr("checked", true);
        //Cargar Sucursal en Modal
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetSucursalesJson";
            params = {
                ccAnalis: ccAnalis
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#SucursalContactoEntregaDirecta").html(sOption);
            });
        } catch (e) {
            alert(e);
        }
    }

    function DepartamentoLugarEntrega_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetProvinciaJSON";
            params = {
                departamento: $("#DepartamentoLugarEntrega").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#ProvinciaLugarEntrega").html(sOption);
                ProvinciaLugarEntrega_click();
            });
        } catch (e) {
            alert(e);
        }
    }

    function ProvinciaLugarEntrega_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetDistritoJSON";
            params = {
                departamento: $("#DepartamentoLugarEntrega").val(),
                provincia: $("#ProvinciaLugarEntrega").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#DistritoLugarEntrega").html(sOption);
                DistritoLugarEntrega_click();
            });
        } catch (e) {
            alert(e);
        }
    }

    function DistritoLugarEntrega_click() {
        try {
            request = new Ajax();
            url = _baseUrl + "Pedido/GetZonaJSON";
            params = {
                departamento: $("#DepartamentoLugarEntrega").val(),
                provincia: $("#ProvinciaLugarEntrega").val(),
                distrito: $("#DistritoLugarEntrega").val()
            };
            request.JsonPost(url, params, function (response) {
                var sOption = "";
                $.each(response, function (inx, item) {
                    sOption += "<option value='" + item.value + "'>" + item.text + "</option>";
                });
                $("#ZonaLugarEntrega").html(sOption);
            });
        } catch (e) {
            alert(e);
        }
    }

    function btnGrabarLugarEntrega_click() {
        var $Form = $("#LugarEntregaForm");
        if ($Form.valid()) {
            try {
                var request = new Ajax();
                var url = _baseUrl + "Pedido/LugarEntregaNew";
                var params = {
                    Direccion: $(".direccion-lugarentrega").val(),
                    Entrega: $("input:radio[name=EntregaLugarEntrega]:checked").val(),
                    Cobranza: $("input:radio[name=CobranzaLugarEntrega]:checked").val(),
                    Departamento: $(".departamento-lugarentrega").val(),
                    Provincia: $(".provincia-lugarentrega").val(),
                    Distrito: $(".distrito-lugarentrega").val(),
                    Zona: $(".zona-lugarentrega").val(),
                    Analisis: $("#cc_analis").val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response.codigo) {
                        $('#PopupNuevoLugarEntrega').modal("hide");
                        LlenaLugarEntrega(response.codigo);
                        toastr.success("Lugar de Entrega Registrado.");
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            }
        }
    }

    function btnGrabarContactoEntregaDirecta_click() {
        var $Form = $("#ContactoEntregaDirectaForm");
        if ($Form.valid()) {
            try {
                var request = new Ajax();
                var url = _baseUrl + "Pedido/ContactoEntregaDirectaNew";
                var params = {
                    EnvioDocs: $("input:radio[name=EnvioDocumentoContactoEntregaDirecta]:checked").val(),
                    ContAlmacen: $('#ContAlmacen').prop('checked'),
                    ContCobranza: $('#ContCobranza').prop('checked'),
                    ContVenta: $('#ContVenta').prop('checked'),
                    CargoLaboral: $(".cargolaboral-contactoentregadirecta").val(),
                    Email: $(".email-contactoentregadirecta").val(),
                    Telefono2: $(".telefono2-contactoentregadirecta").val(),
                    Telefono: $(".telefono-contactoentregadirecta").val(),
                    ApeMaterno: $(".apematerno-contactoentregadirecta").val(),
                    ApePaterno: $(".apepaterno-contactoentregadirecta").val(),
                    Nombres: $(".nombres-contactoentregadirecta").val(),
                    Surcursal: $(".sucursal-contactoentregadirecta").val(),
                    Analisis: $("#cc_analis").val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response.codigo) {
                        $('#PopupNuevoContactoEntregaDirecta').modal("hide");
                        LlenaContactoEntregaDirecta(response.codigo);
                        toastr.success("Contacto de Entrega Directa Registrado.");
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            }
        }
    }
    inicializar();
};
$(document).ready(Pedido);

$(document).ready(function () {
    //Redimensionar cantidad items
    //$('#PedidoDetailViewModel_cc_artic').attr('size', '5');

    $('.modal').on("hidden.bs.modal", function (e) {
        if ($('.modal:visible').length) {
                $('body').addClass('modal-open');
        }
    });
});

var datosEdicion = null;

function cargarDatosParaEditarArticulo(idArticulo, descArticulo, cantidad, precioTM, pesoUnit, precioLista, precioLista2, precioFinal) {

    //var request2 = new Ajax("");
    //var params2 = {
    //    idArticulo: idArticulo
    //};

    gs_editando = 1;

    ////Recupera stock del material seleccionado
    //request2.JsonPost(urlStockSegunArticulo, params2, function (result) {
    //    gl_stock = result[0].stock;
    //});

    cantidad = cantidad.replace(",", "");
    var request = new Ajax("");
    var params = {
        idArticulo: idArticulo
    };
    request.JsonPost(urlObtenerGrupoSubgrupoArtic, params, function (result) {
        datosEdicion = {
            idGrupo: result[0].idGrupo,
            idSubgrupo: result[0].idSubgrupo
        };
        var idGrupo = result[0].idGrupo;
        $("#btnEliminarFila" + idArticulo).trigger("click");
        $("#PedidoDetailViewModel_cc_grupo").val(idGrupo);
        $("#PedidoDetailViewModel_cc_grupo").trigger("change");

        //Asignamos el articulo
        var $ccAnalisNew = $("#PedidoDetailViewModel_cc_artic");
        var option = "<option value='" + idArticulo + "' selected>" + descArticulo.trim() + "</option>";
        $ccAnalisNew.html(option);
        $ccAnalisNew.combobox("setValue", idArticulo);

        //Cargar resto de elementos
        $("#PedidoDetailViewModel_fq_cantidad").val(cantidad);
        $("#PedidoDetailViewModel_fm_precio_tonelada").val(precioTM);
        $("#PedidoDetailViewModel_fq_peso_teorico").val(pesoUnit);
        $("#PedidoDetailViewModel_fm_precio").val(precioLista);
        $("#PedidoDetailViewModel_fm_precio2").val(precioLista2);
        $("#PedidoDetailViewModel_fm_precio_fin").val(precioFinal);
        datosEdicion = null;

        //Este codigo se carga al momento de llamar a editar
        gl_IdArtic = idArticulo;

    });
}
$("#PedidoDetailViewModel_fm_precio_tonelada")
    .focusout(function () {
        calcularPrecioFinalPrecioTM(0);
    });

$("#PedidoDetailViewModel_fm_precio_fin")
    .focusout(function () {
        calcularPrecioFinalPrecioTM(1);
    });




