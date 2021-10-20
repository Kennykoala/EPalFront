

let html = document.queryCommandValue('html')
let logbtn = document.querySelector('#loginmodal')
let signbtn = document.querySelector('#signupmodal')
let modalbtn = document.querySelectorAll('.modalbutton');
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
let statuslistbtn = document.querySelectorAll('.aside-Menu .dropdown-menu button');


let email = document.querySelector(".emailinput");
/*let password = document.querySelector(".passwordinput");*/
let logsignmodalbtn = document.querySelector('.logsign-modal-button');

let mailerror = document.getElementById('email-error');
let passerror = document.getElementById('myinput-error');
let valerror = document.querySelectorAll(".field-validation-error");
//let isRequestAuthenticated = ' @Request.IsAuthenticated';

let logoutbtn = document.querySelector('#logoutbutton')




window.onload = function () {

    //google進行登入程序，使用gapi.auth2.init()方法的client_id參數指定應用程序的客戶端ID
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


    //function updateSigninStatus(isSignedIn) {
    //    if (isSignedIn) {
    //        document.getElementById("loginmodal").style.display = 'none';
    //        document.getElementById("logoutbutton").style.display = 'block';
    //        //makeApiCall();
    //    } else {
    //        document.getElementById("loginmodal").style.display = 'block';
    //        document.getElementById("logoutbutton").style.display = 'none';
    //    }
    //}


    function attachSignin(element) {
        auth2.attachClickHandler(element, {},
            // 登入成功
            function (googleUser) {
                // Useful data for your client-side scripts:
                var profile = googleUser.getBasicProfile();
                console.log("ID: " + profile.getId()); // Don't send this directly to your server!
                console.log('Full Name: ' + profile.getName());
                console.log('Given Name: ' + profile.getGivenName());
                console.log('Family Name: ' + profile.getFamilyName());
                console.log("Image URL: " + profile.getImageUrl());
                console.log("Email: " + profile.getEmail());

                // The ID token you need to pass to your backend:
                var id_token = googleUser.getAuthResponse().id_token;
                console.log("ID Token: " + id_token);
                var OauthId = profile.getId();
                var OauthName = profile.getName();
                var OauthEmail = profile.getEmail();
                var AuthResponse = googleUser.getAuthResponse(true);//true會回傳access token ，false則不會，自行決定。如果只需要Google登入功能應該不會使用到access token
                var LoginMethod = "1";

                $.ajax({
                    url: '/Members/GoogleLogin',
                    method: "post",
                    data: {
                        id_token: id_token,
                        OauthId: OauthId,
                        OauthName: OauthName,
                        OauthEmail: OauthEmail,
                        AuthResponse: AuthResponse,
                        LoginMethod: "1"
                    },
                    success: function (msg) {
                        $("#myModal").modal('hide');
                        //$("#loginmodal").style.display="none";
                        //$("#signupmodal").style.display="none";


                        console.log(msg);
                        swal.fire({
                            title: "Welcome to Epal",
                            icon: "success",
                            //buttons: true,
                            //dangerMode: true
                        });

                        return User.Identity.IsAuthenticated; 

                        //logbtn.style.display="none";
                        //signbtn.style.display = "none";
                        //logoutbtn.style.display = "block";

                    }
                });//end $.ajax


                ////v1
                //var profile = googleUser.getBasicProfile(),
                //    $target = $("#GOOGLE_STATUS_2"),
                //    html = "";

                //html += "ID: " + profile.getId() + "<br/>";
                //html += "會員暱稱： " + profile.getName() + "<br/>";
                //html += "會員頭像：" + profile.getImageUrl() + "<br/>";
                //html += "會員 email：" + profile.getEmail() + "<br/>";
                //html += "id token：" + googleUser.getAuthResponse().id_token + "<br/>";
                //html += "access token：" + googleUser.getAuthResponse(true).access_token + "<br/>";
                //$target.html(html);
            },
            // 登入失敗
            function (error) {
                $("#GOOGLE_STATUS_2").html("");
                alert(JSON.stringify(error, undefined, 2));
            });
    }

    startApp();

    // 點擊登入
    $("#GOOGLE_login").click(function () {
        // 進行登入程序
        startApp();
    });



    //// 點擊登出
    //$("#GOOGLE_logout").click(function () {
    //    var auth2 = gapi.auth2.getAuthInstance();
    //    auth2.signOut().then(function () {
    //        // 登出後的動作
    //        $("#GOOGLE_STATUS_2").html("");
    //    });
    //});











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



    //logsigntab[0].classList.add('logsign-purple-border');
    /*    $('#myModal').modal('show');*/




    //login / signup modal
    modalbtn.forEach((btn, idx) => {
        logsigntab[idx].classList.remove('logsign-purple-border');

        //logsignTabContent[idx].classList.remove('show', 'active');
        //logsigntab[idx].classList.remove('active');    

        logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
        //modalfooter.style.display = idx === 0 ? 'flex' : 'none';
        modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';

        //if (!isRequestAuthenticated)
        $('#loginmodal').trigger('click');


        btn.addEventListener('click', function (event) {
            //初始化modal打開樣式
            logsigntab[idx].classList.add('logsign-purple-border');

            logsigntab[idx].classList.add('active');
            logsignTabContent[idx].classList.add('show', 'active');


            logsigntitle.innerHTML = idx === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
            //modalfooter.style.display = idx === 0 ? 'flex' : 'none';
            modalfooterP.innerHTML = idx === 0 ? 'Or log in with' : 'Or sign up with';

            //if (idx === 0) { logsignBtn[0].value = "Log In"; }
            //else if (idx === 1) { logsignBtn[1].value = "Sign Up"; }

            //maillog.style.display = 'block';

            //if (idx == 0) {
            //    validate[0].innerHTML = "";
            //    validate[1].innerHTML = "";
            //}
            //else if (idx == 1) {
            //    validate[2].innerHTML = "";
            //    validate[3].innerHTML = "";
            //}



            logsigntab.forEach((item, index) => {


                //if ( (idx == 0 && index == 0) || (idx == 1 && index == 0) ) {
                //    hometab.style.display = "block";
                //    profiletab.style.display = "none";
                //}
                //else if ((idx == 1 && index == 1) || (idx == 0 && index == 1)) {
                //    hometab.style.display = "none";
                //    profiletab.style.display = "block";
                //}

                //modal裡面按下不同tab，執行各自的purple border
                item.addEventListener('click', function (event) {
                    // maillog.style.display = 'block';

                    ////登入驗證errormsg清除
                    //document.querySelectorAll(".field-validation-error").forEach(item => {
                    //    item.innerText = "";
                    //})

                    //if ((idx == 0 && index == 0) || (idx == 1 && index == 0)) {
                    //    hometab.style.display = "block";
                    //    profiletab.style.display = "none";
                    //}
                    //else if ((idx == 1 && index == 1) || (idx == 0 && index == 1)) {
                    //    hometab.style.display = "none";
                    //    profiletab.style.display = "block";
                    //}


                    item.classList.remove('logsign-purple-border');

                    logsigntab.forEach(e => {
                        e.classList.remove('logsign-purple-border');
                        logsigntitle.innerHTML = index === 0 ? "Log in and experience ePal services for free" : "Sign up and experience ePal services for free";
                        //modalfooter.style.display = index === 0 ? 'flex' : 'none';
                        modalfooterP.innerHTML = index === 0 ? 'Or log in with' : 'Or sign up with';

                        //if (index === 0) { logsignBtn[0].value = "Log In"; }
                        //else if (index === 1) { logsignBtn[1].value = "Sign Up"; }

                        //e.classList.remove('show', 'active');

                    })
                    event.srcElement.classList.add('logsign-purple-border');
                    //event.srcElement.classList.add('show', 'active');

                    //modal關閉後清除purple border
                    modalbtnclose.addEventListener('click', function (event) {
                        item.classList.remove('logsign-purple-border');
                        logsignTabContent[index].classList.remove('show', 'active');
                        logsigntab[index].classList.remove('active');
                        //idx == "";
                        //index == "";
                        //validate.forEach(item => {
                        //    item.innerHTML = "";
                        //})
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
statuslistbtn.forEach((stabtn, idx) => {
    // statusbar.innerHTML = `<img src="online.png" alt="">ONLINE`;
    stabtn.addEventListener('click', function () {
        statusbar.innerHTML = stabtn.innerHTML;

    })
})


