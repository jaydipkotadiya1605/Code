// slick slider
$('.slides').slick({
	autoplay: true,
	arrows: false,
	dots: true
});

// Slider
$('.slides').on('beforeChange', function (e, slick, currentSlide, nextSlide) {
	$('.slider .category').text(sliderData[nextSlide].Category);
	$('.slider .title').text(sliderData[nextSlide].Title);
	$('.slider .more').attr('href', sliderData[nextSlide].more);
});