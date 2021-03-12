function GetLocation() {
    if (event.keyCode === 13) {
        var locationCode = $("#RequestModel_LocationCode").val();
        run_waitMe($("#load"), 1, "bounce");
        $("#dvLocationResults").load("/HierarchicalCount/GetLocation/?locationCode=" + locationCode);
        $("#RequestModel_LocationCode").attr("disabled", "disabled");
        $("#btnCloseLocation").removeAttr("disabled");
        $("#btnClearLocation").removeAttr("disabled");
        $("#RequestModel_HierarchyCode").removeAttr("disabled");
        $("#dvLocationResults").removeAttr("hidden");
        $("#load").waitMe("hide");
        $("#RequestModel_HierarchyCode").focus();
    }
}

function AddHierarchicalCount() {
    if (event.keyCode === 13) {
        var locationCode = $("#RequestModel_LocationCode").val();
        var hierarchyCode = $("#RequestModel_HierarchyCode").val();

        if (hierarchyCode.substring(0, 4) === "0018" || hierarchyCode.substring(0, 2) === "01") {
            if (hierarchyCode.length > 19) {
                run_waitMe($("#load"), 1, "bounce");
                $.ajax({
                    type: "POST",
                    url: "/HierarchicalCount/AddHierarchicalCount",
                    data: {
                        locationCode: locationCode,
                        hierarchyCode: hierarchyCode
                    },
                    dataType: "json",
                    success: function (data) {
                        $("#load").waitMe("hide");
                        if (data.Status) {
                            $("#dvLocationResults").load("/HierarchicalCount/GetLocation/?locationCode=" + locationCode);
                        }
                        else {
                            $("#load").waitMe("hide");
                            $.alert({
                                title: "Hata",
                                content: data.Message,
                                icon: "fa fa-exclamation-circle",
                                type: "red",
                                animation: "zoom",
                                closeAnimation: "zoom",
                                buttons: {
                                    okay: {
                                        text: "Kapat",
                                        btnClass: "btn-danger"
                                    }
                                },
                                onClose: function () {

                                }
                            });
                        }
                    },
                    failure: function () {
                        $("#load").waitMe("hide");
                        confirm.ConfirmAlert("Hata", "Server ile bağlantı kesildi.", "red", "btn-red");
                    }
                });
                $("#RequestModel_HierarchyCode").val('');
                $("#RequestModel_HierarchyCode").focus();
                return;
            }
        } else {
            confirm.ConfirmAlert("Hata", "Hatalı kod okutuldu.", "red", "btn-red");
            $("#RequestModel_HierarchyCode").val('');
            $("#RequestModel_HierarchyCode").focus();
            return;
        }


    }
}

function CloseLocation() {
    location.reload();
}

function ClearLocation() {
    var locationCode = $("#RequestModel_LocationCode").val().trim();

    if (locationCode.length === 0) {
        $.alert({
            title: "Hata",
            content: "Lokasyon boş olamaz.",
            icon: "fa fa-exclamation-circle",
            type: "red",
            animation: "zoom",
            closeAnimation: "zoom",
            buttons: {
                okay: {
                    text: "Kapat",
                    btnClass: "btn-danger"
                }
            },
            onClose: function () {
                $("#RequestModel_HierarchyCode").val('');
                $("#RequestModel_HierarchyCode").focus();
            }
        });
        return;
    }

    $.alert({
        title: "Lokasyonu Temizle",
        content: "Lokasyonun sayımları temizlensin mi?",
        icon: "fa fa-check-circle",
        type: "orange",
        animation: "zoom",
        closeAnimation: "zoom",
        buttons: {
            okay: {
                text: "Evet",
                btnClass: "btn-green",
                action: function () {
                    run_waitMe($("#load"), 1, "bounce");

                    $.ajax({
                        type: "POST",
                        url: "/HierarchicalCount/ClearLocation",
                        data: {
                            locationCode: locationCode
                        },
                        dataType: "json",
                        success: function (data) {
                            if (data.Status) {
                                $("#load").waitMe("hide");
                                $.alert({
                                    title: "Başarılı",
                                    content: "Lokasyon sayımları temizlendi.",
                                    icon: "fa fa-check-circle",
                                    type: "green",
                                    animation: "zoom",
                                    closeAnimation: "zoom",
                                    buttons: {
                                        okay: {
                                            text: "Kapat",
                                            btnClass: "btn-green"
                                        }
                                    },
                                    onDestroy: function () {
                                        $("#dvLocationResults").load("/HierarchicalCount/GetLocation/?locationCode=" + locationCode);
                                        $("#RequestModel_HierarchyCode").val('');
                                        $("#RequestModel_HierarchyCode").focus();
                                    }
                                });
                            }
                            else {
                                $("#load").waitMe("hide");
                                confirm.ConfirmAlert("Hata", data.Message, "red", "btn-red");
                                return;
                            }
                        },
                        failure: function () {
                            $("#load").waitMe("hide");
                            confirm.ConfirmAlert("Hata", "Server ile bağlantı kesildi.", "red", "btn-red");
                        }
                    });
                    return;
                }
            },
            cancel: {
                text: "Hayır",
                btnClass: "btn-red",
                action: function () {
                }
            }
        },
        onDestroy: function () {
            $("#RequestModel_HierarchyCode").val('');
            $("#RequestModel_HierarchyCode").focus();
        }
    });

    $("#load").waitMe("hide");
    return;
}