var tablePersons;

$(function () {
    tablePersons = $("#tablePersons").DataTable({

        // Configurations Server Side
        "serverSide": true,
        "ajaxSource": "/Person/SearchPersons",
        "processing": true,
        "columns": [
            {
                "name": "PersonID",
                "data": "PersonID"
            },
            {
                "name": "Name",
                "data": "Name"
            },
            {
                "name": "Email",
                "data": "Email"
            },
            {
                "name": "DateOfBirth",
                "data": "DateOfBirth"
            },
            {
                "name": "PhoneNumber",
                "data": "PhoneNumber"
            },
            {
                "name": "City",
                "data": "City"
            }
        ],

        // desabilitando recursos
        "paging": true,
        "ordering": true,
        "info": true,

        // Delay pesquisa
        'searchDelay': 1000,

        // responsive
        "responsive": true,

        // traducao portugues brasil
        'language': {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            },
            "select": {
                "rows": {
                    "_": "Selecionado %d linhas",
                    "0": "Nenhuma linha selecionada",
                    "1": "Selecionado 1 linha"
                }
            }
        }
    });

    new $.fn.dataTable.FixedHeader(tablePersons);
});