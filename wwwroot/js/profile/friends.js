function createFriendsContainer(ownerID, requestId) {
    // Container div for dropdown
    const dropdownDiv = document.createElement("div");
    dropdownDiv.className = "dropdown d-inline-block";

    // Main button
    const button = document.createElement("button");
    button.className = "btn btn-primary";
    button.setAttribute("type", "button");
    button.setAttribute("data-bs-toggle", "dropdown");
    button.setAttribute("aria-expanded", "false");
    button.innerText = "Friends";

    // Dropdown menu
    const menu = document.createElement("ul");
    menu.className = "dropdown-menu";

    // "Unfriend" item
    const unfriendItem = document.createElement("li");
    const unfriendLink = document.createElement("a");
    unfriendLink.className = "dropdown-item text-danger";
    unfriendLink.innerText = "Unfriend";
    unfriendLink.href = `/Profile/UnfriendUserProfile?Id=${ownerID}&RequestId=${requestId}`;
    // unfriendLink.onclick = () => unfriendUser(userId); // Call your function

    unfriendItem.appendChild(unfriendLink);
    menu.appendChild(unfriendItem);

    // Put it all together
    dropdownDiv.appendChild(button);
    dropdownDiv.appendChild(menu);

    return dropdownDiv;
}


document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById("friendContainer");
    if (container) {
        const btn = createFriendsContainer(container.getAttribute("data-user-id"), container.getAttribute("data-request-id"));
        container.appendChild(btn);
    }
});