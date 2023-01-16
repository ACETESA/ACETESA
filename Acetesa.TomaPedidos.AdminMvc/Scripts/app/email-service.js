//Envio de correo para Cotizacion y Pedido
var SendEmailService = function () {
    var url;
    var getEmailByCliente = function (idCliente) {
            var $idCliente = $("#" + idCliente);
            idCliente = $.trim($idCliente.val());
            if (idCliente.length === 0) {
                toastr.warning("Debe seleccionar un cliente.");
                return;
            }
            var request = new Ajax();
            var codigo = "";
            var cn_contacto = "";
            var cn_suc = "";
            if (_tipoFormulario === "Pedido") {
                url = urlGetEmailCliente;
                codigo = $("#cn_pedido").val();
            }
            else {
                url = urlGetEmailCliente;
                codigo = $("#cn_proforma").val();
                cn_contacto = $("#cn_contacto").val();
                cn_suc = $("#cn_suc").val();
            }

            var params = {
                id: idCliente,
                tipo: (_tipoFormulario === "Pedido" ? 'P' : 'C'),
                Nro: codigo,
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
    };

    var sendEmail = function (callback) {
        var $Para = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        var $btnCloseModal = $("#btnCloseModal");
        var $enviarMailForm = $("#enviarMailForm");
        if ($enviarMailForm.valid()) {
            $("#sPara").val($Para.val());
            $("#sConCopia").val($ConCopia.val());
            $("#sAsunto").val($Asunto.val());
            $("#sMensaje").val($Mensaje.val());
            $btnCloseModal.trigger("click");
            callback();
        }
    }
    return {
        GetEmailByCliente: getEmailByCliente,
        Send: sendEmail
    };
}
//Envio de correo para Ventas
var SendEmailVentaService = function(){
    var url;
    var getEmailByClienteVenta = function (idCliente,nroDocumento) {
        if (_tipoFormulario === "Venta") {
            url = _baseUrl + "Venta/GetEmailCliente";
            var params = {
                id: idCliente,
                Nro: nroDocumento
            };
            var request = new Ajax();
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
    };
    var sendEmailVenta = function (callback) {
        var $Para = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        var $btnCloseModal = $("#btnCloseModal");
        var $enviarMailForm = $("#enviarMailForm");
        if ($enviarMailForm.valid()) {
            $("#sPara").val($Para.val());
            $("#sConCopia").val($ConCopia.val());
            $("#sAsunto").val($Asunto.val());
            $("#sMensaje").val($Mensaje.val());
            $btnCloseModal.trigger("click");
            callback();
        }
    }
    return {
        GetEmailByClienteVenta: getEmailByClienteVenta,
        Send: sendEmailVenta
    }
}
//Envio de correo para Estado de Cuenta
var SendEmailEstadoCuentaService = function () {
    var url;
    var getEmailByClienteEstadoCuenta = function (idCliente) {
        if (_tipoFormulario === "EstadoCuenta") {
            url = _baseUrl + "EstadoCuenta/GetEmailCliente";
            var params = {
                id: idCliente
            };
            var request = new Ajax();
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
    };
    var sendEmailEstadoCuenta = function (callback) {
        var $Para = $("#Para");
        var $ConCopia = $("#ConCopia");
        var $Asunto = $("#Asunto");
        var $Mensaje = $("#Mensaje");
        var $btnCloseModal = $("#btnCloseModal");
        var $enviarMailForm = $("#enviarMailForm");
        if ($enviarMailForm.valid()) {
            $("#sPara").val($Para.val());
            $("#sConCopia").val($ConCopia.val());
            $("#sAsunto").val($Asunto.val());
            $("#sMensaje").val($Mensaje.val());
            $btnCloseModal.trigger("click");
            callback();
        }
    }
    return {
        GetEmailByClienteEstadoCuenta: getEmailByClienteEstadoCuenta,
        Send: sendEmailEstadoCuenta
    }
}