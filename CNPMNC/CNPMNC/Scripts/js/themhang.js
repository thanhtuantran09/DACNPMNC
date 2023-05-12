$(document).ready(function () {
    // Bắt sự kiện khi người dùng nhấp vào nút "Thêm vào giỏ hàng"
    $(".home-product-item__cart, .home-product-item__detail-cart").click(function (event) {
        event.preventDefault();
        var productId = $(this).attr("data-product-id");
        // Gửi yêu cầu đến server để thêm sản phẩm vào giỏ hàng
        $.ajax({
            type: "POST",
            url: "/ShoppingCart/AddToCart/" + productId,
            success: function () {
                // Cập nhật số lượng sản phẩm trong giỏ hàng
                $(".header__cart-notice").load("/ShoppingCart/Giohang");
                // Lưu cookie để hiển thị thông báo khi trang được load lại
                document.cookie = "toast=success; path=/";
                // Load lại trang
                location.reload();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });

    // Hiển thị thông báo nếu cookie được lưu trữ
    if (document.cookie.indexOf("toast=success") !== -1) {
        $("#toast").fadeIn(500).delay(1500).fadeOut(500);
        // Xóa cookie sau khi hiển thị thông báo
        document.cookie = "toast=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    }
});