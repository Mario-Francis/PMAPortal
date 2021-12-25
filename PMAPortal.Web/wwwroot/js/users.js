var usersTable;

$(() => {
    // initialize datatable
    usersTable = $('#usersTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: $base + 'users/UsersDataTable',
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
                        + (!status ? `<a class="dropdown-item activate" href="javascript:void(0)" uid="${row.id}">Activate</a>` : '')
                        + (status ? `<a class="dropdown-item deactivate" href="javascript:void(0)" uid="${row.id}">Deactivate</a>` : '')
                        + `<a class="dropdown-item reset" href="javascript:void(0)" uid="${row.id}">Reset Password</a>`
                        + `<div class="dropdown-divider"></div>`
                        + `<a class="dropdown-item edit" href="javascript:void(0)" uid="${row.id}">Edit</a>`
                        + `<a class="dropdown-item delete" href="javascript:void(0)" uid="${row.id}">Delete</a>`
                        + '</div>'
                        + '</div>';
                }
            },
            {
                data: {
                    "filter": "FullName",
                    "display": "fullName"
                }, visible: true
            },
            {
                data: {
                    "filter": "Email",
                    "display": "email"
                }, "render": function (data, type, row, meta) {
                    return `${data} <br />${getRoleBadge(row.roleId)}`;
                }
            },

            {
                data: {
                    "filter": "Code",
                    "display": "code"
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
                    "filter": "IsActive",
                    "display": "isActive"
                }, "render": function (data, type, row, meta) {
                    if (data) {
                        return `<spa class="badge badge-success badge-sm rounded-pill px-2 py-1">Active</span>`;
                    } else {
                        return `<spa class="badge badge-secondary badge-sm rounded-pill px-2 py-1">Inactive</span>`;
                    }
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
                }, orderData: 7
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
                }, orderData: 10
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
                let firstName = $.trim($('#fname').val());
                let lastName = $.trim($('#lname').val());
                let roleId = $('#role').val();
                let email = $.trim($('#email').val());
                let code = $.trim($('#code').val());
                let phoneNumber = $.trim($('#phone').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();

                if (firstName == '' || lastName == '' || roleId == '' || email == '' || phoneNumber=='') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<i class="fa fa-circle-notch fa-spin"></i> Adding user...');
                    let url = $base + 'users/AddUser';
                    let data = {
                        firstName,
                        lastName,
                        roleId,
                        email,
                        code,
                        phoneNumber,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                usersTable.ajax.reload();
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
        let loader = bootLoaderDialog('Fetching user...');
        let user = null;
        try {
            user = await getUser(uid);
            loader.hide();

            $('#e_fname').val(user.firstName);
            $('#e_lname').val(user.lastName);
            $('#e_code').val(user.code);
            $('#e_phone').val(user.phoneNumber);
            $('#e_email').val(user.email);
            $('#e_role').val(user.roleId);

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
                let firstName = $.trim($('#e_fname').val());
                let lastName = $.trim($('#e_lname').val());
                let roleId = $('#e_role').val();
                let email = $.trim($('#e_email').val());
                let code = $.trim($('#e_code').val());
                let phoneNumber = $.trim($('#e_phone').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[1]).val();

                if (firstName == '' || lastName == '' || roleId == '' || email == '' || phoneNumber == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    $('fieldset').prop('disabled', true);
                    btn.html('<i class="fa fa-circle-notch fa-spin"></i> Updating user...');
                    let url = $base + 'users/UpdateUser';
                    let data = {
                        id: uid,
                        firstName,
                        lastName,
                        roleId,
                        email,
                        code,
                        phoneNumber,
                        __RequestVerificationToken
                    };
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        success: (response) => {
                            if (response.isSuccess) {
                                usersTable.ajax.reload();
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
        bootConfirm('Are you sure you want to delete this user?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Deleting user...');
                        let message = await deleteUser(uid);
                        loader.hide();

                        notify(message + '.', 'success');
                        usersTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        notify(ex + '.', 'danger');
                    }
                }
            }
        });
    });

    // on activate
    $(document).on('click', '.activate', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to activate this user?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Activating user...');
                        let message = await updateUserStatus(uid, true);
                        loader.hide();

                        notify(message + '.', 'success');
                        usersTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        if (ex != null) {
                            notify(ex + '.', 'danger');
                        }
                    }
                }
            }
        });
    });

    // on deactivate
    $(document).on('click', '.deactivate', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to deactivate this user?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Deactivating user...');
                        let message = await updateUserStatus(uid, false);
                        loader.hide();

                        notify(message + '.', 'success');
                        usersTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        if (ex != null) {
                            notify(ex + '.', 'danger');
                        }
                    }
                }
            }
        });
    });

    // on password reset
    $(document).on('click', '.reset', async (e) => {
        let loader;
        let uid = $(e.currentTarget).attr('uid');
        bootConfirm('Are you sure you want to reset this user\'s password?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Resetting password...');
                        let message = await resetPassword(uid);
                        loader.hide();

                        notify(message + '.', 'success');
                        usersTable.ajax.reload();
                    } catch (ex) {
                        loader.hide();
                        console.error(ex);
                        if (ex != null) {
                            notify(ex + '.', 'danger');
                        }
                    }
                }
            }
        });
    });


});



function getUser(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid user id');
            } else {
                let url = $base + 'users/GetUser/' + id;
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

function deleteUser(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid user id');
            } else {
                let url = $base + 'users/DeleteUser/' + id;
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

function resetPassword(id) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid user id');
            } else {
                let url = $base + 'users/ResetPassword/' + id;
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


function updateUserStatus(id, isActive = true) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (id == undefined || id == '' || id == 0) {
                reject('Invalid user id');
            } else {
                let url = $base + 'users/UpdateUserStatus/' + id + '?isactive=' + isActive.toString();
                $.ajax({
                    type: 'POST',
                    url: url,
                    success: (response) => {
                        if (response.isSuccess) {
                            resolve(response.message);
                        } else {
                            reject(response.message);
                        }
                    },
                    error: (req, status, err) => {
                        ajaxErrorHandler(req, status, err, {
                            callback: () => {
                                reject(null);
                            }
                        });
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

function getRoleBadge(role) {
    if (role == 1) {
        return `<span class="badge bg-primary text-white badge-sm rounded-pill px-2 py-1">Administrator</span>`;
    } else if (role == 2) {
        return `<span class="badge bg-secondary badge-sm rounded-pill px-2 py-1">Disco Personnel</span>`;
    } else if (role == 3) {
        return `<span class="badge bg-info text-white badge-sm rounded-pill px-2 py-1">Installer</span>`;
    } else {
        return '';
    }
}
