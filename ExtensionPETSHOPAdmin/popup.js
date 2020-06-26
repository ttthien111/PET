$(function () {
    $("#tabs").tabs();
});


document.addEventListener('DOMContentLoaded', function () {
    var sourceName = document.getElementById('sourceName');
    // to disable button in popup
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        //sourceName.value = tabs[0].title;
        if (tabs[0].url.includes("https://petshopecommerce.azurewebsites.net/Admin/Bills/ApprovedBills")) {
            document.getElementById("capture").disabled = false;
            document.getElementById("capture").style.backgroundColor = 'rgb(78, 115, 223)';
            document.getElementById("pawImage").src= "icons/acceptPaw.png"
        }
        else {
            document.getElementById("capture").disabled = true;
            document.getElementById("capture").style.backgroundColor = 'rgb(209, 209, 224)';
            document.getElementById("pawImage").src= "icons/denyPaw.png"
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
                chrome.tabs.sendMessage(tabs[0].id, { value: 1}, function () {
                })
            }
        });
    });
});


