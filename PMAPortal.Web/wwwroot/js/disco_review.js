$(() => {
    loadImages();

    $(document).on('click', '.i-image', e => {
        let img = $(e.currentTarget);
        var url = img.attr('src');
        var caption = img.attr('alt');

        $('#modalImg').attr('src', url);
        $('#modalImg').attr('alt', caption);
        $('#modalCaption').html(caption);

        $('#imageModal').modal('show');
    });

    

    $('#rejectBtn').on('click', async e => {
        let btn = $(e.currentTarget);
        let comment = $.trim($('#comment').val());
        let installationId = $('#installationId').val();
        if (comment == '') {
            notify('comment is required', 'warning');
        } else {
            let loader;
            bootConfirm("Are you sure you want to reject this installation?", {
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
        bootConfirm("Are you sure you want to approve this installation?", {
            title: 'Confirm Action', size: 'small', callback: async (res) => {
                if (res) {
                    try {
                        loader = bootLoaderDialog('Completing installation...');
                        let message = await approveInstallation(installationId, comment);
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
                    <div class="card my-1">
                        <img src="${$base + i.path}" class="card-img-top i-image" style="cursor:pointer;" alt="${i.caption}">
                        <div class="card-body p-2">
                            ${i.caption}
                        </div>
                       <!-- <div class="card-footer p-2">
                            <button class="btn btn-sm btn-danger f10 delete-image d-block w-100" fieldName="${i.name}"><i class="fa fa-trash"></i> Delete</button>
                        </div>-->
                    </div>
                </div>
            `;
        }).join('');
        let body = `
                            <!--<p>Images pending upload</p>
                            <hr class="my-2" />
                            <div class="row align-items-stretch">
                            ${pendingHtml}
                            </div>-->

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

function rejectInstallation(installationId, comment) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/DiscoRejectInstallation`;
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

function approveInstallation(installationId, comment) {
    var promise = new Promise((resolve, reject) => {
        try {
            if (installationId == undefined || installationId == 0) {
                reject('Installation id is required');
            } else {
                let url = $base + `installations/DiscoApproveInstallation`;
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