// one first test, we can run from here

describe("LedgerEntryController",
    function() {

        beforeEach(module("contabilidadApp"));

        var $controller;

        beforeEach(inject(function(_$controller_) {
            $controller = _$controller_;
        }));

        describe("initial elements in lines",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = {};
                    controller = $controller("LedgerEntryController", { $scope: $scope });
                });

                it("When entering new, it has one entry by default with no data",
                    function() {
                        expect($scope.Lines.length).toEqual(1);
                    });

                it("It has defined null properties",
                    function() {
                        expect($scope.Lines[0].Account).toBeNull();
                        expect($scope.Lines[0].Debit).toBeNull();
                        expect($scope.Lines[0].Credit).toBeNull();
                        expect($scope.Lines[0].Remarks).toBeNull();
                    });
            });

        describe("addNewLine",
            function() {
                var $scope, controller;

                beforeEach(function() {
                    $scope = {};
                    controller = $controller("LedgerEntryController", { $scope: $scope });
                });

                it("Adds new line",
                    function() {
                        expect($scope.Lines.length).toEqual(1);
                        $scope.addNewLine();
                        expect($scope.Lines.length).toEqual(2);
                    });
            });
    });