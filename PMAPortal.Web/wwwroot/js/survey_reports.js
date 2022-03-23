var allTable, completedTable;

$(() => {
    if ($('#allTable')) {
        var exclude = [0, 1, 4, 13, 15, 17, 20];
        $('#allTable tfoot th').each(function (i, v) {
            if (!exclude.includes(i)) {
                var title = $(this).text();
                $(this).html('<input type="text" class="form-control bg-light f12" style="min-width:64px;" placeholder="\u{2315} ' + title + '" />');
            } else {
                $(this).html('');
            }
        });
        allTable = $('#allTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'reports/AllSurveysDataTable',
                type: "POST"
            },
            "order": [[13, "desc"]],
            "lengthMenu": [10, 20, 30, 50, 100],
            "paging": true,
            autoWidth: false,
            initComplete: function () {
                var r = $('#allTable tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', '4px 8px');
                });
                $('#allTable thead').append(r);
                $('#search_0').css('text-align', 'center');

                // Apply the search
                this.api().columns().every(function () {
                    var that = this;

                    $('input', this.footer()).on('keyup change clear', function () {
                        if (that.search() !== this.value) {
                            that.search(this.value)
                                .draw();
                        }
                    });
                });
            },
            //rowId: 'id',
            dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
                "<'row'<'col-sm-12'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [{
                extend: 'excel',
                text: `<i class="fa fa-file-excel"></i> Export to excel`,
                className: 'f14 btn-success py-1 my-1 mx-1'
            },
            {
                extend: 'pdfHtml5',
                text: `<i class="fa fa-file-pdf"></i> Export to pdf`,
                className: 'f14 btn-danger py-1 my-1',
                pageSize: 'A3',
                orientation: 'landscape',
                fontSize: 6
            }],
            columns: [
                {
                    data: {
                        "filter": "Id",
                        "display": "id"
                    }, "orderable": true, "render": function (data, type, row, meta) {
                        return (meta.row + 1 + meta.settings._iDisplayStart) + '.';
                    }
                },
                {
                    data: {
                        "filter": "Id",
                        "display": "id"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3 f12" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item ${row.surveyRemark == '' ? 'detail' : 'survey-detail'}" href="javascript:void(0);" cid="${row.id}" sid="${row.surveyId}">View details</a>`
                            + '</div>'
                            + '</div>';
                    }
                },
                {
                    data: {
                        "filter": "BatchNumber",
                        "display": "batchNumber"
                    },
                    visible: true,
                    render: (data, type, row, meta) => {
                        return data ? '#' + data : '';
                    }
                },
                {
                    data: {
                        "filter": "AccountNumber",
                        "display": "accountNumber"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "ARN",
                        "display": "arn"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "CustomerName",
                        "display": "customerName"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "Email",
                        "display": "email"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "PhoneNumber",
                        "display": "phoneNumber"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "SurveyStatus",
                        "display": "surveyStatus"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "SurveyStaff",
                        "display": "surveyStaff"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "SurveyCompany",
                        "display": "surveyCompany"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "AssignedBy",
                        "display": "assignedBy"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "SurveyRemark",
                        "display": "surveyRemark"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "ScheduleDate",
                        "display": "scheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedScheduleDate",
                        "display": "formattedScheduleDate"
                    }, orderData: 13
                },
                {
                    data: {
                        "filter": "SurveyDate",
                        "display": "surveyDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedSurveyDate",
                        "display": "formattedSurveyDate"
                    }, orderData: 15
                },
                {
                    data: {
                        "filter": "DateShared",
                        "display": "dateShared"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedDateShared",
                        "display": "formattedDateShared"
                    }, orderData: 17
                },
                {
                    data: {
                        "filter": "CreatedBy",
                        "display": "createdBy"
                    }
                },
                {
                    data: {
                        "filter": "CreatedDate",
                        "display": "createdDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedCreatedDate",
                        "display": "formattedCreatedDate"
                    }, orderData: 20
                }

            ]
        })
            .on('preXhr.dt', (e, settings, data) => {
                data.filter = {
                    surveyStatus: $('#surveyStatus').val() == '' ? null : $('#surveyStatus').val(),
                    scheduleStatus: $('#scheduleStatus').val() == '' ? null : $('#scheduleStatus').val(),
                    surveyStaff: $('#surveyStaff').val() == '' ? null : $('#surveyStaff').val(),
                    sharedDateFrom: $('#sharedDateFrom').val() == '' ? null : $('#sharedDateFrom').val(),
                    sharedDateTo: $('#sharedDateTo').val() == '' ? null : $('#sharedDateTo').val(),
                    scheduleDateFrom: $('#scheduleDateFrom').val() == '' ? null : $('#scheduleDateFrom').val(),
                    scheduleDateTo: $('#scheduleDateTo').val() == '' ? null : $('#scheduleDateTo').val()
                };
                console.log(data.filter);
            });
            //.buttons().container()
            //.appendTo('#allTable_wrapper .col-md-6:eq(0)');

        $('#filterBtn').on('click', (e) => {
            allTable.ajax.reload();
        });

        $('#exportBtn').on('click', (e) => {
            let data = {
                surveyStatus: $('#surveyStatus').val() == '' ? null : $('#surveyStatus').val(),
                scheduleStatus: $('#scheduleStatus').val() == '' ? null : $('#scheduleStatus').val(),
                surveyStaff: $('#surveyStaff').val() == '' ? null : $('#surveyStaff').val(),
                sharedDateFrom: $('#sharedDateFrom').val() == '' ? null : $('#sharedDateFrom').val(),
                sharedDateTo: $('#sharedDateTo').val() == '' ? null : $('#sharedDateTo').val(),
                scheduleDateFrom: $('#scheduleDateFrom').val() == '' ? null : $('#scheduleDateFrom').val(),
                scheduleDateTo: $('#scheduleDateTo').val() == '' ? null : $('#scheduleDateTo').val()
            };
            var param = jQuery.param(data);
            window.open(`${$base}reports/ExportSurveyReport?${param}`);
        });
    }



    //============== View Customer Details ==============
    // on view detail
    $(document).on('click', '.detail', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('cid');
        try {
            loader = bootLoaderDialog('Fetching customer details...');
            let customer = (await getCustomer(uid));
            loader.hide();

            //console.log(customer);
            $('#cname_sp').html(customer.customerName);
            $('#batchno_p').html(customer.batchNumber);
            $('#accno_p').html(customer.accountNumber);
            $('#arn_p').html(customer.arn);
            $('#cusname_p').html(customer.customerName);
            $('#cisname_p').html(customer.cisName);
            $('#email_p').html(customer.email);
            $('#phone_p').html(customer.phoneNumber);
            $('#adres_p').html(customer.Address);
            $('#cisadres_p').html(customer.cisAddress);
            $('#landmark_p').html(customer.landmark);
            $('#bu_p').html(customer.bu);
            $('#ut_p').html(customer.ut);
            $('#feeder_p').html(customer.feeder);
            $('#dt_p').html(customer.dt);
            $('#tariff_p').html(customer.tariff);
            $('#metered_status_p').html(customer.meteredStatus);
            $('#survey_status_p').html(customer.surveyStatus);
            $('#installation_status_p').html(customer.installationStatus);
            $('#date_shared_p').html(customer.formattedDateShared);
            $('#created_by_p').html(customer.createdBy);
            $('#date_created_p').html(customer.formattedDateCreated);


            setTimeout(() => {
                $('#detailsModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);

        } catch (ex) {
            loader.hide();
            console.error(ex);
            notify(ex + '.', 'danger');
        }
    });

    $(document).on('click', '.survey-detail', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('sid');
        try {
            loader = bootLoaderDialog('Fetching survey details...');
            let survey = (await getSurvey(uid));
            let customer = survey.customer;
            loader.hide();

            //console.log(customer);
            $('#s_cname_sp').html(customer.customerName);

            $('#s_batchno_p').html(customer.batchNumber);
            $('#s_accno_p').html(customer.accountNumber);
            $('#s_arn_p').html(customer.arn);
            $('#s_cusname_p').html(customer.customerName);
            $('#s_cisname_p').html(customer.cisName);
            $('#s_email_p').html(customer.email);
            $('#s_phone_p').html(customer.phoneNumber);
            $('#s_adres_p').html(customer.Address);
            $('#s_cisadres_p').html(customer.cisAddress);
            $('#s_landmark_p').html(customer.landmark);
            $('#s_bu_p').html(customer.bu);
            $('#s_ut_p').html(customer.ut);
            $('#s_feeder_p').html(customer.feeder);
            $('#s_dt_p').html(customer.dt);
            $('#s_tariff_p').html(customer.tariff);
            $('#s_metered_status_p').html(customer.meteredStatus);
            $('#s_survey_status_p').html(customer.surveyStatus);
            $('#s_installation_status_p').html(customer.installationStatus);
            $('#s_date_shared_p').html(customer.formattedDateShared);
            $('#s_created_by_p').html(customer.createdBy);
            $('#s_date_created_p').html(customer.formattedDateCreated);

            $('#s_readyToPay_p').html(survey.readyToPay);
            $('#s_occupierPhone_p').html(survey.occupierPhoneNumber);
            $('#s_bedroomCount_p').html(survey.bedroomCount);
            $('#s_typeOfApartment_p').html(survey.typeOfApartment);
            $('#s_existingMeterType_p').html(survey.existingMeterType);
            $('#s_existingMeterNo_p').html(survey.existingMeterNumber);
            $('#s_customerBillMatchUploadData_p').html(survey.customerBillMatchUploadedData);
            $('#s_estimatedTotalLoad_p').html(survey.estimatedTotalLoadInAmps);
            $('#s_recommendedMeterType_p').html(survey.recommendedMeterType);
            $('#s_installationMode_p').html(survey.installationMode);
            $('#s_loadWireSeparationRequired_p').html(survey.loadWireSeparationRequired);
            $('#s_accountSeparationRequired_p').html(survey.accountSeparationRequired);
            $('#s_noOf1QRequired_p').html(survey.numberOf1QRequired);
            $('#s_noOf3QRequired_p').html(survey.numberOf3QRequired);
            $('#s_map_p').html(survey.map);
            $('#s_surveyRemark_p').html(survey.surveyRemark);
            $('#s_comment_p').html(survey.additionalComment);
            $('#s_survey_date_p').html(survey.formattedSurveyDate);


            setTimeout(() => {
                $('#surveyDetailsModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);

        } catch (ex) {
            loader.hide();
            console.error(ex);
            notify(ex + '.', 'danger');
        }
    });
});


function getSurvey(surveyId) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyId == undefined || surveyId == 0) {
                reject('Survey id is required');
            } else {
                let url = $base + 'surveys/' + surveyId;
                $.ajax({
                    type: 'GET',
                    url: url,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.data);
                        } else {
                            reject(response.message);
                        }
                    },
                    error: (req, status, err) => {
                        ajaxErrorHandler(req, status, err, {});
                        reject(null);
                    }
                });
            }

        } catch (ex) {
            console.error(ex);
            //notify(ex.message, 'danger');
            reject(ex.message);
        }
    });
    return promise;
}

function getCustomer(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid customer id');
            } else {
                let url = $base + 'customers/GetCustomer/' + id;
                $.ajax({
                    type: 'GET',
                    url: url,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.data);
                        } else {
                            reject(response.message);
                        }
                    },
                    error: (req, status, err) => {
                        ajaxErrorHandler(req, status, err, {});
                    }
                });
            }

        } catch (ex) {
            console.error(ex);
            //notify(ex.message, 'danger');
            reject(ex.message);
        }
    });
    return promise;
}