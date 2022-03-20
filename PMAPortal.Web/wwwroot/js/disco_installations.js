var pendingTable, rejectedTable, approvedTable, userId;

$(() => {
    initialize();
    if ($('#pendingTable')) {
        pendingTable = $('#PendingTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'installations/CompletedDataTable',
                type: "POST"
            },
            "order": [[13, "desc"]],
            "lengthMenu": [10, 20, 30, 50, 100],
            "paging": true,
            autoWidth: false,
            //rowId: 'id',
            columns: [
                {
                    data: {
                        "filter": "Id",
                        "display": "id"
                    }, orderable: false, visible:false,
                    "render": function (data, type, row, meta) {
                        return '';
                        //return `
                        //<div class="form-group form-check">
                        //    <input type="checkbox" class="form-check-input rchbx" value="${row.installationId}" ${selectedReassignItems.includes(row.installationId) ? 'checked' : ''} id="rchbx_${row.installationId}">
                        //    <label class="form-check-label" for="rchbx_${row.installationId}"></label>
                        //</div>`;
                    }
                },
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
                        "filter": "InstallationId",
                        "display": "installationId"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View customer details</a>`
                            + `<a class="dropdown-item idetail" href="${$base}installations/${data}/DiscoReview">Reveiw installation</a>`
                            // + `<a class="dropdown-item reassign" href="javascript:void(0)" cid="${row.installationId}">Reassign installation staff</a>`
                            //+ `<div class="dropdown-divider"></div>`
                            //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
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
                        "filter": "Installer",
                        "display": "installer"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IAssignedBy",
                        "display": "iAssignedBy"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterType",
                        "display": "meterType"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterNumber",
                        "display": "meterNumber"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IScheduleDate",
                        "display": "iScheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedIScheduleDate",
                        "display": "formattedIScheduleDate"
                    }, orderData: 11
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
                    }, orderData: 13
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
                    }, orderData: 16
                }

            ]
        });
    }
    if ($('#rejectedTable')) {
        rejectedTable = $('#rejectedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'installations/DiscoRejectedDataTable',
                type: "POST"
            },
            "order": [[13, "desc"]],
            "lengthMenu": [10, 20, 30, 50, 100],
            "paging": true,
            autoWidth: false,
            //rowId: 'id',
            columns: [
                {
                    data: {
                        "filter": "Id",
                        "display": "id"
                    }, orderable: false, visible:false,
                    "render": function (data, type, row, meta) {
                        return '';
                        //return `
                        //<div class="form-group form-check">
                        //    <input type="checkbox" class="form-check-input rchbx" value="${row.installationId}" ${selectedReassignItems.includes(row.installationId) ? 'checked' : ''} id="rchbx_${row.installationId}">
                        //    <label class="form-check-label" for="rchbx_${row.installationId}"></label>
                        //</div>`;
                    }
                },
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
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View customer details</a>`
                            + `<a class="dropdown-item" href="${$base}installations/${row.installationId}/details">View installation details</a>`
                            // + `<a class="dropdown-item reassign" href="javascript:void(0)" cid="${row.installationId}">Reassign installation staff</a>`
                            //+ `<div class="dropdown-divider"></div>`
                            //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
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
                        "filter": "Installer",
                        "display": "installer"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IAssignedBy",
                        "display": "iAssignedBy"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterType",
                        "display": "meterType"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterNumber",
                        "display": "meterNumber"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "Comment",
                        "display": "comment"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IScheduleDate",
                        "display": "iScheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedIScheduleDate",
                        "display": "formattedIScheduleDate"
                    }, orderData: 11
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
                    }, orderData: 13
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
                    }, orderData: 16
                }

            ]
        });
    }
    if ($('#approvedTable')) {
        approvedTable = $('#approvedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'installations/DiscoApprovedDataTable',
                type: "POST"
            },
            "order": [[13, "desc"]],
            "lengthMenu": [10, 20, 30, 50, 100],
            "paging": true,
            autoWidth: false,
            //rowId: 'id',
            columns: [
                {
                    data: {
                        "filter": "Id",
                        "display": "id"
                    }, orderable: false, visible:false,
                    "render": function (data, type, row, meta) {
                        return '';
                        //return `
                        //<div class="form-group form-check">
                        //    <input type="checkbox" class="form-check-input rchbx" value="${row.installationId}" ${selectedReassignItems.includes(row.installationId) ? 'checked' : ''} id="rchbx_${row.installationId}">
                        //    <label class="form-check-label" for="rchbx_${row.installationId}"></label>
                        //</div>`;
                    }
                },
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
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View customer details</a>`
                            + `<a class="dropdown-item" href="${$base}installations/${row.installationId}/details">View installation details</a>`
                            // + `<a class="dropdown-item reassign" href="javascript:void(0)" cid="${row.installationId}">Reassign installation staff</a>`
                            //+ `<div class="dropdown-divider"></div>`
                            //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
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
                        "filter": "Installer",
                        "display": "installer"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IAssignedBy",
                        "display": "iAssignedBy"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterType",
                        "display": "meterType"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "MeterNumber",
                        "display": "meterNumber"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "IScheduleDate",
                        "display": "iScheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedIScheduleDate",
                        "display": "formattedIScheduleDate"
                    }, orderData: 11
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
                    }, orderData: 13
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
                    }, orderData: 16
                }

            ]
        });
    }
    
    //============== View Customer Details ==============
    // on view detail
    $(document).on('click', '.detail', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('cid');
        try {
            loader = bootLoaderDialog('Fetching customer details...');
            let customer = (await getInstallation(uid)).customer;
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
            $('#installation_status_p').html(customer.installationStatus);
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


function getInstallation(installationId) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + 'installations/' + installationId;
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
function refreshTables() {
    if (unassignedTable) {
        unassignedTable.ajax.reload();
    }
    if (assignedTable) {
        assignedTable.ajax.reload();
    }
    if (rejectedTable) {
        rejectedTable.ajax.reload();
    }
    if (completedTable) {
        completedTable.ajax.reload();
    }
    if (discoRejectedTable) {
        discoRejectedTable.ajax.reload();
    }
    if (discoApprovedTable) {
        discoApprovedTable.ajax.reload();
    }
}

function initialize() {
    userId = $('#userId').val();
   
    $('.tab-pane').removeClass('show');
    $($('.tab-pane')[0]).addClass('show active');

    $('.nav-tabs .nav-link').removeClass('active');
    $($('.nav-tabs .nav-link')[0]).addClass('active');
}