// In setEmployee.js
window.setEmployee = // In setEmployee.js
async function setEmployee(orderId) {
    // Abfragen des Tokens aus dem LocalStorage
    const token = localStorage.getItem('skiserviceToken');

    if (!token) {
        alert('Sie sind nicht angemeldet.');
        return;
    }

    // Extrahieren des Benutzernamens aus dem Token
    const payloadBase64 = token.split('.')[1];
    const decodedPayload = atob(payloadBase64);
    const payload = JSON.parse(decodedPayload);
    const username = payload.nameid;

    try {
        const response = await fetch(`https://localhost:7214/api/service/assign/${orderId}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ username })
        });

        if (!response.ok) {
            throw new Error(`HTTP-Fehler: ${response.status}`);
        }

        alert('Auftrag erfolgreich zugewiesen.');

        // Liste der Aufträge neu laden
        window.fetchOrders();
    } catch (error) {
        console.error('Fehler beim Zuweisen des Auftrags:', error);
        alert('Es gab ein Problem beim Zuweisen des Auftrags.');
    }
}
