var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
// 検索ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    var _a;
    (_a = document.getElementById("search-button")) === null || _a === void 0 ? void 0 : _a.addEventListener("click", () => __awaiter(this, void 0, void 0, function* () {
        const lostItem = document.getElementById("lostItem").value;
        const lostPlace = document.getElementById("lostPlace").value;
        const lostDetailedPlace = document.getElementById("lostDetail").value;
        const data = {
            userId: 1,
            lostFlag: 0,
            lostDate: new Date().toISOString(),
            foundDate: null,
            lostItem: lostItem,
            lostPlace: lostPlace,
            lostDetailedPlace: lostDetailedPlace,
        };
        yield fetch("/api/index/select", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });
        alert("紛失データを検索しました!");
        location.reload();
    }));
});
// 追加ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    var _a;
    (_a = document.getElementById("insert-button")) === null || _a === void 0 ? void 0 : _a.addEventListener("click", () => __awaiter(this, void 0, void 0, function* () {
        const lostItem = document.getElementById("lostItem").value;
        const lostPlace = document.getElementById("lostPlace").value;
        const lostDetailedPlace = document.getElementById("lostDetail").value;
        // lostItem, lostPlace, lostDetailedPlaceが空でないことを確認
        if (!lostItem || !lostPlace || !lostDetailedPlace) {
            alert("全ての項目を入力してください。");
            return;
        }
        const data = {
            userId: 1,
            lostFlag: 0,
            lostDate: new Date().toISOString(),
            foundDate: null,
            lostItem: lostItem,
            lostPlace: lostPlace,
            lostDetailedPlace: lostDetailedPlace,
        };
        yield fetch("/api/index/insert", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
            .then(response => {
            if (!response.ok) {
                throw new Error("Failed to insert item");
            }
            return response.json();
        })
            .then(data => {
            console.log("Insert successful:", data);
        })
            .catch(error => {
            console.error("Error:", error);
        });
        location.reload();
    }));
});
let lostList = []; // 起動時に取得したデータを保持
document.addEventListener("DOMContentLoaded", () => __awaiter(this, void 0, void 0, function* () {
    var _a;
    // 起動時にデータを取得
    const response = yield fetch("/api/index/select", { method: "POST" });
    lostList = yield response.json();
    (_a = document.getElementById("update-button")) === null || _a === void 0 ? void 0 : _a.addEventListener("click", () => {
        const checkedIds = Array.from(document.querySelectorAll("input[type='checkbox']:checked"))
            .map((checkbox) => parseInt(checkbox.value));
        const selectedItems = lostList.filter((item) => checkedIds.includes(item.lostId));
        console.log("Selected items:", selectedItems);
    });
}));
//# sourceMappingURL=index.js.map