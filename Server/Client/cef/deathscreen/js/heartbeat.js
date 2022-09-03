var container = document.querySelector('.container');
var pulseMiddle = document.querySelector('.pulseMiddle');
var waveSVG = document.querySelector('.waveSVG');
var numItems = 6;
var beep = document.createElement('audio');
beep.src = 'http://soundbible.com/mp3/Heartbeat-SoundBible.com-1259675634.mp3'

var isDevice = (/android|webos|iphone|ipad|ipod|blackberry/i.test(navigator.userAgent.toLowerCase()));
var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;

TweenMax.set(pulseMiddle, {
  scaleX:-1,
  transformOrigin:'50% 50%'
});

var ease = Linear.easeNone;

var mainPulseTimeline = new TimelineMax({repeat:-1});
mainPulseTimeline.timeScale(1);
for(var i = 0; i < numItems; i++){
  
  //pulse timelines
 	var pulseMiddleClone = pulseMiddle.cloneNode(true);
	waveSVG.appendChild(pulseMiddleClone);  
	TweenMax.set([ pulseMiddleClone], {
    drawSVG:'-1% -1%'
  })  
  
  TweenMax.set([ pulseMiddleClone], {
    alpha:1- (i/numItems),
    filter:(isDevice || isFirefox) ? '' : 'url(#glow)'
    
  });
 
  mainPulseTimeline.add(getPulseTimeline( pulseMiddleClone), i/(numItems*6));  
  
}

waveSVG.removeChild(pulseMiddle);
mainPulseTimeline.addCallback(playBeep, 0.91)


function getPulseTimeline( pulseMiddleClone){
  var pulseTimeline = new TimelineMax();
  
  pulseTimeline.to(pulseMiddleClone, 0.5, {
    drawSVG:'10% 50%',
    ease:ease
  },'-=0.1')
  
  .to(pulseMiddleClone, 0.6, {
    drawSVG:'50% 100%',
    ease:ease
  })
  .to(pulseMiddleClone, 0.3, {
    drawSVG:'100% 100%',
    ease:ease
  });
  
  return pulseTimeline;
}

function playBeep(){
	beep.play();  
}