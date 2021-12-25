var metersTable;

$(() => {
    // initialize datatable
    metersTable = $('#metersTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'meters/MetersDataTable',
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
                    let status = row.isActive;
                    return '<div class="dropdown f14">'
                        + '<button type="button" class="btn px-3 f12" data-toggle="dropdown">'
                        + '<i class="fa fa-ellipsis-v"></i>'
                        + '</button>'
                        + '<div class="dropdown-menu f14">'
                        + `<a class="dropdown-item edit" href="javascript:void(0)" uid="${row.id}">Edit</a>`
                        + `<a class="dropdown-item delete" href="javascript:void(0)" uid="${row.id}">Delete</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "Name",
                    "display": "name"
                }, visible: true
            },
            {
                data: {
                    "filter": "FormattedAmount",
                    "display": "formattedAmount"
                }, "render": function (data, type, row, meta) {
                    return `${data}`;
                }
            },

            {
                data: {
                    "filter": "Description",
                    "display": "description"
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
                }, orderData: 5
            },
            {
                data: {
                    "filter": "UpdatedBy",
                    "display": "updatedBy"
                }, visible: false
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
                }, orderData: 8
            },

        ]
    });


    $('#addBtn').on('click', (e) => {
        $('#addModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    });

    // on add
    $('#createBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let name = $.trim($('#name').val());
                let amount = $.trim($('#amount').val());
                let description = $('#description').val();
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();

                if (name == '' || amount == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<i class="fa fa-circle-notch fa-spin"></i> Adding meter...');
                    let url = $base + 'meters/AddMeter';
                    let data = {
                        name,
                        amount: unformatMoney(amount),
                        description,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                metersTable.ajax.reload();
                                notify(response.message + '.', 'success');

                                form.reset();
                                $('#addModal').modal('hide');

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
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
            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Submit');
            $('fieldset').prop('disabled', false);
        }
    });

    // on edit
    $(document).on('click', '.edit', async (e) => {
        let uid = $(e.currentTarget).attr('uid');
        let loader = bootLoaderDialog('Fetching meter...');
        let meter = null;
        try {
            meter = await getMeter(uid);
            loader.hide();

            $('#e_name').val(meter.name);
            $('#e_amount').val(formatMoney(meter.amount.toString(), { dp: 2, symbol:'\u20A6' }));
            $('#e_description').val(meter.description);

            $('#updateBtn').attr('uid', uid);

            setTimeout(() => {
                $('#editModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            }, 700);
        } catch (ex) {
            console.error(ex);
            notify(ex.message, 'danger');
            loader.hide();
        }
    });


    // on update
    $('#updateBtn').on('click', (e) => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let uid = btn.attr('uid');
        try {
            let form = $("form")[1];
            if (validateForm(form)) {
                let name = $.trim($('#e_name').val());
                let amount = $.trim($('#e_amount').val());
                let description = $('#e_description').val();
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[1]).val();

                if (name == '' || amount == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<i class="fa fa-circle-notch fa-spin"></i> Updating meter...');
                    let url = $base + 'meters/UpdateMeter';
                    let data = {
                        id: uid,
                        name,
                        amount: unformatMoney(amount),
                        description,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                metersTable.ajax.reload();
                                notify(response.message + '.', 'success');

                                form.reset();
                                $('#editModal').modal('hide');

                            } else {
                                notify(response.message, 'danger');
                            }
                            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
                            $('fieldset').prop('disabled', false);
                        },
                        error: (req, status, err) => {
                            ajaxErrorHandler(req, status, err, {
                                callback: () => {
                                    btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
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
            btn.html('<i class="fa fa-check-circle"></i> &nbsp;Update');
            $('fieldset').prop('disabled', false);
        }
    });



    // on remove
    $(document).on('click', '.delete', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to delete this meter?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Deleting meter...');
                        let message = await deleteMeter(uid);
                        loader.hide();

                        notify(message + '.', 'success');
                        metersTable.ajax.reload();
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



function getMeter(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid meter id');
            } else {
                let url = $base + 'meters/GetMeter/' + id;
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

function deleteMeter(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid meter id');
            } else {
                let url = $base + 'meters/DeleteMeter/' + id;
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
