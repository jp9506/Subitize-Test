function ajax(cmd, jsondata) {
    return $.ajax(
        {
            type: "POST",
            url: "webservices.asmx/" + cmd,
            data: jsondata,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
}
function get(cmd, jsondata) {
    return $.ajax(
        {
            type: "POST",
            url: "webservices.asmx/" + cmd,
            data: jsondata,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false
        });
}
function ValidateAuthCode(authcode) {
    var res = false;
    get("ValidateAuthCode", JSON.stringify({ authcode: authcode }))
        .success(function (data, textStatus, jqXHR) {
            res = data.d;
        });
    return res;
}
function ValidateAuthCode_async(authcode) {
    return ajax("ValidateAuthCode", JSON.stringify({ authcode: authcode }));
}
function GetUser(authcode) {
    var res = null;
    get("GetUser", JSON.stringify({ authcode: authcode }))
        .success(function (data, textStatus, jqXHR) {
            res = JSON.parse(data.d);
        });
    return res;
}
function GetUser_async(authcode) {
    return ajax("GetUser", JSON.stringify({ authcode: authcode }));
}
function SetUser(authcode, user) {
    var res = null;
    get("SetUser", JSON.stringify({ authcode: authcode, user: JSON.stringify(user) }))
        .success(function (data, textStatus, jqXHR) {
            res = JSON.parse(data.d);
        });
    return res;
}
function SetUser_async(authcode, user) {
    return ajax("SetUser", JSON.stringify({ authcode: authcode, user: JSON.stringify(user) }));
}
function GetTest(id) {
    var res = null;
    get("GetTest", JSON.stringify({ id: id }))
        .success(function (data, textStatus, jqXHR) {
            res = JSON.parse(data.d);
        });
    return res;
}
function GetTest_async(id) {
    return ajax("GetTest", JSON.stringify({ id: id }));
}
function TakeTest(authcode) {
    var res = null;
    get("TakeTest", JSON.stringify({ authcode: authcode }))
        .success(function (data, textStatus, jqXHR) {
            res = JSON.parse(data.d);
        });
    return res;
}
function TakeTest_async(authcode) {
    return ajax("TakeTest", JSON.stringify({ authcode: authcode }));
}
function HasTest(authcode) {
    var res = false;
    get("HasTest", JSON.stringify({ authcode: authcode }))
        .success(function (data, textStatus, jqXHR) {
            res = data.d;
        });
    return res;
}
function HasTest_async(authcode) {
    return ajax("HasTest", JSON.stringify({ authcode: authcode }));
}