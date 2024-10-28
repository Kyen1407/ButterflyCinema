var listDay = [ 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
var listMonth = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var today = new Date();
var date = listDay[today.getDay()] + ' ' + today.getDate() + ' ' + listMonth[today.getMonth()] + ' ' + today.getFullYear();
var time = today.getHours() + ":" + today.getMinutes();

document.getElementById("day").innerText = date;
document.getElementById("time").innerText = time;

$(document).ready(function(){
    /*****Fixed Menu******/
	var secondaryNav = $('.cd-secondary-nav'),
	secondaryNavTopPosition = secondaryNav.offset().top;
	$(window).on('scroll', function(){
		if($(window).scrollTop() > secondaryNavTopPosition ) {
			secondaryNav.addClass('is-fixed');	
		} else {
			secondaryNav.removeClass('is-fixed');
		}
	});	
		
});

function srcChange(src){
	document.getElementById('map').src = (src);
	
	$('html, body').animate({scrollTop:0},500);
}

function detailAds(idAds) {
	window.location.href = '/Ads/Ads?from=index&idads=' + idAds;
}

//function booking(){
//	window.location.href = '/Booking/Booking#header'
//}

function back() {
	history.back();
}

function checkLogin() {
    var emailInput = document.getElementById("email");
    var passInput = document.getElementById("pass");
    var erroMessage = document.getElementById("erro");

    var emailValue = emailInput.value;
    var passValue = passInput.value;

    if (emailValue === "" || passValue === "") {
        erroMessage.textContent = "Vui lòng nhập thông tin";
    }
    else {

    }
}

let captchaText = document.getElementById('captcha');
var ctx = captchaText.getContext("2d");
ctx.font = "20px Roboto";
ctx.fillStyle = "#cf0000";

let userText = document.getElementById('textBox');
let submitButton = document.getElementById('submitButton');
let output = document.getElementById('output');
let refreshButton = document.getElementById('refreshButton');

var captchaStr = "";

let alphaNums = ['A', 'B', 'C', 'D', 'E', 'F', 'G',
    'H', 'I', 'J', 'K', 'L', 'M', 'N',
    'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
    'c', 'd', 'e', 'f', 'g', 'h', 'i',
    'j', 'k', 'l', 'm', 'n', 'o', 'p',
    'q', 'r', 's', 't', 'u', 'v', 'w',
    'x', 'y', 'z', '0', '1', '2', '3',
    '4', '5', '6', '7', '8', '9'];

function generate_captcha() {
    let emptyArr = [];

    for (let i = 1; i <= 7; i++) {
        emptyArr.push(alphaNums[Math.floor(Math.random() * alphaNums.length)]);
    }

    captchaStr = emptyArr.join('');

    ctx.clearRect(0, 0, captchaText.width, captchaText.height);
    ctx.fillText(captchaStr, captchaText.width / 12, captchaText.height / 1.15);

    output.innerHTML = "";
}
generate_captcha();

function check_captcha() {
    if (userText.value === captchaStr) {
        output.className = "correctCaptcha";
        output.innerHTML = "Correct!";
    } else {
        output.className = "incorrectCaptcha";
        output.innerHTML = "Captcha không h?p l?!";
    }
}

userText.addEventListener('keyup', function (e) {
    if (e.key === 'Enter') {
        check_captcha();
    }
});

submitButton.addEventListener('click', check_captcha);
refreshButton.addEventListener('click', generate_captcha);

function updateButton(input, idSelect) {
    document.getElementById(idSelect).textContent = input;
}

function showAllRoom(idSelect, idObjectTable) {
    // Hiện tên rạp lên dropdown
    document.getElementById(idSelect).textContent = "Tất cả";
    var rows = document.querySelectorAll("#" + idObjectTable + " tbody tr");
    var count = 0;

    rows.forEach(function (row) {
        row.style.display = "";

        count++; // Tăng số đếm
        row.querySelector(".room-number").textContent = count;
    });
}

function filterRooms(cinemaId, cinemaName, idSelect, idObjectTable) {
    // Hiện tên rạp lên dropdown
    document.getElementById(idSelect).textContent = cinemaName;

    // Ẩn tất cả các phòng
    var rows = document.querySelectorAll("#" + idObjectTable + " tbody tr");
    var count = 0;

    rows.forEach(function (row) {
        row.style.display = "none"; // Ẩn tất cả
    });

    // Hiện các phòng thuộc rạp được chọn
    rows.forEach(function (row) {
        if (row.getAttribute("data-cinema-id") === cinemaId) {
            row.style.display = ""; // Hiện phòng

            count++; // Tăng số đếm
            row.querySelector(".room-number").textContent = count;
        }
    });
}