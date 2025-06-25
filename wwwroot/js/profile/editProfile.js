function loadAndOpenModal() {
    // Only load once
    if (document.getElementById("profileModal")) {
        openModal();
        return;
    }

    fetch('/Profile/LoadEditModal')
        .then(res => res.text())
        .then(html => {
            document.getElementById("modalContainer").innerHTML = html;
            openModal();
        });
}

function openModal() {
    document.getElementById("profileModal").style.display = "flex";
    document.body.classList.add("modal-open");

    // Optional: scroll to top if needed
    window.scrollTo(0, 0);
}

function closeModal() {
    document.getElementById("profileModal").style.display = "none";
    document.body.classList.remove("modal-open");
}

function closeOnOutsideClick(event) {
    if (event.target.id === "profileModal") {
        closeModal();
    }
}