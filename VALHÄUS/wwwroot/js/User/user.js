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

                    // Lock & Unlock button
                    let lockButton = lockout > today
                        ? `<a onclick="LockUnlock('${data.id}')" class="btn-user-action btn-user-lock">
                               <i class="bi bi-lock-fill"></i> Lock
                           </a>`
                        : `<a onclick="LockUnlock('${data.id}')" class="btn-user-action btn-user-unlock">
                               <i class="bi bi-unlock-fill"></i> Unlock
                           </a>`;

                    // Permissions button
                    let permissionButton = `<a href="/admin/user/RoleManagment?userId=${data.id}" 
                                                class="btn-user-action btn-user-permission">
                                                <i class="bi bi-pencil-square"></i> Permissions
                                            </a>`;

                    // Delete button 
                    let deleteButton = `<a onclick="DeleteUser('${data.id}')" 
                                           class="btn-user-action btn-user-delete">
                                           <i class="bi bi-trash-fill"></i> Remove
                                        </a>`;

                    return `<div class="user-action-btns">
                                ${lockButton}
                                ${permissionButton}
                                ${deleteButton}
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

// Lock / Unlock user
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

// Delete user function
function DeleteUser(userId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/admin/user/delete',
                type: 'POST',
                data: { id: userId },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);

                        
                        //If you have a delete button with data attribute
                        $(`button[onclick="DeleteUser(${userId})"]`).closest('tr').remove();

                    } else {
                        toastr.error(data.message || 'Failed to delete user');
                    }
                },
                error: function () {
                    toastr.error('An error occurred while deleting the user');
                }
            });
        }
    });
}