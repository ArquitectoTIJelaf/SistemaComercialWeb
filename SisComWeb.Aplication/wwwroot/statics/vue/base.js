/*
 * Dependencias:
 * - jquery
 * - axios
 * - lodash
 * - Vue
 * - vee-validate
 * - momment
 * - numeral
 * - /js/utils/vue/lang.js
 */

/*!********************* Inicializacion APP */
var APP = APP || {};
//APP.path = ["/"].indexOf(window.location.pathname[window.location.pathname.length - 1]) !== -1 ? window.location.pathname : window.location.pathname + "/";
//if ((window.location.origin).includes("localhost"))
//APP.path = window.location.pathname;
APP.path = ["/"].indexOf(window.location.pathname[window.location.pathname.length - 1]) !== -1 ? window.location.pathname : window.location.pathname + "/";
if ((window.location.origin).includes("localhost"))
    APP.path = window.location.pathname;
APP.base = APP.path.substring(0, APP.path.indexOf('/', 1));
APP.area = APP.path.substring(0, APP.path.indexOf('/', APP.path.indexOf('/', 1) + 1));
APP.ctrl = APP.path.substring(0, APP.path.indexOf('/', APP.path.indexOf('/', APP.path.indexOf('/', 1) + 1) + 1));
if (APP.area === "") {
    APP.area = APP.base;
}
if (APP.ctrl === "") {
    APP.ctrl = APP.path;
}
APP.rest = '/api/rest';
/*!********************* Inicializacion APP */


/*!********************* Funciones Generales */
function formatToChar2(value) {
    var auxValue = parseFloat(value);
    if (!isNaN(auxValue)) {
        return (auxValue <= 9) ? '0' + auxValue : '' + auxValue;
    }
    else return '00';
}

function animateScrollTop(duration = 800) {
    var _heightOfBus = $(".img-bus")[0].getBoundingClientRect().top;
    var _heightOfHeader = $("#appVueNavarHeader")[0].getBoundingClientRect().height;

    if (_heightOfBus !== _heightOfHeader) {
        var _topOfBus = $('.img-bus').offset().top;
        var value = _topOfBus - _heightOfHeader;

        if (!isNaN(value))
            $("html, body").animate({ scrollTop: value }, duration);
    }
}

function jsFilterDosDecimales(_valor) {
    return _valor.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

/*!********************* Prototype Vue */
Vue.prototype.tran = function (key) {
    return APP.tran(key);
};

Vue.prototype.changePagination = function (toPage, fromPage) {
    var _this = this;
    _this.calculatePagination(toPage);
};

Vue.prototype.calculatePagination = function (page) {
    var _this = this;
    _this.pagination.page = page;
    _this.pagination.total = _this.results.length;
    _this.pagination.pagetotal = Math.ceil(_this.pagination.total / _this.pagination.per);
    _this.pagination.desde = (_this.pagination.per * (page - 1)) + 1;
    _this.pagination.hasta = _this.pagination.total > (_this.pagination.per * _this.pagination.page) ? _this.pagination.desde + _this.pagination.per - 1 : _this.pagination.total;
    _this.pagination.view = _this.pagination.pagetotal == page ? (page * _this.pagination.per > _this.pagination.total ? _this.pagination.total : _this.pagination.per) : _this.pagination.per;
    _this.pagination.show = _this.pagination.per > _this.results.length ? _this.results.length : _this.pagination.per;
};

/*!********************* Prototype Vue */
Vue.prototype.$getSelectorVselect = function $obtenerSeleccionadorVselect(_list, _objFiltro) {
    var selector = 0;

    if (_objFiltro) {
        var tmpIndex = _.findIndex(_list, { 'id': _objFiltro.id, 'label': _objFiltro.label });
        selector = (tmpIndex >= 0 ? tmpIndex : 0);
    }

    return selector;
};

/*!********************* Filter Vue */
Vue.filter('concatZero', function (value, length = 8) {
    if (typeof value !== 'undefined') {
        value = '0'.repeat(length) + value;
        return value.substr(value.length - length);
    } else {
        return '';
    }
});

Vue.filter('capitalize', function (value) {
    if (!value) return '';
    value = value.toString();
    return value.charAt(0).toUpperCase() + value.slice(1);
});

Vue.filter('status', function (value) {
    if (!value) return '';
    value = value.toString();
    return value == "I" ? 'INACTIVO' : 'ACTIVO';
});

Vue.filter('todmy', function (value) {
    if (!value) return '';
    return moment(value).format("DD/MM/YYYY");
});

Vue.filter('toymd', function (value) {
    if (!value) return '';
    return moment(value).format("YYYY-MM-DD");
});

Vue.filter('tohhmm', function (value) {
    if (!value) return '';
    return moment(value).format("hh:mm");
});

Vue.filter('todmy2', function (value) {
    if (!value) return '';
    return moment(parseFloat(value)).format("DD/MM/YYYY");
});

Vue.filter('toymd2', function (value) {
    if (!value) return '';
    return moment(parseFloat(value)).format("YYYY-MM-DD");
});

Vue.filter('tohhmm2', function (value) {
    if (!value) return '';
    return moment(parseFloat(value)).format("hh:mm");
});

Vue.filter('year', function (value) {
    if (!value) return '';
    var _date = new Date(value);
    var y = _date.getFullYear();
    return '' + y;
});

Vue.filter('sexo', function (value) {
    if (!value) return '';
    value = value.toString();
    return value === "M" ? 'MASCULINO' : value === "F" ? 'FEMENINO' : '';
});

Vue.filter('diaSemana', function (value) {
    if (!value) return '';
    value = value.toString();
    var dias = ["DOMINGO", "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES", "SABADO"];
    var data = value.split('/');
    var dt = new Date(data[1] + ' ' + data[0] + ', ' + data[2] + ' 12:00:00');
    return dias[dt.getUTCDay()];
});

Vue.filter('estados', function (value) {
    return value ? 'Activo' : 'Inactivo';
});

/*Currency */
Vue.filter("currency", function (value) {
    return numeral(value).format("0,0.00");
});

Vue.filter("currency2", function (value) {
    return numeral(value).format("0,0.0");
});

Vue.filter('money', function (value) {
    if (typeof value !== 'undefined' && value !== '') {
        value = parseFloat(value);
        return 'S/ ' + value.toFixed(2);
    } else {
        return 'S/ 0.00';
    }
});

Vue.filter('fixed2', function (value) {
    if (typeof value !== 'undefined' && value !== '') {
        value = parseFloat(value);
        return String(value.toFixed(2));
    } else {
        return '0.00';
    }
});

Vue.filter('maxLenght', function (value, length) {
    if (typeof value !== 'undefined' && value !== '') {
        var final = value.length <= length ? '' : '...';
        return value.substring(0, length) + final;
    }
});

Vue.filter('hexToRgb', function (value, opacity = 1) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(value);
    return result ? `rgb(${parseInt(result[1], 16)}, ${parseInt(result[2], 16)}, ${parseInt(result[3], 16)}, ${opacity})`
        : 'rgb(255, 255, 255)';
});
/*!********************* Filter Vue */




/*!********************* Validate Vue */
// messagesVeeValidate se define en lang.js
const locale = {
    name: 'es',
    messages: messagesVeeValidate,
    attributes: {}
};

if (typeof VeeValidate !== 'undefined' && VeeValidate && typeof VeeValidate.Validator) {
    VeeValidate.Validator.addLocale(locale);
}

const dictionary = {
    es: {
        messages: {
            required: function (value) {
                console.log(value);
                if (value == "email") {
                    return "Ingrese su dirección de correo electrónico.";
                }
                else if (value.toLocaleLowerCase() === "login" || value.toLocaleLowerCase() === "usuario") {
                    return "Ingrese Usuario.";
                }
                else if (value.toLocaleLowerCase() === "password" || value.toLocaleLowerCase() === "password2") {
                    return "Ingrese Contraseña.";
                }
                else {
                    return "El campo " + value + " es obligatorio.";
                }
            },
            email: function (a) {
                return "Ingrese una dirección de correo válida";
            }
        }
    }
};

VeeValidate.Validator.updateDictionary(dictionary);

Vue.use(VeeValidate, {
    locale: 'es'
});
/*!********************* Validate Vue */



/*!********************* Messages */
APP.msg = {};
APP.msg.info = function (msg) {
    $.toast({
        heading: 'Mensaje de Información',
        text: msg,
        position: 'top-right',
        loaderBg: '#ff6849',
        icon: 'info',
        hideAfter: 3500,
        stack: 1
    });
};

APP.msg.success = function (msg) {
    $.toast({
        heading: 'Mensaje de Confirmación',
        text: msg,
        position: 'top-right',
        loaderBg: '#FAF14E',
        icon: 'success',
        hideAfter: 3500,
        stack: 1
    });
};

APP.msg.warning = function (msg) {
    $.toast({
        heading: 'Mensaje de Advertencia',
        text: msg,
        position: 'top-right',
        loaderBg: '#FAF14E',
        icon: 'warning',
        hideAfter: 3500,
        stack: 1
    });
};

APP.msg.error = function (msg) {
    $.toast({
        heading: 'Mensaje de Error',
        text: msg,
        position: 'top-right',
        loaderBg: '#FAF14E',
        icon: 'error',
        hideAfter: 3500,
        stack: 1
    });
};

APP.msg.errorWithoutTime = function (msg) {
    $.toast({
        heading: 'Sesión Expirada',
        text: msg,
        position: 'top-right',
        loaderBg: '#FAF14E',
        icon: 'error',
        hideAfter: false,
        stack: 1
    });
};

APP.msg.ShowSaveAfterSale = async function (_title, _message) {
    var auxBool = false;

    await swal.fire({
        title: _title || "Buen trabajo!",
        text: _message || "Hiciste clic en el botón!",
        type: "success"
    }).then(res => {
        if (res.value) auxBool = res.value;
    }).catch(error => {
        APP.msg.error(error);
    });

    return auxBool;
};

APP.msg.confirm = async function (_title, _message, _textButtonConfirm, _textButtonCancel, _colorOfButton) {
    var _bool = false;

    await swal.fire({
        title: _title || "Mensaje del sistema",
        text: _message || "¿Desea Continuar con la operación?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: _colorOfButton || "#DD6B55",
        confirmButtonText: _textButtonConfirm || "Si",
        cancelButtonText: _textButtonCancel || "No",
        allowOutsideClick: false
    }).then(res => {
        if (res.value) _bool = res.value;
        }).catch(error => {
            APP.msg.error(error);
        });

    return _bool;
};

APP.msg.infoMessage = async function (_title, _message) {
    var _bool = false;

    await swal({
        title: _title || "Mensaje del sistema",
        text: _message || "¿Desea Continuar con la operación?",
        type: "info",
        allowOutsideClick: false
    }).then(res => {
        if (res.value)
            _bool = res.value;
        }).catch(error => {
            APP.msg.error(error);
        });

    return _bool;
};

APP.msg.successMessage = async function (_title, _message) {
    var _bool = false;

    await Swal.fire({  
        title: _title || 'Mensaje del sistema',
        text: _message || 'El proceso se realizó de forma correcta',
        type: 'success',
        allowOutsideClick: false
    }).then(res => {
        if (res.value)
            _bool = res.value;
        }).catch(error => {
            APP.msg.error(error);
        });

    return _bool;
};

APP.msg.errorMessage = async function (_title, _message) {
    var _bool = false;

    await Swal.fire({
        title: _title || 'Mensaje del sistema',
        text: _message || 'Ocurrió un Error. Verifique',
        type: 'error',
        allowOutsideClick: false
    }).then(res => {
        if (res.value)
            _bool = res.value;
        }).catch(error => {
            APP.msg.error(error);
        });

    return _bool;
};

APP.msg.confirmClaveAutorizacion = async function (_title, _message, _tipo, _textButtonConfirm, _textButtonCancel, _colorOfButton) {
    var _bool = false;

    await swal({
        title: _title || "Mensaje del sistema",
        text: _message || "¿Desea Continuar con la operación?",
        input: 'password',
        inputValidator: (value) => {
            return !value && '¡Ingrese clave!';
        },
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: _colorOfButton || "#DD6B55",
        confirmButtonText: _textButtonConfirm || "Si",
        cancelButtonText: _textButtonCancel || "No",
        allowOutsideClick: false,
        showLoaderOnConfirm: true,
        preConfirm: async function (value) {
            switch (_tipo) {
                case 'CanRes_148':
                    {
                        if (ClaveCancelarReserva && value === ClaveCancelarReserva)
                            _bool = true;
                    }
                    break;
                case 'Rei_226':
                    {
                        if (value === ('F' + moment().format('MM') + moment().format('DD') + 'C'))
                            _bool = true;
                    }
                    break;
                default:
                    {
                        var resSendClaveAutorizacion = await appController.sendClaveAutorizacion(value, _tipo);
                        if (resSendClaveAutorizacion && resSendClaveAutorizacion.EsCorrecto)
                            _bool = true;
                    }
                    break;
            }

            if (!_bool)
                Swal.showValidationMessage('Usuario no autorizado o clave incorrecta.');
        }
    }).then(res => {

    }).catch(error => {
        APP.msg.error(error);
    });

    return _bool;
};

APP.msg.confirmAperturaProgramacion = async function (_title, _message, _textButtonConfirm, _textButtonCancel, _colorOfButton) {
    var _option = '';

    var inputOptions = new Promise(function (resolve) {
        resolve({
            'true':
                'Solo para vender' + '<br />' +
                '<p style="font-weight: 400; font-size: 12px; text-align: justify; " >' +
                'Después de realizar todas las ventas tendrá que re-imprimir el manifiesto pero esto será con el mismo número de manifiesto.</p>',
            'false':
                'General' + '<br />' +
                '<p style="font-weight: 400; font-size: 12px; text-align: justify; " >' +
                'Después de realizar todas las ventas tendrá que re-imprimir el manifiesto pero esto será con un nuevo número de manifiesto.</p>'
        });
    });

    await swal({
        title: _title || "Mensaje del sistema",
        text: _message || "",
        input: 'radio',
        inputOptions: inputOptions,
        inputValidator: (value) => {
            return !value && '¡Seleccione una opción!';
        },
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: _colorOfButton || "#DD6B55",
        confirmButtonText: _textButtonConfirm || "Si",
        cancelButtonText: _textButtonCancel || "No",
        allowOutsideClick: false,
        width: '42em'
    }).then(res => {
        _option = res.value;
    }).catch(error => {
        APP.msg.error(error);
    });

    return _option;
};

APP.msg.infoBoletoPromocionadoConDescuento = async function (_title, _message, _objAsiento) {
    var _bool = false;

    await swal({
        title: _title || "Mensaje del sistema",
        html:
            '<p style="font-weight: 300; font-size: 13px; text-align: justify; line-height: normal; " >' +
            _message +
            '</p>' +
            '<table style="text-align: left; display: unset; ">' +
            '<tr><td>Número de Boleto</td><td> : ' + _objAsiento.NroBoleto + '</td></tr>' +
            '<tr><td>Origen de Boleto</td><td> : ' + _objAsiento.OrigenBoleto + '</td></tr>' +
            '<tr><td>Destino de Boleto</td><td> : ' + _objAsiento.DestinoBoleto + '</td></tr>' +
            '<tr><td>Número de Asiento</td><td> : ' + _objAsiento.NroAsiento + '</td></tr>' +
            '</table>'
        ,
        type: "info",
        allowOutsideClick: false
    }).then(res => {
        if (res.value)
            _bool = res.value;
    }).catch(error => {
        APP.msg.error(error);
    });

    return _bool;
};

APP.msg.confirmClaveFechaActualCodiUsuario = async function (_title, _message, _codiUsuario, _textButtonConfirm, _textButtonCancel, _colorOfButton) {
    var _bool = false;

    await swal({
        title: _title || "Mensaje del sistema",
        text: _message || "¿Desea Continuar con la operación?",
        input: 'password',
        inputValidator: (value) => {
            return !value && '¡Ingrese clave!';
        },
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: _colorOfButton || "#DD6B55",
        confirmButtonText: _textButtonConfirm || "Si",
        cancelButtonText: _textButtonCancel || "No",
        allowOutsideClick: false,
        showLoaderOnConfirm: true,
        preConfirm: async function (value) {
            if (value === moment().format('DD') + moment().format('MM') + String(_codiUsuario))
                _bool = true;
            else
                Swal.showValidationMessage('Clave incorrecta.');
        }
    }).then(res => {

    }).catch(error => {
        APP.msg.error(error);
    });

    return _bool;
};

/*!********************* Messages */

/*!********************* Configuracion AJAX GENERAL */
APP.rq = {};
APP.rq.CountAjax = 0;

APP.rq.showLoading = function () {
    APP.rq.CountAjax = APP.rq.CountAjax < 0 ? 1 : APP.rq.CountAjax + 1;
    if (APP.rq.CountAjax === 1) {
        $(".preloader").fadeIn().css("background-color", "rgba(0, 0, 0, 0.3)");
    }
};

APP.rq.hideLoading = function () {
    APP.rq.CountAjax = APP.rq.CountAjax - 1;
    if (APP.rq.CountAjax === 0) {
        $(".preloader").fadeOut().css("background-color", "");
    }
};

APP.rq.clearLoading = function () {
    APP.rq.CountAjax = 0;
    $(".preloader").fadeOut().css("background-color", "");
};

// para verificar el codigo de error de la respuesta ajax
APP.rq.VerifyCustomErrorCode = function (res) {
    var rs = true;
    try {
        if (res.status === 403) {
            var obj = res.data;
            if (obj.hasOwnProperty("CustomErrorCode")) {
                switch (obj.CustomErrorCode) {
                    case 5001:
                        document.location.href = "";
                        rs = false;
                        break;
                    default:
                        console.log("Codigo de error desconocido");
                }
            }
        }
        else if (res.status === 401) {
            rs = false;
            alert("No autorizado");
        }
    } catch (e) {
        console.log(e);
    }
    return rs;
};

// para verificar si el error es por expiracion de session
APP.rq.checkIfErrorSession = function (res) {
    var rs = false;
    try {
        if (res.status === 403 || res.status === 401) {
            var obj = res.data;
            if (obj.hasOwnProperty("CustomErrorCode")) {
                switch (obj.CustomErrorCode) {
                    case 5001:
                        rs = true;
                        break;
                    default:
                        console.log("Codigo de error desconocido");
                }
            }
        }
    } catch (e) {
        console.log(e);
    }
    return rs;
};
/*!********************* Configuracion AJAX GENERAL */




/*!********************* Configuracion AXIOS */
APP.rq.axios_config = {
    headers: { 'X-Requested-With': 'XMLHttpRequest', 'Authorization': "Token " + main_globals.userToken },
    timeout: 0,
    responseType: 'json'
};

// definiendo una funcion de error general
APP.rq.axios_handler_error = function (error) {
    if (error.response) {
        console.log("error.response.data: ", error.response.data);
        console.log("error.response.status: ", error.response.status);
        console.log("error.response.headers: ", error.response.headers);
        APP.rq.VerifyCustomErrorCode(error.response);
    } else if (error.request) {
        console.log("error.request: ", error.request);
    } else {
        console.log('error: ', error.message);
    }
    console.log("error.config: ", error.config);
};

APP.rq.axios_handler_error_alert = function (error) {
    if (error.response && APP.rq.checkIfErrorSession(error.response)) {
        APP.msg.errorWithoutTime("Su sessión ha expirado, por favor inicie sesión de nuevo <a href='{0}'>aquí</a> o presione F5.".format(APP.base));
    } else {
        APP.msg.error("Tuvimos inconvenientes al realizar su solicitud.");
    }
    console.log(error);
};

// instancia de axios con loading
APP.rq.axios = axios.create(APP.rq.axios_config);
APP.rq.axios.interceptors.request.use(
    function (config) {
        APP.rq.showLoading();
        console.log("APP interceptors.request: ", config);
        return config;
    }, function (error) {
        APP.rq.hideLoading();
        console.log("APP interceptors.request.error: ", error);
        return Promise.reject(error);
    });
APP.rq.axios.interceptors.response.use(
    function (response) {
        APP.rq.hideLoading();
        console.log("APP interceptors.response: ", response);
        return response;
    }, function (error) {
        APP.rq.hideLoading();
        console.log("APP interceptors.response.error: ", error);
        return Promise.reject(error);
    });

// instancia de axios sin loading
APP.rq.axios2 = axios.create(APP.rq.axios_config);
APP.rq.axios2.interceptors.request.use(
    function (config) {
        console.log("APP interceptors.request2: ", config);
        return config;
    }, function (error) {
        console.log("APP interceptors.request.error2: ", error);
        return Promise.reject(error);
    });
APP.rq.axios2.interceptors.response.use(
    function (response) {
        console.log("APP interceptors.response2: ", response);
        return response;
    }, function (error) {
        console.log("APP interceptors.response.error2: ", error);
        return Promise.reject(error);
    });
/*!********************* Configuracion AXIOS */




/*!******************** Configuracion AJAX JQUERY */
// constantes
APP.rq.VERB_GET = "GET";
APP.rq.VERB_POST = "POST";
APP.rq.VERB_PUT = "PUT";
APP.rq.VERB_DEL = "DELETE";
APP.rq.CONTENT_TYPE_JSON = "application/json; charset=utf-8";
APP.rq.CONTENT_TYPE_TEXT = "text/plain; charset=utf-8";

// _type = "GET" || "POST"
// _dataType = "xml" || "json" || "script" || "html" // tipo de contenido que se recibe del server
// _contentType = "application/x-www-form-urlencoded; charset=UTF-8" || "application/json; charset=utf-8" || "text/plain; charset=utf-8" // tipo de contenido que se envia al server
// _cache = true || false
// _async = true || false

APP.rq.common = function (_url, _data, _type, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async) {

    if (typeof _url === "undefined" || _url === null || _url === "" || typeof _url !== "string") _url = ""; // location.href;
    if (typeof _data === "undefined" || _data === null || _data === "" || (typeof _data !== "string" && typeof _data !== "object")) _data = undefined;
    if (typeof _type === "undefined" || _type === null || _type === "" || typeof _type !== "string") _type = "GET";
    if (typeof _async === "undefined" || _async === null || _async === "" || typeof _async !== "boolean") _async = true;
    if (typeof _cache === "undefined" || _cache === null || _cache === "" || typeof _cache !== "boolean") _cache = false;
    if (typeof _contentType === "undefined" || _contentType === null || _contentType === "" || typeof _contentType !== "string") _contentType = "application/x-www-form-urlencoded; charset=UTF-8";
    if (typeof _dataType === "undefined" || _dataType === null || _dataType === "" || typeof _dataType !== "string") _dataType = undefined; // "*";

    $.ajax({
        // url: APP.base + "/" + _url,
        url: _url,
        data: _data,
        type: _type,
        async: _async,
        cache: _cache,
        contentType: _contentType,
        dataType: _dataType,
        success: function (data) {
            if ((typeof _success !== "undefined" || _success !== null) && typeof _success === "function") {
                _success(data);
            }
            else {
                console.log(data);
                return null;
            }
        },
        error: function (request, status, error) {
            if (APP.rq.VerifyCustomErrorCode(request)) {
                if ((typeof _error !== "undefined" || _error !== null) && typeof _error === "function") {
                    _error(request, status, error);
                }
                else {
                    console.log(request.status + " - " + request.statusText);
                }
            }
        },
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Token " + main_globals.userToken);
            if ((typeof _before !== "undefined" || _before !== null) && typeof _before === "function") {
                _before(request);
            }
            else {
                APP.rq.showLoading();
            }
        },
        complete: function (request, status) {
            if ((typeof _complete !== "undefined" || _complete !== null) && typeof _complete === "function") {
                _complete(request, status);
            }
            else {
                APP.rq.hideLoading();
            }
        }
    });
};

APP.rq.get = function (_url, _data, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_GET, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async);
};

APP.rq.get_json = function (_url, _data, _success, _error, _before, _complete, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_GET, _success, _error, _before, _complete, "json", APP.rq.CONTENT_TYPE_JSON, _cache, _async);
};

APP.rq.get_json2 = function (_url, _data, _success, _error, _before, _complete, _cache, _async) {
    if (Array.isArray(_data)) { // ($.isArray(_data)) // (_data instanceof Array)
        _url += "/" + _data.join("/");
    }
    else {
        for (var key in _data) {
            if (_data.hasOwnProperty(key)) {
                _url += "/" + _data[key];
            }
        }
    }
    APP.rq.common(_url, {}, APP.rq.VERB_GET, _success, _error, _before, _complete, "json", APP.rq.CONTENT_TYPE_JSON, _cache, _async);
};

APP.rq.post = function (_url, _data, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_POST, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async);
};

APP.rq.post_json = function (_url, _data, _success, _error, _before, _complete, _cache, _async) {
    APP.rq.common(_url, JSON.stringify(_data), APP.rq.VERB_POST, _success, _error, _before, _complete, "json", APP.rq.CONTENT_TYPE_JSON, _cache, _async);
};

APP.rq.put = function (_url, _data, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_PUT, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async);
};

APP.rq.put_json = function (_url, _data, _success, _error, _before, _complete, _cache, _async) {
    APP.rq.common(_url, JSON.stringify(_data), APP.rq.VERB_PUT, _success, _error, _before, _complete, "json", APP.rq.CONTENT_TYPE_JSON, _cache, _async);
};

APP.rq.del = function (_url, _data, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_DEL, _success, _error, _before, _complete, _dataType, _contentType, _cache, _async);
};

APP.rq.del_json = function (_url, _data, _success, _error, _before, _complete, _cache, _async) {
    APP.rq.common(_url, JSON.stringify(_data), APP.rq.VERB_DEL, _success, _error, _before, _complete, "json", APP.rq.CONTENT_TYPE_JSON, _cache, _async);
};

APP.rq.script = function (_url, _data, _success, _error, _before, _complete, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_GET, _success, _error, _before, _complete, "script", _contentType, _cache, _async);
};

APP.rq.html = function (_url, _data, _success, _error, _before, _complete, _contentType, _cache, _async) {
    APP.rq.common(_url, _data, APP.rq.VERB_GET, _success, _error, _before, _complete, "html", _contentType, _cache, _async);
};
/*!******************** Configuracion AJAX JQUERY */




/*!********************* Utils general */
APP.util = (function () {
    return {
        getPathParamByObject: function (_data) {
            var _url = "";
            if (Array.isArray(_data)) { // ($.isArray(_data)) // (_data instanceof Array)
                _url += "/" + _data.join("/");
            }
            else {
                for (var key in _data) {
                    if (_data.hasOwnProperty(key)) {
                        _url += "/" + _data[key];
                    }
                }
            }
            return _url;
        },
        getQueryParamByObject: function (_data) {
            var _querypath = "?";
            for (var key in _data) {
                if (_data.hasOwnProperty(key)) {
                    _querypath += key + "=" + _data[key] + "&";
                }
            }
            return _querypath.slice(0, -1);
        },
        existInArray: function (_value, _array, _key) {
            var result = false;
            for (i = 0; i < _array.length; i++) {
                if (_array[i][_key] == _value) {
                    result = true;
                    break;
                }
            }
            return result;
        },
        getMarcaByCodigo: function (_lista, _codigo) {
            if (_codigo == "") {
                return -1;
            }
            var obj = _.find(_lista, { 'value': _codigo });
            return obj ? obj.id : -1;
        },
        getInfoUsuario: function () {
            var rs = {
                'id': $("#hdnIdUsuario").val(),
                'nom': $("#hdnUserName").val()
            };
            return rs;
        },
        getInfoCliente: function () {
            var rs = {
                'id': '',
                'codigo': '',
                'cliente': '',
                'marca': 0,
                'nom': ''
            };
            var $obj;
            var id = '', nom = '';
            if ($("#ddlCliente").length) {
                $obj = $("#ddlCliente").find(":selected");
                id = $obj.val();
                nom = $obj.text().trim();
            } else {
                $obj = $("#lblCliente");
                id = $obj.data('id');
                nom = $obj.text().trim();
            }
            if ($obj) {
                rs.id = id;
                rs.codigo = $obj.data('codigo');
                rs.cliente = $obj.data('cliente');
                rs.marca = $obj.data('marca');
                rs.nom = nom;
            }
            return rs;
        },
        getInfoMarca: function () {
            var rs = {
                'id': '0',
                'nom': ''
            };
            var $obj;
            var id = '', nom = '';
            if ($("#ddlMarca").length) {
                $obj = $("#ddlMarca").find(":selected");
                id = $obj.val();
                nom = $obj.text();
            } else if ($("#ddlMarca2").length) { // Por contacto de obra en administracion de usuarios
                $obj = $("#ddlMarca2").find(":selected");
                id = $obj.val();
                nom = $obj.text();
            } else {
                $obj = $("#lblMarca");
                id = $obj.data('id');
                nom = $obj.data('nom');
            }
            if ($obj) {
                rs.id = id;
                rs.nom = nom;
            }
            return rs;
        },
        getInfoObra: function () {
            var rs = {
                'id': '0',
                'nom': ''
            };
            var $obj;
            var id = '', nom = '';
            if ($("#ddlObra").length) {
                $obj = $("#ddlObra").find(":selected");
                id = $obj.val();
                nom = $obj.text();
            } else {
                $obj = $("#lblObra");
                id = $obj.data('id');
                nom = $obj.data('nom');
            }
            if ($obj) {
                rs.id = id;
                rs.nom = nom;
            }
            return rs;
        },
        getFiltroObras: function (_callback, _appVue, _isCache) {
            var _this = this;
            var _url = "";
            var _raw = [];
            var _filtro = [];
            var idMarca = _appVue.filtros.marca;
            if (idMarca === "" || idMarca == "0") {
                var urls = [];
                var lenMarcas = _appVue.list.marcas.length;
                for (var i = 0; i < lenMarcas; i++) {
                    if (_appVue.list.marcas[i].value && _appVue.list.marcas[i].value !== "0") {
                        _url = _this.getUrlObrasByUserCliente(_appVue.list.marcas[i].value, _isCache);
                        urls.push(_url);
                    }
                }
                if (urls) {
                    // let promiseMarcas = urls.map(url => APP.rq.axios.get(url));
                    var promiseMarcas = urls.map(function (url) { return APP.rq.axios.get(url) });
                    console.log("promiseMarcas", promiseMarcas);
                    axios.all(promiseMarcas)
                        .then(function (results) {
                            // let allMarcas = results.map(r => r.data);
                            var allMarcas = results.map(function (r) { return r.data });
                            console.log("allMarcas: ", allMarcas, JSON.stringify(allMarcas));
                            // var result = _.unionBy(allMarcas[0], allMarcas[1], "value");
                            var result = _.union.apply(_, allMarcas);
                            result = _.orderBy(result, ['label'], ['asc']);
                            console.log("result: ", result, JSON.stringify(result));
                            _this.buildFiltroObras(result, _callback, _raw, _filtro, _appVue);
                        })
                        .catch(APP.rq.axios_handler_error_alert);
                }
            } else {
                _url = _this.getUrlObrasByUserCliente(idMarca, _isCache);
                APP.rq.axios.get(_url)
                    .then(function (response) {
                        _this.buildFiltroObras(response.data, _callback, _raw, _filtro, _appVue);
                    })
                    .catch(APP.rq.axios_handler_error_alert);
            }
        }
    };
})();


APP.util.getParameterByName = function (_name, _url) {
    _name = _name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + _name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(_url || document.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
};

APP.util.removeURLParameter = function (_param, _url) {
    var url = _url || document.location.href;
    var urlparts = url.split('?');
    if (urlparts.length >= 2) {

        var prefix = encodeURIComponent(_param) + '=';
        var pars = urlparts[1].split(/[&;]/g);

        for (var i = pars.length; i-- > 0;) {
            if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                pars.splice(i, 1);
            }
        }

        url = urlparts[0] + '?' + pars.join('&');
        return url;
    } else {
        return url;
    }
};

APP.util.VerifyBrowserSupportInputByType = function (value) {
    var i = document.createElement("input");
    i.setAttribute("type", value);
    return i.type !== "text";
};

APP.util.getInternetExplorerVersion = function () {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
};

APP.util.checkVersion = function () {
    var msg = "You're not using Internet Explorer.";
    var ver = APP.util.getInternetExplorerVersion();

    if (ver > -1) {
        if (ver >= 6.0)
            msg = "You're using a recent copy of Internet Explorer.";
        else
            msg = "You should upgrade your copy of Internet Explorer.";
    }
    alert(msg);
};

APP.util.endsWith = function (str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
};

APP.util.countProperties = function (obj) {
    if (APP.util.getInternetExplorerVersion() == -1) {
        return Object.keys(obj).length;
    }
    else {
        var count = 0;
        for (var k in obj) {
            if (obj.hasOwnProperty(k)) {
                ++count;
            }
        }
        return count;
    }
};

APP.util.getFileName = function (path_file) {
    var reg = new RegExp(/([^\/\\]+)$/);
    var result = reg.exec(path_file);
    return result != null ? result[0] : result;
};

APP.util.getFileSizeMB = function (id) {
    try {
        var fileSize = 0;
        var scale = 1048576; // 1024*1024
        if (APP.util.getInternetExplorerVersion() > -1 && APP.util.getInternetExplorerVersion() <= 9) {
            fileSize = 2;
        }
        else
            if (APP.util.getInternetExplorerVersion() > 9) {
                var objFSO = new ActiveXObject("Scripting.FileSystemObject");
                var filePath = $("#" + id)[0].value;
                var objFile = objFSO.getFile(filePath);
                if (typeof (objFile) !== "undefined") fileSize = objFile.size / scale;
                //fileSize = 1000;
            }
            else {
                if (typeof ($("#" + id)[0].files[0]) !== "undefined") fileSize = $("#" + id)[0].files[0].size / scale;
            }

        return fileSize;
    }
    catch (e) {
        console.log("Error en obtener el tamaño del archivo :" + e);
        return 0;
    }
};

APP.util.getCookie = function (name) {
    var r = document.cookie.match("\\b" + name + "=([^;]*)\\b");
    //return r ? r[1] : undefined;
    return r ? r[1] : "";
};

APP.util.createCookie = function (name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    //else expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
};

APP.util.readCookie = function (name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
};

APP.util.deleteCookie = function (name) {
    APP.util.createCookie(name, "", -1);
};

APP.util.getPeriod = function (date) {
    try {
        return date.getFullYear() + "" + (("" + (date.getMonth() + 1)).length === 1 ? "0" + (date.getMonth() + 1) : "" + (date.getMonth() + 1));
    } catch (e) {
        console.log(e);
        return "";
    }
};

APP.util.getCurrentPeriod = function () {
    var date = new Date();
    return APP.util.getPeriod(date);
};

// para verificar si un objeto diccionario o array esta vacio
APP.util.isEmpty = function (obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop))
            return false;
    }
    return true;
};

// verificar si un objeto tiene una propiedad y si es del valor pasado
APP.util.checkProp = function (obj, prop, value) {
    if (typeof value === "undefined") {
        return obj.hasOwnProperty(prop);
    }
    else {
        return obj.hasOwnProperty(prop) && obj[prop] == value;
    }
};

APP.util.validateEmail = function (_email, _re) {
    return _re.test(_email);
};

// dates utils
APP.util.addMonthToDate = function (date, months) {
    date.setMonth(date.getMonth() + months);
    return date;
};

Date.prototype.formatDDMMYYYY = function () {
    return (("" + this.getDate()).length === 1 ? "0" + (this.getDate()) : (this.getDate())) +
        "/" + (("" + (this.getMonth() + 1)).length === 1 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1)) +
        "/" + this.getFullYear();
};

Date.prototype.formatDDMMYYYY2 = function () {
    return (("" + this.getDate()).length === 1 ? "0" + (this.getDate()) : (this.getDate())) +
        "-" + (("" + (this.getMonth() + 1)).length === 1 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1)) +
        "-" + this.getFullYear();
};

Date.prototype.formatDDMMYYYY3 = function () {
    return (("" + this.getDate()).length === 1 ? "0" + (this.getDate()) : (this.getDate())) +
        "-" + (("" + (this.getMonth() + 1)).length === 1 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1)) +
        "-" + (this.getFullYear() + "").substr(2, 2);
};

Date.prototype.formatYYYYMMDD = function () {
    return this.getFullYear() +
        "-" + (("" + (this.getMonth() + 1)).length === 1 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1)) +
        "-" + (("" + this.getDate()).length === 1 ? "0" + (this.getDate()) : (this.getDate()));
};

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/\{(\d+)\}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
}


APP.util.timerTemClassId = 0;

(function ($) {

    $.fn.extend({
        addTempClass: function (className, duration) {
            var elements = this;
            APP.util.timerTemClassId = setTimeout(function () {
                elements.removeClass(className);
            }, duration);

            return this.each(function () {
                $(this).addClass(className);
            });
        }
    });

})(jQuery);
/*!********************* Utils general */




/*!********************* Utils datatable */
APP.dt = {};

APP.dt.dataTableSpanish = {
    "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando _START_ al _END_ de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando 0 al 0 de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Filtro:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    }
}

APP.dt.buildDataTable = function (_idTb, _obj) {
    if (typeof _obj !== "undefined" && typeof _obj === "object") {
        _obj.col = typeof _obj.col === "undefined" ? 0 : _obj.col;
        _obj.ord = typeof _obj.ord === "undefined" ? 'desc' : _obj.ord; // desc
        _obj.len = typeof _obj.len === "undefined" ? 10 : _obj.len;
        _obj.search = typeof _obj.search === "undefined" ? true : _obj.search;
        _obj.buttons = typeof _obj.buttons === "undefined" ? false : _obj.buttons;
        _obj.bLengthChange = typeof _obj.bLengthChange === "undefined" ? true : _obj.bLengthChange;
        _obj.bPaginate = typeof _obj.bPaginate === "undefined" ? true : _obj.bPaginate;
        _obj.info = typeof _obj.info === "undefined" ? true : _obj.info;
    } else {
        _obj = {};
        _obj.col = 0;
        _obj.ord = 'desc';
        _obj.len = 10;
        _obj.search = true;
        _obj.buttons = false;
        _obj.bLengthChange = true;
        _obj.bPaginate = true;
        _obj.info = true;
    }

    var arrButtons = ['copy', 'csv', 'print']; //, 'excel', 'pdf', 'pageLength'

    $(_idTb).dataTable({
        "language": APP.dt.dataTableSpanish,
        "order": [[_obj.col, _obj.ord]],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false
        }],
        "pageLength": _obj.len,
        "searching": _obj.search,
        "bLengthChange": _obj.bLengthChange,
        "bPaginate": _obj.bPaginate,
        "info": _obj.info,
        "autoWidth": false,
        "dom": 'Blfrtip', // con 'Blfrtip' sale el selector de tamaño de pagina - 'Bfrtip' se oculta el selector, para setar el boton 'pageLength'
        lengthMenu: [
            [10, 25, 50, 100], // [ 10, 25, 50, 100, -1 ],
            ['10', '25', '50', '100'] // [ '10 rows', '25 rows', '50 rows', '100 rows', 'Show all' ]
        ],
        "buttons": _obj.buttons ? arrButtons : [],
        "initComplete": function (settings) {
            $(_idTb).wrap("<div class='table-responsive'></div>");
            /* agregado para corregir estilos con los botones de exportacion e impresion */
            // $(_idTb + "_filter").after("<div style='clear:both;'></div>");
            $(_idTb).closest(".table-responsive").before("<div style='clear:both;'></div>");
        }
    });
};

APP.dt.renderDataTable = function (_appVue, _data, _obj) {
    var _idTb = _appVue.getIdTable();
    $(_idTb).dataTable().fnDestroy();
    _appVue.setResult(_data);
    if (!APP.util.isEmpty(_data)) {
        _appVue.$nextTick(function () { // con nextTick espera hasta que termine de renderizar
            APP.dt.buildDataTable(_idTb, _obj);
        });
    } else {
        APP.msg.error("No se encontro resultados.");
    }
};

/*!********************* Utils datatable */




/*!********************* Utils handlebar */
APP.hbs = {};
APP.hbs.getCompiledTemplate = function (key, html) {
    if (typeof APP.hbs.compTemplates === "undefined" || typeof APP.hbs.compTemplates[key] === "undefined") {
        if (typeof APP.hbs.compTemplates === "undefined") {
            APP.hbs.compTemplates = {};
        }
        APP.hbs.compTemplates[key] = Handlebars.compile(html);
    }
    return APP.hbs.compTemplates[key];
};

APP.hbs.renderTemplate = function (_selector, _template, _data) {
    if (typeof _data === "undefined") {
        _data = [];
    }
    $(_selector).html("");
    var templatehtml = $(_template).html();
    // var sourcehtml = Handlebars.compile(templatehtml);
    var sourcehtml = APP.hbs.getCompiledTemplate(_template, templatehtml);
    $(_selector).html(sourcehtml({ ListData: _data }));
};

APP.hbs.renderTemplateGen = function (_selector, _template, _context) {
    $(_selector).html("");
    var templatehtml = $(_template).html();
    // var sourcehtml = Handlebars.compile(templatehtml);
    var sourcehtml = APP.hbs.getCompiledTemplate(_template, templatehtml);
    $(_selector).html(sourcehtml(_context));
};

APP.hbs.getTemplate = function (_template, _context) {
    var templatehtml = $(_template).html();
    // var sourcehtml = Handlebars.compile(templatehtml);
    var sourcehtml = APP.hbs.getCompiledTemplate(_template, templatehtml);
    return sourcehtml(_context);
};

APP.hbs.renderTbBodyList = function (_idTb, _idTpl, _data, _obj) {
    $(_idTb).dataTable().fnDestroy();
    APP.hbs.renderTemplate(_idTb + " tbody", _idTpl, _data);
    APP.hbs.buildTable(_idTb, _idTpl, _data, _obj);
};

APP.hbs.renderTbList = function (_idTb, _idTpl, _data, _obj) {
    $(_idTb).dataTable().fnDestroy();
    APP.hbs.renderTemplate(_idTb, _idTpl, _data);
    APP.hbs.buildTable(_idTb, _idTpl, _data, _obj);
};

APP.hbs.buildTable = function (_idTb, _idTpl, _data, _obj) {
    if (typeof _obj !== "undefined" && typeof _obj === "object") {
        _obj.col = typeof _obj.col === "undefined" ? 0 : _obj.col;
        _obj.ord = typeof _obj.ord === "undefined" ? 'asc' : _obj.ord; // desc
        _obj.len = typeof _obj.len === "undefined" ? 25 : _obj.len;
        _obj.search = typeof _obj.search === "undefined" ? false : _obj.search;
    } else {
        _obj = {};
        _obj.col = 0;
        _obj.ord = 'asc';
        _obj.len = 25;
        _obj.search = false;
    }

    if (!APP.util.isEmpty(_data)) {
        $(_idTb).dataTable({
            "language": APP.hbs.dataTableSpanish,
            "order": [[_obj.col, _obj.ord]],
            "pageLength": _obj.len,
            "searching": _obj.search,
            "autoWidth": false,
            "dom": 'Blfrtip', // con 'Blfrtip' sale el selector de tamaño de pagina - 'Bfrtip' se oculta el selector, para setar el boton 'pageLength'
            lengthMenu: [
                [10, 25, 50, 100], // [ 10, 25, 50, 100, -1 ],
                ['10', '25', '50', '100'] // [ '10 rows', '25 rows', '50 rows', '100 rows', 'Show all' ]
            ],
            "buttons": [
                'copy', 'csv', 'print' //, 'excel', 'pdf', 'pageLength'
            ],
            "initComplete": function (settings) {
                $(_idTb).wrap("<div class='table-responsive'></div>");
                /* agregado para corregir estilos con los botones de exportacion e impresion */
                // $(_idTb + "_filter").after("<div style='clear:both;'></div>");
                $(_idTb).closest(".table-responsive").before("<div style='clear:both;'></div>");
            }
        });
    } else {
        APP.msg.error("No se encontro resultados");
    }
};

APP.hbs.dataTableSpanish = {
    "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando _START_ al _END_ de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando 0 al 0 de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Filtro:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    }
};

Handlebars.registerHelper('lang', function (key) {
    var result = APP.tran(key);
    return new Handlebars.SafeString(result);
});

Handlebars.registerHelper('first', function (context, block) {
    return block(context[0]);
});

Handlebars.registerHelper('getVirtualPath', function () {
    var result = main_globals.virtualPath;
    return new Handlebars.SafeString(result);
});

Handlebars.registerHelper('getUserName', function () {
    var result = main_globals.userFullname;
    return new Handlebars.SafeString(result);
});

Handlebars.registerHelper('verifyTour', function (flag, cod, val1, val2) {
    if (flag) {
        return new Handlebars.SafeString(val1 + cod);
    }
    else {
        return new Handlebars.SafeString(val2 + cod);
    }
});

Handlebars.registerHelper('equal', function (lvalue, rvalue, options) {
    if (arguments.length < 3)
        throw new Error("Handlebars Helper equal needs 2 parameters");
    if (lvalue != rvalue) {
        return options.inverse(this);
    } else {
        return options.fn(this);
    }
});

Handlebars.registerHelper("inc", function (value, options) {
    return parseInt(value) + 1;
});

Handlebars.registerHelper('ifCond', function (v1, operator, v2, options) {

    switch (operator) {
        case '==':
            return (v1 == v2) ? options.fn(this) : options.inverse(this);
        case '===':
            return (v1 === v2) ? options.fn(this) : options.inverse(this);
        case '<':
            return (v1 < v2) ? options.fn(this) : options.inverse(this);
        case '<=':
            return (v1 <= v2) ? options.fn(this) : options.inverse(this);
        case '>':
            return (v1 > v2) ? options.fn(this) : options.inverse(this);
        case '>=':
            return (v1 >= v2) ? options.fn(this) : options.inverse(this);
        case '&&':
            return (v1 && v2) ? options.fn(this) : options.inverse(this);
        case '||':
            return (v1 || v2) ? options.fn(this) : options.inverse(this);
        default:
            return options.inverse(this);
    }
});

Handlebars.registerHelper("prettifyDate", function (timestamp) {
    var d = new Date(timestamp.replace('T', ' '));
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    return curr_date + '/' + curr_month + '/' + curr_year;
});

Handlebars.registerHelper("prettifyDateHour", function (timestamp) {
    var d = new Date(timestamp.replace('T', ' '));
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var curr_hour = d.getHours();
    var curr_minute = d.getMinutes();
    return d.format("dd/mm/yy h:MM TT");

});
/*!********************* Utils handlebar */




/*!********************* Document ready */
$(function () {

    if ($('.dp').length) {
        $('.dp').datetimepicker({
            keyBinds: {
                up: null,
                down: null,
                left: null,
                right: null
            },
            useCurrent: false,
            locale: 'es',
            showTodayButton: false,
            showClose: false,
            showClear: false,
            keepOpen: false,
            toolbarPlacement: 'top',
            format: 'DD/MM/YYYY',
            tooltips: {
                today: 'Ir a la fecha actual',
                clear: 'Borrar fecha',
                close: 'Cerrar selector'
            }
        });
    }
});
/*!********************* Document ready */

/*!********************* Vue Autocomplete*/

const Autocomplete = {
    name: "autocomplete",
    template: `
        <div class="autocomplete">
            <input  type="text"
                    class="form-control input-sm"
                    autocomplete="off"
                    v-model="search.Descripcion"
                    @input="onChange"
                    @keyup.down="onArrowDown"
                    @keyup.up="onArrowUp"
                    @keydown.prevent.13="onEnter(search)"
                    @keydown.prevent.9="onEnter(search)"
                    @keydown.8="onDelete(search)"
                    @blur="onBlur()"
                    :id = "a_id"
                    :disabled = "a_disabled"
                    :placeholder = "a_pholder"
                    :style = "a_style" />
            <ul v-show="isOpen" id="autocomplete-results" class="autocomplete-results">
                <li v-if="isLoading" class="loading"> Buscando... </li>
                <li v-else v-for="(result, i) in results"
                            class="autocomplete-result"
                            @mousedown="setResult(result, $event)"
                            :key="i"
                            :class="{ 'is-active': i === arrowCounter }"> {{result.Descripcion}} </li>
            </ul>
        </div>`,
    props: {
        items: {
            required: false,
            default: () => []
        },
        isAsync: {
            type: Boolean,
            required: false,
            default: false
        },
        a_id: '',
        a_disabled: '',
        a_pholder: '',
        a_style: '',
        a_default: {}
    },
    data() {
        return {
            isOpen: false,
            results: [],
            search: {
                Codigo: '',
                Descripcion: ''
            },
            isLoading: false,
            arrowCounter: 0
        };
    },
    methods: {
        onChange() {
            this.$emit("input", this.search);
            this.$emit("changedata", this.search.Descripcion);

            if (this.isAsync) this.isLoading = true;
            else this.filterResults();
        },
        filterResults() {
            this.results = this.items.filter(item => {
                return item.Descripcion.toLowerCase().indexOf(this.search.Descripcion.toLowerCase()) > -1;
            });
            if (this.search.Descripcion.length !== 0) this.isOpen = true;
        },
        setResult(result, e) {
            e.stopPropagation();
            this.search = result;
            this.$emit("result", this.search);
            this.isOpen = false;
            this.arrowCounter = 0;
            this.results = [];
        },
        onArrowDown() {
            if (this.arrowCounter < this.results.length - 1) this.arrowCounter = this.arrowCounter + 1;
            else this.arrowCounter = 0;
        },
        onArrowUp() {
            if (this.arrowCounter > 0) this.arrowCounter = this.arrowCounter - 1;
            else this.arrowCounter = this.results.length - 1;
        },
        onEnter(_search) {
            if (_search.Codigo === '' && _search.Descripcion === '') return;
            this.search = this.results[this.arrowCounter] || _search;
            this.$emit("result", this.search);
            this.isOpen = false;
            this.arrowCounter = 0;
            this.results = [];
        },
        onDelete(_search) {
            if (this.search.Codigo !== undefined) {
                this.search = {
                    Codigo: '',
                    Descripcion: ''
                };
                this.$emit("result", this.search);
                this.search.Descripcion = _search.Descripcion;
            }
            this.isOpen = false;
        },
        onBlur() {
            this.isOpen = false;
        },
        handleClickOutside(e) {
            if (!this.$el.contains(e.target)) {
                this.isOpen = false;
                this.arrowCounter = 0;
                this.search = this.a_default;
            }
        }
    },
    watch: {
        items: function (val) {
            this.arrowCounter = 0;
            this.results = val;
            this.isLoading = false;
        }
    },
    mounted() {
        document.addEventListener("click", this.handleClickOutside);
    },
    destroyed() {
        document.removeEventListener("click", this.handleClickOutside);
    }
};
/*!********************* Vue Autocomplete*/

/*********************** Vue Mask *****/

Vue.use(VueMask.VueMaskPlugin);
/*********************** Vue Mask *****/

/*********************** Vue Select *****/

Vue.component('v-select', VueSelect.VueSelect);
/*!********************* Vue Select *****/

/*************** Variables Generales ****/
APP.ventas = {};
APP.ventas.colorBloqueoInterno = '#FFC36D';
APP.ventas.colorBloqueoExterno = '#607D8B';
APP.ventas.colorReserva = '#DD0707';
APP.ventas.colorPaseCortesia = '#FF6DC6';
APP.ventas.colorDegrade = '#FFFFFF';
/****************************************/


/********************** Vue directive *****/

//Vue.directive('numericOnly', function (el, binding, vnode) {
//    el.type = 'number';
//    el.addEventListener('input', (e) => {
//        return el.validity.valid || (el.value = '');
//    });
//    el.addEventListener('keypress', (e) => {
//        let charCode = (e.which) ? e.which : e.keyCode;
//        if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
//            e.preventDefault();
//        } else {
//            return true;
//        }
//    });
//});


Vue.directive('numb-or-lett', function (el, binding, vnode) {
    el.addEventListener('keypress', (e) => {
        var inp = String.fromCharCode(e.keyCode);
        if (/[a-zA-Z0-9]/.test(inp))
            return true;
        else 
            e.preventDefault();
    });
});

//Vue.directive('only-name', function (el, binding, vnode) {
//    el.addEventListener('keypress', (e) => {
//        var inp = String.fromCharCode(e.keyCode);
//        if (/[a-zA-ZçáàãâéèêíìóòõôúùüñÇÁÀÃÂÉÈÊÍÌÓÒÕÔÚÙÜÑ´,'. ]/.test(inp))
//            return true;
//        else
//            e.preventDefault();
//    });

//    if (el.value)
//        el.value = el.value.match(/[a-zA-ZçáàãâéèêíìóòõôúùüñÇÁÀÃÂÉÈÊÍÌÓÒÕÔÚÙÜÑ,'. ]/gi).join("");
//});

//Vue.directive('only-digit', function (el, binding, vnode) {
//    el.addEventListener('keypress', (e) => {
//        var inp = String.fromCharCode(e.keyCode);
//        if (/[0-9.]/.test(inp))
//            return true;
//        else
//            e.preventDefault();
//    });

//    if (el.value) {
//        var coincidence = el.value.match(/[0-9.]/gi);
//        if (coincidence)
//            el.value = coincidence.length > 1 ? coincidence.join('') : coincidence;
//        else
//            el.value = '';
//    }
//});

//Vue.directive('money', function (el, binding, vnode) {
//    el.addEventListener('input', (e) => {
//        var valid = /^[1-9]{1}\d{0,2}(\.\d{0,2})?$/.test(el.value), val = el.value;
//        if (!valid)
//            el.value = val.substring(0, val.length - 1);
//    });
//});

/**************************************/

/*********************** Utils cte */
APP.cte = [];

APP.cte.Accion = {};
APP.cte.Accion.Anulacion = '03';
APP.cte.Accion.Reintegro = '10';
/*!********************* Utils cte */