// JobHub - front end behaviour (UI only; hook these up to the API later)
(function () {
    'use strict';

    function toast(message, icon) {
        var host = document.getElementById('toastHost');
        if (!host || !window.bootstrap) return;
        var el = document.createElement('div');
        el.className = 'toast app-toast align-items-center border-0';
        el.setAttribute('role', 'alert');
        el.innerHTML = '<div class="d-flex"><div class="toast-body"><i class="bi ' + (icon || 'bi-check-circle-fill text-success') + ' me-2"></i>' + message +
            '</div><button type="button" class="btn-close me-3 m-auto" data-bs-dismiss="toast"></button></div>';
        host.appendChild(el);
        var t = new bootstrap.Toast(el, { delay: 2800 });
        el.addEventListener('hidden.bs.toast', function () { el.remove(); });
        t.show();
    }
    window.jobhubToast = toast;

    document.addEventListener('DOMContentLoaded', function () {
        // Auto-hide server toasts
        document.querySelectorAll('.toast[data-autohide]').forEach(function (el) {
            var t = new bootstrap.Toast(el, { delay: 3500 });
            t.show();
        });

        // Save / unsave job (TODO: POST /api/saved-jobs/{id})
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-save-job]');
            if (!btn) return;
            e.preventDefault();
            var saved = btn.classList.toggle('saved');
            var icon = btn.querySelector('i');
            if (icon) icon.className = 'bi ' + (saved ? 'bi-bookmark-fill' : 'bi-bookmark');
            toast(saved ? 'Job saved to your list' : 'Job removed from saved', saved ? 'bi-bookmark-heart-fill text-primary' : 'bi-bookmark text-muted');
        });

        // Sidebar (mobile)
        var sb = document.getElementById('sidebar'), bd = document.getElementById('sidebarBackdrop'), tg = document.getElementById('sidebarToggle');
        if (sb && tg) {
            var toggle = function () { sb.classList.toggle('open'); bd.classList.toggle('show'); };
            tg.addEventListener('click', toggle);
            bd.addEventListener('click', toggle);
        }

        // Mark all notifications as read
        var markAll = document.querySelector('[data-mark-all-read]');
        if (markAll) markAll.addEventListener('click', function () {
            document.querySelectorAll('.notif-item.unread').forEach(function (n) { n.classList.remove('unread'); });
            toast('All notifications marked as read');
        });

        // Confirm dialogs for destructive actions
        document.querySelectorAll('form[data-confirm]').forEach(function (f) {
            f.addEventListener('submit', function (e) {
                if (!confirm(f.getAttribute('data-confirm'))) e.preventDefault();
            });
        });

        // Auto-submit filters
        document.querySelectorAll('[data-autosubmit]').forEach(function (el) {
            el.addEventListener('change', function () { el.form.submit(); });
        });

        // Show / hide password
        document.querySelectorAll('[data-toggle-pass]').forEach(function (b) {
            b.addEventListener('click', function () {
                var input = document.querySelector(b.getAttribute('data-toggle-pass'));
                var show = input.type === 'password';
                input.type = show ? 'text' : 'password';
                b.querySelector('i').className = 'bi ' + (show ? 'bi-eye-slash' : 'bi-eye');
            });
        });

        // Register: show company field for recruiters
        var roleRadios = document.querySelectorAll('input[name="Role"]');
        var companyBox = document.getElementById('companyBox');
        if (roleRadios.length && companyBox) {
            var sync = function () {
                var r = document.querySelector('input[name="Role"]:checked');
                companyBox.classList.toggle('d-none', !r || r.value !== 'Recruiter');
            };
            roleRadios.forEach(function (r) { r.addEventListener('change', sync); });
            sync();
        }

        // File upload zone (CV)
        var zone = document.getElementById('cvZone'), file = document.getElementById('cvFile');
        if (zone && file) {
            zone.addEventListener('click', function () { file.click(); });
            ['dragover', 'dragenter'].forEach(function (ev) { zone.addEventListener(ev, function (e) { e.preventDefault(); zone.classList.add('drag'); }); });
            ['dragleave', 'drop'].forEach(function (ev) { zone.addEventListener(ev, function (e) { e.preventDefault(); zone.classList.remove('drag'); }); });
            zone.addEventListener('drop', function (e) { file.files = e.dataTransfer.files; file.dispatchEvent(new Event('change')); });
            file.addEventListener('change', function () {
                var label = document.getElementById('cvName');
                if (file.files.length && label) label.textContent = file.files[0].name;
            });
        }

        // Skills tag input
        var box = document.getElementById('skillBox');
        if (box) {
            var input = box.querySelector('input[type=text]'), hidden = document.getElementById('skillsHidden');
            var sync2 = function () {
                hidden.value = Array.from(box.querySelectorAll('.skill-chip')).map(function (c) { return c.dataset.v; }).join(',');
            };
            var add = function (v) {
                v = v.trim(); if (!v) return;
                var chip = document.createElement('span');
                chip.className = 'skill-chip'; chip.dataset.v = v;
                chip.innerHTML = v.replace(/</g, '&lt;') + '<button type="button" aria-label="remove">&times;</button>';
                box.insertBefore(chip, input); sync2();
            };
            box.addEventListener('click', function (e) {
                if (e.target.closest('button')) { e.target.closest('.skill-chip').remove(); sync2(); }
            });
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter' || e.key === ',') { e.preventDefault(); add(input.value); input.value = ''; }
            });
            sync2();
        }
    });
})();
