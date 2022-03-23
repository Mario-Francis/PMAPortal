$(() => {

    $('#submitBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let installationId = $.trim($('#installationId').val());
                let customerId = $.trim($('#customerId').val());
                let answer1 = $.trim($('#a1').val());
                let answer2 = $.trim($('#a2').val());
                let answer3 = $.trim($('#a3').val());
                let answer4 = $.trim($('#a4').val());
                let answer5 = $.trim($('#a5').val());
                let question1Id = $.trim($('#a1').attr('qid'));
                let question2Id = $.trim($('#a2').attr('qid'));
                let question3Id = $.trim($('#a3').attr('qid'));
                let question4Id = $.trim($('#a4').attr('qid'));
                let question5Id = $.trim($('#a5').attr('qid'));

                let comment = $('#comment').val().trim() == '' ? null : $('#comment').val().trim();
                let __RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

                if (answer1 == '' || answer2 == '' || answer3 == '' || answer4 == '' || answer5 == '') {
                    notify('All fields are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<span class="fa fa-circle-notch fa-spin"></span> Submitting feedback...');
                    let url = $base + 'feedbacks/Feedback';
                    let data = {
                        installationId,
                        customerId,
                        answer1,
                        answer2,
                        answer3,
                        answer4,
                        answer5,
                        question1Id,
                        question2Id,
                        question3Id,
                        question4Id,
                        question5Id,
                        comment,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                notify(response.message + '<br /><i class="fa fa-circle-notch fa-spin"></i> Redirecting...', 'success');
                                form.reset();
                                setTimeout(() => {
                                    location.replace($base + 'applications/FeedbackReceived');
                                }, 2000);

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
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
            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
            $('fieldset').prop('disabled', false);
        }
    });

});