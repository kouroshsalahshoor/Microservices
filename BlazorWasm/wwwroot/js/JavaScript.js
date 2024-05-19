window.ShowToastr = (type, message) => {

    toastr.options.toastClass = "toastr";

    if (type === "success") {
        toastr.success(message, "Opration Successful", { timeOut: 3000 });
    }
    if (type === "error") {
        toastr.error(message, "Opration Failed", { timeOut: 3000 });
    }
};

window.ShowSweetAlert2 = (type, message) => {
    if (type === "success") {
        Swal.fire({
            title: "Opration Successful!",
            text: message,
            icon: "success"
        });
    }
    if (type === "error") {
        Swal.fire({
            title: "Opration Failed",
            text: message,
            icon: "error"
        });
    }
};