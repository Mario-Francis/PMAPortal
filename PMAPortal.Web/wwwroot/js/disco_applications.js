$(() => {
    var pendingTable = $('#pendingTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'applications/DiscoApplicationsDataTable?tableType=pending',
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
                }, "orderable": false, "render": function (data, type, row, meta) {
                    return '<div class="dropdown f14">'
                        + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                        + '<i class="fa fa-ellipsis-v"></i>'
                        + '</button>'
                        + '<div class="dropdown-menu f14">'
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">Viw Details</a>`
                        //+ `<div class="dropdown-divider"></div>`
                        //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Applicant",
                    "display": "applicant"
                }
            },
            {
                data: {
                    "filter": "Area",
                    "display": "area"
                }
            },
            {
                data: {
                    "filter": "PhoneNumber",
                    "display": "phoneNumber"
                }
            },
            {
                data: {
                    "filter": "Meter",
                    "display": "meter"
                }
            },
            {
                data: {
                    "filter": "TrackNumber",
                    "display": "trackNumber"
                }
            },
            {
                data: {
                    "filter": "AmountPaid",
                    "display": "amountPaid"
                }
            },
            {
                data: {
                    "filter": "Status",
                    "display": "status"
                }, "render": function (data, type, row, meta) {
                    return data;
                    //return getAssessmentStatus(data);
                }
            },
            {
                data: {
                    "filter": "Installer",
                    "display": "installer"
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
                }, orderData: 10
            },
            {
                data: {
                    "filter": "UpdatedBy",
                    "display": "updatedBy"
                }
            },
            {
                data: {
                    "filter": "UpdatedDate",
                    "display": "updatedDate"
                }, visible: false
            },
            {
                data: {
                    "filter": "FormattedUpdatedDate",
                    "display": "formattedUpdatedDate"
                }, orderData: 13
            },

        ]
    });

    var failedConfirmationTable = $('#failedConfirmationTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'applications/DiscoApplicationsDataTable?tableType=failed',
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
                }, "orderable": false, "render": function (data, type, row, meta) {
                    return '<div class="dropdown f14">'
                        + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                        + '<i class="fa fa-ellipsis-v"></i>'
                        + '</button>'
                        + '<div class="dropdown-menu f14">'
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">Viw Details</a>`
                        //+ `<div class="dropdown-divider"></div>`
                        //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Applicant",
                    "display": "applicant"
                }
            },
            {
                data: {
                    "filter": "Area",
                    "display": "area"
                }
            },
            {
                data: {
                    "filter": "PhoneNumber",
                    "display": "phoneNumber"
                }
            },
            {
                data: {
                    "filter": "Meter",
                    "display": "meter"
                }
            },
            {
                data: {
                    "filter": "TrackNumber",
                    "display": "trackNumber"
                }
            },
            {
                data: {
                    "filter": "AmountPaid",
                    "display": "amountPaid"
                }
            },
            {
                data: {
                    "filter": "Status",
                    "display": "status"
                }, "render": function (data, type, row, meta) {
                    return data;
                    //return getAssessmentStatus(data);
                }
            },
            {
                data: {
                    "filter": "Installer",
                    "display": "installer"
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
                }, orderData: 10
            },
            {
                data: {
                    "filter": "UpdatedBy",
                    "display": "updatedBy"
                }
            },
            {
                data: {
                    "filter": "UpdatedDate",
                    "display": "updatedDate"
                }, visible: false
            },
            {
                data: {
                    "filter": "FormattedUpdatedDate",
                    "display": "formattedUpdatedDate"
                }, orderData: 13
            },

        ]
    });

    var completedTable = $('#completedTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'applications/DiscoApplicationsDataTable?tableType=completed',
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
                }, "orderable": false, "render": function (data, type, row, meta) {
                    return '<div class="dropdown f14">'
                        + '<button type="button" class="btn px-3" data-toggle="dropdown">'
                        + '<i class="fa fa-ellipsis-v"></i>'
                        + '</button>'
                        + '<div class="dropdown-menu f14">'
                        + `<a class="dropdown-item" href="${$base}applications/${data}" aid="${row.id}">Viw Details</a>`
                        //+ `<div class="dropdown-divider"></div>`
                        //+ `<a class="dropdown-item edit" href="javascript:void(0)" aid="${row.id}">Edit</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Applicant",
                    "display": "applicant"
                }
            },
            {
                data: {
                    "filter": "Area",
                    "display": "area"
                }
            },
            {
                data: {
                    "filter": "PhoneNumber",
                    "display": "phoneNumber"
                }
            },
            {
                data: {
                    "filter": "Meter",
                    "display": "meter"
                }
            },
            {
                data: {
                    "filter": "TrackNumber",
                    "display": "trackNumber"
                }
            },
            {
                data: {
                    "filter": "AmountPaid",
                    "display": "amountPaid"
                }
            },
            {
                data: {
                    "filter": "Status",
                    "display": "status"
                }, "render": function (data, type, row, meta) {
                    return data;
                    //return getAssessmentStatus(data);
                }
            },
            {
                data: {
                    "filter": "Installer",
                    "display": "installer"
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
                }, orderData: 10
            },
            {
                data: {
                    "filter": "UpdatedBy",
                    "display": "updatedBy"
                }
            },
            {
                data: {
                    "filter": "UpdatedDate",
                    "display": "updatedDate"
                }, visible: false
            },
            {
                data: {
                    "filter": "FormattedUpdatedDate",
                    "display": "formattedUpdatedDate"
                }, orderData: 13
            },

        ]
    });

});