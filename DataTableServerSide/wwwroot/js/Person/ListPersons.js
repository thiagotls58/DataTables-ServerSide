var tablePersons;

$(function () {
    tablePersons = $("#tablePersons").DataTable({

        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],

        // Inserindo checkbox na tabela
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                }
            },
            {
                'targets': 4,
                'render': function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            }
        ],

        'select': {
            'style': 'multi'
        },

        // ordenando pela primeira coluna
        'order': [[1, 'asc']],

        // Configurations Server Side
        "serverSide": true,
        "ajaxSource": "/Person/SearchPersons",
        "processing": true,
        "columns": [
            {
                "data": "PersonID"
            },
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
});

// Personalizacao da caixa de pesquisa do DataTable
$(document).on("preInit.dt", function () {
    var $sb = $(".dataTables_filter input[type='search']");
    // remove current handler
    $sb.off();

    // Pesquisa quando aperta o 'Enter'
    $sb.on("keypress", function (evtObj) {
        if (evtObj.keyCode == 13) {
            $('#tablePersons').DataTable().search($sb.val()).draw();
        }
    });
    // Pesquisa quando aperta o botão
    var btn = $("<button type='button'>Go</button>");
    $sb.after(btn);
    btn.on("click", function (evtObj) {
        $('#tablePersons').DataTable().search($sb.val()).draw();
    });
});

function getRowsSelecteds() {

    var selectedIds = tablePersons.columns().checkboxes.selected()[0];
    //console.log(selectedIds);
    var rows = [];

    selectedIds.forEach(function (id) {

        for (var i = 0; i < tablePersons.data().length; i++) {
            if(tablePersons.data()[i].PersonID == id) {
                rows.push(tablePersons.data()[i]);
            }
        }
    });
    alert('Veja os dados no console do browser!');
    console.log(rows);
    
}