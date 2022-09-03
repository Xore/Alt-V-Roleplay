// AltV Server Daten
var ip = 'altv.nightout-gaming.de';
var port = '80';

// request
var request = require('request');
var url = 'https://api.altv.mp/server/8136421803e1c473a4b120bc79ca4fa5';

// uptime checks
var tcpp = require('tcp-ping');

// Discord.js
const Discord = require('discord.js');
const { info } = require('console');
const client = new Discord.Client();

// client login
client.login('ODcwMjY2NDAzMDUwMTE5MTk4.YQKQvw.JMKCmJn9Yp97ay4mTl4TCJar-94');

//richPresence bitch
function updateRichPresence() {
    tcpp.probe(ip, port, function(err, available) {
        if (available) {
            request({
                url: url,
                json: true
            }, function(error, response, body) {
                if (!error && response.statusCode === 200) {
                    client.user.setActivity('|' + body.info.players + '|  Online🟢');
                } else if (!error && response.statusCode === 204) {
                    client.user.setActivity('Offline🔴');
                }
            })
        }
    })
    setTimeout(updateRichPresence, 10 * 1000);
}

// if client is ready print to console
client.on('ready', () => {
    console.log('Bot ist online!');
    //start the richPresence updating function
    updateRichPresence();
});

client.on('message', info => {
    if (info.channel.id === '862737929588047872') {
        if (info.content.startsWith('.status')) {
            tcpp.probe(ip, port, function(err, available) {
                if (available) {
                    request({
                        url: url,
                        json: true
                    }, function(error, response, body) {
                        if (!error && response.statusCode === 200) {
                            console.log(body.info.maxPlayers);
                            info.channel.send({
                                embed: {
                                    image: "https://abload.de/img/logo_no-bgynjfy.png",
                                    color: 10181046,
                                    title: '**Server Status**',
                                    fields: [{
                                            name: '**Server Name**',
                                            value: body.info.name
                                        },
                                        {
                                            name: '**Spieler Online**',
                                            value: body.info.players + '/' + body.info.maxPlayers
                                        },
                                        {
                                            name: '**IP:PORT**',
                                            value: 'altv.nightout-gaming.de:80'
                                        },
                                        {
                                            name: '**GameMode**',
                                            value: body.info.gameMode
                                        },
                                        {
                                            name: '**Website**',
                                            value: body.info.website
                                        },
                                        {
                                            name: '**CDN**',
                                            value: body.info.useCdn
                                        },
                                        {
                                            name: '**Tags**',
                                            value: body.info.tags
                                        }
                                    ],
                                    footer: {
                                        text: '© NightOut-Gaming'
                                    }
                                }
                            })
                        }
                    })
                } else {
                    console.log('Server is offline!');
                }
            })
        }
    }
})