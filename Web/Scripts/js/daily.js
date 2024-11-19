
$(document).ready(function () {

    $("#filterCodeCurrency").click(function () {
        const startDate = document.getElementById("startDate").value ? document.getElementById("startDate").value : null;
        const endDate = document.getElementById("endDate").value ? document.getElementById("endDate").value : null;
        const codeCurrency = document.getElementById("codeCurrency").value ? document.getElementById("codeCurrency").value : null;
        filterData(startDate, endDate, codeCurrency);
    });

    fetchCurrencyData();
    function fetchCurrencyData() {
        $.ajax({
            url: '/Currency/GetDailyCurrency',
            type: 'GET',
            data: {},
            dataType: 'json',
            success: function (data) {
                displayCurrencyData(data);
                displayCrossCurrencies(data);
                displaySDRCurrencies(data)

            },
            error: function (xhr, status, error) {
                console.error('AJAX hatası:', status, error);
            }
        });
    }
    //Filtreleme Ajax Kodları
    function filterData(startDate, endDate, codeCurrency) {
        var postData = {
            StartDate: startDate,
            EndDate: endDate,
            CodeCurrency: codeCurrency
        };

        $.ajax({
            url: '/Currency/GetFilterDailyCurrency',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(postData),
            dataType: 'json',
            success: function (data) {
                displayCurrencyData(data);
                displayCrossCurrencies(data);
                displaySDRCurrencies(data)
            },
            error: function (xhr, status, error) {
                console.error('AJAX hatası:', status, error);
            }
        });
    }

    function displayCurrencyData(data) {

        $('#currencyTable tbody').empty();
        $.each(data, function (index, currency) {
            var formatDate = fomateDate(currency.Date);
            $('#currencyTable tbody').append(
                '<tr>' +
                '<td>' + formatDate + '</td>' +
                '<td>' + currency.CurrencyCode + "/TRY" + '</td>' +
                '<td>' + currency.Unit + '</td>' +
                '<td>' + currency.CurrencyName + '</td>' +
                '<td>' + (currency.ForexBuying !== 0 ? currency.ForexBuying : "") + colorChange(currency.FBChangeRate) + '</td>' +
                '<td>' + (currency.ForexSelling !== 0 ? currency.ForexSelling : "") + colorChange(currency.FSChangeRate) + '</td>' +
                '<td>' + (currency.BanknoteBuying !== 0 ? currency.BanknoteBuying : "") + colorChange(currency.BBChangeRate) + '</td>' +
                '<td>' + (currency.BanknoteSelling !== 0 ? currency.BanknoteSelling : "") + colorChange(currency.BSChangeRate) + '</td>' +
                '</tr>'
            );
        });
    }
    function displaySDRCurrencies(data) {
        $('#currencySDRTable tbody').empty();
        $.each(data, function (index, currency) {
            var formatDate = fomateDate(currency.Date);
            if (currency.ForexBuying !== 0 && currency.CurrencyCode == 'XDR') {
                $('#currencySDRTable tbody').append(
                    '<tr>' + 
                    '<td>' + formatDate + '</td>' +
                    '<td>' + currency.CurrencyCode + '/TRY</td>' +
                    '<td>' + currency.Unit + '</td>' +
                    '<td>US DOLLAR</td>' +
                    '<td>' + currency.CrossRateUSD + colorChange(currency.CRUsdChangeRate) + '</td>' +
                    '<td>' + currency.CurrencyName + '</td>' +
                    '</tr>'
                );
            }
            if (currency.CrossRateOther !== 0 && currency.CurrencyCode == 'XDR') {
                $('#currencySDRTable tbody').append(
                    '<tr>' + 
                    '<td>' + formatDate + '</td>' +
                    '<td>' + currency.CurrencyCode + '/USD</td>' +
                    '<td>' + currency.Unit + '</td>' +
                    '<td>' + currency.CurrencyName + '</td>' +
                    '<td>' + currency.CrossRateOther + colorChange(currency.CROChangeRate) + '</td>' +
                    '<td>US DOLLAR</td>' +
                    '</tr>'
                );
            }
        });
    }
    //Cross Currencies Function
    function displayCrossCurrencies(data) {
        $('#crossCurrencyTable tbody').empty();

        $.each(data, function (index, currency) {
            var formatDate = fomateDate(currency.Date);
            if (currency.CrossRateUSD !== 0) {
                $('#crossCurrencyTable tbody').append(
                    '<tr>' +
                    '<td>' + formatDate + '</td>' +
                    '<td>USD/' + currency.CurrencyCode + '</td>' +
                    '<td>' + currency.Unit + '</td>' +
                    '<td>US DOLLAR</td>' +
                    '<td>' + currency.CrossRateUSD + colorChange(currency.CRUsdChangeRate) + '</td>' +
                    '<td>' + currency.CurrencyName + '</td>' +
                    '</tr>'
                );
            }
            if (currency.CrossRateOther !== 0) {
                $('#crossCurrencyTable tbody').append(
                    '<tr>' +
                    '<td>' + formatDate + '</td>' +
                    '<td>' + currency.CurrencyCode + '/USD</td>' +
                    '<td>' + currency.Unit + '</td>' +
                    '<td>' + currency.CurrencyName + '</td>' +
                    '<td>' + currency.CrossRateOther + colorChange(currency.CROChangeRate) + '</td>' +
                    '<td>US DOLLAR</td>' +
                    '</tr>'
                );
            }

        });
    }
    //function signControl(changeRate) {

    //    var color; //= changeRate >= 0 ? 'green' : 'red';
    //    if (changeRate === 0) {
    //        return '';
    //    }
    //    if (changeRate > 0) {
    //        color = 'green';
    //    } else  {
    //        color = 'red';
    //    }
    //    var arrow;
    //    if (changeRate >= 0) {
    //        arrow = '▲';
    //    } else {
    //        arrow = '▼';
    //    }
    //    var changeRateFormatted = changeRate.toFixed(2) + '%';
    //    return '<span style="color: ' + color + '">' + '(' + arrow + ' ' + changeRateFormatted + ')' + '</span>';
    //}
    function colorChange(changeRate) {
        var color;
        var changeAmount;
        if (changeRate > 0) {
            color = 'green';
            changeAmount = '+' + changeRate.toFixed(2) + '%';
        } else if (changeRate < 0) {
            color = 'red';
            changeAmount = changeRate.toFixed(2) + '%';
        } else {
            return '';
        }

        return '<span class="fw-medium" style="color: ' + color + '">' + '(' + changeAmount + ')' + '</span>';
    }

    function fomateDate(dateString) {
        // Verilen stringi sayısal kısmı al
        var unixTime = parseInt(dateString.match(/\d+/)[0]);
        // Unix zamanını Date objesine çevir
        var date = new Date(unixTime);
        // Tarih ve saat bilgisini istediğiniz formata dönüştür
        var formattedDate = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        var formattedTime = ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2);
        var formattedDateTime = formattedDate + " " + formattedTime;
        return formattedDateTime;
    }



    //function fomateDate(unixTime) {
    //    var date = new Date(unixTime);
    //    var formattedDate = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
    //    var formattedTime = ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2);
    //    var formattedDateTime = formattedDate + " " + formattedTime;
    //    return formattedDateTime;

    //}
});