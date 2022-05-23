$(".play_yt").click(function () {
    $(this).hide();
    $(".ytvideo").attr(
        "src",
        "https://www.youtube.com/embed/ciibp6fAcYo?autoplay=1"
    );
});
var swiper = new Swiper(".swiper-container", {
    slidesPerView: 1,
    pagination: {
        el: ".swiper-pagination",
        dynamicBullets: true,
    },
});
var swiperMenu = new Swiper(".swiper-container-menu", {
    slidesPerView: "auto",
    spaceBetween: 63,
    pagination: {
        el: ".swiper-menu-pagination",
        type: "progressbar",
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});