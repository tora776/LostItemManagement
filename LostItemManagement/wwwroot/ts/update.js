var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
// 更新ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    var _a;
    (_a = document.getElementById("submit")) === null || _a === void 0 ? void 0 : _a.addEventListener("click", () => __awaiter(this, void 0, void 0, function* () {
        const selectedItems = getUpdateLostList();
        if (selectedItems.length === 0) {
            alert("更新するデータを選択してください。");
            return;
        }
        // サーバーに選択されたデータを送信
        const response = yield fetch("/Home/Update/update/saveupdates", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(selectedItems),
        });
        if (response.ok) {
            window.location.href = "/Home/Index";
        }
        else {
            console.error("Failed to send data to update page");
            return;
        }
    }));
});
// update対象のデータを取得する関数
function getUpdateLostList() {
    const selectedRows = Array.from(document.querySelectorAll("tbody tr"));
    const selectedItems = selectedRows.map((row) => {
        var _a, _b, _c, _d, _e;
        const cells = row === null || row === void 0 ? void 0 : row.querySelectorAll("td");
        if (!cells)
            return null;
        return {
            lostId: parseInt(((_a = cells[6].querySelector("input[type='hidden']")) === null || _a === void 0 ? void 0 : _a.value) || "0"),
            userId: 1, // 必要に応じて修正
            lostDate: (cells[1].textContent || "").trim(),
            foundDate: (cells[2].textContent || "").trim(),
            lostItem: ((_b = cells[3].querySelector("input")) === null || _b === void 0 ? void 0 : _b.value) || "",
            lostPlace: ((_c = cells[4].querySelector("input")) === null || _c === void 0 ? void 0 : _c.value) || "",
            lostDetailedPlace: ((_d = cells[5].querySelector("input")) === null || _d === void 0 ? void 0 : _d.value) || "",
            lostFlag: parseInt(((_e = cells[7].querySelector("input[type='hidden']")) === null || _e === void 0 ? void 0 : _e.value) || "0")
        };
    }).filter((item) => item !== null);
    return selectedItems;
}
//# sourceMappingURL=update.js.map