function togglePopup() {
    const popup = document.getElementById("popup");
    popup.style.display = popup.style.display === "block" ? "none" : "block";
}

async function confirmProfileRequest(ownerID, requestId) {
    
    // console.log("Responding to friend request");

    togglePopup();
    document.getElementById("respond").remove();

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;


    fetch('/Profile/RespondToRequest', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            requestId: requestId,
            action: "confirm"
        })
    })
        .then(res => {
            if (!res.ok) throw new Error("Failed to confirm request");
            return res.json(); // or .text() if you return plain text
        })
        .then(result => {
            // Handle success
            
            const newBtn = createFriendsContainer(ownerID, requestId); // from friends.js
            document.getElementById("responseDiv").appendChild(newBtn);
        })
        .catch(err => {
            console.error(err);
            alert("Something went wrong confirming the friend request.");
        });

        
}

function deleteProfileRequest() {
    alert("Friend request deleted.");
    // TODO: Add actual logic (fetch to backend)
}

// Optional: click outside to close
document.addEventListener("click", function (e) {
    if (!e.target.closest("div[style*='relative']")) {
        document.getElementById("popup").style.display = "none";
    }
});