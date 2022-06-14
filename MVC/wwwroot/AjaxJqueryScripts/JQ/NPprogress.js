////$(document).ready(function () {
////    NProgress.start();
////});
////$(window).on("load", function () {
////    NProgress.done();
////});
jQuery(document).ajaxStart(function () {
    NProgress.start();
});

jQuery(document).ajaxStop(function () {
    NProgress.done();
});