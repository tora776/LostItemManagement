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
        const response = yield fetch("/api/index/select", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });
        if (!response.ok) {
            console.error("Failed to fetch data");
            return;
        }
        // 取得したデータをJSON形式でパース
        const results = yield response.json();
        // テーブルを初期化
        initTable();
        const tableBody = document.querySelector("table tbody");
        // 検索結果が0件の場合、メッセージを表示
        if (results.length === 0) {
            const message = document.createElement("p");
            message.textContent = "検索結果がありません";
            message.className = "text-center text-danger";
            tableBody === null || tableBody === void 0 ? void 0 : tableBody.appendChild(message);
            return;
        }
        else {
            createTable(results);
        }
        alert("紛失データを検索しました!");
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
        var selectedItems = getCheckLostList();
        console.log("Selected items:", selectedItems);
    });
}));
// チェックボックスで選択された紛失物のリストを取得する関数
function getCheckLostList() {
    const checkedIds = Array.from(document.querySelectorAll("input[type='checkbox']:checked"))
        .map((checkbox) => parseInt(checkbox.value));
    const selectedItems = lostList.filter((item) => checkedIds.includes(item.lostId));
    return selectedItems;
}
// テーブルを初期化し、新規で取得したデータを表示する関数
function initTable() {
    // テーブルを初期化
    const tableBody = document.querySelector("table tbody");
    if (tableBody) {
        tableBody.innerHTML = ""; // テーブルの内容をクリア
    }
}
function createTable(data) {
    // テーブルを初期化
    const tableBody = document.querySelector("table tbody");
    // データをテーブルに追加
    data.forEach((item) => {
        const row = document.createElement("tr");
        const checkboxCell = document.createElement("td");
        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.value = item.lostId.toString();
        checkboxCell.appendChild(checkbox);
        row.appendChild(checkboxCell);
        Object.keys(item).forEach((key) => {
            const cell = document.createElement("td");
            cell.textContent = item[key];
            row.appendChild(cell);
        });
        tableBody.appendChild(row);
    });
}
//# sourceMappingURL=index.js.map