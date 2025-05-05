// 検索ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("search-button")?.addEventListener("click", async () => {
        const lostItem = (document.getElementById("lostItem") as HTMLInputElement).value;
        const lostPlace = (document.getElementById("lostPlace") as HTMLInputElement).value;
        const lostDetailedPlace = (document.getElementById("lostDetail") as HTMLInputElement).value;

        const data = {
            userId: 1,
            lostFlag: 0,
            lostDate: new Date().toISOString(),
            foundDate: null,
            lostItem: lostItem,
            lostPlace: lostPlace,
            lostDetailedPlace: lostDetailedPlace,
        };

        await fetch("/api/index/select", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        alert("紛失データを検索しました!");
        location.reload();
    });
});

// 追加ボタン押下時の処理
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("insert-button")?.addEventListener("click", async () => {
        const lostItem = (document.getElementById("lostItem") as HTMLInputElement).value;
        const lostPlace = (document.getElementById("lostPlace") as HTMLInputElement).value;
        const lostDetailedPlace = (document.getElementById("lostDetail") as HTMLInputElement).value;

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

        await fetch("/api/index/insert", {
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
    });
});


let lostList: Lost[] = []; // 起動時に取得したデータを保持
interface Lost {
    lostId: number;
    userId: number;
    lostFlag: number;
    lostDate: string;
    foundDate: string | null;
    lostItem: string;
    lostPlace: string;
    lostDetailedPlace: string;
}
document.addEventListener("DOMContentLoaded", async () => {
    // 起動時にデータを取得
    const response = await fetch("/api/index/select", { method: "POST" });
    lostList = await response.json();

    document.getElementById("update-button")?.addEventListener("click", () => {
        const checkedIds = Array.from(document.querySelectorAll("input[type='checkbox']:checked"))
            .map((checkbox) => parseInt((checkbox as HTMLInputElement).value));

        const selectedItems = lostList.filter((item) => checkedIds.includes(item.lostId));
        console.log("Selected items:", selectedItems);
    });
});