// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", () => {
    handleNavbarVisibilityOnScroll();
});

const handleNavbarVisibilityOnScroll = () => {

    el_autohide = document.querySelector('.autohide');
    // add padding-top to bady (if necessary)
    navigationMenu_height = document.querySelector('#navigationMenu').offsetHeight;

    if (el_autohide) {
        var last_scroll_top = 0;

        // window:scroll
        window.addEventListener('scroll', function () {
            let scroll_top = window.scrollY;
            // Only allow the behaviour after the scroll goes at least at the navigation menu height
            if (scroll_top >= navigationMenu_height) {
                // Scrolled up
                if (scroll_top < last_scroll_top) {
                    el_autohide.classList.remove('scrolled-down');
                    el_autohide.classList.add('scrolled-up');
                }
                // Scrolled down
                else {
                    el_autohide.classList.remove('scrolled-up');
                    el_autohide.classList.add('scrolled-down');
                }
            }

            last_scroll_top = scroll_top;
        });
        // end window:scroll
    }
}