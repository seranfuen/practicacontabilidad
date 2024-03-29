﻿describe("LedgerEntryController",
    function() {

        beforeEach(module("contabilidadApp"));

        var $controller, $rootScope;

        beforeEach(inject(function(_$controller_, _$rootScope_) {
            $controller = _$controller_;
            $rootScope = _$rootScope_;
        }));

        describe("Lines",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller = $controller("LedgerEntryController", { $scope: $scope, accountService: {} });
                });

                it("When creating new controller, it has one entry by default with no data",
                    function() {
                        expect($scope.Lines.length).toEqual(1);
                    });

                it("Copy remarks copies the remarks of the selected line to all the other lines",
                    function() {
                        $scope.addNewLine();
                        $scope.addNewLine();
                        $scope.Lines[2].Remarks = "HELLO THERE";
                        expect($scope.Lines[0].Remarks).not.toBeDefined();
                        expect($scope.Lines[1].Remarks).not.toBeDefined();
                        $scope.copyRemarksFrom(2);
                        expect($scope.Lines[0].Remarks).toEqual("HELLO THERE");
                        expect($scope.Lines[1].Remarks).toEqual("HELLO THERE");
                    });

                it("Copy remarks from empty empties the other lines",
                    function() {
                        $scope.addNewLine();
                        $scope.addNewLine();
                        $scope.Lines[2].Remarks = "HELLO THERE";
                        expect($scope.Lines[0].Remarks).not.toBeDefined();
                        expect($scope.Lines[1].Remarks).not.toBeDefined();
                        $scope.copyRemarksFrom(1);
                        expect($scope.Lines[2].Remarks).not.toBeDefined();
                    });
            });

        describe("UpdateTotals",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller = $controller("LedgerEntryController", { $scope: $scope, accountService: {} });
                });

                it("The totals line is 0 for both credit and debit if no elements exist",
                    function() {
                        $scope.Lines = [];
                        $scope.updateTotals();
                        expect($scope.TotalsLine.CreditSum).toEqual(0);
                        expect($scope.TotalsLine.DebitSum).toEqual(0);
                    });

                it("The totals line is 0 for both credit and debit if element is undefined",
                    function() {
                        $scope.Lines = [{}];
                        $scope.updateTotals();
                        expect($scope.TotalsLine.CreditSum).toEqual(0);
                        expect($scope.TotalsLine.DebitSum).toEqual(0);
                    });

                it("The totals line is the sum for both debit and credit",
                    function() {
                        $scope.Lines = [
                            {
                                Debit: 300,
                                Credit: 0
                            },
                            {
                                Debit: 100,
                                Credit: 500
                            }
                        ];
                        $scope.updateTotals();
                        expect($scope.TotalsLine.CreditSum).toEqual(500);
                        expect($scope.TotalsLine.DebitSum).toEqual(400);
                    });

                it("The totals line is parse to integer",
                    function() {
                        $scope.Lines = [
                            {
                                Debit: "300",
                                Credit: 0
                            },
                            {
                                Debit: 100,
                                Credit: "500"
                            }
                        ];
                        $scope.updateTotals();
                        expect($scope.TotalsLine.CreditSum).toEqual(500);
                        expect($scope.TotalsLine.DebitSum).toEqual(400);
                    });

            });

        describe("removeLine",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller =
                        $controller("LedgerEntryController", { $scope: $scope, accountService: {} });
                });

                it("Remove when only one line does not remove anything",
                    function() {
                        expect($scope.Lines.length).toEqual(1);
                        $scope.removeLine(0);
                        expect($scope.Lines.length).toEqual(1);
                    });

                it("Remove when two lines, removes the correct line",
                    function() {
                        $scope.Lines[0].Account = "320";
                        $scope.addNewLine();
                        $scope.Lines[1].Account = "400";
                        expect($scope.Lines.length).toEqual(2);
                        $scope.removeLine(0);
                        expect($scope.Lines.length).toEqual(1);
                        expect($scope.Lines[0].Account).toEqual("400");
                    });
            });

        describe("fillAccount",
            function() {

                var $scope, controller;

                var accountService = {
                    Accounts: [{ code: "420000002", name: "General kenobi" }]
                };

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller =
                        $controller("LedgerEntryController", { $scope: $scope, accountService: accountService });
                });


                it("420 becomes 420000000 (padded to complete 9 digits)",
                    function() {
                        $scope.Lines[0].Account = "420";
                        $scope.fillAccount(0);
                        expect($scope.Lines[0].Account).toEqual("420000000");
                    }
                );

                it("420.1 becomes 420000001",
                    function() {
                        $scope.Lines[0].Account = "420.1";
                        $scope.fillAccount(0);
                        expect($scope.Lines[0].Account).toEqual("420000001");
                    });

                it("420.1005 becomes 420001005",
                    function() {
                        $scope.addNewLine();
                        $scope.Lines[1].Account = "420.1005";
                        $scope.fillAccount(1);
                        expect($scope.Lines[1].Account).toEqual("420001005");
                    });

                it("420000000 is unchanged",
                    function() {
                        $scope.addNewLine();
                        $scope.Lines[1].Account = "420000000";
                        $scope.fillAccount(1);
                        expect($scope.Lines[1].Account).toEqual("420000000");
                    });


                it("Empty string is unchanged",
                    function() {
                        $scope.Lines[0].Account = "";
                        $scope.fillAccount(0);
                        expect($scope.Lines[0].Account).toEqual("");
                    });

                it("Account does not match regex /^[0-9]+\.?[0-9]+$/ is left unchanged",
                    function() {
                        $scope.Lines[0].Account = "420.0.1";
                        $scope.fillAccount(0);
                        expect($scope.Lines[0].Account).toEqual("420.0.1");
                    });

                it("Account is longer than 9 with no dot, left unchanged",
                    function() {
                        $scope.Lines[0].Account = "4200000000";
                        $scope.fillAccount(0);
                        expect($scope.Lines[0].Account).toEqual("4200000000");
                    });

                it("Sets accountClass to not-existing if the account does not exist",
                    function() {
                        $scope.Lines[0].Account = "420.1";
                        $scope.fillAccount(0);
                        expect($scope.AccountClass[0]).toEqual("not-existing");
                    }
                );

                it("Sets AccountClass to empty if the account exists exist",
                    function() {
                        $scope.Lines[0].Account = "420.2";
                        $scope.fillAccount(0);
                        expect($scope.AccountClass[0]).toEqual("");
                    }
                );

                it("Sets AccountName if exists, removes if it does not",
                    function() {
                        $scope.addNewLine();
                        $scope.Lines[1].Account = "420.2";
                        $scope.fillAccount(1);
                        expect($scope.AccountName[1]).toEqual("General kenobi");
                        $scope.Lines[1].Account = "420.1";
                        $scope.fillAccount(1);
                        expect($scope.AccountName[1]).toEqual("");
                    });
            });

        describe("addNewLine",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller = $controller("LedgerEntryController", { $scope: $scope, accountService: {} });
                });

                it("Adds new line",
                    function() {
                        expect($scope.Lines.length).toEqual(1);
                        $scope.addNewLine();
                        expect($scope.Lines.length).toEqual(2);
                    });
            });

        describe("changing debit or credit",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();

                    controller = $controller("LedgerEntryController",
                        { $scope: $scope, accountService: { Accounts: [] } });
                });

                it("Removes debit if credit entered and is different from 0",
                    function() {
                        $scope.Lines[0].Debit = 400.50;
                        $scope.$digest();
                        expect($scope.Lines[0].Debit).toEqual(400.50);
                        $scope.Lines[0].Credit = 300;
                        $scope.$digest();
                        expect($scope.Lines[0].Credit).toEqual(300);
                        expect($scope.Lines[0].Debit).toEqual(0);
                    });

                it("Does not allow negative numbers",
                    function() {
                        $scope.Lines[0].Debit = -200;
                        $scope.$digest();
                        expect($scope.Lines[0].Debit).toEqual(0);

                        $scope.addNewLine();
                        $scope.Lines[1].Credit = -0.5;
                        $scope.$digest();
                        expect($scope.Lines[1].Credit).toEqual(0);
                    });
            });

        describe("searchAccounts",
            function() {

                var $scope, controller;

                beforeEach(function() {


                    var accountService = {
                        Accounts: [
                            { code: "420000002", name: "General kenobi" }, { code: "4600000001", name: "Another" },
                            { code: "420000003", name: "Yet another" }
                        ]
                    };

                    $scope = $rootScope.$new();
                    controller = $controller("LedgerEntryController", { $scope: $scope, accountService });
                });

                it("Empty or null string returns nothing",
                    function() {
                        $scope.searchAccounts("");
                        expect($scope.filteredAccounts.length).toEqual(0);
                    });

                it("Not matching with name or code returns nothing",
                    function() {
                        $scope.searchAccounts("");
                        expect($scope.filteredAccounts.length).toEqual(0);
                    });

                it("Matching code returns accounts",
                    function() {
                        $scope.searchAccounts("420000");
                        expect($scope.filteredAccounts.length).toEqual(2);
                        expect($scope.filteredAccounts).toContain({ code: "420000002", name: "General kenobi" });
                        expect($scope.filteredAccounts).toContain({ code: "420000003", name: "Yet another" });
                    });

                it("Matching name returns accounts",
                    function() {
                        $scope.searchAccounts("another");
                        expect($scope.filteredAccounts.length).toEqual(2);
                        expect($scope.filteredAccounts).toContain({ code: "4600000001", name: "Another" });
                        expect($scope.filteredAccounts).toContain({ code: "420000003", name: "Yet another" });
                    });
            });

        describe("error validation",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = $rootScope.$new();
                    controller = $controller("LedgerEntryController",
                        {
                            $scope: $scope,
                            accountService: {
                                Accounts: [
                                    { code: "527000000", name: "General kenobi" },
                                    { code: "420000001", name: "Hello there" }
                                ]
                            }
                        });
                });

                it("If any account does not exist, do not validate",
                    function() {
                        $scope.addNewLine();
                        $scope.Lines[0].Debit = 400;
                        $scope.Lines[0].Account = "420000000";
                        $scope.Lines[1].Credit = 400;
                        $scope.Lines[1].Account = "527000000";
                        $scope.validate();
                        expect($scope.HasErrors).toEqual(true);
                        expect($scope.FormErrors).toContain("La cuenta 420000000 no existe.");

                        $scope.Lines[0].Account = "420000001";
                        $scope.validate();
                        expect($scope.HasErrors).toEqual(false);
                        expect($scope.FormErrors.length).toEqual(0);
                    });

                it("If any line has no debit or credit, do not validate",
                    function() {
                        $scope.addNewLine();
                        $scope.Lines[0].Debit = 400;
                        $scope.Lines[0].Account = "420000001";
                        $scope.Lines[1].Credit = 0;
                        $scope.Lines[1].Account = "527000000";

                        $scope.validate();
                        expect($scope.HasErrors).toEqual(true);
                        expect($scope.FormErrors)
                            .toContain("La línea 2 no tiene ningún valor ni en el debe ni en el haber.");

                        $scope.Lines[1].Credit = 400;
                        $scope.validate();
                        expect($scope.HasErrors).toEqual(false);
                        expect($scope.FormErrors.length).toEqual(0);
                    });

            });

        describe("managePresets",
            function() {

                beforeEach(function() {
                    $scope = $rootScope.$new();

                    presetsService = {
                        AddPresets: function(name, presets) {}
                    };
                    controller = $controller("LedgerEntryController",
                        { $scope: $scope, accountService: {}, presetsService: presetsService });
                    spyOn(presetsService, "AddPresets");
                });

                it("Passes the account, order and nature of all current entries",
                    function() {
                        $scope.addNewLine();
                        $scope.addNewLine();
                        $scope.Lines[0].Debit = 400;
                        $scope.Lines[0].Account = "420000001";
                        $scope.Lines[1].Credit = 320;
                        $scope.Lines[1].Account = "527000000";
                        $scope.Lines[2].Credit = 80;
                        $scope.Lines[2].Account = "537000000";

                        $scope.addCurrentEntriesAsPreset("My Preset");

                        expect(presetsService.AddPresets).toHaveBeenCalledWith("My Preset",
                            [
                                { Account: "420000001", IsDebit: true, Order: 1 },
                                { Account: "527000000", IsDebit: false, Order: 2 },
                                { Account: "537000000", IsDebit: false, Order: 3 }
                            ]);
                    });
            });

        describe("startNewAccountCreation",
            function() {
                var $scope, controller, accountService;

                beforeEach(function() {
                    $scope = $rootScope.$new();

                    accountService = {
                        Accounts: [],
                        CreateAccount: function(code, name, description) {}
                    };
                    controller = $controller("LedgerEntryController",
                        { $scope: $scope, accountService: accountService, presetsService: {} });
                    spyOn(accountService, "CreateAccount");
                });

                it("Sets code to the one provided, leaves rest of fields empty",
                    function() {
                        $scope.startNewAccountCreation("420000000");
                        expect($scope.NewAccount.Code).toEqual("420000000");
                        expect($scope.NewAccount.Name).toEqual("");
                        expect($scope.NewAccount.Description).toEqual("");

                        $scope.NewAccount.Name = "TEST";
                        expect($scope.NewAccount.Name).toEqual("TEST");

                        $scope.startNewAccountCreation("450000000");
                        expect($scope.NewAccount.Code).toEqual("450000000");
                        expect($scope.NewAccount.Name).toEqual("");
                        expect($scope.NewAccount.Description).toEqual("");
                    });

                it("Sends data when calling commitNewAccount",
                    function() {
                        $scope.startNewAccountCreation("420000000");
                        $scope.NewAccount.Name = "Kenobi";
                        $scope.NewAccount.Description = "Hello";
                        $scope.commitNewAccount();
                        expect(accountService.CreateAccount).toHaveBeenCalledWith("420000000",
                            "Kenobi",
                            "Hello",
                            jasmine.any(Function),
                            jasmine.any(Function));
                    });
            });
    });