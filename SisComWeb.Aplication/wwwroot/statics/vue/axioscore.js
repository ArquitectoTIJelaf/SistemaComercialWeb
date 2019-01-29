// https://github.com/mzabriskie/axios
var axios_ns = {};

// utilidades
var axios_util = (function () {
    return {
        path: window.location.pathname,
        base: window.location.pathname.substring(0, window.location.pathname.indexOf('/', 1)),        
        getPathParamByObject: function (_data) {
            var _url = "";
            if (Array.isArray(_data)) { // ($.isArray(_data)) // (_data instanceof Array)
                _url += "/" + _data.join("/");
            }
            else {
                for(var key in _data){
                    if(_data.hasOwnProperty(key)){
                        _url += "/" + _data[key];
                    }
                }
            }
            return _url;
        },
        getQueryParamByObject: function(_data) {
            var _querypath = "?";
            for(var key in _data){
                if(_data.hasOwnProperty(key)){
                    _querypath += key + "=" +_data[key] + "&";
                }
            }
            return _querypath.slice(0, -1);
        },
        existInArray: function(_value, _array, _key) {
            var result = false
            for(i=0; i<_array.length; i++)
            {
                if(_array[i][_key] == _value)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    };
})();

axios_ns.CountAjax = 0;

axios_ns.showLoading = function () {
    axios_ns.CountAjax = axios_ns.CountAjax < 0 ? 1 : axios_ns.CountAjax + 1;
    if (axios_ns.CountAjax == 1) {
        $('#loadingpage').show();
    }
};

axios_ns.hideLoading = function () {
    axios_ns.CountAjax = axios_ns.CountAjax - 1;
    if (axios_ns.CountAjax == 0) {
        $('#loadingpage').hide();
    }
};

axios_ns.clearLoading = function () {
    axios_ns.CountAjax = 0;
    $('#loadingpage').hide();
};

axios_ns.VerifyCustomErrorCode = function (res) {
    var rs = true;
    try {
        if (res.status === 403) {
            // var obj = JSON.parse(res.responseText);
            var obj = res.data;
            if (obj.hasOwnProperty("CustomErrorCode")) {
                switch (obj.CustomErrorCode) {
                    case 5001:
                        document.location.href = "";
                        rs = false;
                        break;
                    default:
                        break;
                }
            }
        }
        else if(res.status === 401){
            rs = false;
            alert("No autorizado");
        }
    } catch (e) {
        console.log(e)
    }
    return rs;
};

// constantes
axios_ns.VERB_GET = "GET";
axios_ns.VERB_POST = "POST";
axios_ns.VERB_PUT = "PUT";
axios_ns.VERB_DEL = "DELETE";
axios_ns.CONTENT_TYPE_JSON = "application/json; charset=utf-8";
axios_ns.CONTENT_TYPE_TEXT = "text/plain; charset=utf-8";


axios_ns.req_config = {
    // `url` is the server URL that will be used for the request
    // url: '/user',
    // `baseURL` will be prepended to `url` unless `url` is absolute.
    // baseURL: 'https://some-domain.com/api/',
    // `headers` are custom headers to be sent
    headers: {'X-Requested-With': 'XMLHttpRequest', 'Authorization': "Token " + main_globals.userToken},
    // `timeout` specifies the number of milliseconds before the request times out.
    // If the request takes longer than `timeout`, the request will be aborted.
    timeout: 0,
    // `responseType` indicates the type of data that the server will respond with
    // options are 'arraybuffer', 'blob', 'document', 'json', 'text', 'stream'
    responseType: 'json', // default
};


// definiendo una funcion de error general
axios_ns.handler_error = function(error) {
    if (error.response) {
        console.log("error.response.data: ", error.response.data);
        console.log("error.response.status: ", error.response.status);
        console.log("error.response.headers: ", error.response.headers);
        axios_ns.VerifyCustomErrorCode(error.response);
    } else if (error.request) {
        console.log("error.request: ", error.request);
    } else {
        console.log('error: ', error.message);
    }
    console.log("error.config: ", error.config);    
};

/*
axios.defaults.baseURL = 'https://api.example.com';
axios.defaults.headers.common['Authorization'] = AUTH_TOKEN;
axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
*/



// namespace para instancias de axios
var rq = {};

// instancia sin loading
rq.axios = axios.create(axios_ns.req_config);
// Alter defaults after instance has been created
// rq.axios.defaults.headers.common['Authorization'] = AUTH_TOKEN;
// rq.axios.defaults.timeout = 2500; // Override timeout default for the library
rq.axios.interceptors.request.use(
    function (config) {
        console.log("interceptors.request: ", config);
        return config;
    }, function (error) {
        console.log("interceptors.request.error: ", error);
        return Promise.reject(error);
    });
rq.axios.interceptors.response.use(
    function (response) {
        console.log("interceptors.response: ", response);
        return response;
    }, function (error) {
        console.log("interceptors.response.error: ", error);
        return Promise.reject(error);
    });


// instancia con loading
rq.axios2 = axios.create(axios_ns.req_config);
rq.axios2.interceptors.request.use(
    function (config) {
        axios_ns.showLoading();
        console.log("interceptors.request2: ", config);
        return config;
    }, function (error) {
        axios_ns.hideLoading();
        console.log("interceptors.request.error2: ", error);
        return Promise.reject(error);
    });
rq.axios2.interceptors.response.use(
    function (response) {
        axios_ns.hideLoading();
        console.log("interceptors.response2: ", response);
        return response;
    }, function (error) {
        axios_ns.hideLoading();
        console.log("interceptors.response.error2: ", error);
        return Promise.reject(error);
    });


$(function () {
    // ready
});


/* samples

// Override timeout for this request as it's known to take a long time
rq.axios.get('/longRequest', {
  timeout: 5000
});

rq.axios.get('/user/12345')
  .then(function(response) {
    console.log(response.data);
    console.log(response.status);
    console.log(response.statusText);
    console.log(response.headers);
    console.log(response.config);
  });

// pasando parametros extras al config 'otherprop' y 'othercall', estos son visibles en 'interceptors.request'
rq.axios.get('/path/other', {timeout: 3000, otherprop: 1234, othercall: function(){ console.log("callback..."); } })
        .then(function(response) {
            console.log("response OK: ", response);
        })
        .catch(function (error) {
            if (error.response) {
                console.log("error.response.data: ", error.response.data);
                console.log("error.response.status: ", error.response.status);
                console.log("error.response.headers: ", error.response.headers);
            } else if (error.request) {
                console.log("error.request: ", error.request);
            } else {
                console.log('error: ', error.message);
            }
            console.log("error.config: ", error.config);
        });

// cambiando el 'responseType' que por defecto es 'json', se cambia a text para recibir html, se se pasa 'document' se obtiene el dom
// se usa una funcion para manejar el error
// se pasa '' como url para invocar al 'location.origin + location.pathname'
rq.axios.get('', {timeout: 3000, responseType: 'text' })
        .then(function(response) {
            console.log("response OK: ", response);
        })
        .catch(axios_ns.handler_error);

// se pasa '/' como url para invocar al 'location.origin'
rq.axios.get('/', {timeout: 3000, responseType: 'text' })
        .then(function(response) {
            console.log("response OK: ", response);
        })
        .catch(axios_ns.handler_error);

// llamada a servicio 
rq.axios.get('/api/rest/data/tienda', {timeout: 3000 })
        .then(function(response) {
            console.log("response OK: ", response);
        })
        .catch(axios_ns.handler_error);



// con mismos request no es concurrente
axios.all([ function() {rq.axios2.get('/api/rest/data/tienda');}(), 
            function() {rq.axios2.get('/api/rest/data/tienda');}(),
            function() {rq.axios2.get('/api/rest/data/tienda');}(),
            function() {rq.axios2.get('/api/rest/data/tienda');}(),
            function() {rq.axios2.get('/api/rest/data/tienda');}(),
            function() {rq.axios2.get('/api/rest/data/tienda');}(),
            function() {rq.axios2.get('/api/rest/data/tienda');}()
    ])
    .then(axios.spread(function (p1, p2, p3, p4, p5, p6, p7) { // la cantidad de parametros depende del numero de request que se pase en 'all'
        console.log("ALL OK");
        console.log(p1, p2, p3, p4, p5, p6, p7);
    }))
    .catch(axios_ns.handler_error); // no es necesario, se deberia manejar los catch en cada llamada



// con mismos request no es concurrente
axios.all([ function() {return rq.axios2.get('/api/rest/data/tienda');}(), 
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}()
    ])
    .then(axios.spread(function (p1, p2, p3, p4, p5, p6, p7) {
        console.log("ALL OK");
        console.log(p1, p2, p3, p4, p5, p6, p7);
    }))
    .catch(axios_ns.handler_error); // no es necesario, se deberia manejar los catch en cada llamada



// con diferentes request se hace concurrente
function getMarcas() {
  return rq.axios2.get('/api/rest/data/tienda');
}
function getMarcas2() {
  return rq.axios2.get('/api/rest/data/tienda2');
}
function getMarcas3() {
  return rq.axios2.get('/api/rest/data/tienda3');
}
function getMarcas4() {
  return rq.axios2.get('/api/rest/data/tienda4');
}
function getMarcas5() {
  return rq.axios2.get('/api/rest/data/tienda5');
}
function getMarcas6() {
  return rq.axios2.get('/api/rest/data/tienda6');
}
function getMarcas7() {
  return rq.axios2.get('/api/rest/data/tienda7');
}
axios.all([ getMarcas(), 
            getMarcas2(),
            getMarcas3(),
            getMarcas4(),
            getMarcas5(),
            getMarcas6(),
            getMarcas7()
    ])
    .then(axios.spread(function (p1, p2, p3, p4, p5, p6, p7) {
        console.log("ALL OK");
        console.log(p1, p2, p3, p4, p5, p6, p7);
    }))
    .catch(axios_ns.handler_error); // no es necesario, se deberia manejar los catch en cada llamada



// con diferentes request se hace concurrente
axios.all([ function() {return rq.axios2.get('/api/rest/data/tienda');}(), 
            function() {return rq.axios2.get('/api/rest/account/change/tienda?tienda=108');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/account/change/tienda?tienda=108');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}(),
            function() {return rq.axios2.get('/api/rest/account/change/tienda?tienda=108');}(),
            function() {return rq.axios2.get('/api/rest/data/tienda');}()
    ])
    .then(axios.spread(function (p1, p2, p3, p4, p5, p6, p7) {
        console.log("ALL OK");
        console.log(p1, p2, p3, p4, p5, p6, p7);
    }))
    .catch(axios_ns.handler_error); // no es necesario, se deberia manejar los catch en cada llamada

*/