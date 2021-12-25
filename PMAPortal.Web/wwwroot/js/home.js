$(() => {
    $('#trackBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let trackNo = $.trim($('#trackNo').val());

                if (trackNo == '') {
                    notify('Track nnumber is required', 'warning');
                } else {
                    let isValid = await validateTrackNumber(trackNo);
                    if (isValid) {
                        location.assign(`${$base}applications/track/${trackNo}`);
                    } else {
                        notify('Track number is invalid!', 'danger');
                    }
                }
            }
        } catch (ex) {
            console.error(ex);
            notify('Something went wrong! Please refresh your browser and try again');
        }
    });

});

function validateTrackNumber(trackNo) {
    var promise = new Promise((resolve, reject) => {
        try {
            var loader = bootLoaderDialog('Validating track number...');
            $.ajax({
                type: 'GET',
                url: `${$base}applications/ValidateTrackNumber/${trackNo}`,
                success: (response) => {
                    loader.hide();
                    if (response.isSuccess) {
                        resolve(true);
                    } else {
                        resolve(false);
                    }
                },
                error: (req, status, err) => {
                    loader.hide();
                    ajaxErrorHandler(req, status, err, {
                        callback: () => {}
                    });
                    reject(null);
                }
            })
        } catch (ex) {
            loader.hide();
            console.log(ex);
            reject(ex.message ? ex.message:ex);
        }
    });
    return promise;
}