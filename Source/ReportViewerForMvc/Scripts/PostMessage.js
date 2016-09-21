Sys.Application.add_load(function () {
    $find("ReportViewer1").add_propertyChanged(viewerPropertyChanged);
});

function viewerPropertyChanged(sender, e) {
    if (e.get_propertyName() === "isLoading") {
        var controlSize = {
            height: document.body.scrollHeight,
            width: document.body.scrollWidth
        };
        top.postMessage(controlSize, '*');
    }
}

//TODO: Get control ID dynamically.