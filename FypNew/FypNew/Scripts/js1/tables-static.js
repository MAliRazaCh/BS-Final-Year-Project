/*! light-blue - v3.2.0 - 2015-10-05 */$(function(){function a(){$(".widget").widgster(),$(".sparkline").each(function(){$(this).sparkline("html",$(this).data())}),$(".js-progress-animate").animateProgressBar()}a(),PjaxApp.onPageLoad(a)});