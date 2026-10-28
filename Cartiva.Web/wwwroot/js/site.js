document.addEventListener("DOMContentLoaded", function () {
    console.log("Cartiva page loaded");

    document.addEventListener("click", function (event) {
        const control = event.target.closest("button, a");

        if (!control) {
            return;
        }

        console.log("Clicked element:", control.innerText.trim());
        console.log("Element type:", control.tagName);
        console.log("Element type attribute:", control.getAttribute("type"));

        debugger;
    });
});