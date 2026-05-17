var dataTable;
var cancelUrl = '';

$(document).ready(function () {
    loadDataTable();

    $('#confirmCancelBtn').on('click', function () {
        window.location.href = cancelUrl;
    });
});

function loadDataTable() {
    dataTable = $('#tblMyBookings').DataTable({
        "ajax": {
            url: '/booking/getall',
            "dataSrc": function (json) {
                // exclude Pending — tranzitoriu, nu relevant pentru client
                return json.data.filter(function (b) {
                    return b.status !== 'Pending';
                });
            }
        },
        "columns": [
            { data: 'villa.name', "width": "20%" },
            { data: 'checkInDate', "width": "12%" },
            { data: 'checkOutDate', "width": "12%" },
            { data: 'nights', "width": "7%" },
            { data: 'numberOfGuests', "width": "7%" },
            {
                data: 'status',
                "width": "12%",
                "render": function (data) {
                    var badgeClass = 'secondary';
                    if (data === 'Approved')  badgeClass = 'success';
                    else if (data === 'CheckedIn') badgeClass = 'info text-dark';
                    else if (data === 'Completed') badgeClass = 'secondary';
                    else if (data === 'Cancelled') badgeClass = 'danger';
                    else if (data === 'Refunded')  badgeClass = 'warning text-dark';
                    return `<span class="badge bg-${badgeClass}">${data}</span>`;
                }
            },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',', '.', 2), "width": "10%" },
            {
                data: null,
                "width": "15%",
                "render": function (data) {
                    const checkIn = new Date(data.checkInDate);
                    const today = new Date();
                    today.setHours(0, 0, 0, 0);
                    const diffDays = Math.floor((checkIn - today) / (1000 * 60 * 60 * 24));
                    const canCancel = data.status === 'Approved' && diffDays > 0;

                    let cancelBtn = '';
                    if (canCancel) {
                        let refundLabel = diffDays > 7 ? '100% refund will be processed'
                                        : diffDays >= 3 ? '50% refund will be processed'
                                        : 'No refund applies (less than 3 days before check-in)';
                        let refundClass = diffDays > 7 ? 'text-success' : diffDays >= 3 ? 'text-warning' : 'text-danger';

                        cancelBtn = `<button class="btn btn-outline-danger btn-sm mx-1"
                            onclick="showCancelModal(${data.id}, '${refundLabel}', '${refundClass}')">
                            <i class="bi bi-x-circle"></i> Cancel
                        </button>`;
                    }

                    return `<div class="d-flex">
                        <a href="/booking/bookingDetails?bookingId=${data.id}" class="btn btn-outline-warning btn-sm mx-1">
                            <i class="bi bi-eye"></i> Details
                        </a>
                        ${cancelBtn}
                    </div>`;
                }
            }
        ],
        "order": [[1, "desc"]]
    });
}
