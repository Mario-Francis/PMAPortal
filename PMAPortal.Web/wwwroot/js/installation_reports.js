var allTable;

$(() => {
    if ($('#allTable')) {
        var exclude = [0, 1, 14, 17];
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
                url: $base + 'reports/AllInstallationsDataTable',
                type: "POST"
            },
            "order": [[14, "desc"]],
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
                            + `<a class="dropdown-item" href="${$base}installations/${row.installationId}/details">View installation details</a>`
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
                        "filter": "InstallationStatus",
                        "display": "installationStatus"
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
                        "filter": "InstallerCompany",
                        "display": "installerCompany"
                    }, visible: true
                },
                {
                    data: {
                        "filter": "iAssignedBy",
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
                        "filter": "ScheduleDate",
                        "display": "scheduleDate"
                    }, visible: false
                },
                {
                    data: {
                        "filter": "FormattedScheduleDate",
                        "display": "formattedScheduleDate"
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
        }).on('preXhr.dt', (e, settings, data) => {
            data.filter = {
                installationStatusId: $('#installationStatus').val() == '' ? null : $('#installationStatus').val(),
                scheduleStatus: $('#scheduleStatus').val() == '' ? null : $('#scheduleStatus').val(),
                installerId: $('#installerId').val() == '' ? null : $('#installerId').val(),
                meterType: $('#meterType').val() == '' ? null : $('#meterType').val(),
                scheduleDateFrom: $('#scheduleDateFrom').val() == '' ? null : $('#scheduleDateFrom').val(),
                scheduleDateTo: $('#scheduleDateTo').val() == '' ? null : $('#scheduleDateTo').val()
            };
        });
            //.buttons().container()
            //.appendTo('#allTable_wrapper .col-md-6:eq(0)');

        $('#filterBtn').on('click', (e) => {
            allTable.ajax.reload();
        });

        //$('#exportBtn').on('click', (e) => {
        //    let data = {
        //        installationStatusId: $('#installationStatus').val() == '' ? null : $('#installationStatus').val(),
        //        scheduleStatus: $('#scheduleStatus').val() == '' ? null : $('#scheduleStatus').val(),
        //        installerId: $('#installerId').val() == '' ? null : $('#installerId').val(),
        //        meterType: $('#meterType').val() == '' ? null : $('#meterType').val(),
        //        scheduleDateFrom: $('#scheduleDateFrom').val() == '' ? null : $('#scheduleDateFrom').val(),
        //        scheduleDateTo: $('#scheduleDateTo').val() == '' ? null : $('#scheduleDateTo').val()
        //    };
        //    var param = jQuery.param(data);
        //    window.open(`${$base}reports/ExportSurveyReport?${param}`);
        //});
    }


});
