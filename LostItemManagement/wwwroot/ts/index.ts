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

        const response = await fetch("/api/index/select", {
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
        const results = await response.json();

        // テーブルを初期化
        initTable();
        const tableBody = document.querySelector("table tbody");

        // 検索結果が0件の場合、メッセージを表示
        if (results.length === 0) {
            const message = document.createElement("p");
            message.textContent = "検索結果がありません";
            message.className = "text-center text-danger";
            tableBody?.appendChild(message);
            return;
        }
        else {
            createTable(results);
        }
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
        var selectedItems = getCheckLostList();
        console.log("Selected items:", selectedItems);
    });
});

// チェックボックスで選択された紛失物のリストを取得する関数
function getCheckLostList() {
    const checkedIds = Array.from(document.querySelectorAll("input[type='checkbox']:checked"))
        .map((checkbox) => parseInt((checkbox as HTMLInputElement).value));

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

function createTable(data: Lost[]) {
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
            cell.textContent = item[key as keyof Lost] as string;
            row.appendChild(cell);
        });
        tableBody.appendChild(row);
    });
}