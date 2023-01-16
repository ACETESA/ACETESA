/// <reference path="../jquery-1.10.2.js" />

var Ajax = function (baseUrl) {

    var _baseUrl = baseUrl == undefined || baseUrl == null ? "" : baseUrl;
    var _nameFunction = null;
    var _argumentsLength = null;
    var _esBloqueado = true;
    if (!window.jQuery) {
        throw ("Se necesita la libreria jQuery.");
    }

    if (!window.jQuery.blockUI) {
        throw ("Se necesita la libreria blockUI.");
    }

    jQuery.ajaxSetup({
        cache: false,
        beforeSend: function (xhr) {
            if (_esBloqueado) {
                jQuery.blockUI({ message: null, overlayCSS: { backgroundColor: '#000', opacity: 0.2, zIndex: 99999 } });
            }
        },
        complete: function (xhr, status) {
            if (_esBloqueado) {
                jQuery.unblockUI();
            }
        }
    });

    var validarParametros = function (url, params, callbackDone) {
        if (_argumentsLength > 0 && _argumentsLength < 6) {
            if (typeof (url) != "string") {
                throw (_nameFunction + "El tipo para el parámetro 'url' debe ser un string.");
            }
            if (typeof (params) != "object") {
                throw (_nameFunction + "El tipo para el parámetro 'params' debe ser un object.");
            }
            if (typeof (callbackDone) != "function") {
                throw (_nameFunction + "El tipo para el parámetro 'callbackDone' debe ser una function.");
            }
        } else {
            throw (_nameFunction + "requiere argumentos.");
        }
    };
    var json = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo Json: ";
        validarParametros(url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.getJSON(url, params).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var jsonPost = function (url, params, callbackDone, bloquear, async) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo JsonPost: ";
        validarParametros(_baseUrl + url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            async: (async == null ? true : async),
            type: 'POST',
            dataType: 'json',
            data: params
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            //alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var jsonPostHeaders = function (url, headers, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo JsonPost: ";
        validarParametros(_baseUrl + url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'POST',
            headers: headers,
            dataType: 'json',
            data: params
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var jsonPostAsync = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo JsonPost: ";
        validarParametros(_baseUrl + url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            data: JSON.stringify(params)
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var jsonPostImage = function (url, idInputFile, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo JsonPostImage: ";
        var image = jQuery("#" + idInputFile);
        if (image.length == 0) {
            throw ("Argumento idInputFile no existe en el documento.");
        }
        if (!image[0].type) {
            throw ("Argumento idInputFile no es de tipo input.");
        }
        if (image[0].type != "file") {
            throw ("Argumento idInputFile no es de tipo input file.");
        }
        url = _baseUrl + url;
        var fileData = image.prop("files")[0];
        var formData = new FormData();
        formData.append("file", fileData);
        return jQuery.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var jsonPostImageIdFacebook = function (url, idInputFile, uid, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo JsonPostImage: ";
        var image = jQuery("#" + idInputFile);
        if (image.length == 0) {
            throw ("Argumento idInputFile no existe en el documento.");
        }
        if (!image[0].type) {
            throw ("Argumento idInputFile no es de tipo input.");
        }
        if (image[0].type != "file") {
            throw ("Argumento idInputFile no es de tipo input file.");
        }
        if (typeof uid != "string") {
            throw ("Argumento uid debe ser de tipo string.");
        }
        url = _baseUrl + url;
        var fileData = image.prop("files")[0];
        var formData = new FormData();
        formData.append("file", fileData);
        formData.append("uid", uid);
        return jQuery.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var htmlPost = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'POST',
            dataType: 'Html',
            data: params
        }).done(function (data) {
            if (callbackDone != null) {
                callbackDone(data);
            }
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var htmlGet = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'GET',
            dataType: 'Html',
            data: params
        }).done(function (data) {
            if (callbackDone != null) {
                callbackDone(data);
            }
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var xmlPost = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo XmlPost: ";
        validarParametros(url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'POST',
            dataType: 'xml',
            data: params
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    var xmlGet = function (url, params, callbackDone, bloquear) {
        _esBloqueado = typeof (bloquear) == "boolean" ? bloquear : true;
        _argumentsLength = arguments.length;
        _nameFunction = "Metodo XmlGet: ";
        validarParametros(url, params, callbackDone);
        url = _baseUrl + url;
        return jQuery.ajax({
            url: url,
            type: 'GET',
            dataType: 'xml',
            data: params
        }).done(function (data) {
            callbackDone(data);
        }).fail(function (xhr) {
            alert("Petición fallida: " + xhr.status + " " + xhr.statusText);
        });
    };
    return {
        Json: json,
        JsonPost: jsonPost,
        JsonPostHeaders : jsonPostHeaders,
        JsonPostAsync:jsonPostAsync,
        JsonPostImage: jsonPostImage,
        JsonPostImageIdFacebook:jsonPostImageIdFacebook,
        HtmlPost: htmlPost,
        HtmlGet: htmlGet,
        XmlPost: xmlPost,
        XmlGet: xmlGet
    };
};