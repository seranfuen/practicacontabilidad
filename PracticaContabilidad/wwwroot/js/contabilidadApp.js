﻿var app = angular.module("contabilidadApp", []);

app.factory("accountService",
    function($http) {
        var result = { Accounts: [] };

        $http.get("/api/account").then(function(response) {
            result.Accounts = response.data;
        });

        return result;
    });

app.controller("LedgerEntryController",
    function($scope, accountService) {
        // We add a new line (ledger entry, apunte contable)
        $scope.addNewLine = function() {
            $scope.Lines.push({});
        };

        $scope.AccountClass = [];
        $scope.AccountName = [];

        // Make a short account code have 9 digits exactly
        // If no dot is present, right pad with zeros
        // If present, left pad up to the digits after the dot
        $scope.fillAccount = function(index) {
            var code = $scope.Lines[index].Account;
            if (!code || code.length > 9) return;
            if (!code.match(/^[0-9]+\.?[0-9]+$/)) return;

            var rightSide = code.slice(code.indexOf(".") + 1);
            var leftSide = code.substr(0, code.indexOf("."));

            if (leftSide.length === 0) {
                leftSide = rightSide;
                rightSide = "";
            }
            $scope.Lines[index].Account = leftSide + rightSide.padStart(9 - leftSide.length, "0");
            $scope.AccountClass[index] = accountService.Accounts.some(function(account) {
                    return account.code === $scope.Lines[index].Account;
                })
                ? ""
                : "not-existing";
            var accountsMatching = accountService.Accounts.filter(function(account) {
                return account.code === $scope.Lines[index].Account;
            });

            $scope.AccountName[index] = accountsMatching.length > 0 ? accountsMatching[0].name : "";
        };

        $scope.updateTotals = function() {
            $scope.TotalsLine.DebitSum = $scope.Lines.reduce(function(total, line) {
                    return total + (parseInt(line.Debit) || 0);
                },
                0);
            $scope.TotalsLine.CreditSum = $scope.Lines.reduce(function(total, line) {
                    return total + (parseInt(line.Credit) || 0);
                },
                0);
        };

        $scope.Lines = [];

        $scope.TotalsLine = {
            DebitSum: 0,
            CreditSum: 0
        };

        $scope.addNewLine();
    });