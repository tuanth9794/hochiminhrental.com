(function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
        'gtm.start':
            new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
        j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-PRZG3L6');

window.fbAsyncInit = function () {
    FB.init({
        xfbml: true,
        version: 'v3.2'
    });
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/vi_VN/sdk/xfbml.customerchat.js';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

$(document).ready(function () {
    var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth;
    if (x > 1000) {
        var html = "<div class='fb-customerchat'   attribution = setup_tool page_id = '1947666421968879' theme_color = '#fded52' logged_in_greeting = 'MONDAY vàng tươi xin chào các bạn!'  logged_out_greeting = 'MONDAY vàng tươi xin chào các bạn!' ></div >";
        $("#chatfb").append(html);
    }
    else {
        var html = "<div class='fb-customerchat' greeting_dialog_display='hide' greeting_dialog_display='true'   attribution=setup_tool page_id = '1947666421968879' theme_color = '#fded52' logged_in_greeting = 'MONDAY xin chào các bạn!'  logged_out_greeting = 'MONDAY  xin chào các bạn!' ></div >";
        $("#chatfb").append(html);
    }
});