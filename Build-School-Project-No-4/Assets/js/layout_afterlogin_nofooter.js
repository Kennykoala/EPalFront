

let html = document.queryCommandValue('html')
let logbtn = document.querySelector('#loginmodal')
let signbtn = document.querySelector('#signupmodal')

let modalbtn = document.querySelectorAll('.navbar-right .modalbutton');
let modalbtnPhone = document.querySelectorAll('.navbar-sm .modalbuttonPhone');

let modalbtnclose = document.querySelector('.modal-btn-close');
let maillog = document.querySelector('.maillog');
let phonelog = document.querySelector('.phonelog');
let mobileicon = document.querySelector('.fa-mobile');
let mailicon = document.querySelector('.fa-envelope');
let logsignBtn = document.querySelectorAll('.loginbutton');
let msgicon = document.querySelectorAll('.message-icon-bk');
let msgiconCl = document.querySelectorAll('.message-icon-cl');
let msgListTitle = document.querySelector('#message-list-title');
let msgClose = document.querySelector('.msg-close');
let msgdropdown = document.querySelector('.dropdown-menu');
let forgetpass = document.querySelector('.forgetpass');
let logsignTabContent = document.querySelectorAll('.logsignTabContent');
let hometab = document.querySelector('#home');
let profiletab = document.querySelector('#profile');
let validate = document.querySelectorAll('.form-group span');


let logsigntab = document.querySelectorAll('.logsign-tab');
let logsigntitle = document.querySelector('.logsign-title');
let modalfooter = document.querySelector('.modalfooter');
let modalfooterP = document.querySelector('.modalfooter>p');
let passinput = document.querySelectorAll('#password');
let passshow = document.querySelectorAll('.passwordshow');


let msgroom = document.querySelector('#dropdown-message');
let searchbar = document.querySelector('.search input');
let statusbar = document.querySelector('.statusbar');
let statusbarPhone = document.querySelector('.navbar-right-phone .statusbar');

let statuslistbtn = document.querySelectorAll('.aside-Menu .dropdown-menu button');
let statuslistbtnPhone = document.querySelectorAll('.navbar-right-phone .aside-Menu .dropdown-menu button');


let email = document.querySelector(".emailinput");

let logsignmodalbtn = document.querySelector('.logsign-modal-button');

let mailerror = document.getElementById('email-error');
let passerror = document.getElementById('myinput-error');
let valerror = document.querySelectorAll(".field-validation-error");


let logoutbtn = document.querySelector('#logoutbutton')




window.onload = function () {

    //google login
    var startApp = function () {
        gapi.load("auth2", function () {
            auth2 = gapi.auth2.init({
                client_id: "1025795679023-8g9j439beq7h92iv9us8nj3d77ifitr7.apps.googleusercontent.com", // 用戶端ID
                cookiepolicy: "single_host_origin"
            });

            //gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);
            //updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
            attachSignin(document.getElementById("GOOGLE_login"));
        });
    };


    function attachSignin(element) {
        auth2.attachClickHandler(element, {},
            // 登入成功
            function (googleUser) {
                // Useful data for your client-side scripts:
                var profile = googleUser.getBasicProfile();

                // The ID token you need to pass to your backend:
                var id_token = googleUser.getAuthResponse().id_token;
                console.log("ID Token: " + id_token);
                var OauthId = profile.getId();
                var OauthName = profile.getName();
                var OauthEmail = profile.getEmail();
                var AuthResponse = googleUser.getAuthResponse(true);//true會回傳access token ，false則不會，自行決定。如果只需要Google登入功能應該不會使用到access token
                //var LoginMethod = "1";

                $.ajax({
                    url: '/Members/GoogleLogin',
                    method: "post",
                    data: {
                        id_token: id_token,
                        OauthId: OauthId,
                        OauthName: OauthName,
                        OauthEmail: OauthEmail,
                        AuthResponse: AuthResponse,
                        //LoginMethod: "1"
                    },
                    success: function (msg) {
                        $("#myModal").modal('hide');

                        console.log(msg);
                        swal.fire({
                            title: "Welcome to Epal",
                            icon: "success",
                            //buttons: true,
                            //dangerMode: true
                        });

                        if (msg == true) {
                            window.location.href = '/'
                        }
                    }
                });//end $.ajax

            },
            // 登入失敗
            function (error) {
                //$("#GOOGLE_STATUS_2").html("");
                alert(JSON.stringify(error, undefined, 2));
                swal.fire({
                    title: "Login Fail",
                    icon: "error",
                    //buttons: true,
                    //dangerMode: true
                });
            });
    }

    startApp();

    // 點擊登入
    $("#GOOGLE_login").click(function () {
        // 進行登入程序
        startApp();
    });









    //FB thirdparty
    window.fbAsyncInit = function () {
        FB.init({
            appId: '241958394640265', // 填入FB APP ID
            cookie: true,
            xfbml: true,
            version: 'v12.0'
        });

        //FB.getLoginStatus(function (response) {
        //    statusChangeCallback(response);
        //});
    };

    // 處理各種登入身份
    function statusChangeCallback(response) {
        console.log('statusChangeCallback/authResponse:');
        console.log(response);

        // 登入 FB 且已加入會員
        if (response.status === 'connected') {
            html = "已登入FB，並加入epal<br/>";

            FB.api('/me?fields=id,name,email', function (response) {

                //console.log('fbapi');
                //console.log(response);


                var fbemail = response.email;
                var fbname = response.name;
                var fbid = response.id;

                FBPassToServer(fbemail, fbname, fbid);
            });

        }

        // 登入 FB, 未偵測到加入會員
        else if (response.status === "not_authorized") {
            //target.innerHTML = "已登入 FB，但未加入epal";
        }

        // 未登入 FB
        else {
            //target.innerHTML = "未登入 FB";
        }
    }


    //資料傳到後端
    function FBPassToServer(fbemail,fbname, fbid) {                      

        var Data = JSON.stringify({
            Fbemail: `${fbemail}`,
            Fbname: `${fbname}`,
            FBId: `${fbid}`
        });

        $.ajax({
            url: '/Members/FBLogin',
            method: 'POST',
            data: Data,
            contentType: 'application/json; charset=utf-8',
            success: function (msg) {
                $("#myModal").modal('hide');

                //console.log(msg);
                swal.fire({
                    title: "Welcome to Epal",
                    icon: "success",
                    //buttons: true,
                    //dangerMode: true
                });


                if (msg == true) {
                    window.location.href = '/'
                }

            },
            error: function (err) {
                console.log(err);

                swal.fire({
                    title: "Login Fail",
                    icon: "error",
                    //buttons: true,
                    //dangerMode: true
                });
            }
        })

    }

    // 點擊登入
    $("#FB_login").click(function () {
        // 進行登入程序
        FB.login(function (response) {
            statusChangeCallback(response);
        }, {
            scope: 'public_profile,email'
        });
    });



    // 載入 FB SDK
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s);
        js.id = id;
        js.src = "https://connect.facebook.net/zh_TW/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));








    //Line Login  
    //建立OAuth 身分驗證頁面並導入
    function AuthWithEmail() {
        var URL = 'https://access.line.me/oauth2/v2.1/authorize?';
        URL += 'response_type=code';
        URL += '&client_id=1656564684';   //TODO:這邊要換成你的client_id
        URL += '&redirect_uri=https://epal-frontstage.azurewebsites.net/Members/LineLoginCallback';   //TODO:要將此redirect url 填回你的 LineLogin後台設定
        URL += '&scope=openid%20profile%20email';
        URL += '&state=abcde';
        window.location.href = URL;
    }
    //Button2 click
    function Button2_click() {
        AuthWithEmail();
    }
    $('#Line_login').click(Button2_click);











    let navItems = document.querySelectorAll('.navItem');
    navItems.forEach(ele => {
        ele.classList.remove('purple-text-border');
        ele.addEventListener('click', function (event) {
            navItems.forEach(e => {
                e.classList.remove('purple-text-border');
            })
            event.srcElement.classList.add('purple-text-border');
        })
    })






    //login / signup modal
    modalbtn.forEach((btn, idx) => {
        logsigntab[idx].classList.remove('logsign-purple-border');

        logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
        modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';

        //$('#loginmodal').trigger('click');


        btn.addEventListener('click', function (event) {
            //初始化modal打開樣式
            logsigntab[idx].classList.add('logsign-purple-border');

            logsigntab[idx].classList.add('active');
            logsignTabContent[idx].classList.add('show', 'active');

            logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
            modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';


            logsigntab.forEach((item, index) => {

                //modal裡面按下不同tab，執行各自的purple border
                item.addEventListener('click', function (event) {

                    item.classList.remove('logsign-purple-border');

                    logsigntab.forEach(e => {
                        e.classList.remove('logsign-purple-border');
                        logsigntitle.innerHTML = index === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
                        modalfooterP.innerHTML = index === 0 ? 'Or log in with' : 'Or sign up with';
                    })
                    event.srcElement.classList.add('logsign-purple-border');

                    //modal關閉後清除purple border
                    modalbtnclose.addEventListener('click', function (event) {
                        item.classList.remove('logsign-purple-border');
                        logsignTabContent[index].classList.remove('show', 'active');
                        logsigntab[index].classList.remove('active');

                    })

                })

            })
        })


    })
    //$('#myModal').modal({ backdrop: 'static', keyboard: false });






    //login / signup modal Phone
    modalbtnPhone.forEach((btn, idx) => {
        logsigntab[idx].classList.remove('logsign-purple-border');

        logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
        modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';

        //$('#loginmodal').trigger('click');


        btn.addEventListener('click', function (event) {
            //初始化modal打開樣式
            logsigntab[idx].classList.add('logsign-purple-border');

            logsigntab[idx].classList.add('active');
            logsignTabContent[idx].classList.add('show', 'active');


            logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
            modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';


            logsigntab.forEach((item, index) => {

                //modal裡面按下不同tab，執行各自的purple border
                item.addEventListener('click', function (event) {
                    // maillog.style.display = 'block';


                    item.classList.remove('logsign-purple-border');

                    logsigntab.forEach(e => {
                        e.classList.remove('logsign-purple-border');
                        logsigntitle.innerHTML = index === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
                        modalfooterP.innerHTML = index === 0 ? 'Or log in with' : 'Or sign up with';

                    })
                    event.srcElement.classList.add('logsign-purple-border');

                    //modal關閉後清除purple border
                    modalbtnclose.addEventListener('click', function (event) {
                        item.classList.remove('logsign-purple-border');
                        logsignTabContent[index].classList.remove('show', 'active');
                        logsigntab[index].classList.remove('active');

                    })

                })

            })
        })


    })
    //$('#myModal').modal({ backdrop: 'static', keyboard: false });


}










//modal password hide/show
passinput.forEach((input, idx) => {

    passshow[idx].addEventListener('click', function () {

        if (input.type === "password") {
            input.type = "text";
            passshow[idx].value = "hide";
        } else {
            input.type = "password";
            passshow[idx].value = "show";
        }
    })

})




//dropdown點選查詢不關閉下拉框
$("body").on('click', '[data-stopPropagation]', function (e) {
    e.stopPropagation();
});



//dropdown-message-ul icon replace
msgicon.forEach((icon, idx) => {
    icon.addEventListener('click', function (event) {
        msgicon[0].src = "/Assets/images/layout/message1.png";
        msgicon[1].src = "/Assets/images/layout/message2.png";
        msgicon[2].src = "/Assets/images/layout/message3.png";

        switch (idx) {
            case 0:
                msgListTitle.innerHTML = "Order Messages";
                icon.src = "/Assets/images/layout/message11.png";
                break;
            case 1:
                msgListTitle.innerHTML = "Social Messages";
                icon.src = "/Assets/images/layout/message21.png";
                break;
            case 2:
                msgListTitle.innerHTML = "System Messages";
                icon.src = "/Assets/images/layout/message31.png";
                break;
            default:
                break;
        }
    })
})


//personal info.  open/close  
function PersonalOpen() {
    document.querySelector(".aside-Menu").style.width = "400px";
    document.querySelector(".aside-Menu").style.right = "0%";
    document.querySelector(".aside-Menu").style.zIndex = "10";
}
function PersonalClose() {
    document.querySelector(".aside-Menu").style.right = "-100%";
}


//personal info.  open/close  Phone
function PersonalOpenPhone() {
    document.querySelector(".navbar-right-phone .aside-Menu").style.width = "100%";
    document.querySelector(".navbar-right-phone .aside-Menu").style.right = "0%";
    document.querySelector(".navbar-right-phone .aside-Menu").style.zIndex = "100";
}
function PersonalClosePhone() {
    document.querySelector(".navbar-right-phone .aside-Menu").style.right = "-100%";
}




//chatroom  open/close
function MsgRoomOpen() {
    document.querySelector(".chat-container").style.display = "block";
}
function MsgRoomClose() {
    document.querySelector(".chat-container").style.display = "none";
}




//gotop
$(function () {
    $('#gotop').click(function () {
        event.preventDefault();
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        return false;
    });

    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('#gotop').fadeIn();
        } else {
            $('#gotop').fadeOut();
        }
    });
});



function changeLong() {
    searchbar.style.width = '250px';
}

function changeShort() {
    searchbar.style.width = '100px';
}




//online/offline change
let linestatusId;
statuslistbtn.forEach((stabtn, idx) => {

    stabtn.addEventListener('click', function () {
        statusbar.innerHTML = stabtn.innerHTML;

        linestatusId = stabtn.value;
        var Data = JSON.stringify({
            //MemberId: `${memberId}`,
            LineStatusId: `${linestatusId}`
        });

        $.ajax({
            url: "/Members/MemberStatus",
            type: "POST",
            data: Data,
            async: true,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            //dataType: "json",
            success: function (res) {
                console.log(res);
            },
            error: function (err) {
                console.log(err);
            }
        });

    })
})


//online/offline change  Phone
statuslistbtnPhone.forEach((stabtn, idx) => {

    stabtn.addEventListener('click', function () {
        statusbarPhone.innerHTML = stabtn.innerHTML;

        linestatusId = stabtn.value;
        var Data = JSON.stringify({
            //MemberId: `${memberId}`,
            LineStatusId: `${linestatusId}`
        });

        $.ajax({
            url: "/Members/MemberStatus",
            type: "POST",
            data: Data,
            async: true,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            //dataType: "json",
            success: function (res) {
                console.log(res);
            },
            error: function (err) {
                console.log(err);
            }
        });

    })
})