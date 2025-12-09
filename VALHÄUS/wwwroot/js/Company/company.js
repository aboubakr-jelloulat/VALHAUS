var dataTable;

$(document).ready(function () {
    loadDataTable();

    // Event delegation for delete buttons
    $('#tblData').on('click', '.btn-delete', function () {
        var id = $(this).data('id');
        Delete('/Admin/Company/Delete/' + id);
    });
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name", width: "25%" },
            { data: "phoneNumber", width: "20%" },
            { data: "city", width: "15%" },
            { data: "state", width: "15%" },
            {
                data: "id",
                width: "25%",
                render: function (id) {
                    return `
                        <div class="action-buttons">
                            <a href="/Admin/Company/Upsert?id=${id}"
                               class="btn-action btn-edit"
                               title="Edit">
                                <i class="bi bi-pencil"></i>
                            </a>

                            <button data-id="${id}" class="btn-action btn-delete" title="Delete">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    `;
                }
            }
        ]
    });
}

// Delete function using SweetAlert and AJAX
function Delete(url) {
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
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function () {
                    toastr.error('An error occurred while deleting the company');
                }
            });
        }
    });
}
