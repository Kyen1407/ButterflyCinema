function back() {
    history.back();
}

// Trang thanh toán
var params = new URLSearchParams(window.location.search);
var rap = params.get("rap");
var suatChieu = params.get("suatChieu");
var selectedSeats = localStorage.getItem("selectedSeats");
var selectedFoods = localStorage.getItem("selectedFoods");
var totalPrice = params.get("totalPrice");
var moviePoster = params.get("moviePoster");
var movieName = params.get("movieName");


// Sử dụng thông tin để cập nhật trang thanh toán
document.getElementById("rap").textContent = rap;
document.getElementById("suatChieu").textContent = suatChieu;
document.getElementById("selectedSeatsHolder").innerHTML = selectedSeats;
document.getElementById("selectedFoodsHolder").innerHTML = selectedFoods;
document.getElementById("totalPrice").textContent = totalPrice;
document.getElementById("moviePoster").src = moviePoster;
document.getElementById("movieName").textContent = movieName;

var newURL = window.location.pathname;
window.history.replaceState({}, document.title, newURL);

