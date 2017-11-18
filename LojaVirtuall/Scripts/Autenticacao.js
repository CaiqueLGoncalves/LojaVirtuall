$(document).ready(function () {
    $("#status").parent().hide();
    $("#botao-entrar").click(function () {
        var login = $("#txtLogin").val();
        var senha = $("#txtSenha").val();

        $.ajax({
            url: "/Autenticacao/AutenticarUsuario",
            data: { Login: login, Senha: senha },
            dataType: "json",
            type: "POST",
            async: true,
            beforeSend: function () {
                $("#status").removeClass("alert-warning");
                $("#status").addClass("alert-info");
                $("#status").html("Autenticando usuário...");
                $("#status").parent().show();
            },
            success: function (dados) {
                if (dados.OK) {
                    $("#status").addClass("alert-success");
                    $("#status").html(dados.Mensagem);
                    $("#status").parent().show();
                    if (dados.Nivel == "Cliente") {
                        setTimeout(function () { window.location.href = "/Home" }, 1000);
                    } else {
                        setTimeout(function () { window.location.href = "/Produtos" }, 1000);
                    }
                } else {
                    $("#status").addClass("alert-warning");
                    $("#status").html(dados.Mensagem);
                    $("#status").parent().show();
                }
            },
            error: function (dados) {
                $("#status").addClass("alert-error");
                $("#status").html(dados.Mensagem);
                $("#status").parent().show();
            }
        });
    });

    $("#logout").click(function () {
        $.ajax({
            url: "/Autenticacao/DesautenticarUsuario",
            dataType: "json",
            type: "POST",
            async: true,
            success: function (dados) {
                if (dados.OK) {
                    window.location.href = "/Home";
                }
            }
        });
    });

    $.ajax({
        url: "/Autenticacao/VerificarAutenticacao",
        dataType: "json",
        type: "POST",
        async: true,
        success: function (dados) {
            if (dados.OK) {
                $(".show-auth").show();
                $(".show-no-auth").hide();

                if (dados.Nivel == "Cliente") {
                    $(".show-admin").hide();
                    $(".show-client").show();
                }

                if (dados.Nivel == "Administrador") {
                    $(".show-client").hide();
                    $(".show-admin").show();
                }
            } else {
                $(".show-auth").hide();
                $(".show-client").hide();
                $(".show-admin").hide();
                $(".show-no-auth").show();
            }
        }
    });
});