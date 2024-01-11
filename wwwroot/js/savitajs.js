(function ($) {
    $(document).ready(function () {
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

        $('#owl-carousel-km').owlCarousel({
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

        $('.bg_row').owlCarousel({
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
                    items: 4
                }
            }
        })

        $('.news_slider').owlCarousel({
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
                    items: 3
                }
            }
        })

        $('.thumbimg').owlCarousel({
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
                    items: 5
                }
            }
        })

        $('.imgSliderSP').owlCarousel({
            loop: true,
            margin: 0,
            autoplay: true,
            nav: true,
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

        $('.thumbimgDoiTac').owlCarousel({
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
                    items: 7
                }
            }
        })
        $('.bg_row_dt').owlCarousel({
            loop: true,
            margin: 10,
            nav: true,
            dots: true,
            autoplay: false,
            autoplayTimeout: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 2
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 4
                }
            }
        })

        $('.bg_row_dt_now').owlCarousel({
            loop: true,
            margin: 10,
            nav: true,
            dots: true,
            autoplay: false,
            autoplayTimeout: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 2
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        })

        $('#newsrlt').owlCarousel({
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
                    items: 3
                }
            }
        })

        $('#aboutsv').owlCarousel({
            loop: true,
            margin: 10,
            nav: true,
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

        $('.catnote').owlCarousel({
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
                    items: 4
                }
            }
        })

        $("#btndangky5").click(function () {
            var phoneRegEx = "/\\(?\\d{3}\\)?[-\\/\\.\\s]?\\d{3}[-\\/\\.\\s]?/";
            var re = /[A-Z0-9._%+-]+@+.[A-Z]{2,4}/igm;
            var hovaten = $("#name").val();
            var noidung = "Đăng ký thông tin dự án";
            var email = $("#email").val();
            var tieude = $("#subject").val();
            if (hovaten == "") {
                alert("Vui lòng Nhập họ tên");
            }
            else if (email == "") {
                alert("Vui lòng nhập số Email");
            }
            else if (tieude == "") {
                alert("Vui lòng nhập tiêu đề liên hệ");
            }
            else if (noidung == "") {
                alert("Vui lòng nhập nội dung liên hệ");a
            }
            else {
                $.ajax({
                    url: '/api/Cascading/ContactDangKy1',
                    type: 'GET',
                    dataType: 'json',
                    data: { Hoten: hovaten, email: email, tieude: tieude, noidung: noidung },
                    success: function (d) {
                        $('.divCart').html(d)
                        $('.divCart').show(250).delay(2000).hide(250);
                        setTimeout(function () { window.location.reload(true); }, 1000);
                    },
                    error: function () {
                        alert('Error!');
                    }
                });
            }
        });
    });
})(jQuery);