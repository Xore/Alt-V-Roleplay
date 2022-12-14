//#region design


var currentCardBackground = Math.floor(Math.random()* 25 + 1)
$(".card-item__bg").attr("src", "assets/img/creditcard/" + currentCardBackground + ".jpeg");
$(function () {
 $(".menu-link").click(function () {
  $(".menu-link").removeClass("is-active");
  $(this).addClass("is-active");
 });
});

$(function () {
 $(".main-header-link").click(function () {
  $(".main-header-link").removeClass("is-active");
  $(this).addClass("is-active");
 });
});

const dropdowns = document.querySelectorAll(".dropdown");
dropdowns.forEach((dropdown) => {
 dropdown.addEventListener("click", (e) => {
  e.stopPropagation();
  dropdowns.forEach((c) => c.classList.remove("is-active"));
  dropdown.classList.add("is-active");
 });
});

$(".search-bar input")
 .focus(function () {
  $(".header").addClass("wide");
 })
 .blur(function () {
  $(".header").removeClass("wide");
 });

$(document).click(function (e) {
 var container = $(".status-button");
 var dd = $(".dropdown");
 if (!container.is(e.target) && container.has(e.target).length === 0) {
  dd.removeClass("is-active");
 }
});

$(function () {
 $(".dropdown").on("click", function (e) {
  $(".content-wrapper").addClass("overlay");
  e.stopPropagation();
 });
 $(document).on("click", function (e) {
  if ($(e.target).is(".dropdown") === false) {
   $(".content-wrapper").removeClass("overlay");
  }
 });
});

$(function () {
 $(".status-button:not(.open)").on("click", function (e) {
  $(".overlay-app").addClass("is-active");
 });
 $(".pop-up .close").click(function () {
  $(".overlay-app").removeClass("is-active");
 });
});

$(".status-button:not(.open)").click(function () {
 $(".pop-up").addClass("visible");
});

$(".pop-up .close").click(function () {
 $(".pop-up").removeClass("visible");
});

(function() {
  feather.replace()  
})();
//#endregion

//#region  custom code to control apps and so on

//  init things

$(document).ready(function() {
	SetInternetAppVehicleStoreContent('[{ "name": "Police Interceptor", "manufactor": "Vapid", "fueltype": "Benzin", "fuellimit": 80, "fuelusage": 2, "storage": 25, "seats": 4, "price": 7500, "hash": "1912215274" }, { "name": "Interceptor", "manufactor": "Vapid", "fueltype": "Diesel", "fuellimit": 40, "fuelusage": 4, "storage": 35, "seats": 4, "price": 9500, "hash": "1912215274" }, { "name": "Police Interceptor", "manufactor": "Vapid", "fueltype": "Benzin", "fuellimit": 80, "fuelusage": 2, "storage": 25, "seats": 4, "price": 7500, "hash": "1912215274" }, { "name": "Interceptor", "manufactor": "Vapid", "fueltype": "Diesel", "fuellimit": 40, "fuelusage": 4, "storage": 35, "seats": 4, "price": 9500, "hash": "1912215274" }, { "name": "Police Interceptor", "manufactor": "Vapid", "fueltype": "Benzin", "fuellimit": 80, "fuelusage": 2, "storage": 25, "seats": 4, "price": 7500, "hash": "1912215274" }, { "name": "Interceptor", "manufactor": "Vapid", "fueltype": "Diesel", "fuellimit": 40, "fuelusage": 4, "storage": 35, "seats": 4, "price": 9500, "hash": "1912215274" }]');
	showArea("homescreen");
	alt.emit("Client:Tablet:cefIsReady");	
});

const homeButton = $("#homebutton");



const financeappButton = $("#financeapp");
const lspdappButton = $("#lspdapp");
const fbiappButton = $("#fbiapp");
const lsfdappButton = $("#lsfdapp");
const aclsappButton = $("#aclsapp");
const justiceappButton = $("#justiceapp");
const bennysappButton = $("#bennysapp");
const taxiappButton = $("#taxiapp");
const vehiclestoreButton = $("#vehiclestoreapp");
const lifeinvaderButton = $("#lifeinvaderapp");
const newsButton = $("#newsapp");
const eventsButton = $("#eventsapp");
const vehiclesButton = $("#vehiclesapp");
const factionButton = $("#factionmanagementapp");
const gangButton = $("#gangmangementapp");

// justice buttons
const justicesearchbtn = $("#justicesearchbtn");
const justicevehiclesearchbtn = $("#justicevehiclesearchbtn");
const justicelicencebtn = $("#justicelicencebtn");
const justiceweaponbtn = $("#justiceweaponbtn");
const justicevehiclelistbtn = $("#justicevehiclelistbtn");
const justicebankcheckbtn = $("#justicebankcheckbtn");

// lspd buttons
const lspdsearchbtn = $("#lspdsearchbtn");
const lspdvehiclesearchbtn = $("#lspdvehiclesearchbtn");
const lspdlicencebtn = $("#lspdlicencebtn");
const lspddispatchesbtn = $("#lspddispatchesbtn");
const lspdvehiclelistbtn = $("#lspdvehiclelistbtn");

// fbi buttons
const fbisearchbtn = $("#fbisearchbtn");
const fbivehiclesearchbtn = $("#fbivehiclesearchbtn");
const fbilicencebtn = $("#fbilicencebtn");
const fbidispatchesbtn = $("#fbidispatchesbtn");
const fbivehiclelistbtn = $("#fbivehiclelistbtn");

// lsfd buttons
const lsfddispatchesbtn = $("#lsfddispatchesbtn");
const lsfdvehiclelistbtn = $("#lsfdvehiclelistbtn");

// taxi buttons
const taxidispatchesbtn = $("#taxidispatchesbtn");
const taxivehiclelistbtn = $("#taxivehiclelistbtn");

// bennys buttons
const bennysdispatchesbtn = $("#bennysdispatchesbtn");
const bennysvehiclelistbtn = $("#bennysvehiclelistbtn");

// acls buttons
const aclssearchbtn = $("#aclssearchbtn");
const aclsvehiclesearchbtn = $("#aclsvehiclesearchbtn");
const aclslicencebtn = $("#aclslicencebtn");
const aclsdispatchesbtn = $("#aclsdispatchesbtn");
const aclsvehiclelistbtn = $("#aclsvehiclelistbtn");

// screens
const homeScreen = $("#homescreen");

// finance screens
const financeScreen = $("#finance");
const financeMain = $("#financemain");
const financeTransaction = $("#financetransaction");
const financehistory = $("#financehistory");
const newTransaction = $("#btntransactions");
const transactionbutton = $("#transactionbutton");
const btnhistory = $("#btnhistory");

// lspd screens
const lspdScreen = $("#lspd");
const lspdMain = $("#lspdmain");
const lspdSearch = $("#lspdsearch");
const lspdvehicleSearch = $("#lspdvehiclesearch");
const lspdlicencecheck = $("#lspdlicencecheck");
const lspddispatches = $("#lspddispatches");
const lspdvehiclelist = $("#lspdvehiclelist");

//  vehicle screens
const vehicleScreen = $("#vehiclescreen");

//  event screen
const eventScreen = $("#eventscreen");
const eventMain = $("#eventMain");
const createEvent = $("#createEvent");
// event buttons
const newEventbtn = $("#newEventbtn");
const createEventbtn = $("#createEventbtn");

// gangmanagement buttons
const gangViewMemberbtn = $("#gangViewMemberbtn");
const gangRankManagementbtn = $("#gangRankManagementbtn");
const gangMemberInvitebtn = $("#gangMemberInvitebtn");
const gangmanagerAppInviteNewMemberbtn = $("#gangmanagerAppInviteNewMemberbtn");

// gangmanagement screen
const gangmanagementScreen = $("#gangmanagementscreen");
const gangMain = $("#gangMain");
const gangMember = $("#gangMember");
const gangNewMember = $("#gangNewMember");
const gangRankMangement = $("#gangRankMangement");

// factionmanagement screen
const factionmanagementScreen = $("#factionmanagementscreen");
const factionMain = $("#factionMain");
const factionMember = $("#factionMember");
const factionNewMember = $("#factionNewMember");
const factionRankManagement = $("#factionRankManagement");

// factionmanagement buttons
const factionViewMemberbtn = $("#factionViewMemberbtn");
const factionRankManagementbtn = $("#factionRankManagementbtn");
const factionMemberInvitebtn = $("#factionMemberInvitebtn");
const factionmanagerAppInviteNewMemberbtn = $("#factionmanagerAppInviteNewMemberbtn");

// news screen
const newsScreen = $("#newsscreen");

// lifeinvader screen
const lifeinvaderScreen = $("#lifeinvaderscreen");

// vehicleshopscreen
const vehicleshopScreen = $("#vehicleshopscreen");

// taxi screen
const taxiScreen = $("#taxi");
const taxiMain = $("#taximain");
const taxidispatches = $("#taxidispatches");
const taxivehiclelist = $("#taxivehiclelist");

// bennys screen
const bennysScreen = $("#bennys");
const bennysMain = $("#bennysmain");
const bennysdispatches = $("#bennysdispatches");
const bennysvehiclelist = $("#bennysvehiclelist");

// justice screen
const justiceScreen = $("#justice");
const justiceMain = $("#justicemain");
const justiceBankcheck = $("#justiceviewbanktransactions");
const justiceSearch = $("#justicesearch");
const justicevehicleSearch = $("#justicevehiclesearch");
const justicelicencecheck = $("#justicelicencecheck");
const justicegiveweaponlicense = $("#justicegiveweaponlicense");
const justicevehiclelist = $("#justicevehiclelist");

// fbi screen
const fbiScreen = $("#fbi");
const fbiMain = $("#fbimain");
const fbiSearch = $("#fbisearch");
const fbivehicleSearch = $("#fbivehiclesearch");
const fbilicencecheck = $("#fbilicencecheck");
const fbidispatches = $("#fbidispatches");
const fbivehiclelist = $("#fbivehiclelist");

// acls screen
const aclsScreen = $("#acls");
const aclsMain = $("#aclsmain");
const aclsdispatches = $("#aclsdispatches");
const aclsvehiclelist = $("#aclsvehiclelist");

// lsfd screen
const lsfdScreen = $("#lsfd");
const lsfdMain = $("#lsfdmain");
const lsfddispatches = $("#lsfddispatches");
const lsfdvehiclelist = $("#lsfdvehiclelist");

// faction buttons

factionViewMemberbtn.bind('click', () => {
	openSubWindow("factionmanagementapp-member");
});

factionRankManagementbtn.bind('click', () => {
	openSubWindow("factionmanagementapp-rankmanagement");
});

factionMemberInvitebtn.bind('click', () => {
	openSubWindow("factionmanagementapp-invitemember");
});

factionmanagerAppInviteNewMemberbtn.bind('click', () => {
	//rework
	showArea("homescreen");
});

// gang buttons

gangViewMemberbtn.bind('click', () => {
	openSubWindow("gangmanagementapp-member");
});

gangRankManagementbtn.bind('click', () => {
	openSubWindow("gangmanagementapp-rankmanagement");
});

gangMemberInvitebtn.bind('click', () => {
	openSubWindow("gangmanagementapp-invitemember");
});

gangmanagerAppInviteNewMemberbtn.bind('click', () => {
	//rework
	showArea("homescreen");
});

// event buttons
newEventbtn.bind('click', () => {
	openSubWindow("eventsapp-newevent");
});

// app buttons (homescreen)

vehiclestoreButton.bind('click', () => {
	showArea("vehicleshopscreen");
});

lifeinvaderButton.bind('click', () => {
	showArea("lifeinvaderscreen");
});

newsButton.bind('click', () => {
	showArea("newsscreen");
});

vehiclesButton.bind('click', () => {
	showArea("vehiclescreen");
});

factionButton.bind('click', () => {
	showArea("factionmanagementscreen");
});

gangButton.bind('click', () => {
	showArea("gangmanagementscreen");
});

eventsButton.bind('click', () => {
	showArea("eventscreen");
});

//  lspd buttons
lspdappButton.bind('click', () => {
	showArea("lspd");
});
lspddispatchesbtn.bind('click', () => {
	openSubWindow("lspdapp-viewdispatches");
});

lspdvehiclelistbtn.bind('click', () => {
	openSubWindow("lspdapp-factionvehicles");
});

lspdlicencebtn.bind('click', () => {
	openSubWindow("lspdapp-licencecheck");
});

lspdsearchbtn.bind('click', () => {
	openSubWindow("lspdapp-search");
});

lspdvehiclesearchbtn.bind('click', () => {
	openSubWindow("lspdapp-vehiclecheck");
});

//  taxi buttons
taxiappButton.bind('click', () => {
	showArea("taxi");
});
taxidispatchesbtn.bind('click', () => {
	openSubWindow("taxiapp-viewdispatches");
});

taxivehiclelistbtn.bind('click', () => {
	openSubWindow("taxiapp-factionvehicles");
});



//  acls buttons
aclsappButton.bind('click', () => {
	showArea("acls");
});
aclsdispatchesbtn.bind('click', () => {
	openSubWindow("aclsapp-viewdispatches");
});

aclsvehiclelistbtn.bind('click', () => {
	openSubWindow("aclsapp-factionvehicles");
});


//  justice buttons
justiceappButton.bind('click', () => {
	showArea("justice");
});
justiceweaponbtn.bind('click', () => {
	openSubWindow("justiceapp-giveweapon");
});

justicevehiclelistbtn.bind('click', () => {
	openSubWindow("justiceapp-factionvehicles");
});

justicebankcheckbtn.bind('click', () => {
	openSubWindow("justiceapp-bankcheck");
});

justicelicencebtn.bind('click', () => {
	openSubWindow("justiceapp-licencecheck");
});

justicesearchbtn.bind('click', () => {
	openSubWindow("justiceapp-search");
});

justicevehiclesearchbtn.bind('click', () => {
	openSubWindow("justiceapp-vehiclecheck");
});

//  bennys buttons
bennysappButton.bind('click', () => {
	showArea("bennys");
});
bennysdispatchesbtn.bind('click', () => {
	openSubWindow("bennysapp-viewdispatches");
});

bennysvehiclelistbtn.bind('click', () => {
	openSubWindow("bennysapp-factionvehicles");
});

//  lsfd buttons
lsfdappButton.bind('click', () => {
	showArea("lsfd");
});
lsfddispatchesbtn.bind('click', () => {
	openSubWindow("lsfdapp-viewdispatches");
});

lsfdvehiclelistbtn.bind('click', () => {
	openSubWindow("lsfdapp-factionvehicles");
});

//  fbi buttons
fbiappButton.bind('click', () => {
	showArea("fbi");
});
fbidispatchesbtn.bind('click', () => {
	openSubWindow("fbiapp-viewdispatches");
});

fbivehiclelistbtn.bind('click', () => {
	openSubWindow("fbiapp-factionvehicles");
});

fbilicencebtn.bind('click', () => {
	openSubWindow("fbiapp-licencecheck");
});

fbisearchbtn.bind('click', () => {
	openSubWindow("fbiapp-search");
});

fbivehiclesearchbtn.bind('click', () => {
	openSubWindow("fbiapp-vehiclecheck");
});

// banking buttons

financeappButton.bind('click', () => {
	showArea("finance");
});

btnhistory.bind('click', () => {
	openSubWindow("financeapp-history");
});

transactionbutton.bind('click', () => {
	bankingButtonClicked();
});

newTransaction.bind('click', () => {
	openSubWindow("financeapp-newtransaction");
});

homeButton.bind('click', () => {
	showArea("homescreen");
});


var intervalId = window.setInterval(function(){
	var formatted = new Date().toLocaleTimeString();
	$('#phone-min-time').html(formatted);
}, 1000);


//  custom functions

function defaultFinance(){
    financeMain.show();
    financeTransaction.hide();
    financehistory.hide();
}

function defaultLspd(){
    lspdMain.show();
    lspdSearch.hide();
    lspdvehicleSearch.hide();
    lspdlicencecheck.hide();
    lspddispatches.hide();
    lspdvehiclelist.hide();
}

function defualtBennys(){
    bennysMain.show();
    bennysdispatches.hide();
    bennysvehiclelist.hide();
}

function defaultJustice(){
    justiceMain.show();
    justiceSearch.hide();
    justicevehicleSearch.hide();
    justicelicencecheck.hide();
    justicegiveweaponlicense.hide();
    justicevehiclelist.hide();
	justiceBankcheck.hide();
}

function defaultFBI(){
    fbiMain.show();
    fbiSearch.hide();
    fbivehicleSearch.hide();
    fbilicencecheck.hide();
    fbidispatches.hide();
    fbivehiclelist.hide();
}

function defaultACLS(){
    aclsMain.show();
    aclsdispatches.hide();
    aclsvehiclelist.hide();
}

function defaultLSFD(){
    lsfdMain.show();
    lsfddispatches.hide();
    lsfdvehiclelist.hide();
}

function defaultTaxi(){
    taxiMain.show();
    taxidispatches.hide();
    taxivehiclelist.hide();
}

function defaultFaction(){
	factionMain.show();
	factionMember.hide();
	factionNewMember.hide();
	factionRankManagement.hide();
}

function defaultGang(){
	gangMain.show();
	gangMember.hide(),
	gangNewMember.hide();
	gangRankMangement.hide();
}

function showHomescreen(){
	defaultACLS();
	defaultFBI();
	defaultFinance();
	defaultJustice();
	defaultLSFD();
	defaultLspd();
	defaultTaxi();
	defualtBennys();
	defaultGang();
	defaultFaction();

	financeScreen.hide();
	lspdScreen.hide();
	fbiScreen.hide();
	bennysScreen.hide();
	justiceScreen.hide();
	lsfdScreen.hide();
	aclsScreen.hide();
	taxiScreen.hide();
	vehicleScreen.hide();
	eventScreen.hide();
	gangmanagementScreen.hide();
	factionmanagementScreen.hide();
	newsScreen.hide();
	lifeinvaderScreen.hide();
	vehicleshopScreen.hide();
	
	homeScreen.show(); 
}

//#endregion
//#region navigation events
var curAppOpened = "none";
    function showArea(area) {
        if (area == "homescreen") {
            // default any screen back
            defaultACLS();
            defaultFBI();
            defaultFinance();
            defaultJustice();
            defaultLSFD();
            defaultLspd();
            defaultTaxi();
            defualtBennys();
            defaultGang();
            defaultFaction();
        }

        if (curAppOpened != "none") {
            $(`#${curAppOpened}`).fadeOut(400);
            $(`#${curAppOpened}main`).fadeOut(400);
        }
        $(`#${area}`).fadeIn(800);
        $(`#${area}main`).fadeIn(800);
        curAppOpened = area;
    }

    function openSubWindow(subwindow) {
        switch (subwindow) {            
            //factionmanagement
            case "factionmanagementapp-overview":
                $("#factionmanagementscreen").fadeIn(500);
                $("#factionMain").fadeIn(500);
                $("#factionMember").fadeOut(500);
                $("#factionNewMember").fadeOut(500);
                $("#factionRankManagement").fadeOut(500);
                break;
            case "factionmanagementapp-member":
                $("#factionmanagementscreen").fadeIn(500);
                $("#factionMember").fadeIn(500);
                $("#factionMain").fadeOut(500);
                $("#factionNewMember").fadeOut(500);
                $("#factionRankManagement").fadeOut(500);
                break;
            case "factionmanagementapp-invitemember":
                $("#factionmanagementscreen").fadeIn(500);
                $("#factionNewMember").fadeIn(500);
                $("#factionMain").fadeOut(500);
                $("#factionMember").fadeOut(500);
                $("#factionRankManagement").fadeOut(500);
                break;
            case "factionmanagementapp-rankmanagement":
                $("#factionmanagementscreen").fadeIn(500);
                $("#factionRankManagement").fadeIn(500);
                $("#factionMain").fadeOut(500);
                $("#factionMember").fadeOut(500);
                $("#factionNewMember").fadeOut(500);
                break;
            //gangmanagement
            case "gangmanagementapp-overview":
                $("#gangmanagementscreen").fadeIn(500);
                $("#gangMain").fadeIn(500);
                $("#gangMember").fadeOut(500);
                $("#gangNewMember").fadeOut(500);
                $("#gangRankMangement").fadeOut(500);
                break;
            case "gangmanagementapp-member":
                $("#gangmanagementscreen").fadeIn(500);
                $("#gangMember").fadeIn(500);
                $("#gangMain").fadeOut(500);
                $("#gangNewMember").fadeOut(500);
                $("#gangRankMangement").fadeOut(500);
                break;
            case "gangmanagementapp-invitemember":
                $("#gangmanagementscreen").fadeIn(500);
                $("#gangNewMember").fadeIn(500);
                $("#gangMain").fadeOut(500);
                $("#gangMember").fadeOut(500);
                $("#gangRankMangement").fadeOut(500);
                break;
            case "gangmanagementapp-rankmanagement":
                $("#gangmanagementscreen").fadeIn(500);
                $("#gangRankMangement").fadeIn(500);
                $("#gangMain").fadeOut(500);
                $("#gangMember").fadeOut(500);
                $("#gangNewMember").fadeOut(500);
                break;
				
            //taxiapp
            case "taxiapp-overview":
                $("#taxi").fadeIn(500);
                $("#taximain").fadeIn(500);
                $("#taxidispatches").fadeOut(500);
                $("#taxivehiclelist").fadeOut(500);
                break;
            case "taxiapp-viewdispatches":
                $("#taxi").fadeIn(500);
                $("#taxidispatches").fadeIn(500);
                $("#taximain").fadeOut(500);
                $("#taxivehiclelist").fadeOut(500);
                break;
            case "taxiapp-factionvehicles":
                $("#taxi").fadeIn(500);
                $("#taxivehiclelist").fadeIn(500);
                $("#taxidispatches").fadeOut(500);
                $("#taximain").fadeOut(500);
                break;
				
            //bennysapp
            case "bennysapp-overview":
                $("#bennys").fadeIn(500);
                $("#bennysmain").fadeIn(500);
                $("#bennysdispatches").fadeOut(500);
                $("#bennysvehiclelist").fadeOut(500);
                break;
            case "bennysapp-viewdispatches":
                $("#bennys").fadeIn(500);
                $("#bennysvehiclelist").fadeIn(500);
                $("#bennysdispatches").fadeOut(500);
                $("#bennysmain").fadeOut(500);
                break;
            case "bennysapp-factionvehicles":
                $("#bennys").fadeIn(500);
                $("#bennysvehiclelist").fadeIn(500);
                $("#bennysdispatches").fadeOut(500);
                $("#bennysmain").fadeOut(500);
                break;	
				
            //justiceapp
            case "justiceapp-overview":
                $("#justice").fadeIn(500);
                $("#justicemain").fadeIn(500);
                $("#justicevehiclelist").fadeOut(500);
                $("#justicelicencecheck").fadeOut(500);
                $("#justicevehiclesearch").fadeOut(500);
                $("#justicesearch").fadeOut(500);
                $("#justicegiveweaponlicense").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;
            case "justiceapp-giveweapon":
                $("#justice").fadeIn(500);
                $("#justicegiveweaponlicense").fadeIn(500);
                $("#justicevehiclelist").fadeOut(500);
                $("#justicelicencecheck").fadeOut(500);
                $("#justicevehiclesearch").fadeOut(500);
                $("#justicesearch").fadeOut(500);
                $("#justicemain").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;
            case "justiceapp-factionvehicles":
                $("#justice").fadeIn(500);
                $("#justicevehiclelist").fadeIn(500);
                $("#justicemain").fadeOut(500);
                $("#justicelicencecheck").fadeOut(500);
                $("#justicevehiclesearch").fadeOut(500);
                $("#justicesearch").fadeOut(500);
                $("#justicegiveweaponlicense").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;			
			case "justiceapp-bankcheck":
				$("#justice").fadeIn(500);
				$("#justiceviewbanktransactions").fadeIn(500);
				$("#justicevehiclelist").fadeOut(500);
				$("#justicemain").fadeOut(500);
				$("#justicelicencecheck").fadeOut(500);
				$("#justicevehiclesearch").fadeOut(500);
				$("#justicesearch").fadeOut(500);
				$("#justicegiveweaponlicense").fadeOut(500);
				break;
            case "justiceapp-licencecheck":
                $("#justice").fadeIn(500);
                $("#justicelicencecheck").fadeIn(500);
                $("#justicevehiclelist").fadeOut(500);
                $("#justicemain").fadeOut(500);
                $("#justicevehiclesearch").fadeOut(500);
                $("#justicesearch").fadeOut(500);
                $("#justicegiveweaponlicense").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;
            case "justiceapp-vehiclecheck":
                $("#justice").fadeIn(500);
                $("#justicevehiclesearch").fadeIn(500);
                $("#justicevehiclelist").fadeOut(500);
                $("#justicelicencecheck").fadeOut(500);
                $("#justicemain").fadeOut(500);
                $("#justicesearch").fadeOut(500);
                $("#justicegiveweaponlicense").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;
            case "justiceapp-search":
                $("#justice").fadeIn(500);
                $("#justicesearch").fadeIn(500);
                $("#justicevehiclelist").fadeOut(500);
                $("#justicelicencecheck").fadeOut(500);
                $("#justicevehiclesearch").fadeOut(500);
                $("#justicemain").fadeOut(500);
                $("#justicegiveweaponlicense").fadeOut(500);
				$("#justiceviewbanktransactions").fadeOut(500);
                break;
				
            //fbiapp
            case "fbiapp-overview":
                $("#fbi").fadeIn(500);
                $("#fbimain").fadeIn(500);
                $("#fbivehiclelist").fadeOut(500);
                $("#fbilicencecheck").fadeOut(500);
                $("#fbivehiclesearch").fadeOut(500);
                $("#fbisearch").fadeOut(500);
                $("#fbidispatches").fadeOut(500);
                break;
            case "fbiapp-viewdispatches":
                $("#fbi").fadeIn(500);
                $("#fbidispatches").fadeIn(500);
                $("#fbivehiclelist").fadeOut(500);
                $("#fbilicencecheck").fadeOut(500);
                $("#fbivehiclesearch").fadeOut(500);
                $("#fbisearch").fadeOut(500);
                $("#fbimain").fadeOut(500);
                break;
            case "fbiapp-factionvehicles":
                $("#fbi").fadeIn(500);
                $("#fbivehiclelist").fadeIn(500);
                $("#fbimain").fadeOut(500);
                $("#fbilicencecheck").fadeOut(500);
                $("#fbivehiclesearch").fadeOut(500);
                $("#fbisearch").fadeOut(500);
                $("#fbidispatches").fadeOut(500);
                break;
            case "fbiapp-licencecheck":
                $("#fbi").fadeIn(500);
                $("#fbilicencecheck").fadeIn(500);
                $("#fbivehiclelist").fadeOut(500);
                $("#fbimain").fadeOut(500);
                $("#fbivehiclesearch").fadeOut(500);
                $("#fbisearch").fadeOut(500);
                $("#fbidispatches").fadeOut(500);
                break;
            case "fbiapp-vehiclecheck":
                $("#fbi").fadeIn(500);
                $("#fbivehiclesearch").fadeIn(500);
                $("#fbivehiclelist").fadeOut(500);
                $("#fbilicencecheck").fadeOut(500);
                $("#fbimain").fadeOut(500);
                $("#fbisearch").fadeOut(500);
                $("#fbidispatches").fadeOut(500);
                break;
            case "fbiapp-search":
                $("#fbi").fadeIn(500);
                $("#fbisearch").fadeIn(500);
                $("#fbivehiclelist").fadeOut(500);
                $("#fbilicencecheck").fadeOut(500);
                $("#fbivehiclesearch").fadeOut(500);
                $("#fbimain").fadeOut(500);
                $("#fbidispatches").fadeOut(500);
                break;
				
				
            //aclsapp
            case "aclsapp-overview":
                $("#acls").fadeIn(500);
                $("#aclsmain").fadeIn(500);
                $("#aclsdispatches").fadeOut(500);
                $("#aclsvehiclelist").fadeOut(500);
                break;
            case "aclsapp-viewdispatches":
                $("#acls").fadeIn(500);
                $("#aclsvehiclelist").fadeIn(500);
                $("#aclsdispatches").fadeOut(500);
                $("#aclsmain").fadeOut(500);
                break;
            case "aclsapp-factionvehicles":
                $("#acls").fadeIn(500);
                $("#aclsvehiclelist").fadeIn(500);
                $("#aclsdispatches").fadeOut(500);
                $("#aclsmain").fadeOut(500);
                break;	
				
            //lsfdapp
            case "lsfdapp-overview":
                $("#lsfd").fadeIn(500);
                $("#lsfdmain").fadeIn(500);
                $("#lsfddispatches").fadeOut(500);
                $("#lsfdvehiclelist").fadeOut(500);
                break;
            case "lsfdapp-viewdispatches":
                $("#lsfd").fadeIn(500);
                $("#lsfdvehiclelist").fadeIn(500);
                $("#lsfddispatches").fadeOut(500);
                $("#lsfdmain").fadeOut(500);
                break;
            case "lsfdapp-factionvehicles":
                $("#lsfd").fadeIn(500);
                $("#lsfdvehiclelist").fadeIn(500);
                $("#lsfddispatches").fadeOut(500);
                $("#lsfdmain").fadeOut(500);
                break;	
			
			//financeapp
			case "financeapp-overview":
                $("#finance").fadeIn(500);
                $("#financemain").fadeIn(500);
                $("#financehistory").fadeOut(500);
                $("#financetransaction").fadeOut(500);
				break;
			case "financeapp-newtransaction":
                $("#finance").fadeIn(500);
                $("#financetransaction").fadeIn(500);
                $("#financemain").fadeOut(500);
                $("#financehistory").fadeOut(500);
				break;
			case "financeapp-history":
                $("#finance").fadeIn(500);
                $("#financehistory").fadeIn(500);
                $("#financemain").fadeOut(500);
                $("#financetransaction").fadeOut(500);
				break;
			
            //lspdapp
            case "lspdapp-overview":
                $("#lspd").fadeIn(500);
                $("#lspdmain").fadeIn(500);
                $("#lspdvehiclelist").fadeOut(500);
                $("#lspdlicencecheck").fadeOut(500);
                $("#lspdvehiclesearch").fadeOut(500);
                $("#lspdsearch").fadeOut(500);
                $("#lspddispatches").fadeOut(500);
                break;
            case "lspdapp-viewdispatches":
                $("#lspd").fadeIn(500);
                $("#lspddispatches").fadeIn(500);
                $("#lspdvehiclelist").fadeOut(500);
                $("#lspdlicencecheck").fadeOut(500);
                $("#lspdvehiclesearch").fadeOut(500);
                $("#lspdsearch").fadeOut(500);
                $("#lspdmain").fadeOut(500);
                break;
            case "lspdapp-factionvehicles":
                $("#lspd").fadeIn(500);
                $("#lspdvehiclelist").fadeIn(500);
                $("#lspdmain").fadeOut(500);
                $("#lspdlicencecheck").fadeOut(500);
                $("#lspdvehiclesearch").fadeOut(500);
                $("#lspdsearch").fadeOut(500);
                $("#lspddispatches").fadeOut(500);
                break;
            case "lspdapp-licencecheck":
                $("#lspd").fadeIn(500);
                $("#lspdlicencecheck").fadeIn(500);
                $("#lspdvehiclelist").fadeOut(500);
                $("#lspdmain").fadeOut(500);
                $("#lspdvehiclesearch").fadeOut(500);
                $("#lspdsearch").fadeOut(500);
                $("#lspddispatches").fadeOut(500);
                break;
            case "lspdapp-vehiclecheck":
                $("#lspd").fadeIn(500);
                $("#lspdvehiclesearch").fadeIn(500);
                $("#lspdvehiclelist").fadeOut(500);
                $("#lspdlicencecheck").fadeOut(500);
                $("#lspdmain").fadeOut(500);
                $("#lspdsearch").fadeOut(500);
                $("#lspddispatches").fadeOut(500);
                break;
            case "lspdapp-search":
                $("#lspd").fadeIn(500);
                $("#lspdsearch").fadeIn(500);
                $("#lspdvehiclelist").fadeOut(500);
                $("#lspdlicencecheck").fadeOut(500);
                $("#lspdvehiclesearch").fadeOut(500);
                $("#lspdmain").fadeOut(500);
                $("#lspddispatches").fadeOut(500);
                break;
            //owned vehicles
            case "myvehicleapp-overview":
                $("#vehiclescreen").fadeIn(500);
                break;
            //EventsApp
            case "eventsapp-overview":
                $("#eventscreen").fadeIn(500);
                $("#eventMain").fadeIn(500);
                $("#createEvent").fadeOut(100);
                break;
            case "eventsapp-newevent":
                $("#createEvent").fadeIn(500);
                $("#eventMain").fadeOut(100);
                break;            
            //NewsApp
            case "lifeinvader-overview":
                $("#lifeinvaderscreen").fadeIn(500);
                break;
			//NewsApp
			case "newsapp-overview":
				$("#newsscreen").fadeIn(500);
				break;
			//NewsApp
			case "vehicleshopapp-overview":
				$("#vehicleshopscreen").fadeIn(500);
				break;
        }
    }
//#endregion
//#region old hud events


// show homescreen done - xore 06.01.2022
// app divs done - xore 06.01.2022
var curAppOpened = "none",
selectedVehStoreColor = "wei??",
selectedNoteColor = "schwarz",
globalSelectedFactionManagerFactionid = 0;
globalSelectedGangManagerGangid = 0;
dispatchAudio = null;

function VehStoreBuyVehicleOnline(hash, shopId) {
	if (hash == undefined || hash == 0 || shopId <= 0) return;
	showArea("homescreen");
	alt.emit('Client:Tablet:VehicleStoreBuyVehicle', `${hash}`, shopId, selectedVehStoreColor);
}

//Banking-App Funktionen

function bankingButtonClicked(){//Neue Transaktion: Button (final)
	var transactiontext = $("#bankingapp-transactiontext").val().replace(/^\s+|\s+$/g, "");
	var targetBankNumber = $("#bankingapp-banknumber").val();
	var moneyAmount = $("#bankingapp-moneyamount").val();
	
	if (targetBankNumber == "" || isNaN(targetBankNumber)) {
		showError("BankingApp-Area-NewTransactionArea", "Du hast keine g??ltige Kontonummer angegeben.");
		return;
	}
	
	if (transactiontext.length <= 0) {
		showError("BankingApp-Area-NewTransactionArea", "Du hast keinen Verwendungszweck angegeben.");
		return;
	}
	
	if (moneyAmount == "" || isNaN(moneyAmount)) {
		showError("BankingApp-Area-NewTransactionArea", "Du hast keinen g??ltigen Betrag angegeben.");
		return;
	}
	
	showError("BankingApp-Area-NewTransactionArea", "none");
	openSubWindow("financeapp-overview");
	$("#bankingapp-banknumber").val("");
	$("#bankingapp-moneyamount").val("");
	$("#bankingapp-transactiontext").val("");
	alt.emit('Client:Tablet:BankingAppnewTransaction', targetBankNumber, transactiontext, moneyAmount);

}

//  done 09.01.2022 xore
function SetBankingAppContent(bankArray, historieArray) {
var bankingHistorieHTML = `<div class="apps-card">`;
bankArray = JSON.parse(bankArray);
historieArray = JSON.parse(historieArray);
var bankNumber = bankArray[0].banknumber.toString();
bankNumber = bankNumber.split('').join(' ');

$("#cardholder").html(`${bankArray[0].charname}`);
$(".currentMoney").html(`$${bankArray[0].bankmoney}`);
$(".card-item__number").html(bankNumber);


for (var i in historieArray) {


	bankingHistorieHTML += `<div class="app-card">`

    if (historieArray[i].type == "Einzahlung" || historieArray[i].type == "Eingehende ??berweisung") {
		bankingHistorieHTML += `<span><font color="green">` + historieArray[i].type + `</font></span>`
    } else {
		bankingHistorieHTML += `<span><font color="red">` + historieArray[i].type + `</font></span>`
    }

    if (historieArray[i].type == "Einzahlung" || historieArray[i].type == "Auszahlung") {
		bankingHistorieHTML += `<div class="app-card__subtext">Standort: ${historieArray[i].location} Betrag: ${historieArray[i].moneyamount}</div>`
    } else if (historieArray[i].type == "Eingehende ??berweisung" || historieArray[i].type == "Ausgehende ??berweisung") {
        if (historieArray[i].type == "Eingehende ??berweisung") {
			bankingHistorieHTML += `<div class="app-card__subtext">Kontonummer (von):  ${historieArray[i].banknumber}`
        } else {
			bankingHistorieHTML += `<div class="app-card__subtext">Kontonummer (an):  ${historieArray[i].banknumber}`
        }
        bankingHistorieHTML += `</br>Verwendungszweck: ${historieArray[i].text}</br>Betrag:  ${historieArray[i].moneyamount}`;
		bankingHistorieHTML += `</div>`
    }

	bankingHistorieHTML += `</div>`
}
bankingHistorieHTML += `</div>`

$("#bankapp-bankhistory-list").html(bankingHistorieHTML);
}

//Events-App Funktionen
$("#createEventbtn").click(function() {
var titleValue = $("#eventsapp-title").val().replace(/^\s+|\s+$/g, "");
var callNumberValue = $("#eventsapp-callnumber").val();
var locationValue = $("#eventsapp-location").val().replace(/^\s+|\s+$/g, "");
var eventTypeValue = $("#eventsapp-eventtype").val().replace(/^\s+|\s+$/g, "");
var informationValue = $("#eventsapp-info").val().replace(/^\s+|\s+$/g, "");
var eventDate = $("#eventsapp-date").val();
var isValidTime = /^([01]?[0-9]|2[0-3]):[0-5][0-9]$/.test($("#eventsapp-clock").val());

if (titleValue.length <= 0) {
    showError("EventsApp-Area-CreateEvent", "Du hast keinen Titel angegeben.");
    return;
}

if (isNaN(callNumberValue)) {
    showError("EventsApp-Area-CreateEvent", "Die Telefonnummer darf nur aus Zahlen bestehen.");
    return;
}

if (eventDate.length < 10 || !isValidDate(eventDate)) {
    showError("EventsApp-Area-CreateEvent", "Das angegebene Datum ist ung??ltig (Format: dd.mm.yyy)");
    return;
}

if (isValidTime == false) {
    showError("EventsApp-Area-CreateEvent", "Die angegebene Uhrzeit ist ung??ltig (Format: hh:mm)");
    return;
}

if (locationValue.length <= 0) {
    showError("EventsApp-Area-CreateEvent", "Du hast keinen Standort angegeben.");
    return;
}

if (eventTypeValue.length <= 0) {
    showError("EventsApp-Area-CreateEvent", "Du hast keine Art der Veranstaltung angegeben.");
    return;
}

if (informationValue.length <= 0) {
    showError("EventsApp-Area-CreateEvent", "Du hast keine weiteren Informationen angegeben.");
    return;
}

var newEventArray = [];

newEventArray.push({
    "title": titleValue,
    "callnumber": callNumberValue,
    "date": eventDate,
    "time": $("#eventsapp-clock").val(),
    "location": locationValue,
    "type": eventTypeValue,
    "moreinfo": informationValue
});


showError("EventsApp-Area-CreateEvent", "none");
openSubWindow("eventsapp-overview");
alt.emit('Client:Tablet:EventsAppNewEntry', titleValue, callNumberValue, eventDate, $("#eventsapp-clock").val(), locationValue, eventTypeValue, informationValue);
$("#eventsapp-title").val("");
$("#eventsapp-callnumber").val("");
$("#eventsapp-location").val("");
$("#eventsapp-eventtype").val("");
$("#eventsapp-info").val("");
$("#eventsapp-date").val("");
$("#eventsapp-clock").val("");
});

function SetEventsAppEventEntrys(eventsArray) {
var eventEntryHTML = `<div class="apps-card">`;
eventsArray = JSON.parse(eventsArray);
for (var i in eventsArray) {
	eventEntryHTML += `<div class="app-card"><span>${eventsArray[i].title}</span>` + 
					  `<div class="app-card__subtext"> Event am: ${eventsArray[i].date} ${eventsArray[i].time} Uhr </br>Veranstalter: ${eventsArray[i].owner} </br>Telefonnr.: ${eventsArray[i].callnumber} </br>Standort: ${eventsArray[i].location} </br>Art der Veranstaltung: ${eventsArray[i].eventtype} </br>Weitere Informationen: ${eventsArray[i].info}</div>` +
					  `</div>`;
}

eventEntryHTML += `</div>`;

$("#event-entry-list").html(eventEntryHTML);
}

//VehiclesApp Funktionen
function VehiclesAppLocateVehicle(posX, posY) {
if (posX != null && posY != null) {
    alt.emit('Client:Tablet:LocateVehicle', posX, posY);
}
}

function SetVehiclesAppListEntrys(vehicleArray) {
var VehiclesAppEntryHTML = `<div class="apps-card">`;
vehicleArray = JSON.parse(vehicleArray);
for (var i in vehicleArray) {
	VehiclesAppEntryHTML += `<div class="app-card"><span>Model: ${vehicleArray[i].name}</span>`;

    if (vehicleArray[i].parkstate == true) {
        //Eingeparkt
		VehiclesAppEntryHTML += `<div class="app-card__subtext">Kennzeichen: ${vehicleArray[i].plate} </br>Garage ${vehicleArray[i].lastgarage} </br>Status In der Garage</div>`;
    } else if (vehicleArray[i].parkstate == false) {
        //Ausgeparkt
		VehiclesAppEntryHTML += `<div class="app-card__subtext">Kennzeichen: ${vehicleArray[i].plate} </br>Garage ${vehicleArray[i].lastgarage} </br>Status Unterwegs</div>`;
    }
	VehiclesAppEntryHTML += `<div class="app-card-buttons"><button class='content-button' onclick='VehiclesAppLocateVehicle(${vehicleArray[i].posX}, ${vehicleArray[i].posY});'><i class='fas fa-search'></i></button></div>`;
	VehiclesAppEntryHTML += `</div>`;
}

	VehiclesAppEntryHTML += `</div>`;
	$("#VehiclesApp-VehicleList").html(VehiclesAppEntryHTML);

}

function SetShopsListEntrys(shopArray) {
var ShopsEntryHTML = "";
shopArray = JSON.parse(shopArray);
for (var i in shopArray) {
    ShopsEntryHTML += `<tr><td>${shopArray[i].id}</td><td>${shopArray[i].name}</td><td>${shopArray[i].bank}$</td><td>${shopArray[i].owner}</td>`;
}

if (ShopsEntryHTML != "") {
    $("#fbiApp-ShopsList").html(ShopsEntryHTML);
}
}

$("#factionmanagerAppInviteNewMember").click(function() { //Mitarbeiter einstellen (Fraktion)
	var charName = $("#factionmanagerapp-name").val().replace(/^\s+|\s+$/g, "");
	var dienstnummer = $("#factionmanagerapp-dienstnummer").val();

	if (charName.length <= 0 || globalSelectedFactionManagerFactionid == 0 || charName == "") {
		return;
	}

	if (isNaN(dienstnummer) || dienstnummer == "") {
		return;
	}

	openSubWindow("factionmanagementapp-overview");
	$("#factionmanagerapp-name").val("");
	$("#factionmanagerapp-dienstnummer").val("");
	alt.emit('Client:Tablet:FactionManagerAppInviteNewMember', charName, dienstnummer, globalSelectedFactionManagerFactionid);
	globalSelectedFactionManagerFactionid = 0;
});

	$("#gangmanagerAppInviteNewMember").click(function() { //Mitarbeiter einstellen (Fraktion)
	var charName = $("#gangmanagerapp-name").val().replace(/^\s+|\s+$/g, "");

	if (charName.length <= 0 || globalSelectedGangManagerGangid == 0 || charName == "") {
		return;
	}

	openSubWindow("gangmanagementapp-overview");
	$("#gangmanagerapp-name").val("");
	alt.emit('Client:Tablet:GangManagerAppInviteNewMember', charName, globalSelectedGangManagerGangid);
	globalSelectedGangManagerGangid = 0;
});

function FactionManagerRankAction(action, charid) {
	if (action != "rankup" && action != "rankdown" && action != "remove") return;
	if (charid <= 0 || charid == undefined) return;
	alt.emit("Client:Tablet:FactionManagerRankAction", action, charid);
}

function GangManagerRankAction(action, charid) {
	if (action != "rankup" && action != "rankdown" && action != "remove") return;
	if (charid <= 0 || charid == undefined) return;
	alt.emit("Client:Tablet:GangManagerRankAction", action, charid);
}

function FactionManagerSetRankPaycheck(rankId, htmlElem) {
	if (rankId <= 0 || htmlElem == null || htmlElem == undefined) return;
	var inputElem = $(htmlElem).parent().find("input");
	var inputVal = $(inputElem).val();
	if (inputVal == "" || inputVal == "" || inputVal < 1) return;
	alt.emit("Client:Tablet:FactionManagerSetRankPaycheck", rankId, inputVal);
}

function SetFactionManagerAppContent(factionId, infoArray, memberArray, rankArray) {
	var factionManagerMemberHTML = `<div class="apps-card">`,
		factionManagerRankHTML = `<div class="apps-card">`,
		infoArray = JSON.parse(infoArray),
		memberArray = JSON.parse(memberArray),
		rankArray = JSON.parse(rankArray);
	if (factionId == 0) return;
	globalSelectedFactionManagerFactionid = factionId;

	for (var i in infoArray) {
		$("#factionmanagerapp-overview-header").html(`${infoArray[i].factionName} (${factionId})`);
		$("#factionmanagerapp-overview-text").html(`<b>Fraktionsleitung: </b>${infoArray[i].factionOwner}<br><b>Kontostand: </b>${infoArray[i].factionBalance}$<br><b>Mitarbeiter: </b>${infoArray[i].factionMemberCount}<br>`);
	}

	for (var i in memberArray) {

		factionManagerMemberHTML += `<div class="app-card"><span>${memberArray[i].charName}</span>` + 
			`<div class="app-card__subtext">Tel. ${memberArray[i].phone} </br>Rang ${memberArray[i].rank} </br>Dienst Nummer ${memberArray[i].serviceNumber}` + 
			`<div class="app-card-buttons"><button type="button" onclick="GangManagerRankAction("rankup", ` + memberArray[i].charId + `);" class="content-button"><i class="far fa-arrow-alt-circle-up"></i></button></div>` + 
			`<button type="button" onclick="GangManagerRankAction("rankdown", ` + memberArray[i].charId + `);" class="content-button"><i class="far fa-arrow-alt-circle-down"></i></button>` + 
			`<button type="button" onclick="GangManagerRankAction("remove", ` + memberArray[i].charId + `);" class="content-button"><i class="fas fa-times"></i></button>` + 
			`</div></div>`;
	}

	for (var i in rankArray) {
		factionManagerRankHTML += `<div class="app-card"><span>>Rank-ID ${rankArray[i].rankId}</span>` + 
								`<div class="app-card__subtext">Rangname ${rankArray[i].rankName}` + 
								`</br>Gehalt</b>${rankArray[i].rankCurPaycheck}$</br>Neues Gehalt</br><input type='number' placeholder='Neues Gehalt..' spellcheck='false' class='form__input' onkeypress='return event.charCode >= 48 && event.charCode <= 57'></div>` + 
								`<div class="app-card-buttons"><button onclick='FactionManagerSetRankPaycheck(" + rankArray[i].rankId + ", this);' type='btn' style='margin-top: 0; padding: .08rem .5rem;' class='content-button'>Setzen</button>` + 
								`</div></div>`;
	}

	factionManagerMemberHTML += `</div>`;
	factionManagerRankHTML += `</div>`;
	$("#FactionManagerApp-Area-ViewAllMembers-List").html(factionManagerMemberHTML);
	$("#FactionManagerApp-Area-RankManage-List").html(factionManagerRankHTML);
}

function SetGangManagerAppContent(gangId, infoArray, memberArray, rankArray) {
	var gangManagerMemberHTML = `<div class="apps-card">`,
		gangManagerRankHTML = `<div class="apps-card">`,
		infoArray = JSON.parse(infoArray),
		memberArray = JSON.parse(memberArray),
		rankArray = JSON.parse(rankArray);
	if (gangId == 0) return;
	globalSelectedGangManagerGangid = gangId;

	for (var i in infoArray) {
		$("#gangmanagerapp-overview-header").html(`${infoArray[i].gangName} (${gangId})`);
		$("#gangmanagerapp-overview-text").html(`<b>Gruppierungsleitung: </b>${infoArray[i].gangOwner}<br><b>Member: </b>${infoArray[i].gangMemberCount}<br>`);
	}

	for (var i in memberArray) {
		gangManagerMemberHTML += `<div class="app-card"><span>${memberArray[i].charName}</span>` + 
								 `<div class="app-card__subtext">Rang ${memberArray[i].rank}` + 
								 `<div class="app-card-buttons"><button type="button" onclick="GangManagerRankAction("rankup", ` + memberArray[i].charId + `);" class="content-button"><i class="far fa-arrow-alt-circle-up"></i></button></div>` + 
								 `<button type="button" onclick="GangManagerRankAction("rankdown", ` + memberArray[i].charId + `);" class="content-button"><i class="far fa-arrow-alt-circle-down"></i></button>` + 
								 `<button type="button" onclick="GangManagerRankAction("remove", ` + memberArray[i].charId + `);" class="content-button"><i class="fas fa-times"></i></button>` + 
								 `</div></div>`;
	}

	for (var i in rankArray) {
		gangManagerRankHTML += `<div class="app-card"><span>>Rank-ID ${rankArray[i].rankId}</span>` + 
								`<div class="app-card__subtext">Rangname ${rankArray[i].rankName}</div></div>`;
	}

		gangManagerMemberHTML += `</div>`;
		gangManagerRankHTML += `</div>`;
	$("#GangManagerApp-Area-ViewAllMembers-List").html(gangManagerMemberHTML);
	$("#GangManagerApp-Area-RankManage-List").html(gangManagerRankHTML);
}


// done div names 06.01.2022 - xore
function SetFactionAppContent(dutyMemberCount, dispatchCount) {
	$("#lspdApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#lsfdApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#lspdaclsApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#fbiApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#bennysApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#taxiApp-Area-Overview-DispatchCount").html(`${dispatchCount}`);
	$("#lspdApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#justiceApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#lsfdApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#lspdaclsApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#fbiApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#bennysApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
	$("#taxiApp-Area-Overview-DutyMemberCount").html(`${dutyMemberCount}`);
}

// done div names 06.01.2022 - xore
function SetLSPDAppPersonSearchData(charName, gender, birthdate, birthplace, address, mainBankAcc, firstJoinDate) {
	$("#lspdApp-Area-SearchPerson-charName").html(`${charName}`);
	$("#fbiApp-Area-SearchPerson-charName").html(`${charName}`);
	$("#justiceApp-Area-SearchPerson-charName").html(`${charName}`);
	$("#lspdApp-Area-SearchPerson-gender").html(`${gender}`);
	$("#fbiApp-Area-SearchPerson-gender").html(`${gender}`);
	$("#justiceApp-Area-SearchPerson-gender").html(`${gender}`);
	$("#lspdApp-Area-SearchPerson-birthdate").html(`${birthdate}`);
	$("#fbiApp-Area-SearchPerson-birthdate").html(`${birthdate}`);
	$("#justiceApp-Area-SearchPerson-birthdate").html(`${birthdate}`);
	$("#lspdApp-Area-SearchPerson-birthplace").html(`${birthplace}`);
	$("#fbiApp-Area-SearchPerson-birthplace").html(`${birthplace}`);
	$("#justiceApp-Area-SearchPerson-birthplace").html(`${birthplace}`);
	$("#lspdApp-Area-SearchPerson-address").html(`${address}`);
	$("#fbiApp-Area-SearchPerson-address").html(`${address}`);
	$("#justiceApp-Area-SearchPerson-address").html(`${address}`);
	$("#lspdApp-Area-SearchPerson-mainBankAccNr").html(`${mainBankAcc}`);
	$("#fbiApp-Area-SearchPerson-mainBankAccNr").html(`${mainBankAcc}`);
	$("#justiceApp-Area-SearchPerson-mainBankAccNr").html(`${mainBankAcc}`);
	$("#lspdApp-Area-SearchPerson-firstJoinDate").html(`${firstJoinDate}`);
	$("#fbiApp-Area-SearchPerson-firstJoinDate").html(`${firstJoinDate}`);
	$("#justiceApp-Area-SearchPerson-firstJoinDate").html(`${firstJoinDate}`);
}

// done div names 06.01.2022 - xore
function LSPDAppSearchPerson() {
	var charName = $("#lspdApp-Area-SearchPerson-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchPerson", charName);
}

// done div names 06.01.2022 - xore
function FBIAppSearchPerson() {
	var charName = $("#fbiApp-Area-SearchPerson-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchPerson", charName);
}

// done div names 06.01.2022 - xore
function JusticeAppSearchPerson() {
	var charName = $("#justiceApp-Area-SearchPerson-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchPerson", charName);
}

// done div names 06.01.2022 - xore
function SetLSPDAppSearchVehiclePlateData(owner, name, manufactor, buydate, trunk, maxfuel, fueltype) {
	$("#lspdApp-Area-SearchVehiclePlate-charName").html(`${owner}`);
	$("#fbiApp-Area-SearchVehiclePlate-charName").html(`${owner}`);
	$("#justiceApp-Area-SearchVehiclePlate-charName").html(`${owner}`);
	$("#lspdApp-Area-SearchVehiclePlate-vehName").html(`${name}`);
	$("#fbiApp-Area-SearchVehiclePlate-vehName").html(`${name}`);
	$("#justiceApp-Area-SearchVehiclePlate-vehName").html(`${name}`);
	$("#lspdApp-Area-SearchVehiclePlate-manufactor").html(`${manufactor}`);
	$("#fbiApp-Area-SearchVehiclePlate-manufactor").html(`${manufactor}`);
	$("#justiceApp-Area-SearchVehiclePlate-manufactor").html(`${manufactor}`);
	$("#lspdApp-Area-SearchVehiclePlate-buydate").html(`${buydate}`);
	$("#fbiApp-Area-SearchVehiclePlate-buydate").html(`${buydate}`);
	$("#justiceApp-Area-SearchVehiclePlate-buydate").html(`${buydate}`);
	$("#lspdApp-Area-SearchVehiclePlate-trunk").html(`${trunk}`);
	$("#fbiApp-Area-SearchVehiclePlate-trunk").html(`${trunk}`);
	$("#justiceApp-Area-SearchVehiclePlate-trunk").html(`${trunk}`);
	$("#lspdApp-Area-SearchVehiclePlate-fuel").html(`${maxfuel}`);
	$("#fbiApp-Area-SearchVehiclePlate-fuel").html(`${maxfuel}`);
	$("#justiceApp-Area-SearchVehiclePlate-fuel").html(`${maxfuel}`);
	$("#lspdApp-Area-SearchVehiclePlate-fueltype").html(`${fueltype}`);
	$("#fbiApp-Area-SearchVehiclePlate-fueltype").html(`${fueltype}`);
	$("#justiceApp-Area-SearchVehiclePlate-fueltype").html(`${fueltype}`);
}

// done div names 06.01.2022 - xore
function LSPDAppSearchVehiclePlate() {
	var plate = $("#lspdApp-Area-SearchVehiclePlate-InputPlate").val();
	if (plate == "" || plate.length <= 0) return;
	var newPlate = plate.replace(" ", "-");
	alt.emit("Client:Tablet:LSPDAppSearchVehiclePlate", newPlate.toUpperCase());
}

// done div names 06.01.2022 - xore
function FBIAppSearchVehiclePlate() {
	var plate = $("#fbiApp-Area-SearchVehiclePlate-InputPlate").val();
	if (plate == "" || plate.length <= 0) return;
	var newPlate = plate.replace(" ", "-");
	alt.emit("Client:Tablet:LSPDAppSearchVehiclePlate", newPlate.toUpperCase());
}

// done div names 06.01.2022 - xore
function JusticeAppSearchVehiclePlate() {
	var plate = $("#justiceApp-Area-SearchVehiclePlate-InputPlate").val();
	if (plate == "" || plate.length <= 0) return;
	var newPlate = plate.replace(" ", "-");
	alt.emit("Client:Tablet:LSPDAppSearchVehiclePlate", newPlate.toUpperCase());
}


// done div names 06.01.2022 - xore
function SetLSPDAppLicenseSearchData(charName, licArray) {
	var licHTML = `<div class="content-section-title">Person: ${charName}</div>`,
		licArray = JSON.parse(licArray);
	licHTML += `<div class="apps-card">`;
	for (var i in licArray) {
		var hasPkw = "Nicht vorhanden",
			hasLKW = "Nicht vorhanden",
			hasBike = "Nicht vorhanden",
			hasBoat = "Nicht vorhanden",
			hasFly = "Nicht vorhanden",
			hasHelicopter = "Nicht vorhanden",
			hasPassengerTransport = "Nicht vorhanden",
			hasWeaponLicense = "Nicht vorhanden";
		if (licArray[i].PKW) hasPkw = "Vorhanden";
		if (licArray[i].LKW) hasLKW = "Vorhanden";
		if (licArray[i].Bike) hasBike = "Vorhanden";
		if (licArray[i].Boat) hasBoat = "Vorhanden";
		if (licArray[i].Fly) hasFly = "Vorhanden";
		if (licArray[i].Helicopter) hasHelicopter = "Vorhanden";
		if (licArray[i].PassengerTransport) hasPassengerTransport = "Vorhanden";
		if (licArray[i].weaponlicense) hasWeaponLicense = "Vorhanden";

		licHTML += `<div class="app-card"><span>${licArray[i].pkwName}: ${hasPkw}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "pkw");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>${licArray[i].lkwName}: ${hasLKW}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "lkw");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>${licArray[i].bikeName}: ${hasBike}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "bike");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>${licArray[i].boatName}: ${hasBoat}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "boat");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>${licArray[i].heliName}: ${hasHelicopter}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "helicopter");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>${licArray[i].passengertransportName}: ${hasPassengerTransport}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "passengertransport");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
		licHTML += `<div class="app-card"><span>Waffenschein: ${hasWeaponLicense}</span><div class="app-card-buttons"><button type='button' onclick='LSPDAppTakeLicense("` + charName + `", "weaponlicense");' class='content-button' style='margin-left: 10px'><i class='fas fa-times'></i></button></div></div>`;
	}
	licHTML += `</div>`;	

	$("#justiceApp-Area-SearchLicense-List").html(licHTML);
	$("#lspdApp-Area-SearchLicense-List").html(licHTML);
	$("#fbiApp-Area-SearchLicense-List").html(licHTML);
}

// done div names 06.01.2022 - xore
function SetJusticeAppSearchedBankAccounts(bankArray) {
	var accHTML = `<div class="apps-card">`,
		bankArray = JSON.parse(bankArray);

	for (var i in bankArray) {
		accHTML += `<div class="app-card"><span>${bankArray[i].accountNumber} (${bankArray[i].money}$)</span>`;
		accHTML += `<div class="app-card-buttons"><button onclick='JusticeAppViewBankTransactions(${bankArray[i].accountNumber});' type='button' class='content-button'><i class='fas fa-check'></i></button></div>`;
	}
	accHTML += `</div>`;
	$("#justiceApp-Area-ViewBankTransactions-AccountList").html(accHTML);
	}

	// done div names 06.01.2022 - xore
	function SetJusticeAppBankTransactions(paperArray) {
	var paperHTML = `<div class="apps-card">`,
		paperArray = JSON.parse(paperArray);

	for (var i in paperArray) {
		paperHTML += `<div class="app-card"><span>${paperArray[i].type} Von/Zu ${paperArray[i].banknumber}</span>`;
		paperHTML += `<div class="app-card__subtext">Betrag ${paperArray[i].moneyamount} </br>Standort${paperArray[i].location}</br>Text<${paperArray[i].text}</div>`;
	}
	paperHTML += `</div>`;
	$("#justiceApp-Area-ViewBankTransactions-HistoryList").html(paperHTML);
}

// done div names 06.01.2022 - xore
function JusticeAppViewBankTransactions(accNumber) {
	if (accNumber.length <= 0 || accNumber == undefined || accNumber == null) return;
	alt.emit("Client:Tablet:JusticeAppViewBankTransactions", accNumber);
}

// done div names 06.01.2022 - xore
function LSPDAppTakeLicense(charName, lic) {
	if (charName == "" || charName.length <= 0 || lic == "" || lic.length <= 0) return;
	if (lic != "pkw" && lic != "lkw" && lic != "bike" && lic != "boat" && lic != "fly" && lic != "helicopter" && lic != "passengertransport" && lic != "weaponlicense") return;
	alt.emit("Client:Tablet:LSPDAppTakeLicense", charName, lic);
}

// done div names 06.01.2022 - xore
function LSPDAppSearchLicense() {
	var charName = $("#lspdApp-Area-SearchLicense-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchLicense", charName);
}

// done div names 06.01.2022 - xore
function FBIAppSearchLicense() {
	var charName = $("#fbiApp-Area-SearchLicense-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchLicense", charName);
}

// done div names 06.01.2022 - xore
function JusticeAppSearchLicense() {
	var charName = $("#justiceApp-Area-SearchLicense-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:LSPDAppSearchLicense", charName);
}

// done div names 06.01.2022 - xore
function JusticeAppGiveWeaponLicense() {
	var charName = $("#justiceApp-Area-GiveWeaponLicense-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0 || charName == "") return;
	alt.emit("Client:Tablet:JusticeAppGiveWeaponLicense", charName);
	openSubWindow("justiceapp-overview");
}

// done div names 06.01.2022 - xore
function JusticeAppSearchBankAccounts() {
	var charName = $("#justiceApp-Area-ViewBankTransactions-InputName").val().replace(/^\s+|\s+$/g, "");
	if (charName.length <= 0) return;
	alt.emit("Client:Tablet:JusticeAppSearchBankAccounts", charName);
}

// done div names 06.01.2022 - xore
function SetFactionAppViewFactionVehicleData(vehicleArray) {
	var viewFactionHTML = `<div class="apps-card">`,
		vehicleArray = JSON.parse(vehicleArray);

	for (var i in vehicleArray) {
		viewFactionHTML += `<div class="app-card">`;
		viewFactionHTML += `<span><img src='../utils/img/vehicles/${vehicleArray[i].vehHash}.png'>${vehicleArray[i].vehName}</span>`;
		viewFactionHTML += `<div class="app-card__subtext">Kennzeichen${vehicleArray[i].vehPlate}</div>`;
		viewFactionHTML += `<div class="app-card-buttons"><button type='button' onclick='VehiclesAppLocateVehicle(${vehicleArray[i].vehPosX}, ${vehicleArray[i].vehPosY});' class='content-button'><i class='fas fa-search'></i></button></div>`;
		viewFactionHTML += `</div></div>`;
	}
	viewFactionHTML += `</div>`;

	$("#justiceApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#lspdApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#lsfdApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#lspdaclsApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#fbiApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#bennysApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
	$("#taxiApp-Area-ViewFactionVehicles-List").html(viewFactionHTML);
}

	// done div names 06.01.2022 - xore
function SendDispatchToFaction(factionId) {
	if (factionId <= 0 || factionId == undefined) return;
	var msg = $("#DispatchApp-dispatchTextField").val();
	if (!isEmptyOrSpaces(msg)) {
		if (msg.length <= 64) {
			showArea("homescreen");
			$("#DispatchApp-dispatchTextField").html(``);
			alt.emit("Client:Tablet:sendDispatchToFaction", factionId, msg);
		} else {
			//Nachricht darf nur 64 Zeichen lang sein
			alt.emit("Client:HUD:sendNotification", 4, 2500, "Die Nachricht darf nicht l??nger als 64 Zeichen sein.");
		}
	} else {
		//Nachricht darf nicht leer sein
		alt.emit("Client:HUD:sendNotification", 4, 2500, "Die Nachricht darf nicht leer sein");
	}
}

// done div names 06.01.2022 - xore
function setFactionDispatches(factionId, dispatchArray) {
	var dispatchHTML = `<div class="apps-card">`,
		dispatchArray = JSON.parse(dispatchArray);

	for (var i in dispatchArray) {
		if (dispatchArray[i].senderCharId == 0) {
			dispatchHTML += `<div class="app-card">`;
			dispatchHTML += `<span> Absender ${dispatchArray[i].altname} ${dispatchArray[i].Date} - ${dispatchArray[i].Time}</span>`;
			dispatchHTML += `<div class="app-card__subtext">Nachricht${dispatchArray[i].message}</div>`;
			dispatchHTML += `<div class="app-card-buttons">`;
			dispatchHTML += `<button type='button' onclick='VehiclesAppLocateVehicle(${dispatchArray[i].posX}, ${dispatchArray[i].posY});' class='content-button'><i class='fas fa-search'></i></button><button type='button' onclick='DeleteDispatch(${dispatchArray[i].factionId}, ${dispatchArray[i].senderCharId});' class='btn btn-danger'><i class='fas fa-times'></i></button>`;
			dispatchHTML += `</div>`;
			dispatchHTML += `</div>`;
		} else {
			dispatchHTML += `<div class="app-card">`;
			dispatchHTML += `<span> Absender ${dispatchArray[i].senderName} ${dispatchArray[i].Date} - ${dispatchArray[i].Time}</span>`;
			dispatchHTML += `<div class="app-card__subtext">Nachricht${dispatchArray[i].message}</div>`;
			dispatchHTML += `<div class="app-card-buttons">`;
			dispatchHTML += `<button type='button' onclick='VehiclesAppLocateVehicle(${dispatchArray[i].posX}, ${dispatchArray[i].posY});' class='content-button'><i class='fas fa-search'></i></button><button type='button' onclick='DeleteDispatch(${dispatchArray[i].factionId}, ${dispatchArray[i].senderCharId});' class='btn btn-danger'><i class='fas fa-times'></i></button>`;
			dispatchHTML += `</div>`;
			dispatchHTML += `</div>`;
		}
	}
	dispatchHTML += `</div>`;

	if (factionId == 2) {
		$("#lspdApp-Area-ViewDispatches-List").html(dispatchHTML);
	} else if (factionId == 3) {
		$("#lsfdApp-Area-ViewDispatches-List").html(dispatchHTML);
	} else if (factionId == 4) {
		$("#aclsApp-Area-ViewDispatches-List").html(dispatchHTML);
	} else if (factionId == 12) {
		$("#fbiApp-Area-ViewDispatches-List").html(dispatchHTML);
	} else if (factionId == 16) {
		$("#taxiApp-Area-ViewDispatches-List").html(dispatchHTML);
	} else if (factionId == 14) {
		$("#bennysApp-Area-ViewDispatches-List").html(dispatchHTML);
	}
}

// done div names 06.01.2022 - xore
	function DeleteDispatch(factionId, senderId) {
	if (factionId <= 0 || senderId < 0) return;
	alt.emit("Client:Tablet:DeleteFactionDispatch", factionId, senderId);
	}

	function isEmptyOrSpaces(str) {
	return str === null || str.match(/^ *$/) !== null;
	}

	function isValidDate(date) {
	var temp = date.split('.');
	var d = new Date(temp[1] + '/' + temp[0] + '/' + temp[2]);
	return (d && (d.getMonth() + 1) == temp[1] && d.getDate() == Number(temp[0]) && d.getFullYear() == Number(temp[2]));
	}

	function showError(area, errorMSG) {
	switch (area) {
		case "EventsApp-Area-CreateEvent":
			if (errorMSG != "none") {
				$("#EventsApp-ErrorMsg").fadeIn(800);
				$("#EventsApp-CreateEvent-Error").html(errorMSG);
			} else {
				$("#EventsApp-ErrorMsg").fadeOut(800);
				$("#EventsApp-CreateEvent-Error").html("");
			}
			break;
		case "BankingApp-Area-NewTransactionArea":
			if (errorMSG != "none") {
				$("#BankingApp-ErrorMsg").fadeIn(800);
				$("#BankingApp-NewTransactionArea-Error").html(errorMSG);
			} else {
				$("#BankingApp-ErrorMsg").fadeOut(800);
				$("#BankingApp-NewTransactionArea-Error").html("");
			}
			break;
	}
	}

	function SetInternetAppVehicleStoreContent(vehstoreArray) {
		var VehStoreHTML = `<div class="apps-card">`;
		vehstoreArray = JSON.parse(vehstoreArray);

		for (var i in vehstoreArray) {
			VehStoreHTML += `<div class="app-card">`;
			VehStoreHTML += `<span>`;
			VehStoreHTML += `<img src='../utils/img/vehicles/${vehstoreArray[i].hash}.png'>`;
			VehStoreHTML += `${vehstoreArray[i].name} ${vehstoreArray[i].manufactor}`;
			VehStoreHTML += `</span>`;
			VehStoreHTML += `<div class="app-card__subtext">${vehstoreArray[i].fueltype} </br>${vehstoreArray[i].fuellimit} Liter </br>${vehstoreArray[i].storage}kg </br>${vehstoreArray[i].seats} Sitzpl??tze</div>`;
			VehStoreHTML += `<div class="app-card-buttons">`;
			VehStoreHTML += `<button type='button' class='content-button' onclick='VehStoreBuyVehicleOnline(${vehstoreArray[i].hash}, ${vehstoreArray[i].shopId});'>Kaufen (${vehstoreArray[i].price}$)</button>`;
			VehStoreHTML += `</div>`;
			VehStoreHTML += `</div>`;
		}
		VehStoreHTML += `</div>`;
		$("#VehicleStore-List").html(VehStoreHTML);
	}

	function playDispatchAudio(path) {
		dispatchAudio = new Audio(path);
		dispatchAudio.play();
	}

	function stopDispatchAudio() {
		if (dispatchAudio != null) {
			dispatchAudio.pause();
			dispatchAudio = null;
		} else {
			return false;
		}
	}

	var acc = document.getElementsByClassName("accordion");
	var i;
	var open = null;

	for (i = 0; i < acc.length; i++) {
	acc[i].addEventListener("click", function() {
		if (open == this) {
			open.classList.toggle("active");
			open = null;
		} else {
			if (open != null) {
				open.classList.toggle("active");
			}
			this.classList.toggle("active");
			open = this;
		}
	});
	}




	function SetInternetAppAppStoreContent(appArray) {
			var appArray = JSON.parse(appArray);

		for (var i in appArray) {
			if (appArray[i].factionmanager == true) {
				$("#factionmanagementapp").show();
			}

			if (appArray[i].gangmanager == true) {
				$("#gangmangementapp").show();
			}

			if (appArray[i].lspdapp == true) {
				$("#lspdapp").show();
			}

			if (appArray[i].lsfdapp == true) {
				$("#lsfdapp").show();
			}

			if (appArray[i].aclsapp == true) {
				$("#aclsapp").show();
			}

			if (appArray[i].fbiapp == true) {
				$("#fbiapp").show();
			}

			if (appArray[i].bennysapp == true) {
				$("#bennysapp").show();
			}

			if (appArray[i].taxiapp == true) {
				$("#taxiapp").show();
			}

			if (appArray[i].justiceapp == true) {
				$("#justiceapp").show();
			}
		}
	}

//#endregion


//#region altv events
	//Alt:on Events
	alt.on("CEF:Tablet:openCEF", () => {
		//showArea("homescreen");
		$(".TabletBG").hide();
		$(".TabletBG").fadeIn(1000);
	});

	alt.on("CEF:Tablet:showTabletArea", (area) => {
		showArea(area);
	});

	alt.on("CEF:Tablet:SetEventsAppEventEntrys", (eventsArray) => {
		SetEventsAppEventEntrys(eventsArray);
	});

	alt.on("CEF:Tablet:SetBankingAppContent", (bankArray, historieArray) => {
		SetBankingAppContent(bankArray, historieArray);
	});

	alt.on("CEF:Tablet:SetVehiclesAppContent", (vehicleArray) => {
		SetVehiclesAppListEntrys(vehicleArray);
	});

	alt.on("CEF:Tablet:SetShopsContent", (shopArray) => {
		SetShopsListEntrys(shopArray);
	});

	alt.on("CEF:Tablet:SetInternetAppAppStoreContent", (appArray) => {
		SetInternetAppAppStoreContent(appArray);
	});

	alt.on("CEF:Tablet:SetVehicleStoreAppContent", (vehStoreArray) => {
		SetInternetAppVehicleStoreContent(vehStoreArray);
	});


	alt.on("CEF:Tablet:SetFactionManagerAppContent", (factionId, infoArray, memberArray, rankArray) => {
		SetFactionManagerAppContent(factionId, infoArray, memberArray, rankArray);
	});

	alt.on("CEF:Tablet:SetGangManagerAppContent", (gangId, infoArray, memberArray, rankArray) => {
		SetGangManagerAppContent(gangId, infoArray, memberArray, rankArray);
	});

	alt.on("CEF:Tablet:SetFactionAppContent", (dutyMemberCount, dispatchCount, vehicleArray) => {
		SetFactionAppContent(dutyMemberCount, dispatchCount);
		SetFactionAppViewFactionVehicleData(vehicleArray);
	});

	alt.on("CEF:Tablet:SetLSPDAppPersonSearchData", (charName, gender, birthdate, birthplace, address, mainBankAcc, firstJoinDate) => {
		SetLSPDAppPersonSearchData(charName, gender, birthdate, birthplace, address, mainBankAcc, firstJoinDate);
	});

	alt.on("CEF:Tablet:SetLSPDAppSearchVehiclePlateData", (owner, name, manufactor, buydate, trunk, maxfuel, tax, fueltype) => {
		SetLSPDAppSearchVehiclePlateData(owner, name, manufactor, buydate, trunk, maxfuel, tax, fueltype);
	});

	alt.on("CEF:Tablet:SetLSPDAppLicenseSearchData", (charName, licArray) => {
		SetLSPDAppLicenseSearchData(charName, licArray);
	});

	alt.on("CEF:Tablet:SetJusticeAppSearchedBankAccounts", (array) => {
		SetJusticeAppSearchedBankAccounts(array);
	});

	alt.on("CEF:Tablet:SetJusticeAppBankTransactions", (array) => {
		SetJusticeAppBankTransactions(array);
	});

	alt.on("CEF:Tablet:SetTutorialAppContent", (array) => {
		SetTutorialAppContent(array);
	});

	alt.on("CEF:Tablet:SetDispatches", (factionid, dispatchArray) => {
		setFactionDispatches(factionid, dispatchArray);
	});

	alt.on("CEF:Tablet:SetShops", () => {
		setShopsList();
	});

	alt.on("CEF:Tablet:playDispatchSound", (filePath) => {
		playDispatchAudio(filePath);
		setTimeout(() => {
			stopDispatchAudio();
		}, 10000);
	})
//#endregion