var tablePersons;

$(function () {
    tablePersons = $("#tablePersons").DataTable({
        "serverSide": true,
        "ajaxSource": "/Person/SearchPersons",
        "processing": true,
        "columns":
            [
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
            ]
    });
});