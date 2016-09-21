var ReportViewerForMvc = ReportViewerForMvc || (function () {

    var _iframeId = {};

    return {
        SetIframeId: function (value) {
            _iframeId = '#' + value;
        },
        GetIframeId: function () {
            return _iframeId;
        }
    };
}());

window.addEventListener('message', function (e) {
    $(ReportViewerForMvc.GetIframeId()).height(e.data.height);
    $(ReportViewerForMvc.GetIframeId()).width(e.data.width);
});