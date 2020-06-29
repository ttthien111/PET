$(function () {
    $("#tabs").tabs();
});



document.addEventListener('DOMContentLoaded', function () {
    var sourceName = document.getElementById('sourceName');
    petArr = ["catndog.png", "catndog2.png", "catndog3.png", "catndog4.png", "hamster.png", "hamster2.png", "rabbit.png"]

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    luckyNumber = getRandomInt(0, 6)

    document.getElementById("catndog").src = "icons/" + petArr[luckyNumber]
    // to disable button in popup
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        //sourceName.value = tabs[0].title;
        if (tabs[0].url.includes("https://petshopecommerce.azurewebsites.net/Admin/Bills/ApprovedBills") ||
            tabs[0].url.includes("https://petshopecommerce.azurewebsites.net/Admin/Bills/WaitingBills") ||
            tabs[0].url.includes("https://petshopecommerce.azurewebsites.net/Admin/Bills") ||
            tabs[0].url.includes("https://localhost:44337/Admin/Bills/WaitingBills")
        ) {
            document.getElementById("capture").disabled = false;
            document.getElementById("capture").style.backgroundColor = 'rgb(77, 148, 255)';
            document.getElementById("pawImage").src = "icons/denyPaw.png"
        }
        else {
            document.getElementById("capture").disabled = true;
            document.getElementById("capture").style.backgroundColor = 'rgb(209, 209, 224)';
            document.getElementById("pawImage").src = "icons/acceptPaw.png"
        }
    });

    var btnCapture = document.getElementById('capture');
    // onClick's logic below:
    btnCapture.addEventListener('click', function () {
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            if (tabs.length == 0) {
                console.log("could not send mesage to current tab");
            }
            else {
                chrome.tabs.sendMessage(tabs[0].id, { value: 1 }, function () {
                })
            }
        });
    });
});


