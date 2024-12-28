function showUygunluk() {
    document.getElementById('uygunlukModal').style.display = 'block';

    const calisanSelect = document.getElementById('modalCalisanSelect');
    calisanSelect.addEventListener('change', function () {
        const calisanId = this.value;
        if (!calisanId) return;

        fetch(`/api/Randevu/uygunluklar/${calisanId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`API Hatası: ${response.statusText}`);
                }
                return response.json();
            })
            .then(data => {
                console.log("API Yanıtı:", data);

                const list = document.getElementById('uygunlukList');
                list.innerHTML = '';

                if (!Array.isArray(data) || data.length === 0) {
                    list.innerHTML = '<li>Uygun zaman bilgisi bulunamadı.</li>';
                    return;
                }

                const gunler = {
                    Monday: "Pazartesi",
                    Tuesday: "Salı",
                    Wednesday: "Çarşamba",
                    Thursday: "Perşembe",
                    Friday: "Cuma",
                    Saturday: "Cumartesi",
                    Sunday: "Pazar"
                };

                data.forEach(item => {
                    const gun = gunler[item.gun] || item.gun;
                    const saat = item.saat;

                    if (gun && saat) {
                        const listItem = document.createElement('li');
                        listItem.textContent = `${gun}: ${saat}`;
                        list.appendChild(listItem);
                    }
                });
            })
            .catch(error => {
                console.error('API Hatası:', error);
                document.getElementById('uygunlukList').innerHTML = '<li>Uygunluk bilgileri alınamadı.</li>';
            });
    });
}

function closeModal() {
    document.getElementById('uygunlukModal').style.display = 'none';
}

 