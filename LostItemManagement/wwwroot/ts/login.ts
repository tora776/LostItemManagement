document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("loginForm") as HTMLFormElement;
    form?.addEventListener("submit", async (e) => {
        e.preventDefault();

        const userId = (document.getElementById("userId") as HTMLInputElement).value;
        const password = (document.getElementById("password") as HTMLInputElement).value;

        const response = await fetch("/login/authenticate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ userId, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("authToken", data.token);
            window.location.href = "/Home/Index";
        } else {
            document.getElementById("loginError")!.style.display = "block";
        }
    });
});

// 30分経過後に自動ログアウト
setInterval(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
        fetch("/login/checktoken", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ token })
        }).then(async res => {
            if (!res.ok) {
                localStorage.removeItem("authToken");
                window.location.href = "/Home/login";
            }
        });
    }
}, 5 * 60 * 1000);