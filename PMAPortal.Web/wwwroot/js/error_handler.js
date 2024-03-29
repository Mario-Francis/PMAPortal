﻿function ajaxErrorHandler(req, status, err, { callback = null }) {
    let res = req.responseJSON;
    if (req.status == 401) {
        notify(res.message, 'danger', "Unauthorized");
    } else if (req.status == 400) {
        let eItems = '';
        if (res) {
            if (res.errorItems != null) {
                eItems = '<ul class="f14">';
                res.errorItems.forEach((v, i) => {
                    eItems += `<li>${i + 1}. ${v}</li>`;
                });
                eItems += '</ul>';
            }
            notify(res.message + eItems, 'danger', "Validation Error");
        } else {
            notify('Something went wrong while submitting your request. Please refresh your browser and try again.', 'danger');
        }
        
    } else if (req.status == 500) {
        if (res) {
            notify(res.message, 'danger');
            console.log(res.errorDetail)
        } else {
            notify('Something went wrong while submitting your request. Please refresh your browser and try again.', 'danger');
            console.error(req);
        }
        
    } else {
        notify('Something went wrong while submitting your request. Please refresh your browser and try again.', 'danger');
        console.error(req);
    }
    if (callback != null)
        callback();
}