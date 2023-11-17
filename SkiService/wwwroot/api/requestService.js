// submitForm.js
document.getElementById('submitForm').addEventListener('submit', function(e) {
    e.preventDefault();

    // Sammeln der Formulardaten
    let formData = {
        firstname: document.getElementById('firstname').value,
        lastname: document.getElementById('lastname').value,
        email: document.getElementById('email').value,
        phone: document.getElementById('phone').value,
        priority: document.querySelector('input[name="list-radio"]:checked').value,
        serviceType: document.getElementById('serviceDropdown').value,
        startDate: document.getElementById('startDate').value,
        endDate: document.getElementById('endDate').value,
    };

    // Erstellen der Request-Optionen
    let requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
    };

    // Durchführen des Fetch-Aufrufs
    fetch('https://localhost:7214/api/service', requestOptions)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
});
