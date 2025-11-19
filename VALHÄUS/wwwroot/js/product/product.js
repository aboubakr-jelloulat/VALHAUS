$(document).ready(function () {

    $('#tblData').DataTable({
        "ajax": {
            "url": "/Product/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "30%" },
            { "data": "sku", "width": "15%" },
            {
                "data": "price",
                "width": "15%",
                "render": function (data) {
                    if (!data) return "";
                    return new Intl.NumberFormat('en-GB', {
                        style: 'currency', currency: 'EUR'
                    }).format(data);
                }
            },
            { "data": "brand", "width": "20%" },
            {
                "data": "id",
                "width": "20%",
                "orderable": false,
                "render": function (data) {
                    return `
                        <div class="action-buttons">
                            <a href="/Product/Upsert/${data}" class="btn-action btn-edit" title="Edit">
                                <i class="bi bi-pencil"></i>
                            </a>
                            <a href="javascript:void(0)" onclick="Delete('/Product/Delete/${data}')" 
                               class="btn-action btn-delete" title="Delete">
                               <i class="bi bi-trash"></i>
                            </a>
                        </div>`;
                }
            }
        ],
        "language": {
            "emptyTable": "No products available"
        },
        "stripeClasses": []
    });
});


function Delete(url) {
    if (!confirm("Are you sure you want to delete this product?")) return;

    fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(res => res.json())
        .then(data => {
            if (data && data.success) {
                $('#tblData').DataTable().ajax.reload();
                alert(data.message || "Deleted successfully");
            } else {
                alert(data ? data.message : "Delete failed");
            }
        })
        .catch(err => {
            console.error(err);
            alert("There was an error deleting the product");
        });
}
