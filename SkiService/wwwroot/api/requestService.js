document.addEventListener('DOMContentLoaded', (event) => {
    document.getElementById('submitForm').addEventListener('submit', function(e) {
        e.preventDefault();

        function convertToISO(dateStr) {
            const parts = dateStr.split('/');
            return new Date(parts[2], parts[1] - 1, parts[0]).toISOString();
        }
    
        let formData = {
            CustomerName: document.getElementById('firstname').value + ' ' + document.getElementById('lastname').value,
            CustomerEmail: document.getElementById('email').value,
            CustomerPhone: document.getElementById('phone').value,
            ServiceType: convertServiceType(document.getElementById('serviceDropdown').value),
            Priority: document.querySelector('input[name="list-radio"]:checked').value,
            Status: "Offen",
            startDate: convertToISO(document.getElementById('startDate').value),
            endDate: convertToISO(document.getElementById('endDate').value),
        };
    
        // Create POST Request
        let requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        };
    
        // FETCH
        fetch('https://localhost:7214/api/service', requestOptions)
            .then(response => response.json())
            .then((json) => {
                console.log(JSON.stringify(json));
                window.location.href = "formconfirm.html";
              })
              .catch((error) => {
                console.log("Es gab einen Fehler", error);
                window.location.href = "formerror.html";
              });
    });
});

function convertServiceType(serviceDropdownValue) {
    switch (serviceDropdownValue) {
        case 'kleinerService':
            return 'Kleiner Service';
        case 'grosserService':
            return 'Grosser Service';
        case 'rennskiService':
            return 'Rennski Service';
        case 'bindung':
            return 'Bindung montieren und einstellen';
        case 'fell':
            return 'Fell zuschneiden pro Paar';
        case 'heisswachsen':
            return 'Heisswachsen';
        default:
            return '';
    }
}