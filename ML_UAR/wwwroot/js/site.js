function ddlSelect(ddlid,ddlval) {
    $(ddlid).val(ddlval);
    $(ddlid).select2({ theme: 'bootstrap4' }).trigger('change');
}

function datePicker(date) {
    var date_inputfrom = date;
    var containerfrom = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    var optionsfrom = {
        format: 'dd/mm/yyyy',
        container: containerfrom,
        todayHighlight: true,
        autoclose: true,
    };
    return date_inputfrom.datepicker(optionsfrom);
}

function fnAlertSuccess(message,page = "") {

    Swal.fire({
        icon: 'success',
        title: message,
        position: 'top-end',
        toast: true,
        showConfirmButton: false,
        timer: 2000
    })
    setTimeout(function () {
        if (page == "" || page == null) {
            location.reload();
        }
        else {
            window.location.href = page;
        }
    }, 1500);
}

function fnAlertError(message, page = "") {

    Swal.fire({
        icon: 'error',
        title: message,
        position: 'top-end',
        toast: true,
        showConfirmButton: false,
        timer: 2000
    })
    setTimeout(function () {
        if (page == "" || page == null) {
            location.reload();
        }
        else {
            window.location.href = page;
        }
    }, 1500);
}

function span_datatable(color,icon,message) {
    return '<small class="badge badge-' + color + '"><i class="' + icon +'"></i> ' + message +'</small>';
}
function href_datatable(color, icon, message,link) {
    return '<a class="btn btn-sm mr-3 btn-outline-' + color + '" href="' + link + '"><i class="' + icon + '"></i> ' + message +'</a>';
}