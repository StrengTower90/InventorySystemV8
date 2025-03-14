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
            "url": "/Admin/User/RetrieveAll"
        },
        "columns": [
            { "data": "email" },
            { "data": "names" },
            { "data": "lastNames" },
            { "data": "phoneNumber" },
            { "data": "role" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd" // This object allows the column to work with many fields at the same time
                },
                "render": function (data) {
                    let today = new Date().getTime();
                    let lock = new Date(data.lockoutEnd).getTime();
                    if (lock > today) {
                        // User is locked
                        return `
                            <div class="text-center">
                                <a onclick=LockAndUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px>
                                 <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                                <a onclick=LockAndUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer", width:150px>
                                 <i class="bi bi-lock-fill"></i> Lock
                                </a>
                            </div>
                        `;
                    }
                },
            }
        ]
    });
}

function LockAndUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockAndUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                datatable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
};