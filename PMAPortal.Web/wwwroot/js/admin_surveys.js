var unassignedTable, assignedTable, completedTable;
var selectedItems = [];
$(() => {
    if (selectedItems.length == 0) {
        $('#assignBtn').prop('disabled', true);
    }

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
                }, "orderable": true,
                orderable:false,
                "render": function (data, type, row, meta) {
                    return `
                        <div class="form-group form-check">
                            <input type="checkbox" class="form-check-input chbx" value="${data}" ${selectedItems.includes(data)?'checked':''} id="chbx_${data}">
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
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">View details</a>`
                        + `<a class="dropdown-item" href="#" aid="${row.id}">Assign survey staff</a>`
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
                }, "orderable": true, "render": function (data, type, row, meta) {
                    return (meta.row + 1 + meta.settings._iDisplayStart) + '.';
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
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">View details</a>`
                        + `<a class="dropdown-item" href="#" aid="${row.id}">Reassign survey staff</a>`
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
                }, "orderable": true, "render": function (data, type, row, meta) {
                    return (meta.row + 1 + meta.settings._iDisplayStart) + '.';
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
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">View details</a>`
                        + `<div class="dropdown-divider"></div>`
                        + `<a class="dropdown-item" href="#" aid="${row.id}">Edit Remark</a>`
                        + `<a class="dropdown-item" href="#" aid="${row.id}">Edit Survey</a>`

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

    $(document).on('change', '.chbx', e => {
        let chbx = $(e.currentTarget);
        if (chbx.prop('checked')) {
            selectedItems.push(parseInt(chbx.val()));
        } else {
            selectedItems = selectedItems.filter(i => i != parseInt(chbx.val()));
        }

        if (selectedItems.length > 0) {
            $('#assignBtn').prop('disabled', false);
        } else {
            $('#assignBtn').prop('disabled', true);
        }
    });
    $('#assignBtn').on('click', e => {
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
                    loader = bootLoaderDialog('Assigning survey(s) to staff...');
                    if (selectedItems.length == 0) {
                        notify('No pending surveys selected!', 'warning');
                    } else {
                        var message = await assignSurveys(selectedItems, surveyStaffId, __RequestVerificationToken);
                        loader.hide();
                        notify(message, 'success');

                        form.reset();
                        selectedItems = [];
                        $('.chbx').prop('checked', false);
                        $('#assignModal').modal('hide');
                        refreshTables();
                    }
                    
                }
            }
        } catch (ex) {
            if (ex != null) {
                console.error(ex);
                notify(ex.message, 'danger');
            }
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
    unassignedTable.ajax.reload();
    assignedTable.ajax.reload();
    completedTable.ajax.reload();
}