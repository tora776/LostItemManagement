var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("loginForm");
    form === null || form === void 0 ? void 0 : form.addEventListener("submit", (e) => __awaiter(this, void 0, void 0, function* () {
        e.preventDefault();
        const userId = document.getElementById("userId").value;
        const password = document.getElementById("password").value;
        const response = yield fetch("/login/authenticate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ userId, password })
        });
        if (response.ok) {
            const data = yield response.json();
            localStorage.setItem("authToken", data.token);
            window.location.href = "/Home/Index";
        }
        else {
            document.getElementById("loginError").style.display = "block";
        }
    }));
});
// 30分経過後に自動ログアウト
setInterval(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
        fetch("/login/checktoken", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ token })
        }).then((res) => __awaiter(this, void 0, void 0, function* () {
            if (!res.ok) {
                localStorage.removeItem("authToken");
                window.location.href = "/Home/login";
            }
        }));
    }
}, 5 * 60 * 1000);
//# sourceMappingURL=login.js.map