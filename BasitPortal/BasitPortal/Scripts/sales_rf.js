﻿var server = "http://localhost:50168";
//var server = "http://.:8087";
var http;
var url;
var params;

var PrintShipLabelCheck = false;//Sevk etiketi kullanımı kapatıldı.

window.onload = function () {
    var isMobile = false;
    // device detection
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        isMobile = true;
    }
    document.getElementById("ORDCODE").focus();
};

function GetOrderInfo(e, isOriginalPack) {
    if (event.keyCode === 13) {

        if (!isOriginalPack) {
            document.getElementById("SSCC").value = "";
            document.getElementById("Kod").value = "";
        }

        var loading = document.getElementById('dvLoading');
        loading.removeAttribute('hidden');

        if (window.XMLHttpRequest) // code for IE7+, Firefox, Chrome, Opera, Safari
            http = new XMLHttpRequest();
        else // code for IE6, IE5
            http = new ActiveXObject("Microsoft.XMLHTTP");

        var ordInput = document.getElementById("ORDCODE");
        var ordIDInput = document.getElementById("ORDID");
        var SSCCInput = document.getElementById("SSCC");
        var kodInput = document.getElementById("Kod");
        var btnKarmaKoli = document.getElementById("btnKarmaKoli");
        var dvTable = document.getElementById('dvTable');
        var dvOrderInfo = document.getElementById("dvOrderInfo");

        url = server + "/Sales/GetOrderInfo";
        params = "ordCode=" + ordInput.value;
        http.open("POST", url, true);

        //Send the proper header information along with the request
        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

        http.onreadystatechange = function (e) {//Call a function when the state changes.
            if (http.readyState === 4 && http.status === 200) {
                var dataObject = http.responseText.split("|");

                if (dataObject[0].toString() === "0") {
                    dvTable.innerHTML = dataObject[2].toString();
                    ordIDInput.value = dataObject[3].toString();
                    ordInput.value = ordInput.value.toUpperCase();
                    document.getElementById("OrderInfoCode").innerHTML = dataObject[4].toString();
                    document.getElementById("OrderInfoCustomer").innerHTML = dataObject[5].toString();

                    dvOrderInfo.removeAttribute("hidden");

                    if (!isOriginalPack) {
                        btnKarmaKoli.removeAttribute("disabled");
                        SSCCInput.setAttribute("required", "required");
                        SSCCInput.removeAttribute("disabled");
                        loading.setAttribute('hidden','hidden');
                        SSCCInput.focus();
                    }
                    else {
                        kodInput.setAttribute("required", "required");
                        kodInput.removeAttribute("disabled");
                        loading.setAttribute('hidden', 'hidden');
                        kodInput.focus();
                    }

                    if (PrintShipLabelCheck && dataObject !== null && dataObject[6].toString() === "True" && confirm("Okutma tamamlandı. Sevk Etiketi basılsın mı?")) {
                        PrintShipLabel();
                    }
                }
                else {
                    alert(dataObject[1].toString());
                    ordInput.value = "";
                    loading.setAttribute('hidden', 'hidden');
                    ordInput.focus();
                }
            }
        }
        http.send(params);
    }
}

function CheckSSCC(e) {
    if (event.keyCode === 13) {
        document.getElementById("Kod").value = "";
        var loading = document.getElementById('dvLoading');
        loading.removeAttribute('hidden');

        if (window.XMLHttpRequest) // code for IE7+, Firefox, Chrome, Opera, Safari
            http = new XMLHttpRequest();
        else // code for IE6, IE5
            http = new ActiveXObject("Microsoft.XMLHTTP");

        //var ordIDInput = document.getElementById("OrderInfoCode"); --Novo için
        var ordIDInput = document.getElementById("ORDID");
        var SSCCInput = document.getElementById("SSCC");
        var KodInput = document.getElementById("Kod");

        url = server + "/Sales/CheckSSCC";
        //params = "ORDID=" + ordIDInput.innerHTML + "&SSCC=" + SSCCInput.value; --Novo için
        params = "ORDID=" + ordIDInput.value + "&SSCC=" + SSCCInput.value;
        http.open("POST", url, true);//Send the proper header information along with the request

        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

        http.onreadystatechange = function (e) {//Call a function when the state changes.
            if (http.readyState === 4 && http.status === 200) {
                var dataObject = http.responseText.split("|");
                if (dataObject[0].toString() === "0") {
                    KodInput.setAttribute("required", "required");
                    KodInput.removeAttribute("disabled");
                    loading.setAttribute("hidden", "hidden");
                    KodInput.focus();
                }
                else {
                    alert(dataObject[1].toString().replace("{0}", SSCCInput.value.toString()));
                    KodInput.setAttribute("disabled", "disabled");
                    loading.setAttribute("hidden", "hidden");
                    SSCCInput.value = "";
                    SSCCInput.focus();
                }
            }
        };
        http.send(params);
    }
}

function PrintSSCCLabel() {
    var ordInput = document.getElementById("ORDCODE");
    var SSCCInput = document.getElementById("SSCC");
    var KodInput = document.getElementById("Kod");


    if (window.XMLHttpRequest) // code for IE7+, Firefox, Chrome, Opera, Safari
        http = new XMLHttpRequest();
    else // code for IE6, IE5
        http = new ActiveXObject("Microsoft.XMLHTTP");

    url = server + "/Sales/PrintSSCCCode";
    //url = server + "/Sales/PrintSSCCCodeForNovo";
    params = "ordCode=" + ordInput.value;
    http.open("POST", url, true);//Send the proper header information along with the request

    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function (e) {//Call a function when the state changes.
        if (http.readyState === 4 && http.status === 200) {

            var dataObject = http.responseText.split("|");
            if (dataObject[0].toString() === "0") {
                //alert("Print complete");
                //SSCCInput.value = dataObject[2].toString();
                //KodInput.setAttribute("required", "required");
                //KodInput.removeAttribute("disabled");
                //KodInput.focus();
            }
            else {
                alert(dataObject[1].toString().replace("{0}", SSCCInput.value.toString()));
            }
        }
    };
    http.send(params);
}

function ProcessRead(e, isOriginalPack) {
    if (event.keyCode === 13) {
        var ordInput = document.getElementById("ORDCODE");
        var ordIDInput = document.getElementById("ORDID");
        var SSCCInput = document.getElementById("SSCC");
        var KodInput = document.getElementById("Kod");
        var dvTable = document.getElementById('dvTable');
        var loading = document.getElementById('dvLoading');

        if (window.XMLHttpRequest) // code for IE7+, Firefox, Chrome, Opera, Safari
            http = new XMLHttpRequest();
        else // code for IE6, IE5
            http = new ActiveXObject("Microsoft.XMLHTTP");

        KodInput.setAttribute("disabled", "disabled");
        loading.removeAttribute('hidden');

        if (KodInput.value.length === 1) {
            switch (KodInput.value) {
                case '3':
                    {
                        if (confirm("Siparişin serilerini silmek istediğinize emin misiniz?")) {
                            url = server + "/Sales/DeleteOrderSerials";
                            params = "ordCode=" + ordInput.value;
                            http.onreadystatechange = function () {//Call a function when the state changes.
                                if (http.readyState === 4 && http.status === 200) {
                                    var dataObject = http.responseText.split("|");
                                    if (dataObject[0].toString() === "0") {
                                        dvTable.innerHTML = dataObject[2].toString();
                                        loading.setAttribute("hidden", "hidden");
                                    }
                                    else {
                                        alert(dataObject[1].toString());
                                        loading.setAttribute("hidden", "hidden");
                                    }
                                    KodInput.value = "";
                                    KodInput.removeAttribute("disabled");
                                    KodInput.focus();
                                }
                            }
                        }
                        KodInput.removeAttribute("disabled"); 
                        loading.setAttribute("hidden", "hidden");
                        KodInput.value = "";
                        KodInput.focus();                        
                    } break;
                case '4':
                    {
                        if (!isOriginalPack) {
                            KodInput.removeAttribute("disabled");
                            KodInput.value = "";
                            KodInput.setAttribute("disabled", "disabled");
                            SSCCInput.value = "";
                            loading.setAttribute('hidden', 'hidden')
                            SSCCInput.focus();
                        }
                        else {
                            KodInput.value = "";
                            KodInput.removeAttribute("disabled");
                            alert("Orijinal Koli Okutmasında SSCC değişimi yapılamaz.");
                            loading.setAttribute('hidden', 'hidden')
                            KodInput.focus();
                        }
                    } break;
                case '5':
                    {
                        dvLoading.removeAttribute("hidden");
                        location.reload(); //başka siparişe geçmek için sayfayı refresh etmek yeterli.
                    } break;
                default:
            }
        } else {
            if (KodInput.value.length < 20) {
                alert('Hatalı Kod girildi.');
                KodInput.value = "";
                KodInput.removeAttribute("disabled");
                loading.setAttribute("hidden", "hidden");
                KodInput.focus();
                return;
            }
            url = server + "/Sales/ProcessRead";
            var ssccValue;

            if (KodInput.value.substring(0, 2) === '**') { //bazen karekodların başında da Char(29) geliyor.
                KodInput.value = KodInput.value.substring(2, KodInput.value.length);
            }

            if (isOriginalPack) {
                ssccValue = KodInput.value;
            }
            else {
                ssccValue = SSCCInput.value;
            }

            params = "ordCode=" + ordInput.value + "&serial=" + KodInput.value + "&sscc=" + ssccValue + "&userId=0&isOriginalPack=" + isOriginalPack;
            http.onreadystatechange = function () {//Call a function when the state changes.
                if (http.readyState === 4 && http.status === 200) {
                    var dataObject = http.responseText.split("|");
                    if (dataObject[0].toString() === "0") {
                        dvTable.innerHTML = dataObject[2].toString();
                    }
                    else {
                        alert(dataObject[1].toString());
                    }

                    if (PrintShipLabelCheck && dataObject !== null && dataObject[3].toString() === "True" && confirm("Okutma tamamlandı. Sevk Etiketi basılsın mı?")) {
                        PrintShipLabel();
                    }

                    KodInput.value = "";
                    KodInput.removeAttribute("disabled");
                    loading.setAttribute("hidden", "hidden");
                    KodInput.focus();
                }
            }
        }

        http.open("POST", url, true);//Send the proper header information along with the request

        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

        http.send(params);
    }
}

function PrintShipLabel() {
    var ordID = document.getElementById("ORDID");
    var SSCCInput = document.getElementById("SSCC");

    if (ordID.value === '0') {
        alert("Sipariş seçilmemiş..!");
        return;
    }

    //var http;
    if (window.XMLHttpRequest) // code for IE7+, Firefox, Chrome, Opera, Safari
        http = new XMLHttpRequest();
    else // code for IE6, IE5
        http = new ActiveXObject("Microsoft.XMLHTTP");

    url = server + "/Sales/PrintShipLabel";
    params = "ordID=" + ordID.value;
    http.open("POST", url, true);//Send the proper header information along with the request

    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function (e) {//Call a function when the state changes.
        if (http.readyState === 4 && http.status === 200) {

            var dataObject = http.responseText.split("|");
            if (dataObject[0].toString() === "0") {
                alert("Sevk etiketi basıldı");
            }
            else {
                alert(dataObject[1].toString().replace("{0}", SSCCInput.value.toString()));
            }
        }
    }
    http.send(params);
}