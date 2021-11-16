let roundDisplay = document.querySelector(".rounds-div input");
roundDisplay.classList.add('round-number');
let minRound = 1;
let maxRound = 999;
let oldVal = 0;
let subtotal = document.querySelector(".subtotal");
let finalPrice = document.querySelector(".final-amount");
let totalRounds = document.querySelector(".rounds-final");
let interval, timeout;
let priceGet = document.getElementById("price").innerText;
let price = parseFloat(priceGet);
let roundInput = document.querySelector(".round-number").value;
let roundMinus = document.querySelector(".round-subtract");
roundMinus.addEventListener("click", () => {
    Subtract();
});
let roundAdd = document.querySelector(".round-add");
roundAdd.addEventListener("click", () => {
    Add();
});

roundAdd.addEventListener('mousedown', () => {
    timeout = setTimeout(function () {
        interval = setInterval(function () {
            Add();
        }, 120);
    }, 400);
});

roundAdd.addEventListener('mouseup', () => {
    clearTimers();
});
roundMinus.addEventListener('mousedown', () => {
    timeout = setTimeout(function () {
        interval = setInterval(function () {
            Subtract();
        }, 120);
    }, 400);
});

roundMinus.addEventListener('mouseup', () => {
    clearTimers();
});

function Subtract() {
    roundInput = document.querySelector(".round-number").value;
    let afterRoundInput = 0;
    if (roundInput > minRound) {
        roundInput--;
        afterRoundInput = roundInput;
        roundDisplay.value = afterRoundInput;
        PriceChange(roundInput, afterRoundInput);
    }
}

function Add() {
    roundInput = document.querySelector(".round-number").value;
    let afterRoundInput = 0;
    if (roundInput < maxRound) {
        roundInput++;
        afterRoundInput = roundInput;
        roundDisplay.value = afterRoundInput;
        PriceChange(roundInput, afterRoundInput);
    }
}

function clearTimers() {
    clearTimeout(timeout);
    clearInterval(interval)
}

function PriceChange(a, b) {
    if (b == "") {
        b = 1;
    }
    if (b > 999) {
        b = 999;
    }
    if (a == undefined) a = oldVal;
    if (Number.isNaN(parseInt(b))) {
        roundDisplay.value = parseInt(a);
    } else {
        roundDisplay.value = parseInt(b);
        oldVal = b;
        subtotal.innerText = (price * parseFloat(oldVal)).toFixed(2);
        finalPrice.innerText = (price * parseFloat(oldVal)).toFixed(2);
        totalRounds.innerText = oldVal;
    }
    let roundDisplayVal = parseFloat(
        document.querySelector(".round-number").value
    );
}

function setOldVal(val) {
    this.oldVal = val;
}

//Time input setting
let inputDate = document.getElementById('start-time');
let d, localDatetime;
let timeNowBtn = document.querySelector('.time-now');
window.addEventListener('load', function () {
    localDatetime = TimeNowPlusTen();
    inputDate.value = localDatetime;
});

timeNowBtn.addEventListener('click', () => {
    localDatetime = TimeNowPlusTen();
    inputDate.value = localDatetime;
});

function TimeNowPlusTen() {
    const now = new Date();
    const offsetMs = (now.getTimezoneOffset() - 10) * 60 * 1000;
    const dateLocal = new Date(now.getTime() - offsetMs);
    const str = dateLocal.toISOString().slice(0, 16).replace();
    return str;
}

function DateCheck(a) {
    localDatetime = TimeNowPlusTen();
    if (Date.parse(localDatetime) > Date.parse(a) || a == "") {
        return inputDate.value = localDatetime;
    } else {
        return inputDate.value = a;
    }
}

//if user is not logged in when "play" button is pressed
//prompt user to log in and prevent entering checkout page
function promptLogin() {
    var loginBtn = document.getElementById('loginmodal');
    loginBtn.click();

}