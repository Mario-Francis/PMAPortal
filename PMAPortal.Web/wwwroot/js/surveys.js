var unassignedTable, assignedTable, myPendingTable, completedTable, myCompletedTable, userId;
var selectedAssignItems = [];
var selectedReassignItems = [];
var selectedScheduleItems = [];


$(() => {
    initialize();
    if ($('#unassignedTable')) {
        unassignedTable = $('#unassignedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'surveys/UnassignedDataTable',
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
                    },
                    orderable: false,
                    "render": function (data, type, row, meta) {
                        return `
                        <div class="form-group form-check">
                            <input type="checkbox" class="form-check-input chbx" value="${data}" ${selectedAssignItems.includes(data) ? 'checked' : ''} id="chbx_${data}">
                            <label class="form-check-label" for="chbx_${data}"></label>
                        </div>`;
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
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.id}">View details</a>`
                            + `<a class="dropdown-item assign" href="javascript:void(0);" cid="${row.id}">Assign survey staff</a>`
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
                        "filter": "DateShared",
                        "display": "dateShared"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedDateShared",
                        "display": "formattedDateShared"
                    }, orderData: 10
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
                    }, orderData: 13
                }

            ]
        });
    }
    if ($('#assignedTable')) {
        assignedTable = $('#assignedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'surveys/AssignedDataTable',
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
                    }, orderable: false,
                    "render": function (data, type, row, meta) {
                        return `
                        <div class="form-group form-check">
                            <input type="checkbox" class="form-check-input rchbx" value="${row.surveyId}" ${selectedReassignItems.includes(row.surveyId) ? 'checked' : ''} id="rchbx_${row.surveyId}">
                            <label class="form-check-label" for="rchbx_${row.surveyId}"></label>
                        </div>`;
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
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.surveyId}">View details</a>`
                            + `<a class="dropdown-item reassign" href="javascript:void(0)" cid="${row.surveyId}">Reassign survey staff</a>`
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
                        "filter": "SurveyStaff",
                        "display": "surveyStaff"
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
                        "filter": "ScheduleDate",
                        "display": "scheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedScheduleDate",
                        "display": "formattedScheduleDate"
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
    if ($('#myPendingTable')) {
        myPendingTable = $('#myPendingTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'surveys/AssignedDataTable/' + userId,
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
                    }, orderable: false,
                    "render": function (data, type, row, meta) {
                        return `
                        <div class="form-group form-check">
                            <input type="checkbox" class="form-check-input schbx" value="${row.surveyId}" ${selectedScheduleItems.includes(row.surveyId) ? 'checked' : ''} id="schbx_${row.surveyId}">
                            <label class="form-check-label" for="schbx_${row.surveyId}"></label>
                        </div>`;
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
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.surveyId}">View details</a>`
                            + `<a class="dropdown-item schedule" href="javascript:void(0)" cid="${row.surveyId}">${!row.scheduleDate ? 'Schedule survey' : 'Reschedule survey'}</a>`
                            + (row.scheduleDate ? `<a class="dropdown-item start" href="javascript:void(0)" cid="${row.surveyId}">Start survey</a>` : '')
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
                        "filter": "AssignedBy",
                        "display": "assignedBy"
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
                    }, orderData: 10
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
                    }, orderData: 12
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
                    }, orderData: 15
                }

            ]
        });
    }
    if ($('#completedTable')) {
        completedTable = $('#completedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'surveys/CompletedDataTable',
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
                    },
                    "orderable": true,
                    "render": function (data, type, row, meta) {
                        return (meta.row + 1 + meta.settings._iDisplayStart) + '.';
                    }, visible: false
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
                            + `<a class="dropdown-item survey-detail" href="javascript:void(0);" cid="${row.surveyId}">View details</a>`
                            + `<div class="dropdown-divider"></div>`
                            + (isAdmin || isSupervisor ? `<a class="dropdown-item edit-remark" href="javascript:void(0)" cid="${row.surveyId}">Edit Remark</a>` : '')
                            + (isAdmin ? `<a class="dropdown-item edit-survey" href="javascript:void(0)" cid="${row.surveyId}">Edit Survey</a>` : '')

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
                        "filter": "SurveyStaff",
                        "display": "surveyStaff"
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
                    }, orderData: 11
                },
                {
                    data: {
                        "filter": "SurveyDate",
                        "display": "surveyDate"
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
                        "filter": "DateShared",
                        "display": "dateShared"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedDateShared",
                        "display": "formattedDateShared"
                    }, orderData: 15
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
                    }, orderData: 18
                }

            ]
        });
    }
    if ($('#myCompletedTable')) {
        myCompletedTable = $('#myCompletedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'surveys/CompletedDataTable/' + userId,
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
                    },
                    "orderable": true,
                    "render": function (data, type, row, meta) {
                        return (meta.row + 1 + meta.settings._iDisplayStart) + '.';
                    }, visible: false
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
                            + `<a class="dropdown-item survey-detail" href="javascript:void(0);" cid="${row.surveyId}">View details</a>`
                            + `<div class="dropdown-divider"></div>`
                            + (isAdmin || isSupervisor ? `<a class="dropdown-item edit-remark" href="javascript:void(0)" cid="${row.surveyId}">Edit Remark</a>` : '')
                            + (isAdmin ? `<a class="dropdown-item edit-survey" href="javascript:void(0)" cid="${row.surveyId}">Edit Survey</a>` : '')

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
                    }, orderData: 10
                },
                {
                    data: {
                        "filter": "SurveyDate",
                        "display": "surveyDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedScheduleDate",
                        "display": "formattedScheduleDate"
                    }, orderData: 12
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
                    }, orderData: 14
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
                    }, orderData: 17
                }

            ]
        });
    }
    //============Assignn====================
    $(document).on('change', '.chbx', e => {
        let chbx = $(e.currentTarget);
        if (chbx.prop('checked')) {
            selectedAssignItems.push(parseInt(chbx.val()));
        } else {
            selectedAssignItems = selectedAssignItems.filter(i => i != parseInt(chbx.val()));
        }

        if (selectedAssignItems.length > 0) {
            $('#assignBtn').prop('disabled', false);
        } else {
            $('#assignBtn').prop('disabled', true);
        }
    });
    $('#assignBtn').on('click', e => {
        $('#assignType').val('multiple');
        $('#assignModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });
    $('#assignSubmitBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let surveyStaffId = $.trim($('#surveyStaffId').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();


                if (surveyStaffId == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    let isSingle = $('#assignType').val() == 'single';
                    if (!isSingle && selectedAssignItems.length == 0) {
                        notify('No pending surveys selected!', 'warning');
                    } else {
                        var message;
                        if (isSingle) {
                            let customerId = btn.attr('cid');
                            loader = bootLoaderDialog('Assigning survey to staff...');
                            message = await assignSurvey(customerId, surveyStaffId, __RequestVerificationToken);
                        } else {
                            loader = bootLoaderDialog('Assigning survey(s) to staff...');
                            message = await assignSurveys(selectedAssignItems, surveyStaffId, __RequestVerificationToken);
                        }
                        loader.hide();
                        notify(message, 'success');

                        form.reset();
                        $('#assignBtn').prop('disabled', true);
                        selectedAssignItems = [];
                        $('.chbx').prop('checked', false);
                        $('#assignModal').modal('hide');
                        refreshTables();
                    }

                }
            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

    $(document).on('click', '.assign', (e) => {
        $('#assignType').val('single');
        $('#assignSubmitBtn').attr('cid', $(e.currentTarget).attr('cid'));
        $('#assignModal').modal({ backdrop: 'static', keyboard: false }, 'show');

    });

    //  ===============Reassign ====================
    $(document).on('change', '.rchbx', e => {
        let chbx = $(e.currentTarget);
        if (chbx.prop('checked')) {
            selectedReassignItems.push(parseInt(chbx.val()));
        } else {
            selectedReassignItems = selectedReassignItems.filter(i => i != parseInt(chbx.val()));
        }

        if (selectedReassignItems.length > 0) {
            $('#reassignBtn').prop('disabled', false);
        } else {
            $('#reassignBtn').prop('disabled', true);
        }
    });

    $('#reassignBtn').on('click', e => {
        $('#reassignType').val('multiple');
        $('#reassignModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });

    $('#reassignSubmitBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[1];
            if (validateForm(form)) {
                let surveyStaffId = $.trim($('#r_surveyStaffId').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[1]).val();


                if (surveyStaffId == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    let isSingle = $('#reassignType').val() == 'single';
                    if (!isSingle && selectedReassignItems.length == 0) {
                        notify('No surveys selected!', 'warning');
                    } else {
                        var message;
                        if (isSingle) {
                            let surveyId = btn.attr('cid');
                            loader = bootLoaderDialog('Reassigning survey to staff...');
                            message = await reassignSurvey(surveyId, surveyStaffId, __RequestVerificationToken);
                        } else {
                            loader = bootLoaderDialog('Reassigning survey(s) to staff...');
                            message = await reassignSurveys(selectedReassignItems, surveyStaffId, __RequestVerificationToken);
                        }
                        loader.hide();
                        notify(message, 'success');

                        form.reset();
                        $('#reassignBtn').prop('disabled', true);
                        selectedReassignItems = [];
                        $('.rchbx').prop('checked', false);
                        $('#reassignModal').modal('hide');
                        refreshTables();
                    }

                }
            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

    $(document).on('click', '.reassign', (e) => {
        $('#reassignType').val('single');
        $('#reassignSubmitBtn').attr('cid', $(e.currentTarget).attr('cid'));
        $('#reassignModal').modal({ backdrop: 'static', keyboard: false }, 'show');

    });

    //===== scchedule ===========
    $(document).on('change', '.schbx', e => {
        let chbx = $(e.currentTarget);
        if (chbx.prop('checked')) {
            selectedScheduleItems.push(parseInt(chbx.val()));
        } else {
            selectedScheduleItems = selectedScheduleItems.filter(i => i != parseInt(chbx.val()));
        }

        if (selectedScheduleItems.length > 0) {
            $('#scheduleBtn').prop('disabled', false);
        } else {
            $('#scheduleBtn').prop('disabled', true);
        }
    });

    $('#scheduleBtn').on('click', e => {
        $('#scheduleType').val('multiple');
        $('#scheduleModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });

    $('#scheduleSubmitBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[2];
            if (validateForm(form)) {
                let scheduleDate = $.trim($('#scheduleDate').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[2]).val();


                if (scheduleDate == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    let isSingle = $('#scheduleType').val() == 'single';
                    if (!isSingle && selectedScheduleItems.length == 0) {
                        notify('No surveys selected!', 'warning');
                    } else {
                        var message;
                        if (isSingle) {
                            let surveyId = btn.attr('cid');
                            loader = bootLoaderDialog('Scheduling survey...');
                            message = await scheduleSurvey(surveyId, scheduleDate, __RequestVerificationToken);
                        } else {
                            loader = bootLoaderDialog('Scheduling survey(s)...');
                            message = await scheduleSurveys(selectedScheduleItems, scheduleDate, __RequestVerificationToken);
                        }
                        loader.hide();
                        notify(message, 'success');

                        form.reset();
                        $('#scheduleBtn').prop('disabled', true);
                        selectedScheduleItems = [];
                        $('.schbx').prop('checked', false);
                        $('#scheduleModal').modal('hide');
                        refreshTables();
                    }

                }
            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

    $(document).on('click', '.schedule', (e) => {
        $('#scheduleType').val('single');
        $('#scheduleSubmitBtn').attr('cid', $(e.currentTarget).attr('cid'));
        $('#scheduleModal').modal({ backdrop: 'static', keyboard: false }, 'show');

    });

    //================ Survey =====================
    $(document).on('click', '.start', (e) => {
        let surveyId = $(e.currentTarget).attr('cid');
        let customer = Array.from(myPendingTable.rows().data()).find(d => d.surveyId == surveyId);

        $('#s_cname').html(customer.customerName);
        $('#s_accno').html(customer.accountNumber);

        $('#surveySubmitBtn').attr('cid', surveyId);

        $('#surveyModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });

    $('#s_existingMeterType').on('change', e => {
        let val = $(e.currentTarget).val();
        if (val != 'NA' && val != '') {
            $('#s_existingMeterNoDiv').show();
            $('#s_existingMeterNo').prop('required', true);
        } else {
            $('#s_existingMeterNoDiv').hide();
            $('#s_existingMeterNo').prop('required', false);
            $('#s_existingMeterNo').val('');
        }
    });

    $('#s_accountSeparationRequired').on('change', e => {
        let val = $(e.currentTarget).val();
        if (val != 'YES') {
            $('#s_noOf1QRequiredDiv').hide();
            $('#s_noOf3QRequiredDiv').hide();
            $('#s_noOf1QRequired').prop('required', false);
            $('#s_noOf3QRequired').prop('required', false);
            $('#s_noOf1QRequired').val('');
            $('#s_noOf3QRequired').val('');
        } else {
            $('#s_noOf1QRequiredDiv').show();
            $('#s_noOf3QRequiredDiv').show();
            $('#s_noOf1QRequired').prop('required', true);
            $('#s_noOf3QRequired').prop('required', true);
        }
    });

    $('#surveySubmitBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[3];
            let surveyId = btn.attr('cid');
            if (validateForm(form)) {
                let data = {
                    id: surveyId,
                    readyToPay: $.trim($('#s_readyToPay').val()),
                    occupierPhoneNumber: $.trim($('#s_occupierPhone').val()),
                    bedroomCount: $.trim($('#s_bedroomCount').val()),
                    typeOfApartment: $.trim($('#s_typeOfApartment').val()),
                    existingMeterType: $.trim($('#s_existingMeterType').val()),
                    existingMeterNumber: $.trim($('#s_existingMeterNo').val()) == '' ? null : $.trim($('#s_existingMeterNo').val()),
                    customerBillMatchUploadedData: $.trim($('#s_customerBillMatchUploadData').val()),
                    estimatedTotalLoadInAmps: $.trim($('#s_estimatedTotalLoad').val()),

                    recommendedMeterType: $.trim($('#s_recommendedMeterType').val()),
                    installationMode: $.trim($('#s_installationMode').val()),
                    loadWireSeparationRequired: $.trim($('#s_loadWireSeparationRequired').val()),
                    accountSeparationRequired: $.trim($('#s_accountSeparationRequired').val()),
                    numberOf1QRequired: $.trim($('#s_noOf1QRequired').val()) == '' ? null : $.trim($('#s_noOf1QRequired').val()),
                    numberOf3QRequired: $.trim($('#s_noOf3QRequired').val()) == '' ? null : $.trim($('#s_noOf3QRequired').val()),

                    map: $.trim($('#s_map').val()),
                    surveyRemark: $.trim($('#s_surveyRemark').val()),
                    additionalComment: $.trim($('#s_comment').val()),
                    __RequestVerificationToken: $($('input[name=__RequestVerificationToken]')[3]).val()
                };

               
                bootConfirm('Are you sure you want to submit this survey?', {
                    title: 'Confirm Action', size: 'small', callback: async (res) => {
                        if (res) {
                            try {
                                loader = bootLoaderDialog('Submitting survey...');
                                message = await submitSurvey(data);

                                loader.hide();
                                notify(message, 'success');

                                form.reset();
                                $('#surveyModal').modal('hide');
                                $('#s_cname').html('');
                                $('#s_accno').html('');
                                refreshTables();
                            } catch (ex) {
                                loader.hide();
                                if (ex != null) {
                                    console.error(ex);
                                    notify(ex.message, 'danger');
                                }
                            }
                        }
                    }
                });
                
            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

    //============= edit survey=================
    $('#es_existingMeterType').on('change', e => {
        let val = $(e.currentTarget).val();
        if (val != 'NA' && val != '') {
            $('#es_existingMeterNoDiv').show();
            $('#es_existingMeterNo').prop('required', true);
        } else {
            $('#es_existingMeterNoDiv').hide();
            $('#es_existingMeterNo').prop('required', false);
            $('#es_existingMeterNo').val('');
        }
    });

    $('#es_accountSeparationRequired').on('change', e => {
        let val = $(e.currentTarget).val();
        if (val != 'YES') {
            $('#es_noOf1QRequiredDiv').hide();
            $('#es_noOf3QRequiredDiv').hide();
            $('#es_noOf1QRequired').prop('required', false);
            $('#es_noOf3QRequired').prop('required', false);
            $('#es_noOf1QRequired').val('');
            $('#es_noOf3QRequired').val('');
        } else {
            $('#es_noOf1QRequiredDiv').show();
            $('#es_noOf3QRequiredDiv').show();
            $('#es_noOf1QRequired').prop('required', true);
            $('#es_noOf3QRequired').prop('required', true);
        }
    });
    $(document).on('click', '.edit-survey', async (e) => {
        let id = $(e.currentTarget).attr('cid');
        let loader = bootLoaderDialog('Fetching survey...');
        try {
            let survey = await getSurvey(id);
            loader.hide();

            $('#es_cname').html(survey.customer.customerName);
            $('#es_accno').html(survey.customer.accountNumber);

            $('#es_readyToPay').val(survey.readyToPay);
            $('#es_occupierPhone').val(survey.occupierPhoneNumber);
            $('#es_bedroomCount').val(survey.bedroomCount);
            $('#es_typeOfApartment').val(survey.typeOfApartment);
            $('#es_existingMeterType').val(survey.existingMeterType);
            $('#es_existingMeterNo').val(survey.existingMeterNumber);
            $('#es_customerBillMatchUploadData').val(survey.customerBillMatchUploadedData);
            $('#es_estimatedTotalLoad').val(survey.estimatedTotalLoadInAmps);
            $('#es_recommendedMeterType').val(survey.recommendedMeterType);
            $('#es_installationMode').val(survey.installationMode);
            $('#es_loadWireSeparationRequired').val(survey.loadWireSeparationRequired);
            $('#es_accountSeparationRequired').val(survey.accountSeparationRequired);
            $('#es_noOf1QRequired').val(survey.numberOf1QRequired);
            $('#es_noOf3QRequired').val(survey.numberOf3QRequired);
            $('#es_map').val(survey.map);
            $('#es_surveyRemark').val(survey.surveyRemark);
            $('#es_comment').val(survey.additionalComment);

            $('#es_existingMeterType').trigger('change');
            $('#es_accountSeparationRequired').trigger('change');

            
            $('#surveyUpdateBtn').attr('cid', id);

            setTimeout(() => {
                $('#editSurveyModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex, 'danger');
            }
            
        }
    });
    $('#surveyUpdateBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[4];
            let surveyId = btn.attr('cid');
            if (validateForm(form)) {
                let data = {
                    id: surveyId,
                    readyToPay: $.trim($('#es_readyToPay').val()),
                    occupierPhoneNumber: $.trim($('#es_occupierPhone').val()),
                    bedroomCount: $.trim($('#es_bedroomCount').val()),
                    typeOfApartment: $.trim($('#es_typeOfApartment').val()),
                    existingMeterType: $.trim($('#es_existingMeterType').val()),
                    existingMeterNumber: $.trim($('#es_existingMeterNo').val()) == '' ? null : $.trim($('#es_existingMeterNo').val()),
                    customerBillMatchUploadedData: $.trim($('#es_customerBillMatchUploadData').val()),
                    estimatedTotalLoadInAmps: $.trim($('#es_estimatedTotalLoad').val()),

                    recommendedMeterType: $.trim($('#es_recommendedMeterType').val()),
                    installationMode: $.trim($('#es_installationMode').val()),
                    loadWireSeparationRequired: $.trim($('#es_loadWireSeparationRequired').val()),
                    accountSeparationRequired: $.trim($('#es_accountSeparationRequired').val()),
                    numberOf1QRequired: $.trim($('#es_noOf1QRequired').val()) == '' ? null : $.trim($('#es_noOf1QRequired').val()),
                    numberOf3QRequired: $.trim($('#es_noOf3QRequired').val()) == '' ? null : $.trim($('#es_noOf3QRequired').val()),

                    map: $.trim($('#es_map').val()),
                    surveyRemark: $.trim($('#es_surveyRemark').val()),
                    additionalComment: $.trim($('#es_comment').val()),
                    __RequestVerificationToken: $($('input[name=__RequestVerificationToken]')[4]).val()
                };


                bootConfirm('Are you sure you want to update this survey?', {
                    title: 'Confirm Action', size: 'small', callback: async (res) => {
                        if (res) {
                            try {
                                loader = bootLoaderDialog('Updating survey...');
                                message = await updateSurvey(data);

                                loader.hide();
                                notify(message, 'success');

                                form.reset();
                                $('#editSurveyModal').modal('hide');
                                $('#es_cname').html('');
                                $('#es_accno').html('');
                                refreshTables();
                            } catch (ex) {
                                loader.hide();
                                if (ex != null) {
                                    console.error(ex);
                                    notify(ex.message, 'danger');
                                }
                            }
                        }
                    }
                });

            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

    // ==============Edit Survey Remark ===============
    $(document).on('click', '.edit-remark', async (e) => {
        let id = $(e.currentTarget).attr('cid');
        let loader = bootLoaderDialog('Fetching survey remark...');
        try {
            let survey = await getSurvey(id);
            loader.hide();

            $('#ers_cname').html(survey.customer.customerName);
            $('#ers_accno').html(survey.customer.accountNumber);
            
            $('#ers_surveyRemark').val(survey.surveyRemark);

            $('#surveyRemarkUpdateBtn').attr('cid', id);

            setTimeout(() => {
                $('#editSurveyRemarkModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex, 'danger');
            }

        }
    });
    $('#surveyRemarkUpdateBtn').on('click', async (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let loader;
        try {
            let form = $("form")[5];
            let surveyId = btn.attr('cid');
            if (validateForm(form)) {
                let data = {
                    surveyId,
                    remark: $.trim($('#ers_surveyRemark').val()),
                    __RequestVerificationToken: $($('input[name=__RequestVerificationToken]')[5]).val()
                };


                bootConfirm('Are you sure you want to update this survey remark?', {
                    title: 'Confirm Action', size: 'small', callback: async (res) => {
                        if (res) {
                            try {
                                loader = bootLoaderDialog('Updating survey remark...');
                                message = await updateRemark(data.surveyId, data.remark, data.__RequestVerificationToken);

                                loader.hide();
                                notify(message, 'success');

                                form.reset();
                                $('#editSurveyRemarkModal').modal('hide');
                                $('#ers_cname').html('');
                                $('#ers_accno').html('');
                                refreshTables();
                            } catch (ex) {
                                loader.hide();
                                if (ex != null) {
                                    console.error(ex);
                                    notify(ex.message, 'danger');
                                }
                            }
                        }
                    }
                });

            }
        } catch (ex) {
            loader.hide();
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
        }
    });

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
        let uid = $(e.currentTarget).attr('cid');
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


function assignSurveys(customerIds, staffId, token=null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (customerIds == undefined || customerIds.length == 0) {
                reject('No pending surveys selected');
            } else {
                let url = $base + 'surveys/AssignSurveys';
                let data = {
                    customerIds: customerIds,
                    surveyStaffId: staffId,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function assignSurvey(customerId, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (customerId == undefined || customerId == 0) {
                reject('Invalid survey id');
            } else {
                let url = $base + 'surveys/AssignSurvey';
                let data = {
                    customerId,
                    surveyStaffId: staffId,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function reassignSurveys(surveyIds, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyIds == undefined || surveyIds.length == 0) {
                reject('No surveys selected');
            } else {
                let url = $base + 'surveys/ReassignSurveys';
                let data = {
                    surveyIds,
                    surveyStaffId: staffId,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function reassignSurvey(surveyId, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyId == undefined || surveyId == 0) {
                reject('Invalid survey id');
            } else {
                let url = $base + 'surveys/ReassignSurvey';
                let data = {
                    surveyId,
                    surveyStaffId: staffId,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function scheduleSurveys(surveyIds, scheduleDate, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyIds == undefined || surveyIds.length == 0) {
                reject('No surveys selected');
            } else {
                let url = $base + 'surveys/ScheduleSurveys';
                let data = {
                    surveyIds,
                    scheduleDate,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function scheduleSurvey(surveyId, scheduleDate, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyId == undefined || surveyId == 0) {
                reject('Invalid survey id');
            } else {
                let url = $base + 'surveys/ScheduleSurvey';
                let data = {
                    surveyId,
                    scheduleDate,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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

function submitSurvey(data) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (data == undefined ) {
                reject('Survey data is required');
            } else {
                let url = $base + 'surveys/CompleteSurvey';
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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
function getSurvey(surveyId) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyId == undefined || surveyId==0) {
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
function updateSurvey(data) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (data == undefined) {
                reject('Survey data is required');
            } else {
                let url = $base + 'surveys/UpdateSurvey';
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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
function updateRemark(surveyId, remark, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (surveyId == undefined || surveyId == 0) {
                reject('Invalid survey id');
            } else {
                let url = $base + 'surveys/UpdateRemark';
                let data = {
                    surveyId,
                    remark,
                    __RequestVerificationToken: token
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
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
function refreshTables() {
    if (unassignedTable) {
        unassignedTable.ajax.reload();
    }
    if (assignedTable) {
        assignedTable.ajax.reload();
    }
    if (completedTable) {
        completedTable.ajax.reload();
    }
    if (myPendingTable) {
        myPendingTable.ajax.reload();
    }
    if (myCompletedTable) {
        myCompletedTable.ajax.reload();
    }
}

function initialize() {
    userId = $('#userId').val();
    if (selectedAssignItems.length == 0) {
        $('#assignBtn').prop('disabled', true);
    }
    if (selectedReassignItems.length == 0) {
        $('#reassignBtn').prop('disabled', true);
    }

    if (selectedScheduleItems.length == 0) {
        $('#scheduleBtn').prop('disabled', true);
    }

    $('.tab-pane').removeClass('show');
    $($('.tab-pane')[0]).addClass('show active');

    $('.nav-tabs .nav-link').removeClass('active');
    $($('.nav-tabs .nav-link')[0]).addClass('active');
}