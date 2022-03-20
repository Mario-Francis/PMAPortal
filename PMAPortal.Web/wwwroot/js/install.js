$(() => {
    loadImages();
    $('#startBtn').on('click', async e => {
        e.preventDefault();
        let btn = $(e.currentTarget);
        let installationId = $('#installationId').val();
        let loader;
        try {
            let form = $("form")[0];
            if (validateForm(form)) {
                let meterType = $.trim($('#meterType').val());
                let meterNumber = $.trim($('#meterNo').val());
                let __RequestVerificationToken = $($('input[name=__RequestVerificationToken]')[0]).val();


                if (meterType == '' || meterNumber == '') {
                    notify('Fields with asteriks (*) are required', 'warning');
                } else {
                    loader = bootLoaderDialog('Mappig meter to customer...');
                    let message = await startInstallation(installationId, meterType, meterNumber, __RequestVerificationToken);
                    loader.hide();
                    notify(message, 'success');

                    form.reset();
                    setTimeout(() => {
                        location.reload();
                    }, 2000);

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

    $(document).on('change', '.file', e => {
        let finput = $(e.currentTarget);
        let file = finput[0].files[0];
        let ftext = $('#' + finput.attr('ftext'));
        let ubtn = $('#' + finput.attr('btn'));

        let arr = file.name.split('.');
        if (arr.length == 0) {
            notify('Invalid file selected!', 'danger');
            finput.val(null);
            ftext.val('');
            btn.prop('disabled', true);
        } else {
            let ext = arr[arr.length - 1];
            if (ext != 'jpg' && ext != 'jpeg' && ext != 'png') {
                notify(`Invalid file selected! Only .jpg, .jpeg and .png files are supported`, 'danger');
                finput.val(null);
                ftext.val('');
                ubtn.prop('disabled', true);
            } else if (file.size > (10 * 1024 * 1024)) {
                notify(`Max upload size exceeded! Max is 10MB`, 'danger');
                finput.val(null);
                ftext.val('');
                ubtn.prop('disabled', true);
            } else {
                ftext.val(file.name);
                ubtn.prop('disabled', false);
            }
        }
    });

    $(document).on('click', '.i-image', e => {
        let img = $(e.currentTarget);
        var url = img.attr('src');
        var caption = img.attr('alt');

        $('#modalImg').attr('src', url);
        $('#modalImg').attr('alt', caption);
        $('#modalCaption').html(caption);

        $('#imageModal').modal('show');
    });

    $(document).on('click', '.upload', async e => {
        let btn = $(e.currentTarget);
        let file = $('#' + btn.attr('file'))[0];
        let fieldName = btn.attr('fieldName');
        let installationId = $('#installationId').val();
        let loader;
        try {

            let data = new FormData();
            data.append('installationId', installationId);
            data.append('file', file.files[0]);
            data.append('fieldName', fieldName);

            loader = bootLoaderDialog('Uploading image...');
            let message = await uploadImage(data);
            loader.hide();
            notify(message, 'success');
            loadImages();

        } catch (ex) {
            loader.hide();
            if (ex) {
                console.error(ex);
                if (typeof (ex) == 'string')
                    notify(ex, 'danger');
            }
        }

    });

    $(document).on('click', '.delete-image', async e => {
        let btn = $(e.currentTarget);
        let fieldName = btn.attr('fieldName');
        let installationId = $('#installationId').val();
        let loader;
        bootConfirm('Are you sure you want to delete this image?', {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {

                        loader = bootLoaderDialog('Deleting image...');
                        let message = await deleteImage(installationId, fieldName);
                        loader.hide();
                        notify(message, 'success');
                        loadImages();

                    } catch (ex) {
                        loader.hide();
                        if (ex) {
                            console.error(ex);
                            if (typeof (ex) == 'string')
                                notify(ex, 'danger');
                        }
                    }
                }
            }
        });
    });

    $('#rejectBtn').on('click', async e => {
        let btn = $(e.currentTarget);
        let comment = $.trim($('#comment').val());
        let installationId = $('#installationId').val();
        if (comment == '') {
            notify('comment is required', 'warning');
        } else {
            let loader;
            bootConfirm("Are you sure you want to reject this installation? You won't be able to make any changes after this.", {
                title: 'Confirm Action', size: 'small', callback: async (res) => {
                    if (res) {
                        try {
                            loader = bootLoaderDialog('Rejecting installation...');
                            let message = await rejectInstallation(installationId, comment);
                            loader.hide();
                            notify(message, 'success');
                            //loadImages();
                            setTimeout(() => {
                                location.reload();
                            }, 1500);

                        } catch (ex) {
                            loader.hide();
                            if (ex) {
                                console.error(ex);
                                if (typeof (ex) == 'string')
                                    notify(ex, 'danger');
                            }
                        }
                    }
                }
            });
        }

    });

    $('#completeBtn').on('click', async e => {
        let btn = $(e.currentTarget);
        let comment = $.trim($('#comment').val());
        let installationId = $('#installationId').val();

        let loader;
        bootConfirm("Are you sure you want to complete this installation? You won't be able to make any changes after this.", {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Completing installation...');
                        let message = await completeInstallation(installationId, comment);
                        loader.hide();
                        notify(message, 'success');
                        //loadImages();
                        setTimeout(() => {
                            location.reload();
                        }, 1500);

                    } catch (ex) {
                        loader.hide();
                        if (ex) {
                            console.error(ex);
                            if (typeof (ex) == 'string')
                                notify(ex, 'danger');
                        }
                    }
                }
            }
        });
    });



});

async function loadImages() {
    let installationId = $('#installationId').val();
    try {
        $('#imagesDiv').hide();
        $('#imgLoader').show();
        let images = await getImages(installationId);
        console.log(images);
        let uploaded = images.filter(i => i.path != null);
        let pending = images.filter(i => i.path == null);


        let pendingHtml = pending.map(i => {
            return `
                <div class="form-group col-lg-6 col-md-12 col-sm-6">
                    <div class="d-flex flex-column h-100">
                        <label for="file_${i.id}" class="f12 flex-fill">${i.required ? '*' : ''} ${i.caption}</label>
                        <div class="input-group mb-3">
                            <input type="text" id="ftext_${i.id}" class="form-control form-control-sm" disabled placeholder="Choose image" required />
                            <input type="file" id="file_${i.id}" class="d-none file" ftext="ftext_${i.id}" btn="fbtn_${i.id}" accept=".jpg,.jpeg,.png" />
                            <div class="input-group-append">
                                <label for="file_${i.id}" class="btn btn-outline-secondary btn-sm" type="button"><i class="fa fa-folder-open"></i></label>
                                <button type="button" id="fbtn_${i.id}" disabled class="btn btn-primary btn-sm py-0 upload"  style="height:31px;" file="file_${i.id}" fieldName="${i.name}"><i class="fa fa-upload"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
             `;
        }).join('');
        let uploadedHtml = uploaded.map(i => {
            return `
                <div class="col-xl-4 col-md-6 col-sm-4 col-6">
                    <div class="card">
                        <img src="${$base + i.path}" class="card-img-top i-image" style="cursor:pointer;" alt="${i.caption}">
                        <div class="card-body p-2">
                            ${i.caption}
                        </div>
                        <div class="card-footer p-2">
                            <button class="btn btn-sm btn-danger f10 delete-image d-block w-100" fieldName="${i.name}"><i class="fa fa-trash"></i> Delete</button>
                        </div>
                    </div>
                </div>
            `;
        }).join('');
        let body = `
                        <p>Images pending upload</p>
                            <hr class="my-2" />
                            <div class="row align-items-stretch">
                            ${pendingHtml}
                            </div>

                            <p>Uploaded images</p>
                            <hr class="my-2" />
                            <div class="row">
                                ${uploadedHtml}
                            </div>
         `;
        $('#imagesDiv').html(body);
        $('#imagesDiv').show();
        $('#imgLoader').hide();

    } catch (ex) {
        $('#imgLoader').hide();
        if (ex != null) {
            console.error(ex);
            notify(ex.message, 'danger');
        }
    }
}

function startInstallation(installationId, meterType, meterNumber, token) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + 'installations/StartInstallation';
                let data = {
                    installationId,
                    meterType,
                    meterNumber,
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

function getImages(installationId) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/${installationId}/images`;
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
            reject(ex.message);
        }
    });
    return promise;
}

function uploadImage(data) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (data == undefined || data == null) {
                reject('data is required');
            } else {
                let url = $base + `installations/UploadImage`;
                $.ajax({
                    type: 'POST',
                    url: url,
                    data,
                    processData: false,
                    contentType: false,
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
            reject(ex.message);
        }
    });
    return promise;
}

function deleteImage(installationId, fieldName) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/${installationId}/DeleteImage?fieldName=${fieldName}`;
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
                        reject(null);
                    }
                });
            }

        } catch (ex) {
            console.error(ex);
            reject(ex.message);
        }
    });
    return promise;
}

function rejectInstallation(installationId, comment) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/RejectInstallation`;
                let data = {
                    installationId,
                    comment: comment?.trim() == '' ? null : comment.trim()
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
            reject(ex.message);
        }
    });
    return promise;
}

function completeInstallation(installationId, comment) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/CompleteInstallation`;
                let data = {
                    installationId,
                    comment: comment?.trim() == '' ? null : comment.trim()
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
            reject(ex.message);
        }
    });
    return promise;
}