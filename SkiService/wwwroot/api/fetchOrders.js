document.addEventListener("DOMContentLoaded", () => {
    const ordersContainer = document.getElementById("ordersContainer");
    const priorityFilterDropdown = document.getElementById("priorityFilter");

    // Funktion zum Generieren der Auftragskarten
    function displayOrders(orders) {
        orders.forEach((order) => {
            const orderCard = document.createElement("div");
            orderCard.className =
                "max-w-sm p-4 bg-white border border-gray-200 rounded-lg shadow-lg text-gray-900 dark:bg-gray-800 dark:border-gray-700 dark:text-white mb-4 mr-4";
            
            // Bestimmen Sie den Text für den Statusänderungsbutton
            let statusButtonText = '';
            if (order.status === 'Offen') {
                statusButtonText = 'In Arbeit';
            } else if (order.status === 'In Arbeit') {
                statusButtonText = 'Abgeschlossen';
            }

            let showStatusButton = order.status !== 'Abgeschlossen' && order.status !== 'Gelöscht';
            let statusButtonHtml = showStatusButton ? `<button class="statusButton mt-4 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700" data-order-id="${order.orderID}" data-order-status="${order.status}">${'Status ändern: ' + statusButtonText}</button>` : '';
    
            orderCard.innerHTML = `
                <div class="flex flex-col">
                  <h3 class="text-xl font-semibold mb-2">Auftrag ${order.orderID}</h3>
                  <h4 class="text-lg mb-2"><span class="font-bold">Status:</span> ${order.status}</h4>
                  <p class="font-bold">Kundeninformationen:</p>
                  <p>Kunde: ${order.customerName}</p>
                  <p>E-Mail: ${order.customerEmail}</p>
                  <p>Telefon: ${order.customerPhone}</p>
                  <p>-----------------------------------</p>
                  <p class="font-bold">Auftragsinformationen:</p>
                  <p>Mitarbeiter: ${order.employeeName || 'N/A'}</p>
                  <p>Dienstleistung: ${order.serviceType}</p>
                  <p>Priorität: ${order.priority}</p>
                  <p>Erstellungsdatum: ${new Date(order.creationDate).toLocaleDateString()}</p>
                  <p>Auftragsstart: ${new Date(order.startDate).toLocaleDateString()}</p>
                  <p>Abholdatum: ${new Date(order.completionDate).toLocaleDateString()}</p>
                  <p>Preis: ${order.sum}</p>
                  <p>Bemerkungen: ${order.comment || 'Kein Kommentar'}</p>

                  ${statusButtonHtml}
                  <div class="flex mt-2 space-x-3 w-full justify-center">
                    <button class="deleteButton text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700" data-order-id="${order.orderID}">Löschen</button>
                    <button class="assignButton text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700" data-order-id="${order.orderID}">Zuweisen</button>
                  </div>
                </div>
            `;
            ordersContainer.appendChild(orderCard);
        });
        document.querySelectorAll('.statusButton').forEach(button => {
            button.addEventListener('click', () => {
                const orderId = button.getAttribute('data-order-id');
                const orderStatus = button.getAttribute('data-order-status');
                updateState(orderId, orderStatus);
            });
        });

        document.querySelectorAll('.deleteButton').forEach(button => {
            button.addEventListener('click', () => {
              const orderId = button.getAttribute('data-order-id');
              deleteOrder(orderId);
            });
          });

          document.querySelectorAll('.assignButton').forEach(button => {
            button.addEventListener('click', () => {
                const orderId = button.getAttribute('data-order-id');
                setEmployee(orderId);
            });
        });
    }
    
    

    // Fetch Orders
    window.fetchOrders = async function fetchOrders() {
        let url = "https://localhost:7214/api/service";
        const selectedPriority = priorityFilterDropdown.value;

        // Hinzufügen des Filters zur URL, falls nicht "Alle" ausgewählt ist
        if (selectedPriority !== "Alle") {
            url += `/priority/${selectedPriority}`;
        }

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

            // clear the container before reloading orders
            const ordersContainer = document.getElementById("ordersContainer");
            ordersContainer.innerHTML = '';
    
            // displayOrders wird nur aufgerufen, wenn das Array nicht leer ist
            if (orders.length > 0) {
                console.log(orders)
                displayOrders(orders);
            } else {
                alert('Keine Serviceaufträge vorhanden oder keine mit der ausgewählten Priorität.');
            }
        } catch (error) {
            console.error('Fehler beim Abrufen der Aufträge:', error);
        }
    }
    

    fetchOrders();
});
