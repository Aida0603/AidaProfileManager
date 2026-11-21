(() => {
    const root = document.documentElement;
    const themeButton = document.querySelector('[data-theme-toggle]');
    const sidebarToggle = document.querySelector('[data-sidebar-toggle]');
    const sidebarClose = document.querySelector('[data-sidebar-close]');

    themeButton?.addEventListener('click', () => {
        const theme = root.dataset.theme === 'dark' ? 'light' : 'dark';
        root.dataset.theme = theme;
        try { localStorage.setItem('aida-theme', theme); } catch { }
    });

    const closeSidebar = () => {
        document.body.classList.remove('sidebar-open');
        sidebarToggle?.setAttribute('aria-expanded', 'false');
    };
    sidebarToggle?.addEventListener('click', () => {
        const opened = document.body.classList.toggle('sidebar-open');
        sidebarToggle.setAttribute('aria-expanded', String(opened));
    });
    sidebarClose?.addEventListener('click', closeSidebar);
    document.addEventListener('keydown', event => {
        if (event.key === 'Escape') closeSidebar();
    });

    document.querySelectorAll('[data-toast]').forEach(toast => {
        const dismiss = () => {
            toast.classList.add('toast-leave');
            window.setTimeout(() => toast.remove(), 260);
        };
        toast.querySelector('[data-toast-close]')?.addEventListener('click', dismiss);
        window.setTimeout(dismiss, 5200);
    });

    document.querySelectorAll('[data-char-count]').forEach(input => {
        const counter = input.closest('.field-group')?.querySelector('[data-char-current]');
        const update = () => { if (counter) counter.textContent = String(input.value.length); };
        input.addEventListener('input', update);
        update();
    });

    document.querySelectorAll('[data-password-input]').forEach(input => {
        const meter = input.parentElement?.querySelector('.password-meter') ?? input.closest('.field-group')?.querySelector('.password-meter');
        const update = () => {
            if (!meter) return;
            const value = input.value;
            const score = [value.length >= 8, /[A-Z]/.test(value), /[a-z]/.test(value), /\d/.test(value), /[^A-Za-z0-9]/.test(value)].filter(Boolean).length;
            meter.dataset.strength = value.length === 0 ? '0' : String(Math.min(4, score - 1));
        };
        input.addEventListener('input', update);
        update();
    });

    const deleteModal = document.getElementById('deleteModal');
    deleteModal?.addEventListener('show.bs.modal', event => {
        const trigger = event.relatedTarget;
        if (!trigger) return;
        deleteModal.querySelector('[data-delete-id-input]').value = trigger.getAttribute('data-delete-id') ?? '';
        deleteModal.querySelector('[data-delete-name]').textContent = trigger.getAttribute('data-delete-name') ?? 'Профиль';
    });

    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', event => {
            if (window.jQuery && jQuery.fn.valid && jQuery(form).data('validator') && !jQuery(form).valid()) return;
            if (!form.checkValidity()) return;
            const button = event.submitter?.matches('[data-loading-button]') ? event.submitter : form.querySelector('[data-loading-button]');
            if (!button || button.dataset.busy === 'true') return;
            button.dataset.busy = 'true';
            button.disabled = true;
            button.insertAdjacentHTML('afterbegin', '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
        });
    });
})();
