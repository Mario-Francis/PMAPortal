var areasTable;

$(() => {
    // initialize datatable
    batchesTable = $('#batchesTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'batches/BatchesDataTable',
            type: "POST"
        },
        "order": [[2, "desc"]],
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
                    let status = row.isActive;
                    return '<div class="dropdown f14">'
                        + '<button type="button" class="btn px-3 f12" data-toggle="dropdown">'
                        + '<i class="fa fa-ellipsis-v"></i>'
                        + '</button>'
                        + '<div class="dropdown-menu f14">'
                        + `<a class="dropdown-item edit" href="#">View Customers</a>`
                        + `<a class="dropdown-item delete" href="javascript:void(0)" uid="${row.id}">Delete</a>`
                        + `<a class="dropdown-item edit" href="${$base}${row.filePath}" download>Download batch file</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Id",
                    "display": "id"
                },
                visible: true,
                render: (data, type, row, meta) => {
                    return '#'+pad('000000', data, true);
                }
            },
            {
                data: {
                    "filter": "FileName",
                    "display": "fileName"
                }, visible: true
            },
            {
                data: {
                    "filter": "CustomerCount",
                    "display": "customerCount"
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
                }, orderData: 5
            },
            {
                data: {
                    "filter": "CreatedBy",
                    "display": "createdBy"
                }, orderData: 6
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
                }, orderData: 8
            }
        ]
    });

    $('#file').on('change', e => {
        let file = $(e.currentTarget)[0].files[0];
        let arr = file.name.split('.');
        if (arr.length == 0) {
            notify('Invalid file selected!', 'danger');
            $('#file').val(null);
            $('#ftext').val('');
        } else {
            let ext = arr[arr.length - 1];
            if (ext != 'xls' && ext != 'xlsx') {
                notify(`Invalid file selected! Only .xls and .xlsx files are supported`, 'danger');
                $('#file').val(null);
                $('#ftext').val('');
            } else if (file.size > (20 * 1024 * 1024)) {
                notify(`Max upload size exceeded! Max is 20MB`, 'danger');
                $('#file').val(null);
                $('#ftext').val('');
            } else {
                $('#ftext').val(file.name);
            }
        }
    });

    $('#addBtn').on('click', (e) => {
        $('#addModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });

    // on add
    $('#uploadBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let dateShared = $.trim($('#dateShared').val());
                let files = $('#file')[0].files;
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();

                if (files.length == 0) {
                    notify('No file selected', 'warning');
                } else if (dateShared=='') {
                    notify('Date shared is required', 'warning');
                }else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<i class="fa fa-circle-notch fa-spin"></i> Uploading batch...');
                    let url = $base + 'batches/UploadBatch';
                    let data = new FormData();
                    data.append('dateShared', dateShared);
                    data.append('file', files[0]);
                    data.append('__RequestVerificationToken', __RequestVerificationToken);

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        processData: false,
                        contentType: false,
                        success: (response) => {
                            if (response.isSuccess) {
                                batchesTable.ajax.reload();
                                notify(response.message + '.', 'success');

                                form.reset();
                                $('#addModal').modal('hide');

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-upload"></i> &nbsp;Upload');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-upload"></i> &nbsp;Upload');
                                    $('fieldset').prop('disabled', false);
                                }
                            });
                        }
                    });
                }
            }
        } catch (ex) {
            console.error(ex);
            notify(ex.message, 'danger');
            btn.html('<i class="fa fa-upload"></i> &nbsp;Upload');
            $('fieldset').prop('disabled', false);
        }
    });


    // on remove
    $(document).on('click', '.delete', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to delete this batch?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Deleting batch...');
                        let message = await deleteBatch(uid);
                        loader.hide();

                        notify(message + '.', 'success');
                        batchesTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        notify(ex + '.', 'danger');
                    }
                }
            }
        });
    });






});



function getBatch(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid batch id');
            } else {
                let url = $base + 'batches/GetBatch/' + id;
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

function deleteBatch(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid batch id');
            } else {
                let url = $base + 'batches/DeleteBatch/' + id;
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
