$(document).on("submit", "#addForm", function (e) {
    e.preventDefault();

    const form = $(this)[0];
    const formData = new FormData(form);

    $.ajax({
        url: "/Intersections/AddIntersection",
        method: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (result) {
            $(".text-danger").text("");

            if (result.success) {
                $("#formModal").modal('hide');
                resetAddModal();
                reloadIntersectionList();
            } else {
                result.errors.forEach(err => {
                    const fieldName = err.propertyName;
                    const errorMessage = err.errorMessage;

                    const input = $(`[name='${fieldName}']`);

                    const span = input.closest('.mb-3').find("span.text-danger");

                    if (span.length > 0) {
                        span.text(errorMessage);
                    }
                });
            }
        },
        error: function () {
            alert("Bir hata oluştu.");
        }
    });
});
function resetAddModal() {
    const modal = $('#formModal');

    modal.find('form')[0].reset();
    modal.find('input[type="file"]').val(null);
    modal.find('.text-danger').text('');
    modal.find('.image-preview').empty();
}

$('body').on('click', '.btn-soft-delete', function () {
    var id = $(this).data('id');
    if (!confirm("Bu kaydı gizlemek istediğinize emin misiniz?")) {
        return;
    }

    $.ajax({
        url: '/Intersections/SoftDelete',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                if (isDetailsPage()) {
                    window.location.href = "/Intersections/Index";
                }
                else {
                    reloadIntersectionList();
                }

            } else {
                alert('Soft delete başarısız!');
            }
        },
        error: function () {
            alert('Hata oluştu!');
        }
    });
});

$('body').off('click', '.btn-make-active').on('click', '.btn-make-active', function () {
    var id = $(this).data('id');
    if (!confirm("Bu kaydı aktif etmek istediğinize emin misiniz?")) {
        return;
    }

    $.ajax({
        url: '/Intersections/MakeActive',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                if (isDetailsPage()) {
                    window.location.href = "/Intersections/DeactiveList";
                }
                else {
                    reloadDeactiveList();
                }
            } else {
                alert("Aktif etme işlemi başarısız");
            }
        },
        error: function () {
            alert('sunucu hatası!');
        }
    });
});

$('body').on('click', '.btn-hard-delete', function () {
    var id = $(this).data('id');
    if (!confirm("Bu kaydı kalıcı olarak silmek istediğinize emin misiniz? Bu işlem geri alınamaz!")) {
        return;
    }

    $.ajax({
        url: '/Intersections/HardDelete',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                if (isDetailsPage()) {
                    window.location.href = "/Intersections/Index";
                }
                else {
                    reloadIntersectionList();
                }

            } else {
                alert('Hard delete başarısız!');
            }
        },
        error: function () {
            alert('Sunucu hatası!');
        }
    });
});

function reloadIntersectionList() {
    $.ajax({
        url: '/Intersections/Index',
        type: 'GET',
        success: function (data) {
            $('#intersectionTable').html(data);
            setTimeout(() => {
                if ($(window).width() < 768) {
                    setupCardFeatures();
                } else {
                    setupTableFeatures();
                }
            }, 50);
        },
        error: function () {
            alert("Liste güncellenemedi")
        }
    });
}
function reloadDeactiveList() {
    $.ajax({
        url: '/Intersections/DeactiveList',
        type: 'GET',
        success: function (data) {
            $('#intersectionTable').html(data);
            setTimeout(() => {
                if ($(window).width() < 768) {
                    setupCardFeatures();
                } else {
                    setupTableFeatures();
                }
            }, 50);
        },
        error: function () {
            alert("Liste güncellenemedi")
        }
    });
}

function isDetailsPage() {
    return $('body').data('page') === 'details';
}
function isIndexPage() {
    return $('body').data('page') === 'index';
}
function isDeactivePage() {
    return $('body').data('page') == 'deactive';
}

$(document).on("click", ".edit-button", function () {
    const id = $(this).data("id");

    $.get("/Intersections/Edit/" + id, function (modalHtml) {
        $("#editModal").remove();
        $("body").append(modalHtml);
        $("#editModal").modal("show");
        loadImages(id);
    });
});

$(document).on('submit', '#editForm', function (e) {
    e.preventDefault();

    var form = $(this)[0];
    var formData = new FormData(form);

    $.ajax({
        url: '/Intersections/Edit',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#editForm .text-danger').text("");

            if (response && response.success) {
                alert(response.message || "Güncelleme başarılı!");
                $('#editModal').modal('hide');
                location.reload();
                return;
            }

            const errors = (response && response.errors) ? response.errors : [];
            if (!errors.length) {
                alert((response && response.message) || "İşlem tamamlanamadı.");
                return;
            }

            errors.forEach(err => {
                const fieldName = err.propertyName;
                const errorMessage = err.errorMessage;

                const input = $(`[name='${fieldName}']`);
                const span = input.closest('.mb-3').find("span.text-danger");

                if (span.length > 0) {
                    span.text(errorMessage);
                }
            });
        },
        error: function () {
            alert("Sunucu hatası!");
        }
    });
});

$(document).on("hidden.bs.modal", "#editModal", function () {
    $(this).remove();
});



function loadImages(id) {
    $.get("/Intersections/GetImages/" + id, function (imagePaths) {
        const container = $("#existing-images");
        container
        container.empty();

        if (imagePaths.length === 0) {
            container.append("<p>Bu kavşağa ait görsel yok. </p>");
            return;
        }

        imagePaths.forEach(path => {
            const wrapper = $("<div>").addClass("position-relative d-inline-block");

            const img = $("<img>")
                .attr("src", path)
                .addClass("img-thumbnail")
                .css({ width: "150px", height: "auto" });

            const closeBtn = $("<button>")
                .addClass("btn-deleteImage position-absolute top-0 end-0")
                .attr("type", "button")
                .attr("data-image-path", path)
                .html("&times;")

            wrapper.append(img).append(closeBtn);
            container.append(wrapper);
        });
    });
}

$(document).on("change", ".imageUpload", function () {
    const preview = $(this).closest("form").find(".image-preview");
    preview.empty();

    const files = this.files;
    if (!files || files.length === 0) return;

    Array.from(files).forEach(file => {
        const reader = new FileReader();
        reader.onload = function (e) {
            const img = $("<img>")
                .attr("src", e.target.result)
                .addClass("img-thumbnail")
                .css({ width: "150px", height: "auto" });
            preview.append(img);
        };
        reader.readAsDataURL(file);
    });
});

$(document).on("click", ".btn-deleteImage", function () {
    const imagePath = $(this).data("image-path");
    const wrapper = $(this).parent();

    $.ajax({
        url: "/Intersections/DeleteImage",
        method: "POST",
        data: { imagePath: imagePath },
        success: function (response) {
            if (response.success) {
                wrapper.remove();
            } else {
                alert("Sunucu resmi silemedi: " + response.message);
            }
        },
        error: function () {
            alert("Silme işlemi sırasında hata oluştu.");
        }
    });
});

$(document).on("click", "#formModal .btn-close", function () {
    const form = $("#addForm");

    form.find("input:not([type='file']), textarea").val("");

    form.find("input[type='file']").val(null);

    form.find(".text-danger").text("");
});


//pagination and search filter
function setupTableFeatures() {
    let currentPage = 1;
    let allRows = $('table tbody tr');

    function getRowsPerPage() {
        return parseInt($('#rowsPerPageSelect').val());
    }

    function showPage(page, filteredRows) {
        currentPage = page;
        let rowsperPage = getRowsPerPage();
        let start = (page - 1) * rowsperPage;
        let end = start + rowsperPage;

        allRows.hide();
        filteredRows.slice(start, end).show();
        updatePagination(filteredRows.length);
    }

    function updatePagination(totalRows) {
        let rowsPerPage = getRowsPerPage();
        let pageCount = Math.ceil(totalRows / rowsPerPage);
        let pagination = $('.pagination');
        pagination.empty();

        let maxVisiblePages = 7;
        let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
        let endPage = startPage + maxVisiblePages - 1;

        if (endPage > pageCount) {
            endPage = pageCount;
            startPage = Math.max(1, endPage - maxVisiblePages + 1);
        }

        if (startPage > 1) {
            pagination.append(`<li class="page-item">
            <a class="page-link bg-warning text-dark" href="#">1</a>
        </li>`);
            if (startPage > 2) {
                pagination.append(`<li class="page-item disabled"><span class="page-link bg-dark text-warning">...</span></li>`);
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            let isActive = i === currentPage;
            let liClass = `page-item ${isActive ? 'active' : ''}`;
            let aClass = `page-link ${isActive ? 'bg-dark text-warning' : 'bg-warning text-dark'}`;

            pagination.append(
                `<li class="${liClass}">
                <a class="${aClass}" href="#">${i}</a>
            </li>`
            );
        }

        if (endPage < pageCount) {
            if (endPage < pageCount - 1) {
                pagination.append(`<li class="page-item disabled"><span class="page-link bg-dark text-warning">...</span></li>`);
            }
            pagination.append(`<li class="page-item">
            <a class="page-link bg-warning text-dark" href="#">${pageCount}</a>
        </li>`);
        }
    }

    function filterRows() {
        allRows = $('table tbody tr');
        let searchVal = $('.searchInput').val().toLowerCase().trim();

        let filtered = allRows.filter(function () {
            let kkcId = $(this).find('td:eq(0)').text().toLowerCase();
            let title = $(this).find('td:eq(1)').text().toLowerCase();
            return kkcId.includes(searchVal) || title.includes(searchVal);
        });

        showPage(1, filtered);
    }

    $('#rowsPerPageSelect').off('change').on('change', function () {
        filterRows();
    });

    $('.searchInput').off('keyup').on('keyup', function () {
        filterRows();
    });

    $(document).off('click', '.pagination .page-link').on('click', '.pagination .page-link', function (e) {
        e.preventDefault();
        let selectedPage = parseInt($(this).text());
        let searchVal = String(($('.searchInput').val() ?? '')).toLowerCase().trim();

        let filtered = allRows.filter(function () {
            let kkcId = $(this).find('td:eq(0)').text().toLowerCase();
            let title = $(this).find('td:eq(1)').text().toLowerCase();
            return kkcId.includes(searchVal) || title.includes(searchVal);
        });

        showPage(selectedPage, filtered);
    });

    filterRows();
}

function setupCardFeatures() {
    let currentPage = 1;
    let allCards = $('.d-block.d-md-none .col');

    function getRowsPerPage() {
        return parseInt($('#rowsPerPageSelect').val());
    }

    function showCardPage(page, filteredCards) {
        currentPage = page;
        let rowsPerPage = getRowsPerPage();
        let start = (page - 1) * rowsPerPage;
        let end = start + rowsPerPage;

        allCards.hide();
        filteredCards.slice(start, end).show();
        updateCardPagination(filteredCards.length);
    }

    function updateCardPagination(totalCards) {
        let rowsPerPage = getRowsPerPage();
        let pageCount = Math.ceil(totalCards / rowsPerPage);
        let pagination = $('.pagination');
        pagination.empty();

        let maxVisiblePages = 7;
        let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
        let endPage = startPage + maxVisiblePages - 1;

        if (endPage > pageCount) {
            endPage = pageCount;
            startPage = Math.max(1, endPage - maxVisiblePages + 1);
        }

        if (startPage > 1) {
            pagination.append(`<li class="page-item">
            <a class="page-link bg-warning text-dark" href="#">1</a>
        </li>`);
            if (startPage > 2) {
                pagination.append(`<li class="page-item disabled"><span class="page-link bg-dark text-warning">...</span></li>`);
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            let isActive = i === currentPage;
            let liClass = `page-item ${isActive ? 'active' : ''}`;
            let aClass = `page-link ${isActive ? 'bg-dark text-warning' : 'bg-warning text-dark'}`;

            pagination.append(
                `<li class="${liClass}">
                <a class="${aClass}" href="#">${i}</a>
            </li>`
            );
        }

        if (endPage < pageCount) {
            if (endPage < pageCount - 1) {
                pagination.append(`<li class="page-item disabled"><span class="page-link bg-dark text-warning">...</span></li>`);
            }
            pagination.append(`<li class="page-item">
            <a class="page-link bg-warning text-dark" href="#">${pageCount}</a>
        </li>`);
        }
    }

    function filterCards() {
        allCards = $('.d-block.d-md-none .col');
        let searchVal = String(($('.searchInput').val() ?? '')).toLowerCase().trim();

        let filtered = allCards.filter(function () {
            let kkcId = $(this).find('.badge').text().toLowerCase();
            let title = $(this).find('.card-title').text().toLowerCase();
            return kkcId.includes(searchVal) || title.includes(searchVal);
        });

        showCardPage(1, filtered);
    }

    $('#rowsPerPageSelect').off('change').on('change', function () {
        filterCards();
    });

    $('.searchInput').off('keyup').on('keyup', function () {
        filterCards();
    });

    $(document).off('click', '.pagination .page-link').on('click', '.pagination .page-link', function (e) {
        e.preventDefault();
        let selectedPage = parseInt($(this).text());
        let searchVal = $('.searchInput').val().toLowerCase().trim();

        let filtered = allCards.filter(function () {
            let kkcId = $(this).find('.badge').text().toLowerCase();
            let title = $(this).find('.card-title').text().toLowerCase();
            return kkcId.includes(searchVal) || title.includes(searchVal);
        });

        showCardPage(selectedPage, filtered);
    });

    filterCards();
}


$(function () {
    if (isIndexPage() || isDeactivePage()) {
        if ($(window).width() < 768) {
            setupCardFeatures();
        } else {
            setupTableFeatures();
        }

    }
});

$(function () {
    let isDateAscGlobal = true;

    function sortTableByDataSort($table, asc) {
        const $tbody = $table.find('tbody');
        const rows = $tbody.children('tr').get();

        rows.sort((a, b) => {
            // Her satırda tarih hücresini data-sort ile bul
            const aKey = $(a).find('td[data-sort]').first().attr('data-sort') || '';
            const bKey = $(b).find('td[data-sort]').first().attr('data-sort') || '';
            const da = aKey ? new Date(aKey) : new Date(0);
            const db = bKey ? new Date(bKey) : new Date(0);
            return asc ? (da - db) : (db - da);
        });

        $tbody.empty().append(rows);
    }

    $(document).off('click', '.sort-date-btn').on('click', '.sort-date-btn', function (e) {
        e.preventDefault();

        const $icon = $(this).find('i');
        $icon.removeClass('bi-arrow-up bi-arrow-down bi-arrow-down-up')
            .addClass(isDateAscGlobal ? 'bi-arrow-up' : 'bi-arrow-down');

        // Mobil: kartları sırala (col[data-sort])
        if (window.matchMedia('(max-width: 767.98px)').matches) {
            const $row = $('.d-block.d-md-none .row');
            const cards = $row.children('.col').get();

            cards.sort((a, b) => {
                const da = new Date($(a).attr('data-sort') || 0);
                const db = new Date($(b).attr('data-sort') || 0);
                return isDateAscGlobal ? (da - db) : (db - da);
            });

            $row.empty().append(cards);
        } else {
            // Masaüstü: sayfadaki tüm tabloları sırala
            $('.js-date-table').each(function () {
                sortTableByDataSort($(this), isDateAscGlobal);
            });
        }

        isDateAscGlobal = !isDateAscGlobal;
    });
});


// Adding Report process


$(document).on('click', '.report-button', function () {
    const intersectionId = $(this).data('id');

    $('#reportModal').remove();

    $.get('/Report/AddReport', function (html) {
        $('body').append(html);
        const $modal = $('#reportModal');
        $modal.find('input[name="IntersectionID"]').val(intersectionId);
        $modal.modal('show');
    });
});

$(document).on('submit', '#reportForm', function (e) {
    e.preventDefault();

    const $form = $(this);
    const fd = new FormData(this);
    const $submit = $form.find('[type="submit"]').prop('disabled', true);

    $form.find('.text-danger').text('');

    $.ajax({
        url: $form.attr('action') || '/Report/AddReport',
        method: $form.attr('method') || 'POST',
        data: fd,
        contentType: false,
        processData: false,
        dataType: 'json',

        success: function (res) {
            if (res.success) {
                $('#reportModal').modal('hide');
                alert('Rapor başarılı bir şekilde eklendi!');
                location.reload();
            } else if (Array.isArray(res.errors)) {
                res.errors.forEach(function (err) {
                    const name = err.propertyName;
                    const msg = err.errorMessage;

                    let $span = $form.find('[data-valmsg-for="' + name + '"]');
                    if ($span.length === 0) {
                        const $input = $form.find('[name="' + name + '"]').first();
                        $span = $input.closest('.mb-3').find('span.text-danger').first();
                    }
                    if ($span.length) $span.text(msg);
                });
            } else {
                alert(res.message || 'Beklenmeyen yanıt.');
            }
        },
        error: function (xhr) {
            alert(xhr.responseJSON?.message || 'Sunucu hatası!');
        },
        complete: function () {
            $submit.prop('disabled', false);
        }
    });
});

$(document).on('hidden.bs.modal', '#reportModal', function () {
    $(this).remove();
});

