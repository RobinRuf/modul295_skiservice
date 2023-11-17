document.addEventListener("DOMContentLoaded", () => {
    const ordersContainer = document.getElementById("ordersContainer");

    // Funktion zum Generieren der Auftragskarten
    function displayOrders(orders) {
        orders.forEach((order) => {
            const orderCard = document.createElement("div");
            orderCard.className =
                "p-4 bg-white border border-gray-200 rounded-lg shadow-lg text-gray-900 dark:bg-gray-800 dark:border-gray-700 dark:text-white mr-4 mb-4";
            orderCard.innerHTML = `
                <div class="flex flex-col">
                  <h3 class="text-xl font-semibold mb-2">Auftrag ${order.orderID}</h3>
                  <p class="font-semibold">Status: ${order.status}</p>
                  <p class="font-semibold">Kundeninformationen:</p>
                  <p>Kunde: ${order.customerName}</p>
                  <p>E-Mail: ${order.customerEmail}</p>
                  <p>Telefon: ${order.customerPhone}</p>
                  <p>-----------------------------------</p>
                  <p class="font-semibold">Auftragsinformationen:</p>
                  <p>Mitarbeiter ID: ${order.employeeId || 'N/A'}</p>
                  <p>Dienstleistung: ${order.serviceType}</p>
                  <p>Priorität: ${order.priority}</p>
                  <p>Erstellungsdatum: ${new Date(order.creationDate).toLocaleDateString()}</p>
                  <p>Auftragsstart: ${new Date(order.startDate).toLocaleDateString()}</p>
                  <p>Abholdatum: ${new Date(order.completionDate).toLocaleDateString()}</p>
                  <p>Bemerkungen: ${order.comments || 'Keine'}</p>
                </div>
            `;
            ordersContainer.appendChild(orderCard);
        });
    }
    

    // Fetch Orders
    async function fetchOrders() {
        try {
            const response = await fetch("https://localhost:7214/api/service", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });
    
            if (!response.ok) {
                throw new Error(`HTTP-Fehler: ${response.status}`);
            }
    
            const orders = await response.json();
    
            // displayOrders wird nur aufgerufen, wenn das Array nicht leer ist
            if (orders.length > 0) {
                console.log(orders)
                displayOrders(orders);
            } else {
                alert('Keine Serviceaufträge vorhanden.');
            }
        } catch (error) {
            console.error('Fehler beim Abrufen der Aufträge:', error);
        }
    }
    

    fetchOrders();
});
