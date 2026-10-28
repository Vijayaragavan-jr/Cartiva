// AUTH TABS

$("#loginTabBtn").click(function () {

    $("#loginTabBtn").addClass("active");
    $("#registerTabBtn").removeClass("active");

    $("#loginForm").removeClass("hidden");
    $("#registerForm").addClass("hidden");

});

$("#registerTabBtn").click(function () {

    $("#registerTabBtn").addClass("active");
    $("#loginTabBtn").removeClass("active");

    $("#registerForm").removeClass("hidden");
    $("#loginForm").addClass("hidden");

});


// TOKEN REFRESH

function refreshAccessToken() {

    var refreshToken =
        localStorage.getItem("refreshToken");

    if (!refreshToken)
        return $.Deferred().reject().promise();

    return $.ajax({
        url: "/api/refreshtoken",
        type: "POST",
        contentType: "application/json",

        data: JSON.stringify({
            refreshToken: refreshToken
        })
    }).then(function (response) {

        localStorage.setItem(
            "accessToken",
            response.accessToken
        );

        return response.accessToken;
    });
}


// API REQUEST

function apiRequest(options) {

    var accessToken =
        localStorage.getItem("accessToken");

    options.headers = options.headers || {};

    options.headers.Authorization =
        "Bearer " + accessToken;

    return $.ajax(options).then(
        function (response) {

            return response;
        },
        function (xhr) {

            if (xhr.status !== 401)
                return $.Deferred().reject(xhr).promise();

            return refreshAccessToken()
                .then(function (newAccessToken) {

                    options.headers.Authorization =
                        "Bearer " + newAccessToken;

                    return $.ajax(options);
                });
        }
    );
}


// REGISTER

$("#registerBtn").click(function () {

    $.ajax({
        url: "/api/auth/register",
        type: "POST",
        contentType: "application/json",

        data: JSON.stringify({
            username: $("#registerUsername").val(),
            email: $("#registerEmail").val(),
            password: $("#registerPassword").val()
        }),

        success: function (response) {

            $("#registerMessage")
                .text(response.message)
                .addClass("success")
                .removeClass("error");

            $("#registerUsername").val("");
            $("#registerEmail").val("");
            $("#registerPassword").val("");

        },

        error: function (xhr) {

            $("#registerMessage")
                .text(
                    xhr.responseJSON?.message ||
                    "Registration failed"
                )
                .addClass("error")
                .removeClass("success");
        }
    });

});


// LOGIN

$("#loginBtn").click(function () {

    $.ajax({
        url: "/api/auth/login",
        type: "POST",
        contentType: "application/json",

        data: JSON.stringify({
            email: $("#loginEmail").val(),
            password: $("#loginPassword").val()
        }),

        success: function (response) {

            localStorage.setItem(
                "accessToken",
                response.accessToken
            );

            localStorage.setItem(
                "refreshToken",
                response.refreshToken
            );

            $("#loginMessage")
                .text(response.message)
                .addClass("success")
                .removeClass("error");

            $("#authSection").addClass("hidden");
            $("#applicationSection").removeClass("hidden");
            $("#mainNavigation").removeClass("hidden");

            loadProducts();

        },

        error: function (xhr) {

            $("#loginMessage")
                .text(
                    xhr.responseJSON?.message ||
                    "Login failed"
                )
                .addClass("error")
                .removeClass("success");
        }
    });

});


// NAVIGATION

$(".nav-button").click(function () {

    var tabId = $(this).data("tab");

    $(".nav-button")
        .removeClass("active");

    $(this)
        .addClass("active");

    $(".tab-content")
        .addClass("hidden");

    $("#" + tabId)
        .removeClass("hidden");

    if (tabId === "productsTab")
        loadProducts();

    if (tabId === "cartTab")
        loadCart();

    if (tabId === "profileTab")
        loadProfile();

});


// LOAD PRODUCTS

function loadProducts() {

    apiRequest({
        url: "/api/product",
        type: "GET"
    })
    .then(function (products) {

        $("#productsContainer").empty();

        if (!products || products.length === 0) {

            $("#productsContainer").html(
                "<p>No products available.</p>"
            );

            return;
        }

        products.forEach(function (product) {

            var card = `
                <div class="product-card">

                    <h3>${product.name}</h3>

                    <p>
                        ${product.description || "No description available"}
                    </p>

                    <p class="product-price">
                        ₹${product.price}
                    </p>

                    <button
                        class="add-cart-button"
                        data-product-id="${product.id}">
                        Add to Cart
                    </button>

                </div>
            `;

            $("#productsContainer").append(card);

        });

    })
    .fail(function (xhr) {

        $("#productsMessage")
            .text(
                xhr.responseJSON?.message ||
                "Unable to load products"
            )
            .addClass("error")
            .removeClass("success");

    });

}


// ADD TO CART

$(document).on(
    "click",
    ".add-cart-button",
    function () {

        var productId =
            $(this).data("product-id");

        apiRequest({
            url:
                "/api/cart/add?productId=" +
                productId +
                "&quantity=1",

            type: "POST"
        })
        .then(function (response) {

            $("#productsMessage")
                .text(response.message)
                .addClass("success")
                .removeClass("error");

        })
        .fail(function (xhr) {

            $("#productsMessage")
                .text(
                    xhr.responseJSON?.message ||
                    "Unable to add product to cart"
                )
                .addClass("error")
                .removeClass("success");

        });

    }
);


// LOAD CART

function loadCart() {

    apiRequest({
        url: "/api/cart",
        type: "GET"
    })
    .then(function (response) {

        $("#cartContainer").empty();

        if (
            !response.items ||
            response.items.length === 0
        ) {

            $("#cartContainer").html(
                "<p>Your cart is empty.</p>"
            );

            return;
        }

        response.items.forEach(function (item) {

            var cartItem = `
                <div class="cart-item">

                    <div class="cart-item-info">

                        <h3>${item.name}</h3>

                        <p>
                            Price: ₹${item.price}
                        </p>

                        <p>
                            Total: ₹${item.total}
                        </p>

                    </div>

                    <div class="cart-item-actions">

                        <input
                            type="number"
                            min="1"
                            value="${item.quantity}"
                            class="quantity-input"
                            data-product-id="${item.productId}">

                        <button
                            class="update-button"
                            data-product-id="${item.productId}">
                            Update
                        </button>

                        <button
                            class="remove-button"
                            data-product-id="${item.productId}">
                            Remove
                        </button>

                    </div>

                </div>
            `;

            $("#cartContainer").append(cartItem);

        });

        $("#cartContainer").append(`
            <div class="cart-total">
                <strong>
                    Grand Total: ₹${response.grandTotal}
                </strong>
            </div>
        `);

    })
    .fail(function (xhr) {

        $("#cartMessage")
            .text(
                xhr.responseJSON?.message ||
                "Unable to load cart"
            )
            .addClass("error")
            .removeClass("success");

    });

}


// UPDATE CART QUANTITY

$(document).on(
    "click",
    ".update-button",
    function () {

        var productId =
            $(this).data("product-id");

        var quantityInput =
            $('.quantity-input[data-product-id="' +
            productId +
            '"]');

        var quantity =
            quantityInput.val();

        apiRequest({
            url:
                "/api/cart/update?productId=" +
                productId +
                "&quantity=" +
                quantity,

            type: "PUT"
        })
        .then(function (response) {

            $("#cartMessage")
                .text(response.message)
                .addClass("success")
                .removeClass("error");

            loadCart();

        })
        .fail(function (xhr) {

            $("#cartMessage")
                .text(
                    xhr.responseJSON?.message ||
                    "Unable to update cart"
                )
                .addClass("error")
                .removeClass("success");

        });

    }
);


// REMOVE FROM CART

$(document).on(
    "click",
    ".remove-button",
    function () {

        var productId =
            $(this).data("product-id");

        apiRequest({
            url:
                "/api/cart/remove/" +
                productId,

            type: "DELETE"
        })
        .then(function (response) {

            $("#cartMessage")
                .text(response.message)
                .addClass("success")
                .removeClass("error");

            loadCart();

        })
        .fail(function (xhr) {

            $("#cartMessage")
                .text(
                    xhr.responseJSON?.message ||
                    "Unable to remove product"
                )
                .addClass("error")
                .removeClass("success");

        });

    }
);


// LOAD PROFILE

function loadProfile() {

    apiRequest({
        url: "/api/profile",
        type: "GET"
    })
    .then(function (response) {

        $("#profileContainer").html(`
            <div class="profile-row">

                <span class="profile-label">
                    Username
                </span>

                <span class="profile-value">
                    ${response.username}
                </span>

            </div>

            <div class="profile-row">

                <span class="profile-label">
                    Email
                </span>

                <span class="profile-value">
                    ${response.email}
                </span>

            </div>

            <div class="profile-row">

                <span class="profile-label">
                    User ID
                </span>

                <span class="profile-value">
                    ${response.userId}
                </span>

            </div>
        `);

    })
    .fail(function (xhr) {

        $("#profileMessage")
            .text(
                xhr.responseJSON?.message ||
                "Unable to load profile"
            )
            .addClass("error")
            .removeClass("success");

    });

}


// LOGOUT

$("#logoutBtn").click(function () {

    var refreshToken =
        localStorage.getItem("refreshToken");

    if (!refreshToken) {

        showLoginScreen();
        return;
    }

    $.ajax({
        url: "/api/auth/logout",
        type: "POST",
        contentType: "application/json",

        data: JSON.stringify({
            refreshToken: refreshToken
        }),

        success: function (response) {

            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");

            alert(response.message);

            showLoginScreen();

        },

        error: function () {

            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");

            showLoginScreen();

        }
    });

});


// SHOW LOGIN SCREEN

function showLoginScreen() {

    $("#authSection").removeClass("hidden");
    $("#applicationSection").addClass("hidden");
    $("#mainNavigation").addClass("hidden");

    $("#loginTabBtn").click();

    $("#loginEmail").val("");
    $("#loginPassword").val("");

    $("#loginMessage").text("");
    $("#registerMessage").text("");

}