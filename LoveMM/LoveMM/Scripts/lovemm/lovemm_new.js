

/*  类目切换  */
$(function () {
    if ($('.teamwork em.current').length < 1) {
        $('.teamwork em').eq(0).addClass('current')
    }
    $('.friendship').css('display', 'none').eq($(".teamwork em.current").index()).css('display', 'block');
    $('.teamwork em').hover(
    function () {
        $(this).addClass('current').siblings().removeClass('current');
        $('.friendship').css('display', 'none').eq($(this).index()).css('display', 'block')
    }
    )
})


/*  banner脚本  */


$(function () {
    //滚动广告
    if ($("#bn_bnt").length > 0) {
        $("#bn_bnt").switchable("#slide-panel > div > span ", {
            effect: "scroll",
            currentCls: "corrent",
            speed: 1
        }).autoplay(3).carousel();
    }
});



$(function () {
    $('.serach input').hover(
      function () {
          $(this).css({ "border": "solid 1px #EF4C79" })
      },
    function () {
        $(this).css({ "border": "solid 1px #ffffff" })
    }
        )
})
$(function () {
    $('.mv_zt_list li').hover(

                        function () {
                            $(this).addClass('hover')
                        },
                        function () {
                            $(this).removeClass('hover')
                        }

    )


})