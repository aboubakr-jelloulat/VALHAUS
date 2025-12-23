var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    // Determine which status filter to apply
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else if (url.includes("cancelled")) {
        loadDataTable("cancelled");
    }
    else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/order/getall?status=' + status,
            type: 'GET',
            dataSrc: function (json) {
                return json.data || json;
            }
        },
        "columns": [
            {
                data: 'id',
                "width": "8%",
                render: function (data) {
                    return `<span style="font-family: 'Courier New', monospace; font-weight: 700; color: #d4af37;">#${data}</span>`;
                }
            },
            {
                data: 'name',
                "width": "18%",
                render: function (data) {
                    return `<span style="font-weight: 500;">${data}</span>`;
                }
            },
            {
                data: 'phoneNumber',
                "width": "15%",
                render: function (data) {
                    return `<span style="color: #5a5a5a;">${data}</span>`;
                }
            },
            {
                data: 'applicationUser.email',
                "width": "20%",
                render: function (data) {
                    return `<span style="color: #5a5a5a; font-size: 14px;">${data}</span>`;
                }
            },
            {
                data: 'orderStatus',
                "width": "12%",
                render: function (data) {
                    if (!data) return '<span class="status-badge status-pending">Unknown</span>';

                    let statusClass = '';
                    let statusText = data;

                    switch (data.toLowerCase()) {
                        case 'pending':
                            statusClass = 'status-pending';
                            statusText = 'Payment Pending';
                            break;
                        case 'approved':
                            statusClass = 'status-approved';
                            statusText = 'Approved';
                            break;
                        case 'inprocess':
                        case 'processing':
                            statusClass = 'status-inprocess';
                            statusText = 'In Process';
                            break;
                        case 'completed':
                        case 'shipped':
                            statusClass = 'status-completed';
                            statusText = 'Completed';
                            break;
                        case 'cancelled':
                            statusClass = 'status-cancelled';
                            statusText = 'Cancelled';
                            break;
                        default:
                            statusClass = 'status-pending';
                    }

                    return `<span class="status-badge ${statusClass}">${statusText}</span>`;
                }
            },
            {
                data: 'orderTotal',
                "width": "12%",
                render: function (data) {
                    return `<span style="color: #d4af37; font-weight: 600; font-size: 15px;">$${parseFloat(data).toFixed(2)}</span>`;
                }
            },
            {
                data: 'id',
                "width": "15%",
                "render": function (data) {
                    return `
                        <div class="action-buttons">
                            <a href="/admin/order/details?orderId=${data}" class="btn-view-details" title="View Order Details">
                                <i class="bi bi-eye-fill"></i>
                                <span>Details</span>
                            </a>
                        </div>
                    `;
                }
            }
        ],
        "language": {
            "emptyTable": "No orders found",
            "info": "Showing _START_ to _END_ of _TOTAL_ orders",
            "infoEmpty": "Showing 0 to 0 of 0 orders",
            "infoFiltered": "(filtered from _MAX_ total orders)",
            "lengthMenu": "Show _MENU_ orders",
            "search": "Search:",
            "zeroRecords": "No matching orders found",
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            }
        },
        "pageLength": 10,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "order": [[0, 'desc']], // Newest orders first
        "responsive": true,
        "autoWidth": false,
        "dom": '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        "processing": true,
        "stateSave": false
    });
}