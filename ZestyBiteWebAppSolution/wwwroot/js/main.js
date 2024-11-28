AOS.init({
    duration: 800,
    easing: 'slide'
});


(function ($) {

    "use strict";

    // Stellar Parallax Scrolling
    if ($.fn.stellar) { // Ensure Stellar is available
        $(window).stellar({
            responsive: true,
            parallaxBackgrounds: true,
            parallaxElements: true,
            horizontalScrolling: false,
            hideDistantElements: false,
            scrollProperty: 'scroll',
            horizontalOffset: 0,
            verticalOffset: 0
        });
    }

    // Scrollax Initialization
    if ($.fn.Scrollax) {
        $.Scrollax();
    }

    // Full Height Adjustments
    var fullHeight = function () {
        // Set initial height of elements with class 'js-fullheight'
        const updateFullHeight = function () {
            document.querySelectorAll('.js-fullheight').forEach(function (element) {
                element.style.height = window.innerHeight + 'px'; // Set height to window height
            });
        };

        // Call the update function initially
        updateFullHeight();

        // Update the height on window resize
        window.addEventListener('resize', updateFullHeight);
    };

    fullHeight();

    // Loader
    var loader = function () {
        setTimeout(function () {
            if ($('#ftco-loader').length > 0) {
                $('#ftco-loader').removeClass('show');
            }
        }, 1);
    };
    loader();

    

    // Owl Carousel for Home Slider
    var carousel = function () {
        if ($.fn.owlCarousel) { // Ensure OwlCarousel is available
            $('.home-slider').owlCarousel({
                loop: true,
                autoplay: true,
                margin: 0,
                animateOut: 'fadeOut',
                animateIn: 'fadeIn',
                nav: false,
                autoplayHoverPause: false,
                items: 1,
                responsive: {
                    0: { items: 1, nav: false },
                    600: { items: 1, nav: false },
                    1000: { items: 1, nav: false }
                }
            });

            $('.carousel-work').owlCarousel({
                autoplay: true,
                center: true,
                loop: true,
                items: 1,
                margin: 30,
                stagePadding: 0,
                nav: true,
                navText: ['<span class="ion-ios-arrow-back">', '<span class="ion-ios-arrow-forward">'],
                responsive: {
                    0: { items: 1, stagePadding: 0 },
                    600: { items: 2, stagePadding: 50 },
                    1000: { items: 3, stagePadding: 100 }
                }
            });
        }
    };
    carousel();
    
    // Scroll behavior for Navbar
    var scrollWindow = function () {
        // Use native JavaScript for scroll event
        window.addEventListener('scroll', function () {
            var st = window.scrollY, // Get scroll position
                navbar = document.querySelector('.ftco_navbar'),
                sd = document.querySelector('.js-scroll-wrap');

            if (st > 150) {
                if (navbar && !navbar.classList.contains('scrolled')) {
                    navbar.classList.add('scrolled');
                }
            }
            if (st < 150) {
                if (navbar && navbar.classList.contains('scrolled')) {
                    navbar.classList.remove('scrolled', 'sleep');
                }
            }
            if (st > 350) {
                if (navbar && !navbar.classList.contains('awake')) {
                    navbar.classList.add('awake');
                }
                if (sd) {
                    sd.classList.add('sleep');
                }
            }
            if (st < 350) {
                if (navbar && navbar.classList.contains('awake')) {
                    navbar.classList.remove('awake');
                    navbar.classList.add('sleep');
                }
                if (sd) {
                    sd.classList.remove('sleep');
                }
            }
        });
    };

    scrollWindow();

    // Counter with Waypoints
    var counter = function () {
        $('#section-counter').waypoint(function (direction) {
            if (direction === 'down' && !$(this.element).hasClass('ftco-animated')) {
                var comma_separator_number_step = $.animateNumber.numberStepFactories.separator(',');
                $('.number').each(function () {
                    var $this = $(this),
                        num = $this.data('number');
                    $this.animateNumber({
                        number: num,
                        numberStep: comma_separator_number_step
                    }, 7000);
                });
            }
        }, { offset: '95%' });
    };
    counter();

    // Content animations with Waypoints
    var contentWayPoint = function () {
        var i = 0;
        $('.ftco-animate').waypoint(function (direction) {
            if (direction === 'down' && !$(this.element).hasClass('ftco-animated')) {
                i++;
                $(this.element).addClass('item-animate');
                setTimeout(function () {
                    $('body .ftco-animate.item-animate').each(function (k) {
                        var el = $(this);
                        setTimeout(function () {
                            var effect = el.data('animate-effect');
                            if (effect === 'fadeIn') {
                                el.addClass('fadeIn ftco-animated');
                            } else if (effect === 'fadeInLeft') {
                                el.addClass('fadeInLeft ftco-animated');
                            } else if (effect === 'fadeInRight') {
                                el.addClass('fadeInRight ftco-animated');
                            } else {
                                el.addClass('fadeInUp ftco-animated');
                            }
                            el.removeClass('item-animate');
                        }, k * 50, 'easeInOutExpo');
                    });
                }, 100);
            }
        }, { offset: '95%' });
    };
    contentWayPoint();

    // One-page navigation
    // One-page navigation
    var OnePageNav = function () {
        $(".smoothscroll[href^='#'], #ftco-nav ul li a[href^='#']").on('click', function (e) {
            e.preventDefault();
            var hash = this.hash;
            var navToggler = document.querySelector('.navbar-toggler'); // Use native querySelector

            // Smooth scroll to the target section
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 700, 'easeInOutExpo', function () {
                window.location.hash = hash;
            });

            // Close the navigation menu if it's visible (toggler behavior)
            if (navToggler && window.getComputedStyle(navToggler).display !== 'none') {
                navToggler.click(); // Trigger click to collapse the navbar
            }
        });
    };
    OnePageNav();

    // Magnific Popup for images and videos
    if ($.fn.magnificPopup) {
        $('.image-popup').magnificPopup({
            type: 'image',
            closeOnContentClick: true,
            closeBtnInside: true,
            fixedContentPos: true,
            mainClass: 'mfp-no-margins mfp-with-zoom',
            gallery: {
                enabled: true,
                navigateByImgClick: true,
                preload: [0, 1]
            },
            image: {
                verticalFit: true
            },
            zoom: {
                enabled: true,
                duration: 300
            }
        });

        $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
            disableOn: 700,
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            fixedContentPos: false
        });
    }

    // Datepicker and Timepicker
    $('#appointment_date').datepicker({
        format: 'm/d/yyyy',
        autoclose: true
    });

    $('#appointment_time').timepicker();

})(jQuery);
