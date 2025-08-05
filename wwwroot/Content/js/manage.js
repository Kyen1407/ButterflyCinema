document.addEventListener("DOMContentLoaded", function () {
    // Xử lý client-side validation
    const form = document.querySelector("#modal-addCombo form");

    if (form) {
        form.addEventListener("submit", function (e) {
            let selectedItems = [];
            let totalQuantity = 0;
            let hasError = false;
            let errorMessage = "";

            for (let i = 0; i < 3; i++) {
                const foodSelect = document.querySelector(`select[name='ComboItems[${i}].ConcessionId']`);
                const quantityInput = document.querySelector(`input[name='ComboItems[${i}].Quantity']`);

                const selectedValue = foodSelect?.value;
                const quantity = parseInt(quantityInput?.value || "0");

                if (selectedValue && quantity > 0) {
                    if (selectedItems.includes(selectedValue)) {
                        errorMessage = "Mỗi món ăn chỉ được chọn một lần.";
                        hasError = true;
                        break;
                    }
                    selectedItems.push(selectedValue);
                    totalQuantity += quantity;
                }
            }

            if (!hasError && totalQuantity < 2) {
                errorMessage = "Tổng số lượng món ăn phải ít nhất là 2.";
                hasError = true;
            }

            if (hasError) {
                e.preventDefault(); // Ngăn gửi form
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi nhập liệu',
                    text: errorMessage,
                    confirmButtonText: 'OK'
                });
            }
        });
    }

    // Nếu có lỗi từ server → hiển thị bằng Swal.fire
    const serverError = @Html.Raw(Json.Serialize(ViewBag.FormError));
    if (serverError) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi từ máy chủ',
            text: serverError,
            confirmButtonText: 'OK'
        });

        // Mở lại modal nếu có lỗi từ server
        $('#modal-addCombo').modal('show');
    }

    const successMessage = @Html.Raw(Json.Serialize(TempData["SuccessMessage"]));
    if (successMessage) {
        Swal.fire({
            icon: 'success',
            title: 'Thành công',
            text: successMessage,
            confirmButtonText: 'OK'
        });
    }
});