
var datatable;

$(document).ready(function () {
    var url = window.location.search; // this prop retrieve the url 
    if (url.includes("approved")) {
        loadDataTable("RetrieveOrderList?status=approved");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("RetrieveOrderList?status=completed");
        }
        else {
            loadDataTable("RetrieveOrderList?status=all");
        }
    }

});

function loadDataTable(url) {
    datatable = $('#tblDatos').DataTable({
        //"language": {
        //    "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
        //    "zeroRecords": "Ningun Registro",
        //    "info": "Mostrar page _PAGE_ de _PAGES_",
        //    "infoEmpty": "no hay registros",
        //    "infoFiltered": "(filtered from _MAX_ total registros)",
        //    "search": "Buscar",
        //    "paginate": {
        //        "first": "Primero",
        //        "last": "Último",
        //        "next": "Siguiente",
        //        "previous": "Anterior"
        //    }
        //},
        "ajax": {
            "url": "/Admin/Order/" + url
        },
        "columns": [
            { "data": "id" },
            { "data": "clientNames" },
            { "data": "telephone" },
            { "data": "userApplication.email" },
            { "data": "orderStatus" },
            {
                "data": "totalOrder", "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Order/Detail/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-ticket-detailed"></i>
                            </a>                           
                        </div>
                        `;
                }
            }
        ]
    });
}

