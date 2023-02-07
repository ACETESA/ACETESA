$("#mostrar-grilla-contactoEntregaDirecta").on("click", btnMostrarGrillaContactoEntregaDirecta_click);
$("#btnCerrarContactoEntregaDirecta").on("click", btnCerrarContactoEntregaDirecta_click);
$("#btnNuevoContactoEntregaDirecta").on("click", btnNuevoContactoEntregaDirecta_click);
$("#btnGrabarContactoEntregaDirecta").on("click", btnGrabarContactoEntregaDirecta_click);
var datosEdicion = null;

var gsCnSuc = "";
var gsCnContacto = "";
var gsCcAnalis = "";

function cargarDatosParaEditarArticulo(idArticulo, descArticulo, cantidad, precioTM, pesoUnit, precioLista, precioLista2, precioFinal) {

    gs_editando = 1;

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

        //Selecciona el Grupo
        var idGrupo = result[0].idGrupo;
        $("#btnEliminarFila" + idArticulo).trigger("click");
        $("#CotizacionDetailViewModel_cc_grupo").val(idGrupo);
        //Carga el Subgrupo
        $("#CotizacionDetailViewModel_cc_subgrupo").html("");
        $.ajax({
            destroy: true,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: urlGetSubGrupos,
            data: JSON.stringify({
                codGrupo: $("#CotizacionDetailViewModel_cc_grupo").val(),
            }),
            dataType: "json",
            success: function (resultSG) {
                $SubGrupo = $("#CotizacionDetailViewModel_cc_subgrupo")
                $.each(resultSG, function (index, item) {
                    var html = "<option value='" + item.Value + "'>";
                    html += item.Text;
                    html += "</option>";
                    $SubGrupo.append(html);
                });
                if (datosEdicion != null) {
                    $SubGrupo.val(datosEdicion.idSubgrupo);
                }
                //Selecciona el Subgrupo
                var idSubgrupo = result[0].idSubgrupo;
                $("#CotizacionDetailViewModel_cc_subgrupo").val(idSubgrupo);
                //Carga el Articulo
                $("#CotizacionDetailViewModel_cc_artic").html("");
                $.ajax({
                    destroy: true,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: urlGetArticulosByGrupoParam,
                    data: JSON.stringify({
                        grupo: $("#CotizacionDetailViewModel_cc_grupo").val(),
                        subGrupo: $("#CotizacionDetailViewModel_cc_subgrupo").val(),
                        param: '',
                        cc_tienda: $("#Tienda").val()
                    }),
                    dataType: "json",
                    success: function (resultArt) {
                        $.each(resultArt.data, function (i, item) {
                            sCcArtic.append("<option value='" + item.value + "'>" + item.text + "</option>");
                        });
                        $('#CotizacionDetailViewModel_cc_artic').selectpicker('refresh');
                        //Selecciona el articulo
                        $('.selectpicker').selectpicker('val', [idArticulo]);
                        $('.selectpicker').selectpicker('refresh');
                    },
                    error: function (resultArt) {
                        alert("Error en javascript...");
                    }
                });

            },
            error: function (resultSG) {
                alert("Error en javascript...");
            }
        });


        //Cargar resto de elementos
        $("#CotizacionDetailViewModel_fq_cantidad").val(cantidad);
        $("#CotizacionDetailViewModel_fm_precio_tonelada").val(precioTM);
        $("#CotizacionDetailViewModel_fq_peso_teorico").val(pesoUnit);
        $("#CotizacionDetailViewModel_fm_precio").val(precioLista);
        $("#CotizacionDetailViewModel_fm_precio2").val(precioLista2);
        $("#CotizacionDetailViewModel_fm_precio_fin").val(precioFinal);
        //datosEdicion = null;
    });

    $([document.documentElement, document.body]).animate({
        scrollTop: $("#AgregarArticulos").offset().top
    }, 2000);
}

$("#CotizacionDetailViewModel_fm_precio_tonelada")
    .focusout(function () {
        calcularPrecioFinalPrecioTM(0);
    });

$("#CotizacionDetailViewModel_fm_precio_fin")
    .focusout(function () {
        calcularPrecioFinalPrecioTM(1);
    });

//INICIO: MODAL PARA REGISTRAR NUEVO CONTACTO
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
    document.getElementById("titleModalListaContacto").innerText = "Seleccione un contacto";
    document.getElementById("titleModalRegistrarContacto").innerText = "Nuevo contacto";
    gsCcAnalis = "";
    gsCnSuc = "";
    gsCnContacto = "";
    $('#PopupGrillaContactoEntregaDirecta').modal('show');
    var bCargado = false;
    $('#PopupGrillaContactoEntregaDirecta').one('shown.bs.modal', function (e) {
        document.getElementById("tblContactoEntregaDirecta").innerHTML = "";
        if (bCargado == false) {
            $("#tblContactoEntregaDirecta").DataTable($.extend(
                getConfiguracionDataTable(window.columnagrillaContactoEntregaDirecta), {
                    ajax: {
                        url: urlGetContactoEntregaDirectaJsonTodos,
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
            $("#tblContactoEntregaDirecta").one('draw.dt',
                function () {
                    $("#tblContactoEntregaDirecta").DataTable().$("td")
                        .filter(":not(.acciones,.dataTables_empty,.check-datatable)")
                        .unbind("click")
                        //.click(seleccionarContactoEntregaDirecta)
                        .end();
                    $('#tblContactoEntregaDirecta').find('tr').each(function (i) {
                        $(this).find('td').eq(1).after('<td><button class="btn btn-outline-primary fa fa-pencil-square-o" onclick="SeleccionarContactoAEditar(' + i +');"></button></td>');
                        var celdaNombres = document.getElementById("tblContactoEntregaDirecta").rows[i].cells[1];
                        if (typeof (celdaNombres) != 'undefined' && celdaNombres != null) {
                            // Exists.
                            celdaNombres.setAttribute("onclick", "seleccionarContactoEntregaDirecta(" + i + ")");
                        }
                    }); 
                    $("thead tr th.cn_suc").each(function () {
                        this.setAttribute("style", "display:none;");
                    });
                    $("tbody tr td.cn_suc").each(function (i) {
                        this.setAttribute("style", "display:none;");
                        }
                    );
                }
            );
            //Cargar Select en Modal Registrar Contacto
            try {
                request = new Ajax();
                url = urlGetSucursalesJson;
                params = {
                    ccAnalis: $("#cc_analis").val()
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
        bCargado = true;
    });
}
function seleccionarContactoEntregaDirecta(fila) {
    var table = document.getElementById("tblContactoEntregaDirecta");
    //Obteniendo valor Contacto
    var celda = table.rows[fila].cells[1];
    var cn_contacto = celda.innerText;
    cn_contacto = cn_contacto.substring(0, 2);
    //Obteniendo valor Sucursal
    var celda = table.rows[fila].cells[0];
    var cn_suc = celda.innerText;
    try {
        LlenaSeleccionaContactoSelect(cn_contacto,cn_suc);
        $('#PopupGrillaContactoEntregaDirecta').modal("hide");
    } catch (e) {
        alert(e);
    }
}

sContacto = $("#cn_contacto");
function LlenaSeleccionaContactoSelect(cn_contacto, cn_suc) {
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlContactoPorSucursal,
        data: JSON.stringify({
            cn_suc: cn_suc,
            cc_analis: $("#cc_analis").val()
        }),
        dataType: "json",
        success: function (result) {
            var sOption = "<option value=''>Ninguno</option>";
            for (var i = 0; i < result.length; i++) {
                sOption += "<option value='" + result[i].cn_contacto + "'>" + result[i].cd_contacto + "</option>";
            }
            sContacto.html(sOption);
            if (cn_contacto != null) {
                $('#cn_suc option[value=' + cn_suc + ']').attr('selected', 'selected');
                $('#cn_suc option[value=' + cn_suc + ']').prop('selected', 'selected');

                $('#cn_contacto option[value=' + cn_contacto + ']').attr('selected', 'selected');
            }
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

function btnCerrarContactoEntregaDirecta_click() {
    //$('#PopupGrillaContactoEntregaDirecta').modal("toggle");
}

function btnNuevoContactoEntregaDirecta_click() {
    $('#PopupGrillaContactoEntregaDirecta').modal("toggle");
    $('#PopupNuevoContactoEntregaDirecta').modal('show');
    $('#ContactoEntregaDirectaForm')[0].reset();
    $("#EnvioDocumento0").attr("checked", true);
    $("#TipoContacto3").attr("checked", true);
}

function btnGrabarContactoEntregaDirecta_click() {
    if (gsCnSuc.length>0) {//Editar
        ActualizarContacto();
    } else {//Nuevo
        var $Form = $("#ContactoEntregaDirectaForm");
        if ($Form.valid()) {
            try {
                var request = new Ajax();
                var url = urlContactoEntregaDirectaNew;
                var params = {
                    EnvioDocs: $("input:radio[name=EnvioDocumentoContactoEntregaDirecta]:checked").val(),
                    ContAlmacen: $('#ContAlmacen').prop('checked'),
                    ContCobranza: $('#ContCobranza').prop('checked'),
                    ContVenta: $('#ContVenta').prop('checked'),
                    CargoLaboral: $(".cargolaboral-contactoentregadirecta").val(),
                    Email: $(".email-contactoentregadirecta").val(),
                    Telefono2: $(".telefono2-contactoentregadirecta").val(),
                    Telefono: $(".telefono-contactoentregadirecta").val(),
                    //ApeMaterno: $(".apematerno-contactoentregadirecta").val(),
                    //ApePaterno: $(".apepaterno-contactoentregadirecta").val(),
                    Nombres: $(".nombres-contactoentregadirecta").val(),
                    Surcursal: $(".sucursal-contactoentregadirecta").val(),
                    Analisis: $("#cc_analis").val()
                };
                request.JsonPost(url, params, function (response) {
                    if (response.codigo) {
                        $('#PopupNuevoContactoEntregaDirecta').modal("hide");
                        LlenaSeleccionaContactoSelect(response.codigo, $(".sucursal-contactoentregadirecta").val());
                        toastr.success("Contacto registrado.");
                    }
                }).fail(function (data) {
                    OnFailure(data);
                });
            } catch (e) {
                toastr.error(e);
            }
        }
    }
}

function SeleccionarContactoAEditar(fila) {
    var table = document.getElementById("tblContactoEntregaDirecta");
    //Obteniendo valor Contacto
    var celda = table.rows[fila].cells[1];
    var cn_contacto = celda.innerText;
    cn_contacto = cn_contacto.substring(0, 2);
    //Obteniendo valor Sucursal
    var celda = table.rows[fila].cells[0];
    var cn_suc = celda.innerText;
    //Obteniendo valor Cliente
    var cc_analis = $("#cc_analis").val();
    //Cargamos datos para editar y abrir Modal
    LlenarFormularioEditarContacto(cn_suc,cn_contacto,cc_analis);
}

var gsResultado;
function LlenarFormularioEditarContacto(cn_suc, cn_contacto, cc_analis) {
    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlGetContactoEditar,
        data: JSON.stringify({
            cc_analis: cc_analis,
            cn_suc: cn_suc,
            cn_contacto: cn_contacto
        }),
        dataType: "json",
        success: function (result) {

            document.getElementById("titleModalRegistrarContacto").innerText = "Editar contacto";
            for (var i = 0; i < result.length; i++) {
                var ct_nombres = result[i].ct_nombres;
                //var ct_appaterno = result[i].ct_appaterno;
                //var ct_apmaterno = result[i].ct_apmaterno;
                var cargo_laboral = result[i].cd_cargo_laboral;
                var cn_telf1 = result[i].cn_telf1;
                var cn_telf2 = result[i].cn_telf2;
                var cd_email = result[i].cd_email;
                var ContVenta = result[i].cbContVenta;
                var ContCobranza = result[i].cbContCobranza;
                var ContAlmacen = result[i].cbContAlmacen;
                var cb_envio_docum = result[i].cb_envio_docum;

                var form = document.forms['ContactoEntregaDirectaForm'];
                form.elements["Nombres"].value = ct_nombres.trim();
                //form.elements["ApePaterno"].value = ct_appaterno.trim();
                //form.elements["ApeMaterno"].value = ct_apmaterno.trim();
                form.elements["CargoLaboral"].value = cargo_laboral.trim();
                form.elements["Telefono"].value = cn_telf1.trim();
                form.elements["Telefono2"].value = cn_telf2.trim();
                form.elements["Email"].value = cd_email.trim();
                $('#ContVenta').prop('checked', ContVenta);
                $('#ContCobranza').prop('checked', ContCobranza);
                $('#ContAlmacen').prop('checked', ContAlmacen);
                $('input[name=EnvioDocumentoContactoEntregaDirecta][value="' +cb_envio_docum+'"]').prop('checked', true);
                $('#SucursalContactoEntregaDirecta option[value=' + cn_suc + ']').attr('selected', 'selected');

                gsCcAnalis = cc_analis;
                gsCnSuc = cn_suc;
                gsCnContacto = cn_contacto;
            }
            $('#PopupGrillaContactoEntregaDirecta').modal("hide");
            $('#PopupNuevoContactoEntregaDirecta').modal("show");
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}

$(document).ready(function () {
    $('.modal').on("hidden.bs.modal", function (e) {
        if ($('.modal:visible').length) {
            $('body').addClass('modal-open');
        }
    });
});

function ActualizarContacto() {
    var form = document.forms['ContactoEntregaDirectaForm'];
    var inombres= form.elements["Nombres"].value;
    //var iApePaterno = form.elements["ApePaterno"].value;
    //var iApeMaterno = form.elements["ApeMaterno"].value;
    var iCargoLaboral= form.elements["CargoLaboral"].value;
    var iTelefono1= form.elements["Telefono"].value;
    var iTelefono2= form.elements["Telefono2"].value;
    var iEmail = form.elements["Email"].value;

    if (inombres.length == 0) {
        var $validator = $("#ContactoEntregaDirectaForm").validate();
        var error = { Nombres: "Debe ingresar el nombre del contacto" };
        $validator.showErrors(error);
        return false;
    }

    var listaContactos = [
        {
        ct_nombres: inombres,
        //ct_appaterno: iApePaterno,
        //ct_apmaterno: iApeMaterno,
        cd_cargo_laboral: iCargoLaboral,
        cn_telf1: iTelefono1,
        cn_telf2: iTelefono2,
        cd_email: iEmail,
        cbContAlmacen: $('#ContAlmacen').prop('checked'),
        cbContCobranza: $('#ContCobranza').prop('checked'),
        cbContVenta: $('#ContVenta').prop('checked'),
        cb_envio_docum: $("input:radio[name=EnvioDocumentoContactoEntregaDirecta]:checked").val(),
        cc_analis: gsCcAnalis,
        cn_suc: gsCnSuc,
        cn_contacto: gsCnContacto
        }
    ]

    $.ajax({
        destroy: true,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: urlActualizarContacto,
        data: JSON.stringify({listaContactos}),
        dataType: "json",
        success: function (result) {
            toastr.success("Se han actualizado los datos del contacto");
            LlenaSeleccionaContactoSelect(gsCnContacto, gsCnSuc);
            $('#PopupNuevoContactoEntregaDirecta').modal('hide');
        },
        error: function (result) {
            alert("Error en javascript...");
        }
    });
}