window.blazerHelcimPay = function (checkoutToken, dotNetRef) {
  console.log('[HelcimInterop] blazerHelcimPay called, token:', checkoutToken);

  if (!checkoutToken) {
    console.error('[HelcimInterop] checkoutToken is null/empty');
    return 'ERROR: checkoutToken is empty';
  }

  if (typeof appendHelcimPayIframe !== 'function') {
    console.error('[HelcimInterop] appendHelcimPayIframe is NOT defined — Helcim start.js may not have loaded.');
    return 'ERROR: Helcim start.js not loaded';
  }

  try {
    appendHelcimPayIframe(checkoutToken);
    console.log('[HelcimInterop] appendHelcimPayIframe called successfully');
  } catch(e) {
    console.error('[HelcimInterop] appendHelcimPayIframe threw:', e);
    return 'ERROR: ' + e.message;
  }

  function handler(event) {
    var key = 'helcim-pay-js-' + checkoutToken;
    if (event.data.eventName !== key) return;

    if (event.data.eventStatus === 'SUCCESS') {
      removeHelcimPayIframe();
      window.removeEventListener('message', handler);
      var resp = typeof event.data.eventMessage === 'string'
        ? JSON.parse(event.data.eventMessage)
        : event.data.eventMessage;
      var tx = resp.data || resp;
      dotNetRef.invokeMethodAsync('OnPaymentSuccess');
    } else if (event.data.eventStatus === 'ABORTED') {
      removeHelcimPayIframe();
      window.removeEventListener('message', handler);
      dotNetRef.invokeMethodAsync('OnPaymentDeclined');
    } else if (event.data.eventStatus === 'HIDE') {
      dotNetRef.invokeMethodAsync('OnPaymentHide');
    }
  }
  window.addEventListener('message', handler);

  return 'OK';
};
