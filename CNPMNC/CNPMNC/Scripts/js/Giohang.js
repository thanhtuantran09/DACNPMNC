$(document).ready(function () {
    $('body').on('click', '.home-product-item__cart-order', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        alert(id);

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'Post',
            data: { id: id, SL: SL },
            success: function (rs) {

            }

         });
    });
});

