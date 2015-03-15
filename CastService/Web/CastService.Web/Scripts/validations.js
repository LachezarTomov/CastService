$(document).ready(function () {
    jQuery.fn.ForceNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;

                // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                //	alert(key);
                return (
                    key === 8 || key === 9 || key === 46 || key === 189 ||
                    (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)
                );
            });
        });
    };
    jQuery.fn.ForceFloatOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;

                // allow dot, comma, backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                return (
                    key === 8 || key === 9 || key === 46 ||
                    key === 109 || key === 110 || key === 189 || key === 190 ||
                    (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)
                );
            });
        });
    };

    function isInt(data) {
        data = parseInt(data);
        if (Math.floor(data) === data && $.isNumeric(data)) {
            return true;
        }
        return false;
    }

    function isHourCorrect(txtHour) {
        var hourArray = txtHour.split(":");
        if (hourArray[0] > 59 || hourArray[1] > 59) {
            return false;
        }

        return true;
    }

    function isDate(txtDate) {
        var currVal = txtDate;
        if (currVal === '') {
            return false;
        }

        //Declare Regex 
        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray === null) {
            return false;
        }
        //Checks for dd/mm/yyyy format.
        dtDay = dtArray[1];
        dtMonth = dtArray[3];
        dtYear = dtArray[5];

        if (dtMonth < 1 || dtMonth > 12) {
            return false;
        }
        else if (dtDay < 1 || dtDay > 31) {
            return false;
        }
        else if ((dtMonth === 4 || dtMonth === 6 || dtMonth === 9 || dtMonth === 11) && dtDay === 31) {
            return false;
        }
        else if (dtMonth === 2) {
            var isleap = (dtYear % 4 === 0 && (dtYear % 100 !== 0 || dtYear % 400 === 0));
            if (dtDay > 29 || (dtDay === 29 && !isleap)) {
                return false;
            }
        }
        return true;
    }

})