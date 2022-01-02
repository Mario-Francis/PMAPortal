$(() => {
    
    $(".chosen-select").chosen({ disable_search_threshold: 10 });
    var reportsTable = $('#reportsTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'reports/ReportsDataTable',
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
    })
        .on('preXhr.dt', (e, settings, data) => {
            //data.sessionId = $('#sessionId').val();
            data.filter = {
                fromDate: $('#from').val() == '' ? null : $('#from').val(),
                toDate: $('#to').val() == '' ? null : $('#to').val(),
                meterId: $('#meter').val() == '' ? null : $('#meter').val(),
                area: $('#area').val() == '' ? null : $('#area').val(),
                statusId: $('#status').val() == '' ? null : $('#status').val(),
                installerId: $('#installer').val() == '' ? null : $('#installer').val()
            };
        }).on('xhr.dt', (e, settings, json, xhr) => {
            if (json != null) {
                console.log(json);
                $('#totalMeterCount').html(json.meterCount);
                $('#meterCountsTable').html(json.meterCountByTypes.map(m => `<tr><td>${m.name}</td><td class="text-right">${m.count}</td></tr>`).join(''));
                $('#totalAmount').html(json.formattedTotalAmount);
                $('#meterAmountsTable').html(json.totalAmountByTypes.map(m => `<tr><td>${m.name}</td><td class="text-right">${m.formattedAmount}</td></tr>`).join(''));
                $('#areaCount').html(json.areaCount);
                $('#areasTable').html(json.areas.map(a => `<tr><td>${a}</td></tr>`).join(''));
            }
        });

    $('#filterBtn').on('click', (e) => {
        reportsTable.ajax.reload();
    });

    $('#exportBtn').on('click', (e) => {
        let data = {
            fromDate: $('#from').val() == '' ? null : $('#from').val(),
            toDate: $('#to').val() == '' ? null : $('#to').val(),
            meterId: $('#meter').val() == '' ? null : $('#meter').val(),
            area: $('#area').val() == '' ? null : $('#area').val(),
            statusId: $('#status').val() == '' ? null : $('#status').val(),
            installerId: $('#installer').val() == '' ? null : $('#installer').val()
        };
        var param = jQuery.param(data);
        window.open(`${$base}reports/exportReport?${param}`);
    });
});