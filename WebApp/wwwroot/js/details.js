function Order(id,count,email) {

    Swal.fire({
        title: "Confirm your order?",
        text: "Lets make the order :)",
        icon: "info",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, make the order!"
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
