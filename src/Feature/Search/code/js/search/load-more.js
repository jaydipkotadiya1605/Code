var url = window.location.origin + window.location.pathname;
var loadmoreParams = getUrlVars();

$('.full-btn').click(function (event) {
    event.preventDefault();
    var renderingId = this.attributes['data-rendering-Id'] ? this.attributes['data-rendering-Id'].value : "";
    var pageIndex = parseInt(this.attributes['data-page-index'] ? this.attributes['data-page-index'].value : "1") || 1;
    var anchor = this.attributes['data-anchor'] ? this.attributes['data-anchor'].value : "";
    loadmoreParams.pageIndex = pageIndex + 1;
    loadmoreParams.renderingId = renderingId;

    navigateWithParams(loadmoreParams, anchor);
});