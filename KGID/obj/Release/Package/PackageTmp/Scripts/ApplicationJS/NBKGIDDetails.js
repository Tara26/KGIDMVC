var tablePremium;
$(document).ready(function () {
    if ($("#hdnSentBackAppliaction").val() == 1) {
        $("#txtAmount").prop("readonly", true);
    } else {
        $("#txtAmount").prop("readonly", false);
    }
    $(".Num").keyup(function () {
        this.value = this.value.replace(/[^0-9]/g, '');        
    });
    $("#spnPayscale").text($("#txtPayscleCode").val());
    if ($.fn.dataTable.isDataTable('#tblKgidPremium')) {
        tablePremium = $('#tblKgidPremium').DataTable();
    }
    else {
        tablePremium = $("#tblKgidPremium").DataTable({
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };

                // Total over all pages
                total = api
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                // Total over this page
                pageTotal = api
                    .column(2, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                // Update footer
                $(api.column(2).footer()).html(
                    parseFloat(pageTotal)
                );
            }
        });
    }

    if ($("#tblKgidPremium TBODY TR").find("td").hasClass("dataTables_empty")) {
        return true;
    } else if ($("#tblKgidPremium TBODY TR").length === 0) {
        return true;
    } else {
        $("#txtAmount").val("0");
        return true;
    }
});

