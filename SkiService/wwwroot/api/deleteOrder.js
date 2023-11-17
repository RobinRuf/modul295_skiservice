window.deleteOrder = async function deleteOrder(orderId) {
    // Abfragen des Tokens aus dem LocalStorage
    const token = localStorage.getItem('skiserviceToken'); // Ersetzen Sie 'skiserviceToken' durch den tatsächlichen Schlüssel, unter dem der Token gespeichert ist
  
    if (!token) {
      alert('Sie sind nicht angemeldet.');
      return;
    }
  
    if (!confirm('Sind Sie sicher, dass Sie diesen Auftrag löschen möchten?')) {
      return;
    }
  
    try {
      const response = await fetch(`https://localhost:7214/api/service/${orderId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      });
  
      if (!response.ok) {
        throw new Error(`HTTP-Fehler: ${response.status}`);
      }
  
      alert('Auftrag erfolgreich gelöscht.');

      // refetch orders
      window.fetchOrders();
    } catch (error) {
      console.error('Fehler beim Löschen des Auftrags:', error);
      alert('Es gab ein Problem beim Löschen des Auftrags.');
    }
  }
  