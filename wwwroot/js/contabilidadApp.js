var app = angular.module("contabilidadApp", []);

app.controller("LedgerEntryController",
    function($scope) {
        // We add a new line (ledger entry, apunte contable)
        $scope.addNewLine = function() {
            $scope.Lines.push({
                Account: null,
                Debit: null,
                Credit: null,
                Remarks: null
            });
        };

        // for user convenience, by default show already one line
        $scope.Lines = [];
        $scope.addNewLine();
    });