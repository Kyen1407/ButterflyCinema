const allSeatCont = document.querySelectorAll(".grid-container .seat");
const allSeat = document.querySelectorAll(".grid-container .seat .icon-text");
const selectedSeatsHolderEl = document.getElementById("selectedSeatsHolder");
const seatContEl = document.querySelectorAll(".grid-container .seat:not(.occupied)");
const selectedFoodsHolderEl = document.getElementById("selectedFoodsHolder");

// Lấy mã ghế
let initialSeatValue = 0;
allSeatCont.forEach((seat) => {
    const attr = document.createAttribute("data-seatid");
    attr.value = allSeat[initialSeatValue].textContent;
    seat.setAttributeNode(attr);
    initialSeatValue++;
});

// Tạo sự kiện click cho từng ghế
let takenSeats = [];
var seatCont = 0
seatContEl.forEach((seat) => {
    seat.addEventListener("click", (e) => {
        let isSelected = seat.classList.contains("selected");

        let seatId = seat.dataset.seatid;
        if (selectedSeats == 0) {
            Swal.fire(
                'Vui lòng chọn vé',
            )
        }
        else if (!isSelected) {
            if (seatCont >= selectedSeats) {
                Swal.fire(
                    'Bạn đã chọn đủ số lượng ghế',
                )
            }
            else {
                seat.classList.add("selected");
                takenSeats.push(seatId);
                seatCont++;
            }
        } else if (isSelected) {

            seat.classList.remove("selected");
            seatCont--;

            takenSeats = takenSeats.filter((seat) => {
                if (seat !== seatId) {
                    return seat;
                }
            });
        }
        updateSeats();
    });
});

// Lấy tất cả các nút "ticket-plus" và "ticket-minus"
var plusButtons = document.querySelectorAll(".ticket-plus");
var minusButtons = document.querySelectorAll(".ticket-minus");
var selectedSeats = 0;
var allPrice = 0;

// Lặp qua từng nút "ticket-plus" và xử lý sự kiện
plusButtons.forEach(function (button) {
    button.addEventListener("click", function () {
        var row = this.closest(".table-content");
        var quantityInput = row.querySelector(".ticket-qty");
        var moviePrice = parseInt(row.querySelector(".movie-price").textContent);
        var ticketPrice = parseInt(row.querySelector(".ticket-price").textContent);

        var currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity < 10) {
            currentQuantity++;
            quantityInput.value = currentQuantity;
            ticketPrice = currentQuantity * moviePrice;
            row.querySelector(".ticket-price").textContent = ticketPrice + " đ";
        }

        selectedSeats++;
        allPrice = calculateTotalPrice();
        updatePrice(allPrice, 1)
    });
});

// Lặp qua từng nút "ticket-minus" và xử lý sự kiện
minusButtons.forEach(function (button) {
    button.addEventListener("click", function () {
        var row = this.closest(".table-content");
        var quantityInput = row.querySelector(".ticket-qty");
        var moviePrice = parseInt(row.querySelector(".movie-price").textContent);
        var ticketPrice = parseInt(row.querySelector(".ticket-price").textContent);

        var currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity > 0) {
            currentQuantity--;
            quantityInput.value = currentQuantity;
            ticketPrice = currentQuantity * moviePrice;
            row.querySelector(".ticket-price").textContent = ticketPrice + " đ";
        }

        seatContEl.forEach((seat) => {
            seat.classList.remove("selected");
            seatCont = 0;
            takenSeats = [];
            updateSeats();
            updatePrice(0, 0);
        });

        selectedSeats--;
        allPrice = calculateTotalPrice();
        updatePrice(allPrice, 1)
    });
});

// Lấy tất cả các nút "food-plus" và "ticket-minus"
var plusButtonsFood = document.querySelectorAll(".food-plus");
var minusButtonsFood = document.querySelectorAll(".food-minus");
var selectedFoods = 0;

// Lặp qua từng nút "food-plus" và xử lý sự kiện
plusButtonsFood.forEach(function (button) {
    button.addEventListener("click", function () {
        var row = this.closest(".table-content");
        var name = row.querySelector(".food-name").textContent;
        var quantityInput = row.querySelector(".food-qty");
        var foodPrice = row.querySelector("#food-price").textContent;
        foodPrice = foodPrice.toString().replace("Giá: ", "");
        foodPrice = parseInt(foodPrice.toString().replace(" đ", ""));
        var totalPriceFood = parseInt(row.querySelector("#total-food-price").textContent);

        var currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity < 10) {
            currentQuantity++;
            quantityInput.value = currentQuantity;
            totalPriceFood = currentQuantity * foodPrice;
            row.querySelector("#total-food-price").textContent = totalPriceFood + " đ";
            takenFoods.push(name);
        }

        updateFoods();
        selectedFoods++;
        allPrice = calculateTotalPrice();
        updatePrice(allPrice, 1)
    });
});

// Lặp qua từng nút "food-minus" và xử lý sự kiện
minusButtonsFood.forEach(function (button) {
    button.addEventListener("click", function () {
        var row = this.closest(".table-content");
        var name = row.querySelector(".food-name").textContent;
        var quantityInput = row.querySelector(".food-qty");
        var foodPrice = row.querySelector("#food-price").textContent;
        foodPrice = foodPrice.toString().replace("Giá: ", "");
        foodPrice = parseInt(foodPrice.toString().replace(" đ", ""));
        var totalPriceFood = parseInt(row.querySelector("#total-food-price").textContent);

        var currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity > 0) {
            currentQuantity--;
            quantityInput.value = currentQuantity;
            totalPriceFood = currentQuantity * foodPrice;
            row.querySelector("#total-food-price").textContent = totalPriceFood + " đ";
        }

        const removeIndex = takenFoods.findIndex((item) => item === name);
        takenFoods.splice(removeIndex, 1);

        updateFoods();
        selectedFoods--;
        allPrice = calculateTotalPrice();
        updatePrice(allPrice, 1)
    });
});

// Hàm updata thức ăn
var takenFoods = [];
var takenQty = [];
function updateFoods() {
    selectedFoodsHolderEl.innerHTML = ` `;
    let counts = {};

    takenFoods.forEach((item) => {
        if (counts[item]) {
            counts[item]++;
        } else {
            counts[item] = 1;
        }
    });

    for (let key in counts) {
        const value = counts[key];
        const foodHolder = document.createElement("div");
        selectedFoodsHolderEl.appendChild(foodHolder);
        foodHolder.innerHTML = key + " x " + value;
    }
}

// Hàm update giá
function updatePrice(price, seats) {
    const totalPriceEl = document.getElementById("totalPrice");
    let total = seats * price;
    totalPriceEl.innerHTML = `${total} đ`;
}

// Hàm update ghế
function updateSeats() {
    selectedSeatsHolderEl.innerHTML = ` `;

    takenSeats.forEach((seat) => {
        const seatHolder = document.createElement("div");
        seatHolder.classList.add("selectedSeat");
        selectedSeatsHolderEl.appendChild(seatHolder);

        seatHolder.innerHTML = seat;
    });
}

// Hàm tính tổng tiền
function calculateTotalPrice() {
    var totalPrice = 0;
    var ticketPriceElements = document.querySelectorAll(".ticket-price");
    var foodPriceElements = document.querySelectorAll("#total-food-price");

    ticketPriceElements.forEach(function (ticketPriceElement) {
        var priceText = ticketPriceElement.textContent;
        var price = parseInt(priceText);
        totalPrice += price;
    });

    foodPriceElements.forEach(function (foodPriceElement) {
        var priceText = foodPriceElement.textContent;
        var price = parseInt(priceText);
        totalPrice += price;
    });

    return totalPrice;
}

// Hàm quay lại trang trước
function back() {
    history.back();
}

function checkout() {
    // Trang đặt vé
    var rap = document.getElementById("rap").textContent;
    var suatChieu = document.getElementById("suatChieu").textContent;
    var selectedSeats = document.getElementById("selectedSeatsHolder").innerHTML;
    var selectedFoods = document.getElementById("selectedFoodsHolder").innerHTML;
    var totalPrice = document.getElementById("totalPrice").textContent;

    var moviePoster = document.querySelector('.poster img').src;
    var movieName = document.querySelector('.movie-name p').textContent;

    localStorage.setItem("selectedSeats", selectedSeats);
    localStorage.setItem("selectedFoods", selectedFoods);
    localStorage.setItem("moviePoster", moviePoster);
    localStorage.setItem("movieName", movieName);

    if (selectedSeats == "") {
        Swal.fire("Vui lòng chọn ghế");
    }
    else {
        var queryParams = "?rap=" + encodeURIComponent(rap) +
            "&suatChieu=" + encodeURIComponent(suatChieu) +
            "&selectedFoods=" + encodeURIComponent(selectedFoods) +
            "&totalPrice=" + encodeURIComponent(totalPrice) +
            "&moviePoster=" + encodeURIComponent(moviePoster) +
            "&movieName=" + encodeURIComponent(movieName);

        window.location.href = '/Checkout/Checkout' + queryParams + '#header';
    }
}