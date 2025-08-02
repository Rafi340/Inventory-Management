function fetchData(url, method, params, callback) {
    $.ajax({
        url: url,
        type: method,
        data: params,
        success: function (data, status) {
            if (callback) callback(data, status);
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            if (callback) callback(null, status);
        }
    });
}
