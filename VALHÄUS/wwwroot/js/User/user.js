let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        autoWidth: false,
        ajax: {
            url: '/admin/user/getall',
            dataSrc: function (json) {
                // Update user count
                if (json.data) {
                    $('#userCount').text(json.data.length);
                }
                return json.data;
            }
        },
        columns: [
            { data: "name", width: "16%" },
            { data: "email", width: "20%" },
            { data: "phoneNumber", width: "12%" },
            { data: "company.name", width: "15%" },
            { data: "role", width: "12%" },
            {
                data: null,
                render: function (data) {
                    let today = new Date().getTime();
                    let lockout = data.lockoutEnd
                        ? new Date(data.lockoutEnd).getTime()
                        : 0;

                    // Determine lock/unlock button
                    let lockButton = lockout > today
                        ? `<a onclick="LockUnlock('${data.id}')" class="btn-user-action btn-user-lock">
                                <i class="bi bi-lock-fill"></i> Lock
                           </a>`
                        : `<a onclick="LockUnlock('${data.id}')" class="btn-user-action btn-user-unlock">
                                <i class="bi bi-unlock-fill"></i> Unlock
                           </a>`;

                    return `
                        <div class="user-action-btns">
                            ${lockButton}
                            <a href="/admin/user/RoleManagment?userId=${data.id}" 
                               class="btn-user-action btn-user-permission">
                                <i class="bi bi-pencil-square"></i> Permissions
                            </a>
                            <a onclick="DeleteUser('${data.id}')" 
                               class="btn-user-action btn-user-delete">
                                <i class="bi bi-trash-fill"></i> Remove
                            </a>
                        </div>`;
                },
                width: "25%",
                orderable: false,
                className: "actions-col"
            }
        ],
        language: {
            emptyTable: "No users found",
            search: "Search:",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ users",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        },
        pageLength: 10,
        order: [[0, 'asc']]
    });
}

function LockUnlock(userId) {
    $.ajax({
        type: "POST",
        url: "/admin/user/lockunlock",
        data: { id: userId },
        success: function (response) {
            if (response.success) {
                dataTable.ajax.reload(null, false);
            } else {
                alert(response.message || "Action failed");
            }
        },
        error: function () {
            alert("Error while processing request");
        }
    });
}

function DeleteUser(userId) {
    if (confirm("Are you sure you want to remove this user? This action cannot be undone.")) {
        $.ajax({
            type: "POST",
            url: "/admin/user/delete",
            data: { id: userId },
            success: function (response) {
                if (response.success) {
                    dataTable.ajax.reload(null, false);
                } else {
                    alert(response.message || "Failed to delete user");
                }
            },
            error: function () {
                alert("Error while deleting user");
            }
        });
    }
}