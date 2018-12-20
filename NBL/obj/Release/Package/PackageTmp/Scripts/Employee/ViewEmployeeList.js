
$.ajax({
    url: '/editor/employee/GetAllEmployee',
    type: "GET",
    datatype: "JSON",
    success: function (data) {
        $('#table_employee_list').dataTable({
            data: data,
            responsive: true,
            "order": [[ 0, "desc" ]],
            columns: [
                { 'data': 'EmployeeNo' },
                { 'data': 'EmployeeName' },
                { 'data': 'Department.DepartmentName' },
                { 'data': 'Designation.DesignationName' },
                {
                    'data': 'PresentAddress'
                    
                },
                {
                    'data': 'Phone'

                },
                {
                    'data': 'Email'

                },
                {
                    'data': 'JoiningDate',
                    className: "text-center",
                    'render': function (jsonDate) {
                        var date = new Date(parseInt(jsonDate.substr(6)));
                        var month = date.getMonth() + 1;
                        return date.getDate() + "-" + month + "-" + date.getFullYear();
                    }
                }
            ]
        });
    }
});

