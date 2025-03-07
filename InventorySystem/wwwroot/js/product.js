let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        // To change DataTable Language
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
            "url": "/Admin/Product/RetrieveAll"
        },
        "columns": [
            { "data": "serialNumber" },
            { "data": "description" },
            { "data": "category.name" },
            { "data": "brand.name" },
            {
                "data": "price", "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&, ');
                    return d;
                }
            },
            {
                "data": "state",
                "render": function (data) {
                    if (data == true) {
                        return "Active";
                    } else {
                        return "Inactive";
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                             <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure to delete the Product?",
        text: "This record won't be retrieve again",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((dele) => {
        if (dele) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}