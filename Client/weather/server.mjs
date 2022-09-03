import * as alt from 'alt';
import Weather from './weather';

let weatherSync = new Weather("0e153395f31d9af678edb8882b474a44", "Madrid", "ES");
//let weatherSync = new Weather("0e153395f31d9af678edb8882b474a44", "Stockholm", "SE");

alt.on('consoleCommand', (msg) => {
    switch (msg) {
        case "startWeather":
            weatherSync.startSync();
            break;
        case "stopWeather":
            weatherSync.stopSync();
            break;
        case "currentTemp":
            weatherSync.getTemp();
            break;
        case "currentData":
            weatherSync.getCurrentData();
            break;
        default:
            break;
    }
});