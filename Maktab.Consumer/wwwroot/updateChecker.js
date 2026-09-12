window.appUpdateChecker = (() => {
    let dotNetRef = null;
    let notified = false;
    let reloading = false;

    function isLocalDev() {
        return location.hostname === 'localhost' || location.hostname === '127.0.0.1';
    }

    function register() {
        if (!('serviceWorker' in navigator)) return;
        if (isLocalDev()) {
            navigator.serviceWorker.getRegistrations().then(regs => regs.forEach(r => r.unregister()));
            return;
        }
        navigator.serviceWorker.register('service-worker.js', { updateViaCache: 'none' });
    }

    function reload() {
        if (reloading) return;
        reloading = true;
        window.location.replace(window.location.href);
    }

    function notifyUpdate() {
        if (notified || !dotNetRef) return;
        notified = true;
        dotNetRef.invokeMethodAsync('OnUpdateAvailable');
    }

    function waitForSwReady(timeoutMs) {
        return new Promise(resolve => {
            const timeout = setTimeout(resolve, timeoutMs);
            const onMessage = (event) => {
                if (event.data?.type === 'SW_READY') {
                    clearTimeout(timeout);
                    navigator.serviceWorker.removeEventListener('message', onMessage);
                    resolve();
                }
            };
            navigator.serviceWorker.addEventListener('message', onMessage);
        });
    }

    async function init(ref) {
        dotNetRef = ref;
        if (!('serviceWorker' in navigator) || isLocalDev()) return;

        const reg = await navigator.serviceWorker.ready;
        if (reg.waiting) notifyUpdate();

        reg.addEventListener('updatefound', () => {
            reg.installing?.addEventListener('statechange', () => {
                if (reg.waiting) notifyUpdate();
            });
        });

        document.addEventListener('visibilitychange', () => {
            if (!document.hidden) reg.update().catch(() => { });
        });
    }

    async function applyUpdate() {
        if (reloading) return;

        const reg = await navigator.serviceWorker.getRegistration();
        if (reg?.waiting) {
            reg.waiting.postMessage({ type: 'SKIP_WAITING' });
            await waitForSwReady(3000);
        }

        reload();
    }

    return { register, init, applyUpdate };
})();
