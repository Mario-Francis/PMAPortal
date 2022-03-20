var unassignedTable, assignedTable, rejectedTable, completedTable, discoRejectedTable, discoApprovedTable, userId;
var selectedAssignItems = [];
var selectedReassignItems = [];



$(() => {
    initialize();
    if ($('#unassignedTable')) {
        unassignedTable = $('#unassignedTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: $base + 'installations/UnassignedDataTable',
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
                        "filter": "InstallationId",
                        "display": "installationId"
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
                        "filter": "InstallationId",
                        "display": "installationId"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${data}">View details</a>`
                            + `<a class="dropdown-item assign" href="javascript:void(0);" cid="${data}">Assign installation staff</a>`
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
                url: $base + 'installations/AssignedDataTable',
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
                            <input type="checkbox" class="form-check-input rchbx" value="${row.installationId}" ${selectedReassignItems.includes(row.installationId) ? 'checked' : ''} id="rchbx_${row.installationId}">
                            <label class="form-check-label" for="rchbx_${row.installationId}"></label>
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
                        "filter": "InstallationId",
                        "display": "installationId"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View details</a>`
                            + `<a class="dropdown-item" href="${$base}installations/${data}/details">View installation details</a>`
                            + `<a class="dropdown-item reassign" href="javascript:void(0)" cid="${row.installationId}">Reassign installation staff</a>`
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
                url: $base + 'installations/RejectedDataTable',
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
                            + `<a class="dropdown-item" href="${$base}installations/${data}/details">View installation details</a>`
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
    if ($('#completedTable')) {
        completedTable = $('#completedTable').DataTable({
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
                    }, orderable: false, visible: false,
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
                            + `<a class="dropdown-item" href="${$base}installations/${data}/details">View installation details</a>`
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
    if ($('#discoRejectedTable')) {
        discoRejectedTable = $('#discoRejectedTable').DataTable({
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
                        "filter": "InstallationId",
                        "display": "installationId"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View customer details</a>`
                            + `<a class="dropdown-item" href="${$base}installations/${data}/details">View installation details</a>`
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
    if ($('#discoApprovedTable')) {
        discoApprovedTable = $('#discoApprovedTable').DataTable({
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
                        "filter": "InstallationId",
                        "display": "installationId"
                    }, "orderable": false, "render": function (data, type, row, meta) {
                        return '<div class="dropdown f14">'
                            + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                            + '<i class="fa fa-ellipsis-v"></i>'
                            + '</button>'
                            + '<div class="dropdown-menu f14">'
                            + `<a class="dropdown-item detail" href="javascript:void(0);" cid="${row.installationId}">View customer details</a>`
                            + `<a class="dropdown-item" href="${$base}installations/${data}/details">View installation details</a>`
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
                let installerId = $.trim($('#installerId').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();


                if (installerId == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    let isSingle = $('#assignType').val() == 'single';
                    if (!isSingle && selectedAssignItems.length == 0) {
                        notify('No pending installations selected!', 'warning');
                    } else {
                        var message;
                        if (isSingle) {
                            let installationId = btn.attr('cid');
                            loader = bootLoaderDialog('Assigning installation to staff...');
                            message = await assignInstallation(installationId, installerId, __RequestVerificationToken);
                        } else {
                            loader = bootLoaderDialog('Assigning installation(s) to staff...');
                            message = await assignInstallations(selectedAssignItems, installerId, __RequestVerificationToken);
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
                let installerId = $.trim($('#r_installerId').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[1]).val();


                if (installerId == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    let isSingle = $('#reassignType').val() == 'single';
                    if (!isSingle && selectedReassignItems.length == 0) {
                        notify('No installations selected!', 'warning');
                    } else {
                        var message;
                        if (isSingle) {
                            let installationId = btn.attr('cid');
                            loader = bootLoaderDialog('Reassigning installation to staff...');
                            message = await reassignInstallation(installationId, installerId, __RequestVerificationToken);
                        } else {
                            loader = bootLoaderDialog('Reassigning installation(s) to staff...');
                            message = await reassignInstallations(selectedReassignItems, installerId, __RequestVerificationToken);
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

    $(document).on('click', '.installation-detail', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('cid');
        try {
            loader = bootLoaderDialog('Fetching installation details...');
            let installation = (await getInstallation(uid));
            let customer = installation.customer;
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
            $('#s_installation_status_p').html(customer.installationStatus);
            $('#s_installation_status_p').html(customer.installationStatus);
            $('#s_date_shared_p').html(customer.formattedDateShared);
            $('#s_created_by_p').html(customer.createdBy);
            $('#s_date_created_p').html(customer.formattedDateCreated);

            $('#s_readyToPay_p').html(installation.readyToPay);
            $('#s_occupierPhone_p').html(installation.occupierPhoneNumber);
            $('#s_bedroomCount_p').html(installation.bedroomCount);
            $('#s_typeOfApartment_p').html(installation.typeOfApartment);
            $('#s_existingMeterType_p').html(installation.existingMeterType);
            $('#s_existingMeterNo_p').html(installation.existingMeterNumber);
            $('#s_customerBillMatchUploadData_p').html(installation.customerBillMatchUploadedData);
            $('#s_estimatedTotalLoad_p').html(installation.estimatedTotalLoadInAmps);
            $('#s_recommendedMeterType_p').html(installation.recommendedMeterType);
            $('#s_installationMode_p').html(installation.installationMode);
            $('#s_loadWireSeparationRequired_p').html(installation.loadWireSeparationRequired);
            $('#s_accountSeparationRequired_p').html(installation.accountSeparationRequired);
            $('#s_noOf1QRequired_p').html(installation.numberOf1QRequired);
            $('#s_noOf3QRequired_p').html(installation.numberOf3QRequired);
            $('#s_map_p').html(installation.map);
            $('#s_installationRemark_p').html(installation.installationRemark);
            $('#s_comment_p').html(installation.additionalComment);
            $('#s_installation_date_p').html(installation.formattedInstallationDate);


            setTimeout(() => {
                $('#installationDetailsModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);

        } catch (ex) {
            loader.hide();
            console.error(ex);
            notify(ex + '.', 'danger');
        }
    });

});


function assignInstallations(installationIds, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationIds == undefined || installationIds.length == 0) {
                reject('No pending installations selected');
            } else {
                let url = $base + 'installations/AssignInstallations';
                let data = {
                    installationIds: installationIds,
                    installerId: staffId,
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

function assignInstallation(installationId, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Invalid installation id');
            } else {
                let url = $base + 'installations/AssignInstallation';
                let data = {
                    installationId,
                    installerId: staffId,
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

function reassignInstallations(installationIds, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationIds == undefined || installationIds.length == 0) {
                reject('No installations selected');
            } else {
                let url = $base + 'installations/ReassignInstallations';
                let data = {
                    installationIds,
                    installerId: staffId,
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

function reassignInstallation(installationId, staffId, token = null) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Invalid installation id');
            } else {
                let url = $base + 'installations/ReassignInstallation';
                let data = {
                    installationId,
                    installerId: staffId,
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

function completeInstallation(installationId) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + 'installations/CompleteInstallation';
                $.ajax({
                    type: 'GET',
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
    if (selectedAssignItems.length == 0) {
        $('#assignBtn').prop('disabled', true);
    }
    if (selectedReassignItems.length == 0) {
        $('#reassignBtn').prop('disabled', true);
    }


    $('.tab-pane').removeClass('show');
    $($('.tab-pane')[0]).addClass('show active');

    $('.nav-tabs .nav-link').removeClass('active');
    $($('.nav-tabs .nav-link')[0]).addClass('active');
}