// 更新ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("submit")?.addEventListener("click", async () => {
        const selectedItems = getUpdateLostList();

        if (selectedItems.length === 0) {
            alert("更新するデータを選択してください。");
            return;
        }

        // サーバーに選択されたデータを送信
        const response = await fetch("/api/index/update", {
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
    });
});


// update対象のデータを取得する関数
function getUpdateLostList() {
    const selectedRows = Array.from(document.querySelectorAll("tbody tr"))

    const selectedItems: Lost[] = selectedRows.map((row) => {
        const cells = row?.querySelectorAll("td");
        if (!cells) return null;

        return {
            lostId: parseInt((cells[6].querySelector("input[type='hidden']") as HTMLInputElement)?.value || "0"),
            userId: 1, // 必要に応じて修正
            lostDate: (cells[1].textContent || "").trim(),
            foundDate: (cells[2].textContent || "").trim(),
            lostItem: (cells[3].querySelector("input") as HTMLInputElement)?.value || "",
            lostPlace: (cells[4].querySelector("input") as HTMLInputElement)?.value || "",
            lostDetailedPlace: (cells[5].querySelector("input") as HTMLInputElement)?.value || "",
            lostFlag: parseInt((cells[7].querySelector("input[type='hidden']") as HTMLInputElement)?.value || "0")
        };
    }).filter((item): item is Lost => item !== null);

    return selectedItems;
}

interface Lost {
    lostId: number;
    userId: number;
    lostDate: string;
    foundDate: string | null;
    lostItem: string;
    lostPlace: string;
    lostDetailedPlace: string;
    lostFlag: number;
}