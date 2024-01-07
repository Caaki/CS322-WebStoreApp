function Order(id,count,email) {

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
                url: `/Customer/Home/Order?id=${id}&count=${count}&email=${email}`,
                type: 'POST', 
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.warning(data.message);
                }

            })
            }
    });


}
