    var selectedBank = "None",
        curATMPin = undefined,
        curATMAccountNumber = undefined,
        curATMzoneName = undefined,
        globalFactionATMtype = undefined,
        globalFactionATMfactionid = undefined;

    $(document).ready(function() {
        alt.emit("Client:HUD:cefIsReady");
    });

    function numberWithCommas(x) {
        let numberwithComma = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return numberwithComma.replace(/,/g, '.');
    }

    function createBankAccountManageForm(bankArray, curBank) {
        var bankFormHTML = "";
        var bankAccountCount = 0;
        bankArray = JSON.parse(bankArray);
        for (var i in bankArray) {
            bankFormHTML +=
                `<li class='list-group-item'><p class='title'>Konto Nr.: ${bankArray[i].accountNumber}</p><p class='floatleft'>Kontostand:</p><p class='floatright'>${bankArray[i].money}$</p><p class='floatleft'>Gründungsort:</p><p class='floatright'>${bankArray[i].createZone}</p>` +
                "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`generatepin`, " + bankArray[i]
                .accountNumber + ");'>Pin Ändern</button>";

            if (bankArray[i].closed === true) {
                bankFormHTML += "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`lock`, " +
                    bankArray[i].accountNumber + ");'>Konto Entsperren</button>";
            } else {
                bankFormHTML += "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`lock`, " +
                    bankArray[i].accountNumber + ");'>Konto Sperren</button>";
            }

            if (bankArray[i].mainAccount === true) {
                bankFormHTML += "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`setMain`, " +
                    bankArray[i].accountNumber + ");'>Hauptkonto</button>";
                bankAccountCount++;
            } else {
                bankFormHTML += "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`setMain`, " +
                    bankArray[i].accountNumber + ");'>Hauptkonto Setzen</button>";
                bankAccountCount++;
            }
            bankFormHTML += "<button class='btn btn-primary' onclick='BankAccountManageFormAction(`copycard`, " +
                bankArray[i].accountNumber + ");'>Neue Karte Beantragen</button>";
            bankFormHTML += "</li>";
        }

        if (bankAccountCount < 2) {
            bankFormHTML +=
                "<li class='list-group-item new' onclick='BankAccountManageFormCreateNewAccount()'><p class='title'>Neues Konto</p><i class='fas fa-plus'></i></li>";
        }
        selectedBank = curBank;
        $("#bankAccountManageFormList").html(bankFormHTML);
        $("#bankAccountManageForm").fadeTo(1000, 1, function() {});
    }

    function BankAccountManageFormDestroyCEF() {
        $("#bankAccountManageForm").fadeOut(500, function() {
            $("#bankAccountManageForm").hide();
        });
        alt.emit('Client:Bank:BankAccountdestroyCEF');
    }

    $("#bankFactionATMForm-DepositBtn").click(function() {
        var selectedAmount = $("#bankFactionATMForm-DepositInput").val();
        if (selectedAmount < 1 || selectedAmount == "" || globalFactionATMfactionid == undefined ||
            globalFactionATMfactionid == 0 || globalFactionATMfactionid == "0") return;
        if (globalFactionATMtype == undefined || globalFactionATMtype == 0 || globalFactionATMtype == "")
            return;
        alt.emit("Client:FactionBank:DepositMoney", globalFactionATMtype, globalFactionATMfactionid,
            selectedAmount);
        BankFactionATMFormDestroyCEF();
    });

    $("#bankFactionATMForm-WithdrawBtn").click(function() {
        var selectedAmount = $("#bankFactionATMForm-WithdrawInput").val();
        if (selectedAmount < 1 || selectedAmount == "" || globalFactionATMfactionid == undefined ||
            globalFactionATMfactionid == 0 || globalFactionATMfactionid == "0") return;
        if (globalFactionATMtype == undefined || globalFactionATMtype == 0 || globalFactionATMtype == "")
            return;
        alt.emit("Client:FactionBank:WithdrawMoney", globalFactionATMtype, globalFactionATMfactionid,
            selectedAmount);
        BankFactionATMFormDestroyCEF();
    });


    function BankFactionATMFormDestroyCEF() {
        $("#bankFactionATMForm").fadeOut(500, function() {
            globalFactionATMfactionid = undefined;
            globalFactionATMtype = undefined;
            $("#bankFactionATMForm").hide();
        });
        alt.emit("Client:FactionBank:destroyCEF");
    }

    function BankAccountManageFormCreateNewAccount() {
        if (selectedBank == "None") return;
        alt.emit("Client:Bank:BankAccountCreateNewAccount", selectedBank);
        $("#bankAccountManageForm").hide();
    }

    function BankAccountManageFormAction(action, accountNumber) {
        alt.emit("Client:Bank:BankAccountAction", action, `${accountNumber}`);
        $("#bankAccountManageForm").hide();
    }

    function BankATMcreateCEF(PIN, accountNumber) {
        curATMPin = PIN;
        curATMAccountNumber = accountNumber;
        BankATMshowSite("bankATMbox-PinArea");
        $("#bankATMbox").fadeTo(1000, 1, function() {});
        $("#sidenav").hide();
    }

    function BankATMdestroyCEF() {
        $("#bankATMbox").fadeOut(500, function() {
            $("#bankATMbox").hide();
        });
        alt.emit("Client:ATM:BankATMdestroyCEF");
    }

    $("#bankATMBox-PinInput-Button").click(function() {
        var inputPin = $("#bankATMBox-PinInput").val();
        if (curATMPin == inputPin) {
            alt.emit("Client:ATM:TryPin", "reset", curATMAccountNumber);
            BankATMshowSite("bankATMbox-OverviewArea");
            $("#sidenav").fadeTo(1000, 1, function() {});
        } else {
            alt.emit("Client:ATM:TryPin", "increase", curATMAccountNumber);
        }
    });

    $("#bankATMBox-DepositArea-Button").click(function() {
        var selectedAmount = $("#bankATMBox-DepositInput").val();
        if (selectedAmount < 1 || selectedAmount == "") return;
        alt.emit("Client:ATM:DepositMoney", curATMAccountNumber, selectedAmount, curATMzoneName);
        BankATMshowSite("bankATMbox-OverviewArea");
    });

    $("#bankATMBox-WithdrawArea-Button").click(function() {
        var selectedAmount = $("#bankATMBox-WithdrawInput").val();
        if (selectedAmount < 1 || selectedAmount == "") return;
        alt.emit("Client:ATM:WithdrawMoney", curATMAccountNumber, selectedAmount, curATMzoneName);
        BankATMshowSite("bankATMbox-OverviewArea");
    });

    $("#bankATMBox-DoMoneyTransfer-Button").click(function() {
        var targetAccount = $("#bankATMBox-MoneyTransferTargetInput").val();
        var selectedAmount = $("#bankATMBox-MoneytransferInput").val();
        if (selectedAmount < 1 || selectedAmount == "" || targetAccount == 0 || targetAccount == "") return;
        alt.emit("Client:ATM:TransferMoney", curATMAccountNumber, targetAccount, selectedAmount,
            curATMzoneName);
        BankATMshowSite("bankATMbox-OverviewArea");
    });

    function BankATMshowSite(site) {
        $("#bankATMbox-PinArea").hide();
        $("#bankATMbox-WithDrawArea").hide();
        $("#bankATMbox-DepositArea").hide();
        $("#bankATMbox-ViewTransactionArea").hide();
        $("#bankATMbox-OverviewArea").hide();
        $("#bankATMbox-DoMoneyTransfer").hide();
        $(`#${site}`).fadeTo(500, 1, function() {
            $(`#${site}`).show();
        });

        if (site == "bankATMbox-PinArea") {
            $("#bankATMboxDescriptionTitle").html(
                "Um weitere Aktionen durchführen zu können geben Sie bitte Ihre Geheimzahl ein.<br>Geben Sie Ihre Geheimzahl zu oft falsch ein wird Ihre Karte gesperrt."
            );
        } else if (site == "bankATMbox-OverviewArea") {
            $("#bankATMboxDescriptionTitle").html("Wählen Sie eine der unten aufgelisteten Möglichkeiten aus.");
            document.getElementById("bankATMBox-OverviewCardNumber").value = curATMAccountNumber;
            alt.emit("Client:ATM:requestBankData", curATMAccountNumber);
        }
    }

    function BankATMSetRequestedData(curBalance, bankPaperArray) {
        bankPaperArray = JSON.parse(bankPaperArray);
        let bankPaperHTML = "";

        for (var i in bankPaperArray) {
            if (bankPaperArray[i].Type == "Einzahlung") {
                bankPaperHTML += "<div class='card text-white green'>";
            } else if (bankPaperArray[i].Type == "Auszahlung") {
                bankPaperHTML += "<div class='card text-white red'>";
            } else if (bankPaperArray[i].Type == "Eingehende Überweisung" || bankPaperArray[i].Type ==
                "Ausgehende Überweisung") {
                bankPaperHTML += "<div class='card text-white blue'>";
            }

            bankPaperHTML +=
                `<div class='card-header'>Aktivität vom ${bankPaperArray[i].Date} ${bankPaperArray[i].Time} Uhr</div>` +
                `<div class='card-body'><h6 class='card-title'>${bankPaperArray[i].Type}</h6><p class='card-text'>`;

            if (bankPaperArray[i].Type == "Einzahlung" || bankPaperArray[i].Type == "Auszahlung") {
                bankPaperHTML += `<b>Standort: </b>${bankPaperArray[i].zoneName}<br>` +
                    `<b>Betrag: </b>${bankPaperArray[i].moneyAmount}$<br>`;
            } else if (bankPaperArray[i].Type == "Ausgehende Überweisung") {
                bankPaperHTML += `<b>Absender: </b>${bankPaperArray[i].accountNumber}<br>` +
                    `<b>Empfänger: </b>${bankPaperArray[i].ToOrFrom}<br>` +
                    `<b>Standort: </b>${bankPaperArray[i].zoneName}<br>` +
                    `<b>Betrag: </b>${bankPaperArray[i].moneyAmount}$<br>`;
            } else if (bankPaperArray[i].Type == "Eingehende Überweisung") {
                bankPaperHTML += `<b>Absender: </b>${bankPaperArray[i].ToOrFrom}<br>` +
                    `<b>Empfänger: </b>${bankPaperArray[i].accountNumber}<br>` +
                    `<b>Standort: </b>${bankPaperArray[i].zoneName}<br>` +
                    `<b>Betrag: </b>${bankPaperArray[i].moneyAmount}$<br>`;
            }
            bankPaperHTML += `</p></div></div>`;
        }

        $("#bankATMBox-WithdrawBalanceInput").val(`${curBalance}$`);
        $("#bankATMBox-DepositBalanceInput").val(`${curBalance}$`);
        $("#bankATMBox-MoneyTransferBalanceInput").val(`${curBalance}$`);
        $("#bankATMbox-ViewTransactionArea-List").html(bankPaperHTML);
    }

    alt.on("CEF:Bank:createBankAccountManageForm", (bankArray, curBank) => {
        createBankAccountManageForm(bankArray, curBank);
    });

    alt.on("CEF:ATM:BankATMcreateCEF", (pin, accNumber, zoneName) => {
        BankATMcreateCEF(pin, accNumber);
        curATMzoneName = zoneName;
    });

    alt.on("CEF:ATM:BankATMSetRequestedData", (curBalance, paperArray) => {
        BankATMSetRequestedData(curBalance, paperArray);
    });

    alt.on("CEF:ATM:BankATMdestroyCEF", () => {
        BankATMdestroyCEF();
    });

    alt.on("CEF:General:hideAllCEFs", (hideCursor) => {
        $("#bankAccountManageForm").fadeOut(500, function() {
            $("#bankAccountManageForm").hide();
        });

        $("#bankATMbox").fadeOut(500, function() {
            $("#bankATMbox").hide();
        });

        $("#bankFactionATMForm").fadeOut(500, function() {
            globalFactionATMfactionid = undefined;
            globalFactionATMtype = undefined;
            $("#bankFactionATMForm").hide();
        });
    });

    alt.on("CEF:FactionBank:createCEF", (type, factionId, factionBalance) => {
        globalFactionATMfactionid = factionId;
        globalFactionATMtype = type;
        if (type == "faction") {
            $("#bankFactionATMForm-Title").html(`FRAKTIONSBANK`);
        } else if (type == "company") {
            $("#bankFactionATMForm-Title").html(`UNTERNEHMENSKONTO`);
        }
        $("#bankFactionATMForm-CurBalance").html(`${factionBalance}$`);
        $("#bankFactionATMForm").fadeTo(1000, 1, function() {});
    });