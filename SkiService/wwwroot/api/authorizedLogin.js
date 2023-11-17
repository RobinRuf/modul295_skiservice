document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", async (e) => {
            e.preventDefault();

            const username = document.getElementById("username").value;
            const password = document.getElementById("password").value;

            try {
                const response = await fetch("https://localhost:7214/api/auth/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ username, password })
                });

                if (response.ok) {
                    const token = await response.text();
                    localStorage.setItem("skiserviceToken", token);
                    window.location.href = "orders.html";
                } else if (response.status === 401) {
                    alert("Die Einloggdaten sind nicht korrekt.");
                } else {
                    throw new Error("Ein Fehler ist aufgetreten beim Einloggen.");
                }                
            } catch (error) {
                console.error("Fehler:", error);
            }
        });
    }
});
