window.deleteOrder = async function deleteOrder(orderId) {
    // get token from localstorage
    const token = localStorage.getItem('skiserviceToken'); 

    if (!token) {
      alert('Sie sind nicht angemeldet.');
      return;
    }
  
    if (!confirm('Sind Sie sicher, dass Sie diesen Auftrag löschen möchten?')) {
      return;
    }
  
    try {
      const response = await fetch(`https://localhost:7214/api/service/delete/${orderId}`, {
        method: 'PATCH',
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
  