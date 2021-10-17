//#region Notifications

var popupNotification;

$(document).ready(function () {

    popupNotification = $("#popupNotification").kendoNotification({
        autoHideAfter: 3000,
        position: {
            right: 50,
            top: 30
        },
        stacking: "down"
    }).data("kendoNotification");

    $("#showPopupNotification").click(function () {
        var d = new Date();
        popupNotification.show(kendo.toString(d, 'HH:MM:ss.') + kendo.toString(d.getMilliseconds(), "000"), "error");
    });

    $("#hideAllNotifications").click(function () {
        popupNotification.hide();
        //staticNotification.hide();
    });
});

function ShowMessageInternal (message, msgType) {
    // Remove previous notifications (if any) before showing new one(s)
    //popupNotification.getNotifications().each(function () {
    //    $(this).parent().remove();
    //});

    if (Array.isArray(message))
        $(message).each(function (index, element) {
            popupNotification.show(element, msgType);
        });
    else
        popupNotification.show(message, msgType);
}

function ShowError (message) {
    ShowMessageInternal(message, "error");
}

function ShowMessage (message) {
    ShowMessageInternal(message, "info");
}

function ShowSuccess (message) {
    ShowMessageInternal(message, "success");
}

function ShowWarning (message) {
    ShowMessageInternal(message, "warning");
}

//#endregion

//#region Confirmation window

function ShowConfirmation (title, text, confirmCallback, denyCallback) {
    swal({
        title: title,
        text: text,
        icon: "warning",
        closeOnClickOutside: false,
        buttons: ['No', 'Yes'],
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {
            confirmCallback();
        } else {
            if (denyCallback)
                denyCallback();
        }
    });
}

//#endregion

//#region Kendo grid

/**
 * A helper function that refreshes Telerik Grid
 * @param {string} gridName Grid name.
 */
function refreshKendoGrid(gridName) {
    let grid = $(`#${gridName}`);

    if (grid) {
        try {
            grid.data("kendoGrid").dataSource.read();
            grid.data("kendoGrid").refresh();
        }
        catch (e) {
            console.log(e);
        }
    }
}

/**
 * A helper function, to collapse Telerik Grid group rows
 * @param {string} gridName Grid name.
 * @param {boolean} isLoading True/False.
 */
function setKendoGridLoading(gridName, isLoading) {
    let grid = $(`#${gridName}`);

    if (grid) {
        try {
            kendo.ui.progress(grid, isLoading);
        }
        catch (e) {
            console.log(e);
        }
    }
}

/**
 * A helper function that collapses Telerik Grid group rows
 * @param {string} gridName Grid name.
 */
function collapseKendoGridGroupRows(gridName) {
    let grid = $(`#${gridName}`).data("kendoGrid");
    var groupRows = grid.element.find(".k-grouping-row");
    groupRows.each(function (row) {
        grid.collapseRow(this);
    });
}

//#endregion

/**
 * Returns file size description in appropriate format. 
 * Example: Instead of "1024 KB", returns "1 MB".
 * @param {number} bytes Size in bytes.
 */
function formatBytes(bytes) {
    var marker = 1024; // Change to 1000 if required
    var decimal = 2; // Change as required
    var kiloBytes = marker; // One Kilobyte is 1024 bytes
    var megaBytes = marker * marker; // One MB is 1024 KB
    var gigaBytes = marker * marker * marker; // One GB is 1024 MB
    var teraBytes = marker * marker * marker * marker; // One TB is 1024 GB

    // return bytes if less than a KB
    if (bytes < kiloBytes) return bytes + " Bytes";
    // return KB if less than a MB
    else if (bytes < megaBytes) return (bytes / kiloBytes).toFixed(decimal) + " KB";
    // return MB if less than a GB
    else if (bytes < gigaBytes) return (bytes / megaBytes).toFixed(decimal) + " MB";
    // return GB if less than a TB
    else if (bytes < teraBytes) return (bytes / gigaBytes).toFixed(decimal) + " GB";
    // return TB
    else return (bytes / teraBytes).toFixed(decimal) + " TB";
}

function downloadURI(uri) {
    var link = document.createElement("a");
    link.style.display = 'none';
    //link.target = "_blank";
    link.href = uri;
    document.body.appendChild(link);
    link.click();
    link.remove();
}

function formatDate(dateArg, locale = 'it-IT') {
    if (!Date.parse(dateArg))
        return;

    let date = new Date(dateArg);
    return date.toLocaleDateString(locale, { year: 'numeric', month: 'short', day: 'numeric'});
}

function formatTime(dateArg, locale = 'it-IT') {
    if (!Date.parse(dateArg))
        return;

    let date = new Date(dateArg);
    return date.toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit', hourCycle: 'h24' });
}

function startCountdown(dateArg, headlineArg) {
    if (!Date.parse(dateArg))
        return;
    const second = 1000,
        minute = second * 60,
        hour = minute * 60,
        day = hour * 24;

    let startDate = dateArg,
        countDown = new Date(startDate).getTime(),
        x = setInterval(function () {

            let now = new Date().getTime(),
                distance = countDown - now;

            document.getElementById("days").innerText = Math.floor(distance / (day)),
                document.getElementById("hours").innerText = Math.floor((distance % (day)) / (hour)),
                document.getElementById("minutes").innerText = Math.floor((distance % (hour)) / (minute)),
                document.getElementById("seconds").innerText = Math.floor((distance % (minute)) / second);

            //do something later when date is reached
            if (distance < 0) {
                let headline = document.getElementById("headline"),
                    countdown = document.getElementById("countdown"),
                    spinningDots = document.getElementById("spinning-dots");

                headline.innerText = headlineArg;
                countdown.remove();
                spinningDots.style.display = "block";

                clearInterval(x);
            }
            //seconds
        }, 0)
}