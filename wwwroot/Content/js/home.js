// =====================================================================
// BIẾN TOÀN CỤC VÀ KHỞI TẠO
// =====================================================================
var listDay = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
var listMonth = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var today = new Date();
var date = listDay[today.getDay()] + ' ' + today.getDate() + ' ' + listMonth[today.getMonth()] + ' ' + today.getFullYear();
var time = today.getHours() + ":" + (today.getMinutes() < 10 ? "0" + today.getMinutes() : today.getMinutes());

// Biến captcha toàn cục
var currentCaptchaStr = "";
var captchaTextCanvas = null;
var ctx = null;

var alphaNums = ['A', 'B', 'C', 'D', 'E', 'F', 'G',
    'H', 'I', 'J', 'K', 'L', 'M', 'N',
    'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
    'c', 'd', 'e', 'f', 'g', 'h', 'i',
    'j', 'k', 'l', 'm', 'n', 'o', 'p',
    'q', 'r', 's', 't', 'u', 'v', 'w',
    'x', 'y', 'z', '0', '1', '2', '3',
    '4', '5', '6', '7', '8', '9'];

function hideModal(modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        // Cách 1: Sử dụng Bootstrap 5
        if (typeof bootstrap !== 'undefined') {
            var bsModal = bootstrap.Modal.getInstance(modal);
            if (bsModal) {
                bsModal.hide();
            } else {
                // Tạo instance mới nếu chưa có
                var newModal = new bootstrap.Modal(modal);
                newModal.hide();
            }
        }
        // Cách 2: Sử dụng Bootstrap 4 với jQuery
        else if (typeof $ !== 'undefined' && $.fn.modal) {
            $(modal).modal('hide');
        }
        // Cách 3: Vanilla JavaScript fallback
        else {
            modal.style.display = 'none';
            modal.classList.remove('show');
            document.body.classList.remove('modal-open');

            // Xóa backdrop nếu có
            var backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) {
                backdrop.remove();
            }
        }
    }
}

// =====================================================================
// DOCUMENT READY - KHỞI TẠO TẤT CẢ
// =====================================================================
$(document).ready(function () {
    console.log("Document ready - Khởi tạo ứng dụng");

    // Cập nhật thời gian
    updateDateTime();

    // Fixed Menu
    initFixedMenu();

    // Khởi tạo captcha
    initCaptcha();

    // Gán tất cả event listeners
    bindAllEvents();
});

// =====================================================================
// CẬP NHẬT THỜI GIAN
// =====================================================================
function updateDateTime() {
    var dayElement = document.getElementById("day");
    var timeElement = document.getElementById("time");

    if (dayElement) dayElement.innerText = date;
    if (timeElement) timeElement.innerText = time;
}

// =====================================================================
// FIXED MENU
// =====================================================================
function initFixedMenu() {
    var secondaryNav = $('.cd-secondary-nav');
    if (secondaryNav.length > 0) {
        var secondaryNavTopPosition = secondaryNav.offset().top;
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > secondaryNavTopPosition) {
                secondaryNav.addClass('is-fixed');
            } else {
                secondaryNav.removeClass('is-fixed');
            }
        });
    }
}

// =====================================================================
// KHỞI TẠO CAPTCHA
// =====================================================================
function initCaptcha() {
    captchaTextCanvas = document.getElementById('captcha');
    if (captchaTextCanvas) {
        ctx = captchaTextCanvas.getContext("2d");
        ctx.font = "20px Roboto";
        ctx.fillStyle = "#cf0000";
        generate_captcha(); // Dòng 81
    }
}

// =====================================================================
// GÁN TẤT CẢ EVENT LISTENERS
// =====================================================================
function bindAllEvents() {
    console.log("Đang gán tất cả event listeners...");

    // Event cho captcha
    bindCaptchaEvents();

    // Event cho đăng nhập (thay thế onclick)
    bindLoginEvents();

    // Event cho đăng xuất
    bindLogoutEvents();

    // Event cho đăng ký (thay thế onclick)
    bindRegisterEvents();

    // Event cho quên mật khẩu
    bindForgotPasswordEvents();
}

// =====================================================================
// EVENT CAPTCHA
// =====================================================================
function bindCaptchaEvents() {
    var userTextInput = document.getElementById('textBox');
    var refreshButton = document.getElementById('refreshButton');

    if (userTextInput) {
        userTextInput.addEventListener('keyup', function (e) {
            if (e.key === 'Enter') {
                check_captcha(function (isCorrect) {
                    if (isCorrect) {
                        handleForgotPasswordRequest();
                    }
                });
            }
        });
    }

    if (refreshButton) {
        refreshButton.addEventListener('click', function (e) {
            e.preventDefault();
            generate_captcha();
        });
    }
}

// =====================================================================
// EVENT ĐĂNG NHẬP
// =====================================================================
function bindLoginEvents() {
    // Thay thế onclick bằng event listener
    var loginButton = document.querySelector('#modalLogin .btn-login');
    if (loginButton) {
        // Xóa onclick cũ nếu có
        loginButton.removeAttribute('onclick');
        loginButton.addEventListener('click', function (e) {
            e.preventDefault();
            handleLogin();
        });
        console.log("Đã gán event cho nút đăng nhập");
    }

    // Event cho Enter key trong form đăng nhập
    $('#modalLogin #email, #modalLogin #pass').on('keyup', function (e) {
        if (e.key === 'Enter') {
            handleLogin();
        }
    });
}

// =====================================================================
// EVENT ĐĂNG XUẤT
// =====================================================================
function bindLogoutEvents() {
    var logoutButton = document.getElementById('logoutButton');
    if (logoutButton) {
        logoutButton.addEventListener('click', function (e) {
            e.preventDefault();
            handleLogout();
        });
        console.log("Đã gán event cho nút đăng xuất");
    }
}

// =====================================================================
// EVENT ĐĂNG KÝ
// =====================================================================
function bindRegisterEvents() {
    var registerButton = document.getElementById('registerButton');
    if (registerButton) {
        registerButton.addEventListener('click', function (e) {
            e.preventDefault();
            handleRegister();
        });
        console.log("Đã gán event cho nút đăng ký");
    }
}

// =====================================================================
// EVENT QUÊN MẬT KHẨU
// =====================================================================
function bindForgotPasswordEvents() {
    var submitButton = document.getElementById('submitButton');
    if (submitButton) {
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            check_captcha(function (isCorrect) {
                if (isCorrect) {
                    console.log("Captcha đúng, xử lý quên mật khẩu");
                    handleForgotPasswordRequest();
                }
            });
        });
        console.log("Đã gán event cho nút gửi quên mật khẩu");
    }
}

// =====================================================================
// XỬ LÝ ĐĂNG NHẬP
// =====================================================================
function handleLogin() {
    console.log("Đăng nhập được kích hoạt");

    var email = $('#modalLogin #email').val().trim();
    var password = $('#modalLogin #pass').val();
    var errorMessage = $('#modalLogin #erro');

    errorMessage.text('');

    if (!email || !password) {
        errorMessage.text('Vui lòng nhập email và mật khẩu.');
        return;
    }

    // Hiển thị loading
    var loginButton = document.querySelector('#modalLogin .btn-login');
    var originalText = loginButton.textContent;
    loginButton.textContent = 'Đang xử lý...';
    loginButton.disabled = true;

    $.ajax({
        url: '/Account/Login',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Email: email, Password: password }),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Đăng nhập thành công!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    hideModal('modalLogin');
                    window.location.href = '/Home/Index';
                });
            } else {
                errorMessage.text(response.message || 'Email hoặc mật khẩu không đúng.');
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Có lỗi xảy ra trong quá trình đăng nhập. Vui lòng thử lại sau.',
                confirmButtonText: 'Đóng'
            });
            console.error("Lỗi đăng nhập: ", error);
        },
        complete: function () {
            loginButton.textContent = originalText;
            loginButton.disabled = false;
        }
    });
}

// =====================================================================
// XỬ LÝ ĐĂNG XUẤT
// =====================================================================
function handleLogout() {
    Swal.fire({
        title: 'Bạn có chắc chắn muốn đăng xuất?',
        text: "Bạn sẽ cần đăng nhập lại để sử dụng các tính năng!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý, đăng xuất!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            // Nếu người dùng đồng ý, chuyển hướng đến URL đăng xuất
            window.location.href = '/Account/Logout';
        }
    });
}

// =====================================================================
// XỬ LÝ ĐĂNG KÝ
// =====================================================================
function handleRegister() {
    console.log("Đăng ký được kích hoạt");

    var gender = $('input[name="gender"]:checked').val();
    var name = $('#name').val();
    var phone = $('#phone').val();
    var dob = $('#date').val();
    var email = $('#re-email').val().trim();
    var password = $('#regis-pass').val();
    var confirmPassword = $('#regis-re-pass').val();
    var agreePolicy = $('#agree').is(':checked');
    var errorMessageElement = $('#modalRegister #erro');
    console.log("Thông tin đăng ký:", gender, name, phone, dob, email, password, confirmPassword, agreePolicy);

    errorMessageElement.text('');

    // Validation
    if (!gender) {
        errorMessageElement.text('Vui lòng chọn giới tính.');
        return;
    }
    if (!name || !phone || !dob || !email || !password || !confirmPassword) {
        errorMessageElement.text('Vui lòng điền đầy đủ thông tin.');
        return;
    }
    if (password.length < 6) {
        errorMessageElement.text('Mật khẩu phải có ít nhất 6 ký tự.');
        return;
    }
    if (password !== confirmPassword) {
        errorMessageElement.text('Mật khẩu xác nhận không khớp.');
        return;
    }
    if (!agreePolicy) {
        errorMessageElement.text('Bạn phải đồng ý với chính sách bảo mật.');
        return;
    }

    // Validate email format
    if (!/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email)) {
        errorMessageElement.text('Email không đúng định dạng.');
        return;
    }

    // Validate phone format
    if (!/^\d{10,11}$/.test(phone)) {
        errorMessageElement.text('Số điện thoại không đúng định dạng.');
        return;
    }

    // Hiển thị loading
    var registerButton = document.getElementById('registerButton');
    var originalText = registerButton.textContent;
    registerButton.textContent = 'Đang xử lý...';
    registerButton.disabled = true;

    var registrationData = {
        Gender: gender,
        Name: name,
        Phone: phone,
        DateOfBirth: dob,
        Email: email,
        Password: password,
        ConfirmPassword: confirmPassword
    };

    $.ajax({
        url: '/Account/Register',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(registrationData),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Đăng ký thành công!',
                    text: response.message || 'Vui lòng kiểm tra email của bạn để xác thực tài khoản.',
                    confirmButtonText: 'Đóng'
                }).then(() => {
                    hideModal('modalRegister');
                    $('#modalRegister form')[0].reset();
                });
            } else {
                errorMessageElement.text(response.message || 'Đăng ký thất bại. Vui lòng thử lại.');
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại sau.',
                confirmButtonText: 'Đóng'
            });
            console.error("Lỗi đăng ký: ", error);
        },
        complete: function () {
            registerButton.textContent = originalText;
            registerButton.disabled = false;
        }
    });
}

// =====================================================================
// XỬ LÝ QUÊN MẬT KHẨU
// =====================================================================
function handleForgotPasswordRequest() {
    console.log("Quên mật khẩu được kích hoạt");

    // Sử dụng đúng ID từ HTML
    var email = $('#modalFogotPass #emailFogotPass').val();
    var errorMessageElement = $('#modalFogotPass #output');

    errorMessageElement.text('');

    if (!email) {
        errorMessageElement.text('Vui lòng nhập email.');
        return;
    }

    if (!/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email)) {
        errorMessageElement.text('Email không đúng định dạng.');
        return;
    }

    // Hiển thị loading
    var submitButton = document.getElementById('submitButton');
    var originalText = submitButton.textContent;
    submitButton.textContent = 'Đang gửi...';
    submitButton.disabled = true;

    $.ajax({
        url: '/Account/ForgotPassword',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Email: email }),
        success: function (response) {
            Swal.fire({
                icon: 'info',
                title: 'Kiểm tra email',
                text: response.message || 'Nếu email của bạn tồn tại trong hệ thống, một liên kết đặt lại mật khẩu đã được gửi đến bạn.',
                confirmButtonText: 'Đóng'
            }).then(() => {
                hideModal('modalFogotPass');
                $('#modalFogotPass form')[0].reset();
                generate_captcha();
            });
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Có lỗi xảy ra. Vui lòng thử lại sau.',
                confirmButtonText: 'Đóng'
            });
            console.error("Lỗi quên mật khẩu: ", error);
        },
        complete: function () {
            submitButton.textContent = originalText;
            submitButton.disabled = false;
        }
    });
}

// =====================================================================
// CÁC HÀM CAPTCHA
// =====================================================================
function generate_captcha() {
    if (!captchaTextCanvas || !ctx) {
        console.warn("Canvas captcha chưa được khởi tạo");
        return;
    }

    let emptyArr = [];

    for (let i = 1; i <= 7; i++) {
        emptyArr.push(alphaNums[Math.floor(Math.random() * alphaNums.length)]);
    }
    currentCaptchaStr = emptyArr.join('');

    ctx.clearRect(0, 0, captchaTextCanvas.width, captchaTextCanvas.height);
    ctx.fillText(currentCaptchaStr, captchaTextCanvas.width / 12, captchaTextCanvas.height / 1.15);

    var outputCaptcha = document.getElementById('output');
    var userTextInput = document.getElementById('textBox');

    if (outputCaptcha) outputCaptcha.innerHTML = "";
    if (userTextInput) userTextInput.value = "";

    console.log("Đã tạo captcha mới: " + currentCaptchaStr);
}

function check_captcha(callback) {
    var userTextInput = document.getElementById('textBox');
    var outputCaptcha = document.getElementById('output');

    if (!userTextInput || !outputCaptcha) {
        console.error("Không tìm thấy elements captcha");
        if (typeof callback === 'function') callback(false);
        return;
    }

    if (userTextInput.value === currentCaptchaStr) {
        outputCaptcha.className = "correctCaptcha";
        outputCaptcha.innerHTML = "Chính xác!";
        if (typeof callback === 'function') {
            callback(true);
        }
    } else {
        outputCaptcha.className = "incorrectCaptcha";
        outputCaptcha.innerHTML = "Captcha không hợp lệ!";
        generate_captcha(); // Tạo lại captcha khi người dùng nhập sai
        if (typeof callback === 'function') {
            callback(false);
        }
    }
}

// =====================================================================
// CÁC HÀM TIỆN ÍCH
// =====================================================================
function srcChange(src) {
    document.getElementById('map').src = (src);
    $('html, body').animate({ scrollTop: 0 }, 500);
}

function detailAds(idAds) {
    window.location.href = '/Ads/Ads?from=index&idads=' + idAds;
} // <-- Sửa lỗi cú pháp: xóa dấu `}` thừa ở đây

function back() {
    history.back();
}

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
        count++;
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
        row.style.display = "none";
    });

    // Hiện các phòng thuộc rạp được chọn
    rows.forEach(function (row) {
        if (row.getAttribute("data-cinema-id") === cinemaId) {
            row.style.display = "";
            count++;
            row.querySelector(".room-number").textContent = count;
        }
    });

}

// =====================================================================
// HÀM TOÀN CỤC ĐỂ TƯƠNG THÍCH VỚI ONCLICK (NẾU CẦN)
// =====================================================================
// Các hàm này có thể được gọi từ onclick nếu cần
window.checkLogin = handleLogin;
window.handleForgotPasswordRequest = handleForgotPasswordRequest; // Dòng 534
window.generate_captcha = generate_captcha;