(function ($) {
    $.fn.menumaker = function (options) {
        var cssmenu = $(this), settings = $.extend({
            format: "dropdown",
            sticky: false
        }, options);
        return this.each(function () {
            $(this).find(".button").on('click', function () {
                $(this).toggleClass('menu-opened');
                var mainmenu = $(this).next('ul');
                if (mainmenu.hasClass('open')) {
                    mainmenu.slideToggle().removeClass('open');
                }
                else {
                    mainmenu.slideToggle().addClass('open');
                    if (settings.format === "dropdown") {
                        mainmenu.find('ul').show();
                    }
                }
            });
            $(this).find("#head-mobile").on('click', function () {
                $(this).toggleClass('menu-opened');
                $(".button").toggleClass('menu-opened');
                var mainmenu = $(this).nextAll('ul').slice(0, 2);
                if (mainmenu.hasClass('open')) {
                    mainmenu.slideToggle().removeClass('open');
                }
                else {
                    mainmenu.slideToggle().addClass('open');
                    if (settings.format === "dropdown") {
                        mainmenu.find('ul').show();
                    }
                }
            });
            cssmenu.find('li ul').parent().addClass('has-sub');
            multiTg = function () {
                cssmenu.find(".has-sub").prepend('<span class="submenu-button"></span>');
                cssmenu.find('.submenu-button').on('click', function () {
                    $(this).toggleClass('submenu-opened');
                    if ($(this).siblings('ul').hasClass('open')) {
                        $(this).siblings('ul').removeClass('open').slideToggle();
                    }
                    else {
                        $(this).siblings('ul').addClass('open').slideToggle();
                    }
                });
            };
            if (settings.format === 'multitoggle') multiTg();
            else cssmenu.addClass('dropdown');
            if (settings.sticky === true) cssmenu.css('position', 'fixed');
            resizeFix = function () {
                var mediasize = 1000;
                if ($(window).width() > mediasize) {
                    cssmenu.find('ul').show();
                }
                if ($(window).width() <= mediasize) {
                    cssmenu.find('ul').hide().removeClass('open');
                }
            };
            resizeFix();
            return $(window).on('resize', resizeFix);
        });
    };
})(jQuery);


(function ($) {
    $(document).ready(function () {
        $("#cssmenu").menumaker({
            format: "multitoggle"
        });
        $('#owl-carousel').owlCarousel({
            loop: true,
            margin: 10,
            nav: false,
            dots: false,
            autoplay: false,
            autoplayTimeout: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 3
                },
                600: {
                    items: 3
                },
                1000: {
                    items: 3
                }
            }
        })

        $('#myCarousel2').owlCarousel({
            loop: true,
            margin: 10,
            nav: false,
            dots: false,
            autoplay: false,
            autoplayTimeout: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        })

        var owl = $('.owl-carousel');

        // Carousel initialization
        owl.owlCarousel({
            loop: false,
            margin: 0,
            navSpeed: 500,
            nav: true,
            navText: ["<img src='/Theme/Monday/images/owprew.png'>","<img src='/Theme/Monday/images/ownext.png'>"],
            autoplay: true,
            rewind: true,
            dots: true,
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 3
                }
            }
        });

        var owl1 = $('.owl-carousel1');

        // Carousel initialization
        owl1.owlCarousel({
            loop: true,
            margin: 0,
            navSpeed: 500,
            nav: false,
            navText: ["<img src='/Theme/Monday/images/owprew.png'>", "<img src='/Theme/Monday/images/ownext.png'>"],
            autoplay: true,
            //rewind: true,
            dots: false,
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });

        $('#owlbn').owlCarousel({
            loop: true,
            margin: 10,
            nav: false,
            dots: false,
            autoplay: false,
            autoplayTimeout: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 3
                },
                600: {
                    items: 3
                },
                1000: {
                    items: 3
                }
            }
        })      
    });
})(jQuery);