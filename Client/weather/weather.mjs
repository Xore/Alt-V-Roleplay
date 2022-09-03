import * as alt from 'alt';
import fetch from 'node-fetch';


export default class Weather {
    constructor(apiKey, city, countryCode) {
        alt.log('~g~RealWeatherTimeSync: started');
        this.apiKey = apiKey;
        this.city = city;
        this.url = 'https://api.openweathermap.org/data/2.5/weather?q=' + this.city + ',' + this.countryCode + '&appid=' + this.apiKey;
        this.countryCode = countryCode;
        this.interval = null;
        this.dateInterval = null;
        this.currentWeatherData = {};
        this.currentWeatherType = 0;
        this.currentDate = new Date();
        this.registerEvents();
        this.init();
        this.initWeatherData();
    }

    registerEvents() {
        alt.on('playerConnect', (player) => {
            alt.emitClient(player, 'disableClock');
            player.setWeather(this.currentWeatherType);
            this.setDate(player);
        });
    }

    async initWeatherData() {
        try {
            let res = await fetch(this.url);
            let json = await res.json();
            if (json !== undefined) {
                this.currentWeatherData = json;
                this.currentWeatherType = await this.getWeatherType();
                this.syncNewData();
            } else {
                alt.log("Weather data couldn´t be updated");
            }
        } catch (err) {
            console.log('Fetching weather failed: ' + err);
        }
    }

    /*getWeatherType() {
        //Weather Type Returns thingy
        // | EXTRASUNNY: 0 | CLEAR: 1 | CLOUDS: 2 | SMOG: 3 | FOGGY: 4 | OVERCAST: 5 | RAIN: 6 | THUNDER: 7 | CLEARING: 8 | NEUTRAL: 9 | SNOW: 10 | BLIZZARD: 11 | SNOWLIGHT: 12 | XMAS: 13 | HALLOWEEN: 14 |
        //For Snow, Blizzard, Snowlight and XMAS you need to set 2 Natives Clientside for the Snow to Stay on the Ground. they are marked in "clientWeather.js"
        switch (this.currentWeatherData.weather[0].main) {
            case 'Drizzle':
                return 13;
            case 'Clear':
                return 13;
            case 'Clouds':
                return 13;
            case 'Rain':
                return 13;
            case 'Thunderstorm':
                return 13;
            case 'Thunder':
                return 13;
            case 'Foggy':
                return 13;
            case 'Fog':
                return 13;
            case 'Mist':
                return 13;
            case 'Smoke':
                return 13;
            case 'Smog':
                return 13;
            case 'Overcast':
                return 13;
            case 'Snowing':
                return 13;
            case 'Snow':
                return 13;
            case 'Blizzard':
                return 13;
            default:
                return 13;
        }
    }*/

    getWeatherType() {
        switch (this.currentWeatherData.weather[0].main) {
            case 'Drizzle':
                return 8;
            case 'Clear':
                return 1;
            case 'Clouds':
                return 2;
            case 'Rain':
                return 6;
            case 'Thunderstorm':
                return 7;
            case 'Thunder':
                return 7;
            case 'Foggy':
                return 4;
            case 'Fog':
                return 4;
            case 'Mist':
                return 4;
            case 'Smoke':
                return 4;
            case 'Smog':
                return 3;
            case 'Overcast':
                return 5;
            case 'Snowing':
                return 10;
            case 'Snow':
                return 10;
            case 'Blizzard':
                return 11;
            default:
                return 1;
        }
    }

    init() {
        this.dateInterval = setInterval(() => {
            if (alt.Player.all.length !== 0) {
                this.currentDate = new Date();
                alt.Player.all.forEach((player) => {
                    this.setDate(player);
                });
            }
        }, 3000); //3000ms = 3 seconds -> every 3 seconds the date is synced
        this.interval = setInterval(() => this.initWeatherData(), 900000); //900000ms = 15 minutes
    }

    syncNewData() {
        if (alt.Player.all.length !== 0) {
            this.currentDate = new Date();
            alt.Player.all.forEach((player) => {
                player.setWeather(this.currentWeatherType);
                this.setDate(player);
            });
        }
    }

    setDate(player) {
        //  my name is rockstar, how to handle the fkin 31th day of the month? I DONT KNOW
        if (this.currentDate.getDate() == 31) {
            var curDay = 30;
        } else {
            var curDay = this.currentDate.getDate();
        }
        player.setDateTime(curDay, this.currentDate.getMonth(), this.currentDate.getFullYear(),
            this.currentDate.getHours(), this.currentDate.getMinutes(), this.currentDate.getSeconds());
    }

    tempToCelsius(tempInKelvin) {
        return tempInKelvin - 273.15;
    }

    stopSync() {
        if (this.interval) {
            clearInterval(this.interval);
        }
        if (this.dateInterval) {
            clearInterval(this.dateInterval);
        }
        alt.log('RealWeatherTimeSync: stopped');
    }

    startSync() {
        if (this.interval) {
            clearInterval(this.interval);
        }
        if (this.dateInterval) {
            clearInterval(this.dateInterval);
        }
        this.initWeatherData();
        this.init();
        alt.log('RealWeatherTimeSync: started');
    }

    getTemp() {
        console.log(this.tempToCelsius(this.currentWeatherData.main.temp));
    }

    getCurrentData() {
        console.log(this.currentWeatherData.weather[0]);
        console.log("WeatherType: " + this.currentWeatherType);
    }
}