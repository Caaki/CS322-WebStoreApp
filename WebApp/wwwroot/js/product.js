$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/product/getall'},
        "columns": [
            { data: 'title', "width": "20%" },
            { data: 'author', "width": "'15%" },
            { data: 'category.name', "width": "15%" },
            { data: 'price', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group text-center" role="group">
                        <a href="/admin/product/upsert?id=${data}" asp-route-id="@obj.Id"
                    class="btn btn-primary rounded-5 pr-1 pl-1 pt-2 pb-1" style="margin-right:10px" >
                        <i class="bi bi-pencil-square"></i>
                                </a >
                           <a onClick=Delete("/admin/product/delete?id=${data}")
                                class="btn btn-outline-danger rounded-5 pr-1 pl-1 pt-2 pb-1">
                                <i class="bi bi-trash"></i>
                            </a>
                       `
                }, "width": "15%"
 
            }
        ]
    });

}


function Delete(url) {

    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.warning(data.message);
                }
            })
        }
    });


}

