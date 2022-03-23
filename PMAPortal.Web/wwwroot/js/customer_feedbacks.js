$(() => {
    feedbacksTable = $('#feedbacksTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'feedbacks/FeedbacksDataTable',
            type: "POST"
        },
        "order": [[2, "asc"]],
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
                        + `<a class="dropdown-item" href="${$base}feedbacks/${data}" uid="${row.id}">View Details</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Customer",
                    "display": "customer"
                }
            },
            {
                data: {
                    "filter": "AccountNumber",
                    "display": "accountNumber"
                }
            },
            {
                data: {
                    "filter": "Rating",
                    "display": "rating"
                }
            },
            {
                data: {
                    "filter": "Comment",
                    "display": "comment"
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
                }, orderData: 3
            }

        ]
    });

});