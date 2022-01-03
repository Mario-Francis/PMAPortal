$(() => {
    $(".chosen-select").chosen({ disable_search_threshold: 10 });

    if ($('#status')) {
        $('#status').on('change', (e) => {
            let val = $(e.currentTarget).val();
            if (val == 5 || val == 7) {
                $('#comment').attr('required', true);
                $('#commentDiv').show();
            } else {
                $('#comment').val('').attr('required', false);
                $('#commentDiv').hide();
            }
        });
    }

    $('#updateBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let id = $.trim($('#applicationId').val());
                let statusId = $.trim($('#status').val());
                let comment = $('#comment').val().trim() == '' ? null : $('#comment').val().trim();
                let __RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

                if (statusId == '') {
                    notify('All fields are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<span class="fa fa-circle-notch fa-spin"></span> Updating status...');
                    let url = $base + 'applications/UpdateSatus';
                    let data = {
                        applicationId: id,
                        statusId,
                        comment,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                notify(response.message + '<br /><i class="fa fa-circle-notch fa-spin"></i> Refreshing...', 'success');
                                form.reset();
                                setTimeout(() => {
                                    location.reload();
                                }, 2000);

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
                                    $('fieldset').prop('disabled', false);
                                }
                            });
                        }
                    });
                }
            }
        } catch (ex) {
            console.error(ex);
            notify(ex.message, 'danger');
            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
            $('fieldset').prop('disabled', false);
        }
    });

    $('#assignBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[$("form").length-1];
            if (validateForm(form)) {
                let id = $.trim($('#applicationId').val());
                let installerId = $.trim($('#installer').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[$('input[name=__RequestVerificationToken]').length-1]).val();

                if (installerId == '') {
                    notify('All fields are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<span class="fa fa-circle-notch fa-spin"></span> Assigning installer...');
                    let url = $base + 'applications/AssignInstaller';
                    let data = {
                        applicationId: id,
                        installerId,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                notify(response.message + '<br /><i class="fa fa-circle-notch fa-spin"></i> Refreshing...', 'success');
                                form.reset();
                                setTimeout(() => {
                                    location.reload();
                                }, 2000);

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Assign');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-check-circle"></i> &nbsp;Assign');
                                    $('fieldset').prop('disabled', false);
                                }
                            });
                        }
                    });
                }
            }
        } catch (ex) {
            console.error(ex);
            notify(ex.message, 'danger');
            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Assign');
            $('fieldset').prop('disabled', false);
        }
    });
});