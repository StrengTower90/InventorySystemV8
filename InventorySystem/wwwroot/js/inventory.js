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
            "url": "/Inventory/Inventory/RetrieveAll"
        },
        "columns": [
            { "data": "store.name" },
            { "data": "product.description" },
            {
                "data": "product.cost", "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&, ');
                    return d;
                }
            },
            { "data": "amount", "className": 'text-end' },

        ]
    });
}
