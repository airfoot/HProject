$(function () {
    $('.submenu').click(function (e) {



        $(this).next().slideToggle();

        var el = $(this).children("span").last();
        if (el.hasClass('fa-caret-right')) {
            el.removeClass('fa-caret-right');
            el.addClass('fa-caret-down');
        } else {
            el.removeClass('fa-caret-down');
            el.addClass('fa-caret-right');
        }
        e.stopPropagation();
    });


})