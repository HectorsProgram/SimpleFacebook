/*

<div class="d-flex justify-content-between align-items-center">
    <div class="d-flex align-items-center">
        <a href="@Url.Action("View", "Profile", new { id = Model.Post.User.Id })">
            <img src="@Model.Post.User?.ProfilePicturePath" alt="Profile Picture"
                 class="rounded-circle me-2" style="width: 40px; height: 40px; object-fit: cover;">
        </a>
        <a href="@Url.Action("View", "Profile", new { id = Model.Post.User.Id })" class="text-decoration-none">
            <strong>@Model.Post.User?.Username</strong>
        </a>
    </div>
    <small class="text-muted">@Model.Post.CreatedAt.ToString("g")</small>
</div>
<p class="mb-1">@Model.Post.Content</p>

                <div class="d-flex align-items-center mb-1">
                    <img src="@comment.User?.ProfilePicturePath" alt="Profile Picture"
                         class="rounded-circle me-2" style="width: 30px; height: 30px; object-fit: cover;">
                    <strong>@comment.User?.Username</strong>
                    <small class="text-muted ms-2">@comment.CreatedAt.ToString("g")</small>
                </div>
*/

/*

friend request model
code business logic to handle the notification popup for friend requests
load the notifications from the database and display them in popup


 

*/