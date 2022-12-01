/*
* @Author: Romain Antunes
*/

var authToken = findGetParameter("token");

window.addEventListener ?
    window.addEventListener("load", finishLoad, false) :
    window.attachEvent && window.attachEvent("onload", finishLoad);

function finishLoad() {
    if (authToken) {
        setTimeout(() => {
            window.ui.preauthorizeApiKey("Bearer", "Bearer " + authToken);
        }, 1_000)
    } else {
        console.warn("No token found in URL");
    }
}


function findGetParameter(parameterName) {
    var result = null,
        tmp = [];
    location.search
        .substr(1)
        .split("&")
        .forEach(function (item) {
            tmp = item.split("=");
            if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
        });
    return result;
}