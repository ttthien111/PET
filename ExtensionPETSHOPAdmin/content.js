//Get respond from background script and popup
chrome.runtime.onMessage.addListener(
    function (request, sender, sendResponse) {
        //Button Article
        if (request.value === 1) {
            //articleToPDF(formatFileName(request.fileName));
            chotdon();      
        }
});


function chotdon(){
    img = document.querySelector("div#billDetailContent.modal-content");
    fileName = img.querySelector("#billDetailContent > div.p-4 > div > h3");
    console.log(img)
    if (fileName.textContent!==null && fileName.textContent !==undefined){
        fileName = formatFileName(fileName.textContent);
    }
    else {
        alert("Vui lòng chọn đơn muốn chốt!!!")
        return;
    }
    html2canvas(img, {
        useCORS: true,
        scale: 1,
        allowTaint: true,
        logging: true
    })
    .then(canvas => {
        var img = canvas.toDataURL("image/png");
        saveAs(img, formatFileName(fileName));
    });

}

function saveAs(uri, filename) {
    var link = document.createElement('a');
    if (typeof link.download === 'string') {
        link.href = uri;
        link.download = filename;
        //Firefox requires the link to be in the body
        document.body.appendChild(link);
        //simulate click
        link.click();
        //remove the link when done
        document.body.removeChild(link);
    } else {
        window.open(uri);
    }
}

function formatFileName(fileName) {
    return fileName.replace("Đơn hàng #", "");
}
