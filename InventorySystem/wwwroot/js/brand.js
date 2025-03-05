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
            "url": "/Admin/Brand/RetrieveAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "description", "width": "40%" },
            {
                "data": "state",
                "render": function (data) {
                    if (data == true) {
                        return "Active";
                    } else {
                        return "Inactive";
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Brand/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Brand/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
        title: "Are you sure to delete the Brand?",
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