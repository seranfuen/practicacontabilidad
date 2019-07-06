var app = angular.module("contabilidadApp", []);

app.factory("accountService",
    function($http) {

        var result = {
            Accounts: [],
            CreateAccount: function(code, name, description, promiseSuccess, promiseFailure) {
                $http.post("/api/account",
                    {
                        code: code,
                        name: name,
                        description: description
                    }).then(function(response) {
                        result.Accounts.push(response.data);
                        promiseSuccess();
                    },
                    function() {
                        promiseFailure(code);
                    });
            }
        };

        $http.get("/api/account").then(function(response) {
            result.Accounts = response.data;
        });

        return result;
    });

app.controller("LedgerEntryController",
    function($scope, $http, $window, accountService) {
        // We add a new line (ledger entry, apunte contable)

        $scope.addNewLine = function() {
            $scope.Lines.push({
                Debit: 0,
                Credit: 0
            });
        };

        $scope.AccountClass = [];
        $scope.AccountName = [];


        function fillAccountName(index) {

            var accountsMatching = accountService.Accounts.filter(function(account) {
                return account.code === $scope.Lines[index].Account;
            });

            $scope.AccountName[index] = accountsMatching.length > 0 ? accountsMatching[0].name : "";
        }

        function setAccountClass(index) {
            $scope.AccountClass[index] = existsAccount($scope.Lines[index].Account)
                ? ""
                : "not-existing";
        }

        function existsAccount(accountCode) {
            return accountService.Accounts.some(function(account) {
                return account.code === accountCode;
            });
        }

        $scope.onAccountSearchChanged = function() {
            $scope.searchAccounts($scope.searchTerm);
        };

        $scope.searchTerm = "";

        // Make a short account code have 9 digits exactly
        // If no dot is present, right pad with zeros
        // If present, left pad up to the digits after the dot
        // It also fills in account name information, if it exists
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

            fillAccountName(index);
            setAccountClass(index);
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

        $scope.filteredAccounts = [];

        $scope.searchAccounts = function(searchTerm) {
            if (!searchTerm) {
                $scope.filteredAccounts = [];
            } else {
                var termInLowerCase = searchTerm.toLowerCase();
                $scope.filteredAccounts = accountService.Accounts.filter(function(account) {
                    return account.code.includes(termInLowerCase) ||
                        account.name.toLowerCase().includes(termInLowerCase);
                });
            }
        };

        $scope.$watch("Lines",
            function(newValue, oldValue) {
                for (var i = 0; i < $scope.Lines.length; i++) {
                    if (newValue[i].Debit < 0) {
                        $scope.Lines[i].Debit = 0;
                    }
                    if (newValue[i].Credit < 0) {
                        $scope.Lines[i].Credit = 0;
                    }

                    // Need to check if there was previously no line at the current index
                    if (typeof oldValue[i] === "undefined") continue;

                    if (oldValue[i].Debit !== newValue[i].Debit && newValue[i].Debit !== 0) {
                        $scope.Lines[i].Credit = 0;
                    } else if (oldValue[i].Credit !== newValue[i].Credit && newValue[i].Credit !== 0) {
                        $scope.Lines[i].Debit = 0;
                    }
                }
            },
            true);

        $scope.TotalsLine = {
            DebitSum: 0,
            CreditSum: 0
        };

        $scope.HasErrors = false;
        $scope.FormErrors = [];

        $scope.validate = function() {
            $scope.HasErrors = false;
            $scope.FormErrors = [];
            for (var i = 0; i < $scope.Lines.length; i++) {

                var line = $scope.Lines[i];
                if (!existsAccount(line.Account)) {
                    $scope.HasErrors = true;
                    $scope.FormErrors.push("La cuenta " + line.Account + " no existe.");
                }

                if (line.Debit <= 0 && line.Credit <= 0) {
                    $scope.HasErrors = true;
                    $scope.FormErrors.push("La línea " +
                        (i + 1) +
                        " no tiene ningún valor ni en el debe ni en el haber.");
                }
            }
        };

        $scope.submit = function() {
            $scope.validate();

            if ($scope.HasErrors === true) return;

            $http.post("/api/JournalEntries", $scope.Lines).then(function() {
                    $window.location.href = "/journalentries";
                },
                function(response) {
                    $scope.FormErrors = [];
                    $scope.FormErrors.push("Hubo un error al procesar tu solicitud: " +
                        response.status +
                        " " +
                        response.statusText);
                });

        };

        $scope.onLastLineKeyPress = function(event) {
            if (event.keyCode !== 9 && event.keyCode !== 13)
                return;

            addNewLine();
        };

        $scope.copyRemarksFrom = function(index) {
            var remarks = $scope.Lines[index].Remarks;

            for (var i = 0; i < $scope.Lines.length; i++) {
                $scope.Lines[i].Remarks = remarks;
            }
        };

        $scope.removeLine = function(index) {
            if ($scope.Lines.length <= 1) return;
            $scope.Lines.splice(index, 1);
        };

        $scope.startNewAccountCreation = function(code) {
            $scope.NewAccount = {
                Code: code,
                Name: "",
                Description: ""
            };
        };

        $scope.commitNewAccount = function() {
            var newAccount = $scope.NewAccount;
            accountService.CreateAccount(newAccount.Code,
                newAccount.Name,
                newAccount.Description,
                function() {
                    for (var i = 0; i < $scope.Lines.length; i++)
                        $scope.fillAccount(i);
                },
                function(code) {
                    $scope.ErrorMessage = "Ha ocurrido un error al intentar guardar la cuenta " + code;
                });
        };

        $scope.addNewLine();
    });