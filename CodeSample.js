/*

<button id="friendBtn" class="btn btn-primary btn-sm" onclick="toggleFriendAppearance(this)">
    Add Friend
</button>

ask gpt what if i want another button to respond to onclick event of this button

<script>
    function toggleFriendAppearance(button) {
        if (button.innerText === "Add Friend") {
            button.innerText = "Cancel Request";
            button.classList.remove("btn-primary");
            button.classList.add("btn-secondary");
        } else {
            button.innerText = "Add Friend";
            button.classList.remove("btn-secondary");
            button.classList.add("btn-primary");
        }
    }
</script>






<button id="friendBtn"
        class="btn btn-primary btn-sm"
        data-user-id="@Model.ProfileOwnerId"
        data-status="notSent"
        onclick="toggleFriendRequest(this)">
    Add Friend
</button>

<script>
    async function toggleFriendRequest(button) {
        const userId = button.getAttribute("data-user-id");
        let status = button.getAttribute("data-status");

        try {
            const response = await fetch(`/Friendship/ToggleRequest/${userId}`, {
                method: 'POST'
            });

            if (!response.ok) throw new Error("Server error");

            const result = await response.json();

            if (result.status === "sent") {
                button.innerText = "Cancel Request";
                button.classList.remove("btn-primary");
                button.classList.add("btn-secondary");
                button.setAttribute("data-status", "sent");
            } else {
                button.innerText = "Add Friend";
                button.classList.remove("btn-secondary");
                button.classList.add("btn-primary");
                button.setAttribute("data-status", "notSent");
            }
        } catch (err) {
            alert("Something went wrong.");
            console.error(err);
        }
    }
</script>



*/

/*




add toggle response method to controller
use toggle respond in view
make js dropdown to let the added either confirm or delete request
add "respond" button if added by a profile

view needs to check if theres a pending friend request before showing the button
system creates/removes friend request in during friend request toggle


system creates notification for friend request in SendFriendRequest method in FriendService
system notifies receiver
receiver accepts friend request
system makes friend

make seed data for entire user model
delete migrations and reset to start fresh


add friend sevice
unfreind function


code business logic to handle the notification popup for friend requests
load the notifications from the database and display them in popup

people you may know dropdown in profile page
 
put claims to access logined current user id instead of using session

make profile service/profile controller to get info from user model not from post model

in comment service, get comments by user posts to solve n+1 problem

User Profiles
View your own profile & others’
Profile picture, username, basic info
Edit your own profile

2. Posts & Comments
Create posts
Comment on posts
View all posts on profile pages
Optional: Like/Unlike

3. Friend System
Send/cancel friend requests
Accept/reject requests
View friends list
Conditional UI: Add friend / Friends / Cancel request

4. Notifications (Friend-Related Only)
Notify when someone sends a request
Notify when a request is accepted
Optional: Mark as read / unread

5. Basic Messaging (Optional or Placeholder)
Just a placeholder chat or one-way messaging for now
Doesn’t have to be real-time
Could be: “Leave a message” button that saves messages

*/