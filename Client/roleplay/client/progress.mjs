import * as alt from 'alt-client';
import * as native from 'natives';

/*let progressView;
var progressCount = 0;

alt.onServer("xpert:progress:send", (time, text, cb) => {
    progressCount++;
    if (progressView == null) {
        progressView = new alt.WebView('http://resource/client/cef/progress/index.html');
        progressView.on("ready", function() {
            progressView.emit('xpert:notification:sendToWebview', time, text);
            progressView.emit("start", time, text)

            progressView.on("finish", () => {
                cb();
                progressView.destroy();
            })
        });
    } else {
        progressView.emit('xpert:notification:sendToWebview', time, text);
    }
});*/



let ProgressUI = null

export function drawProgress(time, text, cb) {
    if (ProgressUI) { return; }
    ProgressUI = new alt.WebView("http://resource/client/cef/progress/index.html")
    ProgressUI.on("ready", function() {
        ProgressUI.emit("start", time, text)

        ProgressUI.on("finish", () => {
            cb();
            ProgressUI.destroy();
        })
    })
}