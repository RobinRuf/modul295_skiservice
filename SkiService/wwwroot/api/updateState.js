// In updateState.js
window.updateState = async function updateState(orderId, currentStatus) {
    // get token from localstorage
    const token = localStorage.getItem('skiserviceToken');

    if (!token) {
        alert('Sie sind nicht angemeldet.');
        return;
    }

    let newStatus = '';
    if (currentStatus === 'Offen') {
        newStatus = 'In Arbeit';
    } else if (currentStatus === 'In Arbeit') {
        newStatus = 'Abgeschlossen';
    }

    try {
        const response = await fetch(`https://localhost:7214/api/service/update/${orderId}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ status: newStatus })
        });

        if (!response.ok) {
            throw new Error(`HTTP-Fehler: ${response.status}`);
        }

        alert('Auftragsstatus erfolgreich geändert.');

        // refetch orders
        window.fetchOrders();
    } catch (error) {
        console.error('Fehler beim Ändern des Auftragsstatus:', error);
        alert('Es gab ein Problem beim Ändern des Auftragsstatus.');
    }
}
