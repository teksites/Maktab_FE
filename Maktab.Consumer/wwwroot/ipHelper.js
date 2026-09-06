window.ipHelper = {
     getClientIp: async function () {
          console.log('[ipHelper] getClientIp called');
          try {
               let response = await fetch("https://api.ipify.org?format=json");
               if (!response) {
                    console.error('[ipHelper] Failed to fetch IP Response');
                    return '';
               }

               let data = await response.json();
               if (!data || !data.ip) {
                    console.error('[ipHelper] Failed to fetch IP Data');
                    return '';
               }

               return data.ip;

          } catch (e) {
               console.error("Error fetching IP:", e);
               return null;
          }
     }
};
