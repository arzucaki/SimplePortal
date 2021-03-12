var confirm = {
    RegisterConfirm: function () {
        confirm.AlertButton();
        confirm.SuccessButton();
        confirm.ConfirmButton();
    },

    AlertButton: function () {
        $(".Alert").click(function () {
            confirm.ConfirmAlert("Hata", $(this).attr("data-content"), "red", "btn-red");
        });
    },
    SuccessButton: function () {
        $(".Success").click(function () {
            confirm.ConfirmAlert("Başarılı", $(this).attr("data-content"), "green", "btn-green");
        });
    },
    ConfirmButton: function () {
        $(".Confirm").click(function () {
            confirm.ConfirmMessage($(this).attr("data-content"), $(this).attr("data-title"));
        });
    },
    ConfirmAlert: function (title, content, type, btnclass, reloadPage = false) {
        $.alert({
            title: title,
            content: content,
            icon: title === "Hata" ? "fa fa-exclamation-circle" : "fa fa-check-circle",
            type: type,
            animation: "zoom",
            closeAnimation: "zoom",
            buttons: {
                okay: {
                    text: "Kapat",
                    btnClass: btnclass
                }
            },
            onClose: function () {
                if (reloadPage) {
                    location.reload();
                }
            }
        });
    },
    ConfirmMessage: function (content, title, callback) {
        $.confirm({
            title: title,
            content: content,
            buttons: {
                EVET: function () {
                    callback();
                },
                HAYIR: function () {
                }
            }
        });
    },
    ConfirmQuestion: function (title, content, data, reloadPage = false) {
        $.confirm({
            title: title,
            content: content,
            buttons: {
                sil: {
                    text: 'Sil',
                    btnClass: 'btn-red',
                    keys: ['enter', 'shift'],
                    action: function () {
                        alert('sil basıldı');
                    }
                },
                iptal: function () {
                    $.alert('iptal basıldı.');
                }
            },
            onClose: function () {
                if (reloadPage) {
                    location.reload();
                }
            }
        });
    }
};

confirm.RegisterConfirm();
