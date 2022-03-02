var customersTable;

$(() => {
    // initialize datatable
    customersTable = $('#customersTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'customers/CustomersDataTable',
            type: "POST"
        },
        "order": [[5, "asc"]],
        "lengthMenu": [10, 20, 30, 50, 100],
        "paging": true,
        autoWidth: false,
        //rowId: 'id',
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
                        + `<a class="dropdown-item detail" href="javascript:void(0)" uid="${row.id}">Details</a>`
                        //+ `<a class="dropdown-item delete" href="javascript:void(0)" uid="${row.id}">Delete</a>`
                        + `<a class="dropdown-item" href="#" download>Survey</a>`
                        + `<a class="dropdown-item" href="#" download>Installation Info</a>`
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
                    return data?'#' +data:'';
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
                }, visible: true
            },
            {
                data: {
                    "filter": "CustomerName",
                    "display": "customerName"
                }, visible: true
            },
            {
                data: {
                    "filter": "CISName",
                    "display": "cisName"
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
                    "filter": "InstallationStatus",
                    "display": "installationStatus"
                }, visible: true
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
                }, orderData: 11
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
                }, orderData: 14
            }
        ]
    });

    //$('#addBtn').on('click', (e) => {
    //    $('#addModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    //});


    // on remove
    $(document).on('click', '.delete', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to delete this customer?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Deleting customer...');
                        let message = await deleteCustomer(uid);
                        loader.hide();

                        notify(message + '.', 'success');
                        customersTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        notify(ex + '.', 'danger');
                    }
                }
            }
        });
    });

    // on view detail
    $(document).on('click', '.detail', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        try {
            loader = bootLoaderDialog('Fetching customer...');
            let customer = await getCustomer(uid);
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

});



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

function deleteCustomer(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid customer id');
            } else {
                let url = $base + 'customers/DeleteCustomer/' + id;
                $.ajax({
                    type: 'GET',
                    url: url,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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
