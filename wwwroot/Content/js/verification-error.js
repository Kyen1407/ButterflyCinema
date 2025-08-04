// Configurable error page
const ErrorPage = {
    // Configuration object for different error types
    errorTypes: {
        '404': {
            title: 'Trang không tìm thấy!',
            message: 'Trang bạn đang tìm kiếm không tồn tại hoặc đã bị di chuyển. Vui lòng kiểm tra lại đường dẫn.',
            iconClass: 'fas fa-search'
        },
        '500': {
            title: 'Lỗi máy chủ!',
            message: 'Có lỗi xảy ra từ phía máy chủ. Chúng tôi đang khắc phục sự cố. Vui lòng thử lại sau.',
            iconClass: 'fas fa-server'
        },
        '403': {
            title: 'Không có quyền truy cập!',
            message: 'Bạn không có quyền truy cập vào trang này. Vui lòng đăng nhập hoặc liên hệ quản trị viên.',
            iconClass: 'fas fa-lock'
        },
        'network': {
            title: 'Lỗi kết nối mạng!',
            message: 'Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối internet và thử lại.',
            iconClass: 'fas fa-wifi'
        },
        'timeout': {
            title: 'Hết thời gian chờ!',
            message: 'Yêu cầu của bạn đã hết thời gian chờ. Vui lòng thử lại sau.',
            iconClass: 'fas fa-clock'
        },
        'payment': {
            title: 'Thanh toán thất bại!',
            message: 'Giao dịch thanh toán không thành công. Vui lòng kiểm tra thông tin và thử lại.',
            iconClass: 'fas fa-credit-card'
        },
        'booking': {
            title: 'Đặt vé thất bại!',
            message: 'Không thể hoàn tất việc đặt vé. Ghế có thể đã được đặt bởi người khác.',
            iconClass: 'fas fa-ticket-alt'
        },
        'login': {
            title: 'Đăng nhập thất bại!',
            message: 'Thông tin đăng nhập không chính xác. Vui lòng kiểm tra lại email và mật khẩu.',
            iconClass: 'fas fa-user-times'
        },
        'register': {
            title: 'Xác thực thất bại!',
            message: 'Không thể xác thực tài khoản. Liên kết xác thực không hợp lệ hoặc đã được sử dụng.',
            iconClass: 'fas fa-user-plus'
        },
        'validation': {
            title: 'Dữ liệu không hợp lệ!',
            message: 'Thông tin bạn nhập không đúng định dạng. Vui lòng kiểm tra và nhập lại.',
            iconClass: 'fas fa-exclamation-circle'
        }
    },

    // Initialize error page
    init: function () {
        // Get error type from URL parameters or data attributes
        const errorType = this.getErrorType();
        this.setErrorContent(errorType);
        this.initInteractions();
    },

    // Get error type from various sources
    getErrorType: function () {
        // Check URL parameters
        const urlParams = new URLSearchParams(window.location.search);
        let errorType = urlParams.get('type') || urlParams.get('error');

        // Check data attribute on body
        if (!errorType) {
            errorType = document.body.getAttribute('data-error-type');
        }

        // Check if there's a specific error code in the URL
        if (!errorType) {
            const path = window.location.pathname;
            if (path.includes('404')) errorType = '404';
            else if (path.includes('500')) errorType = '500';
            else if (path.includes('403')) errorType = '403';
        }

        return errorType || 'default';
    },

    // Set error content based on type
    setErrorContent: function (errorType) {
        const config = this.errorTypes[errorType];

        if (config) {
            // Update title
            const titleElement = document.getElementById('errorTitle');
            if (titleElement) {
                titleElement.textContent = config.title;
            }

            // Update message
            const messageElement = document.getElementById('errorMessage');
            if (messageElement) {
                messageElement.textContent = config.message;
            }

            // Update icon
            const iconElement = document.querySelector('.error-icon i');
            if (iconElement && config.iconClass) {
                iconElement.className = config.iconClass;
            }
        }
    },

    // Initialize interactive elements
    initInteractions: function () {
        // Flying butterfly effect on click
        document.addEventListener('click', this.createFlyingButterfly);

        // Add retry functionality
        this.setupRetryButton();

        // Add auto-refresh for certain error types
        this.setupAutoRefresh();
    },

    // Create flying butterfly on click
    createFlyingButterfly: function (e) {
        const butterfly = document.createElement('div');
        butterfly.innerHTML = '🦋';
        butterfly.style.position = 'fixed';
        butterfly.style.left = e.pageX + 'px';
        butterfly.style.top = e.pageY + 'px';
        butterfly.style.fontSize = '20px';
        butterfly.style.color = '#cf0000';
        butterfly.style.pointerEvents = 'none';
        butterfly.style.zIndex = '1000';
        butterfly.style.animation = 'flyAway 2s ease-out forwards';

        document.body.appendChild(butterfly);

        setTimeout(() => {
            butterfly.remove();
        }, 2000);
    },

    // Setup retry button functionality
    setupRetryButton: function () {
        const retryBtn = document.querySelector('.btn-retry');
        if (retryBtn) {
            retryBtn.addEventListener('click', function (e) {
                // Add loading state
                const originalText = this.innerHTML;
                this.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang thử lại...';
                this.style.pointerEvents = 'none';

                // Simulate retry delay
                setTimeout(() => {
                    this.innerHTML = originalText;
                    this.style.pointerEvents = 'auto';
                    // Then execute the actual retry logic
                    if (this.getAttribute('href') === 'javascript:history.back()') {
                        history.back();
                    }
                }, 1500);
            });
        }
    },

    // Setup auto-refresh for certain error types
    setupAutoRefresh: function () {
        const errorType = this.getErrorType();
        const autoRefreshTypes = ['network', 'timeout', '500'];

        if (autoRefreshTypes.includes(errorType)) {
            // Show auto-refresh countdown
            this.showAutoRefreshCountdown(30); // 30 seconds
        }
    },

    // Show auto-refresh countdown
    showAutoRefreshCountdown: function (seconds) {
        const messageElement = document.getElementById('errorMessage');
        if (!messageElement) return;

        const originalMessage = messageElement.textContent;
        let countdown = seconds;

        const countdownInterval = setInterval(() => {
            messageElement.innerHTML = originalMessage +
                `<br><small style="color: var(--text-muted); margin-top: 10px; display: block;">
                            <i class="fas fa-refresh"></i> Tự động tải lại sau ${countdown} giây...
                        </small>`;

            countdown--;

            if (countdown < 0) {
                clearInterval(countdownInterval);
                window.location.reload();
            }
        }, 1000);

        // Allow user to cancel auto-refresh
        document.addEventListener('click', () => {
            clearInterval(countdownInterval);
            messageElement.textContent = originalMessage;
        }, { once: true });
    },

    // Custom error handler for different scenarios
    handleSpecificError: function (errorType, additionalData = {}) {
        this.setErrorContent(errorType);

        // Add specific handling based on error type
        switch (errorType) {
            case 'payment':
                this.addPaymentErrorActions(additionalData);
                break;
            case 'booking':
                this.addBookingErrorActions(additionalData);
                break;
            case 'login':
                this.addLoginErrorActions();
                break;
            case 'network':
                this.addNetworkErrorActions();
                break;
        }
    },

    // Add payment-specific error actions
    addPaymentErrorActions: function (data) {
        const btnContainer = document.querySelector('.btn-container');
        if (btnContainer && data.bookingId) {
            const resumeBtn = document.createElement('a');
            resumeBtn.href = `/Payment/Resume?bookingId=${data.bookingId}`;
            resumeBtn.className = 'btn btn-retry';
            resumeBtn.innerHTML = '<i class="fas fa-credit-card"></i> Thử thanh toán lại';
            btnContainer.insertBefore(resumeBtn, btnContainer.firstChild);
        }
    },

    // Add booking-specific error actions
    addBookingErrorActions: function (data) {
        const btnContainer = document.querySelector('.btn-container');
        if (btnContainer && data.movieId) {
            const backToMovieBtn = document.createElement('a');
            backToMovieBtn.href = `/Movie/Details/${data.movieId}`;
            backToMovieBtn.className = 'btn btn-retry';
            backToMovieBtn.innerHTML = '<i class="fas fa-film"></i> Chọn suất khác';
            btnContainer.insertBefore(backToMovieBtn, btnContainer.firstChild);
        }
    },

    // Add login-specific error actions
    addLoginErrorActions: function () {
        const btnContainer = document.querySelector('.btn-container');
        if (btnContainer) {
            const forgotPasswordBtn = document.createElement('a');
            forgotPasswordBtn.href = '/Account/ForgotPassword';
            forgotPasswordBtn.className = 'btn btn-retry';
            forgotPasswordBtn.innerHTML = '<i class="fas fa-key"></i> Quên mật khẩu?';
            btnContainer.insertBefore(forgotPasswordBtn, btnContainer.firstChild);
        }
    },

    // Add network-specific error actions
    addNetworkErrorActions: function () {
        // Check network status
        if ('navigator' in window && 'onLine' in navigator) {
            const updateNetworkStatus = () => {
                const messageElement = document.getElementById('errorMessage');
                if (messageElement) {
                    if (navigator.onLine) {
                        messageElement.innerHTML = 'Kết nối mạng đã được khôi phục. <a href="javascript:window.location.reload()" style="color: var(--primary-color);">Tải lại trang</a>';
                    } else {
                        messageElement.textContent = 'Bạn đang ở chế độ offline. Vui lòng kiểm tra kết nối mạng.';
                    }
                }
            };

            window.addEventListener('online', updateNetworkStatus);
            window.addEventListener('offline', updateNetworkStatus);
        }
    }
};

// CSS for additional animations
const additionalStyles = `
            @keyframes flyAway {
                0% {
                    transform: scale(1) rotate(0deg);
                    opacity: 1;
                }
                100% {
                    transform: scale(0.3) rotate(360deg) translateY(-200px);
                    opacity: 0;
                }
            }
            
            .btn-loading {
                pointer-events: none;
                opacity: 0.7;
            }
            
            .error-details {
                margin-top: 20px;
                padding: 15px;
                background: rgba(207, 0, 0, 0.1);
                border-radius: 10px;
                border-left: 4px solid var(--primary-color);
            }
            
            .error-details h4 {
                color: var(--primary-color);
                margin-bottom: 10px;
                font-size: 14px;
            }
            
            .error-details p {
                color: var(--text-muted);
                font-size: 13px;
                margin: 0;
            }
        `;

// Add additional styles
const styleSheet = document.createElement('style');
styleSheet.textContent = additionalStyles;
document.head.appendChild(styleSheet);

// Initialize error page when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    ErrorPage.init();
});

// Expose ErrorPage to global scope for external usage
window.ErrorPage = ErrorPage;

// Example usage functions for different scenarios:

// Function to show specific error (can be called from other scripts)
window.showError = function (type, data = {}) {
    ErrorPage.handleSpecificError(type, data);
};

// Function to add custom error details
window.addErrorDetails = function (title, details) {
    const errorContent = document.querySelector('.error-content');
    if (errorContent) {
        const errorDetails = document.createElement('div');
        errorDetails.className = 'error-details';
        errorDetails.innerHTML = `
                    <h4>${title}</h4>
                    <p>${details}</p>
                `;
        errorContent.insertBefore(errorDetails, errorContent.querySelector('.btn-container'));
    }
};

// Function to update retry button URL
window.updateRetryUrl = function (url) {
    const retryBtn = document.querySelector('.btn-retry');
    if (retryBtn) {
        retryBtn.href = url;
    }
};

// Auto-detect error type from URL if not manually set
if (window.location.search.includes('type=') || window.location.search.includes('error=')) {
    // Error type will be handled by ErrorPage.init()
} else {
    // Try to detect from current page context
    const currentPath = window.location.pathname.toLowerCase();
    if (currentPath.includes('error') || currentPath.includes('404') || currentPath.includes('500')) {
        // The error type detection will be handled automatically
    }
}