(function () {
    function debounce(fn, ms) { let t; return (...a) => { clearTimeout(t); t = setTimeout(() => fn(...a), ms); }; }

    function FilteredPaginator(opts) {
        this.itemSelector = opts.itemSelector;
        this.paginationId = opts.paginationId;
        this.noResultId = opts.noResultId || null;
        this.perPage = opts.perPage || 10;
        this.current = 1;

        this.itemsAll = Array.from(document.querySelectorAll(this.itemSelector));
        this.itemsFiltered = this.itemsAll.slice();
        this.pagination = document.getElementById(this.paginationId);
        this.noResultEl = this.noResultId ? document.getElementById(this.noResultId) : null;

        if (!this.itemsAll.length && this.pagination) this.pagination.innerHTML = '';
        this.render();
    }

    FilteredPaginator.prototype.totalPages = function () {
        return Math.max(1, Math.ceil(this.itemsFiltered.length / this.perPage));
    };

    FilteredPaginator.prototype._applyDisplay = function (start, end) {
        // önce hepsini gizle
        this.itemsAll.forEach(el => el.style.display = 'none');
        // sonra sadece filtrelenenlerin sayfaya düşenlerini göster
        this.itemsFiltered.forEach((el, idx) => {
            if (idx >= start && idx < end) el.style.display = '';
        });
        // sonuç yok mesajı
        if (this.noResultEl) this.noResultEl.classList.toggle('d-none', this.itemsFiltered.length !== 0);
    };

    FilteredPaginator.prototype.goto = function (page) {
        const total = this.totalPages();
        this.current = Math.min(Math.max(1, page), total);
        this.render();
    };

    FilteredPaginator.prototype.renderControls = function () {
        if (!this.pagination) return;
        const total = this.totalPages();
        const curr = this.current;

        const makeLi = (label, page, disabled = false, active = false) => {
            const li = document.createElement('li');
            li.className = 'page-item' + (disabled ? ' disabled' : '') + (active ? ' active' : '');
            const a = document.createElement('a');
            a.className = 'page-link';
            a.href = '#';
            a.textContent = label;
            a.addEventListener('click', (e) => {
                e.preventDefault();
                if (!disabled && !active) this.goto(page);
            });
            li.appendChild(a);
            return li;
        };

        const ul = document.createElement('ul');
        ul.className = 'pagination pagination-sm mb-0';

        ul.appendChild(makeLi('«', curr - 1, curr === 1));

        const maxButtons = 7;
        let start = Math.max(1, curr - Math.floor(maxButtons / 2));
        let end = start + maxButtons - 1;
        if (end > total) { end = total; start = Math.max(1, end - maxButtons + 1); }

        if (start > 1) {
            ul.appendChild(makeLi('1', 1, false, curr === 1));
            if (start > 2) {
                const ell = document.createElement('li');
                ell.className = 'page-item disabled';
                ell.innerHTML = '<span class="page-link">…</span>';
                ul.appendChild(ell);
            }
        }

        for (let p = start; p <= end; p++) {
            ul.appendChild(makeLi(String(p), p, false, p === curr));
        }

        if (end < total) {
            if (end < total - 1) {
                const ell = document.createElement('li');
                ell.className = 'page-item disabled';
                ell.innerHTML = '<span class="page-link">…</span>';
                ul.appendChild(ell);
            }
            ul.appendChild(makeLi(String(total), total, false, curr === total));
        }

        ul.appendChild(makeLi('»', curr + 1, curr === total));

        this.pagination.innerHTML = '';
        this.pagination.appendChild(ul);
    };

    FilteredPaginator.prototype.render = function () {
        const start = (this.current - 1) * this.perPage;
        const end = start + this.perPage;
        this._applyDisplay(start, end);
        this.renderControls();
    };

    FilteredPaginator.prototype.applyFilter = function (query) {
        const q = (query || '').trim().toLocaleLowerCase('tr-TR');
        if (!q) {
            this.itemsFiltered = this.itemsAll.slice();
            this.current = 1;
            this.render();
            return;
        }
        this.itemsFiltered = this.itemsAll.filter(el => {
            const kkcid = (el.getAttribute('data-kkcid') || '').toString().toLocaleLowerCase('tr-TR');
            const title = (el.getAttribute('data-title') || '').toString().toLocaleLowerCase('tr-TR');
            return kkcid.includes(q) || title.includes(q);
        });
        this.current = 1;
        this.render();
    };

    document.addEventListener('DOMContentLoaded', function () {
        const ixD = new FilteredPaginator({ itemSelector: '.ix-item-d', paginationId: 'ixPaginationD', noResultId: 'ixNoResultD', perPage: 8 });
        const ixM = new FilteredPaginator({ itemSelector: '.ix-item-m', paginationId: 'ixPaginationM', noResultId: 'ixNoResultM', perPage: 5 });

        const ixSearchD = document.getElementById('ixSearchD');
        if (ixSearchD) {
            const run = debounce(v => ixD.applyFilter(v), 120);
            ixSearchD.addEventListener('input', e => run(e.target.value));
            ixSearchD.addEventListener('keydown', e => { if (e.key === 'Escape') { ixSearchD.value = ''; ixD.applyFilter(''); } });
        }
        const ixSearchM = document.getElementById('ixSearchM');
        if (ixSearchM) {
            const run = debounce(v => ixM.applyFilter(v), 120);
            ixSearchM.addEventListener('input', e => run(e.target.value));
            ixSearchM.addEventListener('keydown', e => { if (e.key === 'Escape') { ixSearchM.value = ''; ixM.applyFilter(''); } });
        }

        const rpD = new FilteredPaginator({ itemSelector: '.rp-item-d', paginationId: 'rpPaginationD', noResultId: 'rpNoResultD', perPage: 8 });
        const rpM = new FilteredPaginator({ itemSelector: '.rp-item-m', paginationId: 'rpPaginationM', noResultId: 'rpNoResultM', perPage: 5 });

        const rpSearchD = document.getElementById('rpSearchD');
        if (rpSearchD) {
            const run = debounce(v => rpD.applyFilter(v), 120);
            rpSearchD.addEventListener('input', e => run(e.target.value));
            rpSearchD.addEventListener('keydown', e => { if (e.key === 'Escape') { rpSearchD.value = ''; rpD.applyFilter(''); } });
        }
        const rpSearchM = document.getElementById('rpSearchM');
        if (rpSearchM) {
            const run = debounce(v => rpM.applyFilter(v), 120);
            rpSearchM.addEventListener('input', e => run(e.target.value));
            rpSearchM.addEventListener('keydown', e => { if (e.key === 'Escape') { rpSearchM.value = ''; rpM.applyFilter(''); } });
        }
    });
})();

function isEmployeePage() {
    return $('body').data('page') === 'getEmployee';
}

$('body').on('click', '.btn-user-delete', function () {
    var id = $(this).data('id');
    const userTable = document.querySelector("#userTable");
    if (!confirm("Kullanıcıyı silmek istediğine emin misin ?")) {
        return;
    }

    $.ajax({
        url: '/Employee/DeleteEmployee',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                if (!isEmployeePage()) {
                    $(userTable).load("/Employee/AllEmployees?ts=" + Date.now());
                }
                else {
                    window.location.href = "/Employee/AllEmployees";
                }
            }
            else {
                alert("Kullanıcı silinemedi: " + response.message)
            }
        },
        error: function() {
            alert("Silme işlemi sırasında hata oluştu");
        }
    });
});