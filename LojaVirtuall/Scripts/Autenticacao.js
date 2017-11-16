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
                $("#status").html("Autenticando usuário...");
                $("#status").parent().show();
            },
            success: function (dados) {
                if (dados.OK) {
                    $("#status").html(dados.Mensagem);
                    setTimeout(function () {
                        window.location.href="/Home"
                    }, 10000);
                    $("#status").parent().show();
                } else {
                    $("#status").html(dados.Mensagem);
                    $("#status").parent().show();
                }
            },
            error: function (dados) {
                $("#status").html(dados.Mensagem);
                $("#status").parent().show();
            }
        });
    });
});