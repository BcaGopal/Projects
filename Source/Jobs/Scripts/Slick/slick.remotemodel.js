(function ($) {
    /***
     * A sample AJAX data store implementation.
     * Right now, it's hooked up to load search results from Octopart, but can
     * easily be extended to support any JSONP-compatible backend that accepts paging parameters.
     */
    function RemoteModel() {
        // private
        var PAGESIZE = 100;
        var data = { length: 0 };
        var searchstr = "";
        var sortcol = null;
        var sortdir = 1;
        var h_request = null;
        var req = null; // ajax request

        // events
        var onDataLoading = new Slick.Event();
        var onDataLoaded = new Slick.Event();


        function init() {
        }


        function isDataLoaded(from, to) {
            for (var i = from; i <= to; i++) {
                if (data[i] == undefined || data[i] == null) {
                    return false;
                }
            }

            return true;
        }


        function clear() {
            for (var key in data) {
                delete data[key];
            }
            data.length = 0;
        }


        function ensureData(from, to) {
            if (req) {
                req.abort();
                for (var i = req.fromPage; i <= req.toPage; i++)
                    data[i * PAGESIZE] = undefined;
            }

            if (from < 0) {
                from = 0;
            }

            if (data.length > 0) {
                to = Math.min(to, data.length - 1);
            }

            var fromPage = Math.floor(from / PAGESIZE);
            var toPage = Math.floor(to / PAGESIZE);

            while (data[fromPage * PAGESIZE] !== undefined && fromPage < toPage)
                fromPage++;

            while (data[toPage * PAGESIZE] !== undefined && fromPage < toPage)
                toPage--;

            if (fromPage > toPage || ((fromPage == toPage) && data[fromPage * PAGESIZE] !== undefined)) {
                // TODO:  look-ahead
                onDataLoaded.notify({ from: from, to: to });
                return;
            }

            var url = "/PurchaseQuotationHeader/ProductIndex?search=" + searchstr + "&start=" + (fromPage * PAGESIZE) + "&count=" + (((toPage - fromPage) * PAGESIZE) + PAGESIZE);

            if (sortcol != null) {
                url += ("&sortby=" + sortcol + ((sortdir > 0) ? "+asc" : "+desc"));
            }

            if (h_request != null) {
                clearTimeout(h_request);
            }

            h_request = setTimeout(function () {
                for (var i = fromPage; i <= toPage; i++)
                    data[i * PAGESIZE] = null; // null indicates a 'requested but not available yet'

                onDataLoading.notify({ from: from, to: to });

                req = $.jsonp({
                    url: url,
                    callbackParameter: "callback",
                    cache: true,
                    success: onSuccess,
                    error: function () {
                        onError(fromPage, toPage)
                    }
                });
                req.fromPage = fromPage;
                req.toPage = toPage;
            }, 50);
        }


        function onError(fromPage, toPage) {
            alert("error loading pages " + fromPage + " to " + toPage);
        }


        ////SAMPLE DATA
        //var tmp = {
        //    "hits": 10,
        //    "request": {
        //        "start": 0,
        //    },
        //    "results": {
        //        "item": [
        //          {
        //              "title": "CAE-1001",
        //              "description": "CAE-1001",
        //              "url": "https://192.168.2.110:44309/Uploads/84283ad0-b56b-46d2-9aa2-03d8bb02d3cc/Thumbs/CAE-1001_c2e41481-d5ac-48fb-935d-f62883fd80d5.PNG",
        //          },
        //          {
        //              "title": "CAE-1003",
        //              "description": "CAE-1003",
        //              "url": "https://192.168.2.110:44309/Uploads/b8a1eb7b-ce50-4eb9-bfa4-e41b1d669cc6/Thumbs/CAE-1003_c5819767-084f-40b9-949b-f446ce3c53b7.PNG",
        //          },
        //          {
        //              "title": "CAE-1007",
        //              "description": "CAE-1007",
        //              "url": "https://192.168.2.110:44309/Uploads/d32f6b40-91c4-4d77-8d41-5e940e483a7d/Thumbs/CAE-1007_0cc3dce7-f16e-4223-92ae-d5cf938d5006.jpg",
        //          },
        //        ]
        //    }
        //};

        function onSuccess(tmp) {
            var from = tmp.request.start, to = from + tmp.results.length;
            data.length = parseInt(tmp.hits);

            for (var i = 0; i < tmp.results.length; i++) {
                var item = tmp.results[i];

                data[from + i] = item;
                data[from + i].index = from + i;
            }

            req = null;

            onDataLoaded.notify({ from: from, to: to });
        }


        function reloadData(from, to) {
            for (var i = from; i <= to; i++)
                delete data[i];

            ensureData(from, to);
        }


        function setSort(column, dir) {
            sortcol = column;
            sortdir = dir;
            clear();
        }

        function setSearch(str) {
            searchstr = str;
            clear();
        }


        init();

        return {
            // properties
            "data": data,

            // methods
            "clear": clear,
            "isDataLoaded": isDataLoaded,
            "ensureData": ensureData,
            "reloadData": reloadData,
            "setSort": setSort,
            "setSearch": setSearch,

            // events
            "onDataLoading": onDataLoading,
            "onDataLoaded": onDataLoaded
        };
    }

    // Slick.Data.RemoteModel
    $.extend(true, window, { Slick: { Data: { RemoteModel: RemoteModel } } });
})(jQuery);