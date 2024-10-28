window.addEventListener('load', function() {
    // Kiểm tra xem trang đã được chuyển đến từ trang index hay không
    const queryString = window.location.search;
    const params = new URLSearchParams(queryString);
    const fromIndex = params.get('from');
    const idAds = params.get('idads');

    if (fromIndex === 'index') {
        //Cuộn đến mục khuyến mãi tương ứng
        scrollToElement(idAds);
    }
});

function scrollToElement(elementId) {
    const targetElement = document.getElementById(elementId);

    if (targetElement) {
        // Tính toán vị trí cuộn đến
        const windowHeight = window.innerHeight;
        const elementHeight = targetElement.clientHeight;
        const offset = ((windowHeight - elementHeight) / 2) + 70;
        const targetOffset = targetElement.getBoundingClientRect().top + window.scrollY - offset;

        window.scrollTo({
            top: targetOffset,
            behavior: 'smooth'
        });
    } else {
        console.error(`Không tìm thấy phần tử với ID ${elementId}`);
    }
}