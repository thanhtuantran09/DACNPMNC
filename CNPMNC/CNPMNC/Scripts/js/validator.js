//Đối tượng 'validator'
function Validator(options) {

    function getParent(element, selector) {
        while (element.parentElement) {
            if (element.parentElement.matches(selector)) {
                return element.parentElement;
            }
            element = element.parentElement;
        }
    }

    var selectorRules = {};

    //Hàm thực hiện validate
    function validate(inputElement, rule) {
        
        var errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector);

        var errorMessage;

        // Lấy ra các rules của selector
        var rules = selectorRules[rule.selector];

        // Lặp qua từng rule và kiểm tra
        // Nếu có lỗi thì dừng kiểm tra
        for (var i = 0; i < rules.length; ++i) {
            errorMessage = rules[i](inputElement.value);
            if (errorMessage) break;
        }
                    
        if (errorMessage) {
            errorElement.innerText = errorMessage;
            getParent(inputElement, options.formGroupSelector).classList.add('invalid');
        } else {
             errorElement.innerText = '';
             getParent(inputElement, options.formGroupSelector).classList.remove('invalid');
        }

        return !errorMessage;
    }
    //Lấy element của form cần validate
    var formElement = document.querySelector(options.form);
    if (formElement) {

        //Khi submit form
        formElement.onsubmit = function (e) {
            e.preventDefault();

            var isFormValid = true;


            // Lặp qua từng rules và validate
            options.rules.forEach(function (rule) {
                var inputElement = formElement.querySelector(rule.selector);
                 var isValid = validate(inputElement, rule);
                 if (!isValid) {
                     isFormValid = false;
                 }
            });


            if (isFormValid) {
                // Trường hợp submit với javascript
                if (typeof options.onSubmit === 'function') {

                    var enableInputs = formElement.querySelectorAll('[name]');

                    var formValues = Array.from(enableInputs).reduce(function  (values, input) {
                        values[input.name] = input.value;
                        return values;
                    }, {});
                    options.onSubmit(formValues);
                    alert("Thành công!!!");
                } 
                // Trường hợp submit với hành vi mặc định
                else {
                    formElement.submit();
                }
            }
        }


        // Lặp qua mỗi rule và xử lý (lắng nghe sự kiện blur, input, ...)
        options.rules.forEach(function (rule) {

            //Lưu lại các rules cho mỗi input

            if (Array.isArray(selectorRules[rule.selector])) {
                selectorRules [rule.selector].push(rule.test);
            } else {
                selectorRules [rule.selector] = [rule.test];
            }

            
            var inputElement = formElement.querySelector(rule.selector);
            
            if (inputElement) {
                // Xử lý trường hợp blur khỏi input
                inputElement.onblur = function () {
                    //value: inputElement.value
                    //test func: rule.test
                   validate(inputElement, rule);
                }

                // Xử lý mỗi khi người dùng nhập vào input
                inputElement.oninput = function () {
                    var errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector);
                    errorElement.innerText = '';
                    getParent(inputElement, options.formGroupSelector).classList.remove('invalid');
                }
            }
        });
    }
}


//Định nghĩa rules
//Nguyên tắc của các rules:
//1. Khi có lỗi --> Trả ra messae lỗi
//2. Khi hợp lệ --> không trả ra cái gì cả (undefined)
Validator.isRequired = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            return value.trim() ? undefined : message || 'Vui lòng nhập trường này'
        }
    };
}

Validator.isEmail = function (selector, message) {
     return {
        selector: selector,
        test: function (value) {
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            return regex.test(value) ? undefined : message || 'Vui lòng nhập email'
        }
    };
}

Validator.minLength = function (selector, min, message) {
    return {
       selector: selector,
       test: function (value) {
           return value.length >= min ? undefined : message || `Vui lòng nhập tối thiểu ${min} ký tự`;
       }
   };
}

Validator.isConfirmed = function (selector, getConfirmValue, message) {
    return {
        selector: selector,
        test: function (value) {
            return value === getConfirmValue() ? undefined : message || 'Giá trị nhập vào không chính xác';
        }
    }
}


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




