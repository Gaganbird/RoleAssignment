app.controller('setting_users', function ($scope, $attrs, $http, CommonSvc, SweetAlert) {
    var common = CommonSvc.getData($scope);

    $scope.onInit = function () {
        $('#setting_users0grid0 thead th').each(function (e) {
            if (e == 1)
                $(this).append('<br /><input style="margin: 8px;" onclick="Click_Role(\'Delta\')" type="checkbox" id="Delta" />');
            else if (e == 2)
                $(this).append('<br /><input style="margin: 8px;" onclick="Click_Role(\'NMR\')" type="checkbox" id="NMR" />');
            else if (e == 3)
                $(this).append('<br /><input style="margin: 8px;" onclick="Click_Role(\'Waiting\')" type="checkbox" id="Waiting" />');
            else if (e == 4)
                $(this).append('<br /><input style="margin: 8px; visibility: hidden;" type="checkbox" />');
        });
    };

    Click_Role = function (role) {
        var isChecked = $('#' + role).is(':checked');
        var arr = $scope.ui.data.AllUsers.Options;
        arr.forEach((element, index) => {
            if (role == 'Waiting') {
                if (isChecked)
                    arr[index][role] = 'True';
                else
                    arr[index][role] = 'False';
            }
            else
                arr[index][role] = isChecked;
        });
        $scope.$apply();
    };

    $scope.Click_Update = function () {
        common.webApi.post('user/save', '', $scope.ui.data.AllUsers.Options).then(function (Response) {
            if (Response.data.IsSuccess) {
                window.parent.ShowNotification('Role Assignment', '[L:UserUpdatedSuccess]', 'success');
                $scope.Pipe_AllUserPagging();
                $('#Delta, #NMR, #Waiting').prop("checked", false);
            }
        });
    };

    $scope.Pipe_AllUserPagging = function (tableState) {
        //Pipe_(event) is a Method of Smart table(Grid) and using for Pagging
        var SearchKeys = {};

        if (tableState != null && tableState != 'undefiend' && tableState != '') {
            tableState.pagination.numberOfPages = 0;
            $scope.allUserPagginationData = tableState;
        }
        if (tableState != null && tableState != 'undefiend' && tableState != '') {
            SearchKeys.skip = tableState.pagination.start,
                SearchKeys.pagesize = tableState.pagination.number;
        }
        else {
            SearchKeys.skip = $scope.allUserPagginationData.pagination.start,
                SearchKeys.pagesize = $scope.allUserPagginationData.pagination.number;
        }
        SearchKeys.pagesize = parseInt($('#setting_users0grid0').attr('pagesize'));

        common.webApi.get('user/getusers', 'pageindex=' + SearchKeys.skip / SearchKeys.pagesize + '&pagesize=' + SearchKeys.pagesize).then(function (data) {
            if (data.data != null && data.data.Data != null && data.data.IsSuccess && !data.data.HasErrors) {
                if (tableState != null && tableState != 'undefiend' && tableState != '') {
                    tableState.pagination.numberOfPages = Math.ceil(data.data.Data.TotalResults / SearchKeys.pagesize);
                }
                else {
                    $scope.allUserPagginationData.pagination.numberOfPages = Math.ceil(data.data.Data.TotalResults / SearchKeys.pagesize);
                    $scope.allUserPagginationData.pagination.start = 0;
                }
                $scope.ui.data.AllUsers.Options = data.data.Data.Results;
            }
        });
    };
});