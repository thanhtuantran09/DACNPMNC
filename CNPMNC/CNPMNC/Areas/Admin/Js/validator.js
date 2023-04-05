
// accordion

//Lấy các button mô tả nội dung
var acc = document.getElementsByClassName("accordion-align");
var i;
//lặp qua các button để gán sự kiện
for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    /* Thêm/xóa class active để đánh dấu các button đã được click */  
    this.classList.toggle("active");
    /* Hiển thị hoặc ẩn nội dung khi button được click */
    var panel = this.nextElementSibling;
    if (panel.style.maxHeight) {
      panel.style.maxHeight = null;
    } else {
      panel.style.maxHeight = panel.scrollHeight + "px";
    } 
  });
}

// shopping cart
let carts = document.querySelectorAll('.home-product-item__detail-cart-button, .home-product-item__cart');



function onLoadCartNumbers() {
    let productNumbers = localStorage.getItem('cartNumbers');

    if (productNumbers) {
        document.querySelector('.header__cart-notice').textContent = productNumbers;
    }
}

function cartNumbers(product) {
    let productNumbers = localStorage.getItem('cartNumbers');


    productNumbers = parseInt(productNumbers);

    if (productNumbers) {
        localStorage.setItem('cartNumbers', productNumbers + 1);
        document.querySelector('.header__cart-notice').textContent = productNumbers + 1;
    } else {
        localStorage.setItem('cartNumbers', 1);
        document.querySelector('.header__cart-notice').textContent = 1;
    }
    setItems(product);
    
}

function setItems(product) {
    let cartItems = localStorage.getItem('productsInCart');
    cartItems = JSON.parse(cartItems);

    if (cartItems != null) {
        if (cartItems[product.id] == undefined) {
            cartItems = {
                ...cartItems,
                [product.id]: product
            }
        }
        cartItems[product.id].incart += 1; 
    } else {
        product.incart = 1;
        cartItems = {
            [product.id]: product
        }
    }
    localStorage.setItem("productsInCart", JSON.stringify(cartItems));
}

function totalCost(product) {
    let cartCost = localStorage.getItem('totalCost');

    if (cartCost != null) {
        cartCost = parseInt(cartCost);
        localStorage.setItem("totalCost", cartCost + product.price);
        document.querySelector('.cart-cost').textContent = cartCost + product.price;
    } else {       
        localStorage.setItem("totalCost", product.price);
        document.querySelector('.cart-cost').textContent = product.price;
    }

}

function displayCart() {
    let cartItems = localStorage.getItem("productsInCart");
    cartItems = JSON.parse(cartItems);
    let productContainer = document.querySelector(".products-items")
    let productContainer2 = document.querySelector(".total-cart-cost")
    let cartCost = localStorage.getItem('totalCost');

    if (cartItems && productContainer) {
        productContainer.innerHTML = '';
        Object.values(cartItems).map(item => {
            let price = item.incart * item.price;
            productContainer.innerHTML += `
                <tbody class="products-items">
                    <tr>
                        <td>
                        <div class="products-items-info">
                            <i class="fas fa-times-circle" onclick="removeItems(this)"></i>
                            <div class="products-items-info-img-name">
                                <img src="${item.imgSrc}"/>
                                <p>${item.name}</p>                      
                            </div>
                        </div>                      
                        </td>
                        <td>${item.price} đ</td>
                        <td>${item.incart}</td>
                        <td class="total">
                            <div>${price} đ</div>
                        </td>
                    </tr>
                        
                </tbody>
                
                    
                
            `    
        });
        productContainer2.innerHTML += `
            <h3>Tổng đơn hàng</h3>
            <p class="cart-cost">${cartCost} đ</p>
        
        `
    }
}


function removeItems (id) {
    var tr = id.parentElement.parentElement.parentElement;
    tr.remove();  
    localStorage.clear();
    
}



function displayCart2() {
    let cartItems = localStorage.getItem("productsInCart");
    cartItems = JSON.parse(cartItems);
    let productContainer = document.querySelector(".header__cart-list-item")
    let productContainer3 = document.querySelector(".header__cart-list")
    let cartCost = localStorage.getItem('totalCost');
    productContainer.innerHTML = `
        <img src="./assets/img/empty-cart.jpg"alt="" class="header__cart-no-cart-img"/>
        <span class="header__cart-list-no-cart-msg">Chưa có thông báo</span>
    `
    if (cartItems && productContainer) {
        productContainer.innerHTML = '';
        Object.values(cartItems).map(item => {
            productContainer.innerHTML += `
                <li class="header__cart-item">
                    <img src="${item.imgSrc}" alt="" class="header__cart-img"/>
                    <div class="header__cart-item-info">
                        <div class="header__cart-item-head">
                            <h5 class="header__cart-item-name">
                                ${item.name} 
                            </h5>
                            <div class="header__cart-item-price-wrap">
                                <span class="header__cart-item-price">${item.price}<span>đ</span></span>
                                <span class="header__cart-item-multiply">x</span>
                                <span class="header__cart-item-qnt">${item.incart}</span>
                            </div>
                        </div>
                        <div class="header__cart-item-body">
                            <span class="header__cart-item-remove" onclick="removeItems(this)">Xóa</span>
                        </div>
                    </div>
                </li>
            `
            
        });
        productContainer3.innerHTML += `
            <a href="/cartitems.html" class="header__cart-view-cart btn btn--primary">Xem giỏ hàng</a>
        `
        
    }
}

onLoadCartNumbers();
displayCart();
displayCart2();


var dropdown = document.getElementsByClassName("category-item-button");
var i;

for (i = 0; i < dropdown.length; i++) {
  dropdown[i].addEventListener("click", function() {
    this.classList.toggle("kichhoat");
    var dropdownContent = this.nextElementSibling;
    if (dropdownContent.style.display === "block") {
      dropdownContent.style.display = "none";
    } else {
      dropdownContent.style.display = "block";
    }
  });
}

function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
  }
  
  // Close the dropdown if the user clicks outside of it
  window.onclick = function(e) {
    if (!e.target.matches('.useracc')) {
    var myDropdown = document.getElementById("myDropdown");
      if (myDropdown.classList.contains('show')) {
        myDropdown.classList.remove('show');
      }
    }
  }




