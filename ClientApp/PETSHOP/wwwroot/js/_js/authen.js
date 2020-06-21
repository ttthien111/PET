
$(function () {

    var userLogin = JSON.parse(localStorage.getItem("u_login"));
    // check authen to show icon
    if (localStorage.getItem("u_login") != undefined) {
        var v = $("#authen");
        var profile = userLogin.profile;
        v.html("");
        v.html('<span class="row"><a href="#" class="d-flex align-items-center justify-content-center text-white"  style="width:300px;" id="checkLogout"><span style="color: white"><i class="sr-only"></i></span>Loged on: ('+ profile.userProfileFirstName + " " + profile.userProfileMiddleName + " " + profile.userProfileLastName +')</a></span>')
    }
    else {
        var v = $("#authen");
        v.html("");
        v.html('<a href="#" class="d-flex align-items-center justify-content-center text-white" style="width:100px" id="checklogin"><span style="color: white"><i class="sr-only"></i></span>Đăng nhập</a>')
    }

    // login or logout
    $('#authen').click(function () {
        if (localStorage.getItem("u_login") == undefined) {
            $("#loginModal").modal('show');

            $("#btnHideModal").click(function () {
                $("#loginModal").modal('hide');
            });
        }
        else {
            $("#logoutModal").modal('show');

            $("#btnHideModalLogout").click(function () {
                $("#logoutModal").modal('hide');
            });
        }
    });

    // login
    $("#login").click(function () {
        var username = document.getElementById('inputUserName').value;
        var password = document.getElementById('inputPassword').value;
        var loginInformation = {
            'Username': username,
            'Password': password
        }

        $.ajax({
            url: "https://localhost:44380/api/LoginAuthentication/Authenticate",
            type: "POST",
            data: JSON.stringify(loginInformation),
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                var infoLogin = {
                    "AccountUserName": data.accountUserName,
                    "JwToken": data.jwtoken,
                    "AccountId": data.accountId,
                    "Profile": data.userProfile[0]
                }

                // create credential 
                $.ajax({
                    url: "/Auth/Authenticate",
                    type: "POST",
                    data: infoLogin,
                    contentType: "application/json",
                    success: function (data) {
                        
                    },
                    error: function (err) {
                        alert("Login failed");
                        console.log(err);
                    }
                });

                //close modal
                $("#loginModal").modal('hide');

                location.reload();
            },
            error: function (err) {
                alert("Login failed");
                console.log(err);
            }
        });
    });

    // logout

    $("#logout").click(function () {
        localStorage.removeItem("u_login");
        location.reload();
    });

})
