document.getElementById("notificationButton").addEventListener("click", function () {
    const popup = document.getElementById("notificationPopup");

    if (popup.style.display === "none") {
        fetch("/Notification/GetNotifications")
            .then(response => response.text())
            .then(html => {
                popup.innerHTML = html;
                popup.style.display = "block";
            });
    } else {
        popup.style.display = "none";
    }
    
});

/*
sort controller/views using Areas
make partial view
make js file
make controller
make notification model
add to appdbcontext
migrate/database update


 

*/